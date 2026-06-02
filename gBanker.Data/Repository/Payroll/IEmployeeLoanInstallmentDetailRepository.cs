using gHRM.Core.Utilities.Constants;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.CodeFirstMigration.Payroll;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.Repository.Payroll
{
    public interface IEmployeeLoanInstallmentDetailRepository : IRepository<EmployeeLoanInstallmentDetail>
    {
        Task<bool> IsExistRunningLoan(int employeeId);
        Task<bool> IsExistRunningLoan2(int employeeId, int prcomponentid);
        List<EmployeeLoanInstallmentDetail> AddEmployeeLoanInstallmentDetail(List<EmployeeLoanInstallmentDetail> objs);
    }
    public class EmployeeLoanInstallmentDetailRepository : RepositoryBaseCodeFirst<EmployeeLoanInstallmentDetail>, IEmployeeLoanInstallmentDetailRepository
    {
        public EmployeeLoanInstallmentDetailRepository(IDatabaseFactoryCodeFirst databaseFactory)  //check
            : base(databaseFactory)
        {

        }
        public List<EmployeeLoanInstallmentDetail> AddEmployeeLoanInstallmentDetail(List<EmployeeLoanInstallmentDetail> objs)
        {
            DataContext.EmployeeLoanInstallmentDetail.AddRange(objs);
            DataContext.SaveChanges();
            return objs;
        }

        public async Task<bool> IsExistRunningLoan(int employeeId)
        {
           return await DataContext.LoanInstallmentDetail.AnyAsync(f=> f.IsActive
                                                        && f.EmployeeId==employeeId 
                                                   &&   f.LoanStatus==LoanStatusConstants.Running
                                                );           
        }
        public async Task<bool> IsExistRunningLoan2(int employeeId, int prcompnentid)
        {
            return await DataContext.LoanInstallmentDetail.AnyAsync(f => f.IsActive
                                                         && f.EmployeeId == employeeId
                                                    && f.LoanStatus == LoanStatusConstants.Running && f.PRComponentId == prcompnentid
                                                 );
        }
    }
}
