using System;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static 共通UI.UcGridPager;
using ViewModel;


namespace GridPager
{
    public partial class Form売上一覧 : Form
    {
        // ----------------------------------------------------------------
        // 表示する一覧の定義
        // ----------------------------------------------------------------
        public class ds売上一覧
        {
            public int No { get; set; }
            public int 伝票番号 { get; set; }
            public DateTime 売上日 { get; set; }
            public string 得意先コード { get; set; }
            public string 得意先名 { get; set; }
            public string 担当社員番号 { get; set; }
            public string 担当社員名 { get; set; }
            public int 売上高 { get; set; }
        }

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
        public Form売上一覧()
        {
            InitializeComponent();

            dtp開始.Value = DateTime.Today.AddDays(-7);
            dtp終了.Value = DateTime.Today;

            // グリッドのイベント
            this.ucGridPager.OnGridFormat += OnGrid_Format;
            this.ucGridPager.OnGridDoubleClick += OnGrid_DoubleClick;
        }

        // ----------------------------------------------------------------
        // ロードイベント
        // ----------------------------------------------------------------
        private void Form売上一覧_Load(object sender, EventArgs e)
        {
            DataLoad();
        }
        private void btn検索_Click(object sender, EventArgs e)
        {
            DataLoad();
        }

        // ----------------------------------------------------------------
        // データ読み込み
        // ----------------------------------------------------------------
        private async void DataLoad()
        {
            if (DesignMode) return;

            // ロード中
            ShowLoading();

            vm売上 = new VM売上();

            DateTime 期間開始 = this.dtp開始.Value.Date;
            DateTime 期間終了 = this.dtp終了.Value.Date;

            // 非同期でデータ取得
            await Task.Run(() =>
            {
                vm売上.LoadT売上(期間開始, 期間終了);

            });

            var list = vm売上.list売上
                .OrderBy(it => it.売上日)
                .ThenBy(it => it.得意先コード)
                .Select((it, i) => new ds売上一覧
                {
                    No = i + 1,
                    伝票番号 = it.ID,
                    売上日 = it.売上日,
                    得意先コード = it.得意先コード,
                    得意先名 = it.得意先名,
                    担当社員番号 = it.担当社員番号,
                    担当社員名 = it.担当社員名,
                    売上高 = it.売上高,
                })
                .ToList()
                ;

            this.ucGridPager.RowsInPage = 100;
            this.ucGridPager.SetFullDatasource<ds売上一覧>(list);
            this.ucGridPager.ShowPage();

            // ロード終了
            OnLoaded();

        }

        // ロード中
        private void ShowLoading()
        {
            this.ucロード中.Visible = true;
            this.ucロード中.BringToFront();
        }

        private void OnLoaded()
        {
            this.ucロード中.Visible = false;
        }

        // ----------------------------------------------------------------
        // グリッドの書式
        // ----------------------------------------------------------------
        // グリッドの書式設定
        private void OnGrid_Format()
        {
            DataGridView dg = this.ucGridPager.pagerDataGridView;

            // 書式
            dg.Columns[(int)ds売上一覧_Col.売上日].DefaultCellStyle.Format = "MM/dd";
            dg.Columns[(int)ds売上一覧_Col.売上高].DefaultCellStyle.Format = "C";

            ucGridPager_SizeChanged(this, null);
        }


        // ----------------------------------------------------------------
        // 一覧のサイズ変更
        // ----------------------------------------------------------------
        private void ucGridPager_SizeChanged(object sender, EventArgs e)
        {

            DataGridView dg = this.ucGridPager.pagerDataGridView;

            // データ0行ならなにもしない
            if (dg.RowCount == 0) return;

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

        // ----------------------------------------------------------------
        // 一覧のダブルクリック
        // ----------------------------------------------------------------
        private void OnGrid_DoubleClick(OnGridDoubleClickArgs args)
        {
            if (!(args.Row is ds売上一覧)) return;
            var ds = (ds売上一覧)args.Row;


            StringBuilder sb = new StringBuilder("");
            sb.Append("伝票番号:");
            sb.AppendLine(ds.伝票番号.ToString());
            sb.Append("売上日:");
            sb.AppendLine(ds.売上日.ToString("yyyy/MM/dd"));
            sb.Append("得意先:");
            sb.AppendLine(ds.得意先名);

            //仮で メッセージボックス
            MessageBox.Show(sb.ToString(),
                "確認",
                MessageBoxButtons.OK,
                MessageBoxIcon.Exclamation,
                MessageBoxDefaultButton.Button1);
        }

    }
}