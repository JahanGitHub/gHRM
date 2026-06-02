using gHRM.Core.Utilities;

namespace gHRM.Data.DBDetailModels.Employee
{
    public class EmployeeAddOrEditApiModel
    {
        public long id { get; set; }
        public long apiEmployeeId { get; set; }
        public string fullName { get; set; }
        public int? designationId { get; set; }
        public string designation { get; set; }
        public string contactNumber { get; set; }
        public string email { get; set; }
        public string employeeCode { get; set; }
        public bool? active { get; set; }

        public EmployeeOfficeApiModel center { get; set; }
      
    }

    public class EmployeeOfficeApiModel
    {
        public int id { get; set; }
        public int apiOfficeId { get; set; }
    }
}
