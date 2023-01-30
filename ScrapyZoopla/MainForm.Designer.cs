
namespace ScrapyZoopla
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            this.txtLogs = new System.Windows.Forms.TextBox();
            this.btnStartTask = new System.Windows.Forms.Button();
            this.gridPostCodes = new System.Windows.Forms.DataGridView();
            this.gridPostCodes_PostCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gridPostCodes_Status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnLoadPostCodes = new System.Windows.Forms.Button();
            this.gridDatas = new System.Windows.Forms.DataGridView();
            this.gridDatas_Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gridDatas_PostCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gridDatas_UPRN = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gridDatas_Status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.btnStopTask = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.gridPostCodes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridDatas)).BeginInit();
            this.SuspendLayout();
            // 
            // txtLogs
            // 
            this.txtLogs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtLogs.BackColor = System.Drawing.Color.White;
            this.txtLogs.Location = new System.Drawing.Point(668, 8);
            this.txtLogs.Multiline = true;
            this.txtLogs.Name = "txtLogs";
            this.txtLogs.ReadOnly = true;
            this.txtLogs.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtLogs.Size = new System.Drawing.Size(530, 480);
            this.txtLogs.TabIndex = 0;
            this.txtLogs.WordWrap = false;
            // 
            // btnStartTask
            // 
            this.btnStartTask.Location = new System.Drawing.Point(156, 7);
            this.btnStartTask.Name = "btnStartTask";
            this.btnStartTask.Size = new System.Drawing.Size(75, 23);
            this.btnStartTask.TabIndex = 1;
            this.btnStartTask.Text = "Start Task";
            this.btnStartTask.UseVisualStyleBackColor = true;
            this.btnStartTask.Click += new System.EventHandler(this.btnStartTask_Click);
            // 
            // gridPostCodes
            // 
            this.gridPostCodes.AllowUserToAddRows = false;
            this.gridPostCodes.AllowUserToDeleteRows = false;
            this.gridPostCodes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridPostCodes.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
            this.gridPostCodes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridPostCodes.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.gridPostCodes_PostCode,
            this.gridPostCodes_Status});
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.gridPostCodes.DefaultCellStyle = dataGridViewCellStyle6;
            this.gridPostCodes.Location = new System.Drawing.Point(3, 36);
            this.gridPostCodes.Name = "gridPostCodes";
            this.gridPostCodes.ReadOnly = true;
            this.gridPostCodes.RowHeadersVisible = false;
            this.gridPostCodes.RowTemplate.Height = 25;
            this.gridPostCodes.Size = new System.Drawing.Size(228, 451);
            this.gridPostCodes.TabIndex = 2;
            // 
            // gridPostCodes_PostCode
            // 
            this.gridPostCodes_PostCode.DataPropertyName = "PostCode";
            this.gridPostCodes_PostCode.HeaderText = "PostCode";
            this.gridPostCodes_PostCode.Name = "gridPostCodes_PostCode";
            this.gridPostCodes_PostCode.ReadOnly = true;
            // 
            // gridPostCodes_Status
            // 
            this.gridPostCodes_Status.DataPropertyName = "Status";
            this.gridPostCodes_Status.HeaderText = "Status";
            this.gridPostCodes_Status.Name = "gridPostCodes_Status";
            this.gridPostCodes_Status.ReadOnly = true;
            // 
            // btnLoadPostCodes
            // 
            this.btnLoadPostCodes.Location = new System.Drawing.Point(3, 7);
            this.btnLoadPostCodes.Name = "btnLoadPostCodes";
            this.btnLoadPostCodes.Size = new System.Drawing.Size(123, 23);
            this.btnLoadPostCodes.TabIndex = 3;
            this.btnLoadPostCodes.Text = "LoadPostCodes";
            this.btnLoadPostCodes.UseVisualStyleBackColor = true;
            this.btnLoadPostCodes.Click += new System.EventHandler(this.btnLoadPostCodes_Click);
            // 
            // gridDatas
            // 
            this.gridDatas.AllowUserToAddRows = false;
            this.gridDatas.AllowUserToDeleteRows = false;
            this.gridDatas.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridDatas.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle7;
            this.gridDatas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridDatas.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.gridDatas_Id,
            this.gridDatas_PostCode,
            this.gridDatas_UPRN,
            this.gridDatas_Status});
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.gridDatas.DefaultCellStyle = dataGridViewCellStyle8;
            this.gridDatas.Location = new System.Drawing.Point(237, 7);
            this.gridDatas.Name = "gridDatas";
            this.gridDatas.ReadOnly = true;
            this.gridDatas.RowHeadersVisible = false;
            this.gridDatas.RowTemplate.Height = 25;
            this.gridDatas.Size = new System.Drawing.Size(425, 480);
            this.gridDatas.TabIndex = 4;
            // 
            // gridDatas_Id
            // 
            this.gridDatas_Id.DataPropertyName = "Id";
            this.gridDatas_Id.FillWeight = 60F;
            this.gridDatas_Id.HeaderText = "Id";
            this.gridDatas_Id.Name = "gridDatas_Id";
            this.gridDatas_Id.ReadOnly = true;
            this.gridDatas_Id.Width = 60;
            // 
            // gridDatas_PostCode
            // 
            this.gridDatas_PostCode.DataPropertyName = "PostCode";
            this.gridDatas_PostCode.HeaderText = "PostCode";
            this.gridDatas_PostCode.Name = "gridDatas_PostCode";
            this.gridDatas_PostCode.ReadOnly = true;
            // 
            // gridDatas_UPRN
            // 
            this.gridDatas_UPRN.DataPropertyName = "UPRN";
            this.gridDatas_UPRN.HeaderText = "UPRN";
            this.gridDatas_UPRN.Name = "gridDatas_UPRN";
            this.gridDatas_UPRN.ReadOnly = true;
            this.gridDatas_UPRN.Width = 140;
            // 
            // gridDatas_Status
            // 
            this.gridDatas_Status.DataPropertyName = "Status";
            this.gridDatas_Status.HeaderText = "Status";
            this.gridDatas_Status.Name = "gridDatas_Status";
            this.gridDatas_Status.ReadOnly = true;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Filter = "txt file|*.txt";
            this.openFileDialog1.Title = "open the PostCodes.txt";
            // 
            // btnStopTask
            // 
            this.btnStopTask.BackColor = System.Drawing.Color.MistyRose;
            this.btnStopTask.Location = new System.Drawing.Point(156, 8);
            this.btnStopTask.Name = "btnStopTask";
            this.btnStopTask.Size = new System.Drawing.Size(75, 23);
            this.btnStopTask.TabIndex = 5;
            this.btnStopTask.Text = "Stop Task";
            this.btnStopTask.UseVisualStyleBackColor = false;
            this.btnStopTask.Click += new System.EventHandler(this.btnStopTask_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1201, 493);
            this.Controls.Add(this.btnStopTask);
            this.Controls.Add(this.gridDatas);
            this.Controls.Add(this.btnLoadPostCodes);
            this.Controls.Add(this.gridPostCodes);
            this.Controls.Add(this.btnStartTask);
            this.Controls.Add(this.txtLogs);
            this.Name = "MainForm";
            this.Text = "MainForm";
            this.Load += new System.EventHandler(this.MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gridPostCodes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridDatas)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtLogs;
        private System.Windows.Forms.Button btnStartTask;
        private System.Windows.Forms.DataGridView gridPostCodes;
        private System.Windows.Forms.Button btnLoadPostCodes;
        private System.Windows.Forms.DataGridView gridDatas;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.DataGridViewTextBoxColumn gridPostCodes_PostCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn gridPostCodes_Status;
        private System.Windows.Forms.DataGridViewTextBoxColumn gridDatas_Id;
        private System.Windows.Forms.DataGridViewTextBoxColumn gridDatas_PostCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn gridDatas_UPRN;
        private System.Windows.Forms.DataGridViewTextBoxColumn gridDatas_Status;
        private System.Windows.Forms.Button btnStopTask;
    }
}

