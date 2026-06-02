using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Core.Filters
{
    public class BaseSearchFilter
    {
        public BaseSearchFilter()
        {
            this.SortDirection = "ASC";
        }
        public string SortDirection { get; set; }
        public string SortColumn { get; set; }
        public string SearchTerm { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public int? OfficeId { get; set; }
        public int? OfficeTypeId { get; set; }
        public int? OfficeLocationId { get; set; }
        public int? DepartmentId { get; set; }
        public int? PFTypeId { get; set; }
        public int? BranchId { get; set; }
        public int? Year { get; set; }
        public int? Month { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string StartDateInString { get; set; }
        public string EndDateInString { get; set; }
        public int? EmployeeId { get; set; }
        public int? PRComponentId { get; set; }
        public string EmployeeCode { get; set; }
        public long? EmployeeTypeId { get; set; }
        public long? EmploymentTypeId { get; set; } //like pay scale and not pay scale
        public int? EmployeeStatusId { get; set; }
        public int? PreparedBy { get; set; }
        public int? Id { get; set; }
        //payroll
        public int PrComponentId { get; set; }
        public string ComponentName { get; set; }
        public string ComponentCategory { get; set; }

        //leave
        public int? LeaveTypeId { get; set; }

        public int? SalaryMonth { get; set; }
        public int? SalaryYear { get; set; }

        public bool? IsSendForApproval { get; set; }
        public bool? IsApproved { get; set; }
        public bool? IsRejected { get; set; }
        public bool? IsActive { get; set; }

        public string GHRMPlusCompany { get; set; }
        public int? ProductId { get; set; }
        public int? SerialId { get; set; }
        public string OfficeCode { get; set; }
        public int RoleId { get; set; }
        public int TotalCount { get; set; }

        // eRecruit


        //[Display(Name = "Start Date")]
        //public DateTime? StartDate { get; set; }

        //[Display(Name = "End Date")]
        //public DateTime? EndDate { get; set; }

        public int? CountryId { get; set; }
        public int? StateOrProvinceId { get; set; }
        public int? DistrictId { get; set; }
        public int? ThanaId { get; set; }
        public int? CompanyId { get; set; }
        public string DegreeTitle { get; set; }
        public string NationalId { get; set; }
        public string ApplicantName { get; set; }
        public long ApplicationId { get; set; }
        public string RollNoVerify { get; set; }
        public long? ApplicantId { get; set; }
        public string BoardName { get; set; }
        public string PassingYear { get; set; }

        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }


    }
}