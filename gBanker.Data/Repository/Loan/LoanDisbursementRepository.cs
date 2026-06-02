using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.CodeFirstMigration.Loan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.Repository.Loan
{
    public interface ILoanDisbursementRepository : IRepository<LoanDisbursement> { }
    public class LoanDisbursementRepository : RepositoryBaseCodeFirst<LoanDisbursement>, ILoanDisbursementRepository
    {
        public LoanDisbursementRepository(IDatabaseFactoryCodeFirst databaseFactory) : base(databaseFactory) { }
        
    }
}
