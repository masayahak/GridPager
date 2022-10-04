using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ViewModel;
using 共通UI;

namespace GridPager
{
    public partial class Formテスト : Form
    {
        // ----------------------------------------------------------------
        // 表示する一覧の定義
        // ----------------------------------------------------------------
        public enum ds売上一覧_Col
        {
            No = 0,
            伝票番号 = 1,
            売上日 = 2,
            得意先コード = 3,
            得意先名 = 4,
            担当社員番号 = 5,
            担当社員名 = 6,
            売上高 = 7,
        }

        private VM売上 vm売上;

        // ----------------------------------------------------------------
        // コンストラクタ
        // ----------------------------------------------------------------
        public Formテスト()
        {
            InitializeComponent();

            dtp開始.Value = new DateTime(2022, 1, 1, 0, 0, 0, DateTimeKind.Local);
            dtp終了.Value = new DateTime(2022, 12, 31, 0, 0, 0, DateTimeKind.Local);
        }

        // ----------------------------------------------------------------
        // ロードイベント
        // ----------------------------------------------------------------
        private void Form_Load(object sender, EventArgs e)
        {
            // DataGirdViewのパフォーマンス・チューニング
            InitDataGridView();
        }

        private void InitDataGridView()
        {
            Type dgvtype = typeof(DataGridView);

            // プロパティ設定の取得
            System.Reflection.PropertyInfo dgvPropertyInfo =
                dgvtype.GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance
                | System.Reflection.BindingFlags.NonPublic);
            dgvPropertyInfo.SetValue(dataGridView, true, null);

            dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            dataGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            dataGridView.RowHeadersVisible = false;
        }


        private void btn検索_Click(object sender, EventArgs e)
        {
            // Stopwatchクラス生成
            var sw = new System.Diagnostics.Stopwatch();
            // 計測開始
            sw.Start();

            // 計測対象の処理
            DataLoad();

            // 計測停止
            sw.Stop();

            TimeSpan ts = sw.Elapsed;
            Console.WriteLine($"検索ボタンクリック 計測時間 {ts.TotalMilliseconds}ミリ秒");

        }

        // ----------------------------------------------------------------
        // データ読み込み
        // ----------------------------------------------------------------
        private async void DataLoad()
        {
            if (DesignMode) return;

            ShowLoading();

            vm売上 = new VM売上();

            DateTime 期間開始 = this.dtp開始.Value.Date;
            DateTime 期間終了 = this.dtp終了.Value.Date;

            // 非同期でデータ取得
            var list = new List<Object>();

            await Task.Run(() =>
            {
                vm売上.LoadT売上(期間開始, 期間終了);
                list = vm売上.Get売上一覧();
            });

            // バインド
            BindingSource bs = new BindingSource();
            bs.DataSource = list;
            dataGridView.DataSource = bs;

            // 書式設定
            SetDgvFormat();

            LoadEnd();
        }

        // ロード中
        private void ShowLoading()
        {
            this.ucロード中.Visible = true;
            this.ucロード中.BringToFront();
        }

        private void LoadEnd()
        {
            this.ucロード中.Visible = false;
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
                    column.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
                }
                if (column.ValueType == typeof(DateTime))
                {
                    column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    column.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
            }
            // 実装側で追加の書式設定
            OnGridFormat();
        }


        // ----------------------------------------------------------------
        // グリッドの書式設定
        // ----------------------------------------------------------------
        private void OnGridFormat()
        {
            DataGridView dg = dataGridView;

            // 書式
            dg.Columns[(int)ds売上一覧_Col.売上日].DefaultCellStyle.Format = "MM/dd";
            dg.Columns[(int)ds売上一覧_Col.売上高].DefaultCellStyle.Format = "C";

            // グリッド列の幅を調整する
            GridColumun_Resize();
        }

        // ----------------------------------------------------------------
        // サイズ変更
        // ----------------------------------------------------------------
        FormWindowState SavedWindowState = FormWindowState.Normal;

        // 最大化、最初化はResizedEndイベントが発生しない
        private void Form_SizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState != SavedWindowState)
            {
                GridColumun_Resize();
                SavedWindowState = this.WindowState;
            }
        }

        // 動的に列の情報を操作するためにはResizedEndイベントが最もスムーズ
        private void Form_ResizedEnd(object sender, EventArgs e)
        {
            GridColumun_Resize();
        }

        // グリッド列の幅を調整する
        private void GridColumun_Resize()
        {
            DataGridView dg = dataGridView;

            // データ0行ならなにもしない
            if (dg.RowCount == 0) return;

            // 表示列の再計算
            dg.Columns[(int)ds売上一覧_Col.得意先名].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;

            int ColWidthSum = 0;
            foreach (DataGridViewColumn column in dg.Columns)
            {
                ColWidthSum += column.Width;
            }

            if (dg.Width > ColWidthSum)
            {
                dg.Columns[(int)ds売上一覧_Col.得意先コード].Visible = true;
                dg.Columns[(int)ds売上一覧_Col.担当社員番号].Visible = true;

                dg.Columns[(int)ds売上一覧_Col.得意先名].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
            else
            {
                dg.Columns[(int)ds売上一覧_Col.得意先コード].Visible = false;
                dg.Columns[(int)ds売上一覧_Col.担当社員番号].Visible = false;

                dg.Columns[(int)ds売上一覧_Col.得意先名].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
        }


        // --------------------------------------------------------------
        // EXCELへの転送処理
        // --------------------------------------------------------------
        private void buttonExcel_Click(object sender, EventArgs e)
        {

            // Stopwatchクラス生成
            var sw = new System.Diagnostics.Stopwatch();
            // 計測開始
            sw.Start();

            // 計測対象の処理
            SendToExcel();

            // 計測停止
            sw.Stop();

            TimeSpan ts = sw.Elapsed;
            Console.WriteLine($"EXCELへの転送処理 計測時間 {ts.TotalMilliseconds}ミリ秒");

        }

        private async void SendToExcel()
        {
            // グリッドが0件ならなにもしない
            if (dataGridView.Rows.Count == 0) return;

            // ボタンを連続でクリックできなくする
            this.buttonExcel.Enabled = false;
            Cursor.Current = Cursors.WaitCursor;

            // 非同期処理
            await Task.Run(() =>
            {
                // EXCEL転送クラス
                var gridToExcel = new GridToExcel(dataGridView);

                // 転送実行
                gridToExcel.SendToExcel();

            });

            //元に戻す
            Cursor.Current = Cursors.Default;
            this.buttonExcel.Enabled = true;

        }

    }
}
