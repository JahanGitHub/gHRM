using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace gHRM.Data.CodeFirstMigration
{
    [Table("att.View_TimeKeepingDetail")]
    public class View_TimeKeepingDetail
    {
        [Key]
        public int rowSl { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public DateTime AttenDate { get; set; }
        public string LogInTime { get; set; }
        public string ExpectedTime { get; set; }
        public string AttenTypeFullName { get; set; }

        [NotMapped]
        public int? OfficeId { get; set; }

    }
}
