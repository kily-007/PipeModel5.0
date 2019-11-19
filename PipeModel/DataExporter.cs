//----------------------------------------------------------------------------- 
// <copyright file="DataExporter.cs" company="KEYENCE">
//	 Copyright (c) 2013 KEYENCE CORPORATION.  All rights reserved.
// </copyright>
//----------------------------------------------------------------------------- 
using System;
using System.IO;
using System.Text;
using PipeModel.Datas;
using System.Collections.Generic;

namespace PipeModel
{
    public static class DataExporter
    {
        #region Method




        /// <summary>
        /// Measurement value output
        /// </summary>
        /// <param name="datas">Measurement data</param>
        /// <param name="fileName">File name</param>
        /// <returns></returns>
        public static bool ExportText(string[][] data, string path)
        {
            try
            {
                Encoding unicode = System.Text.Encoding.GetEncoding("utf-16");
                using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    using (StreamWriter sw = new StreamWriter(fs))
                    {
                        try
                        {
                            StringBuilder str = new StringBuilder();
                            str.Append("loca           time                  XL  XR   Y   YM  YR  Z id").Append("\r\n");
                            for (int i = 0; i < data.Length; i++)
                            {
                                for (int j = 1; j < data[i].Length; j++)
                                {
                                    str.Append(data[i][j]).Append(" ");
                                }
                                str.Append(data[i][0]);

                                str.Append("\r\n");
                            }
                            sw.WriteLine("{0}", str, DateTime.Now);
                            sw.Flush();
                        }
                        finally
                        {
                            sw.Close();
                        }

                    }
                }

            }
            catch (Exception ex)
            {
                // File save failure
                System.Diagnostics.Debug.WriteLine(ex.Message);
                System.Diagnostics.Debug.Assert(false);
                return false;
            }

            return true;
        }












        /// <summary>
        /// Profile output
        /// </summary>
        /// <param name="datas">Profile data</param>
        /// <param name="profileNo">Profile information</param>
        /// <param name="fileName">File name</param>
        /// <returns></returns>
        static public bool ExportOneTxtProfile(ProfileData[] datas, string path)
        {
            try
            {
                Encoding unicode = System.Text.Encoding.GetEncoding("utf-16");
                using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    using (StreamWriter sw = new StreamWriter(fs))
                    {
                        try
                        {
                            string result = string.Join(" ", datas[0].ProfDatas);
                            sw.BaseStream.Seek(0, SeekOrigin.End);
                            sw.WriteLine("{0}", result, DateTime.Now);
                            sw.Flush();
                        }
                        finally
                        {
                            sw.Close();
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                // File save failure
                System.Diagnostics.Debug.WriteLine(ex.Message);
                System.Diagnostics.Debug.Assert(false);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Measurement value output
        /// </summary>
        /// <param name="datas">Measurement data</param>
        /// <param name="fileName">File name</param>
        /// <returns></returns>
        static public bool ExportHeightSpeedTxtData(List<int[]> datas, string path)
        {
            try
            {
                Encoding unicode = System.Text.Encoding.GetEncoding("utf-16");
                using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    using (StreamWriter sw = new StreamWriter(fs))
                    {
                        try
                        {
                            StringBuilder str = new StringBuilder();
                            for (int i = 0; i < datas.Count; i++)
                            {
                                str.Append(string.Join(" ", datas[i]));
                                str.Append("\r\n");
                            }
                            sw.BaseStream.Seek(0, SeekOrigin.End);
                            sw.WriteLine("{0}", str, DateTime.Now);
                            sw.Flush();
                        }
                        finally
                        {
                            sw.Close();
                        }

                    }
                }

            }
            catch (Exception ex)
            {
                // File save failure
                System.Diagnostics.Debug.WriteLine(ex.Message);
                System.Diagnostics.Debug.Assert(false);
                return false;
            }

            return true;
        }
        


        /// <summary>
        /// Measurement value output
        /// </summary>
        /// <param name="datas">Measurement data</param>
        /// <param name="fileName">File name</param>
        /// <returns></returns>
        static public bool WriteTxt(string str , string path)
        {
            try
            {
                Encoding unicode = System.Text.Encoding.GetEncoding("utf-16");
                using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    using (StreamWriter sw = new StreamWriter(fs))
                    {
                        try
                        {
                            sw.BaseStream.Seek(0, SeekOrigin.End);
                            sw.WriteLine("{0}", str, DateTime.Now);
                            sw.Flush();
                        }
                        finally
                        {
                            sw.Close();
                        }

                    }
                }

            }
            catch (Exception ex)
            {
                // File save failure
                System.Diagnostics.Debug.WriteLine(ex.Message);
                System.Diagnostics.Debug.Assert(false);
                return false;
            }

            return true;
        }





        /// <summary>
		/// Measurement value output
		/// </summary>
		/// <param name="datas">Measurement data</param>
		/// <param name="fileName">File name</param>
		/// <returns></returns>
		static public bool ExportHeightSpeedBinData(List<int[]> datas, string path)
        {
            if (datas.Count <= 0)
                return false ;
            try
            {
                Encoding unicode = System.Text.Encoding.GetEncoding("utf-16");
                using (FileStream fs = new FileStream(path, FileMode.Append, FileAccess.Write))
                {
                    using (BinaryWriter bw = new BinaryWriter(fs))
                    {
                        try
                        {
                            for (int i = 0; i < datas.Count; i++)
                            {
                                bw.Write(IntArrayToByteArray(datas[i])); //写文件
                            }
                            bw.Close();
                        }
                        finally
                        {
                            fs.Close();
                        }

                    }
                }

            }
            catch (Exception ex)
            {
                // File save failure
                System.Diagnostics.Debug.WriteLine(ex.Message);
                System.Diagnostics.Debug.Assert(false);
                return false;
            }

            return true;
        }


        /// <summary>
        /// int[] 转 byte[]
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        static private byte[] IntArrayToByteArray(int[] array)
        {
            byte[] values = new byte[array.Length * 4];
            int temp = 0;
            for (int i = 0; i < array.Length; i++)
            {
                values[temp + 3] = (byte)((array[i] >> 24) & 0xFF);
                values[temp + 2] = (byte)((array[i] >> 16) & 0xFF);
                values[temp + 1] = (byte)((array[i] >> 8) & 0xFF);
                values[temp] = (byte)(array[i] & 0xFF);
                temp += 4;
            }
            return values;
        }


        #endregion
    }
}
