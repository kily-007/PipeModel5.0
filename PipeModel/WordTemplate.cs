using Microsoft.Office.Interop.Word;
using System;
using System.IO;
using System.Windows.Forms;

namespace PipeModel
{
    class WordTemplate
    {
        private object missing;
        private _Application oWord;
        public object templatePath;//模板路径
        private _Document oDoc;
        public string wordName;
        private UInt32 R = 0x1, G = 0x100, B = 0x10000;
        public static object filename;

        /// <summary>
        /// 构造函数，初始化
        /// </summary>
        public WordTemplate()
        {
            missing = System.Reflection.Missing.Value;
            oWord= new Microsoft.Office.Interop.Word.Application();
            string str = MainForms._defaultSavePath+ "PipeModel\\WordTemplate\\Result.docx";
            templatePath = str;
            oDoc = oWord.Documents.Add(ref templatePath, ref missing, ref missing, ref missing);
            wordName = "测量报表";
                                                                                                                                                                                                     
        }

       /// <summary>
       /// 生成表格
       /// </summary>
       /// <param name="bookMarkTable"></param>
       /// <param name="row"></param>
       /// <param name="col"></param>
        public void insertDataToTable(object bookMarkTable,string[] tableHead,string[][] data,int tableWidth)
        {
            if (data.Length <= 0) return;
            int row = data.Length+2;
            int col = data[0].Length;
            Range range = oDoc.Bookmarks.get_Item(ref bookMarkTable).Range;//表格插入位置      
            Table allPipeATable = oDoc.Tables.Add(range, row, col, ref missing, ref missing);//开辟一个row行col列的表格
            allPipeATable.Range.Font.Size = 10.5F;//字体大小
            //添加表格边框
            setTableBorderStyle(allPipeATable, WdBorderType.wdBorderLeft);
            setTableBorderStyle(allPipeATable, WdBorderType.wdBorderBottom);
            setTableBorderStyle(allPipeATable, WdBorderType.wdBorderRight);
            setTableBorderStyle(allPipeATable, WdBorderType.wdBorderTop);
            setTableBorderStyle(allPipeATable, WdBorderType.wdBorderVertical);
            setTableBorderStyle(allPipeATable, WdBorderType.wdBorderHorizontal);
            //表格宽度
            for (int i = 1; i <= col; i++)
            {
                allPipeATable.Columns[i].Width = tableWidth;
            }
            //表格高度与颜色设置
            for (int i = 1; i <= row; i++)
            {
                allPipeATable.Rows[i].Height = 16;
                if(i%2==0)
                    allPipeATable.Rows[i].Range.Shading.BackgroundPatternColor=(WdColor)(R * 222 + G * 234 + B * 246);

            }
            //文字居中,文字黑色
            allPipeATable.Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            allPipeATable.Range.Cells.VerticalAlignment = WdCellVerticalAlignment.wdCellAlignVerticalCenter;
            allPipeATable.Range.Paragraphs.SpaceAfter = 0f;
            allPipeATable.Rows.Alignment = WdRowAlignment.wdAlignRowCenter;
            allPipeATable.Range.Font.Color = WdColor.wdColorBlack;
            //表头内容
            allPipeATable.Cell(1, 1).Merge(allPipeATable.Cell(1, col));//合并表头
            allPipeATable.Cell(1, 1).Range.Shading.BackgroundPatternColor= (WdColor)(R * 91 + G * 155 + B * 213);
            allPipeATable.Cell(1, 1).Range.Font.Color = WdColor.wdColorWhite;
            allPipeATable.Cell(1, 1).Range.Font.Bold = 1;
            allPipeATable.Cell(1, 1).Range.Font.Name = "等线";
            allPipeATable.Cell(1, 1).Range.Text = tableHead[0];
            for (int i = 1; i < tableHead.Length; i++)
            {
                allPipeATable.Cell(2, i).Range.Text = tableHead[i];
            }
            //填充表格数据
            for (int i = 1; i <= row-2; i++)
            {
                for (int j = 1; j <= col; j++)
                {
                    allPipeATable.Cell(i+2, j).Range.Text = data[i-1][j-1];
                }
            }
            
        }

        public void setTableBorderStyle(Table allPipeATable, WdBorderType bord)
        {
            allPipeATable.Borders[bord].Visible = true;
            allPipeATable.Borders[bord].Color = (WdColor)(R * 156 + G * 194 + B * 229);
            allPipeATable.Borders[bord].LineWidth = WdLineWidth.wdLineWidth050pt;
        }



        /// <summary>
        /// 填充标签
        /// </summary>
        public void insertDataToLabel(object[] oBookMarkLabel, string[] data)
        {
            for (int i = 0; i < oBookMarkLabel.Length; i++)
            {
                oDoc.Bookmarks.get_Item(ref oBookMarkLabel[i]).Range.Text = data[i];
            }
        }


        /// <summary>
        /// 保存word文档
        /// </summary>
        public void saveWordFile(string reportPath)
        {
            //object filename;
            filename = (object)(reportPath);
            try
            {
                oDoc.SaveAs(ref filename, ref missing, ref missing, ref missing,
               ref missing, ref missing, ref missing, ref missing, ref missing,
               ref missing, ref missing, ref missing, ref missing, ref missing,
               ref missing, ref missing);
                oDoc.Close(ref missing, ref missing, ref missing);
                oWord.Quit(ref missing, ref missing, ref missing);

                FileInfo fi = new FileInfo(filename.ToString());
                string size = (fi.Length / 1024.00).ToString("f2");
                if (double.Parse(size) > 1024)//文件大小超过1G,转换单位
                    size = (double.Parse(size) / 1024.00).ToString() + "M";
                else
                    size = size + "KB";
                
                //插入报表记录
                string[] path = filename.ToString().Split('\\');
                string sqlStr = "UPDATE historyData set h_reportFileName='" + path[path.Length - 1] + "',h_reportFilePath='" + filename.ToString().ToString().Replace("\\", "\\\\") + "',h_reportFileSize='" + size + "' where h_dataFileName='" + path[path.Length-1].Substring(4,15)+"bin'";
                MysqlConnection.executeInsert(sqlStr);


            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            
        }



    }
}
