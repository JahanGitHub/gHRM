using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.CodeFirstMigration
{
    [Table("ReceivedCertificates")]
    public class ReceivedCertificates
    {
        [Key]
        public int Id { get; set; }
        public long EmployeeId { get; set; }
        public string EmployeeCode { get; set; }
        public string DegreeCode { get; set; }
        public int Memo { get; set; }
        public int? NoOfCopies { get; set; }
        public string EmployeeCertificateStatus { get; set; }
        public DateTime StatusDate { get; set; }
        public string Comment { get; set; }
        public bool IsActive { get; set; }
        public long CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public long? UpdateBy { get; set; }
        public DateTime UpdateDate { get; set; }
        public string CertificateType { get; set; }
    }
}
