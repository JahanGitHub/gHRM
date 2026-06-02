using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.CodeFirstMigration.PF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.Repository.PF
{
    public interface ILoanTypeRepository : IRepository<LoanType>
    {
        IEnumerable<LoanType> GetLoanTypeByName(string loanType);
        LoanType GetLoanTypeLoanTypeId(int loanTypeId);
    }
    //public class LoanTypeRepository : PFRepositoryBaseCodeFirst<LoanType>, ILoanTypeRepository
    public class LoanTypeRepository : RepositoryBaseCodeFirst<LoanType>, ILoanTypeRepository
    {
        public LoanTypeRepository(IDatabaseFactoryCodeFirst databaseFactory): base(databaseFactory)
        {
        }

        public IEnumerable<LoanType> GetLoanTypeByName(string loanType)
        {
            IQueryable<LoanType> results = null;
            results = DataContext.LoanType.Where(x => x.LoanTypeName == loanType);
            return results;
        }
        public LoanType GetLoanTypeLoanTypeId(int loanTypeId)
        {
            LoanType results = null;
            results = DataContext.LoanType.Where(x => x.LoanTypeId == loanTypeId && x.IsDeleted == false).FirstOrDefault();
            return results;
        }

       

    }
}
