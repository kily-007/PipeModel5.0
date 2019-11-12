//----------------------------------------------------------------------------- 
// <copyright file="Define.cs" company="KEYENCE">
//	 Copyright (c) 2013 KEYENCE CORPORATION.  All rights reserved.
// </copyright>
//----------------------------------------------------------------------------- 
namespace PipeModel
{
    /// <summary>
    /// Constant class
    /// </summary>
    public static class Define
    {
        #region Constant

        

        ///编码器触发,下列值调整为：
        /// V=0.6
        /// F=1000
        /// <summary>
        /// 电梯检修速度
        /// </summary>
        public const double V = 0.6;//(单位：m/s)
        /// <summary>
        /// 触发器出发频率
        /// </summary>
        public const double F = 1000;//(单位：HZ)

        /// <summary>
        ///光点间距
        /// </summary>
        public const double XLEN = 0.3;//(单位：mm)



        /// <summary>
        /// Maximum amount of data for 1 profile
        /// </summary>
        public const int MAX_PROFILE_COUNT = 3200;

        /// <summary>
        /// Device ID (fixed to 0)
        /// </summary>
        public const int DEVICE_ID = 0;

        /// <summary>
        /// Size of data for sending and getting settings
        /// </summary>
        public const int WRITE_DATA_SIZE = 20 * 1024;

        /// <summary>
        /// Upper limit for the size of data to get
        /// </summary>
        public const int READ_DATA_SIZE = 1024 * 1024;

        /// <summary>
        /// Maximum amount of profile data to retain
        /// </summary>
        public const int PROFILE_DATA_MAX = 10;

        /// <summary>
        /// Measurement range X direction
        /// </summary>
        public const int MEASURE_RANGE_FULL = 800;
        public const int MEASURE_RANGE_MIDDLE = 600;
        public const int MEASURE_RANGE_SMALL = 400;

        /// <summary>
        /// Light reception characteristic
        /// </summary>
        public const int RECEIVED_BINNING_OFF = 1;
        public const int RECEIVED_BINNING_ON = 2;

        public const int COMPRESS_X_OFF = 1;
        public const int COMPRESS_X_2 = 2;
        public const int COMPRESS_X_4 = 4;
        /// <summary>
        /// Default name to use when exporting profiles
        /// </summary>
        public const string DEFAULT_PROFILE_FILE_NAME = @"ReceiveData_CS.txt";

        /// <summary>
        /// Unit conversion factor (mm) for profile values
        /// </summary>
        public const double PROFILE_UNIT_MM = 1E-5;


        /// <summary>
        /// SingleProfile坐标轴最值
        /// </summary>
        public const int PROFILE_MIN_Y = 0;
        public const int PROFILE_MAX_Y = 600;
        public const int PROFILE_MIN_X = 0;
        public const int PROFILE_MAX_X = 800;
        public const int PROFILE_Interval_X = 100;//刻度线
        public const int PROFILE_Interval_Y = 100;
        /// <summary>
        /// 2D坐标轴最值
        /// </summary>
        public const int PROFILE2D_MIN_Y = 0;
        public const int PROFILE2D_MAX_Y = 800;
        public const int PROFILE2D_MIN_X = 0;
        public const int PROFILE2D_MAX_X = 1000;
        public const int PROFILE2D_Interval_X = 200;//刻度线
        public const int PROFILE2D_Interval_Y = 100;
        /// <summary>
        /// VerticalProfile坐标轴最值
        /// </summary>
        public const int VERTICAL_MIN_Y = 100;
        public const int VERTICAL_MAX_Y = 600;
        public const int VERTICAL_MIN_X = 0;
        public const int VERTICAL_MAX_X = 1000;
        public const int VERTICAL_Interval_X = 200;//刻度线
        public const int VERTICAL_Interval_Y= 100;

        //3D实时队列长度
        public const int QUEUE3D_LENGTH = 600;

        /// <summary>
        /// 无效数据区
        /// </summary>
        public const int INVALID_DATA = 21775;
        public const int INVALID_DATA_ORIGINAL = 0;

        /// <summary>
        /// chart图表参数
        /// </summary>
        public const int CSPH = 90;
        public const int CSPW = 95;
        public const int CSPX = 0;
        public const int CSPY = 10;
        public const int CTPX = 10;
        public const int CTPY = 5;



        #endregion
    }
}
