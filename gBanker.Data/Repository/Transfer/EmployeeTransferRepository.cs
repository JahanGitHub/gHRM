using System.Linq;
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using System.Collections.Generic;

namespace gHRM.Data.Repository
{
    public interface IEmployeeTransferRepository : IRepository<EmployeeTransfer>
    {
        void GetDataFromExcelData(string EmployeeCode, string OfficeName, string DepartmentName, string SectionName, string ResponsibilityName, out long EmployeeId, out int OfficeId, out int DepartmentId, out int SectionId, out int ResponsibilityId);
    }
    public class EmployeeTransferRepository : RepositoryBaseCodeFirst<gHRM.Data.CodeFirstMigration.EmployeeTransfer>, IEmployeeTransferRepository
    {
        public EmployeeTransferRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }

        public void GetDataFromExcelData(string EmployeeCode, string OfficeName, string DepartmentName, string SectionName, string ResponsibilityName, out long EmployeeId, out int OfficeId, out int DepartmentId, out int SectionId, out int ResponsibilityId)
        {
            int EDepartmentId = 0;
            EmployeeId = DataContext.Employees.Where(x => x.IsActive && x.EmployeeCode.Trim().ToLower() == EmployeeCode.Trim().ToLower()).Select(x => x.EmployeeId).FirstOrDefault();
            // OfficeId = DataContext.Offices.Where(x => x.IsActive && x.OfficeName.Trim().ToLower() == OfficeName.Trim().ToLower()).Select(x => x.OfficeId).FirstOrDefault();
            OfficeId = DataContext.Offices
             .Where(x => x.IsActive && x.OfficeName.Trim().ToLower().Contains(OfficeName.Trim().ToLower()))
             .Select(x => x.OfficeId)
             .FirstOrDefault();


           // EDepartmentId = DataContext.EmployeeDepartments.Where(x => x.IsActive && x.DepartmentName.Trim().ToLower() == DepartmentName.Trim().ToLower()).Select(x => x.DepartmentId).FirstOrDefault();
            EDepartmentId = DataContext.EmployeeDepartments.Where(x => x.IsActive && x.DepartmentName.Trim().ToLower().Contains(DepartmentName.Trim().ToLower())).Select(x => x.DepartmentId).FirstOrDefault();



           // SectionId = DataContext.EmployeeDepartmentSection.Where(x => x.IsActive && x.DepartmentId == EDepartmentId && x.SectionName.Trim().ToLower() == SectionName.Trim().ToLower()).Select(x => x.SectionId).FirstOrDefault();
            SectionId = DataContext.EmployeeDepartmentSection.Where(x => x.IsActive && x.DepartmentId == EDepartmentId && x.SectionName.Trim().ToLower().Contains(SectionName.Trim().ToLower())).Select(x => x.SectionId).FirstOrDefault();

           // ResponsibilityId = DataContext.OfficeDesignations.Where(x => x.IsActive && x.OffcDesignName.Trim().ToLower() == ResponsibilityName.Trim().ToLower()).Select(x => x.OfficeDesignationId).FirstOrDefault();
            ResponsibilityId = DataContext.OfficeDesignations.Where(x => x.IsActive && x.OffcDesignName.Trim().ToLower().Contains( ResponsibilityName.Trim().ToLower())).Select(x => x.OfficeDesignationId).FirstOrDefault();
            DepartmentId = EDepartmentId;
        }
    }
}