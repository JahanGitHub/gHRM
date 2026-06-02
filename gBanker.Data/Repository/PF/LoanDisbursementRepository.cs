using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.CodeFirstMigration.Loan;
using gHRM.Data.CodeFirstMigration.PF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.Repository.PF
{
    public interface ILoanDisbursementRepository : IRepository<LoanDisbursement>
    {
        IEnumerable<LoanDisbursement> GetLoanDisburseInfoByEmployeeId(long employeeId, int loanTypeId);
    }
   
    public class LoanDisbursementRepository : RepositoryBaseCodeFirst<LoanDisbursement>, ILoanDisbursementRepository
    {
        public LoanDisbursementRepository(IDatabaseFactoryCodeFirst databaseFactory): base(databaseFactory)
        {
            
        }

        public IEnumerable<LoanDisbursement> GetLoanDisburseInfoByEmployeeId(long employeeId, int loanTypeId)
        {
            IQueryable<LoanDisbursement> results = null;
            results = DataContext.LoanDisbursements.Where(x => x.EmployeeId == employeeId  && x.IsDeleted == false);
            return results;
        }

       
    }
}
