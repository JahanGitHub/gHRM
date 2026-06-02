using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gHRM.Data.CodeFirstMigration
{
    [Table("Office")]
    public partial class Office
    {
        public Office()
        {
            ApplicationSettings = new HashSet<ApplicationSetting>();
            EmployeeOfficeMappings = new HashSet<EmployeeOfficeMapping>();
            EmployeePostingHistories = new HashSet<EmployeeTransfer>();
            //SchedulerDetails = new HashSet<SchedulerDetail>();
        }

        public int OfficeId { get; set; }
        public int? CompanyId { get; set; }

        public int? OfficeTypeId { get; set; }
        [Required]
        [StringLength(10)]
        public string OfficeCode { get; set; }

        [Required]
       // [StringLength(40)]
        public string OfficeName { get; set; }

        public string OfficeNameBn { get; set; }

        public int OfficeLevel { get; set; }

        [Required]
        [StringLength(10)]
        public string FirstLevel { get; set; }

        [StringLength(10)]
        public string SecondLevel { get; set; }

        [StringLength(10)]
        public string ThirdLevel { get; set; }

        [StringLength(10)]
        public string FourthLevel { get; set; }

        [Column(TypeName = "date")]
        public DateTime OperationStartDate { get; set; }

        [StringLength(155)]
        public string OfficeAddress { get; set; }

        [StringLength(10)]
        public string PostCode { get; set; }

        //public int? GeoLocationID { get; set; }

        [StringLength(45)]
        public string Email { get; set; }

        [StringLength(35)]
        public string Phone { get; set; }
        public string ImagePath { get; set; }
        public Nullable<int> PRWorkAreaID { get; set; }

        public bool IsActive { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? InActiveDate { get; set; }

        public long? CreateUser { get; set; }
        public long? UpdateUser { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? CreateDate { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? UpdateDate { get; set; }
        public int? OfficeLocationId { get; set; }

        public virtual ICollection<ApplicationSetting> ApplicationSettings { get; set; }

        public virtual ICollection<EmployeeOfficeMapping> EmployeeOfficeMappings { get; set; }

        public virtual ICollection<EmployeeTransfer> EmployeePostingHistories { get; set; }

        //public virtual GeoLocation GeoLocation { get; set; }
        public virtual Company Company { get; set; }

        //public virtual ICollection<SchedulerDetail> SchedulerDetails { get; set; }
    }
}
