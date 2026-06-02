using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.CodeFirstMigration
{
    [Table("trns.EmployeeTransfer")]
    public class EmployeeTransfer
    {
        [Key]
        public int Id { get; set; }
        //public long? TransferProposalId { get; set; }
        public long EmployeeId { get; set; }
        //public string EmployeeCode { get; set; }
        public int OfficeId { get; set; }
        public int DepartmentId { get; set; }
        public int OfficeDesignationId { get; set; }

        public long OrderNo { get; set; }
        public DateTime OrderDate { get; set; }
        public bool IsTADAApplicable { get; set; }
        public bool IsMutual { get; set; }

        public bool IsPlanned { get; set; }
        public DateTime? PlannedJoiningDate { get; set; }
        public DateTime? PlannedReleaseDate { get; set; }

        public bool IsApproved { get; set; }
        public DateTime? JoiningDate { get; set; }
        public DateTime? ReleaseDate { get; set; }
       
        public bool IsActive { get; set; }
        public long CreateUser { get; set; }
        public DateTime CreateDate { get; set; }
        public long? UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? SectionId { get; set; }

        public string ChangingStatus { get; set; }

        public bool? HasJoined { get; set; }

                     


    }
}
