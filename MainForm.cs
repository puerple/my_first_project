using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.SqlServer.Server;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using S7.Net;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;


namespace S7DataExporter
{
    public partial class MainForm : Form
    {
        private Plc plc;
        private bool isCollecting = false;
        private Thread collectionThread;
        private readonly float x; //定义当前窗体的宽度
        private readonly float y; //定义当前窗体的高度       
        private bool triggerActive = false;
        private DateTime triggerStartTime;


        public MainForm()
        {
            InitializeComponent();//初始化组件
            this.DoubleBuffered = true; // 启用双缓冲
            InitializeDataGridView();//表格初始化
            x = this.ClientRectangle.Width;
            y = this.ClientRectangle.Height;
            ReWinformLayoutHerlper.setTag(this);   // 设置窗体控件的缩放     
        }
        private void Form1_Resize(object sender, EventArgs e)
        {
            this.SuspendLayout(); // 暂停布局逻辑
            ReWinformLayoutHerlper.ReWinformLayout(this.ClientRectangle, x, y, this);
            this.ResumeLayout(true); // 恢复布局逻辑，一次性更新
        }
        private void InitializeDataGridView()
        {
            // 清除现有列
            dataGridView1.Columns.Clear();

            // 创建复选框列
            DataGridViewCheckBoxColumn checkBoxColumn = new DataGridViewCheckBoxColumn();
            checkBoxColumn.HeaderText = "选择";
            checkBoxColumn.Name = "selected";
            checkBoxColumn.Width = 50;
            dataGridView1.Columns.Add(checkBoxColumn);

            // 添加其他列
            dataGridView1.Columns.Add("name", "变量名");
            dataGridView1.Columns.Add("address", "PLC地址");
            dataGridView1.Columns.Add("type", "数据类型");

            // 设置列宽
            dataGridView1.Columns["name"].Width = 150;
            dataGridView1.Columns["address"].Width = 100;
            dataGridView1.Columns["type"].Width = 80;
        }

        

        

        private void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                // 获取连接参数
                string ip = txtIP.Text.Trim();
                short rack = (short)nudRack.Value;
                short slot = (short)nudSlot.Value;

                // 创建PLC连接
                plc = new Plc(CpuType.S71500, ip, rack, slot);
                plc.Open();
                plc.ReadTimeout = 5000;

                if (plc.IsConnected)
                {
                    UpdateStatus($"已连接到PLC {ip}", true);
                    btnConnect.Enabled = false;
                    btnDisconnect.Enabled = true;
                    //gbConnection.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"连接失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                File.AppendAllText("error.log", $"{DateTime.Now}: {ex}\n");
            }
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            try
            {
                if (plc != null && plc.IsConnected)
                {
                    plc.Close();
                    UpdateStatus("已断开PLC连接", false);
                    btnConnect.Enabled = true;
                    btnDisconnect.Enabled = false;
                    //gbConnection.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"断开连接时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                File.AppendAllText("error.log", $"{DateTime.Now}: {ex}\n");
            }
            StopCollection();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (!isCollecting)
            {
                // 验证输出路径
                if (string.IsNullOrWhiteSpace(txtOutputPath.Text))
                {
                    MessageBox.Show("请选择输出文件路径", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                //检查文件是否存在且是否有数据（判断是否需要写表头）
                bool fileExists = File.Exists(txtOutputPath.Text);
                bool writeHeader = !fileExists || new FileInfo(txtOutputPath.Text).Length == 0;

                // 获取采集间隔
                int interval = (int)nudInterval.Value * 1000;

                // 重置触发状态
                triggerState = TriggerState.Idle;
                lastTriggerState = false;

                // 启动采集线程
                isCollecting = true;
                btnStart.Enabled = false;
                btnStop.Enabled = true;

                collectionThread = new Thread(() => CollectData(interval, writeHeader));
                collectionThread.Start();

                UpdateStatus("数据采集中...", true);
            }
        }
        
        private bool lastTriggerState = false; // 记录上一次触发信号的状态
        private bool waitingForFall = false;   // 标记是否在等待触发信号下降沿
        private DateTime lastStatusUpdate = DateTime.MinValue;
        private void CollectData(int interval, bool writeHeader)//采集数据
        {
            try
            {
                // 确定输出格式
                bool exportToExcel = rbExcel.Checked;
                string filePath = txtOutputPath.Text;

                // 检查文件可访问性
                try
                {
                    if (writeHeader)
                    {
                        using (var fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read))
                        {
                            // 测试文件可写性
                        }
                    }
                }
                catch (Exception ex)
                {
                    this.Invoke((MethodInvoker)delegate
                    {
                        MessageBox.Show($"无法访问文件: {ex.Message}\n请选择其他文件路径", "错误",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        StopCollection();
                        File.AppendAllText("error.log", $"{DateTime.Now}: {ex}\n");
                    });
                    return;
                }
                // 确定记录方式
                bool isPeriodicMode = true;
                string triggerAddress = "";
                double delaySeconds = 0;

                // 检查UI控件确定记录模式
                this.Invoke((MethodInvoker)delegate
                {
                    foreach (System.Windows.Forms.Control c in Controls)
                    {
                        if (c is GroupBox && c.Text == "记录方式")
                        {
                            foreach (RadioButton rb in c.Controls.OfType<RadioButton>())
                            {
                                if (rb.Checked)
                                {
                                    isPeriodicMode = (rb.Tag as string) == "periodic";
                                    break;
                                }
                            }

                            if (!isPeriodicMode)
                            {
                                triggerAddress = txtTriggerAddress.Text;
                                delaySeconds = (double)nudTriggerDelay.Value;
                            }
                            break;
                        }
                    }
                });


                // 持续采集数据
                while (isCollecting)
                {
                    try
                    {
                        bool shouldRecord = false;// 默认不记录
                        bool isPeriodicMode = true; // 实际应从UI获取

                        // 触发记录模式处理
                        if (!isPeriodicMode)
                        {

                            // 检查触发条件
                            if (!triggerActive)
                            {
                                // 读取触发信号
                                object triggerValue = plc.Read(triggerAddress);
                                bool currentTriggerState = false;
                                if (triggerValueObj is bool)
                                {
                                    currentTriggerState = (bool)triggerValueObj;
                                }
                                // 修改2：添加调试输出（可选）
                                Debug.WriteLine($"触发信号: {currentTriggerState} (上次: {lastTriggerState}) at {DateTime.Now:HH:mm:ss.fff}");

                                // 检查是否在等待下降沿
                                if (waitingForFall)
                                {
                                    if (!currentTriggerState) // 检测到下降沿
                                    {
                                        waitingForFall = false;
                                        UpdateStatus("触发信号已复位，等待下一次上升沿", false);
                                    }
                                }
                                // 检查上升沿（从假变为真）
                                else if (!lastTriggerState && currentTriggerState)
                                {
                                    // 检测到上升沿，激活触发
                                    triggerActive = true;
                                    triggerStartTime = DateTime.Now;
                                    UpdateStatus($"触发激活 - 开始计时...", true);
                                }
                            }

                            // 如果触发已激活，检查是否达到延迟时间
                            if (triggerActive)
                            {
                                double elapsedSeconds = (DateTime.Now - triggerStartTime).TotalSeconds;

                                // 限制状态更新频率：每500ms更新一次
                                if ((DateTime.Now - lastStatusUpdate).TotalMilliseconds >= 500)
                                {
                                    UpdateStatus($"触发激活 - 已等待: {elapsedSeconds:F1}/{delaySeconds:F1}秒", true);
                                    lastStatusUpdate = DateTime.Now;
                                }

                                // 如果未达到延迟时间，跳过本次记录
                                if (elapsedSeconds >= delaySeconds)
                                {
                                    shouldRecord = true; // 延迟时间到，允许记录
                                    triggerActive = false; // 结束本次触发
                                    waitingForFall = true; // 开始等待下降沿
                                    UpdateStatus($"触发记录完成，等待信号复位", true);
                                }
                                // 更新上一次触发信号状态
                                lastTriggerState = currentTriggerState;
                            }
                            else
                            {
                                shouldRecord = false; // 延迟时间未到，禁止记录
                            }
                        }
                        else// 周期记录模式处理
                        {
                            shouldRecord = true;
                        }

                        if (shouldRecord)
                        {
                            var selectedVars = GetSelectedVariables();
                            var data = ReadPlcData(selectedVars);
                            SafeAppendToCsv(filePath, data, writeHeader);
                            writeHeader = false;
                            // 添加触发信号重置检查
                            object currentTrigger = plc.Read(triggerAddress);
                            if (currentTrigger is bool && (bool)currentTrigger)
                            {
                                UpdateStatus("警告：触发信号仍为高电平", true);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        this.Invoke((MethodInvoker)delegate
                        {
                            lblLastUpdate.Text = $"错误: {ex.Message}";
                            File.AppendAllText("error.log", $"{DateTime.Now}: {ex}\n");
                        });
                    }

                    Thread.Sleep(interval);
                }
            }
            catch (Exception ex)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    MessageBox.Show($"采集线程崩溃: {ex.Message}", "严重错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    ForceStopCollection();
                    File.AppendAllText("error.log", $"{DateTime.Now}: {ex}\n");
                    //StopCollection();
                });
            }
        }
        private void SafeAppendToCsv(string filePath, DataTable data, bool writeHeader)
        {
            this.Invoke((MethodInvoker)delegate
            {
                try
                {
                    using (var fs = new FileStream(filePath, FileMode.Append, FileAccess.Write, FileShare.Read))
                    using (var sw = new StreamWriter(fs))
                    {
                        if (writeHeader)
                        {
                            sw.Write("Timestamp,");
                            foreach (DataGridViewRow row in dataGridView1.Rows)
                            {
                                if (Convert.ToBoolean(row.Cells["selected"].Value))
                                {
                                    sw.Write(row.Cells["name"].Value + ",");
                                }
                            }
                            sw.WriteLine();
                        }

                        foreach (DataRow row in data.Rows)
                        {
                            sw.Write(row["Timestamp"] + ",");
                            for (int i = 1; i < data.Columns.Count; i++)
                            {
                                sw.Write(row[i] + ",");
                            }
                            sw.WriteLine();
                        }
                    }

                    lblLastUpdate.Text = "最后更新: " + DateTime.Now.ToString("HH:mm:ss");
                }
                catch (Exception ex)
                {
                    throw new Exception($"写入CSV失败: {ex.Message}");
                    File.AppendAllText("error.log", $"{DateTime.Now}: {ex}\n");
                }
            });
        }
        private void ForceStopCollection()
        {
            try
            {
                isCollecting = false;
                collectionThread?.Abort(); // 紧急情况下终止线程
            }
            finally
            {
                this.Invoke((MethodInvoker)delegate
                {
                    btnStart.Enabled = true;
                    btnStop.Enabled = false;
                    UpdateStatus("采集已强制停止", false);
                });
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (isCollecting)
            {
                DialogResult result = MessageBox.Show("数据采集中，确定要退出吗？", "警告",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.No)
                {
                    e.Cancel = true;
                    return;
                }
            }

            StopCollection();
            base.OnFormClosing(e);
        }
        private DataTable GetSelectedVariables()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Name");
            dt.Columns.Add("Address");
            dt.Columns.Add("Type");

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (Convert.ToBoolean(row.Cells["selected"].Value))
                {
                    DataRow dr = dt.NewRow();
                    dr["Name"] = row.Cells["name"].Value.ToString();
                    dr["Address"] = row.Cells["address"].Value.ToString();
                    dr["Type"] = row.Cells["type"].Value.ToString();
                    dt.Rows.Add(dr);
                }
            }
            return dt;
        }

        private DataTable ReadPlcData(DataTable variables)
        {
            DataTable result = new DataTable();
            result.Columns.Add("Timestamp");

            foreach (DataRow var in variables.Rows)
            {
                result.Columns.Add(var["Name"].ToString());
            }

            DataRow row = result.NewRow();
            row["Timestamp"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            foreach (DataRow var in variables.Rows)
            {
                string address = var["Address"].ToString();
                string type = var["Type"].ToString().ToUpper(); // 转换为大写确保匹配
                string varName = var["Name"].ToString();

                try
                {
                    object value = plc.Read(address);

                    // 根据数据类型进行转换
                    switch (type)
                    {
                        case "REAL":
                            if (value is uint)
                            {
                                byte[] bytes = BitConverter.GetBytes((uint)value);
                                float floatValue = BitConverter.ToSingle(bytes, 0);
                                row[varName] = floatValue.ToString("F4");
                            }
                            else
                            {
                                row[varName] = value?.ToString() ?? "NULL";
                            }
                            break;

                        case "INT":
                            if (value is short)
                            {
                                row[varName] = ((short)value).ToString();
                            }
                            else
                            {
                                row[varName] = value?.ToString() ?? "NULL";
                            }
                            break;

                        case "DINT":
                            if (value is int)
                            {
                                row[varName] = ((int)value).ToString();
                            }
                            else
                            {
                                row[varName] = value?.ToString() ?? "NULL";
                            }
                            break;

                        case "WORD": // 16位无符号整数
                            if (value is ushort)
                            {
                                // 转换为4位十六进制字符串
                                row[varName] = "0x" + ((ushort)value).ToString("X4");
                            }
                            else
                            {
                                row[varName] = value?.ToString() ?? "NULL";
                            }
                            break;

                        default:
                            row[varName] = value?.ToString() ?? "NULL";
                            break;
                    }
                }
                catch
                {
                    row[varName] = "ERROR";
                }
            }
            result.Rows.Add(row);
            return result;
        }
        
        private void CreateCsvFile(string filePath)
        {
            // 如果文件已存在且不为空，则不再写入表头
            if (File.Exists(filePath) && new FileInfo(filePath).Length > 0)
                return;
            using (StreamWriter sw = new StreamWriter(filePath, false))
            {
                sw.Write("Timestamp,");
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (Convert.ToBoolean(row.Cells["selected"].Value))
                    {
                        string varName = row.Cells["name"].Value.ToString();
                        string type = row.Cells["type"].Value.ToString();
                        //sw.Write(row.Cells["name"].Value + ",");
                        if (type == "REAL")
                        {
                            sw.Write(varName + "(REAL),");
                        }
                        else if (type == "INT")
                        {
                            sw.Write(varName + "(INT),");
                        }
                        else if (type == "DINT")
                        {
                            sw.Write(varName + "(DINT),");
                        }
                        else if (type == "BOOL")
                        {
                            sw.Write(varName + "(BOOL),");
                        }
                        else
                        {
                            sw.Write(varName + ",");
                        }
                    }
                    sw.WriteLine();
                }
            }
        }


        private void AppendToExcel(string filePath, DataTable data)
        {
            // 实际项目中应使用EPPlus等库
            AppendToCsv(filePath, data); // 简化为追加到CSV
        }
        private void AppendToCsv(string filePath, DataTable data)
        {
            this.Invoke((MethodInvoker)delegate
            {
                using (StreamWriter sw = new StreamWriter(filePath, true))
                {
                    foreach (DataRow row in data.Rows)
                    {
                        sw.Write(row["Timestamp"] + ",");
                        for (int i = 1; i < data.Columns.Count; i++)
                        {
                            sw.Write(row[i] + ",");
                        }
                        sw.WriteLine();
                    }
                }

                lblLastUpdate.Text = "最后更新: " + DateTime.Now.ToString("HH:mm:ss");
            });
        }
        private void btnStop_Click(object sender, EventArgs e)
        {
            StopCollection();
        }
        private void StopCollection()
        {
            try
            {
                isCollecting = false;
                if (collectionThread != null && collectionThread.IsAlive)
                {
                    if (!collectionThread.Join(2000)) // 等待5秒
                    {
                        collectionThread.Abort(); // 超时后强制终止线程
                    }
                }
            }
            finally
            {
                this.Invoke((MethodInvoker)delegate
                {
                    btnStart.Enabled = true;
                    btnStop.Enabled = false;
                    UpdateStatus("采集已停止", false);
                });
            }
        }
        private void btnBrowse_Click(object sender, EventArgs e)
        {
            // 使用Invoke确保在UI线程执行
            this.Invoke((MethodInvoker)delegate
            {
                try
                {
                    SaveFileDialog saveFileDialog = new SaveFileDialog();
                    saveFileDialog.Filter = rbExcel.Checked ? "Excel文件|*.xlsx" : "CSV文件|*.csv";
                    saveFileDialog.DefaultExt = rbExcel.Checked ? ".xlsx" : ".csv";
                    saveFileDialog.OverwritePrompt = true; // 启用覆盖提示

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        // 在新线程中检查文件
                        Task.Run(() =>
                        {
                            try
                            {
                                string selectedPath = saveFileDialog.FileName;
                                // 检查文件是否被占用
                                if (File.Exists(selectedPath))
                                {
                                    using (FileStream fs = File.Open(selectedPath, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
                                    {
                                        // 文件可访问
                                    }
                                }

                                this.Invoke((MethodInvoker)delegate
                                {
                                    txtOutputPath.Text = selectedPath;
                                });
                            }
                            catch (IOException ex)
                            {
                                this.Invoke((MethodInvoker)delegate
                                {
                                    MessageBox.Show($"文件被占用或不可访问: {ex.Message}", "错误",
                                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                                });
                            }
                        });
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"选择文件时出错: {ex.Message}", "错误",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            });


            /*SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = rbExcel.Checked ?
             "Excel文件|*.xlsx" : "CSV文件|*.csv";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                    txtOutputPath.Text = saveFileDialog.FileName;
            }*/
        }

        private void UpdateStatus(string message, bool isError = false)
        {
            // 添加资源检查
            if (this.IsDisposed || !this.IsHandleCreated)
                return;

            // 确保不会频繁创建新Timer
            if (isError && this.components == null)
            {
                this.components = new System.ComponentModel.Container();
            }

            this.Invoke((MethodInvoker)delegate
            {
                // 限制文本最大长度（建议30个字符）
                string displayText = message.Length > 30 ? message.Substring(0, 27) + "..." : message;

                lblLastUpdate.Text = displayText;
                lblLastUpdate.ForeColor = isError ? System.Drawing.Color.Red : System.Drawing.Color.Black;

                // 添加ToolTip显示完整信息
                toolTip1.SetToolTip(lblLastUpdate, message);
                

                // 自动恢复正常状态（5秒后）
                if (isError)
                {
                    System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer { Interval = 5000 };
                    timer.Tick += (s, e) =>
                    {
                        lblLastUpdate.Text = "最后更新: " + DateTime.Now.ToString("HH:mm:ss");
                        lblLastUpdate.ForeColor = System.Drawing.Color.Black;
                        timer.Stop();
                        timer.Dispose();
                    };
                    timer.Start();
                }
            });
        }

        private string configFilePath = Path.Combine(Application.StartupPath, "S7DataExporter.config");
        private void SaveSettings()//保存输入框的设置
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(configFilePath, false))
                {
                    // 保存连接设置
                    sw.WriteLine(txtIP.Text);
                    sw.WriteLine(nudRack.Value);
                    sw.WriteLine(nudSlot.Value);

                    

                    // 保存采集间隔
                    sw.WriteLine(nudInterval.Value);

                    // 保存输出路径
                    sw.WriteLine(txtOutputPath.Text);

                    // 保存输出格式
                    sw.WriteLine(rbExcel.Checked ? "Excel" : "CSV");

                    // 保存变量列表
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        if (row.Cells["name"].Value != null)
                        {
                            sw.WriteLine($"{row.Cells["selected"].Value}|{row.Cells["name"].Value}|{row.Cells["address"].Value}|{row.Cells["type"].Value}");
                        }
                    }
                    // 保存记录模式设置
                    string recordMode = "periodic";
                    string triggerAddress = "";
                    decimal delay = 5;

                    this.Invoke((MethodInvoker)delegate
                    {
                        foreach (System.Windows.Forms.Control c in Controls)
                        {
                            if (c is GroupBox && c.Text == "记录方式")
                            {
                                foreach (RadioButton rb in c.Controls.OfType<RadioButton>())
                                {
                                    if (rb.Checked)
                                    {
                                        recordMode = rb.Tag as string;
                                        break;
                                    }
                                }

                                triggerAddress = txtTriggerAddress.Text;
                                delay = nudTriggerDelay.Value;
                                break;
                            }
                        }
                    });

                    sw.WriteLine(recordMode);
                    sw.WriteLine(triggerAddress);
                    sw.WriteLine(delay);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"保存设置时出错: {ex.Message}");
                File.AppendAllText("error.log", $"{DateTime.Now}: {ex}\n");
            }
        }

        private void LoadSettings()//加载输入框的设置
        {
            if (File.Exists(configFilePath))
            {
                try
                {
                    using (StreamReader sr = new StreamReader(configFilePath))
                    {
                        // 加载连接设置
                        txtIP.Text = sr.ReadLine();
                        nudRack.Value = decimal.Parse(sr.ReadLine());
                        nudSlot.Value = decimal.Parse(sr.ReadLine());

                        

                        // 加载采集间隔
                        nudInterval.Value = decimal.Parse(sr.ReadLine());

                        // 加载输出路径
                        txtOutputPath.Text = sr.ReadLine();

                        // 加载输出格式
                        string format = sr.ReadLine();
                        rbExcel.Checked = (format == "Excel");
                        rbCsv.Checked = (format != "Excel");

                        // 加载变量列表
                        dataGridView1.Rows.Clear();
                        while (!sr.EndOfStream)
                        {
                            string[] parts = sr.ReadLine().Split('|');
                            if (parts.Length >= 4)
                            {
                                bool selected = bool.Parse(parts[0]);
                                string name = parts[1];
                                string address = parts[2];
                                string type = parts[3];

                                dataGridView1.Rows.Add(selected, name, address, type);
                            }
                        }

                        // 加载记录模式设置
                        string recordMode = sr.ReadLine();
                        string triggerAddress = sr.ReadLine();
                        decimal delay = decimal.Parse(sr.ReadLine());

                        this.Invoke((MethodInvoker)delegate
                        {
                            foreach (System.Windows.Forms.Control c in Controls)
                            {
                                if (c is GroupBox && c.Text == "记录方式")
                                {
                                    foreach (RadioButton rb in c.Controls.OfType<RadioButton>())
                                    {
                                        if ((rb.Tag as string) == recordMode)
                                        {
                                            rb.Checked = true;
                                            break;
                                        }
                                    }

                                    txtTriggerAddress.Text = triggerAddress;
                                    nudTriggerDelay.Value = delay;

                                    // 根据模式启用/禁用控件
                                    txtTriggerAddress.Enabled = (recordMode == "triggered");
                                    nudTriggerDelay.Enabled = (recordMode == "triggered");
                                    break;
                                }
                            }
                        });
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"加载设置时出错: {ex.Message}");
                    File.AppendAllText("error.log", $"{DateTime.Now}: {ex}\n");
                }
            }
            
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            StopCollection();

            if (plc != null && plc.IsConnected)
            {
                plc.Close();
            }
            // 保存设置
            SaveSettings();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            LoadSettings();
            //ReWinformLayoutHerlper.ReWinformLayout(this.ClientRectangle, x, y, this);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                // 确保不在采集数据时删除变量
                if (isCollecting)
                {
                    MessageBox.Show("请先停止数据采集再删除变量", "提示",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 获取选中的行索引（倒序处理，避免索引变化问题）
                var selectedIndices = dataGridView1.SelectedCells
                    .OfType<DataGridViewCell>()
                    .Select(c => c.RowIndex)
                    .Distinct()
                    .OrderByDescending(i => i)
                    .ToList();

                if (selectedIndices.Count == 0)
                {
                    MessageBox.Show("请先选择要删除的变量行", "提示",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // 确认删除
                if (MessageBox.Show($"确定要删除选中的 {selectedIndices.Count} 个变量吗？", "确认删除",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    return;
                }

                // 删除选中的行（倒序删除避免索引问题）
                foreach (int rowIndex in selectedIndices)
                {
                    if (rowIndex >= 0 && rowIndex < dataGridView1.Rows.Count)
                    {
                        dataGridView1.Rows.RemoveAt(rowIndex);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"删除变量时出错: {ex.Message}", "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                File.AppendAllText("error.log", $"{DateTime.Now}: {ex}\n");
            }
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            try
            {
                if (isCollecting)
                {
                    MessageBox.Show("请先停止数据采集再导入变量", "提示",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.Filter = "Excel文件|*.xlsx;*.xls";
                    openFileDialog.Title = "选择变量配置文件";

                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        ImportVariablesFromExcel(openFileDialog.FileName);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"导入变量失败: {ex.Message}", "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void ImportVariablesFromExcel(string filePath)
        {
            try
            {
                // 使用 OpenXml 读取 Excel 文件
                using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(filePath, false))
                {
                    WorkbookPart workbookPart = spreadsheetDocument.WorkbookPart;
                    WorksheetPart worksheetPart = workbookPart.WorksheetParts.First();
                    SheetData sheetData = worksheetPart.Worksheet.Elements<SheetData>().First();

                    // 检查表头格式
                    Row headerRow = sheetData.Elements<Row>().First();
                    if (GetCellValue(workbookPart, headerRow.Elements<Cell>().ElementAt(0)) != "变量名" ||
                        GetCellValue(workbookPart, headerRow.Elements<Cell>().ElementAt(1)) != "PLC地址" ||
                        GetCellValue(workbookPart, headerRow.Elements<Cell>().ElementAt(2)) != "数据类型")
                    {
                        throw new Exception("Excel格式不正确，请使用标准模板");
                    }

                    // 清空现有变量
                    dataGridView1.Rows.Clear();

                    // 读取数据行（从第2行开始）
                    foreach (Row row in sheetData.Elements<Row>().Skip(1))
                    {
                        string name = GetCellValue(workbookPart, row.Elements<Cell>().ElementAt(0));
                        string address = GetCellValue(workbookPart, row.Elements<Cell>().ElementAt(1));
                        string type = GetCellValue(workbookPart, row.Elements<Cell>().ElementAt(2));

                        if (string.IsNullOrEmpty(name)) // 如果变量名为空，跳过
                            continue;

                        // 验证数据类型
                        if (!new[] { "REAL", "BOOL", "INT", "DINT" }.Contains(type))
                        {
                            throw new Exception($"行 {row.RowIndex}: 不支持的数据类型 '{type}'");
                        }

                        // 添加到 DataGridView
                        dataGridView1.Rows.Add(true, name, address, type);
                    }

                    MessageBox.Show($"成功导入 {dataGridView1.Rows.Count} 个变量", "导入完成",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"导入变量失败: {ex.Message}", "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // 辅助方法：获取单元格的值
        private string GetCellValue(WorkbookPart workbookPart, Cell cell)
        {
            if (cell == null || cell.CellValue == null)
                return string.Empty;

            string value = cell.CellValue.InnerText;

            // 如果是共享字符串（如 Excel 的文本单元格）
            if (cell.DataType != null && cell.DataType.Value == CellValues.SharedString)
            {
                SharedStringTablePart stringTablePart = workbookPart.GetPartsOfType<SharedStringTablePart>().First();
                value = stringTablePart.SharedStringTable.ElementAt(int.Parse(value)).InnerText;
            }

            return value;
        }

        private void btnExportTemplate_Click(object sender, EventArgs e)
        {
            try
            {
                using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.Filter = "Excel文件|*.xlsx";
                    saveFileDialog.Title = "保存变量模板";
                    saveFileDialog.FileName = "变量导入模板.xlsx";

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        // 创建新的 Excel 文件
                        using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Create(
                            saveFileDialog.FileName, SpreadsheetDocumentType.Workbook))
                        {
                            // 添加 WorkbookPart
                            WorkbookPart workbookPart = spreadsheetDocument.AddWorkbookPart();
                            workbookPart.Workbook = new Workbook();

                            // 添加 WorksheetPart
                            WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                            worksheetPart.Worksheet = new Worksheet(new SheetData());

                            // 添加 Sheets
                            Sheets sheets = spreadsheetDocument.WorkbookPart.Workbook.AppendChild(new Sheets());
                            Sheet sheet = new Sheet()
                            {
                                Id = spreadsheetDocument.WorkbookPart.GetIdOfPart(worksheetPart),
                                SheetId = 1,
                                Name = "变量模板"
                            };
                            sheets.Append(sheet);

                            // 获取 SheetData
                            SheetData sheetData = worksheetPart.Worksheet.GetFirstChild<SheetData>();

                            // 添加表头
                            Row headerRow = new Row();
                            headerRow.Append(
                                new Cell() { CellValue = new CellValue("变量名"), DataType = CellValues.String },
                                new Cell() { CellValue = new CellValue("PLC地址"), DataType = CellValues.String },
                                new Cell() { CellValue = new CellValue("数据类型"), DataType = CellValues.String }
                            );
                            sheetData.Append(headerRow);

                            // 添加示例数据
                            Row exampleRow1 = new Row();
                            exampleRow1.Append(
                                new Cell() { CellValue = new CellValue("温度1"), DataType = CellValues.String },
                                new Cell() { CellValue = new CellValue("DB1.DBD0"), DataType = CellValues.String },
                                new Cell() { CellValue = new CellValue("REAL"), DataType = CellValues.String }
                            );
                            sheetData.Append(exampleRow1);

                            Row exampleRow2 = new Row();
                            exampleRow2.Append(
                                new Cell() { CellValue = new CellValue("运行状态"), DataType = CellValues.String },
                                new Cell() { CellValue = new CellValue("DB1.DBX0.0"), DataType = CellValues.String },
                                new Cell() { CellValue = new CellValue("BOOL"), DataType = CellValues.String }
                            );
                            sheetData.Append(exampleRow2);

                            workbookPart.Workbook.Save();
                        }

                        MessageBox.Show("模板导出成功", "完成",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"导出模板失败: {ex.Message}", "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        //周期性记录方式的处理
        private void rbPeriodic_CheckedChanged(object sender, EventArgs e)
        {
            txtTriggerAddress.Enabled = false;
            nudTriggerDelay.Enabled = false;
            nudInterval.Enabled = true; // 启用采集间隔设置
        }
        // 触发性记录方式的处理
        private void rbTriggered_CheckedChanged(object sender, EventArgs e)
        {
            txtTriggerAddress.Enabled = true;
            nudTriggerDelay.Enabled = true;
            nudInterval.Enabled = false; // 禁用采集间隔设置
        }
    }
}



