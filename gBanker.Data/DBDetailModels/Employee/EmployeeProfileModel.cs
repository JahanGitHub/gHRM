using gHRM.Core.Utilities;

namespace gHRM.Data.DBDetailModels.Employee
{
    public class EmployeeProfileModel
    {
        public string EmployeeName { get; set; }
        public string ContactNo1 { get; set; }
        public string ContactNo2 { get; set; }
        public string Email { get; set; }
        public string OfficialEmail { get; set; }
        public string PresentAddress { get; set; }
        public string CompanyName { get; set; }
        public string CompanyNameOther { get; set; }
        public string CompanyShortName { get; set; }
        public string CompanyAddress { get; set; }
        public string CompanyEmail { get; set; }
        public string CompanyMobile { get; set; }
        public string CompanyPhone { get; set; }
        public byte[] EmployeeImage { get; set; }
        public string EmployeeImageLink { get; set; }
        public byte[] EmployeeImageLinkImage
        {
            get
            {
                var newEmployeeImage = !string.IsNullOrWhiteSpace(EmployeeImageLink)
                    ? EmployeeImageLink.ImagePathToByte(): EmployeeImage;
                return newEmployeeImage;
            }
        }       
    }
}
