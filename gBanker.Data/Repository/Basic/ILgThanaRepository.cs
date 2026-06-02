using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.DBDetailModels;
using System.Collections.Generic;
using System.Linq;

namespace gHRM.Data.Repository
{
    public interface ILgThanaRepository : IRepository<LgThana>
    {
       // IEnumerable<DBDistrictDetailModel> GetDistrictDetail(string filterColumnName, string filterValue, int startRowIndex, string jtSorting, int pageSize, out long TotCount);
        IEnumerable<DBThanaDetailsModel> GetThanaDetail(int DistID, string filterColumnName, string filterValue, int startRowIndex, string jtSorting, int pageSize, out long TotCount);
    }
    public class LgThanaRepository : RepositoryBaseCodeFirst<gHRM.Data.CodeFirstMigration.LgThana>, ILgThanaRepository
    {
        public LgThanaRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
        public IEnumerable<DBThanaDetailsModel> GetThanaDetail(int DistID, string filterColumnName, string filterValue, int startRowIndex, string jtSorting, int pageSize, out long TotCount)
        {

            IQueryable<LgThana> results = null;
            
            if (DistID > 0)
            {
                if (filterColumnName == "thana_code")
                    results = DataContext.LgThanas.Where(x => x.thana_code.Contains(filterValue) && x.district_id == DistID);
                else if (filterColumnName == "thana_name_eng")
                    results = DataContext.LgThanas.Where(x => x.thana_name_eng.Contains(filterValue) && x.district_id == DistID);
                else
                    results = DataContext.LgThanas.Where(x => x.district_id == DistID);                
            }
            else
            {
                if (filterColumnName == "thana_code")
                    results = DataContext.LgThanas.Where(x => x.thana_code.Contains(filterValue));
                else if (filterColumnName == "thana_name_eng")
                    results = DataContext.LgThanas.Where(x => x.thana_name_eng.Contains(filterValue));
                else
                    results = DataContext.LgThanas;
            }


            TotCount = results.LongCount();

            var obj = results.OrderBy(o => o.thana_id).Skip(startRowIndex).Take(pageSize).Select(s => new DBThanaDetailsModel ()
            {
                thana_id=s.thana_id ,
                district_name_eng =s.District.district_name_eng ,
                thana_code = s.thana_code,
                thana_name_eng = s.thana_name_eng ,
                                                                     // DataFilter               
            });

            if (!string.IsNullOrWhiteSpace(jtSorting))
            {
                if (jtSorting == "district_name_eng ASC")
                    return obj.OrderBy(o => o.district_name_eng  );
                else if (jtSorting == "district_name_eng DESC")
                    return obj.OrderByDescending(o => o.district_name_eng );
                else if (jtSorting == "thana_code ASC")
                    return obj.OrderBy(o => o.thana_code);
                else if (jtSorting == "thana_code DESC")
                    return obj.OrderByDescending(o => o.thana_code);
                else if (jtSorting == "thana_name_eng ASC")                       //DataSorting
                    return obj.OrderBy(o => o.thana_name_eng  );
                else if (jtSorting == "thana_name_eng DESC")
                    return obj.OrderByDescending(o => o.thana_name_eng);              

                else
                    return obj.OrderBy(o => o.district_name_eng );
            }
            else
                return obj.OrderBy(o => o.district_name_eng );
        }
    }
}
