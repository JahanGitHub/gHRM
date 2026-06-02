using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Service.ViewsEmployee
{
   public class EmployeeStatusServiceModel
    {
        /// <summary>
        /// EmployeeStatusId
        /// </summary>
        public int StatusId { get; set; }
        public string StatusName { get; set; }
        public string StatusValue { get; set; }
        public int ViewOrder { get; set; }
        public bool IsActive { get; set; }
        /// <summary>
        /// 1 = Valid Employee 
        /// 0 OR Null = invalid Employee
        /// </summary>
        public bool IsValid { get; set; }
        /// <summary>
        /// 1 = Valid Employee for Salary
        /// 0 OR Null = invalid Employee for Salary
        /// </summary>
        public bool? IsSalaryApplicable { get; set; }
    }
}
