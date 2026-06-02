using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.CodeFirstMigration.Loan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.Repository.Loan
{
    public interface ILoanPurposeRepository : IRepository<LoanPurpose> { }
    public class LoanPurposeRepository : RepositoryBaseCodeFirst<LoanPurpose>, ILoanPurposeRepository
    {
        public LoanPurposeRepository(IDatabaseFactoryCodeFirst databaseFactory) : base(databaseFactory) { }
        
    }
}
