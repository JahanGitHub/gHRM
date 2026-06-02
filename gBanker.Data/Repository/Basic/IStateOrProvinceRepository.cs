using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.DBDetailModels;
using System.Collections.Generic;
using System.Linq;
namespace gHRM.Data.Repository
{
    public interface IStateOrProvinceRepository : IRepository<StateOrProvince>
    {
        IEnumerable<DBStateOrProvinceOrDivisionDetailModel> GetStateOrProvinceOrDivisionDetail(int cotryId, int statProId, int startRowIndex, string jtSorting, int pageSize, out long TotCount);
    }
    public class StateOrProvinceRepository : RepositoryBaseCodeFirst<StateOrProvince>, IStateOrProvinceRepository
    {
        public StateOrProvinceRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
        public IEnumerable<DBStateOrProvinceOrDivisionDetailModel> GetStateOrProvinceOrDivisionDetail(int cotryId, int statProId, int startRowIndex, string jtSorting, int pageSize, out long TotCount)
        {
            IQueryable<StateOrProvince> results = null;
            if (cotryId > 0 && statProId == 0)
                results = DataContext.StateOrProvinces.Where(x =>x.CountryId == 18);
            else if (statProId > 0)
                results = DataContext.StateOrProvinces.Where(x =>x.StateOrProvinceId == statProId);          
            else
                results = DataContext.StateOrProvinces;

            //else
            //    results = DataContext.StateOrProvinces.Where(x =>x.StateOrProvinceId);

            TotCount = results.LongCount();

            var obj = results.OrderBy(o => o.StateOrProvinceId).Skip(startRowIndex).Take(pageSize).Select(s => new DBStateOrProvinceOrDivisionDetailModel()
            {
                CountryId = s.CountryId,
                CountryName = s.Country.CountryName,                
                StateOrProvinceId=s.StateOrProvinceId,
                Code =s.Code,                
                Name = s.Name,
                               
            });

            if (!string.IsNullOrWhiteSpace(jtSorting))
            {
                if (jtSorting == "Code ASC")
                    return obj.OrderBy(o => o.Code);
                else if (jtSorting == "Code DESC")
                    return obj.OrderByDescending(o => o.Code);
                if (jtSorting == "Name ASC")
                    return obj.OrderBy(o => o.Name);
                else if (jtSorting == "Name DESC")
                    return obj.OrderByDescending(o => o.Name);
                else if (jtSorting == "CountryId ASC")
                    return obj.OrderBy(o => o.CountryId);
                else if (jtSorting == "CountryId DESC")
                    return obj.OrderByDescending(o => o.CountryId);
                else if (jtSorting == "CountryName ASC")
                    return obj.OrderBy(o => o.CountryName);
                else if (jtSorting == "CountryName DESC")
                    return obj.OrderByDescending(o => o.CountryName);
                else if (jtSorting == "CountryShortCode ASC")
                    return obj.OrderBy(o => o.CountryShortCode);
                else if (jtSorting == "CountryShortCode DESC")
                   return obj.OrderByDescending(o => o.CountryShortCode);
               
                else
                    return obj.OrderBy(o => o.Code);
            }
            else
                return obj.OrderBy(o => o.Code);
        }

    }
}
