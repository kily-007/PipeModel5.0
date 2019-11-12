using System.Drawing;

namespace PipeModel
{
    partial class MainForms
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

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForms));
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea17 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend17 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series17 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea18 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend18 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series18 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.文件ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.新建ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.打开ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.保存ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.退出ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.操作ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.usb连接ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.断开连接ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.调试ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.扫描ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.开始高速扫描ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.结束本次扫描ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.报表ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.生成报表ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.导出报表ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.显示ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.实时波形图ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.实时2D图ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.实时3D图ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.实时3D图ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.补正ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.补正ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.还原ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.显示ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this._timerHighSpeed = new System.Windows.Forms.Timer(this.components);
            this._profileFileSave = new System.Windows.Forms.SaveFileDialog();
            this._profileOpenFile = new System.Windows.Forms.OpenFileDialog();
            this._pnlDeviceId = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this._lblReceiveProfileCount0 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this._rdDevice0 = new System.Windows.Forms.RadioButton();
            this._lblDeviceStatus0 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tabControl_View = new System.Windows.Forms.TabControl();
            this.tabPage_View = new System.Windows.Forms.TabPage();
            this.chart_Profile = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.chart_Vertical = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.wb_Profile = new System.Windows.Forms.WebBrowser();
            this.tabPage_Data = new System.Windows.Forms.TabPage();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.btn_refresh = new System.Windows.Forms.Button();
            this.btn_query = new System.Windows.Forms.Button();
            this.tbx_queryKeyword = new System.Windows.Forms.TextBox();
            this.dt_queryEndTime = new System.Windows.Forms.DateTimePicker();
            this.dt_queryStartTime = new System.Windows.Forms.DateTimePicker();
            this.label6 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.dgv_historyDataTable = new System.Windows.Forms.DataGridView();
            this.label7 = new System.Windows.Forms.Label();
            this.lb_status = new System.Windows.Forms.Label();
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.skinEngine = new Sunisoft.IrisSkin.SkinEngine();
            this.cb_selectSkin = new System.Windows.Forms.ComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtBox_Log = new System.Windows.Forms.TextBox();
            this.dgv_scanInfoDataTable = new System.Windows.Forms.DataGridView();
            this.menuStrip1.SuspendLayout();
            this._pnlDeviceId.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.tabControl_View.SuspendLayout();
            this.tabPage_View.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart_Profile)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart_Vertical)).BeginInit();
            this.tabPage_Data.SuspendLayout();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_historyDataTable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_scanInfoDataTable)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.文件ToolStripMenuItem,
            this.操作ToolStripMenuItem,
            this.扫描ToolStripMenuItem1,
            this.报表ToolStripMenuItem,
            this.显示ToolStripMenuItem,
            this.补正ToolStripMenuItem,
            this.显示ToolStripMenuItem1});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1462, 29);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 文件ToolStripMenuItem
            // 
            this.文件ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.新建ToolStripMenuItem,
            this.打开ToolStripMenuItem,
            this.保存ToolStripMenuItem,
            this.退出ToolStripMenuItem});
            this.文件ToolStripMenuItem.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.文件ToolStripMenuItem.Name = "文件ToolStripMenuItem";
            this.文件ToolStripMenuItem.Size = new System.Drawing.Size(54, 25);
            this.文件ToolStripMenuItem.Text = "文件";
            // 
            // 新建ToolStripMenuItem
            // 
            this.新建ToolStripMenuItem.Name = "新建ToolStripMenuItem";
            this.新建ToolStripMenuItem.Size = new System.Drawing.Size(112, 26);
            this.新建ToolStripMenuItem.Text = "新建";
            // 
            // 打开ToolStripMenuItem
            // 
            this.打开ToolStripMenuItem.Name = "打开ToolStripMenuItem";
            this.打开ToolStripMenuItem.Size = new System.Drawing.Size(112, 26);
            this.打开ToolStripMenuItem.Text = "打开";
            // 
            // 保存ToolStripMenuItem
            // 
            this.保存ToolStripMenuItem.Name = "保存ToolStripMenuItem";
            this.保存ToolStripMenuItem.Size = new System.Drawing.Size(112, 26);
            this.保存ToolStripMenuItem.Text = "保存";
            this.保存ToolStripMenuItem.Click += new System.EventHandler(this.保存ToolStripMenuItem_Click);
            // 
            // 退出ToolStripMenuItem
            // 
            this.退出ToolStripMenuItem.Name = "退出ToolStripMenuItem";
            this.退出ToolStripMenuItem.Size = new System.Drawing.Size(112, 26);
            this.退出ToolStripMenuItem.Text = "退出";
            this.退出ToolStripMenuItem.Click += new System.EventHandler(this.退出ToolStripMenuItem_Click);
            // 
            // 操作ToolStripMenuItem
            // 
            this.操作ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.usb连接ToolStripMenuItem,
            this.断开连接ToolStripMenuItem,
            this.调试ToolStripMenuItem});
            this.操作ToolStripMenuItem.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.操作ToolStripMenuItem.Name = "操作ToolStripMenuItem";
            this.操作ToolStripMenuItem.Size = new System.Drawing.Size(54, 25);
            this.操作ToolStripMenuItem.Text = "连接";
            // 
            // usb连接ToolStripMenuItem
            // 
            this.usb连接ToolStripMenuItem.Name = "usb连接ToolStripMenuItem";
            this.usb连接ToolStripMenuItem.Size = new System.Drawing.Size(152, 26);
            this.usb连接ToolStripMenuItem.Text = "USB连接";
            this.usb连接ToolStripMenuItem.Click += new System.EventHandler(this.USBToolStripMenuItem_Click);
            // 
            // 断开连接ToolStripMenuItem
            // 
            this.断开连接ToolStripMenuItem.Name = "断开连接ToolStripMenuItem";
            this.断开连接ToolStripMenuItem.Size = new System.Drawing.Size(152, 26);
            this.断开连接ToolStripMenuItem.Text = "网线连接";
            this.断开连接ToolStripMenuItem.Click += new System.EventHandler(this.网线连接ToolStripMenuItem_Click);
            // 
            // 调试ToolStripMenuItem
            // 
            this.调试ToolStripMenuItem.Name = "调试ToolStripMenuItem";
            this.调试ToolStripMenuItem.Size = new System.Drawing.Size(152, 26);
            this.调试ToolStripMenuItem.Text = "调试";
            this.调试ToolStripMenuItem.Click += new System.EventHandler(this.调试ToolStripMenuItem_Click_1);
            // 
            // 扫描ToolStripMenuItem1
            // 
            this.扫描ToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.开始高速扫描ToolStripMenuItem,
            this.结束本次扫描ToolStripMenuItem});
            this.扫描ToolStripMenuItem1.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.扫描ToolStripMenuItem1.Name = "扫描ToolStripMenuItem1";
            this.扫描ToolStripMenuItem1.Size = new System.Drawing.Size(54, 25);
            this.扫描ToolStripMenuItem1.Text = "扫描";
            // 
            // 开始高速扫描ToolStripMenuItem
            // 
            this.开始高速扫描ToolStripMenuItem.Name = "开始高速扫描ToolStripMenuItem";
            this.开始高速扫描ToolStripMenuItem.Size = new System.Drawing.Size(176, 26);
            this.开始高速扫描ToolStripMenuItem.Text = "开始高速扫描";
            this.开始高速扫描ToolStripMenuItem.Click += new System.EventHandler(this.开始高速扫描ToolStripMenuItem_Click);
            // 
            // 结束本次扫描ToolStripMenuItem
            // 
            this.结束本次扫描ToolStripMenuItem.Name = "结束本次扫描ToolStripMenuItem";
            this.结束本次扫描ToolStripMenuItem.Size = new System.Drawing.Size(176, 26);
            this.结束本次扫描ToolStripMenuItem.Text = "结束高速扫描";
            this.结束本次扫描ToolStripMenuItem.Click += new System.EventHandler(this.结束本次扫描ToolStripMenuItem_Click);
            // 
            // 报表ToolStripMenuItem
            // 
            this.报表ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.生成报表ToolStripMenuItem,
            this.导出报表ToolStripMenuItem});
            this.报表ToolStripMenuItem.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.报表ToolStripMenuItem.Name = "报表ToolStripMenuItem";
            this.报表ToolStripMenuItem.Size = new System.Drawing.Size(54, 25);
            this.报表ToolStripMenuItem.Text = "报表";
            // 
            // 生成报表ToolStripMenuItem
            // 
            this.生成报表ToolStripMenuItem.Name = "生成报表ToolStripMenuItem";
            this.生成报表ToolStripMenuItem.Size = new System.Drawing.Size(144, 26);
            this.生成报表ToolStripMenuItem.Text = "生成报表";
            this.生成报表ToolStripMenuItem.Click += new System.EventHandler(this.生成报表ToolStripMenuItem_Click);
            // 
            // 导出报表ToolStripMenuItem
            // 
            this.导出报表ToolStripMenuItem.Name = "导出报表ToolStripMenuItem";
            this.导出报表ToolStripMenuItem.Size = new System.Drawing.Size(144, 26);
            this.导出报表ToolStripMenuItem.Text = "导出报表";
            this.导出报表ToolStripMenuItem.Click += new System.EventHandler(this.导出报表ToolStripMenuItem_Click);
            // 
            // 显示ToolStripMenuItem
            // 
            this.显示ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.实时波形图ToolStripMenuItem,
            this.实时2D图ToolStripMenuItem,
            this.实时3D图ToolStripMenuItem1,
            this.实时3D图ToolStripMenuItem});
            this.显示ToolStripMenuItem.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.显示ToolStripMenuItem.Name = "显示ToolStripMenuItem";
            this.显示ToolStripMenuItem.Size = new System.Drawing.Size(54, 25);
            this.显示ToolStripMenuItem.Text = "显示";
            // 
            // 实时波形图ToolStripMenuItem
            // 
            this.实时波形图ToolStripMenuItem.Name = "实时波形图ToolStripMenuItem";
            this.实时波形图ToolStripMenuItem.Size = new System.Drawing.Size(160, 26);
            this.实时波形图ToolStripMenuItem.Text = "实时波形图";
            this.实时波形图ToolStripMenuItem.Click += new System.EventHandler(this.实时波形图ToolStripMenuItem_Click);
            // 
            // 实时2D图ToolStripMenuItem
            // 
            this.实时2D图ToolStripMenuItem.Name = "实时2D图ToolStripMenuItem";
            this.实时2D图ToolStripMenuItem.Size = new System.Drawing.Size(160, 26);
            this.实时2D图ToolStripMenuItem.Text = "实时2D图";
            this.实时2D图ToolStripMenuItem.Click += new System.EventHandler(this.实时2D图ToolStripMenuItem_Click);
            // 
            // 实时3D图ToolStripMenuItem1
            // 
            this.实时3D图ToolStripMenuItem1.Name = "实时3D图ToolStripMenuItem1";
            this.实时3D图ToolStripMenuItem1.Size = new System.Drawing.Size(160, 26);
            this.实时3D图ToolStripMenuItem1.Text = "实时3D图";
            this.实时3D图ToolStripMenuItem1.Click += new System.EventHandler(this.实时3D图ToolStripMenuItem1_Click);
            // 
            // 实时3D图ToolStripMenuItem
            // 
            this.实时3D图ToolStripMenuItem.Name = "实时3D图ToolStripMenuItem";
            this.实时3D图ToolStripMenuItem.Size = new System.Drawing.Size(160, 26);
            this.实时3D图ToolStripMenuItem.Text = "3D轮廓图";
            this.实时3D图ToolStripMenuItem.Click += new System.EventHandler(this.实3D轮廓图ToolStripMenuItem_Click);
            // 
            // 补正ToolStripMenuItem
            // 
            this.补正ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.补正ToolStripMenuItem1,
            this.还原ToolStripMenuItem});
            this.补正ToolStripMenuItem.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.补正ToolStripMenuItem.Name = "补正ToolStripMenuItem";
            this.补正ToolStripMenuItem.Size = new System.Drawing.Size(54, 25);
            this.补正ToolStripMenuItem.Text = "纠偏";
            // 
            // 补正ToolStripMenuItem1
            // 
            this.补正ToolStripMenuItem1.Name = "补正ToolStripMenuItem1";
            this.补正ToolStripMenuItem1.Size = new System.Drawing.Size(112, 26);
            this.补正ToolStripMenuItem1.Text = "补正";
            this.补正ToolStripMenuItem1.Click += new System.EventHandler(this.补正ToolStripMenuItem1_Click);
            // 
            // 还原ToolStripMenuItem
            // 
            this.还原ToolStripMenuItem.Name = "还原ToolStripMenuItem";
            this.还原ToolStripMenuItem.Size = new System.Drawing.Size(112, 26);
            this.还原ToolStripMenuItem.Text = "还原";
            this.还原ToolStripMenuItem.Click += new System.EventHandler(this.还原ToolStripMenuItem_Click);
            // 
            // 显示ToolStripMenuItem1
            // 
            this.显示ToolStripMenuItem1.Name = "显示ToolStripMenuItem1";
            this.显示ToolStripMenuItem1.Size = new System.Drawing.Size(44, 25);
            this.显示ToolStripMenuItem1.Text = "测试";
            this.显示ToolStripMenuItem1.Click += new System.EventHandler(this.显示ToolStripMenuItem1_Click);
            // 
            // _timerHighSpeed
            // 
            this._timerHighSpeed.Interval = 200;
            this._timerHighSpeed.Tick += new System.EventHandler(this._timerHighSpeed_Tick);
            // 
            // _profileFileSave
            // 
            this._profileFileSave.Filter = "Profile (*.txt)|*.txt | all files (*.*)|*.*";
            // 
            // _profileOpenFile
            // 
            this._profileOpenFile.FileName = "openFileDialog1";
            // 
            // _pnlDeviceId
            // 
            this._pnlDeviceId.BackColor = System.Drawing.Color.DarkGray;
            this._pnlDeviceId.Controls.Add(this.panel1);
            this._pnlDeviceId.Controls.Add(this.label5);
            this._pnlDeviceId.Controls.Add(this.label4);
            this._pnlDeviceId.Controls.Add(this._rdDevice0);
            this._pnlDeviceId.Controls.Add(this._lblDeviceStatus0);
            this._pnlDeviceId.Location = new System.Drawing.Point(6, 20);
            this._pnlDeviceId.Name = "_pnlDeviceId";
            this._pnlDeviceId.Size = new System.Drawing.Size(315, 80);
            this._pnlDeviceId.TabIndex = 2;
            this._pnlDeviceId.Tag = "";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this._lblReceiveProfileCount0);
            this.panel1.Location = new System.Drawing.Point(186, 5);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(126, 72);
            this.panel1.TabIndex = 59;
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Tahoma", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(3, 6);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(107, 30);
            this.label3.TabIndex = 0;
            this.label3.Text = "Number of \r\nreceived profiles";
            // 
            // _lblReceiveProfileCount0
            // 
            this._lblReceiveProfileCount0.AutoSize = true;
            this._lblReceiveProfileCount0.BackColor = System.Drawing.Color.Transparent;
            this._lblReceiveProfileCount0.Location = new System.Drawing.Point(4, 39);
            this._lblReceiveProfileCount0.Name = "_lblReceiveProfileCount0";
            this._lblReceiveProfileCount0.Size = new System.Drawing.Size(16, 16);
            this._lblReceiveProfileCount0.TabIndex = 1;
            this._lblReceiveProfileCount0.Text = "0";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Tahoma", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(38, 12);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(97, 13);
            this.label5.TabIndex = 1;
            this.label5.Text = "State (USB / IP)";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Tahoma", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(12, 12);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(20, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "ID";
            // 
            // _rdDevice0
            // 
            this._rdDevice0.AutoSize = true;
            this._rdDevice0.Checked = true;
            this._rdDevice0.Location = new System.Drawing.Point(3, 39);
            this._rdDevice0.Name = "_rdDevice0";
            this._rdDevice0.Size = new System.Drawing.Size(34, 20);
            this._rdDevice0.TabIndex = 2;
            this._rdDevice0.TabStop = true;
            this._rdDevice0.Tag = "0";
            this._rdDevice0.Text = "&0";
            this._rdDevice0.UseVisualStyleBackColor = true;
            // 
            // _lblDeviceStatus0
            // 
            this._lblDeviceStatus0.AutoSize = true;
            this._lblDeviceStatus0.Location = new System.Drawing.Point(39, 41);
            this._lblDeviceStatus0.Name = "_lblDeviceStatus0";
            this._lblDeviceStatus0.Size = new System.Drawing.Size(96, 16);
            this._lblDeviceStatus0.TabIndex = 3;
            this._lblDeviceStatus0.Text = "Unconnected";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(12, 440);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(312, 329);
            this.pictureBox1.TabIndex = 17;
            this.pictureBox1.TabStop = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this._pnlDeviceId);
            this.groupBox1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox1.Location = new System.Drawing.Point(6, 50);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(327, 106);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "连接状态";
            // 
            // tabControl_View
            // 
            this.tabControl_View.Controls.Add(this.tabPage_View);
            this.tabControl_View.Controls.Add(this.tabPage_Data);
            this.tabControl_View.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tabControl_View.Location = new System.Drawing.Point(339, 28);
            this.tabControl_View.Name = "tabControl_View";
            this.tabControl_View.SelectedIndex = 0;
            this.tabControl_View.Size = new System.Drawing.Size(1115, 769);
            this.tabControl_View.TabIndex = 18;
            this.tabControl_View.MouseClick += new System.Windows.Forms.MouseEventHandler(this.tabControl_View_MouseClick);
            // 
            // tabPage_View
            // 
            this.tabPage_View.Controls.Add(this.dgv_scanInfoDataTable);
            this.tabPage_View.Controls.Add(this.chart_Profile);
            this.tabPage_View.Controls.Add(this.chart_Vertical);
            this.tabPage_View.Controls.Add(this.wb_Profile);
            this.tabPage_View.Location = new System.Drawing.Point(4, 26);
            this.tabPage_View.Name = "tabPage_View";
            this.tabPage_View.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_View.Size = new System.Drawing.Size(1107, 739);
            this.tabPage_View.TabIndex = 0;
            this.tabPage_View.Text = "实时显示";
            this.tabPage_View.UseVisualStyleBackColor = true;
            // 
            // chart_Profile
            // 
            this.chart_Profile.BackColor = System.Drawing.Color.Transparent;
            chartArea17.Area3DStyle.Enable3D = true;
            chartArea17.Area3DStyle.WallWidth = 0;
            chartArea17.BackColor = System.Drawing.Color.Gainsboro;
            chartArea17.BackImageAlignment = System.Windows.Forms.DataVisualization.Charting.ChartImageAlignmentStyle.BottomLeft;
            chartArea17.BackImageTransparentColor = System.Drawing.Color.White;
            chartArea17.BorderColor = System.Drawing.Color.Transparent;
            chartArea17.Name = "C_SingleProfile";
            chartArea17.Position.Auto = false;
            chartArea17.Position.Height = 100F;
            chartArea17.Position.Width = 100F;
            this.chart_Profile.ChartAreas.Add(chartArea17);
            legend17.Name = "Legend1";
            this.chart_Profile.Legends.Add(legend17);
            this.chart_Profile.Location = new System.Drawing.Point(5, 5);
            this.chart_Profile.Margin = new System.Windows.Forms.Padding(2);
            this.chart_Profile.Name = "chart_Profile";
            series17.ChartArea = "C_SingleProfile";
            series17.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            series17.Legend = "Legend1";
            series17.Name = "S_SingleProfile";
            this.chart_Profile.Series.Add(series17);
            this.chart_Profile.Size = new System.Drawing.Size(728, 390);
            this.chart_Profile.TabIndex = 8;
            this.chart_Profile.Text = "chart";
            // 
            // chart_Vertical
            // 
            chartArea18.BackHatchStyle = System.Windows.Forms.DataVisualization.Charting.ChartHatchStyle.BackwardDiagonal;
            chartArea18.Name = "ChartArea1";
            chartArea18.Position.Auto = false;
            chartArea18.Position.Height = 100F;
            chartArea18.Position.Width = 100F;
            this.chart_Vertical.ChartAreas.Add(chartArea18);
            legend18.Name = "Legend1";
            this.chart_Vertical.Legends.Add(legend18);
            this.chart_Vertical.Location = new System.Drawing.Point(5, 405);
            this.chart_Vertical.Name = "chart_Vertical";
            series18.ChartArea = "ChartArea1";
            series18.Legend = "Legend1";
            series18.Name = "Series1";
            this.chart_Vertical.Series.Add(series18);
            this.chart_Vertical.Size = new System.Drawing.Size(692, 319);
            this.chart_Vertical.TabIndex = 15;
            this.chart_Vertical.Text = "chart1";
            // 
            // wb_Profile
            // 
            this.wb_Profile.Location = new System.Drawing.Point(3, 3);
            this.wb_Profile.Margin = new System.Windows.Forms.Padding(1);
            this.wb_Profile.MinimumSize = new System.Drawing.Size(20, 20);
            this.wb_Profile.Name = "wb_Profile";
            this.wb_Profile.Size = new System.Drawing.Size(1067, 735);
            this.wb_Profile.TabIndex = 17;
            this.wb_Profile.Url = new System.Uri("http://·", System.UriKind.Absolute);
            // 
            // tabPage_Data
            // 
            this.tabPage_Data.Controls.Add(this.groupBox4);
            this.tabPage_Data.Controls.Add(this.dgv_historyDataTable);
            this.tabPage_Data.Controls.Add(this.label7);
            this.tabPage_Data.Location = new System.Drawing.Point(4, 26);
            this.tabPage_Data.Name = "tabPage_Data";
            this.tabPage_Data.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_Data.Size = new System.Drawing.Size(1164, 739);
            this.tabPage_Data.TabIndex = 1;
            this.tabPage_Data.Text = "历史数据";
            this.tabPage_Data.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.btn_refresh);
            this.groupBox4.Controls.Add(this.btn_query);
            this.groupBox4.Controls.Add(this.tbx_queryKeyword);
            this.groupBox4.Controls.Add(this.dt_queryEndTime);
            this.groupBox4.Controls.Add(this.dt_queryStartTime);
            this.groupBox4.Controls.Add(this.label6);
            this.groupBox4.Controls.Add(this.label2);
            this.groupBox4.Controls.Add(this.label1);
            this.groupBox4.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox4.Location = new System.Drawing.Point(9, 61);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(952, 89);
            this.groupBox4.TabIndex = 1;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "查询选项";
            // 
            // btn_refresh
            // 
            this.btn_refresh.Location = new System.Drawing.Point(875, 28);
            this.btn_refresh.Name = "btn_refresh";
            this.btn_refresh.Size = new System.Drawing.Size(71, 23);
            this.btn_refresh.TabIndex = 4;
            this.btn_refresh.Text = "刷新";
            this.btn_refresh.UseVisualStyleBackColor = true;
            this.btn_refresh.Click += new System.EventHandler(this.btn_refresh_Click);
            // 
            // btn_query
            // 
            this.btn_query.Location = new System.Drawing.Point(744, 29);
            this.btn_query.Name = "btn_query";
            this.btn_query.Size = new System.Drawing.Size(75, 23);
            this.btn_query.TabIndex = 3;
            this.btn_query.Text = "查询";
            this.btn_query.UseVisualStyleBackColor = true;
            this.btn_query.Click += new System.EventHandler(this.btn_query_Click);
            // 
            // tbx_queryKeyword
            // 
            this.tbx_queryKeyword.Location = new System.Drawing.Point(555, 29);
            this.tbx_queryKeyword.Name = "tbx_queryKeyword";
            this.tbx_queryKeyword.Size = new System.Drawing.Size(162, 26);
            this.tbx_queryKeyword.TabIndex = 2;
            // 
            // dt_queryEndTime
            // 
            this.dt_queryEndTime.CustomFormat = "";
            this.dt_queryEndTime.Location = new System.Drawing.Point(324, 30);
            this.dt_queryEndTime.Name = "dt_queryEndTime";
            this.dt_queryEndTime.Size = new System.Drawing.Size(139, 26);
            this.dt_queryEndTime.TabIndex = 1;
            // 
            // dt_queryStartTime
            // 
            this.dt_queryStartTime.CustomFormat = "";
            this.dt_queryStartTime.Location = new System.Drawing.Point(85, 30);
            this.dt_queryStartTime.Name = "dt_queryStartTime";
            this.dt_queryStartTime.Size = new System.Drawing.Size(135, 26);
            this.dt_queryStartTime.TabIndex = 1;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(495, 34);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(72, 16);
            this.label6.TabIndex = 0;
            this.label6.Text = "关键字：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(241, 34);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(88, 16);
            this.label2.TabIndex = 0;
            this.label2.Text = "结束日期：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(2, 36);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "开始日期：";
            // 
            // dgv_historyDataTable
            // 
            this.dgv_historyDataTable.AllowUserToAddRows = false;
            this.dgv_historyDataTable.AllowUserToDeleteRows = false;
            this.dgv_historyDataTable.AllowUserToResizeColumns = false;
            this.dgv_historyDataTable.AllowUserToResizeRows = false;
            this.dgv_historyDataTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_historyDataTable.Location = new System.Drawing.Point(6, 166);
            this.dgv_historyDataTable.Name = "dgv_historyDataTable";
            this.dgv_historyDataTable.RowTemplate.Height = 23;
            this.dgv_historyDataTable.Size = new System.Drawing.Size(955, 564);
            this.dgv_historyDataTable.TabIndex = 0;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("宋体", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label7.Location = new System.Drawing.Point(372, 19);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(160, 24);
            this.label7.TabIndex = 0;
            this.label7.Text = "历史数据查询";
            // 
            // lb_status
            // 
            this.lb_status.AutoSize = true;
            this.lb_status.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lb_status.Location = new System.Drawing.Point(628, 5);
            this.lb_status.Name = "lb_status";
            this.lb_status.Size = new System.Drawing.Size(76, 16);
            this.lb_status.TabIndex = 19;
            this.lb_status.Text = "状态显示";
            // 
            // skinEngine
            // 
            this.skinEngine.@__DrawButtonFocusRectangle = true;
            this.skinEngine.DisabledButtonTextColor = System.Drawing.Color.Gray;
            this.skinEngine.DisabledMenuFontColor = System.Drawing.SystemColors.GrayText;
            this.skinEngine.InactiveCaptionColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.skinEngine.SerialNumber = "";
            this.skinEngine.SkinFile = null;
            // 
            // cb_selectSkin
            // 
            this.cb_selectSkin.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cb_selectSkin.FormattingEnabled = true;
            this.cb_selectSkin.Location = new System.Drawing.Point(1160, 6);
            this.cb_selectSkin.Name = "cb_selectSkin";
            this.cb_selectSkin.Size = new System.Drawing.Size(147, 24);
            this.cb_selectSkin.TabIndex = 20;
            this.cb_selectSkin.Text = "主题";
            this.cb_selectSkin.SelectedIndexChanged += new System.EventHandler(this.cb_selectSkin_SelectedIndexChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtBox_Log);
            this.groupBox2.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox2.Location = new System.Drawing.Point(6, 172);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(324, 262);
            this.groupBox2.TabIndex = 21;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "日志";
            // 
            // txtBox_Log
            // 
            this.txtBox_Log.Location = new System.Drawing.Point(6, 20);
            this.txtBox_Log.Multiline = true;
            this.txtBox_Log.Name = "txtBox_Log";
            this.txtBox_Log.Size = new System.Drawing.Size(312, 236);
            this.txtBox_Log.TabIndex = 0;
            // 
            // dgv_scanInfoDataTable
            // 
            this.dgv_scanInfoDataTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_scanInfoDataTable.Location = new System.Drawing.Point(719, 16);
            this.dgv_scanInfoDataTable.Name = "dgv_scanInfoDataTable";
            this.dgv_scanInfoDataTable.RowHeadersVisible = false;
            this.dgv_scanInfoDataTable.RowTemplate.Height = 23;
            this.dgv_scanInfoDataTable.Size = new System.Drawing.Size(365, 708);
            this.dgv_scanInfoDataTable.TabIndex = 2;
            // 
            // MainForms
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1462, 796);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.cb_selectSkin);
            this.Controls.Add(this.lb_status);
            this.Controls.Add(this.tabControl_View);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.pictureBox1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForms";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PipeModel";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this._pnlDeviceId.ResumeLayout(false);
            this._pnlDeviceId.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.tabControl_View.ResumeLayout(false);
            this.tabPage_View.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chart_Profile)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart_Vertical)).EndInit();
            this.tabPage_Data.ResumeLayout(false);
            this.tabPage_Data.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_historyDataTable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_scanInfoDataTable)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        

        #endregion
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 文件ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 新建ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 打开ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 保存ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 退出ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 操作ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem usb连接ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 断开连接ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 扫描ToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem 开始高速扫描ToolStripMenuItem;
        private System.Windows.Forms.Timer _timerHighSpeed;
        private System.Windows.Forms.SaveFileDialog _profileFileSave;
        private System.Windows.Forms.ToolStripMenuItem 显示ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 实时波形图ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 实时3D图ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 报表ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 生成报表ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 导出报表ToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog _profileOpenFile;
        private System.Windows.Forms.Panel _pnlDeviceId;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label _lblReceiveProfileCount0;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.RadioButton _rdDevice0;
        private System.Windows.Forms.Label _lblDeviceStatus0;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ToolStripMenuItem 实时2D图ToolStripMenuItem;
        private System.Windows.Forms.TabControl tabControl_View;
        private System.Windows.Forms.TabPage tabPage_View;
        private System.Windows.Forms.WebBrowser wb_Profile;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart_Profile;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart_Vertical;
        private System.Windows.Forms.TabPage tabPage_Data;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button btn_query;
        private System.Windows.Forms.TextBox tbx_queryKeyword;
        private System.Windows.Forms.DateTimePicker dt_queryEndTime;
        private System.Windows.Forms.DateTimePicker dt_queryStartTime;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dgv_historyDataTable;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ToolStripMenuItem 结束本次扫描ToolStripMenuItem;
        private System.Windows.Forms.Label lb_status;
        private System.Windows.Forms.BindingSource bindingSource1;
        private System.Windows.Forms.ToolStripMenuItem 调试ToolStripMenuItem;
        private System.Windows.Forms.Button btn_refresh;
        private System.Windows.Forms.ToolStripMenuItem 补正ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 补正ToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem 还原ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 实时3D图ToolStripMenuItem1;
        private Sunisoft.IrisSkin.SkinEngine skinEngine;
        private System.Windows.Forms.ComboBox cb_selectSkin;
        private System.Windows.Forms.ToolStripMenuItem 显示ToolStripMenuItem1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox txtBox_Log;
        private System.Windows.Forms.DataGridView dgv_scanInfoDataTable;
    }
}

