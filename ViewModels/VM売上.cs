using GridPager.Models;
using System;
using System.Collections.Generic;
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

        // -------------------------------------------------------------------------
        // Grid用売上一覧
        // -------------------------------------------------------------------------
        public List<Object> Get売上一覧()
        {
            var list = list売上
                .OrderBy(it => it.売上日)
                .ThenBy(it => it.得意先コード)
                .Select((it, i) => new
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
                .Cast<Object>().ToList();

            return list;
        }



    }
}
