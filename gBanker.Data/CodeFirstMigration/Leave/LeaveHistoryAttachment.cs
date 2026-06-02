using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gHRM.Data.CodeFirstMigration
{
    [Table("leave.LeaveHistoryAttachment")]
    public partial class LeaveHistoryAttachment
    {
        [Key]
        public long Id { get; set; }
        public long LeaveHistoryId { get; set; }
        public string FileName { get; set; }
        public string FileLocation { get; set; }
        public bool IsActive { get; set; }
        public long CreateUser { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
