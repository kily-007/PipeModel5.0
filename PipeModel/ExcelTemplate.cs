using Microsoft.Office.Interop.Excel;
using System;

namespace PipeModel
{
    class ExcelTemplate
    {

        private object missing;
        public string filepath;
        private Workbook _workbook;
        private _Application _app;
        private _Worksheet workSheet;
        private Range range;

        /// <summary>
        /// 构造函数，初始化
        /// </summary>
        public ExcelTemplate()
        {
            _app = new Application();
            _workbook = _app.Workbooks.Add(true);
            workSheet = _workbook.ActiveSheet;
            missing = System.Reflection.Missing.Value;
            range = workSheet.get_Range("A1", missing);
            filepath = @"E:\\test3.xlsx";
        }


        public void writeExcelFile(object[,] data)
        {
           
            try
            {
                range = range.get_Resize(data.Rank, data.GetLength(0));
                range.set_Value(System.Reflection.Missing.Value, data);
                _app.Visible = true;
                _workbook.SaveAs(filepath, missing, missing, missing, missing, missing, XlSaveAsAccessMode.xlNoChange, missing, missing, missing);
                _workbook.Close(false, missing, missing);
                _app.Quit();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

        }



    }
}
