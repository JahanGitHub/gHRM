using gHRM.Data.CodeFirstMigration.Discipline;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.DBDetailModels.Discipline;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.Repository.Discipline
{
    public interface IDiscCrimeRepository : IRepository<DiscCrime>
    {
        IEnumerable<DBDiscCrimeDetailsModel> GetCrimeDetail(string filterColumnName, string filterValue, int startRowIndex, string jtSorting, int pageSize, out long TotCount);
    }
    public class DiscCrimeRepository : RepositoryBaseCodeFirst<DiscCrime>, IDiscCrimeRepository
    {
        public DiscCrimeRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
        public IEnumerable<DBDiscCrimeDetailsModel> GetCrimeDetail(string filterColumnName, string filterValue, int startRowIndex, string jtSorting, int pageSize, out long TotCount)
        {
            IQueryable<DiscCrime> results = null;
            if (filterColumnName == "CrimeCode")
                results = DataContext.DiscCrimes.Where(x => x.IsActive == true && x.CrimeCode.Contains(filterValue));
            else if (filterColumnName == "CrimeName")
                results = DataContext.DiscCrimes.Where(x => x.IsActive == true && x.CrimeName.Contains(filterValue));
            else
                results = DataContext.DiscCrimes.Where(x => x.IsActive == true);

            TotCount = results.LongCount();

            var obj = results.OrderBy(o => o.CrimeId).Skip(startRowIndex).Take(pageSize).Select(s => new DBDiscCrimeDetailsModel()
            {
                CrimeId = s.CrimeId,
                CrimeCode = s.CrimeCode,
                CrimeName = s.CrimeName,
                Remarks = s.Remarks,                                                       // DataFilter               
            });

            if (!string.IsNullOrWhiteSpace(jtSorting))
            {
                if (jtSorting == "CrimeCode ASC")
                    return obj.OrderBy(o => o.CrimeCode);
                else if (jtSorting == "CrimeCode DESC")
                    return obj.OrderByDescending(o => o.CrimeCode);
                else if (jtSorting == "CrimeName ASC")
                    return obj.OrderBy(o => o.CrimeName);
                else if (jtSorting == "CrimeName DESC")
                    return obj.OrderByDescending(o => o.CrimeName);
                else
                    return obj.OrderBy(o => o.CrimeId);
            }
            else
                return obj.OrderBy(o => o.CrimeId);
        }
    }
}
