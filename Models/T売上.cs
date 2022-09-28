namespace GridPager.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class T売上
    {
        public int ID { get; set; }

        [Required]
        [StringLength(20)]
        public string 得意先コード { get; set; }

        [Required]
        [StringLength(256)]
        public string 得意先名 { get; set; }

        [Required]
        [StringLength(20)]
        public string 担当社員番号 { get; set; }

        [Required]
        [StringLength(256)]
        public string 担当社員名 { get; set; }

        public DateTime 売上日 { get; set; }

        public int 売上高 { get; set; }

        public int 消費税額 { get; set; }
    }
}
