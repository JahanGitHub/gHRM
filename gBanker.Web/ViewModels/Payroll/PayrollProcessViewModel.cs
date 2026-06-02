
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gHRM.Web.ViewModels.Payroll
{
    public class TmpEmployeeSalaryView
    {
        public int rowSl { get; set; }
        public long EmployeeId { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public double TotalEarning { get; set; }
        public double TotalDeduction { get; set; }
        public double NetPayable { get; set; }
        public string Designation { get; set; }
        public string Department { get; set; }
    }

    public class TmpEmployeeSalaryView_Challan
    {
        public int rowSl { get; set; }
        public long EmployeeId { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public double TotalEarning { get; set; }
        public double TotalDeduction { get; set; }
        public double NetPayable { get; set; }
        public string Designation { get; set; }
        public string Department { get; set; }

        public string ChallanNo { get; set; }
        public string ChallanDate { get; set; }
    }



    public class TmpEmployeeDuplicateCheck
    {
        public long EmployeeId { get; set; }
    }


    public class TmpEmployeeDuplicateCheck_Challan
    {
        public long EmployeeId { get; set; }
    }

    public class TempComponentForIncrement
    {
        public int ComponentId { get; set; }
        public string ComponentName { get; set; }
        public decimal ComponentAmount { get; set; }
        public long EmployeeId { get; set; }
        public long PRSalaryConfigurationId { get; set; }
        public string ComponentCategory { get; set; }
        public string TransactionType { get; set; }
        public int OfficeId { get; set; }

    }

    public class TempComponent
    {
        public int PRComponentID { get; set; }
        public string ComponentName { get; set; }
    }    
}