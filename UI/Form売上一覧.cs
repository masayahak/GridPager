using System;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static 共通UI.UcGridPager;
using ViewModel;
using System.Collections.Generic;

namespace GridPager
{
    public partial class Form売上一覧 : Form
    {
        // ----------------------------------------------------------------
        // 表示する一覧の定義
        // ----------------------------------------------------------------

        private const int グリッドの表示行数 = 1000;

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

            ucGridPager.RowsInPage = グリッドの表示行数;
            ucGridPager.OnGridFormat += OnGrid_Format;
            ucGridPager.OnGridDoubleClick += OnGrid_DoubleClick;
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

            ShowLoading();

            vm売上 = new VM売上();

            DateTime 期間開始 = this.dtp開始.Value.Date;
            DateTime 期間終了 = this.dtp終了.Value.Date;

            // 非同期でデータ取得
            var list = new List<ds売上一覧>();

            await Task.Run(() =>
            {
                vm売上.LoadT売上(期間開始, 期間終了);

                list = vm売上.list売上
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
                    .ToList();
            });

            this.ucGridPager.SetFullDatasource<ds売上一覧>(list);
            this.ucGridPager.ShowPage();

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
        // グリッドの書式設定
        // ----------------------------------------------------------------
        private void OnGrid_Format()
        {
            DataGridView dg = this.ucGridPager.pagerDataGridView;

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
            DataGridView dg = this.ucGridPager.pagerDataGridView;

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