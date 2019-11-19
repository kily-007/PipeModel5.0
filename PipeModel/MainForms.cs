using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PipeModel;
using PipeModel.Datas;
using System.Collections;
using System.IO;
using System.Runtime.InteropServices;
using PipeModel.Properties;
using PipeModel.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using PipeModel.DataAnalysis;

namespace PipeModel
{
    //设置Com对外可访问
    [System.Runtime.InteropServices.ComVisible(true)]
    public partial class MainForms : Form
    {
        
        #region Enum

        /// <summary>
        /// Send command definition
        /// </summary>
        /// <remark>Defined for separate return code distinction</remark>
        public enum SendCommand
        {
            /// <summary>None</summary>
            None,
            /// <summary>Restart</summary>
            RebootController,
            /// <summary>Trigger</summary>
            Trigger,
            /// <summary>Start measurement</summary>
            StartMeasure,
            /// <summary>Stop measurement</summary>
            StopMeasure,
            /// <summary>Auto zero</summary>
            AutoZero,
            /// <summary>Timing</summary>
            Timing,
            /// <summary>Reset</summary>
            Reset,
            /// <summary>Program switch</summary>
            ChangeActiveProgram,
            /// <summary>Get measurement results</summary>
            GetMeasurementValue,

            /// <summary>Get profiles</summary>
            GetProfile,
            /// <summary>Get batch profiles (operation mode "high-speed (profile only)")</summary>
            GetBatchProfile,
            /// <summary>Get profiles (operation mode "advanced (with OUT measurement)")</summary>
            GetProfileAdvance,
            /// <summary>Get batch profiles (operation mode "advanced (with OUT measurement)").</summary>
            GetBatchProfileAdvance,

            /// <summary>Start storage</summary>
            StartStorage,
            /// <summary>Stop storage</summary>
            StopStorage,
            /// <summary>Get storage status</summary>
            GetStorageStatus,
            /// <summary>Manual storage request</summary>
            RequestStorage,
            /// <summary>Get storage data</summary>
            GetStorageData,
            /// <summary>Get profile storage data</summary>
            GetStorageProfile,
            /// <summary>Get batch profile storage data.</summary>
            GetStorageBatchProfile,

            /// <summary>Initialize USB high-speed data communication</summary>
            HighSpeedDataUsbCommunicationInitalize,
            /// <summary>Initialize Ethernet high-speed data communication</summary>
            HighSpeedDataEthernetCommunicationInitalize,
            /// <summary>Request preparation before starting high-speed data communication</summary>
            PreStartHighSpeedDataCommunication,
            /// <summary>Start high-speed data communication</summary>
            StartHighSpeedDataCommunication,
            
        }


        public enum View
        {
            /// <summary>None</summary>
            None,
            /// <summary>波形图  </summary>
            HightSpeed,
            /// <summary>2图 </summary>
            HightSpeed2D,
            /// <summary>3图 </summary>
            HightSpeed3D,

        }

        #endregion

        #region Field

        /// <summary>Ethernet settings structure </summary>
        private LJV7IF_ETHERNET_CONFIG _ethernetConfig;
        /// <summary>Measurement data list</summary>
        private List<MeasureData> _measureDatas;
        /// <summary>Current device ID</summary>
        private int _currentDeviceId;
        /// <summary>Send command</summary>
        private SendCommand _sendCommand;
        /// <summary>Scan command</summary>
        //private ViewProfile _viewCommand;
        /// <summary>Callback function used during high-speed communication</summary>
        private HighSpeedDataCallBack _callback;
        /// <summary>Callback function used during high-speed communication (count only)</summary>
        private HighSpeedDataCallBack _callbackOnlyCount;
        /// The following are maintained in arrays to support multiple controllers.
        /// <summary>Array of profile information structures</summary>
        private LJV7IF_PROFILE_INFO[] _profileInfo;
        /// <summary>Array of controller information</summary>
        private DeviceData[] _deviceData;
        /// <summary>Array of labels that indicate the controller status</summary>
        private Label[] _deviceStatusLabels;
        /// <summary>判别高速扫描时是否存储数据</summary>
        private bool _heightSpeedSaveIsOpen = false;
        /// <summary>判别高速扫描时是否存储数据</summary>
        private View drawShap= View.HightSpeed;
        /// <summary> VerticalProfile轴辅助队列 </summary>
        private Queue<int> _queueVertical = new Queue<int>(Define.VERTICAL_MAX_X);
        /// <summary> 2DProfile轴辅助队列 </summary>
        public static Queue<int[]> _queue2D = new Queue<int[]>(Define.PROFILE2D_MAX_X);
        /// <summary> 实时3D扫描轴辅助队列 </summary>
        private Queue<int[]> _queue3DCurrentData = new Queue<int[]>(Define.QUEUE3D_LENGTH);
        /// <summary> 扫描次数 </summary>
        public static int _countScan = 0;
        /// <summary> 记录衔接处扫描次数 </summary>
        public static int _countF = 0;
        /// <summary> 记录单根灌道扫描次数 </summary>
        public static int _countG = 0;
        /// <summary> 记录灌道根数 </summary>
        public static int _countSing = 0;
        /// <summary> 保存地址 </summary>
        public static string _defaultSavePath = @"E:\pipeModel";
        /// <summary> 纠偏补正参数 </summary>
        private double k = 0;
        // <summary> 是否计算过纠偏值k </summary>
        private static bool _correctionEd = false;
        /// <summary> 判断是否点击过停止扫描按钮 </summary>
        private bool _updateChartData = true;
        /// <summary>保存开始存储时间戳 </summary>
        public static string _startBinDataTime = "";
        /// <summary>保存结束存储时间戳 </summary>
        public static string _endtBinDataTime = "";
        /// <summary>保存开始存储时间戳-文件名 </summary>
        public static string _binFileDataTime = "";
        /// <summary>历史数据查询表头 </summary>
        public static string[] _historyDataTableHead = { "序号","时间", "报表文件", "文件大小","原始数据","操作"};
        /// <summary>实时扫描信息表头 </summary>
        public static string[] _scanInfoDataTableHead = { "类型","横向错位x(mm)", "纵向错位y(mm)","缝隙长度z(mm)"};
        /// <summary>历史数据查询表表按钮名/txt </summary>
        private static readonly string[] historyTableBtnText = new string[4] { "导出", "删除", "生成", "原始数据" };
        private static readonly string[] historyTableBtnName = new string[4] { "btnExportReport", "btnDelete", "btnMakeReport", "btnDataBack" };
        /// <summary>扫描批处理行数</summary>
        private const int _frequencyCount= 200;
        /// <summary>历史数据表中的四个按钮,每页最多放置27行数据</summary>
        private Button[][] btn = new Button[27][];
        /// <summary>历史上数据表中每次刷新数据，有效数据行数</summary>
        private int effectBtn = 0;//记录有效button数

        /// <summary>
        ///记录网线连接高速端口
        /// </summary>
        private string HighSpeedPort;
        /// <summary>
        /// SendPos：发送起始位置
        /// 0：从上次发送完毕的位置开始 （首次执行就从最早的数据开始）
        /// 1：从最早的数据开始
        /// 2：从下一数据开
        /// </summary>
        private string StartProfileNo="2";

        #endregion


        #region Field
        /// <summary>
        /// Specify the position, etc., of the profiles to get.
        /// </summary>
        private LJV7IF_GET_PROFILE_REQ _req;
        #endregion



        #region Property
        /// <summary>
        /// Specify the position, etc., of the profiles to get.
        /// </summary>
        public LJV7IF_GET_PROFILE_REQ Req
        {
            get { return _req; }
        }

        #endregion


        int index = 0;
        public MainForms()
        {
            //WindowState = FormWindowState.Maximized;   //最大化
            InitializeComponent();
            
            LoadSkin("office2007.ssk");//初始化皮肤
            InitChart_Profile();//初始化轮廓实时图
            InitChart_Vertical();//初始化Z方向实时图
            InitChart_LineMisa();//初始化全局磨损曲线
            InitChart_LeftRight();//初始化全局leftright曲线

            chart_Profile.Visible = true;
            chart_Vertical.Visible = true;
            wb_Profile.Visible = false;

            //初始化vertical队列
            for (int i = 0; i < Define.VERTICAL_MAX_X; i++)
            {
                _queueVertical.Enqueue(0);
            }

            for (int i = 0; i < Define.PROFILE2D_MAX_X; i++)
            {
                _queue2D.Enqueue(new int[2] { 0, 0 });
            }
            for (int i = 0; i < Define.QUEUE3D_LENGTH; i++)
            {
                _queue3DCurrentData.Enqueue(new int[807]);
            }
            

            //添加历史数据表头
            for (int i = 0; i < _historyDataTableHead.Length-1; i++)
            {
                DataGridViewTextBoxColumn txtClum = new DataGridViewTextBoxColumn();
                txtClum.HeaderText = _historyDataTableHead[i];
                dgv_historyDataTable.Columns.Add(txtClum);
            }

            //添加实时扫描信息表头
            for (int i = 0; i < _scanInfoDataTableHead.Length; i++)
            {
                DataGridViewTextBoxColumn txtClum = new DataGridViewTextBoxColumn();
                txtClum.HeaderText = _scanInfoDataTableHead[i];
                dgv_scanInfoDataTable.Columns.Add(txtClum);
                txtClum.Width = 92;
                dgv_scanInfoDataTable.Font = new Font("宋体",11);
            }

            index = dgv_scanInfoDataTable.Rows.Add();
            dgv_scanInfoDataTable.Rows[index].Cells[0].Value = _countSing+"灌道▅";
            dgv_scanInfoDataTable.Rows[index].Cells[1].Value = 0;
            dgv_scanInfoDataTable.Rows[index].Cells[2].Value = 0;
            dgv_scanInfoDataTable.Rows[index].Cells[3].Value = 0;
            dgv_scanInfoDataTable.Rows[index].Cells[0].Style.ForeColor = Color.Blue;
            dgv_scanInfoDataTable.Rows[index].Cells[1].Style.ForeColor = Color.Blue;
            dgv_scanInfoDataTable.Rows[index].Cells[2].Style.ForeColor = Color.Blue;
            dgv_scanInfoDataTable.Rows[index].Cells[3].Style.ForeColor = Color.Blue;


            //初始化实时扫描信息队列
            for (int i = 0; i < 2; i++)
            {
                int[] temp = new int[3] { 0,0,0};
                misaQueue.Enqueue(temp);
            }

            dgv_historyDataTable.Columns[0].Width = 70;
            dgv_historyDataTable.Columns[1].Width = 190;
            dgv_historyDataTable.Columns[2].Width = 250;
            dgv_historyDataTable.Columns[3].Width = 100;
            dgv_historyDataTable.Columns[4].Width = 200;
            dgv_historyDataTable.Columns.Add("ColBtnEdit", "操作");
            dgv_historyDataTable.Columns[5].Width = 310;
            dgv_historyDataTable.Columns[5].Resizable = DataGridViewTriState.False;

            //SetStyle(ControlStyles.UserPaint, true);
            //SetStyle(ControlStyles.AllPaintingInWmPaint, true); // 禁止擦除背景.
            //SetStyle(ControlStyles.DoubleBuffer, true); // 双缓冲

            _req = new LJV7IF_GET_PROFILE_REQ();

            _req.byTargetBank = Convert.ToByte("00", 16);
            _req.byPosMode = Convert.ToByte("00", 16);
            _req.dwGetProfNo = Convert.ToUInt16("0");
            _req.byGetProfCnt = Convert.ToByte("1");
            _req.byErase = Convert.ToByte("0");
            

            // Field initialization
            _currentDeviceId = 0;
            _sendCommand = SendCommand.None;
            _deviceData = new DeviceData[NativeMethods.DeviceCount];
            _measureDatas = new List<MeasureData>();
            _callback = new HighSpeedDataCallBack(ReceiveHighSpeedData);
            _callbackOnlyCount = new HighSpeedDataCallBack(CountProfileReceive);
            _deviceStatusLabels = new Label[] { _lblDeviceStatus0 };

            for (int i = 0; i < NativeMethods.DeviceCount; i++)
            {
                _deviceData[i] = new DeviceData();
                _deviceStatusLabels[i].Text = _deviceData[i].GetStatusString();
            }
            _profileInfo = new LJV7IF_PROFILE_INFO[NativeMethods.DeviceCount];

            creatFile();

            //OpenForm(new SingleProfileForm());
            //修改注册表信息来兼容WebBriwser当前程序

            //int IEVersion=WebBrowserIE.getIEVersionEmulation();
            // WebBrowserIE.setIEVersionEmulation(IEVersion);


            // For use in profile export control
            //_txtboxProfileFilePath.Text = Directory.GetCurrentDirectory() + @"\" + Define.DEFAULT_PROFILE_FILE_NAME;
            //_txtboxProfileFilePath.SelectionStart = _txtboxProfileFilePath.Text.Length;
        }


        /// <summary>
        /// 加载皮肤
        /// </summary>
        private void LoadSkin(string defaultSkin)
        {
            string skinPath = Application.StartupPath + "\\Skins\\";
            string[] files = Directory.GetFiles(skinPath, "*.ssk", SearchOption.AllDirectories);//获取目录文件名称集合
            foreach (string file in files)
            {
                FileInfo fi = new FileInfo(file);
                cb_selectSkin.Items.Add(fi.Name);
            }
            //加载默认皮肤
            skinEngine.SkinFile = skinPath + defaultSkin;
        }


        /// <summary>
        /// 初始化历史数据列表
        /// </summary>
        private void InitHistoryDataTable(string[][] data)
        {
            //清空数据表
            dgv_historyDataTable.Rows.Clear();
            effectBtn = data.Length;//有效button
            for (int i = 0; i < data.Length; i++)
            {
                //添加数据
                int index = dgv_historyDataTable.Rows.Add();
                dgv_historyDataTable.Rows[index].Cells[0].Value = i;
                for (int j = 0; j < data[0].Length; j++)
                {
                    dgv_historyDataTable.Rows[index].Cells[j + 1].Value = data[i][j];

                }
                //添加按钮
                btn[i] = new Button[historyTableBtnText.Length];
                for (int j = 0; j < historyTableBtnText.Length; j++)
                {
                    btn[i][j] = new Button();
                    btn[i][j].Text = historyTableBtnText[j];
                    btn[i][j].Name = historyTableBtnName[j] + "," + i + "," + j;
                    btn[i][j].Click += btn_Click;
                    dgv_historyDataTable.Controls.Add(btn[i][j]);
                    Rectangle rectangle = dgv_historyDataTable.GetCellDisplayRectangle(data[i].Length + 1, i, true);//获取当前单元格上的矩形区域
                    btn[i][j].Size = btn[i][j].Size = btn[i][j].Size = new Size(rectangle.Width / 4 + 1, rectangle.Height);
                    btn[i][j].Location = new Point(rectangle.Left + j * (rectangle.Width / 4), rectangle.Top);
                }
            }
        }




        /// <summary>
        /// 响应历史数据查询表中按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Click(object sender, EventArgs e)
        {
            if (sender is Button)
            {
                Button btnTemp = (Button)sender;
                string[] info = btnTemp.Name.Split(',');


                if (info[0].Equals(historyTableBtnName[0]))//导出按钮
                {

                    if (dgv_historyDataTable.Rows[int.Parse(info[1])].Cells[2].Value.Equals("-1"))
                    {
                        MessageBox.Show("暂无报表，请生成该次扫描报表。");
                        return;
                    }
                    SaveFileDialog sfd = new SaveFileDialog();
                    sfd.Filter = "Word Document(*.docx)|*.docx";
                    sfd.DefaultExt = "Word Document(*.docx)|*.docx";
                    sfd.FileName = _defaultSavePath + dgv_historyDataTable.Rows[int.Parse(info[1])].Cells[2].Value;
                    sfd.InitialDirectory = _defaultSavePath;
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            string sql = "select h_reportFilePath from historyData where h_id in (select h_id from historyData where h_dataFileName='" + dgv_historyDataTable.Rows[int.Parse(info[1])].Cells[4].Value + "')";
                            string[][] reportPath = MysqlConnection.executeQuery_data(sql);
                            File.Copy(reportPath[0][0], sfd.FileName, true);
                        }
                        catch (Exception ex)
                        {
                            throw new Exception(ex.Message);
                        }
                    }

                }
                else if (info[0].Equals(historyTableBtnName[1]))//删除按钮
                {
                    DialogResult dr = MessageBox.Show("将会删除原始数据与对应报表，确认删除？", "删除", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                    if (dr == DialogResult.OK)
                    {
                        MysqlConnection.executeDelete("delete from historyData where h_dataFileName='" + dgv_historyDataTable.Rows[int.Parse(info[1])].Cells[4].Value + "'");
                        //将所有btn设为不可见
                        for (int i = 0; i < effectBtn; i++)
                        {
                            for (int j = 0; j < historyTableBtnText.Length; j++)
                            {
                                btn[i][j].Tag = skinEngine.DisableTag;
                                btn[i][j].Visible = false;
                            }
                            
                        }
                        string sqlStr = "select h_startTime,h_reportFileName,h_reportFileSize,h_dataFileName from historydata order by h_id DESC limit 20";
                        string[][] data = MysqlConnection.executeQuery_data(sqlStr);
                        InitHistoryDataTable(data);
                    }
                    else{}
                        
                }
                else if (info[0].Equals(historyTableBtnName[2]))//生成按钮
                {
                    //导出left，right，middleLeft，middle，middleRight，misa

                    if (!dgv_historyDataTable.Rows[int.Parse(info[1])].Cells[2].Value.Equals("-1"))
                    {
                        //读原始数据，计算生成对应报表
                        //获取原始数据对应存储路径
                        string dataFilePath = MysqlConnection.executeQuery_data("select h_dataFilePath from historyData where h_dataFileName='" + dgv_historyDataTable.Rows[int.Parse(info[1])].Cells[4].Value + "'")[0][0];
                        //读bin文件生成报表
                        FileStream fs = new FileStream(dataFilePath, FileMode.Open, FileAccess.Read);
                        BinaryReader br = new BinaryReader(fs);
                        br.BaseStream.Seek(0, SeekOrigin.Begin);
                        MysqlConnection.truncateTableData("misaligned");//清除数据库表
                        while (br.BaseStream.Position + 4 * 807 * _frequencyCount < br.BaseStream.Length)
                        {
                            byte[] bs = br.ReadBytes(4 * 807 * _frequencyCount);
                            List<int[]> data = byteArrayToIntArrayList(bs);
                            MisalignedScan.MisalignedAnalizeData(data, 0);//数据分析入库,参数0表示不补正
                        }
                        ReportViewer report = new ReportViewer();
                        report.makeReport();//制作报表
                        report.writeWord(_defaultSavePath + "data\\report\\测量报表" + dgv_historyDataTable.Rows[int.Parse(info[1])].Cells[4].Value.ToString().Split('.')[0] + ".docx");//写入word文档
                        //刷新表格
                        string[][] strTable = MysqlConnection.executeQuery_data("select h_reportFileName,h_reportFileSize from historyData where h_dataFileName='" + dgv_historyDataTable.Rows[int.Parse(info[1])].Cells[4].Value + "'");
                        dgv_historyDataTable.Rows[int.Parse(info[1])].Cells[2].Value = strTable[0][0];
                        dgv_historyDataTable.Rows[int.Parse(info[1])].Cells[3].Value = strTable[0][1];

                    }
                    else
                    {
                        MessageBox.Show("已生成过报表。");
                    }
                }
                else if (info[0].Equals(historyTableBtnName[3]))//导出原始数据按钮
                {
                    try
                    {
                        //导出原始数据
                        //获取原始数据对应存储路径
                        string dataFilePath = MysqlConnection.executeQuery_data("select h_dataFilePath from historyData where h_dataFileName='" + dgv_historyDataTable.Rows[int.Parse(info[1])].Cells[4].Value + "'")[0][0];
                        //读bin文件生成报表
                        FileStream fs = new FileStream(dataFilePath, FileMode.Open, FileAccess.Read);
                        BinaryReader br = new BinaryReader(fs);
                        br.BaseStream.Seek(0, SeekOrigin.Begin);
                        string txtDataFilePath = _defaultSavePath + "data\\source\\" + dgv_historyDataTable.Rows[int.Parse(info[1])].Cells[4].Value.ToString().Split('.')[0] + ".txt";
                        while (br.BaseStream.Position + 4 * 807 * _frequencyCount < br.BaseStream.Length)
                        {
                            byte[] bs = br.ReadBytes(4 * 807 * _frequencyCount);
                            List<int[]> data = byteArrayToIntArrayList(bs);
                            DataExporter.ExportHeightSpeedTxtData(data, txtDataFilePath);
                        }
                        SaveFileDialog sfd = new SaveFileDialog();
                        sfd.Filter = "Word Document(*.txt)|*.txt";
                        sfd.DefaultExt = "Word Document(*.txt)|*.txt";
                        sfd.FileName = dgv_historyDataTable.Rows[int.Parse(info[1])].Cells[4].Value.ToString().Split('.')[0] + ".txt";
                        sfd.InitialDirectory = _defaultSavePath;
                        if (sfd.ShowDialog() == DialogResult.OK)
                        {
                            File.Copy(txtDataFilePath, sfd.FileName, true);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.ToString());
                    }
                   
                }

            }
        }
        


        //解析原始数据
        private List<int[]> byteArrayToIntArrayList(byte[] bs)
        {
            List<int[]> list = new List<int[]>();
            for (int i = 0; i < bs.Length/(807*4); i++)
            {
                int[] profileData = new int[807];
                for (int j = 0; j < 807; j++)
                {
                    byte[] tempBs = new byte[4];
                    Array.Copy(bs,(i*807+ j)*4,tempBs,0,4);
                    profileData[j] = BitConverter.ToInt32(tempBs, 0);
                }
                list.Add(profileData);
            }
            return list;
        }

        /// <summary>
        /// 创建存储文件夹
        /// </summary>
        private void creatFile()
        {
            string[] strPath = Environment.CurrentDirectory.Split('\\');
            string str = "";
            for (int i = 0; i < strPath.Length - 4; i++)
            {
                str += strPath[i] + "\\";
            }
            if (!Directory.Exists(str)|| !Directory.Exists(str+"data"))
            {
                Directory.CreateDirectory(str);
                Directory.CreateDirectory(str + "data");
            }
            _defaultSavePath = str;
        }
        

        /// <summary>
        /// 2D表与扫描线表
        /// </summary>
        private void InitChart_Profile()
        {
            chart_Profile.ChartAreas.Clear();
            ChartArea chartArea_SingleProfile = new ChartArea("C_SingleProfile");
            chartArea_SingleProfile.Position.Height = Define.CSPH;
            chartArea_SingleProfile.Position.Width = Define.CSPW;
            chartArea_SingleProfile.Position.X = Define.CSPX;
            chartArea_SingleProfile.Position.Y = Define.CSPY;
            chart_Profile.ChartAreas.Add(chartArea_SingleProfile);

            chart_Profile.ChartAreas[0].AxisX.Title = "待测物宽度(单位:mm)";
            chart_Profile.ChartAreas[0].AxisX.TitleAlignment = StringAlignment.Far;
            //chart_Profile.ChartAreas[0].AxisY.Title = "探头与待测物距离单位(nmm)";

            chart_Profile.Series.Clear();
            Series series_SingleProfile = new Series("S_SingleProfile");
            series_SingleProfile.ChartArea = "C_SingleProfile";
            chart_Profile.Series.Add(series_SingleProfile);

            chart_Profile.Series[0].Color = Color.Blue;
            chart_Profile.Series[0].ChartType = SeriesChartType.FastLine;
            //设置最大最小值
            chart_Profile.ChartAreas[0].AxisY.Minimum = Define.PROFILE_MIN_Y;
            chart_Profile.ChartAreas[0].AxisY.Maximum = Define.PROFILE_MAX_Y;
            //设置刻度
            chart_Profile.ChartAreas[0].AxisY.Interval = Define.PROFILE_Interval_Y;
            chart_Profile.ChartAreas[0].AxisX.Interval = Define.PROFILE_Interval_X;

            chart_Profile.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.Silver;
            chart_Profile.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.Silver;

            chart_Profile.Titles.Clear();
            chart_Profile.Titles.Add("探头距离\n(单位:mm)");
            chart_Profile.Titles[0].Position.X = Define.CTPX;
            chart_Profile.Titles[0].Position.Y = Define.CTPY;

        }


        /// <summary>
        /// 纵表
        /// </summary>
        private void InitChart_Vertical()
        {
            chart_Vertical.ChartAreas.Clear();
            ChartArea chartArea_VerticalProfile = new ChartArea("C_VerticalProfile");
            chartArea_VerticalProfile.Position.Height = Define.CSPH;
            chartArea_VerticalProfile.Position.Width = Define.CSPW;
            chartArea_VerticalProfile.Position.X = Define.CSPX;
            chartArea_VerticalProfile.Position.Y = Define.CSPY;
            chart_Vertical.ChartAreas.Add(chartArea_VerticalProfile);

            chart_Vertical.Series.Clear();
            Series series_VerticalProfile = new Series("S_VerticalProfile");
            series_VerticalProfile.ChartArea = "C_VerticalProfile";
            chart_Vertical.Series.Add(series_VerticalProfile);

            chart_Vertical.ChartAreas[0].AxisX.Title = "探头移动距离(单位：m)";
            chart_Vertical.ChartAreas[0].AxisX.TitleAlignment = StringAlignment.Far;
            chart_Vertical.ChartAreas[0].AxisX.Interval = 0.1;
            chart_Vertical.ChartAreas[0].AxisY.Minimum = Define.VERTICAL_MIN_Y;
            chart_Vertical.ChartAreas[0].AxisY.Maximum = Define.VERTICAL_MAX_Y;
            chart_Vertical.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.Silver;
            chart_Vertical.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.Silver;
            chart_Vertical.ChartAreas[0].AxisY.Interval = Define.VERTICAL_Interval_Y;
            

            chart_Vertical.Titles.Clear();
            chart_Vertical.Titles.Add("探头距离\n(单位:mm)");
            chart_Vertical.Titles[0].Position.X = Define.CTPX;
            chart_Vertical.Titles[0].Position.Y = Define.CTPY;

            chart_Vertical.Series[0].Color = Color.Red;
            chart_Vertical.Series[0].ChartType = SeriesChartType.FastLine;

        }


        /// <summary>
        /// 初始化全局磨损曲线
        /// </summary>
        private void InitChart_LineMisa()
        {
            chart_Misa.ChartAreas.Clear();
            ChartArea chartArea = new ChartArea("C_MisaProfile");
            chartArea.Position.Height = Define.CSPH;
            chartArea.Position.Width = Define.CSPW;
            chartArea.Position.X = Define.CSPX;
            chartArea.Position.Y = Define.CSPY;
            chart_Misa.ChartAreas.Add(chartArea);
            chart_Misa.ChartAreas[0].AxisX.Title = "探头移动距离(单位：m)";
            chart_Misa.ChartAreas[0].AxisX.TitleAlignment = StringAlignment.Far;
            chart_Misa.ChartAreas[0].AxisY.Minimum = Define.MISA_MIN_Y;
            chart_Misa.ChartAreas[0].AxisY.Maximum = Define.MISA_MAX_Y;
            chart_Misa.ChartAreas[0].AxisY.Interval = Define.MISA_Interval_Y;
            //chart_Misa.ChartAreas[0].AxisX.Minimum = Define.MISA_MIN_X;
            //chart_Misa.ChartAreas[0].AxisX.Maximum = Define.MISA_MAX_X;

            chart_Misa.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.Silver;
            chart_Misa.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.Silver;

            chart_Misa.Titles.Clear();
            chart_Misa.Titles.Add("磨损值\n(单位:mm)");
            //chart_misa.Titles[0].Text = "探头A";
            chart_Misa.Titles[0].Position.X = Define.CTPX;
            chart_Misa.Titles[0].Position.Y = Define.CTPY;
            chart_Misa.Titles[0].ForeColor = Color.Black;
            chart_Misa.Titles[0].Font = new Font("Microsoft Sans Serif", 10F);

            chart_Misa.Series.Clear();
            Series series_LineMisa = new Series("S_MisaProfile");
            series_LineMisa.ChartArea = "C_MisaProfile";
            chart_Misa.Series.Add(series_LineMisa);
            chart_Misa.Series[0].Color = Color.Red;
            chart_Misa.Series[0].ChartType = SeriesChartType.FastLine;

        }


        /// <summary>
        /// 初始化全局leftRight曲线
        /// </summary>
        private void InitChart_LeftRight()
        {
            chart_LeftRight.ChartAreas.Clear();
            ChartArea chartArea_Left = new ChartArea("C_LeftMiddleRightProfile");

            chart_LeftRight.ChartAreas.Add(chartArea_Left);

            //绘图区域在chart中的位置
            chart_LeftRight.ChartAreas[0].Position.X = 0;
            chart_LeftRight.ChartAreas[0].Position.Y = 13;
            chart_LeftRight.ChartAreas[0].Position.Width = 90;
            chart_LeftRight.ChartAreas[0].Position.Height = 85;

            chart_LeftRight.ChartAreas[0].AxisX.Title = "探头移动距离(单位：m)";
            chart_LeftRight.ChartAreas[0].AxisX.TitleAlignment = StringAlignment.Far;
            chart_LeftRight.ChartAreas[0].AxisY.Minimum = Define.LMR_MIN_Y;
            chart_LeftRight.ChartAreas[0].AxisY.Maximum = Define.LMR_MAX_Y;
            chart_LeftRight.ChartAreas[0].AxisY.Interval = Define.LMR_Interval_Y;
            //chart_LeftRight.ChartAreas[0].AxisX.Minimum = Define.LMR_MIN_X;
            //chart_LeftRight.ChartAreas[0].AxisX.Maximum = Define.LMR_MAX_X;

            chart_LeftRight.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.Silver;
            chart_LeftRight.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.Silver;

            chart_LeftRight.Titles.Clear();
            chart_LeftRight.Titles.Add("待测物宽度\n(单位:mm)");
            //chart_LeftRight.Titles[0].Text = "探头A";
            chart_LeftRight.Titles[0].Position.X = Define.CTPX;
            chart_LeftRight.Titles[0].Position.Y = Define.CTPY;
            chart_LeftRight.Titles[0].ForeColor = Color.Black;
            chart_LeftRight.Titles[0].Font = new Font("Microsoft Sans Serif", 10F);

            chart_LeftRight.Series.Clear();
            Series series_Left = new Series("S_LeftProfile");
            Series series_Middle= new Series("S_MiddleProfile");
            Series series_Right = new Series("S_RightProfile");
            series_Left.ChartArea = "C_LeftMiddleRightProfile";
            series_Middle.ChartArea = "C_LeftMiddleRightProfile";
            series_Right.ChartArea = "C_LeftMiddleRightProfile";

            chart_LeftRight.Series.Add(series_Left);
            chart_LeftRight.Series.Add(series_Middle);
            chart_LeftRight.Series.Add(series_Right);


            chart_LeftRight.Series[0].Color = Color.Blue;
            chart_LeftRight.Series[1].Color = Color.Red;
            chart_LeftRight.Series[2].Color = Color.Blue;
            chart_LeftRight.Series[0].ChartType = SeriesChartType.FastLine;
            chart_LeftRight.Series[1].ChartType = SeriesChartType.FastLine;
            chart_LeftRight.Series[2].ChartType = SeriesChartType.FastLine;



        }



        /// <summary>
        /// Method that is called from the DLL as a callback function
        /// </summary>
        /// <param name="buffer">Leading pointer of the received data</param>
        /// <param name="size">Size in units of bytes of one profile</param>
        /// <param name="count">Number of profiles</param>
        /// <param name="notify">Completion flag</param>
        /// <param name="user">Thread ID (value passed during initialization)</param>
        public static void ReceiveHighSpeedData(IntPtr buffer, uint size, uint count, uint notify, uint user)
        {
            // @Point
            // Take care to only implement storing profile data in a thread save buffer in the callback function.
            // As the thread used to call the callback function is the same as the thread used to receive data,
            // the processing time of the callback function affects the speed at which data is received,
            // and may stop communication from being performed properly in some environments.

            uint profileSize = (uint)(size / Marshal.SizeOf(typeof(int)));
            List<int[]> receiveBuffer = new List<int[]>();
            int[] bufferArray = new int[profileSize * count];
            Marshal.Copy(buffer, bufferArray, 0, (int)(profileSize * count));

            // Profile data retention
            for (int i = 0; i < count; i++)
            {
                int[] oneProfile = new int[profileSize];
                Array.Copy(bufferArray, i * profileSize, oneProfile, 0, profileSize);
                receiveBuffer.Add(oneProfile);
            }

            ThreadSafeBuffer.Add((int)user, receiveBuffer, notify);
        }

        /// <summary>
        /// Method that is called from the DLL as a callback function
        /// </summary>
        /// <param name="buffer">Leading pointer of the received data</param>
        /// <param name="size">Size in units of bytes of one profile</param>
        /// <param name="count">Number of profiles</param>
        /// <param name="notify">Completion flag</param>
        /// <param name="user">Thread ID (value passed during initialization)</param>
        public static void CountProfileReceive(IntPtr buffer, uint size, uint count, uint notify, uint user)
        {
            // @Point
            // Take care to only implement storing profile data in a thread save buffer in the callback function.
            // As the thread used to call the callback function is the same as the thread used to receive data,
            // the processing time of the callback function affects the speed at which data is received,
            // and may stop communication from being performed properly in some environments.

            ThreadSafeBuffer.AddCount((int)user, count, notify);
        }



        /// <summary>
        /// Common return code log output
        /// </summary>
        /// <param name="rc">Return code</param>
        private void CommonErrorLog(int rc)
        {
            switch (rc)
            {
                case (int)Rc.Ok:
                    AddLog(string.Format(Resources.SID_RC_FORMAT, Resources.SID_RC_OK));
                    break;
                case (int)Rc.ErrOpenDevice:
                    //AddLog(string.Format(Resources.SID_RC_FORMAT, Resources.SID_RC_ERR_OPEN_DEVICE));
                    break;
                case (int)Rc.ErrNoDevice:
                    AddLog(string.Format(Resources.SID_RC_FORMAT, Resources.SID_RC_ERR_NO_DEVICE));
                    break;
                case (int)Rc.ErrSend:
                    AddLog(string.Format(Resources.SID_RC_FORMAT, Resources.SID_RC_ERR_SEND));
                    break;
                case (int)Rc.ErrReceive:
                    AddLog(string.Format(Resources.SID_RC_FORMAT, Resources.SID_RC_ERR_RECEIVE));
                    break;
                case (int)Rc.ErrTimeout:
                    AddLog(string.Format(Resources.SID_RC_FORMAT, Resources.SID_RC_ERR_TIMEOUT));
                    break;
                case (int)Rc.ErrParameter:
                    AddLog(string.Format(Resources.SID_RC_FORMAT, Resources.SID_RC_ERR_PARAMETER));
                    break;
                case (int)Rc.ErrNomemory:
                    AddLog(string.Format(Resources.SID_RC_FORMAT, Resources.SID_RC_ERR_NOMEMORY));
                    break;
                default:
                    AddLog(string.Format(Resources.SID_NOT_DEFINE_FROMAT, rc));
                    break;
            }
        }


        /// <summary>
		/// Individual return code log output
		/// </summary>
		/// <param name="rc">Return code</param>
		private void IndividualErrorLog(int rc)
        {
            switch (_sendCommand)
            {
                case SendCommand.RebootController:
                    {
                        switch (rc)
                        {
                            case 0x80A0:
                                AddLog(string.Format(Resources.SID_RC_FORMAT, @"Accessing the save area"));
                                break;
                            default:
                                AddLog(string.Format(Resources.SID_NOT_DEFINE_FROMAT, rc));
                                break;
                        }
                    }
                    break;
                case SendCommand.Trigger:
                    {
                        switch (rc)
                        {
                            case 0x8080:
                                AddLog(string.Format(Resources.SID_RC_FORMAT, @"The trigger mode is not [external trigger]"));
                                break;
                            default:
                                AddLog(string.Format(Resources.SID_NOT_DEFINE_FROMAT, rc));
                                break;
                        }
                    }
                    break;
                case SendCommand.StartMeasure:
                case SendCommand.StopMeasure:
                    {
                        switch (rc)
                        {
                            case 0x8080:
                                AddLog(string.Format(Resources.SID_RC_FORMAT, @"Batch measurements are off"));
                                break;
                            case 0x80A0:
                                AddLog(string.Format(Resources.SID_RC_FORMAT, @"Batch measurement start processing could not be performed because the REMOTE terminal is off or the LASER_OFF terminal is on"));
                                break;
                            default:
                                AddLog(string.Format(Resources.SID_NOT_DEFINE_FROMAT, rc));
                                break;
                        }
                    }
                    break;
                case SendCommand.AutoZero:
                case SendCommand.Timing:
                case SendCommand.Reset:
                case SendCommand.GetMeasurementValue:
                    {
                        switch (rc)
                        {
                            case 0x8080:
                                AddLog(string.Format(Resources.SID_RC_FORMAT, @"The operation mode is [high-speed (profile only)]"));
                                break;
                            default:
                                AddLog(string.Format(Resources.SID_NOT_DEFINE_FROMAT, rc));
                                break;
                        }
                    }
                    break;
                case SendCommand.ChangeActiveProgram:
                    {
                        switch (rc)
                        {
                            case 0x8080:
                                AddLog(string.Format(Resources.SID_RC_FORMAT, @"The change program setting is [terminal]"));
                                break;
                            default:
                                AddLog(string.Format(Resources.SID_NOT_DEFINE_FROMAT, rc));
                                break;
                        }
                    }
                    break;
                case SendCommand.GetProfile:
                case SendCommand.GetProfileAdvance:
                    {
                        switch (rc)
                        {
                            case 0x8080:
                                AddLog(string.Format(Resources.SID_RC_FORMAT, @"The operation mode is [advanced (with OUT measurement)]"));
                                break;
                            case 0x8081:
                                AddLog(string.Format(Resources.SID_RC_FORMAT, @"Batch measurements on and profile compression (time axis) off"));
                                break;
                            case 0x80A0:
                                AddLog(string.Format(Resources.SID_RC_FORMAT, @"No profile data"));
                                break;
                            default:
                                AddLog(string.Format(Resources.SID_NOT_DEFINE_FROMAT, rc));
                                break;
                        }
                    }
                    break;
                case SendCommand.GetBatchProfile:
                case SendCommand.GetBatchProfileAdvance:
                    {
                        switch (rc)
                        {
                            case 0x8080:
                                AddLog(string.Format(Resources.SID_RC_FORMAT, @"The operation mode is [advanced (with OUT measurement)]"));
                                break;
                            case 0x8081:
                                AddLog(string.Format(Resources.SID_RC_FORMAT, @"Not [batch measurements on and profile compression (time axis) off]"));
                                break;
                            case 0x80A0:
                                AddLog(string.Format(Resources.SID_RC_FORMAT, @"No batch data (batch measurements not run even once)"));
                                break;
                            default:
                                AddLog(string.Format(Resources.SID_NOT_DEFINE_FROMAT, rc));
                                break;
                        }
                    }
                    break;

                case SendCommand.StartStorage:
                case SendCommand.StopStorage:
                    {
                        switch (rc)
                        {
                            case 0x8080:
                                AddLog(string.Format(Resources.SID_RC_FORMAT, @"The operation mode is [high-speed (profile only)]"));
                                break;
                            case 0x8081:
                                AddLog(string.Format(Resources.SID_RC_FORMAT, @"Storage target setting is [OFF] (no storage)"));
                                break;
                            case 0x8082:
                                AddLog(string.Format(Resources.SID_RC_FORMAT, @"The storage condition setting is not [terminal/command]"));
                                break;
                            default:
                                AddLog(string.Format(Resources.SID_NOT_DEFINE_FROMAT, rc));
                                break;
                        }
                    }
                    break;
                case SendCommand.GetStorageStatus:
                    {
                        switch (rc)
                        {
                            case 0x8080:
                                AddLog(string.Format(Resources.SID_RC_FORMAT, @"The operation mode is [high-speed (profile only)]"));
                                break;
                            default:
                                AddLog(string.Format(Resources.SID_NOT_DEFINE_FROMAT, rc));
                                break;
                        }
                    }
                    break;
                case SendCommand.GetStorageData:
                    {
                        switch (rc)
                        {
                            case 0x8080:
                                AddLog(string.Format(Resources.SID_RC_FORMAT, @"The operation mode is [high-speed (profile only)]"));
                                break;
                            case 0x8081:
                                AddLog(string.Format(Resources.SID_RC_FORMAT, @"The storage target setting is not [OUT value]"));
                                break;
                            default:
                                AddLog(string.Format(Resources.SID_NOT_DEFINE_FROMAT, rc));
                                break;
                        }
                    }
                    break;
                case SendCommand.GetStorageProfile:
                    {
                        switch (rc)
                        {
                            case 0x8080:
                                AddLog(string.Format(Resources.SID_RC_FORMAT, @"The operation mode is [high-speed (profile only)]"));
                                break;
                            case 0x8081:
                                AddLog(string.Format(Resources.SID_RC_FORMAT, @"The storage target setting is not profile, or batch measurements on and profile compression (time axis) off"));
                                break;
                            case 0x8082:
                                AddLog(string.Format(Resources.SID_RC_FORMAT, @"Batch measurements on and profile compression (time axis) off"));
                                break;
                            default:
                                AddLog(string.Format(Resources.SID_NOT_DEFINE_FROMAT, rc));
                                break;
                        }
                    }
                    break;
                case SendCommand.GetStorageBatchProfile:
                    {
                        switch (rc)
                        {
                            case 0x8080:
                                AddLog(string.Format(Resources.SID_RC_FORMAT, @"The operation mode is [high-speed (profile only)]"));
                                break;
                            case 0x8081:
                                AddLog(string.Format(Resources.SID_RC_FORMAT, @"The storage target setting is not profile, or batch measurements on and profile compression (time axis) off"));
                                break;
                            case 0x8082:
                                AddLog(string.Format(Resources.SID_RC_FORMAT, @"Not [batch measurements on and profile compression (time axis) off]"));
                                break;
                            default:
                                AddLog(string.Format(Resources.SID_NOT_DEFINE_FROMAT, rc));
                                break;
                        }
                    }
                    break;
                case SendCommand.HighSpeedDataUsbCommunicationInitalize:
                case SendCommand.HighSpeedDataEthernetCommunicationInitalize:
                    {
                        switch (rc)
                        {
                            case 0x8080:
                                AddLog(string.Format(Resources.SID_RC_FORMAT, @"The operation mode is [advanced (with OUT measurement)]"));
                                break;
                            case 0x80A1:
                                AddLog(string.Format(Resources.SID_RC_FORMAT, @"Already performing high-speed communication error (for high-speed communication)"));
                                break;
                            default:
                                AddLog(string.Format(Resources.SID_NOT_DEFINE_FROMAT, rc));
                                break;
                        }
                    }
                    break;
                case SendCommand.PreStartHighSpeedDataCommunication:
                case SendCommand.StartHighSpeedDataCommunication:
                    {
                        switch (rc)
                        {
                            case 0x8080:
                                AddLog(string.Format(Resources.SID_RC_FORMAT, @"The operation mode is [advanced (with OUT measurement)]"));
                                break;
                            case 0x8081:
                                AddLog(string.Format(Resources.SID_RC_FORMAT, @"The data specified as the send start position does not exist"));
                                break;
                            case 0x80A0:
                                AddLog(string.Format(Resources.SID_RC_FORMAT, @"A high-speed data communication connection was not established"));
                                break;
                            case 0x80A1:
                                AddLog(string.Format(Resources.SID_RC_FORMAT, @"Already performing high-speed communication error (for high-speed communication)"));
                                break;
                            case 0x80A4:
                                AddLog(string.Format(Resources.SID_RC_FORMAT, @"The send target data was cleared"));
                                break;
                            default:
                                AddLog(string.Format(Resources.SID_NOT_DEFINE_FROMAT, rc));
                                break;
                        }
                    }
                    break;
                default:
                    AddLog(string.Format(Resources.SID_NOT_DEFINE_FROMAT, rc));
                    break;
            }
        }

        /// <summary>
		/// Log output
		/// </summary>
		/// <param name="strLog">Output log</param>
		private void AddLog(string strLog)
        {
            txtBox_Log.AppendText(strLog + Environment.NewLine);
            txtBox_Log.SelectionStart = txtBox_Log.Text.Length;
            txtBox_Log.Focus();
            txtBox_Log.ScrollToCaret();
        }


        /// <summary>
		/// Error log output
		/// </summary>
		/// <param name="rc">Return code</param>
		private void AddErrorLog(int rc)
        {
            if (rc < 0x8000)
            {
                // Common return code
                CommonErrorLog(rc);
            }
            else
            {
                // Individual return code
                IndividualErrorLog(rc);
            }
        }


        /// <summary>
		/// Communication command result log output
		/// </summary>
		/// <param name="rc">Return code from the DLL</param>
		/// <param name="commandName">Command name to be output in the log</param>
		private void AddLogResult(int rc, string commandName)
        {
            if (rc == (int)Rc.Ok)
            {
                //AddLog(string.Format(Resources.SID_LOG_FORMAT, commandName, Resources.SID_RESULT_OK, rc));
            }
            else
            {
                //AddLog(string.Format(Resources.SID_LOG_FORMAT, commandName, Resources.SID_RESULT_NG, rc));
                AddErrorLog(rc);
            }
        }



        /// <summary>
        /// 自定义打印日志
        /// </summary>
        /// <param name="rs"></param>
        private void PrintLog(string strLog)
        {
            txtBox_Log.AppendText(strLog + Environment.NewLine);
            txtBox_Log.SelectionStart = txtBox_Log.Text.Length;
            txtBox_Log.Focus();
            txtBox_Log.ScrollToCaret();
        }


        private bool CheckReturnCode(Rc rc)
        {
            if (rc == Rc.Ok) return true;
            MessageBox.Show(this, string.Format("Error: 0x{0,8:x}", rc), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;
        }



        /// <summary>
        /// AnalyzeProfileData
        /// </summary>
        /// <param name="profileCount">Number of profiles that were read</param>
        /// <param name="profileInfo">Profile information structure</param>
        /// <param name="profileData">Acquired profile data</param>
        private void AnalyzeProfileData(int profileCount, ref LJV7IF_PROFILE_INFO profileInfo, int[] profileData)
        {
            int dataUnit = ProfileData.CalculateDataSize(profileInfo);
            AnalyzeProfileData(profileCount, ref profileInfo, profileData, 0, dataUnit);
        }

        /// <summary>
        /// AnalyzeProfileData
        /// </summary>
        /// <param name="profileCount">Number of profiles that were read</param>
        /// <param name="profileInfo">Profile information structure</param>
        /// <param name="profileData">Acquired profile data</param>
        /// <param name="startProfileIndex">Start position of the profiles to copy</param>
        /// <param name="dataUnit">Profile data size</param>
        private void AnalyzeProfileData(int profileCount, ref LJV7IF_PROFILE_INFO profileInfo, int[] profileData, int startProfileIndex, int dataUnit)
        {
            int readPropfileDataSize = ProfileData.CalculateDataSize(profileInfo);
            int[] tempRecvieProfileData = new int[readPropfileDataSize];

            // Profile data retention
            for (int i = 0; i < profileCount; i++)
            {
                Array.Copy(profileData, (startProfileIndex + i * dataUnit), tempRecvieProfileData, 0, readPropfileDataSize);
                _deviceData[_currentDeviceId].ProfileData.Add(new ProfileData(tempRecvieProfileData, profileInfo));
            }
        }



        public void DealWithHightData2D(List<int[]> data)
        {
            for (int i = 0; i < data.Count; i++)
            {
                int[] rs = new int[2];
                int left = 6;
                int right = data[i].Length - 1;
                while (left < right && data[i][left] == 0) left++;
                while (left < right && data[i][right] == 0) right--;
                rs[0] = left;
                rs[1] = right;
                _queue2D.Dequeue();
                _queue2D.Enqueue(rs);
            }
        }


        /// <summary>
        /// USB连接
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void USBToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Rc rc = Rc.Ok;
            // Initialize the DLL
            rc = (Rc)NativeMethods.LJV7IF_Initialize();
            if (!CheckReturnCode(rc)) return;


            int rcUSB = NativeMethods.LJV7IF_UsbOpen(_currentDeviceId);
            AddLogResult(rcUSB, Resources.SID_USB_OPEN);

            // @Point
            // # Enter the "_currentDeviceId" set here for the communication settings into the arguments of each DLL function.
            // # If you want to get data from multiple controllers, prepare and set multiple "_currentDeviceId" values,
            //   enter these values into the arguments of the DLL functions, and then use the functions.

            
            if (rcUSB == (int)Rc.Ok)
            {
                PrintLog("[USB连接]:OK(0x0000) 设备连接成功.");
                _deviceData[_currentDeviceId].Status = DeviceStatus.Usb;
                lb_status.Text = "状态：扫描调试状态";
                if (!StartHightSpeedScan())
                {
                    StopHightSpeedScan();
                    StartHightSpeedScan();
                }
            }
            else
            {
                PrintLog("[USB连接]:NG(0x1000) 设备连接失败.");
                _deviceData[_currentDeviceId].Status = DeviceStatus.NoConnection;
            }
            _deviceStatusLabels[_currentDeviceId].Text = _deviceData[_currentDeviceId].GetStatusString();
        }


        /// <summary>
        /// 网线连接
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 网线连接ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenEthernetForm openEthernetForm = new OpenEthernetForm())
            {
                if (DialogResult.OK == openEthernetForm.ShowDialog())
                {
                    LJV7IF_ETHERNET_CONFIG ethernetConfig = openEthernetForm.EthernetConfig;
                    // @Point
                    // # Enter the "_currentDeviceId" set here for the communication settings into the arguments of each DLL function.
                    // # If you want to get data from multiple controllers, prepare and set multiple "_currentDeviceId" values,
                    //   enter these values into the arguments of the DLL functions, and then use the functions.

                    int rc = NativeMethods.LJV7IF_EthernetOpen(_currentDeviceId, ref ethernetConfig);
                    AddLogResult(rc, Resources.SID_ETHERNET_OPEN);

                    if (rc == (int)Rc.Ok)
                    {
                        PrintLog("[网线连接]:OK(0x0000) 网线连接成功.");
                        _deviceData[_currentDeviceId].Status = DeviceStatus.Ethernet;
                        _deviceData[_currentDeviceId].EthernetConfig = ethernetConfig;

                        if (!StartHightSpeedScan())
                        {
                            StopHightSpeedScan();
                            StartHightSpeedScan();
                            PrintLog("[高速扫描]:OK(0x0000) 高速通道连接成功." + Environment.NewLine + "    ->   Scanning...");
                        }

                    }
                    else
                    {
                        PrintLog("[网线连接]:NG(0x1000) 网线连接失败.");
                        _deviceData[_currentDeviceId].Status = DeviceStatus.NoConnection;
                    }
                    _deviceStatusLabels[_currentDeviceId].Text = _deviceData[_currentDeviceId].GetStatusString();
                    _ethernetConfig = openEthernetForm.EthernetConfig;//打开网络ip设置时，传输网线参数信息
                    HighSpeedPort = openEthernetForm._txtHighSpeedPort.Text;//传输端口
                }
            }
        }


        /// <summary>
        /// 断开设备连接
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 断开连接ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            int rc = NativeMethods.LJV7IF_CommClose(_currentDeviceId);
            AddLogResult(rc, Resources.SID_COMM_CLOSE);
            if(rc == (int)Rc.Ok)
            {
                PrintLog("[USB断开]:OK(0x0000) 设备断开成功.");
                StopHightSpeedScan();
            }
            else
            {
                PrintLog("[USB断开]:NG(0x1000) 设备断开失败");
            }

            _deviceData[_currentDeviceId].Status = DeviceStatus.NoConnection;
            _deviceStatusLabels[_currentDeviceId].Text = _deviceData[_currentDeviceId].GetStatusString();
        }


        ///// <summary>
        ///// 断开设备连接
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void 断开连接ToolStripMenuItem1_Click(object sender, EventArgs e)
        //{
        //    int rc = NativeMethods.LJV7IF_CommClose(_currentDeviceId);
        //    AddLogResult(rc, Resources.SID_COMM_CLOSE);
        //    if (rc == (int)Rc.Ok)
        //    {
        //        PrintLog("[USB断开]:OK(0x0000) 设备断开成功.");
        //        StopHightSpeedScan();
        //    }
        //    else
        //    {
        //        PrintLog("[USB断开]:NG(0x1000) 设备断开失败");
        //    }

        //    _deviceData[_currentDeviceId].Status = DeviceStatus.NoConnection;
        //    _deviceStatusLabels[_currentDeviceId].Text = _deviceData[_currentDeviceId].GetStatusString();
        //}


        /// <summary>
        /// 单次扫描
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 单次扫描ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (_lblDeviceStatus0.Text.Equals("NoConnection"))
            {
                PrintLog("[单次扫描]:NG(0x1000) 单次扫描失败. " + Environment.NewLine + "请先连接仪器：连接->USB连接/网线连接");
                return;
            }
            _sendCommand = SendCommand.GetProfile;

            _deviceData[_currentDeviceId].ProfileData.Clear();
            _deviceData[_currentDeviceId].MeasureData.Clear();
            LJV7IF_GET_PROFILE_REQ req = Req;
            LJV7IF_GET_PROFILE_RSP rsp = new LJV7IF_GET_PROFILE_RSP();
            LJV7IF_PROFILE_INFO profileInfo = new LJV7IF_PROFILE_INFO();
            uint oneProfDataBuffSize = 12828;
            uint allProfDataBuffSize = oneProfDataBuffSize * req.byGetProfCnt;
            int[] profileData = new int[allProfDataBuffSize / Marshal.SizeOf(typeof(int))];//profileData[6-806]轮廓信息
            using (PinnedObject pin = new PinnedObject(profileData))
            {
                int rc = NativeMethods.LJV7IF_GetProfile(_currentDeviceId, ref req, ref rsp, ref profileInfo, pin.Pointer, allProfDataBuffSize);
                AddLogResult(rc, Resources.SID_GET_PROFILE);
                if (rc == (int)Rc.Ok)
                {
                    // Response data display
                    AnalyzeProfileData((int)rsp.byGetProfCnt, ref profileInfo, profileData);
                    ProfileData[] profiles = _deviceData[_currentDeviceId].ProfileData.ToArray();//list转array

                    if(DataExporter.ExportOneTxtProfile(profiles, _defaultSavePath+"\\data\\SingleProfile"+ DateTime.Now.ToString("-yyyyMMdd HH:mm:ss") +".txt"))
                        PrintLog("[单次扫描]:OK(0x0000) 扫描成功.");
                    else
                        PrintLog("[单次扫描]:NG(0x1000) 扫描失败.");

                    //对800个轮廓值，进行数据处理
                    int[] data = ProfileDataAnalysis.DealWithSingProfileData(profiles[0].ProfDatas);

                    //绘制单次扫描轮廓
                    chart_Profile.Series[0].Points.Clear();
                    for (int i = 0; i < data.Length; i++)
                    {
                        chart_Profile.Series[0].Points.AddXY((i + Define.PROFILE_MIN_X + 1), data[i]);
                    }


                }
                else
                {
                    PrintLog("[单次扫描]:NG(0x1000) 扫描失败.");

                }
            }
        }



        /// <summary>
        /// 开始高速扫描
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 开始高速扫描ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(_deviceData[_currentDeviceId].Status== DeviceStatus.NoConnection)
            {
                PrintLog("[高速扫描]:NG(0x1000) 高速通道连接失败. " + Environment.NewLine + "请先连接仪器：连接->USB连接/网线连接");
                return;
            }
            MysqlConnection.truncateTableData("misaligned");

            
            _deviceData[_currentDeviceId].Status = DeviceStatus.UsbFast;
            _deviceStatusLabels[_currentDeviceId].Text = _deviceData[_currentDeviceId].GetStatusString();

            _countScan = 0;
            _heightSpeedSaveIsOpen = true;
            _updateChartData = true;

            _startBinDataTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"); 
            _binFileDataTime = _startBinDataTime.Replace("/","").Replace(":","").Replace(" ","");

        }
        

        private void 结束本次扫描ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_deviceData[_currentDeviceId].Status != DeviceStatus.UsbFast)
                return;
            //StopHightSpeedScan();
            _deviceData[_currentDeviceId].Status = DeviceStatus.Usb;
            _deviceStatusLabels[_currentDeviceId].Text = _deviceData[_currentDeviceId].GetStatusString();
            _heightSpeedSaveIsOpen = false;
            _updateChartData = false;

            _endtBinDataTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            lb_status.Text = "状态：扫描结束 " + _startBinDataTime + "-" + _endtBinDataTime.Substring(11,8);
            //清除数据缓存
            for (int i = 0; i < Define.VERTICAL_MAX_X; i++)
            {
                _queueVertical.Dequeue();
                _queueVertical.Enqueue(0);
            }

            for (int i = 0; i < Define.PROFILE2D_MAX_X; i++)
            {
                _queue2D.Dequeue();
                _queue2D.Enqueue(new int[2] { 0, 0 });
            }

            //将原始数据BIN信息记录写入数据库
            FileInfo fi = new FileInfo(_defaultSavePath + "\\data\\source\\scandata.bin");
            string binPath = _defaultSavePath + "data\\source\\" + _binFileDataTime + ".bin";
            string size = (fi.Length / 1024.00 / 1024.00).ToString("f2");
            fi.MoveTo(binPath);
            if (double.Parse(size) > 1024)//文件大小超过1G,转换单位
                size = (double.Parse(size)/1024.00).ToString()+"G";
            else
                size= size+"M";
            string sqlStr = "INSERT INTO historydata (h_id,h_startTime,h_endTime,h_dataFileName,h_dataFilePath,h_dataFileSize,h_sqlRecordPath,h_reportFileName,h_reportFilePath,h_reportFileSize) VALUES (NULL,'" + _startBinDataTime +"','"+ _endtBinDataTime + "','" + _binFileDataTime + ".bin','" + binPath.Replace("\\", "\\\\") + "','" + size+ "','-1','-1','-1','-1')";
            MysqlConnection.executeInsert(sqlStr);
            MessageBox.Show("请生成此次扫描报表并及时导出。");
        }


        private bool StartHightSpeedScan()
        {
            // Stop and finalize high-speed data communication.
            //NativeMethods.LJV7IF_StopHighSpeedDataCommunication(Define.DEVICE_ID);
            //NativeMethods.LJV7IF_HighSpeedDataCommunicationFinalize(Define.DEVICE_ID);

            // Initialize the data.
            ThreadSafeBuffer.Clear(Define.DEVICE_ID);
            Rc rc = Rc.Ok;

            // Initialize the high-speed communication path
            // High-speed communication start preparations
            LJV7IF_HIGH_SPEED_PRE_START_REQ req = new LJV7IF_HIGH_SPEED_PRE_START_REQ();

            try
            {
                uint frequency = Convert.ToUInt32(_frequencyCount);//每200次触发，返回一次
                uint threadId = (uint)Define.DEVICE_ID;

                //选择USB还是网口
                if (_deviceData[_currentDeviceId].Status.Equals(DeviceStatus.Usb)||_deviceData[_currentDeviceId].Status.Equals(DeviceStatus.UsbFast))
                {
                    // Clear the retained profile data.
                    _deviceData[_currentDeviceId].ProfileData.Clear();
                    _deviceData[_currentDeviceId].MeasureData.Clear();

                    // Initialize USB high-speed data communication
                    rc = (Rc)NativeMethods.LJV7IF_HighSpeedDataUsbCommunicationInitalize(Define.DEVICE_ID, _callback, frequency, threadId);
                    PrintLog("[USB高速扫描]:OK(0x0000) USB高速通道连接成功." + Environment.NewLine + "    ->   Scanning...");
                    //PrintLog("[高速扫描]:OK(0x0000) 高速通道连接成功." + Environment.NewLine + "    ->   Scanning...");
                    //_deviceData[_currentDeviceId].Status = DeviceStatus.UsbFast;
                    //_deviceStatusLabels[_currentDeviceId].Text = _deviceData[_currentDeviceId].GetStatusString();
                }
                else if (_deviceData[_currentDeviceId].Status.Equals(DeviceStatus.Ethernet))
                {
                    // Generate the settings for Ethernet communication.
                    ushort highSpeedPort = 0;
                    highSpeedPort = Convert.ToUInt16(HighSpeedPort);

                    // Initialize Ethernet high-speed data communication
                    rc = (Rc)NativeMethods.LJV7IF_HighSpeedDataEthernetCommunicationInitalize(Define.DEVICE_ID, ref _ethernetConfig,
                        highSpeedPort, _callback, frequency, threadId);
                    //PrintLog("[网口高速扫描]:OK(0x0000) 网口高速通道连接成功." + Environment.NewLine + "     ->   Scanning...");
                }
                if (!CheckReturnCode(rc))
                {
                    PrintLog("[高速扫描通道]:NG(0x1000) 高速扫描通道连接失败.");
                    return false;
                }
                req.bySendPos = Convert.ToByte(StartProfileNo);

            }
            catch (FormatException ex)
            {
                MessageBox.Show(this, ex.Message);
                return false;
            }
            catch (OverflowException ex)
            {
                MessageBox.Show(this, ex.Message);
                return false;
            }

            // High-speed data communication start preparations
            LJV7IF_PROFILE_INFO profileInfo = new LJV7IF_PROFILE_INFO();
            rc = (Rc)NativeMethods.LJV7IF_PreStartHighSpeedDataCommunication(Define.DEVICE_ID, ref req, ref profileInfo);
            if (rc != Rc.Ok) return false;

            // Start high-speed data communication.
            rc = (Rc)NativeMethods.LJV7IF_StartHighSpeedDataCommunication(Define.DEVICE_ID);
            if (rc != Rc.Ok) return false;
            
            _sendCommand = SendCommand.StartHighSpeedDataCommunication;
            _timerHighSpeed.Start();
            return true;
        }



        private void StopHightSpeedScan()
        {
            // Stop high-speed data communication.
            //重命名bin存储文件


            Rc rc = (Rc)NativeMethods.LJV7IF_StopHighSpeedDataCommunication(Define.DEVICE_ID);
            if (CheckReturnCode(rc))
            {
                // Finalize high-speed data communication.
                rc = (Rc)NativeMethods.LJV7IF_HighSpeedDataCommunicationFinalize(Define.DEVICE_ID);
                CheckReturnCode(rc);
                _countScan = 0;

                switch (_deviceData[_currentDeviceId].Status)
                {
                    case DeviceStatus.UsbFast:
                        _deviceData[_currentDeviceId].Status = DeviceStatus.Usb;
                        break;
                    case DeviceStatus.EthernetFast:
                        LJV7IF_ETHERNET_CONFIG config = _deviceData[_currentDeviceId].EthernetConfig;
                        _deviceData[_currentDeviceId].Status = DeviceStatus.Ethernet;
                        _deviceData[_currentDeviceId].EthernetConfig = config;
                        break;
                    default:
                        break;
                }
                _deviceStatusLabels[_currentDeviceId].Text = _deviceData[_currentDeviceId].GetStatusString();
            }
        }
        

        /// <summary>
        /// Timer event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _timerHighSpeed_Tick(object sender, EventArgs e)
        {
            uint notify = 0;
            int batcnNo = 0;
            List<int[]> data = ThreadSafeBuffer.Get(Define.DEVICE_ID, out notify, out batcnNo);
            if (data.Count > 0)
            {              

                ProfileDataAnalysis.DealWithHightProfileData(data);//数据解析
                //纠偏补正
                if (_correctionEd)
                    k =correctionSuppl(data[0]);


                if (drawShap == View.HightSpeed)//绘制x轴扫描轮廓
                {
                    //绘制x轴扫描轮廓
                    if (_updateChartData)
                        DrawHightProfile(data[0],k);
                    int[] dataVertical = DealWithVerticalProfileData(data);
                    if (_updateChartData)
                        DrawVerticalProfile(dataVertical);

                }
                else if (drawShap == View.HightSpeed2D)//绘制2D图
                {
                    DealWithHightData2D(data);
                    if (_updateChartData)
                        DrawHight2DProfile();
                    int[] dataVertical = DealWithVerticalProfileData(data);
                    //绘制z轴轮廓
                    if (_updateChartData)
                        DrawVerticalProfile(dataVertical);
                }
                else if (drawShap == View.HightSpeed3D)//绘制3D图
                {

                }

                //计算分析与存储
                if (_heightSpeedSaveIsOpen)
                {
                    //根据补正参
                    double[] rs=MisalignedScan.MisalignedAnalizeData(data, k);//数据分析入库

                    chart_Misa.Series[0].Points.AddXY(_countScan*0.6/1000 , rs[0]);//绘制磨损全局曲线
                    chart_LeftRight.Series[0].Points.AddXY(_countScan * 0.6 / 1000, rs[1]*0.3);
                    //chart_LeftRight.Series[1].Points.AddXY(_countScan * 0.6 / 1000, rs[2]);
                    chart_LeftRight.Series[2].Points.AddXY(_countScan * 0.6 / 1000, rs[3]*0.3);

                    //(i + Define.VERTICAL_MIN_X+1)*0.6/1000 +_countScan*0.6/1000
                    if (!DataExporter.ExportHeightSpeedBinData(data, _defaultSavePath + "data\\source\\scandata.bin"))
                    {
                        PrintLog("[存储高速扫描数据]:NG(0x1000) 存储异常.");
                    }

                    MisalignedScanInfo(data);//实时计算缝隙与灌道长度

                    _countScan += data.Count;
                    _lblReceiveProfileCount0.Text = _countScan.ToString();
                    lb_status.Text = "状态：扫描中 " + _startBinDataTime + "-" + DateTime.Now.ToLongTimeString().ToString();
                }

            }

            if ((notify & 0xFFFF) != 0)
            {
                // If the lower 16 bits of the notification are not 0, high-speed communication was interrupted, so stop the timer.
                _timerHighSpeed.Stop();
                PrintLog("[高速扫描通道]: 高速扫描通道关闭.");
                //MessageBox.Show(string.Format("Finalize communication.0x{0:x8}", notify));
            }
            else if ((notify & 0x10000) != 0)
            {
                // A descriptor is included here if processing when batch measurement is finished is required.
            }
        }

        ///做缓存，存储计算值，供实时计算
        private static Queue<int[]> misaQueue = new Queue<int[]>(2);//1KHZ ，200帧一个list，缓存10个list，即为2秒数据
        


        //实时计算缝隙与错位
        public void MisalignedScanInfo(List<int[]> datas)
        {
            for (int i = 0; i < datas.Count; i++)
            {
                int leftTx = 6;
                int rightTx = 806;
                int[] info = new int[3];
                while (leftTx < rightTx && datas[i][leftTx] == Define.INVALID_DATA_ORIGINAL) leftTx++;
                while (leftTx < rightTx && datas[i][rightTx] == Define.INVALID_DATA_ORIGINAL) rightTx--;
                //灌道
                if (rightTx - leftTx > 300)
                {
                    
                    info[0] = leftTx;
                    for (int j = (leftTx+rightTx)/2; j < (leftTx + rightTx) / 2+10; j++)
                    {
                        info[1] += datas[i][j];
                    }
                    info[1] /= 10;
                    info[2] = rightTx;
                    misaQueue.Dequeue();
                    misaQueue.Enqueue(info);

                    //刚过完衔接缝
                    if (_countF != 0)
                    {
                        //计算上根灌道与该灌道的错位参数
                        int[][] temp = misaQueue.ToArray();
                        //求均值
                        int misaX = 0;
                        int misaY = 0;
                        for (int j = 0; j < temp.Length-1; j++)
                        {
                            misaX += temp[j][0];
                            misaY += temp[j][1];
                        }
                        misaX /= (temp.Length-1);
                        misaY /= (temp.Length-1);
                        misaX -= info[0];
                        misaY -= info[1];
                        dgv_scanInfoDataTable.Rows[index].Cells[1].Value = Math.Abs(misaX * Define.XLEN) +"mm";
                        dgv_scanInfoDataTable.Rows[index].Cells[2].Value = Math.Abs(misaY) +"mm";


                        _countF = 0;
                        index = dgv_scanInfoDataTable.Rows.Add();
                        dgv_scanInfoDataTable.Rows[index].Cells[0].Value = _countSing+"灌道▅";
                        dgv_scanInfoDataTable.Rows[index].Cells[1].Value = "";
                        dgv_scanInfoDataTable.Rows[index].Cells[2].Value = "";
                        dgv_scanInfoDataTable.Rows[index].Cells[3].Value = "";
                        dgv_scanInfoDataTable.Rows[index].Cells[0].Style.ForeColor = Color.Blue;
                        dgv_scanInfoDataTable.Rows[index].Cells[1].Style.ForeColor = Color.Blue;
                        dgv_scanInfoDataTable.Rows[index].Cells[2].Style.ForeColor = Color.Blue;
                        dgv_scanInfoDataTable.Rows[index].Cells[3].Style.ForeColor = Color.Blue;
                    }

                    //更新灌道里程
                    _countG++;
                    //dgv_scanInfoDataTable.Rows[index].Cells[3].Value = (_countG+1) * (Define.V/ Define.F)+"m";
                    
                }
                //衔接缝
                else{
                    //刚过完灌道
                    if (_countF == 0)
                    {
                        //灌道数+1
                        _countSing++;
                        //将上根灌道扫描次数置零
                        _countG = 0;
                        index = dgv_scanInfoDataTable.Rows.Add();
                        dgv_scanInfoDataTable.Rows[index].Cells[0].Value = "衔接缝隙";
                        dgv_scanInfoDataTable.Rows[index].Cells[1].Value = 0;
                        dgv_scanInfoDataTable.Rows[index].Cells[2].Value = 0;
                        dgv_scanInfoDataTable.Rows[index].Cells[3].Value = 0;
                        dgv_scanInfoDataTable.Rows[index].Cells[0].Style.ForeColor = Color.Red;
                        dgv_scanInfoDataTable.Rows[index].Cells[1].Style.ForeColor = Color.Red;
                        dgv_scanInfoDataTable.Rows[index].Cells[2].Style.ForeColor = Color.Red;
                        dgv_scanInfoDataTable.Rows[index].Cells[3].Style.ForeColor = Color.Red;
                    }
                    //更新衔接缝里程
                    _countF++;
                    dgv_scanInfoDataTable.Rows[index].Cells[3].Value = (_countF+1) * (Define.V / Define.F)*1000 + "mm";
                }
            }
        }




        /// <summary>
        /// z轴数据处理,显示中线
        /// </summary>
        /// <param name="idata"></param>
        /// <returns></returns>
        private int[] DealWithVerticalProfileData(List<int[]> idata)
        {
            int left = 6;
            int right = idata[0].Length-1;
            for (int i = 0; i < idata.Count; i++)
            {
                left = 6;
                right = idata[i].Length - 1;
                while (left < right && idata[i][left] == 0) left++;
                while (left < right && idata[i][right] == 0) right--;
                _queueVertical.Dequeue();
                if (left != right)
                {
                    _queueVertical.Enqueue((int)(idata[i][(left+right)/2]));
                }else
                    _queueVertical.Enqueue(0);

            }
            return _queueVertical.ToArray();
        }


        /// <summary>
        /// 绘制高速扫描轮廓,
        /// </summary>
        /// <param name="data"></param>
        private void DrawHightProfile(int[] dataY,double pk)
        {
            //高速扫描
            chart_Profile.Series[0].Points.Clear();
            for (int i = 6; i < dataY.Length - 6; i++)
            {
                if(dataY[i]!=0)
                    chart_Profile.Series[0].Points.AddXY((i + Define.PROFILE_MIN_X + 1 - 6)*0.3, dataY[i]-pk*i);
                else
                    chart_Profile.Series[0].Points.AddXY((i + Define.PROFILE_MIN_X + 1 - 6)*0.3, dataY[i]);
            }
        }

        /// <summary>
        /// 绘制z轴方向（垂直大地方向）轮廓//z方向轮廓绘制（x轴：800个宽度，)
        /// </summary>
        private void DrawVerticalProfile(int[] dataVertical)
        {
            chart_Vertical.Series[0].Points.Clear();
            
            for (int i = 0; i < dataVertical.Length; i++)
            {
                chart_Vertical.Series[0].Points.AddXY((i + Define.VERTICAL_MIN_X+1)*0.6/1000 +_countScan*0.6/1000, dataVertical[i]);
            }

        }
        /// <summary>
        /// 绘制2D轮廓值
        /// </summary>
        /// <param name="profileDeal"></param>
        private void DrawHight2DProfile()
        {
            int i = 0;
            chart_Profile.Series[0].Points.Clear();
            foreach (int[] item in _queue2D)
            {
                if (true)
                {
                    chart_Profile.Series[0].Points.AddXY((i +1)* 0.6 / 1000 + _countScan * 0.6 / 1000, item[0]*0.3);
                    chart_Profile.Series[0].Points.AddXY((i +1 ) * 0.6 / 1000 + _countScan * 0.6 / 1000, item[1]*0.3);
                }
                i++;
            }
        }


        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_ResetScanCount_Click(object sender, EventArgs e)
        {
            _countScan = 0;
        }

        private void 实时波形图ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            chart_Profile.Visible = true;
            chart_Vertical.Visible = true;
            pBox_chart2D.Visible = true;
            label8.Visible = true;
            pictureBox3.Visible = true;
            label9.Visible = true;
            wb_Profile.Visible = false;


            drawShap = View.HightSpeed;


            pBox_chart2D.Image = Resources.line;
            chart_Profile.ChartAreas[0].AxisX.Title = "待测物宽度（单位：mm）";
            chart_Profile.Titles.Clear();
            chart_Profile.Titles.Add("探头距离\n(单位:mm)");
            chart_Profile.Titles[0].Position.X = Define.CTPX;
            chart_Profile.Titles[0].Position.Y = Define.CTPY;


            //设置最大最小值
            chart_Profile.ChartAreas[0].AxisY.Minimum = Define.PROFILE_MIN_Y;
            chart_Profile.ChartAreas[0].AxisY.Maximum = Define.PROFILE_MAX_Y;
            chart_Profile.ChartAreas[0].AxisX.Interval = Define.PROFILE_Interval_X;
            //设置刻度
            chart_Profile.ChartAreas[0].AxisY.Interval = Define.PROFILE_Interval_Y;
            PrintLog("[波形切换]:OK(0x0000) 实时波形切换成功.");
        }


        private void 实时2D图ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            chart_Profile.Visible = true;
            chart_Vertical.Visible = true;
            pBox_chart2D.Visible = true;
            label8.Visible = true;
            pictureBox3.Visible = true;
            label9.Visible = true;
            wb_Profile.Visible = false;
            drawShap = View.HightSpeed2D;

            pBox_chart2D.Image = Resources.part2D;
            
            //设置最大最小值
            chart_Profile.ChartAreas[0].AxisY.Minimum = Define.PROFILE2D_MIN_Y;
            chart_Profile.ChartAreas[0].AxisY.Maximum = Define.PROFILE2D_MAX_Y;
            chart_Profile.ChartAreas[0].AxisX.Interval = 0.1;
            //设置刻度
            chart_Profile.ChartAreas[0].AxisY.Interval = Define.PROFILE2D_Interval_Y;


            chart_Profile.ChartAreas[0].AxisX.Title = "探头移动距离（单位：m）";
            chart_Profile.Titles.Clear();
            chart_Profile.Titles.Add("待测物宽度\n(单位:mm)");
            chart_Profile.Titles[0].Position.X = Define.CTPX;
            chart_Profile.Titles[0].Position.Y = Define.CTPY;
            
            PrintLog("[波形切换]:OK(0x0000) 实时波形切换成功.");
        }
        

        private void 实3D轮廓图ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            chart_Profile.Visible = false;
            chart_Vertical.Visible = false;
            wb_Profile.Visible = true;
            pBox_chart2D.Visible = false;
            label8.Visible = false;
            pictureBox3.Visible = false;
            label9.Visible = false;


            drawShap = View.HightSpeed3D;
            try
            {
                //绘制3D图
                initWebBrowser();

                string str = Environment.CurrentDirectory;
                wb_Profile.Url = new Uri(str + "\\PipeProfile3D.html");

                wb_Profile.Document.InvokeScript("ShopXG");
                

            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
                //PrintLog("[波形切换]:NG(0x1000) 实时3D图切换失败，请到注册表HKEY_LOCAL_MACHINE\\SOFTWARE\\WOW6432Node\\Microsoft\\Internet Explorer\\Main\\FeatureControl\\FEATURE_BROWSER_EMULATION添加名称为PipeModel，十进制值为9000的项");
            }
        }

        /// <summary>
        /// 与js传输数据
        /// </summary>
        /// <returns></returns>
        public string getStatic3DData()
        {
            //获取原始数据对应存储路径
            string dataFilePath = MysqlConnection.executeQuery_data("select h_dataFilePath from historyData order by h_id DESC limit 1")[0][0];
            //string dataFilePath = @"E:\workplaceC#\PipeModel5.0\data\source\20190728171906.bin";
            //读bin文件生成
            FileStream fs = new FileStream(dataFilePath, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            br.BaseStream.Seek(0, SeekOrigin.Begin);
            double frame = Math.Ceiling(br.BaseStream.Length / 4 / 800 / 1000.00);//计算抽取帧数
            StringBuilder strData = new StringBuilder();

            while (br.BaseStream.Position + 4 * 807 * _frequencyCount* frame < br.BaseStream.Length)
            {
                //读数据取样抽帧
                byte[] bsRs = new byte[4*807*_frequencyCount];
                for (int i = 0; i < _frequencyCount; i++)
                {
                    br.BaseStream.Position += 4 * 807 * Convert.ToInt32(frame/2);
                    byte[] bs = br.ReadBytes(4 * 807);//4 * 807 * frequencyCount
                    Array.Copy(bs,0,bsRs,4*807*i,bs.Length);
                }


                List<int[]> data = byteArrayToIntArrayList(bsRs);
                for (int i = 0; i < data.Count; i++)
                {
                    for (int j = 7; j < data[i].Length; j++)
                    {
                        strData.Append(",").Append(data[i][j]);
                    }

                }

            }
            //temp = strData.ToString().Substring(1);
            return strData.ToString().Substring(1);
        }




        private void 导出磨损曲线图ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }


        private void 实时3D图ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //chart_Profile.Visible = false;
            //chart_Vertical.Visible = false;
            //wb_Profile.Visible = true;

            //drawShap = View.HightSpeed3D;
            //try
            //{
            //    //绘制3D图
            //    initWebBrowser();

            //    string str = Environment.CurrentDirectory;
            //    wb_Profile.Url = new Uri(str + "\\Profile3D.html");

            //    wb_Profile.Document.InvokeScript("ShopXG");

            //    PrintLog("[波形切换]:OK(0x0000) 3D图切换切换成功.");


            //}
            //catch (Exception ex)
            //{
            //    throw new Exception(ex.ToString());
            //    //PrintLog("[波形切换]:NG(0x1000) 实时3D图切换失败，请到注册表HKEY_LOCAL_MACHINE\\SOFTWARE\\WOW6432Node\\Microsoft\\Internet Explorer\\Main\\FeatureControl\\FEATURE_BROWSER_EMULATION添加名称为PipeModel，十进制值为9000的项");
            //}
        }

        

        /// <summary>
        /// 与js传输数据
        /// </summary>
        /// <returns></returns>
        public string getCurrent3DData()
        {
            string temp = "";
            int[][] data = _queue3DCurrentData.ToArray();
            StringBuilder str = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                for (int j = 7; j < data[i].Length; j++)
                {
                    str.Append(",").Append(data[i][j]);
                }
            }
            temp = str.ToString().Substring(1);

            return temp;
        }




        /// <summary>
        /// 初始化浏览器
        /// </summary>
        private void initWebBrowser()
        {
            //防止 WebBrowser 控件打开拖放到其上的文件。
            wb_Profile.AllowWebBrowserDrop = true;
            //防止 WebBrowser 控件在用户右击它时显示其快捷菜单.
            wb_Profile.IsWebBrowserContextMenuEnabled = true;
            //以防止 WebBrowser 控件响应快捷键。
            wb_Profile.WebBrowserShortcutsEnabled = false;
            //以防止 WebBrowser 控件显示脚本代码问题的错误信息。    
            wb_Profile.ScriptErrorsSuppressed = false;
            //（这个属性比较重要，可以通过这个属性，把WINFROM中的变量，传递到JS中，供内嵌的网页使用；但设置到的类型必须是COM可见的，所以要设置     [System.Runtime.InteropServices.ComVisibleAttribute(true)]，因为我的值设置为this,所以这个特性要加载窗体类上）
            wb_Profile.ObjectForScripting = this;

        }
        
        /// <summary>
        /// 更改数据保存地址
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 保存ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            //dialog.SelectedPath = _defaultSavePath;
            dialog.Description = "请选择数据存储路径";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                //_defaultSavePath = dialog.SelectedPath;
            }
        }
        

        private void 生成报表ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //查询上一次的数据是否生成过报表
            string[][] data = MysqlConnection.executeQuery_data("select h_sqlRecordPath,h_reportFileName from historyData order by h_id DESC limit 1");
            if (data[0][0] != "-1"&&data[0][1]!="-1")//已经生成过报表sql记录
            {
                MessageBox.Show("已经生成过报表，请导出查看。");
            }
            else
            {
                if (_binFileDataTime == "")
                    _binFileDataTime = MysqlConnection.executeQuery_data("select h_dataFileName from historyData order by h_id DESC limit 1")[0][0].Split('.')[0];
                ReportViewer report = new ReportViewer();
                report.makeReport();//制作报表
                report.writeWord(_defaultSavePath + "\\data\\report\\测量报表" + _binFileDataTime + ".docx");//写入word文档
                MessageBox.Show("报表生成成功，请及时导出。");
            }
        }
        

        private void 导出报表ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (WordTemplate.filename != null)
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "Word Document(*.docx)|*.docx";
                sfd.DefaultExt = "Word Document(*.docx)|*.docx";
                string[] reoprtPath = WordTemplate.filename.ToString().Split('\\');
                sfd.FileName = _defaultSavePath + reoprtPath[reoprtPath.Length-1];
                sfd.InitialDirectory = _defaultSavePath;
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    File.Copy(WordTemplate.filename.ToString(),sfd.FileName,true);
                }
            }
        }

        private void 历史ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            string[] files = Directory.GetFiles(_defaultSavePath+"\\data");//得到文件
            foreach (string file in files)//循环文件
            {
                string exname = file.Substring(file.LastIndexOf(".") + 1);//得到后缀名
                if (".bin".IndexOf(file.Substring(file.LastIndexOf(".") + 1)) > -1)//如果后缀名为.txt文件
                {
                    FileInfo fi = new FileInfo(file);//建立FileInfo对象
                    //dataHistoryForm.listBox_HistoryData.Items.Add(fi.FullName);//把.txt文件全名加人到FileInfo对象
                }
            }
        }

        /// <summary>
        /// 点击历史数据选项卡
        /// </summary>
        private void tabControl_View_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl_View.SelectedTab.Name.Equals("tabPage_Data"))
            {
                string sqlStr = "select h_startTime,h_reportFileName,h_reportFileSize,h_dataFileName from historydata order by h_id DESC limit 20";
                string[][] data = MysqlConnection.executeQuery_data(sqlStr);
                InitHistoryDataTable(data);
            }
        }

        
        private void 调试ToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            _heightSpeedSaveIsOpen = false;
            _updateChartData = true;
        }

        /// <summary>
        /// 历史数据选项卡，查询按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_query_Click(object sender, EventArgs e)
        {
            string startTime = dt_queryStartTime.Value.ToString("yyyy/MM/dd 00:00:00");
            string endTime = dt_queryEndTime.Value.ToString("yyyy/MM/dd 23:59:59");
            StringBuilder sqlStr = new StringBuilder("select h_startTime,h_reportFileName,h_reportFileSize,h_dataFileName from historydata");
            if (!startTime.Equals(endTime))//带上时间属性查询
            {
                sqlStr.Append(" where h_startTime between  '" + startTime + "'and '" + endTime + "'");
            }
            string [][]data=MysqlConnection.executeQuery_data(sqlStr.ToString());
            for (int i = 0; i < effectBtn; i++)
            {
                for (int j = 0; j < historyTableBtnText.Length; j++)
                {
                    btn[i][j].Visible = false;
                }
            }

            InitHistoryDataTable(data);
        }

        

        /// <summary>
        /// 历史数据选项卡，刷新按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_refresh_Click(object sender, EventArgs e)
        {
            //读文件夹，将信息写进数据库
            //string[] dataFiles = Directory.GetFiles(_defaultSavePath+"data\\source", "*.bin", SearchOption.AllDirectories);//获取目录文件名称集合
            //string[] reportFiles = Directory.GetFiles(_defaultSavePath + "data\\report", "*.docx", SearchOption.AllDirectories);//获取目录文件名称集合
            //string[] sqlFiles = Directory.GetFiles(_defaultSavePath + "data\\sql", "*.txt", SearchOption.AllDirectories);//获取目录文件名称集合
            //for (int i = 0; i < dataFiles.Length; i++)
            //{
            //    FileInfo dataFi = new FileInfo(dataFiles[i]);
            //    FileInfo reportFi = new FileInfo(reportFiles[i]);
            //    FileInfo sqlFi = new FileInfo(sqlFiles[i]);
            //    MysqlConnection.executeInsert("insert into historyData (h_id,h_startTime,h_endTime,h_dataFileName,h_dataFilePath,h_dataFileSize,h_sqlRecordPath,h_reportFileName,h_reportFilePath,h_reportFileSize) values (null,'" + dataFi.Name.Split('.')[0]+"','-1','"+dataFi.Name+"','"+dataFi.FullName.Replace("\\","\\\\")+"','"+ Math.Ceiling(dataFi.Length / 1024.00) + "KB','" +sqlFi.FullName+ "','"+reportFi.Name + "','" + reportFi.FullName.Replace("\\", "\\\\") + "','" + Math.Ceiling(reportFi.Length / 1024.00) + "KB')");

            //}

            dgv_historyDataTable.DataSource = null;
            string sqlStr = "select h_startTime,h_reportFileName,h_reportFileSize,h_dataFileName from historydata order by h_id DESC limit 20";
            string[][] data = MysqlConnection.executeQuery_data(sqlStr);
            InitHistoryDataTable(data);
        }
        

       

        /// <summary>
        /// 扫描线纠偏补正
        /// </summary>
        /// <param name="profileDeal"></param>
        private double correctionSuppl(int[] profileDeal)
        {
            int leftT = 6;
            int rightT = profileDeal.Length - 1;

            while (leftT <= rightT && profileDeal[leftT] == Define.INVALID_DATA_ORIGINAL) leftT++;
            while (leftT <= rightT && profileDeal[rightT] == Define.INVALID_DATA_ORIGINAL) rightT--;
            if (rightT - leftT < 20)
                return 0;
            leftT += 10;
            rightT -= 10;
            k = (profileDeal[rightT] - profileDeal[leftT])/ Convert.ToDouble(rightT-leftT);
            _correctionEd = false;//标志已经计算过一次
            return k;
        }

       

        private void 补正ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //_isOpenCorrection = true;

            //每点击一下补正，则计算一次补正参数k值
            _correctionEd = true;
        }

        private void 还原ToolStripMenuItem_Click(object sender, EventArgs e)
        {
             k = 0;
            //_isOpenCorrection = false;
            _correctionEd = false;
        }

        private void 轮廓图ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //pl_3D.Visible = true;
            //string sqlStr = "select m_misaXLeft,m_misaXRight,m_misaYLeft,m_misaYMiddle,m_misaYRight from misaligned";
            //string[][] dataStr = MysqlConnection.executeQuery_data(sqlStr);
            //Graphics g = pl_3D.CreateGraphics();
            //for (int i = 0; i < dataStr.Length; i++)
            //{
            //    int p = ((int.Parse(dataStr[i][4]) - int.Parse(dataStr[i][2])) * int.Parse(dataStr[i][3])) % 255;
            //    g.DrawLine(new Pen(Color.FromArgb(100, p, 200, 200), 1), int.Parse(dataStr[i][0]), i, int.Parse(dataStr[i][1]), i);
            //}

        }

        private void cb_selectSkin_SelectedIndexChanged(object sender, EventArgs e)
        {
            string skinPath = Application.StartupPath + "\\Skins\\";
            skinEngine.SkinFile = skinPath+cb_selectSkin.SelectedItem.ToString();
        }

        private void 测试ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
             chart_Profile.SaveImage("\\image.png", ChartImageFormat.Png);
        }


        /// <summary>
        ///      //导出left，right，middleLeft，middle，middleRight，misa
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 导出特征数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!_startBinDataTime.Equals(""))
            {
                string sql = "SELECT * FROM misaligned";
                string[][] str = MysqlConnection.executeQuery_data(sql);
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "Word Document(*.txt)|*.html";
                sfd.DefaultExt = "Word Document(*.txt)|*.html";

                sfd.FileName = _startBinDataTime+"特征数据.txt";
                sfd.InitialDirectory = _defaultSavePath + "\\data";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    DataExporter.ExportText(str, sfd.FileName);
                }
                MessageBox.Show("导出成功");
            }
            else
            {
                MessageBox.Show("无数据");
            }

        }

        private void 导出表ToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Word Document(*.png)|*.jpg";
            sfd.DefaultExt = "Word Document(*.png)|*.jpg";

            sfd.FileName = _startBinDataTime + "chartMisa_表3.png";
            sfd.InitialDirectory = _defaultSavePath + "\\data\\images";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                chart_Misa.SaveImage(sfd.FileName, ChartImageFormat.Png);
            }
            
        }

        private void 导出表ToolStripMenuItem4_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Word Document(*.png)|*.jpg";
            sfd.DefaultExt = "Word Document(*.png)|*.jpg";

            sfd.FileName = _startBinDataTime + "chartLeftRight_表4.png";
            sfd.InitialDirectory = _defaultSavePath + "\\data\\images";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                chart_Misa.SaveImage(sfd.FileName, ChartImageFormat.Png);
            }
        }
    }
}
