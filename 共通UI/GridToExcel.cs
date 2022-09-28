using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace 共通UI
{
    internal class GridToExcel
    {
        private DataGridView dataGridView;
        private List<Object> fullDataSource;

        public GridToExcel(DataGridView dataGridView, List<Object> fullDataSource)
        {
            this.dataGridView = dataGridView;
            this.fullDataSource = fullDataSource;
        }

        // -----------------------------------------------------
        // Excel 出力
        // -----------------------------------------------------
        public void SendToExcel()
        {
            if (dataGridView.Rows.Count == 0) return;

            //Excelオブジェクトの初期化
            Excel.Application ExcelApp = null;
            Excel.Workbooks exCurrentBooks = null;
            Excel.Workbook exCurrentBook = null;
            Excel.Worksheet exCurrentSheet = null;

            try
            {
                //Excelシートのインスタンスを作る
                ExcelApp = new Excel.Application();

                exCurrentBooks = ExcelApp.Workbooks;
                exCurrentBook = ExcelApp.Workbooks.Add();
                exCurrentSheet = exCurrentBook.Worksheets[1];
                exCurrentSheet.Select(Type.Missing);

                // ヘッダー
                SetExcelHeader(exCurrentSheet);

                // リスト
                SetExcelList(exCurrentSheet);

                // 書式
                SetExcelFormat(exCurrentSheet);

            }

            finally
            {

                exCurrentSheet.Cells.Columns.AutoFit();

                //excel表示
                ExcelApp.Visible = true;

                // Excelオブジェクトの開放
                Marshal.ReleaseComObject(exCurrentSheet);
                exCurrentSheet = null;
                Marshal.ReleaseComObject(exCurrentBook);
                exCurrentBook = null;
                Marshal.ReleaseComObject(exCurrentBooks);
                exCurrentBooks = null;

                // アプリケーションの終了前にガベージ コレクトを強制。
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();

                // Application オブジェクトを破棄。
                Marshal.ReleaseComObject(ExcelApp);
                ExcelApp = null;

                // Application オブジェクトのガベージ コレクトを強制。
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
            }

        }


        // -----------------------------------------------------
        // ヘッダー
        // -----------------------------------------------------
        private void SetExcelHeader(Excel.Worksheet CurrentSheet)
        {
            int cols = dataGridView.Columns.Count;
            string[] Header = new string[cols];

            int col = 0;
            foreach (DataGridViewColumn column in dataGridView.Columns)
            {
                Header[col] = column.HeaderText;
                col++;
            }

            Excel.Range rangeHeader = (Excel.Range)CurrentSheet.Cells[1, 1];
            rangeHeader = rangeHeader.get_Resize(1, cols);

            rangeHeader.Value = Header;
            rangeHeader.Interior.Color = Color.DarkBlue;
            rangeHeader.Font.Color = Color.White;
            rangeHeader.Font.Bold = true;
        }

        // -----------------------------------------------------
        // リスト
        //  ・大量データをEXCELへ転送する可能性があるので、
        //  　１セルずつちまちま転送していては遅い。
        //  　２次元配列へデータを加工し、まとめて１回で転送する。
        //  ・リソースはdataGridViewだけにしたいが、グリッドには
        //  　全件のデータは無い。表示中のページのみ。
        //  　なので、DataSourceは必須。
        // -----------------------------------------------------
        private void SetExcelList(Excel.Worksheet CurrentSheet)
        {
            int rows = fullDataSource.Count;
            int cols = dataGridView.Columns.Count;

            object[,] ListToExcel = new object[rows, cols];


            Type myType = fullDataSource[0].GetType();
            IList<PropertyInfo> props = new List<PropertyInfo>(myType.GetProperties());

            int row = 0;
            foreach (object ds in fullDataSource)
            {
                int col = 0;

                foreach (PropertyInfo prop in props)
                {
                    object propValue = prop.GetValue(ds, null);
                    ListToExcel[row, col] = propValue;
                    col++;
                }
                row++;
            }

            Excel.Range range = CurrentSheet.Cells[2, 1];
            range = range.get_Resize(rows, cols);
            range.Value = ListToExcel;
        }

        // -----------------------------------------------------
        // 書式
        // -----------------------------------------------------
        private void SetExcelFormat(Excel.Worksheet CurrentSheet)
        {

            int cols = dataGridView.Columns.Count;

            Excel.Range range = CurrentSheet.UsedRange;
            range.NumberFormatLocal = "0";

            // Z列を超える一覧は想定しない
            if (cols > 26)
            {
                throw new Exception("列数が多すぎる。EXCELへの転送は26列までしか想定していない。");
            }

            int c = 0;

            foreach (DataGridViewColumn column in dataGridView.Columns)
            {
                char currentLetter = (char)(c + 65);

                if (column.DefaultCellStyle.Format == "MM/dd")
                {
                    Excel.Range range1 = CurrentSheet.get_Range(currentLetter + ":" + currentLetter);
                    range1.NumberFormatLocal = "yyyy/MM/dd";
                }

                if (column.DefaultCellStyle.Format == "C")
                {
                    Excel.Range range2 = CurrentSheet.get_Range(currentLetter + ":" + currentLetter);
                    range2.NumberFormatLocal = "\\ #,##0";
                }
                c++;
            }
        }
    }
}
