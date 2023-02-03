using HtmlAgilityPack;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ScrapyZoopla
{
    public partial class MainForm : Form
    {
        private string _SqlConnectionStringPath = "ConnectionString.txt";
        private string _SqlConnectionString = "";
        private SqlConnection? _SqlConn;
        private DataTable _PostCodes;
        private DataTable _Datas;
        private bool _StopTask = false;

        public MainForm()
        {
            InitializeComponent();

            gridPostCodes.AutoGenerateColumns = false;
            gridDatas.AutoGenerateColumns = false;
            btnStartTask.Enabled = false;
            btnStopTask.Visible = false;

            _PostCodes = new DataTable();
            _PostCodes.Columns.Add("Id", typeof(int));
            _PostCodes.Columns.Add("PostCode");
            _PostCodes.Columns.Add("Status");

            _Datas = new DataTable();
            _Datas.Columns.Add("Id", typeof(int));
            _Datas.Columns.Add("PostCode");
            _Datas.Columns.Add("UPRN");
            _Datas.Columns.Add("Status");

            gridPostCodes.DataSource = _PostCodes;
            gridDatas.DataSource = _Datas;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            if (!File.Exists(_SqlConnectionStringPath))
            {
                MessageBox.Show("The connection string file does't exists.");
                Close();
                return;
            }

            _SqlConnectionString = File.ReadAllLines(_SqlConnectionStringPath)[0];
            _SqlConn = new SqlConnection(_SqlConnectionString);
        }

        private void btnLoadPostCodes_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                btnStartTask.Enabled = false;
                _PostCodes.Rows.Clear();
                Log($"Load PostCodes file:{openFileDialog1.FileName}");
                try
                {
                    string[] lines = File.ReadAllLines(openFileDialog1.FileName);
                    int id = 0;
                    foreach (string line in lines)
                    {
                        _PostCodes.Rows.Add(id++, line, "");
                    }
                    Log($"Got {_PostCodes.Rows.Count:N0} PostCodes");
                    btnStartTask.Enabled = true;
                }
                catch (Exception ex)
                {
                    Log($"*** Load PostCode file failure.\n{ex.Message}");
                    //MessageBox.Show($"Load PostCode file failure.\n{ex.Message}");
                    return;
                }
            }
        }

        private async void btnStartTask_Click(object sender, EventArgs e)
        {
            if (_SqlConn == null) return;

            btnLoadPostCodes.Enabled = false;
            btnStopTask.Enabled = true;
            btnStartTask.Visible = false;
            btnStopTask.Visible = true;
            _StopTask = false;
            try
            {
                Log("Open SQL connection");
                if (_SqlConn.State == ConnectionState.Closed)
                {
                    _SqlConn.Open();
                }

                using (SqlCommand cmd = _SqlConn.CreateCommand())
                {
                    Log("Get ProcessRunID");
                    int v_zest_ProcessRunID = await zest_ProcessRuns_Insert(cmd);

                    try
                    {
                        Log($"ProcessRunID = {v_zest_ProcessRunID}");
                        foreach (DataRow row in _PostCodes.Rows)
                        {
                            row["Status"] = "";
                        }

                        _Datas.Rows.Clear();
                        for (int i = 0; i < _PostCodes.Rows.Count; i++)
                        {
                            DataRow row = _PostCodes.Rows[i];

                            if (_StopTask) break;
                            gridPostCodes.CurrentCell = gridPostCodes.Rows[i].Cells[0];

                            row["Status"] = "Processing";
                            string? postCode = row["PostCode"] as string;
                            if (postCode == null) continue;

                            Log($"===== PostCode [{postCode}] =====");
                            await ScrapyData(cmd, postCode, v_zest_ProcessRunID);
                            row["Status"] = "Finish";
                        }
                        await zest_ProcessRuns_Update(cmd, v_zest_ProcessRunID, _StopTask ? "Abort" : "Success");
                        Log(_StopTask ? "*** Task aborted." : "Task finish.");
                    }
                    catch
                    {
                        await zest_ProcessRuns_Update(cmd, v_zest_ProcessRunID, "Failure");
                        throw;
                    }

                }
            }
            catch (Exception ex)
            {
                Log($"*** Exception:{ex.Message}");
            }
            finally
            {
                btnLoadPostCodes.Enabled = true;
                btnStartTask.Visible = true;
                btnStopTask.Visible = false;
                _SqlConn.Close();
            }
        }
        private void btnStopTask_Click(object sender, EventArgs e)
        {
            btnStopTask.Enabled = false;
            _StopTask = true;
        }

        private CookieContainer _cookies = new CookieContainer();

        private async Task ScrapyData(SqlCommand cmd, string argsPostCode, int processRunID)
        {
            var htmlWeb = new HtmlWeb();
            var urlPostCode1 = argsPostCode.Replace(" ", "-").ToLower();
            var urlPostCode2 = argsPostCode.Replace(" ", "%20");
            int page = 1;
            while (!_StopTask)
            {
                List<QueryItem> queryItems = new List<QueryItem>();
                string url = $"https://www.zoopla.co.uk/house-prices/{urlPostCode1}/?q={urlPostCode2}&search_source=house-prices&pn={page}";

                Log($"Read data from url:{url}");

              
                string html=await Clearance.Get(url);
                HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                doc.LoadHtml(html);

                //htmlWeb.UseCookies= true;
                //var doc = await htmlWeb.LoadFromWebAsync(url);
                var item = doc.DocumentNode.SelectNodes("//script[contains(@id, '__NEXT_DATA__')]");

                string jsonStr = item[0].InnerText;
                JObject? root = JsonConvert.DeserializeObject<JObject>(jsonStr);

                if (root == null || root.Count == 0) break;
                JArray? edges = root["props"]?["pageProps"]?["data"]?["propertiesSearch"]?["edges"] as JArray;

                if (edges == null || edges.Count == 0)
                {
                    Log($"Read nothing from page {page}.");
                    break;
                }

                Log($"Read {edges.Count:N0} data from page {page}.");
                foreach (var edge in edges)
                {
                    JObject? node = edge["node"] as JObject;
                    if (node == null) continue;

                    string uprn = node["uprn"]?.ToString() ?? "";

                    if (string.IsNullOrWhiteSpace(uprn)) continue;
                    string? propertyId = node["propertyId"]?.ToString();
                    string[] addressParts = (node["address"]?["fullAddress"]?.ToString() ?? "").Split(',');

                    string address_1 = String.Join(",", addressParts.Take(addressParts.Length - 2)).Trim();
                    string address_2 = addressParts[addressParts.Length - 2].Trim();
                    string postCode = addressParts[addressParts.Length - 1].Trim();

                    int? beds = null, baths = null, recs = null, estLow = null, estHigh = null, lastSoldPrice = null;
                    int val;
                    JObject? attributes = node["attributes"] as JObject;
                    JObject? saleEstimate = node["saleEstimate"] as JObject;
                    JObject? lastSale = node["lastSale"] as JObject;

                    string? lastSoldDateText = lastSale != null && lastSale.ContainsKey("date") ? node["lastSale"]?["date"]?.ToString() : "";
                    DateTime? lastSoldDate = null;
                    if (DateTime.TryParse(lastSoldDateText, out DateTime dateTime))
                    {
                        lastSoldDate = dateTime;
                    }

                    if (attributes != null && attributes.ContainsKey("bathrooms") && Int32.TryParse(attributes["bathrooms"]?.ToString(), out val))
                    {
                        baths = val;
                    }

                    if (attributes != null && attributes.ContainsKey("bedrooms") && Int32.TryParse(attributes["bathrooms"]?.ToString(), out val))
                    {
                        beds = val;
                    }

                    if (attributes != null && attributes.ContainsKey("livingRooms") && Int32.TryParse(attributes["bathrooms"]?.ToString(), out val))
                    {
                        recs = val;
                    }

                    if (saleEstimate != null && saleEstimate.ContainsKey("lowerPrice") && Int32.TryParse(saleEstimate["lowerPrice"]?.ToString(), out val))
                    {
                        estLow = val;
                    }

                    if (saleEstimate != null && saleEstimate.ContainsKey("upperPrice") && Int32.TryParse(saleEstimate["upperPrice"]?.ToString(), out val))
                    {
                        estHigh = val;
                    }

                    if (lastSale != null && lastSale.ContainsKey("price") && Int32.TryParse(lastSale["price"]?.ToString(), out val))
                    {
                        lastSoldPrice = val;
                    }

                    DataRow row = _Datas.Rows.Add(_Datas.Rows.Count + 1, postCode, uprn, "");
                    queryItems.Add(new QueryItem()
                    {
                        URL_Property = $"https://www.zoopla.co.uk/property/uprn/{uprn}/",
                        Uprn = uprn,
                        Address_1 = address_1,
                        Address_2 = address_2,
                        PostCode = postCode,
                        NumBeds = beds,
                        NumBaths = baths,
                        NumRec = recs,
                        EstLow = estLow,
                        EstHigh = estHigh,
                        LastSoldDate = lastSoldDate,
                        LastSoldPrice = lastSoldPrice,
                        Zoopla_PropertyID = propertyId,
                        Zest_ProcessRunID = processRunID,
                        Row = row

                    });
                }

                gridDatas.CurrentCell = gridDatas.Rows[gridDatas.Rows.Count - 1].Cells[0];

                List<Task> tasks = new List<Task>();
                foreach (var queryItem in queryItems)
                {
                    if (_StopTask) break;
                    if (queryItem.Row == null) continue;

                    if (!string.IsNullOrWhiteSpace(queryItem.Uprn))
                    {
                        queryItem.Row["Status"] = "Loading";

                        Log($"----- Collect further data {queryItem.Uprn} -----");
                        Log($"Read data from {queryItem.URL_Property}.");
                        tasks.Add(LoadFurtherData(queryItem));
                    }
                }
                await Task.WhenAll(tasks.ToArray());

                foreach (var queryItem in queryItems)
                {
                    if (_StopTask) break;
                    if (queryItem.Row == null) continue;

                    Log($"Write {queryItem.Uprn} to database.");
                    queryItem.Row["Status"] = "Inserting";
                    cmd.CommandText = "EXEC[zest_Properties_Insert] " +
                                      "@p_URL_Property," +
                                      "@p_Address_1," +
                                      "@p_Address_2," +
                                      "@p_PostCode," +
                                      "@p_NumBeds," +
                                      "@p_NumBaths," +
                                      "@p_NumRec," +
                                      "@p_EstLow," +
                                      "@p_EstHigh," +
                                      "@p_LastSoldDate," +
                                      "@p_LastSoldPrice," +
                                      "@p_UPRN," +
                                      "@p_Zoopla_PropertyID," +
                                      "@p_zest_ProcessRunID," +
                                      "@p_Size," +
                                      "@p_URL_Listing1," +
                                      "@p_URL_Listing2," +
                                      "@p_ListingID1," +
                                      "@p_ListingID2";
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@p_URL_Property", queryItem.URL_Property);
                    cmd.Parameters.AddWithValue("@p_Address_1", queryItem.Address_1);
                    cmd.Parameters.AddWithValue("@p_Address_2", queryItem.Address_2);
                    cmd.Parameters.AddWithValue("@p_PostCode", queryItem.PostCode);
                    cmd.Parameters.AddWithValue("@p_NumBeds", (object?)(queryItem.NumBeds) ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@p_NumBaths", (object?)(queryItem.NumBaths) ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@p_NumRec", (object?)(queryItem.NumRec) ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@p_EstLow", (object?)(queryItem.EstLow) ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@p_EstHigh", (object?)(queryItem.EstHigh) ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@p_LastSoldDate", (object?)(queryItem.LastSoldDate) ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@p_LastSoldPrice", (object?)(queryItem.LastSoldPrice) ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@p_UPRN", (string.IsNullOrWhiteSpace(queryItem.Uprn) ? DBNull.Value : (object)queryItem.Uprn));
                    cmd.Parameters.AddWithValue("@p_Zoopla_PropertyID", queryItem.Zoopla_PropertyID);
                    cmd.Parameters.AddWithValue("@p_zest_ProcessRunID", queryItem.Zest_ProcessRunID);
                    cmd.Parameters.AddWithValue("@p_Size", (object?)(queryItem.Size) ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@p_URL_Listing1", (object?)(queryItem.URL_Listing1) ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@p_URL_Listing2", (object?)(queryItem.URL_Listing2) ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@p_ListingID1", (object?)(queryItem.ListingID1) ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@p_ListingID2", (object?)(queryItem.ListingID2) ?? DBNull.Value);
                    await cmd.ExecuteNonQueryAsync();
                    queryItem.Row["Status"] = "Finish";
                }

                //foreach (var queryItem in queryItems)
                //{
                //    if (_StopTask) break;
                //    if (queryItem.Row == null) continue;

                //    if (!string.IsNullOrWhiteSpace(queryItem.Uprn))
                //    {
                //        queryItem.Row["Status"] = "Loading";

                //        Log($"----- Collect further data {queryItem.Uprn} -----");
                //        Log($"Read data from {queryItem.URL_Property}.");

                //        html = await Clearance.Get(queryItem.URL_Property);
                //        doc.LoadHtml(html);
                //        //doc = await htmlWeb.LoadFromWebAsync(queryItem.URL_Property);
                //        item = doc.DocumentNode.SelectNodes("//script[contains(@id, '__NEXT_DATA__')]");
                //        jsonStr = item[0].InnerText;
                //        root = JsonConvert.DeserializeObject<JObject>(jsonStr);
                //        if (root == null) continue;

                //        int? size = null;
                //        JObject? attributes = root["props"]?["pageProps"]?["data"]?["property"]?["attributes"] as JObject;
                //        if (attributes != null && attributes.ContainsKey("floorAreaSqM") && int.TryParse(attributes["floorAreaSqM"]?.ToString(), out int val))
                //        {
                //            size = val;
                //        }


                //        queryItem.Size = size;
                //        JArray? array = root["props"]?["pageProps"]?["data"]?["property"]?["liveListings"] as JArray;
                //        if (array != null)
                //        {
                //            if (array.Count > 0)
                //            {
                //                queryItem.URL_Listing1 = array[0]["uri"]?.ToString();
                //                queryItem.ListingID1 = array[0]["listingId"]?.ToString();
                //            }
                //            if (array.Count > 1)
                //            {
                //                queryItem.URL_Listing2 = array[1]["uri"]?.ToString();
                //                queryItem.ListingID2 = array[1]["listingId"]?.ToString();
                //            }
                //        }
                //    }

                //    Log($"Write to database.");
                //    queryItem.Row["Status"] = "Inserting";
                //    cmd.CommandText = "EXEC[zest_Properties_Insert] " +
                //                      "@p_URL_Property," +
                //                      "@p_Address_1," +
                //                      "@p_Address_2," +
                //                      "@p_PostCode," +
                //                      "@p_NumBeds," +
                //                      "@p_NumBaths," +
                //                      "@p_NumRec," +
                //                      "@p_EstLow," +
                //                      "@p_EstHigh," +
                //                      "@p_LastSoldDate," +
                //                      "@p_LastSoldPrice," +
                //                      "@p_UPRN," +
                //                      "@p_Zoopla_PropertyID," +
                //                      "@p_zest_ProcessRunID," +
                //                      "@p_Size," +
                //                      "@p_URL_Listing1," +
                //                      "@p_URL_Listing2," +
                //                      "@p_ListingID1," +
                //                      "@p_ListingID2";
                //    cmd.Parameters.Clear();
                //    cmd.Parameters.AddWithValue("@p_URL_Property", queryItem.URL_Property);
                //    cmd.Parameters.AddWithValue("@p_Address_1", queryItem.Address_1);
                //    cmd.Parameters.AddWithValue("@p_Address_2", queryItem.Address_2);
                //    cmd.Parameters.AddWithValue("@p_PostCode", queryItem.PostCode);
                //    cmd.Parameters.AddWithValue("@p_NumBeds", (object?)(queryItem.NumBeds) ?? DBNull.Value);
                //    cmd.Parameters.AddWithValue("@p_NumBaths", (object?)(queryItem.NumBaths) ?? DBNull.Value);
                //    cmd.Parameters.AddWithValue("@p_NumRec", (object?)(queryItem.NumRec) ?? DBNull.Value);
                //    cmd.Parameters.AddWithValue("@p_EstLow", (object?)(queryItem.EstLow) ?? DBNull.Value);
                //    cmd.Parameters.AddWithValue("@p_EstHigh", (object?)(queryItem.EstHigh) ?? DBNull.Value);
                //    cmd.Parameters.AddWithValue("@p_LastSoldDate", (object?)(queryItem.LastSoldDate) ?? DBNull.Value);
                //    cmd.Parameters.AddWithValue("@p_LastSoldPrice", (object?)(queryItem.LastSoldPrice) ?? DBNull.Value);
                //    cmd.Parameters.AddWithValue("@p_UPRN", (string.IsNullOrWhiteSpace(queryItem.Uprn) ? DBNull.Value : (object)queryItem.Uprn));
                //    cmd.Parameters.AddWithValue("@p_Zoopla_PropertyID", queryItem.Zoopla_PropertyID);
                //    cmd.Parameters.AddWithValue("@p_zest_ProcessRunID", queryItem.Zest_ProcessRunID);
                //    cmd.Parameters.AddWithValue("@p_Size", (object?)(queryItem.Size) ?? DBNull.Value);
                //    cmd.Parameters.AddWithValue("@p_URL_Listing1", (object?)(queryItem.URL_Listing1) ?? DBNull.Value);
                //    cmd.Parameters.AddWithValue("@p_URL_Listing2", (object?)(queryItem.URL_Listing2) ?? DBNull.Value);
                //    cmd.Parameters.AddWithValue("@p_ListingID1", (object?)(queryItem.ListingID1) ?? DBNull.Value);
                //    cmd.Parameters.AddWithValue("@p_ListingID2", (object?)(queryItem.ListingID2) ?? DBNull.Value);
                //    await cmd.ExecuteNonQueryAsync();

                //    queryItem.Row["Status"] = "Finish";
                //}
                page++;
            }
        }

        private async Task LoadFurtherData(QueryItem queryItem)
        {
                string html = await Clearance.Get(queryItem.URL_Property);
                HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                doc.LoadHtml(html);
                //doc = await htmlWeb.LoadFromWebAsync(queryItem.URL_Property);
                var item = doc.DocumentNode.SelectNodes("//script[contains(@id, '__NEXT_DATA__')]");
                var jsonStr = item[0].InnerText;
                var root = JsonConvert.DeserializeObject<JObject>(jsonStr);
                if (root == null) return;

                int? size = null;
                JObject? attributes = root["props"]?["pageProps"]?["data"]?["property"]?["attributes"] as JObject;
                if (attributes != null && attributes.ContainsKey("floorAreaSqM") && int.TryParse(attributes["floorAreaSqM"]?.ToString(), out int val))
                {
                    size = val;
                }


                queryItem.Size = size;
                JArray? array = root["props"]?["pageProps"]?["data"]?["property"]?["liveListings"] as JArray;
                if (array != null)
                {
                    if (array.Count > 0)
                    {
                        queryItem.URL_Listing1 = array[0]["uri"]?.ToString();
                        queryItem.ListingID1 = array[0]["listingId"]?.ToString();
                    }
                    if (array.Count > 1)
                    {
                        queryItem.URL_Listing2 = array[1]["uri"]?.ToString();
                        queryItem.ListingID2 = array[1]["listingId"]?.ToString();
                    }
                }
        }

        private async Task<int> zest_ProcessRuns_Insert(SqlCommand cmd)
        {
            cmd.CommandText = "DECLARE @v_Now DATETIME = GETDATE();" +
                              "EXEC[zest_ProcessRuns_Insert] @v_Now, NULL, 1 ,NULL;" +
                              "SET @v_zest_ProcessRunID = @@IDENTITY;";
            SqlParameter processRunID = new SqlParameter("@v_zest_ProcessRunID", SqlDbType.Int) { Direction = ParameterDirection.Output };
            cmd.Parameters.Clear();
            cmd.Parameters.Add(processRunID);
            await cmd.ExecuteNonQueryAsync();
            return Convert.ToInt32(processRunID.Value);
        }


        private async Task zest_ProcessRuns_Update(SqlCommand cmd, int rocessRunID, string status)
        {
            cmd.CommandText = "DECLARE @v_Now DATETIME = GETDATE();" +
                              $"EXEC[zest_ProcessRuns_Update] @v_zest_ProcessRunID, @v_Now, {status};";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@v_zest_ProcessRunID", rocessRunID);
            await cmd.ExecuteNonQueryAsync();
        }

        private void Log(string log)
        {
            txtLogs.AppendText($"{DateTime.Now:HH:mm:ss.fff} - {log}\r\n");
        }
    }


    public class QueryItem
    {
        public string URL_Property { get; set; } = "";
        public string? Uprn { get; set; }
        public string? Zoopla_PropertyID { get; set; }
        public string? Address_1 { get; set; }
        public string? Address_2 { get; set; }
        public string? PostCode { get; set; }
        public DateTime? LastSoldDate { get; set; }
        public int? NumBeds { get; set; }
        public int? NumBaths { get; set; }
        public int? NumRec { get; set; }
        public int? EstLow { get; set; }
        public int? EstHigh { get; set; }
        public int? LastSoldPrice { get; set; }
        public long Zest_ProcessRunID { get; set; }
        public int? Size { get; set; }
        public string? ListingID1 { get; set; }
        public string? ListingID2 { get; set; }
        public string? URL_Listing1 { get; set; }
        public string? URL_Listing2 { get; set; }
        public DataRow? Row { get; set; }
    }

}
