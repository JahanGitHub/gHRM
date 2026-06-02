using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace gHRM.Web.ViewModels
{
    public class EmployeeEquivalentDesignationViewModel:BaseModel
    {
        [Key]
        public int EquivalentDesigId { get; set; }
        public string EquivalentDesignationName { get; set; }
        public bool IsActive { get; set; }
        public long? CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public long? UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}