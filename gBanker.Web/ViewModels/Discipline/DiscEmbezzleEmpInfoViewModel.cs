using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace gHRM.Web.ViewModels.Discipline
{
    public class DiscEmbezzleEmpInfoViewModel : BaseModel
    {
        public int EmbezzleEmpId { get; set; }
        public long EmployeeId { get; set; }
    }
}