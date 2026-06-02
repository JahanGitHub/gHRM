using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.CodeFirstMigration.Loan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.Repository.Loan
{
    public interface ILoanRegisterRepository : IRepository<LoanRegister> { }
    public class LoanRegisterRepository : RepositoryBaseCodeFirst<LoanRegister>, ILoanRegisterRepository
    {
        public LoanRegisterRepository(IDatabaseFactoryCodeFirst databaseFactory) : base(databaseFactory) { }
        
    }
}
