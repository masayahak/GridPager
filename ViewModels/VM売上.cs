using GridPager.Models;
using System;
using System.Linq;

namespace ViewModel
{
    public class VM売上
    {

        public T売上[] list売上 { get; private set; }

        // -------------------------------------------------------------------------
        // コンストラクタ
        // -------------------------------------------------------------------------
        public VM売上()
        {
        }

        // -------------------------------------------------------------------------
        // T売上の読み込み
        // -------------------------------------------------------------------------
        public void LoadT売上(DateTime 期間開始, DateTime 期間終了)
        {
            using (var db = new MyDbContext())
            {
                list売上 = db.T売上s
                            .Where(it => it.売上日 >= 期間開始 && it.売上日 <= 期間終了)
                            .ToArray();
            }
        }
    }
}
