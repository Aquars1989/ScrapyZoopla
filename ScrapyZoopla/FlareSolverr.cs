using FlareSolverrSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ScrapyZoopla
{
    public static class Clearance
    {
        public static string FlareSolverrUrl = "http://localhost:8191/";

        public static async Task<string> Get(string url)
        {
            string msg = "";
            for (int i = 0; i < 10; i++)
            {
                try
                {
                    var handler = new ClearanceHandler(FlareSolverrUrl)
                    {
                        MaxTimeout = 60000
                    };

                    var client = new HttpClient(handler);
                    var content = await client.GetStringAsync(url);
                    return content;
                }
                catch (Exception ex) { msg = ex.Message; }
            }
            throw new Exception(msg);
        }
    }
}