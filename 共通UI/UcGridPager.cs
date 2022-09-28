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
            MaxRowCount = fullDataSource.Count;
        }

        // -------------------------------------------------------
        // 公開プロパティ
        // -------------------------------------------------------

        private int rowsInPage = 100;
        /// <summary>
        /// １ページに何行表示するか。デフォルト100行／ページ
        /// </summary>
        public int RowsInPage
        {
            get { return rowsInPage; }
            set
            {
                rowsInPage = value;
            }
        }

        // 現在表示している行範囲の先頭
        private int currentCount;
        private int CurrentCount
        {
            get { return currentCount; }
            set
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
        }

        // 一覧に表示するリストの最大行数
        private int maxRowCount;
        private int MaxRowCount
        {
            set
            {
                maxRowCount = value;
                this.lblMaxCount.Text = maxRowCount.ToString();

                // 件数が変わった＝初期化された。
                CurrentCount = 1;
            }
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

            if (CurrentCount == 1)
            {
                this.btnFirst.Enabled = false;
                this.btnBack.Enabled = false;
            }

            if (CurrentCount + RowsInPage > maxRowCount)
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
            MaxRowCount = 0;

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

            var list = fullDataSource
                .Skip(CurrentCount - 1)
                .Take(RowsInPage)
                .ToList()
                ;

            bindingSource.DataSource = list;
            dataGridView.DataSource = bindingSource;

            // ０行だと書式設定できない
            if (list.Count == 0) return;

            SetDgvFormat();

        }

        private void btnFirst_Click(object sender, EventArgs e)
        {
            CurrentCount = 1;
            ShowPage();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            CurrentCount -= RowsInPage;
            ShowPage();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            CurrentCount += RowsInPage;
            ShowPage();
        }

        private void btnLast_Click(object sender, EventArgs e)
        {
            while (CurrentCount < maxRowCount)
            {
                CurrentCount += RowsInPage;
            }
            CurrentCount -= RowsInPage;

            ShowPage();
        }

        // ----------------------------------------------------------------
        // グリッドの書式
        // ----------------------------------------------------------------
        private void SetDgvFormat()
        {
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
