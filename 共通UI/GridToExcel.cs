using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace 共通UI
{
    internal class GridToExcel
    {

        // -----------------------------------------------------
        // コンストラクタ
        // -----------------------------------------------------
        private DataGridView dataGridView;

        /// <summary>
        /// DataGridViewからExcelへ転送する。
        /// </summary>
        public GridToExcel(DataGridView dataGridView)
        {
            this.dataGridView = dataGridView;
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

                // EXCELオブジェクトのガベージ コレクトを強制。
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
        //
        // -----------------------------------------------------
        private void SetExcelList(Excel.Worksheet CurrentSheet)
        {
            // dataGridViewからデータソースを取得する。
            BindingSource bs = (BindingSource)dataGridView.DataSource;
            var fullDataSource = (List<Object>)bs.DataSource;

            // 行数
            int rows = fullDataSource.Count;

            // 0行ならなにもしない
            if (rows == 0) return;

            // 列数
            IList<PropertyInfo> props = new List<PropertyInfo>(
                fullDataSource[0].GetType().GetProperties());
            int cols = props.Count;

            // 一回で転送するための２次元オブジェクト
            object[,] ListToExcel = new object[rows, cols];

            int row = 0;
            foreach (object rowItem in fullDataSource)
            {
                int col = 0;
                foreach (PropertyInfo prop in props)
                {
                    ListToExcel[row, col] = prop.GetValue(rowItem, null);
                    col++;
                }
                row++;
            }

            // 出力先を指定して転送
            Excel.Range range = CurrentSheet.Cells[2, 1];
            range = range.get_Resize(rows, cols);
            range.Value = ListToExcel;
        }

        // -----------------------------------------------------
        // 書式
        // -----------------------------------------------------
        private void SetExcelFormat(Excel.Worksheet CurrentSheet)
        {

            int col = 0;
            foreach (DataGridViewColumn column in dataGridView.Columns)
            {
                string currentLetter = IntToChars(col);

                Excel.Range range = CurrentSheet.get_Range(currentLetter + ":" + currentLetter);

                if (column.DefaultCellStyle.Format == "MM/dd" || column.DefaultCellStyle.Format == "yyyy/MM/dd")
                {
                    // グリッド上での日付を年月日表示
                    range.NumberFormatLocal = "yyyy/MM/dd";
                }
                else if (column.DefaultCellStyle.Format == "C")
                {
                    // グリッド上での通貨を円で表示
                    range.NumberFormatLocal = "\\ #,##0";
                }
                else
                {
                    // バーコードの様な長い数字を省略せずに表示する
                    range.NumberFormatLocal = "0";

                }
                col++;
            }
        }

        // 列番号の数値をEXCELの列番号（A列とか）へ変換（A列～ZZ列までの範囲）
        private string IntToChars(int value)
        {
            int i上1桁 = (int)Math.Floor(value / 26.0) - 1;
            int i下1桁 = (int)(value % 26.0);

            string c上1桁 = "";
            if (i上1桁 >= 0 )
            {
                ((char)(i上1桁 + 65)).ToString();
            }
            string c下1桁 = ((char)(i下1桁 + 65)).ToString();

            return c上1桁 + c下1桁;
        }

    }
}
