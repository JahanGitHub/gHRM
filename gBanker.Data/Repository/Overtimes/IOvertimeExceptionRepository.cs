using gHRM.Core.Filters;
using gHRM.Core.Utilities;
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.CodeFirstMigration.Payroll;
using gHRM.Data.DBDetailModels;
using gHRM.Data.DBDetailModels.Overtimes;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
namespace gHRM.Data.Repository
{
    public interface IOvertimeExceptionRepository : IRepository<OvertimeException>
    {
        IEnumerable<OvertimeExceptionModel> GetListingByFilter(BaseSearchFilter filter);
        BaseResponse IsValidOvertimeExceptionEffectiveDate(OvertimeException model);
    }
    public class OvertimeExceptionRepository : RepositoryBaseCodeFirst<OvertimeException>, IOvertimeExceptionRepository
    {
        public OvertimeExceptionRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }

        public IEnumerable<OvertimeExceptionModel> GetListingByFilter(BaseSearchFilter filter)
        {
            var id = filter.Id > 0 ? filter.Id.ToString() : "NULL";
            var sqlCommand = $"[prl].[GetOvertimeExceptionInfo] {id}";
            var overtimeExceptions = DataContext.Database.SqlQuery<OvertimeExceptionModel>(sqlCommand).AsParallel().ToList();
            
            return overtimeExceptions;
        }
        public BaseResponse IsValidOvertimeExceptionEffectiveDate(OvertimeException model)
        {
            var response = new BaseResponse();
            var isFound = true;
            
            var Id = model.Id > 0 ? model.Id.ToString() : "NULL";
            var startDate = model.EffectiveStartDate.ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture);
            var endDate = model.EffectiveEndDate.ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture);

            var sqlCommand = $"[prl].[OvertimeExceptionDateValidate] '{startDate}', '{endDate}', {model.EmployeeId}, {Id}";

            var overtimeExceptions = DataContext.Database.SqlQuery<OvertimeExceptionModel>(sqlCommand).ToList();
            if(overtimeExceptions !=null && overtimeExceptions.Any())
                isFound = true;
            else
                isFound = false;

            response = new BaseResponse
            {
                IsSuccess = !isFound, //not valid
                Message = isFound ? $" Already exist. Please try again" : "Success"
            };

            return response;
        }
    }
}
