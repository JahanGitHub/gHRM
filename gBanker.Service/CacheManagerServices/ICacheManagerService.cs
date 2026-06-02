using gHRM.Data.CodeFirstMigration;
using System.Collections.Generic;

namespace eRecruitment.Infrastructure.Service.CacheManagerServices
{
    public interface ICacheManagerService
    {
        IEnumerable<Country> GetAllCountries();
        IEnumerable<Country> ResetCountryCache();    
        
        IEnumerable<StateOrProvince> GetAllStateOrProvinces();
        IEnumerable<StateOrProvince> ResetStateOrProvinceCache();

        IEnumerable<District> GetAllDistricts(); 
        IEnumerable<District> ResetDistrictCache();

        IEnumerable<LgThana> GetAllLgThanas(); 
        IEnumerable<LgThana> ResetLgThanaCache();

        IEnumerable<LgUnion> GetAllLgUnions();
        IEnumerable<LgUnion> ResetLgUnionCache();

        IEnumerable<EducationDegree> GetAllEducationDegrees();
        IEnumerable<EducationDegree> ResetEducationDegreeCache();
    }
}
