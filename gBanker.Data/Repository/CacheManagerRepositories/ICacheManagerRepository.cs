using gHRM.Data.CodeFirstMigration;
using System.Collections.Generic;

namespace gHRM.Data.Repository.CacheManagerRepositories
{
    public interface ICacheManagerRepository
    {
        IEnumerable<Country> GetCountryToStoreInCache();
        IEnumerable<StateOrProvince> GetStateOrProvinceToStoreInCache();
        IEnumerable<District> GetDistrictToStoreInCache();
        IEnumerable<LgThana> GetLgThanaToStoreInCache();
        IEnumerable<LgUnion> GetLgUnionToStoreInCache();
        IEnumerable<EducationDegree> GetEducationDegreeToStoreInCache();
    }
}
