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
    public interface IEmployeeSalaryDeductionRepository : IRepository<EmployeeSalaryDeduction>
    {
        List<EmployeeSalaryDeduction> AddEmplyoeeSalaryDeductionList(List<EmployeeSalaryDeduction> objs);
        Task<BaseResponse> IsValidDeductionByEffectiveDates(BaseSearchFilter filter);
    }
    public class EmployeeSalaryDeductionRepository : RepositoryBaseCodeFirst<EmployeeSalaryDeduction>, IEmployeeSalaryDeductionRepository
    {
        public EmployeeSalaryDeductionRepository(IDatabaseFactoryCodeFirst databaseFactory)  //check
            : base(databaseFactory)
        {

        }

        public List<EmployeeSalaryDeduction> AddEmplyoeeSalaryDeductionList(List<EmployeeSalaryDeduction> objs)
        {
            DataContext.EmployeeSalaryDeduction.AddRange(objs);
            //objs.ForEach(p => DataContext.Entry(p).State = EntityState.Modified);           
            DataContext.SaveChanges();
            return objs;
        }

        public async Task<BaseResponse> IsValidDeductionByEffectiveDates(BaseSearchFilter filter)
        {
            try
            {
                var employeeId = filter.EmployeeId > 0 ? filter.EmployeeId.ToString() : "NULL";
                var prComponentId = filter.PRComponentId > 0 ? filter.PRComponentId.ToString() : "NULL";
                var startDate = ((DateTime)filter.StartDate).ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture);
                var endDate = ((DateTime)filter.EndDate).ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture);

                var sqlCommand = $"[prl].[EmployeeSalaryIncentive_CheckEmployeeSalaryDeduction] {employeeId},'{startDate}', '{endDate}', {prComponentId}, {filter.ProductId},{filter.SerialId}";

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
