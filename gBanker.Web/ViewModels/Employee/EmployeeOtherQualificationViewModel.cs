using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace gHRM.Web.ViewModels
{
    public class EmployeeOtherQualificationViewModel
    {
        public long QualificationId { get; set; }

        public long EmployeeId { get; set; }
        public string Language { get; set; }
        public string FluencyLevel { get; set; }
        //public string EducationalQualification { get; set; }
        public bool IsActive { get; set; }
        public long? CreateUser { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? CreateDate { get; set; }

        public long? UpdateUser { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? UpdateDate { get; set; }

    }
}