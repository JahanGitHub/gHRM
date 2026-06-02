using gHRM.Data.CodeFirstMigration;
using gHRM.Web.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace gHRM.Web.ViewModels
{
    public class EmployeeEducationViewModel : BaseModel
    {
        public long EducationId { get; set; }

        public long EmployeeId { get; set; }

        [Required(ErrorMessage = "Degree/Title is required")]
        [Display(Name = "Degree/Title")]
        public string DegreeTitle { get; set; }

        
        public string Concentration { get; set; }


        public string ConcentrationName { get; set; }

        [Required(ErrorMessage = "Institution Name is required")]
        [Display(Name = "Institution Name")]
        public string InstitutionName { get; set; }

        [Required(ErrorMessage = "Passing Year is required")]
        [Display(Name = "Passing Year")]
        public string PassingYear { get; set; }

        [Required(ErrorMessage = "Result Type is required")]
        [Display(Name = "Result Type")]
        public string ResultType { get; set; }

        [StringLength(11)]
        public string Division { get; set; }

        [StringLength(11)]
        [Display(Name = "Marks Percentage")]
        public string MarksPercentage { get; set; }

        [StringLength(10)]
        public string CGPA { get; set; }

        [StringLength(10)]
        [Display(Name = "CGPA Scale")]
        public string CGPAScale { get; set; }

        [StringLength(20)]
        public string Duration { get; set; }

        [StringLength(500)]
        public string Acheivements { get; set; }

        public bool IsActive { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? InActiveDate { get; set; }

        [StringLength(50)]
        public string CreateUser { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? CreateDate { get; set; }

        [StringLength(50)]
        public string UpdateUser { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? UpdateDate { get; set; }

        //public virtual Employee Employee { get; set; }
        public string DegreeName { get; set; }
        public int DegreeLevelId { get; set; }
        public string DegreeLevel { get; set; }

    }
}