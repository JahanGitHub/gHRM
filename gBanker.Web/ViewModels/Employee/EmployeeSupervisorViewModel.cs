using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gHRM.Web.ViewModels
{
    public class EmployeeSupervisorViewModel
    {
        public string rowSl { get; set; }
        public int Id { get; set; }
        public long EmployeeId { get; set; }
        public long SupervisorId { get; set; }
        public bool IsActive { get; set; }
        public long CreateUser { get; set; }
        public DateTime CreateDate { get; set; }
        public long? UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string SupervisorName { get; set; }
    }
}