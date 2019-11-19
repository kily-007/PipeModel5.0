using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data;
using System.IO;
using System.Windows.Forms;

namespace PipeModel
{
    public class MysqlConnection
    {
        // server=127.0.0.1/localhost 代表本机，端口号port默认是3306可以不写
        private static string connetStr = "server=127.0.0.1;port=3306;user=root;password=root;database=pipemodel;";

        /// <summary>
        /// 
        ///插入
        /// </summary>
        /// <param name="data"></param>
        /// <param name="table"></param>
        /// <returns></returns>
        public static int executeInsert(List<int[]> xList, List<int[]> yList, List<int> zList, string location)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connetStr))
                {
                    connection.Open();
                    StringBuilder strSql = new StringBuilder();
                    strSql.Append("insert into misaligned values (null,'").Append(location + "','").Append(GetTimeStamp())
                        .Append("',").Append(xList[0][0]).Append(",").Append(xList[0][1])
                        .Append(",").Append(yList[0][0]).Append(",").Append(yList[0][1]).Append(",").Append(yList[0][2])
                        .Append(",").Append(zList[0]).Append(")");

                    for (int i = 1; i < xList.Count; i++)
                    {
                        strSql.Append(",(null,'").Append(location + "','").Append(GetTimeStamp())
                        .Append("',").Append(xList[i][0]).Append(",").Append(xList[i][1])
                        .Append(",").Append(yList[i][0]).Append(",").Append(yList[i][1]).Append(",").Append(yList[i][2])
                        .Append(",").Append(zList[i]).Append(")");
                    }

                    DataExporter.WriteTxt(strSql.Append(";").ToString(), MainForms._defaultSavePath + "data\\sql\\SQL-" + MainForms._binFileDataTime + ".txt");
                    using (MySqlCommand command = new MySqlCommand(strSql.ToString(), connection))
                    {
                        command.ExecuteNonQuery();
                    }
                    return xList.Count;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
                
            }

        }



        /// <summary>
        /// 
        ///生成报表时，将统计值插入数据库
        /// </summary>
        /// <param name="data"></param>
        /// <param name="table"></param>
        /// <returns></returns>
        public static int executeInsert(double []data,string time,int pipeNum)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connetStr))
                {
                    connection.Open();
                    StringBuilder strSql = new StringBuilder("insert into pipeline values (null,'S" + pipeNum.ToString().PadLeft(2, '0') +"','"+time+"'");
                    for (int i = 0; i < data.Length; i++)
                    {
                        strSql.Append(",").Append(data[i]);
                    }
                    strSql.Append(")");
                    using (MySqlCommand command = new MySqlCommand(strSql.ToString(), connection))
                    {
                        command.ExecuteNonQuery();
                    }
                    return data.Length;
                }
            }
            catch (Exception w)
            {
                throw new Exception(w.Message);
            }

        }



        /// <summary>
        /// 
        ///生成报表时，计算灌道之间缝隙的xyz方向错位值，将统计值插入数据库
        /// </summary>
        /// <param name="data"></param>
        /// <param name="table"></param>
        /// <returns></returns>
        public static int executeInsert(double[] data, int pipeNum)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connetStr))
                {
                    connection.Open();
                    StringBuilder strSql = new StringBuilder("insert into pipepart values (null,'S" + pipeNum.ToString().PadLeft(2, '0') +"-S"+ (pipeNum+1).ToString().PadLeft(2, '0')+"'");
                    for (int i = 0; i < data.Length; i++)
                    {
                        strSql.Append(",").Append(Math.Round(data[i], 2));//保留2位小数
                    }
                    strSql.Append(")");
                    using (MySqlCommand command = new MySqlCommand(strSql.ToString(), connection))
                    {
                        command.ExecuteNonQuery();
                    }
                    return data.Length;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }



        /// <summary>
        /// 插入每根灌道中，计算出的磨损位置和值
        /// </summary>
        /// <param name="data"></param>
        public static int executeInsert_misaData(List<string[]> data)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connetStr))
                {
                    connection.Open();
                    StringBuilder strSql = new StringBuilder("insert into allpipemisa values (null, '" + data[0][0] + "', " + data[0][1] + ", " + data[0][2] + ", " + data[0][3]+")");
                    for (int i = 1; i < data.Count; i++)
                    {
                        strSql.Append(",(null, '").Append(data[i][0]).Append("', ").Append(data[i][1]).Append(", ").Append(data[i][2]).Append(", ").Append(data[i][3]).Append(")");
                    }
                    using (MySqlCommand command = new MySqlCommand(strSql.ToString(), connection))
                    {
                        command.ExecuteNonQuery();
                    }
                    return data.Count;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }


        /// <summary>
        /// 自动填充磨损等级表
        /// </summary>
        /// <param name="data"></param>
        public static void executeInsert(string sql)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connetStr))
                {
                    connection.Open();
                    using (MySqlCommand command = new MySqlCommand(sql, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }


        /// <summary>
        /// Test测试函数，将SQL.txt数据导入数据库
        /// </summary>
        public static void executeInsert()
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connetStr))
                {
                    connection.Open();
                    StreamReader file = new StreamReader("E:\\SQLTest.txt");
                    string line;
                    while ((line = file.ReadLine()) != null)
                    {
                        using (MySqlCommand command = new MySqlCommand(line, connection))
                        {
                            command.ExecuteNonQuery();
                        }
                    }
                    file.Close();
                    Console.ReadLine();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }


        /// <summary>
        /// 删除数据记录
        /// </summary>
        /// <param name="data"></param>
        public static void executeDelete(string sql)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connetStr))
                {
                    connection.Open();
                    using (MySqlCommand command = new MySqlCommand(sql, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }






        /// <summary>
        /// 查询id
        /// </summary>
        /// <param name="SQLString"></param>
        /// <returns></returns>
        public static int[] executeQuery_id(string sqlstr)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connetStr))
                {
                    connection.Open();
                    DataSet ds = new DataSet();
                    using (MySqlDataAdapter command = new MySqlDataAdapter(sqlstr, connection))
                    {
                        command.Fill(ds, "ds");
                    }
                    int[] m_id = new int[ds.Tables[0].Rows.Count];
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        m_id[i] = Convert.ToInt32(ds.Tables[0].Rows[i][0]);
                    }
                    return m_id;
                }
            }
            catch (MySqlException ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 查询磨损级别对应的值
        /// </summary>
        /// <param name="SQLString"></param>
        /// <returns></returns>
        public static int[] executeQuery_misa(string sqlstr)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connetStr))
                {
                    connection.Open();
                    DataSet ds = new DataSet();
                    using (MySqlDataAdapter command = new MySqlDataAdapter(sqlstr, connection))
                    {
                        command.Fill(ds, "ds");
                    }

                    int[] m_misa = new int[2];
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Columns.Count; i++)
                        {
                            m_misa[i] = Convert.ToInt32(ds.Tables[0].Rows[0][i]);
                        }
                    }
                    return m_misa;
                }
            }
            catch (MySqlException ex)
            {
                throw new Exception(ex.Message);

            }
        }



        /// <summary>
        /// 查询数据库中表行数
        /// </summary>
        /// <param name="sqlstr"></param>
        /// <returns></returns>
        public static int executeQuery_Count(string sqlstr)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connetStr))
                {
                    connection.Open();
                    using (MySqlCommand command = new MySqlCommand(sqlstr, connection))
                    {
                        return Convert.ToInt32(command.ExecuteScalar());
                    }
                }
            }
            catch (MySqlException ex)
            {
                return -1;
                throw new Exception(ex.Message);

            }
        }


        /// <summary>
        /// 清空数据库中指定表
        /// </summary>
        /// <param name="tableName"></param>
        public static void truncateTableData(string tableName)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connetStr))
                {
                    connection.Open();
                    using (MySqlCommand command = new MySqlCommand("TRUNCATE TABLE "+tableName, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }



        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="SQLString"></param>
        /// <returns></returns>
        public static string[][] executeQuery_data(string sqlstr)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connetStr))
                {
                    DataSet ds = new DataSet();
                    using (MySqlDataAdapter command = new MySqlDataAdapter(sqlstr, connection))
                    {
                        command.Fill(ds, "ds");
                    }
                    string[][] str = new string[ds.Tables[0].Rows.Count][];
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        str[i] = new string[ds.Tables[0].Columns.Count];
                        for (int j =0; j < ds.Tables[0].Columns.Count; j++)
                        {
                            str[i][j] = ds.Tables[0].Rows[i][j].ToString();
                        }
                    }
                    return str;
                }
            }
            catch (MySqlException ex)
            {

                throw new Exception(ex.Message);
            }
        }


        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="SQLString"></param>
        /// <returns></returns>
        public static object[,] executeQuery_2data(string sqlstr)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connetStr))
                {
                    DataSet ds = new DataSet();
                    using (MySqlDataAdapter command = new MySqlDataAdapter(sqlstr, connection))
                    {
                        command.Fill(ds, "ds");
                    }
                    object[,] str = new object[ds.Tables[0].Rows.Count, ds.Tables[0].Columns.Count];
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        for (int j = 0; j < ds.Tables[0].Columns.Count; j++)
                        {
                            str[i,j] = ds.Tables[0].Rows[i][j].ToString();
                        }
                    }
                    return str;
                }
            }
            catch (MySqlException ex)
            {

                throw new Exception(ex.Message);
            }
        }


        /// <summary> 
        /// 获取时间戳 
        /// </summary> 
        /// <returns></returns> 
        public static string GetTimeStamp()
        {
            return DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss:fff");
        }


        /// <summary>
        /// 写txt
        /// </summary>
        /// <param name="datas"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        static public bool WriteStrTxt(string str, string path)
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
                            sw.Write("{0}", str, DateTime.Now);
                            sw.Flush();
                        }
                        finally
                        {
                            sw.Close();
                            fs.Close();
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                // File save failure
                throw new Exception(ex.Message);
            }

            return true;
        }
    }
}
