using gHRM.Core.Filters.Employee;
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.DBDetailModels.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace gHRM.Data.Repository
{
    public interface IEmployeeDocumentRepository : IRepository<EmployeeDocument>
    {
        Task<IEnumerable<EmployeeDigitalIDModel>> GetEmployeeDigitalIDInfo(EmployeeSearchFilter filter);       
    }

    public class EmployeeDocumentRepository : RepositoryBaseCodeFirst<gHRM.Data.CodeFirstMigration.EmployeeDocument>, IEmployeeDocumentRepository
    {
        public EmployeeDocumentRepository(IDatabaseFactoryCodeFirst databaseFactory) : base(databaseFactory)
        {

        }

        public async Task<IEnumerable<EmployeeDigitalIDModel>> GetEmployeeDigitalIDInfo(EmployeeSearchFilter filter)
        {
            var filteredList = new List<EmployeeDigitalIDModel>();
            try
            {
                var employeeCode = filter.EmployeeCode;                
                employeeCode = string.IsNullOrWhiteSpace(filter.EmployeeCode) ? "''" : $"'{string.Join(",", employeeCode)}'";
                var officeId = filter.OfficeId > 0 ? (int)filter.OfficeId : 0;
                var departmentId = filter.DepartmentId > 0 ? (int)filter.DepartmentId : 0;

                var sqlCommand = $@"[dbo].[Employee_GetEmployeeDigitalIDCard] {officeId},{departmentId},{employeeCode}";

                filteredList = await DataContext.Database
                    .SqlQuery<EmployeeDigitalIDModel>(sqlCommand)
                    .ToListAsync();

            }
            catch (Exception ex)
            {
                return new List<EmployeeDigitalIDModel>();
            }

            return filteredList;
        }
    }
}
