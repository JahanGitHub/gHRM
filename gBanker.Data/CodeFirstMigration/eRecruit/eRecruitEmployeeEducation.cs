using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace gHRM.Data.CodeFirstMigration.eRecruit
{
    [Table("eRecruit.EmployeeEducation")]

    public partial class eRecruitEmployeeEducation
    {
        [Key]
        public long EducationId { get; set; }
        public long EmployeeId { get; set; }
        public string DegreeTitle { get; set; }
        public string InstitutionName { get; set; }
        public string UniversityName { get; set; }
        public string PassingYear { get; set; }
        public string GPA { get; set; }
        //public string DivisionOrClass { get; set; }
        public string RollNo { get; set; }
        public long? RegNo { get; set; }
        public string GroupName { get; set; }
        public string SubjectName { get; set; }
        public string BoardName { get; set; }
        public string ObtainedMarks { get; set; }
        public bool IsActive { get; set; }
        public long? CreateUser { get; set; }
        public DateTime? CreateDate { get; set; }
        public long? UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? GradeTypeId { get; set; }
    }

}
