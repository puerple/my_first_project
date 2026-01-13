namespace S7DataExporter
{
    partial class MainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnConnect = new System.Windows.Forms.Button();
            this.btnDisconnect = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtIP = new System.Windows.Forms.TextBox();
            this.nudRack = new System.Windows.Forms.NumericUpDown();
            this.nudSlot = new System.Windows.Forms.NumericUpDown();
            this.gbConnection = new System.Windows.Forms.GroupBox();
            this.gbVariables = new System.Windows.Forms.GroupBox();
            this.groupRecordMode = new System.Windows.Forms.GroupBox();
            this.nudTriggerDelay = new System.Windows.Forms.NumericUpDown();
            this.lblDelayTime = new System.Windows.Forms.Label();
            this.txtTriggerAddress = new System.Windows.Forms.TextBox();
            this.lblTriggerAddress = new System.Windows.Forms.Label();
            this.rbTriggered = new System.Windows.Forms.RadioButton();
            this.rbPeriodic = new System.Windows.Forms.RadioButton();
            this.btnExportTemplate = new System.Windows.Forms.Button();
            this.btnImport = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.btnDelete = new System.Windows.Forms.Button();
            this.gbCollection = new System.Windows.Forms.GroupBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.txtOutputPath = new System.Windows.Forms.TextBox();
            this.rbExcel = new System.Windows.Forms.RadioButton();
            this.rbCsv = new System.Windows.Forms.RadioButton();
            this.nudInterval = new System.Windows.Forms.NumericUpDown();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lblLastUpdate = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.labelS = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRack)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSlot)).BeginInit();
            this.gbConnection.SuspendLayout();
            this.gbVariables.SuspendLayout();
            this.groupRecordMode.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudTriggerDelay)).BeginInit();
            this.gbCollection.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudInterval)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3,
            this.Column4});
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Left;
            this.dataGridView1.Location = new System.Drawing.Point(3, 24);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowHeadersWidth = 62;
            this.dataGridView1.RowTemplate.Height = 30;
            this.dataGridView1.Size = new System.Drawing.Size(804, 515);
            this.dataGridView1.TabIndex = 0;
            // 
            // Column1
            // 
            this.Column1.FillWeight = 100.1669F;
            this.Column1.HeaderText = "√";
            this.Column1.MinimumWidth = 8;
            this.Column1.Name = "Column1";
            // 
            // Column2
            // 
            this.Column2.FillWeight = 99.57325F;
            this.Column2.HeaderText = "变量名";
            this.Column2.MinimumWidth = 8;
            this.Column2.Name = "Column2";
            // 
            // Column3
            // 
            this.Column3.FillWeight = 100.4648F;
            this.Column3.HeaderText = "PLC地址";
            this.Column3.MinimumWidth = 8;
            this.Column3.Name = "Column3";
            // 
            // Column4
            // 
            this.Column4.FillWeight = 99.79501F;
            this.Column4.HeaderText = "数据类型";
            this.Column4.MinimumWidth = 8;
            this.Column4.Name = "Column4";
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(48, 146);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(130, 38);
            this.btnConnect.TabIndex = 1;
            this.btnConnect.Text = "连接";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // btnDisconnect
            // 
            this.btnDisconnect.Location = new System.Drawing.Point(212, 146);
            this.btnDisconnect.Name = "btnDisconnect";
            this.btnDisconnect.Size = new System.Drawing.Size(130, 38);
            this.btnDisconnect.TabIndex = 1;
            this.btnDisconnect.Text = "断开";
            this.btnDisconnect.UseVisualStyleBackColor = true;
            this.btnDisconnect.Click += new System.EventHandler(this.btnDisconnect_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(38, 54);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 18);
            this.label2.TabIndex = 3;
            this.label2.Text = "IP地址：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(38, 98);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 18);
            this.label3.TabIndex = 3;
            this.label3.Text = "机架号：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(201, 98);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(80, 18);
            this.label4.TabIndex = 3;
            this.label4.Text = "插槽号：";
            // 
            // txtIP
            // 
            this.txtIP.Location = new System.Drawing.Point(114, 51);
            this.txtIP.Name = "txtIP";
            this.txtIP.Size = new System.Drawing.Size(222, 28);
            this.txtIP.TabIndex = 4;
            // 
            // nudRack
            // 
            this.nudRack.Location = new System.Drawing.Point(123, 92);
            this.nudRack.Name = "nudRack";
            this.nudRack.Size = new System.Drawing.Size(56, 28);
            this.nudRack.TabIndex = 5;
            // 
            // nudSlot
            // 
            this.nudSlot.Location = new System.Drawing.Point(279, 92);
            this.nudSlot.Name = "nudSlot";
            this.nudSlot.Size = new System.Drawing.Size(56, 28);
            this.nudSlot.TabIndex = 5;
            // 
            // gbConnection
            // 
            this.gbConnection.AutoSize = true;
            this.gbConnection.Controls.Add(this.btnDisconnect);
            this.gbConnection.Controls.Add(this.nudSlot);
            this.gbConnection.Controls.Add(this.btnConnect);
            this.gbConnection.Controls.Add(this.nudRack);
            this.gbConnection.Controls.Add(this.label2);
            this.gbConnection.Controls.Add(this.txtIP);
            this.gbConnection.Controls.Add(this.label3);
            this.gbConnection.Controls.Add(this.label4);
            this.gbConnection.Location = new System.Drawing.Point(12, 12);
            this.gbConnection.Name = "gbConnection";
            this.gbConnection.Size = new System.Drawing.Size(369, 224);
            this.gbConnection.TabIndex = 6;
            this.gbConnection.TabStop = false;
            this.gbConnection.Text = "PLC连接配置";
            // 
            // gbVariables
            // 
            this.gbVariables.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbVariables.AutoSize = true;
            this.gbVariables.Controls.Add(this.dataGridView1);
            this.gbVariables.Location = new System.Drawing.Point(12, 228);
            this.gbVariables.Name = "gbVariables";
            this.gbVariables.Size = new System.Drawing.Size(810, 542);
            this.gbVariables.TabIndex = 7;
            this.gbVariables.TabStop = false;
            this.gbVariables.Text = "变量列表";
            // 
            // groupRecordMode
            // 
            this.groupRecordMode.Controls.Add(this.nudTriggerDelay);
            this.groupRecordMode.Controls.Add(this.lblDelayTime);
            this.groupRecordMode.Controls.Add(this.txtTriggerAddress);
            this.groupRecordMode.Controls.Add(this.lblTriggerAddress);
            this.groupRecordMode.Controls.Add(this.rbTriggered);
            this.groupRecordMode.Controls.Add(this.rbPeriodic);
            this.groupRecordMode.Location = new System.Drawing.Point(830, 252);
            this.groupRecordMode.Margin = new System.Windows.Forms.Padding(4);
            this.groupRecordMode.Name = "groupRecordMode";
            this.groupRecordMode.Padding = new System.Windows.Forms.Padding(4);
            this.groupRecordMode.Size = new System.Drawing.Size(354, 224);
            this.groupRecordMode.TabIndex = 4;
            this.groupRecordMode.TabStop = false;
            this.groupRecordMode.Text = "记录方式";
            // 
            // nudTriggerDelay
            // 
            this.nudTriggerDelay.Enabled = false;
            this.nudTriggerDelay.Location = new System.Drawing.Point(164, 116);
            this.nudTriggerDelay.Name = "nudTriggerDelay";
            this.nudTriggerDelay.Size = new System.Drawing.Size(116, 28);
            this.nudTriggerDelay.TabIndex = 4;
            // 
            // lblDelayTime
            // 
            this.lblDelayTime.AutoSize = true;
            this.lblDelayTime.Location = new System.Drawing.Point(22, 122);
            this.lblDelayTime.Name = "lblDelayTime";
            this.lblDelayTime.Size = new System.Drawing.Size(134, 18);
            this.lblDelayTime.TabIndex = 3;
            this.lblDelayTime.Text = "延迟时间（秒）";
            // 
            // txtTriggerAddress
            // 
            this.txtTriggerAddress.Location = new System.Drawing.Point(108, 81);
            this.txtTriggerAddress.Name = "txtTriggerAddress";
            this.txtTriggerAddress.Size = new System.Drawing.Size(172, 28);
            this.txtTriggerAddress.TabIndex = 2;
            // 
            // lblTriggerAddress
            // 
            this.lblTriggerAddress.AutoSize = true;
            this.lblTriggerAddress.Location = new System.Drawing.Point(20, 81);
            this.lblTriggerAddress.Name = "lblTriggerAddress";
            this.lblTriggerAddress.Size = new System.Drawing.Size(80, 18);
            this.lblTriggerAddress.TabIndex = 1;
            this.lblTriggerAddress.Text = "触发地址";
            // 
            // rbTriggered
            // 
            this.rbTriggered.AutoSize = true;
            this.rbTriggered.Location = new System.Drawing.Point(175, 38);
            this.rbTriggered.Name = "rbTriggered";
            this.rbTriggered.Size = new System.Drawing.Size(105, 22);
            this.rbTriggered.TabIndex = 0;
            this.rbTriggered.TabStop = true;
            this.rbTriggered.Text = "触发记录";
            this.rbTriggered.UseVisualStyleBackColor = true;
            this.rbTriggered.CheckedChanged += new System.EventHandler(this.rbTriggered_CheckedChanged);
            // 
            // rbPeriodic
            // 
            this.rbPeriodic.AutoSize = true;
            this.rbPeriodic.Location = new System.Drawing.Point(20, 38);
            this.rbPeriodic.Name = "rbPeriodic";
            this.rbPeriodic.Size = new System.Drawing.Size(105, 22);
            this.rbPeriodic.TabIndex = 0;
            this.rbPeriodic.TabStop = true;
            this.rbPeriodic.Text = "周期记录";
            this.rbPeriodic.UseVisualStyleBackColor = true;
            this.rbPeriodic.CheckedChanged += new System.EventHandler(this.rbPeriodic_CheckedChanged);
            // 
            // btnExportTemplate
            // 
            this.btnExportTemplate.Location = new System.Drawing.Point(26, 168);
            this.btnExportTemplate.Name = "btnExportTemplate";
            this.btnExportTemplate.Size = new System.Drawing.Size(150, 40);
            this.btnExportTemplate.TabIndex = 3;
            this.btnExportTemplate.Text = "导出变量模板";
            this.btnExportTemplate.UseVisualStyleBackColor = true;
            this.btnExportTemplate.Click += new System.EventHandler(this.btnExportTemplate_Click);
            // 
            // btnImport
            // 
            this.btnImport.Location = new System.Drawing.Point(26, 105);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(150, 40);
            this.btnImport.TabIndex = 3;
            this.btnImport.Text = "导入变量";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(22, 236);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(242, 18);
            this.label7.TabIndex = 2;
            this.label7.Text = "添加变量后点击首列勾选保存";
            // 
            // btnDelete
            // 
            this.btnDelete.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnDelete.Location = new System.Drawing.Point(26, 50);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(150, 40);
            this.btnDelete.TabIndex = 1;
            this.btnDelete.Text = "删除选中变量";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // gbCollection
            // 
            this.gbCollection.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.gbCollection.AutoSize = true;
            this.gbCollection.Controls.Add(this.btnBrowse);
            this.gbCollection.Controls.Add(this.txtOutputPath);
            this.gbCollection.Controls.Add(this.rbExcel);
            this.gbCollection.Controls.Add(this.rbCsv);
            this.gbCollection.Controls.Add(this.nudInterval);
            this.gbCollection.Controls.Add(this.btnStop);
            this.gbCollection.Controls.Add(this.btnStart);
            this.gbCollection.Controls.Add(this.lblStatus);
            this.gbCollection.Controls.Add(this.lblLastUpdate);
            this.gbCollection.Controls.Add(this.label6);
            this.gbCollection.Controls.Add(this.label5);
            this.gbCollection.Controls.Add(this.labelS);
            this.gbCollection.Controls.Add(this.label1);
            this.gbCollection.Location = new System.Drawing.Point(388, 12);
            this.gbCollection.Name = "gbCollection";
            this.gbCollection.Size = new System.Drawing.Size(801, 224);
            this.gbCollection.TabIndex = 1;
            this.gbCollection.TabStop = false;
            this.gbCollection.Text = "数据采集配置";
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(664, 81);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(82, 40);
            this.btnBrowse.TabIndex = 8;
            this.btnBrowse.Text = "浏览";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // txtOutputPath
            // 
            this.txtOutputPath.Location = new System.Drawing.Point(134, 88);
            this.txtOutputPath.Name = "txtOutputPath";
            this.txtOutputPath.Size = new System.Drawing.Size(500, 28);
            this.txtOutputPath.TabIndex = 10;
            // 
            // rbExcel
            // 
            this.rbExcel.AutoSize = true;
            this.rbExcel.Location = new System.Drawing.Point(550, 34);
            this.rbExcel.Name = "rbExcel";
            this.rbExcel.Size = new System.Drawing.Size(78, 22);
            this.rbExcel.TabIndex = 9;
            this.rbExcel.TabStop = true;
            this.rbExcel.Text = "Excel";
            this.rbExcel.UseVisualStyleBackColor = true;
            // 
            // rbCsv
            // 
            this.rbCsv.AutoSize = true;
            this.rbCsv.Location = new System.Drawing.Point(470, 36);
            this.rbCsv.Name = "rbCsv";
            this.rbCsv.Size = new System.Drawing.Size(60, 22);
            this.rbCsv.TabIndex = 9;
            this.rbCsv.TabStop = true;
            this.rbCsv.Text = "csv";
            this.rbCsv.UseVisualStyleBackColor = true;
            // 
            // nudInterval
            // 
            this.nudInterval.Location = new System.Drawing.Point(140, 34);
            this.nudInterval.Name = "nudInterval";
            this.nudInterval.Size = new System.Drawing.Size(98, 28);
            this.nudInterval.TabIndex = 8;
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(224, 128);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(118, 33);
            this.btnStop.TabIndex = 1;
            this.btnStop.Text = "停止采集";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(62, 128);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(118, 33);
            this.btnStart.TabIndex = 1;
            this.btnStart.Text = "开始采集";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(72, 182);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(62, 18);
            this.lblStatus.TabIndex = 0;
            this.lblStatus.Text = "状态：";
            // 
            // lblLastUpdate
            // 
            this.lblLastUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblLastUpdate.Location = new System.Drawing.Point(332, 182);
            this.lblLastUpdate.Name = "lblLastUpdate";
            this.lblLastUpdate.Size = new System.Drawing.Size(200, 18);
            this.lblLastUpdate.TabIndex = 0;
            this.lblLastUpdate.Text = "最后更新：";
            this.lblLastUpdate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(38, 92);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(98, 18);
            this.label6.TabIndex = 0;
            this.label6.Text = "输出路径：";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(368, 38);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(98, 18);
            this.label5.TabIndex = 0;
            this.label5.Text = "输出格式：";
            // 
            // labelS
            // 
            this.labelS.AutoSize = true;
            this.labelS.Location = new System.Drawing.Point(254, 45);
            this.labelS.Name = "labelS";
            this.labelS.Size = new System.Drawing.Size(26, 18);
            this.labelS.TabIndex = 0;
            this.labelS.Text = "秒";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(38, 45);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(98, 18);
            this.label1.TabIndex = 0;
            this.label1.Text = "采集间隔：";
            // 
            // toolTip1
            // 
            this.toolTip1.ShowAlways = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnDelete);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.btnExportTemplate);
            this.groupBox1.Controls.Add(this.btnImport);
            this.groupBox1.Location = new System.Drawing.Point(831, 484);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(352, 279);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "变量编辑";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1202, 782);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.gbCollection);
            this.Controls.Add(this.groupRecordMode);
            this.Controls.Add(this.gbVariables);
            this.Controls.Add(this.gbConnection);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "MainForm";
            this.Text = "S7-1500数据导出工具";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.SizeChanged += new System.EventHandler(this.Form1_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRack)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSlot)).EndInit();
            this.gbConnection.ResumeLayout(false);
            this.gbConnection.PerformLayout();
            this.gbVariables.ResumeLayout(false);
            this.groupRecordMode.ResumeLayout(false);
            this.groupRecordMode.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudTriggerDelay)).EndInit();
            this.gbCollection.ResumeLayout(false);
            this.gbCollection.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudInterval)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Button btnDisconnect;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtIP;
        private System.Windows.Forms.NumericUpDown nudRack;
        private System.Windows.Forms.NumericUpDown nudSlot;
        private System.Windows.Forms.GroupBox gbConnection;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.GroupBox gbVariables;
        private System.Windows.Forms.GroupBox gbCollection;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label lblLastUpdate;
        private System.Windows.Forms.TextBox txtOutputPath;
        private System.Windows.Forms.RadioButton rbCsv;
        private System.Windows.Forms.NumericUpDown nudInterval;
        private System.Windows.Forms.Label labelS;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button btnExportTemplate;
        private System.Windows.Forms.GroupBox groupRecordMode;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rbExcel;
        private System.Windows.Forms.TextBox txtTriggerAddress;
        private System.Windows.Forms.Label lblTriggerAddress;
        private System.Windows.Forms.RadioButton rbTriggered;
        private System.Windows.Forms.RadioButton rbPeriodic;
        private System.Windows.Forms.NumericUpDown nudTriggerDelay;
        private System.Windows.Forms.Label lblDelayTime;
    }
}

