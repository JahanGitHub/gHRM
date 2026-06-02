using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace gHRM.Web.ViewModels.eRecruits
{
    public class EducationDegreeViewModel
    {


        public int DegreeId { get; set; }

        public int DegreeLevelId { get; set; }

        [Required]
        [StringLength(100)]
        public string DegreeLevel { get; set; }

        [Required]
        [StringLength(100)]
        public string DegreeCode { get; set; }

        [Required]
        [StringLength(100)]
        public string DegreeName { get; set; }

        public int? CompanyId { get; set; }

        public bool IsActive { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? InActiveDate { get; set; }

        public long? CreateUser { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? CreateDate { get; set; }

        public long? UpdateUser { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? UpdateDate { get; set; }

    }
}