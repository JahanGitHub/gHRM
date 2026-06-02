using eRecruitment.Infrastructure.Service.CacheManagerServices;
using gHRM.Core.Utilities.eRecruitUtilities;
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.Repository.CacheManagerRepositories;
using System.Collections.Generic;

namespace gHRM.Service.CacheManagerServices
{
    public class CacheManagerService : ICacheManagerService
    {
        private readonly ICacheManagerRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public CacheManagerService(ICacheManagerRepository repository, 
            
            IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        #region Country
        public IEnumerable<Country> GetAllCountries()
        {
            var cacheKey = string.Format(ErecruitmentCacheKeyConstants.COUNTRY);
            IEnumerable<Country> countries = null;
            if (Helper.PageRequestHelper.IsInCache(cacheKey))
            {
                countries = Helper.PageRequestHelper.GetCacheData(cacheKey) as List<Country>;
                return countries;
            }

            countries = repository.GetCountryToStoreInCache();
            Helper.PageRequestHelper.CacheData(cacheKey, countries);

            return countries;
        }

        public IEnumerable<Country> ResetCountryCache()
        {
            var cacheKey = string.Format(ErecruitmentCacheKeyConstants.COUNTRY);
            //let's remove cache
            Helper.PageRequestHelper.PurgeCacheItems(cacheKey);

            IEnumerable<Country> listings = null;
            listings = repository.GetCountryToStoreInCache();

            //store data into cache
            Helper.PageRequestHelper.CacheData(cacheKey, listings);

            return listings;
        }

        #endregion

        #region State Or Province
        public IEnumerable<StateOrProvince> GetAllStateOrProvinces()
        {
            var cacheKey = string.Format(ErecruitmentCacheKeyConstants.STATEORPROVINCE);
            IEnumerable<StateOrProvince> stateOrProvinces = null;
            if (Helper.PageRequestHelper.IsInCache(cacheKey))
            {
                stateOrProvinces = Helper.PageRequestHelper.GetCacheData(cacheKey) as List<StateOrProvince>;
                return stateOrProvinces;
            }

            stateOrProvinces = repository.GetStateOrProvinceToStoreInCache();
            Helper.PageRequestHelper.CacheData(cacheKey, stateOrProvinces);

            return stateOrProvinces;
        }

        public IEnumerable<StateOrProvince> ResetStateOrProvinceCache()
        {
            var cacheKey = string.Format(ErecruitmentCacheKeyConstants.STATEORPROVINCE);
            //let's remove cache
            Helper.PageRequestHelper.PurgeCacheItems(cacheKey);

            IEnumerable<StateOrProvince> listings = null;
            listings = repository.GetStateOrProvinceToStoreInCache();

            //store data into cache
            Helper.PageRequestHelper.CacheData(cacheKey, listings);

            return listings;
        }

        #endregion

        #region District
        public IEnumerable<District> GetAllDistricts()
        {
            var cacheKey = string.Format(ErecruitmentCacheKeyConstants.DISTRICT);
            IEnumerable<District> districts = null;
            if (Helper.PageRequestHelper.IsInCache(cacheKey))
            {
                districts = Helper.PageRequestHelper.GetCacheData(cacheKey) as List<District>;
                return districts;
            }

            districts = repository.GetDistrictToStoreInCache();
            Helper.PageRequestHelper.CacheData(cacheKey, districts);

            return districts;
        }

        public IEnumerable<District> ResetDistrictCache()
        {
            var cacheKey = string.Format(ErecruitmentCacheKeyConstants.DISTRICT);
            //let's remove cache
            Helper.PageRequestHelper.PurgeCacheItems(cacheKey);

            IEnumerable<District> listings = null;
            listings = repository.GetDistrictToStoreInCache();

            //store data into cache
            Helper.PageRequestHelper.CacheData(cacheKey, listings);

            return listings;
        }

        #endregion

        #region LgThana
        public IEnumerable<LgThana> GetAllLgThanas()
        {
            var cacheKey = string.Format(ErecruitmentCacheKeyConstants.LGTHANA);
            IEnumerable<LgThana> lgThanas = null;
            if (Helper.PageRequestHelper.IsInCache(cacheKey))
            {
                lgThanas = Helper.PageRequestHelper.GetCacheData(cacheKey) as List<LgThana>;
                return lgThanas;
            }

            lgThanas = repository.GetLgThanaToStoreInCache();
            Helper.PageRequestHelper.CacheData(cacheKey, lgThanas);

            return lgThanas;
        }

        public IEnumerable<LgThana> ResetLgThanaCache()
        {
            var cacheKey = string.Format(ErecruitmentCacheKeyConstants.LGTHANA);
            //let's remove cache
            Helper.PageRequestHelper.PurgeCacheItems(cacheKey);

            IEnumerable<LgThana> listings = null;
            listings = repository.GetLgThanaToStoreInCache();

            //store data into cache
            Helper.PageRequestHelper.CacheData(cacheKey, listings);

            return listings;
        }

        #endregion

        #region LgUnion
        public IEnumerable<LgUnion> GetAllLgUnions()
        {
            var cacheKey = string.Format(ErecruitmentCacheKeyConstants.LGUNION);
            IEnumerable<LgUnion> lgUnions = null;
            if (Helper.PageRequestHelper.IsInCache(cacheKey))
            {
                lgUnions = Helper.PageRequestHelper.GetCacheData(cacheKey) as List<LgUnion>;
                return lgUnions;
            }

            lgUnions = repository.GetLgUnionToStoreInCache();
            Helper.PageRequestHelper.CacheData(cacheKey, lgUnions);

            return lgUnions;
        }

        public IEnumerable<LgUnion> ResetLgUnionCache()
        {
            var cacheKey = string.Format(ErecruitmentCacheKeyConstants.LGUNION);
            //let's remove cache
            Helper.PageRequestHelper.PurgeCacheItems(cacheKey);

            IEnumerable<LgUnion> listings = null;
            listings = repository.GetLgUnionToStoreInCache();

            //store data into cache
            Helper.PageRequestHelper.CacheData(cacheKey, listings);

            return listings;
        }

        #endregion

        #region Education Degree
        public IEnumerable<EducationDegree> GetAllEducationDegrees()
        {
            var cacheKey = string.Format(ErecruitmentCacheKeyConstants.EDUCATIONDEGREE);
            IEnumerable<EducationDegree> educationDegrees = null;
            if (Helper.PageRequestHelper.IsInCache(cacheKey))
            {
                educationDegrees = Helper.PageRequestHelper.GetCacheData(cacheKey) as List<EducationDegree>;
                return educationDegrees;
            }

            educationDegrees = repository.GetEducationDegreeToStoreInCache();
            Helper.PageRequestHelper.CacheData(cacheKey, educationDegrees);

            return educationDegrees;
        }

        public IEnumerable<EducationDegree> ResetEducationDegreeCache()
        {
            var cacheKey = string.Format(ErecruitmentCacheKeyConstants.EDUCATIONDEGREE);
            //let's remove cache
            Helper.PageRequestHelper.PurgeCacheItems(cacheKey);

            IEnumerable<EducationDegree> listings = null;
            listings = repository.GetEducationDegreeToStoreInCache();

            //store data into cache
            Helper.PageRequestHelper.CacheData(cacheKey, listings);

            return listings;
        }

        #endregion

        #region Private Methods



        #endregion
    }
}
