using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using System.Collections.Generic;
using System.Linq;

namespace gHRM.Data.Repository.CacheManagerRepositories
{
    public class CacheManagerRepository : RepositoryBaseCodeFirst<object>, ICacheManagerRepository
    {
        public CacheManagerRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }             

        public IEnumerable<Country> GetCountryToStoreInCache()
        {           
            var listings = DataContext.Countries.AsParallel().ToList();
            return listings;
        }

        public IEnumerable<StateOrProvince> GetStateOrProvinceToStoreInCache()
        {
            var listings = DataContext.StateOrProvinces.Where(f=>f.Status).AsParallel().ToList();
            return listings;
        }

        public IEnumerable<District> GetDistrictToStoreInCache()
        {
            var listings = DataContext.Districts.Where(f => f.IsActive).AsParallel().ToList();
            return listings;
        }

        public IEnumerable<LgThana> GetLgThanaToStoreInCache()
        {
            var listings = DataContext.LgThanas.AsParallel().ToList();
            return listings;
        }

        public IEnumerable<LgUnion> GetLgUnionToStoreInCache()
        {
            var listings = DataContext.LgUnions.AsParallel().ToList();
            return listings;
        }
        public IEnumerable<EducationDegree> GetEducationDegreeToStoreInCache()
        {
            var listings = DataContext.EducationDegree.Where(f=>f.IsActive==true).AsParallel().ToList();
            return listings;
        }
    }
}
