using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;


namespace gHRM.Data.CodeFirstMigration.Basic
{
    [Table("Notice")]
    public class Notice
    {
        [Key]
        public int NoticeId { get; set; }

        public string Title { get; set; }
        [AllowHtml]
        public string NoticeText { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime PublishDate { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime LiveFrom { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime LiveTo { get; set; }
        public bool IsActive { get; set; }

        public long? CreateUser { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? CreateDate { get; set; }

        public long? UpdateUser { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? UpdateDate { get; set; }
        public int RoleId { get; set; }
        public int OfficeTypeId { get; set; }
    }
}
