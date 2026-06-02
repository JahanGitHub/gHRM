using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.DBDetailModels;
using System.Collections.Generic;
using System.Linq;

namespace gHRM.Data.Repository
{
    public interface IBranchRepository:IRepository<Branch>
    {
        IEnumerable<DBBranchDetails> GetBranchDetail(int companyId, int branchId, int startRowIndex, string jtSorting, int pageSize, out long TotCount);
    }
    public class BranchRepository : RepositoryBaseCodeFirst<gHRM.Data.CodeFirstMigration.Branch>, IBranchRepository
    {
        public BranchRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
        public IEnumerable<DBBranchDetails> GetBranchDetail(int companyId, int branchId, int startRowIndex, string jtSorting, int pageSize, out long TotCount)
        {
            IQueryable<Branch> results = null;
            if (companyId > 0 && branchId==0)
                results = DataContext.Branches.Where(x =>x.IsActive==true && x.CompanyId == companyId);
            else if (branchId > 0)
                results = DataContext.Branches.Where(x => x.IsActive==true && x.BranchId == branchId);
            else
                results = DataContext.Branches.Where(x => x.IsActive==true);

            TotCount = results.LongCount();

            var obj = results.OrderBy(o => o.BranchId).Skip(startRowIndex).Take(pageSize).Select(s => new DBBranchDetails()
            {
                BranchId = s.BranchId,
                BranchName = s.BranchName,
                BranchAddress = s.BranchAddress,
                BranchEmail = s.BranchEmail,
                BranchPhone = s.BranchPhone,
                CompanyId = s.CompanyId,
                CompanyName = s.Company.CompanyName,              
            });

            if (!string.IsNullOrWhiteSpace(jtSorting))
            {
                if (jtSorting == "BranchId ASC")
                    return obj.OrderBy(o => o.BranchId);
                else if (jtSorting == "BranchId DESC")
                    return obj.OrderByDescending(o => o.BranchId);
                else if (jtSorting == "BranchName ASC")
                    return obj.OrderBy(o => o.BranchName);
                else if (jtSorting == "BranchName DESC")
                    return obj.OrderByDescending(o => o.BranchName);
                else if (jtSorting == "BranchAddress ASC")
                    return obj.OrderBy(o => o.BranchAddress);
                else if (jtSorting == "BranchAddress DESC")
                    return obj.OrderByDescending(o => o.BranchAddress);               
                else
                    return obj.OrderBy(o => o.BranchId);
            }
            else
                return obj.OrderBy(o => o.BranchId);
        }
    }
}
