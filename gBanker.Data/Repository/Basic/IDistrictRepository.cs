using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.DBDetailModels;
using System.Collections.Generic;
using System.Linq;

namespace gHRM.Data.Repository
{
     public interface  IDistrictRepository :IRepository<District>
    {
         IEnumerable<DBDistrictDetailModel> GetDistrictDetail(int DivId, int DistId, int startRowIndex, string jtSorting, int pageSize, out long TotCount);
        //IEnumerable<Employee> GetEmployeeInfo(string filterColumnName, string filterValue, int startRowIndex, string jtSorting, int pageSize, out long TotCount);      
    }
   public class DistrictRepository : RepositoryBaseCodeFirst<gHRM.Data.CodeFirstMigration.District>, IDistrictRepository
    {
        public DistrictRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
        public IEnumerable<DBDistrictDetailModel> GetDistrictDetail(int DivId, int DistId, int startRowIndex, string jtSorting, int pageSize, out long TotCount)
        {
            IQueryable<District> results = null;
            if (DivId > 0 && DistId == 0)
                results = DataContext.Districts.Where(x => x.division_Id==DivId);
            else if (DistId > 0)
                results = DataContext.Districts.Where(x =>x.district_id == DistId);
            else
                results = DataContext.Districts;

            TotCount = results.LongCount();

            var obj = results.OrderBy(o => o.district_id).Skip(startRowIndex).Take(pageSize).Select(s => new DBDistrictDetailModel()
            {
                district_id = s.district_id,
                district_code = s.district_code,
                district_name_eng = s.district_name_eng,
                StateOrProvinceId = s.StateOrProvince.StateOrProvinceId,
                Name=s.StateOrProvince.Name                                                         // DataFilter               
            });

            if (!string.IsNullOrWhiteSpace(jtSorting))
            {
                if (jtSorting == "district_id ASC")
                    return obj.OrderBy(o => o.district_id);
                else if (jtSorting == "district_id DESC")
                    return obj.OrderByDescending(o => o.district_id);
                else if (jtSorting == "district_code ASC")
                    return obj.OrderBy(o => o.district_code);
                else if (jtSorting == "district_code DESC")
                    return obj.OrderByDescending(o => o.district_code);
                else if (jtSorting == "district_name_eng ASC")                       //DataSorting
                    return obj.OrderBy(o => o.district_name_eng);
                else if (jtSorting == "district_name_eng DESC")
                    return obj.OrderByDescending(o => o.district_name_eng);
                else if (jtSorting == "Name ASC")                                           //DataSorting
                    return obj.OrderBy(o => o.Name);
                else if (jtSorting == "Name DESC")
                    return obj.OrderByDescending(o => o.Name);
                else if (jtSorting == "StateOrProvinceId ASC")
                    return obj.OrderBy(o => o.StateOrProvinceId);
                else if (jtSorting == "StateOrProvinceId DESC")
                    return obj.OrderByDescending(o => o.StateOrProvinceId);
                
                else
                    return obj.OrderBy(o => o.district_id);
            }
            else
                return obj.OrderBy(o => o.district_id);
        }
    }
}
