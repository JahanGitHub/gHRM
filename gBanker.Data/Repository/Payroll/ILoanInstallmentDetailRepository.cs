using gHRM.Core.Utilities;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.CodeFirstMigration.Payroll;
using gHRM.Data.DBDetailModels.Payroll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.Repository.Payroll
{
    public interface ILoanInstallmentDetailRepository : IRepository<LoanInstallmentDetail>
    {
        Task<BaseResponse> UpdatePreviousLoanAsClosed(UpdatePreviousLoanAsClosedModel model);
        List<LoanInstallmentDetail> AddLoanExcel(List<LoanInstallmentDetail> objs);
    }

    public class LoanInstallmentDetailRepository : RepositoryBaseCodeFirst<LoanInstallmentDetail>, ILoanInstallmentDetailRepository
    {

        public LoanInstallmentDetailRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }

        public List<LoanInstallmentDetail> AddLoanExcel(List<LoanInstallmentDetail> objs)
        {
            DataContext.LoanInstallmentDetail.AddRange(objs);
            DataContext.SaveChanges();
            return objs;
        }
        public async Task<BaseResponse> UpdatePreviousLoanAsClosed(UpdatePreviousLoanAsClosedModel model)
        {
            var response = new BaseResponse
            {
                IsSuccess = true,
                Message = "Update Successfull!"
            };
            try
            {

                var sqlCommand = $@"LoanInstallmentDetail_UpdatePreviousLoanAsClosed {model.EmployeeId},{model.LoanInstallmentDetailId}, '{model.PreviousLoanStatus}','{model.NewLoanStatus}'";
                await DataContext.Database.ExecuteSqlCommandAsync(sqlCommand);

                return response;
            }
            catch 
            {
                response.IsSuccess = false;
                return response;
            }            
        }
    }
}
