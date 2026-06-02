using gHRM.Core.Filters;
using gHRM.Core.Utilities;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.CodeFirstMigration.Payroll;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.Repository.Payroll
{
    public interface IEmployeeSalaryIncentiveRepository : IRepository<EmployeeSalaryIncentive>
    {
        List<EmployeeSalaryIncentive> AddTADA(List<EmployeeSalaryIncentive> objs);
        Task<BaseResponse> IsValidIncentiveByEffectiveDates(BaseSearchFilter filter);
    }
    public class EmployeeSalaryIncentiveRepository : RepositoryBaseCodeFirst<EmployeeSalaryIncentive>, IEmployeeSalaryIncentiveRepository
    {
        public EmployeeSalaryIncentiveRepository(IDatabaseFactoryCodeFirst databaseFactory)  //check
            : base(databaseFactory)
        {

        }

        public List<EmployeeSalaryIncentive> AddTADA(List<EmployeeSalaryIncentive> objs)
        {
            DataContext.EmployeeSalaryIncentive.AddRange(objs);
            DataContext.SaveChanges();
            return objs;
        }

        public async Task<BaseResponse> IsValidIncentiveByEffectiveDates(BaseSearchFilter filter)
        {
            try
            {
                var employeeId = filter.EmployeeId > 0 ? filter.EmployeeId.ToString() : "NULL";
                var prComponentId = filter.PRComponentId > 0 ? filter.PRComponentId.ToString() : "NULL";
                var startDate = ((DateTime)filter.StartDate).ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture);
                var endDate = ((DateTime)filter.EndDate).ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture);

                var sqlCommand = $"[prl].[EmployeeSalaryIncentive_CheckEmployeeSalaryIncentive] {employeeId},'{startDate}', '{endDate}', {prComponentId}, {filter.ProductId},{filter.SerialId}";

                var response = await DataContext.Database.SqlQuery<BaseResponse>(sqlCommand).FirstOrDefaultAsync();

                return response;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

    }
}
