using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PipeModel
{
    class ReportViewer
    {

        public void writeWord(string path)
        {
            //标签数据初始化
            object[] oBookMarkLabel = new object[4] { "time_11", "class_11", "classANum_31", "classBNum_31" };//标签书签
            string[] oBookMarkLabelData = new string[4] { DateTime.Now.ToString("yyyy年MM月dd日 HH:mm:ss"),"A","2","16" };//标签对应的内容
            //表格数据初始化
            object[] oBookMarkTable = new object[5] { "allPipeATable_11", "allPipeBTable_11", "pipePartATable_21","pipePartBTable_21", "misaTable_31" };//表格书签
            string[] allPipeTableHead = new string[9] { "传感探头A" , "灌道编号" , "最小值" ,"最大值","平均值","方差","磨损值","磨损等级","备注"};
            string[] pipePartTableHead = new string[6]{ "传感头A","部位", "X方向错位\r\n(单位：mm)", "Y方向错位\r\n(单位：mm)", "Z方向错位\r\n(单位：mm)", "错位综合评估" };
            string[] pipeMisaTableHead = new string[6] { "局部磨损列表","序号", "部位描述", "横向磨损", "纵向磨损", "磨损等级" };

            string[][] dataPipe = MysqlConnection.executeQuery_data("select p_pipeNum,p_minYMiddle,p_maxYMiddle,p_avgYMiddle,p_varYMiddle,p_misa,evaluationLevel.e_misaClass,p_remarks from pipeline join evaluationLevel on pipeline.e_id=evaluationLevel.e_id");
            string[][] dataPipePart = MysqlConnection.executeQuery_data("select p_location,p_misaX,p_misaY,p_misaZ,evaluationLevel.e_misaClass from pipepart join evaluationLevel on pipepart.e_id=evaluationLevel.e_id");
            string[][] dataPipeMisa= MysqlConnection.executeQuery_data("select a_id,a_location,a_misaX,a_misaY,evaluationLevel.e_misaClass from allPipeMisa join evaluationLevel on allPipeMisa.e_id=evaluationLevel.e_id ORDER BY 'a_id' ASC");
            WordTemplate wordTemp = new WordTemplate();
            //填充标签
            wordTemp.insertDataToLabel(oBookMarkLabel, oBookMarkLabelData);
            //填充表格
            wordTemp.insertDataToTable(oBookMarkTable[0], allPipeTableHead, dataPipe,56);
            wordTemp.insertDataToTable(oBookMarkTable[2], pipePartTableHead, dataPipePart,84);
            wordTemp.insertDataToTable(oBookMarkTable[4], pipeMisaTableHead, dataPipeMisa, 84);
            //保存文档
            wordTemp.saveWordFile(path);
           
        }
        

        //生成报表数据
        public void makeReport()
        {
            //清除数据库表：pipeline,pipepart ,allPipeMisa
            MysqlConnection.truncateTableData("pipeline");
            MysqlConnection.truncateTableData("pipepart");
            MysqlConnection.truncateTableData("allPipeMisa");

            //查询每根灌道id起始及结束信息
            int[] m_id = MysqlConnection.executeQuery_id("select m_id from misaligned where m_misaZ='0'");
            List<int> aSingle_id = new List<int>();
            aSingle_id.Add(m_id[0]);
            for (int i = 0; i < m_id.Length-1; i++)
            {
                if (m_id[i + 1] - m_id[i] != 1)
                {
                    aSingle_id.Add(m_id[i]);
                    aSingle_id.Add(m_id[i + 1]);
                }
            }
            aSingle_id.Add(m_id[m_id.Length - 1]);


            //检测磨损评估表是否为空
            int[] caveatValue = MysqlConnection.executeQuery_misa("select e_id,e_misa from evaluationLevel where e_misaClass='良'");//警告的id与阈值
            int[] dangerValue = MysqlConnection.executeQuery_misa("select e_id,e_misa from evaluationLevel where e_misaClass='警告'");//危险的id与阈值
            if (caveatValue.Length == 0 || dangerValue.Length == 0)
            {
                MessageBox.Show("磨损等级表为空,请先生成磨损等级评估");
                //MysqlConnection.executeInsert("INSERT INTO evaluationLevel VALUES(NULL,2,'优'),VALUES(NULL,4,'良'),VALUES(NULL,6,'警告')VALUES(NULL,'n','危险')");
            }

            //根据id查询每根灌道的数据信息
            for (int i = 0; i < aSingle_id.Count-1; i+=2)
            {
                string strSql="select * from misaligned where m_id<='"+aSingle_id[i+1]+"' and m_id>="+ aSingle_id[i];
                string[][] aSingle_data = MysqlConnection.executeQuery_data(strSql);
                if (aSingle_data.Length > 0)
                {
                    //统计每根灌道整体情况特征值并入库
                    dealSingleChannel(aSingle_data, i/2+1);
                    //计算单根灌道xyz方向所有超过阈值的磨损并入库
                    dealSingleChannelMisa(aSingle_data, caveatValue, dangerValue, i);
                }
            }

            
            //计算管道之间缝隙xyz方向错位
            double misaX = 0;
            double misaY = 0;
            double misaZ = 0;
            double misaClass = 0;
            for (int i = 0; i < aSingle_id.Count-2; i+=2)
            {
                string[][] pipePart_data= MysqlConnection.executeQuery_data("select m_misaXLeft,m_misaYMiddle from misaligned where m_id='" + (aSingle_id[i+1]-10) + "' or m_id=" + (aSingle_id[i+2]+10));
                misaX = Math.Abs(int.Parse(pipePart_data[0][0]) - int.Parse(pipePart_data[1][0]))*0.3;
                misaY = Math.Abs(int.Parse(pipePart_data[0][1]) - int.Parse(pipePart_data[1][1]));
                misaZ = (aSingle_id[i + 2] - aSingle_id[i + 1])*0.6;
                if (misaX == 0)
                    misaClass = 1;
                else
                    misaClass= MysqlConnection.executeQuery_id("select e_id from evaluationLevel where e_misa<" + misaX).Max();//查询磨损值所在等级
                double []misa = new double[4] { misaX, misaY, misaZ, misaClass };
                MysqlConnection.executeInsert(misa, i/2+1);

            }
            if(MainForms._binFileDataTime!="")
                MysqlConnection.executeInsert("update historyData set h_sqlRecordPath='" + (MainForms._defaultSavePath + "data\\sql\\SQL-" + MainForms._binFileDataTime + ".txt").Replace("\\", "\\\\")+ "' where h_id="+MysqlConnection.executeQuery_id("select h_id from historyData order by h_id DESC limit 1")[0]);
           
        }



        /// <summary>
        /// 统计单根灌道整体情况，计算每根灌道特征值并入库
        /// </summary>
        /// <param name="data"></param>
        public void dealSingleChannel(string[][] data, int pipeNum)
        {
            double[] Eigenvalue = new double[23];
            int min = 0, max = 0;
            double avg = 0, var = 0;
            int p = 0;
            int tempMiddleMin = 0;
            int tempMiddleMax = 0;
            //计算单根灌道每列特征值
            for (int j = 3; j < 8; j++)
            {
                //根据需求更改，在此仅计算y方向的磨损度
                min = int.Parse(data[0][j]);
                max = int.Parse(data[0][j]);
                avg = 0;
                //计算最大值、最小值、均值
                for (int i = 0; i < data.Length; i++)
                {
                    if (int.Parse(data[i][j]) < min)
                        min = int.Parse(data[i][j]);
                    else if (int.Parse(data[i][j]) > max)
                        max = int.Parse(data[i][j]);
                    avg += int.Parse(data[i][j]);

                }
                avg /= data.Length;
                avg = Math.Round(avg, 2);
                //方差
                for (int i = 0; i < data.Length; i++)
                {
                    var += Math.Pow(avg - int.Parse(data[i][j]), 2);
                }
                var /= data.Length;
                var = Math.Round(var, 2);
                Eigenvalue[p++] = min;
                Eigenvalue[p++] = max;
                Eigenvalue[p++] = avg;
                Eigenvalue[p++] = var;
                if (j == 6)//仅计算y方向磨损
                {
                    tempMiddleMax = max;
                    tempMiddleMin = min;
                }
            }
            int misa = tempMiddleMax - tempMiddleMin;//定义磨损值
            Eigenvalue[p++] = misa;
            if (misa != 0)
                Eigenvalue[p++] = MysqlConnection.executeQuery_id("select e_id from evaluationLevel where e_misa<" + misa).Max();
            else
                Eigenvalue[p++] = 1;
            Eigenvalue[p++] = -1;
            MysqlConnection.executeInsert(Eigenvalue, data[0][2], pipeNum);

        }




        /// <summary>
        /// 计算单根灌道xy方向所有超过阈值的磨损
        /// </summary>
        /// <param name="aSingle_data"></param>
        /// <param name="caveatValue"></param>
        /// <param name="dangerValue"></param>
        /// <param name="i"></param>
        private void dealSingleChannelMisa(string[][] aSingle_data, int[] caveatValue, int[] dangerValue,int i)
        {
            List<string[]> aPipeLineAllMisaList = new List<string[]>();
            for (int j = 0; j < aSingle_data.Length - 1; j+=2)
            {
                string[] misaStr = new string[4];
                int misaValueY = Math.Abs(((int.Parse(aSingle_data[j + 1][5]) + int.Parse(aSingle_data[j][7])) / 2) - int.Parse(aSingle_data[j][6]));
                if (misaValueY > caveatValue[1])//警告||危险
                {
                    misaStr[0] = "S" + (i / 2 + 1).ToString().PadLeft(2, '0') + "：" + (j * 0.6 / 10) + "cm";//灌道号
                    misaStr[1] = "0";
                    misaStr[2] = misaValueY.ToString();
                    if ( misaValueY > dangerValue[1])
                        misaStr[3] = (dangerValue[0] + 1).ToString();
                    else
                        misaStr[3] = (caveatValue[0] + 1).ToString();
                    aPipeLineAllMisaList.Add(misaStr);
                }

                //int misaValueX = Math.Abs(int.Parse(aSingle_data[j + 1][3]) - int.Parse(aSingle_data[j][3]));
                //int misaValueY = Math.Abs(((int.Parse(aSingle_data[j + 1][5]) + int.Parse(aSingle_data[j][7]))/2)- int.Parse(aSingle_data[j][6]));
                //if (misaValueX > caveatValue[1] || misaValueY > caveatValue[1])//警告||危险
                //{
                //    misaStr[0] = "S" + (i / 2 + 1).ToString().PadLeft(2, '0') + "：" + (j * 0.6 / 10) + "cm";//灌道号
                //    misaStr[1] = misaValueX.ToString();
                //    misaStr[2] = misaValueY.ToString();
                //    if (misaValueX > dangerValue[1] || misaValueY > dangerValue[1])
                //        misaStr[3] = (dangerValue[0] + 1).ToString();
                //    else
                //        misaStr[3] = (caveatValue[0] + 1).ToString();
                //    aPipeLineAllMisaList.Add(misaStr);
                //}

            }
            if (aPipeLineAllMisaList.Count > 0)
                MysqlConnection.executeInsert_misaData(aPipeLineAllMisaList);
        }

        
    }
}
