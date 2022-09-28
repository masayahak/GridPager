using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace 共通UI
{
    public partial class UcGridPager : UserControl
    {
        // -------------------------------------------------------
        // 公開プロパティ
        // -------------------------------------------------------

        // 外部からDataGridViewを直接参照用
        public DataGridView pagerDataGridView
        {
            get
            {
                return dataGridView;
            }
        }

        // こちらに全ページ分のDataSourceをセット
        private List<Object> fullDataSource;

        public void SetFullDatasource<T>(List<T> list)
        {
            fullDataSource = list.Cast<Object>().ToList();
            SetMaxRowCount(fullDataSource.Count);
        }

        private int rowsInPage = 1000;
        /// <summary>
        /// １ページに何行表示するか。デフォルト1000行／ページ
        /// </summary>
        public int RowsInPage
        {
            get { return rowsInPage; }
            set
            {
                rowsInPage = value;
            }
        }

        // -------------------------------------------------------
        // プロパティ
        // -------------------------------------------------------
        // 一覧に表示するリストの最大行数
        private int maxRowCount;
        private void SetMaxRowCount(int value)
        {
            maxRowCount = value;
            this.lblMaxCount.Text = maxRowCount.ToString();

            // 件数が変わった＝初期化された。
            SetCurrentCount(1);
        }

        // 現在表示している行範囲の先頭
        private int currentCount;
        private void SetCurrentCount(int value)
        {
            currentCount = value;

            int end;
            if (value + RowsInPage < maxRowCount)
                end = value + RowsInPage - 1;
            else
                end = maxRowCount;

            if (maxRowCount == 0)
            {
                this.lblCurrentCount.Text = "0";
            }
            else
            {
                this.lblCurrentCount.Text = value.ToString() + "～" + end.ToString();
            }

            // ボタン制御
            SwitchButtonEnabled();
        }

        // -------------------------------------------------------------
        // ページボタンの制御
        // -------------------------------------------------------------
        private void SwitchButtonEnabled()
        {
            // ボタン制御
            this.btnFirst.Enabled = true;
            this.btnBack.Enabled = true;
            this.btnNext.Enabled = true;
            this.btnLast.Enabled = true;

            if (maxRowCount == 0)
            {
                this.btnFirst.Enabled = false;
                this.btnBack.Enabled = false;
                this.btnNext.Enabled = false;
                this.btnLast.Enabled = false;
                return;
            }

            if (currentCount == 1)
            {
                this.btnFirst.Enabled = false;
                this.btnBack.Enabled = false;
            }

            if (currentCount + RowsInPage > maxRowCount)
            {
                this.btnNext.Enabled = false;
                this.btnLast.Enabled = false;
            }
        }

        // -------------------------------------------------------------
        // グリッドの書式定義 デリゲート
        // -------------------------------------------------------------
        public delegate void OnGridFormatEhandler();
        public event OnGridFormatEhandler OnGridFormat;

        // -------------------------------------------------------------
        // グリッドのダブルクリック デリゲート
        // -------------------------------------------------------------
        public delegate void OnGridDoubleClickEhandler(OnGridDoubleClickArgs args);
        public event OnGridDoubleClickEhandler OnGridDoubleClick;

        public class OnGridDoubleClickArgs : EventArgs
        {
            public object Row;
        }

        // -------------------------------------------------------
        // コンストラクタ
        // -------------------------------------------------------
        public UcGridPager()
        {
            InitializeComponent();
            SetMaxRowCount(0);

            // DataGirdViewのTypeを取得
            Type dgvtype = typeof(DataGridView);
            // プロパティ設定の取得
            System.Reflection.PropertyInfo dgvPropertyInfo = 
                dgvtype.GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance
                | System.Reflection.BindingFlags.NonPublic);
            // 対象のDataGridViewにtrueをセットする
            dgvPropertyInfo.SetValue(dataGridView, true, null);

        }

        // -------------------------------------------------------
        // ページを表示
        // -------------------------------------------------------
        public void ShowPage()
        {
            if (fullDataSource == null) return;

            // 計測対象の処理
            var list = fullDataSource
                .Skip(currentCount - 1)
                .Take(RowsInPage)
                .ToList()
                ;

            // バインドデータを更新する間、レイアウトの描画を停止
            try
            {

                // バインド前に高速化のためにグリッドの描画を一旦止める
                dataGridView.SuspendLayout();
                dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;

                // バインド
                BindingSource bs = new BindingSource();
                bs.DataSource = list;
                dataGridView.DataSource = bs;   
            }
            finally
            {
                // ０行だと書式設定できない
                if (list.Count > 0)
                {
                    // 書式設定
                    SetDgvFormat();

                }
                // グリッドの描画再開
                dataGridView.ResumeLayout();
            }

        }

        private void btnFirst_Click(object sender, EventArgs e)
        {
            SetCurrentCount(1);
            ShowPage();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            SetCurrentCount(currentCount - RowsInPage);
            ShowPage();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            SetCurrentCount(currentCount + RowsInPage);
            ShowPage();
        }

        private void btnLast_Click(object sender, EventArgs e)
        {
            
            while (currentCount < maxRowCount)
            {
                SetCurrentCount(currentCount + RowsInPage);
            }
            SetCurrentCount(currentCount - RowsInPage);

            ShowPage();
        }

        // ----------------------------------------------------------------
        // グリッドの書式
        // ----------------------------------------------------------------
        private bool IsFormatted = false;

        private void SetDgvFormat()
        {
            if (IsFormatted) return;

            // 列幅の設定:
            foreach (DataGridViewColumn column in dataGridView.Columns)
            {
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;

                if (column.ValueType == typeof(int))
                {
                    column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }
                if (column.ValueType == typeof(DateTime))
                {
                    column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
            }
            // 実装側で追加の書式設定
            OnGridFormat();

            IsFormatted = true;
        }

        // ----------------------------------------------------------------
        // 一覧のダブルクリック
        // ----------------------------------------------------------------
        private void dataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                var row = dataGridView.Rows[e.RowIndex].DataBoundItem;
                var arg = new OnGridDoubleClickArgs()
                {
                    Row = row
                };

                // 実装側で追加のダブルクリックイベント
                OnGridDoubleClick(arg);
            }
            catch
            {
                return;
            }
        }

        // -----------------------------------------------------
        // Excel 出力
        // -----------------------------------------------------
        private async void buttonExcel_Click(object sender, EventArgs e)
        {
            // ボタンを連続でクリックできなくする
            this.buttonExcel.Enabled = false;
            Cursor.Current = Cursors.WaitCursor;

            await Task.Run(() =>
            {
                // 非同期処理
                var gridToExcel = new GridToExcel(dataGridView, fullDataSource);
                gridToExcel.SendToExcel();
            });

            //元に戻す
            Cursor.Current = Cursors.Default;
            this.buttonExcel.Enabled = true;
        }

        // ------------------------------------------------------------
        // Grid内のデータ検索
        // ------------------------------------------------------------
        private void textBox検索_KeyPress(object sender, KeyPressEventArgs e)
        {
            //EnterやEscapeキーでビープ音が鳴らないようにする
            if (e.KeyChar == (char)Keys.Enter || e.KeyChar == (char)Keys.Escape)
            {
                e.Handled = true;
            }
        }

        private void textBox検索_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Set検索対象行();
            }

        }

        private void button検索_Click(object sender, EventArgs e)
        {

            Set検索対象行();
        }

        // キーワードを検索
        private void Set検索対象行()
        {
            // 入力値
            var キーワード = this.textBox検索.Text;
            if (string.IsNullOrWhiteSpace(キーワード)) return;

            // 複数ページにまたがる一覧の可能性はあるが、表示しているページ内だけで検索する
            // Datasourceから検索することも考えたが、テーブルの構造もわからないので、
            // 直接DataGridView内を検索する事にした。

            int currentRow = dataGridView.SelectedRows[0].Index + 1;

            var キーワードRow = GetRowByキーワード(キーワード, currentRow);

            if (キーワードRow >= 0)
            {
                SelectAndShowRow(キーワードRow);
            }
            else
            {
                // 先頭から検索し直し
                キーワードRow = GetRowByキーワード(キーワード, 0);
                if (キーワードRow >= 0)
                {
                    SelectAndShowRow(キーワードRow);
                }
            }

        }

        private int GetRowByキーワード(string キーワード, int startRow)
        {
            for (int i = startRow; i < dataGridView.Rows.Count; i++)
            {
                for (int j = 0; j < dataGridView.ColumnCount; j++)
                {
                    var cellVal = dataGridView.Rows[i].Cells[j].Value.ToString();
                    if (cellVal.Contains(キーワード))
                    {
                        return i;
                    }
                }
            }

            return -1;
        }

        private void SelectAndShowRow(int row)
        {
            dataGridView.Rows[row].Selected = true;
            if (!dataGridView.SelectedRows[0].Displayed)
            {
                // 対象行が画面内に表示されて居ないときだけ、表示範囲を切り替える
                dataGridView.FirstDisplayedScrollingRowIndex = row;
            }
        }
    }
}
