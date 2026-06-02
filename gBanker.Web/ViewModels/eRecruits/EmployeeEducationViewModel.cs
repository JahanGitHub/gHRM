using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace gHRM.Web.ViewModels.eRecruits
{
    public class EmployeeEducationViewModel
    {


        public long EducationId { get; set; }

        public long EmployeeId { get; set; }

        [Required]
        [StringLength(250)]
        public string DegreeTitle { get; set; }

        [Required]
        [StringLength(500)]
        public string InstitutionName { get; set; }

        [StringLength(50)]
        public string PassingYear { get; set; }

        [StringLength(10)]
        public string GPA { get; set; }

        public int? RollNo { get; set; }

        public int? RegNo { get; set; }

        [StringLength(20)]
        public string GroupName { get; set; }

        [StringLength(50)]
        public string SubjectName { get; set; }

        [StringLength(20)]
        public string BoardName { get; set; }

        public bool IsActive { get; set; }

        public long? CreateUser { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? CreateDate { get; set; }

        public long? UpdateUser { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? UpdateDate { get; set; }


    }
}