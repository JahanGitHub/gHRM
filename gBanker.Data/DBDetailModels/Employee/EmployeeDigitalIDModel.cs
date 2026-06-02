using gHRM.Core.Utilities;

namespace gHRM.Data.DBDetailModels.Employee
{
    public class EmployeeDigitalIDModel
    {
        public string CompanyName { get; set; }
        public string CompanyLogo { get; set; }
        public string CompanyEmail { get; set; }
        public string CompanySlogan { get; set; }
        public long EmployeeId { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string DesignationName { get; set; }
        public string EmployeeImageLink { get; set; }
        public byte[] EmployeeImage { get; set; }       
        public string NameofOperation { get; set; }
        public string BloodGroup { get; set; }
        public string IssueDate { get; set; }
        public string ContactNo { get; set; }
        public string CompanyAddress { get; set; }
        public string CompanyMobile { get; set; }
        public string WebsiteUrl { get; set; }
        public string QRCode { get; set; }
        public string CompanySignaturePath { get; set; }
    }
}
