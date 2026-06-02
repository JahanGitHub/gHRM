using eRecruitment.Infrastructure.Service.CacheManagerServices;
using gHRM.Core.Common;
using gHRM.Core.Filters;
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.DBDetailModels;
using gHRM.Data.Repository;
using gHRM.Service.eRecruit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;


namespace gHRM.Service
{
    public interface IStateOrProvinceService : IServiceBase<StateOrProvince>
    {

        IEnumerable<ValidationResult> IsValidStateOrProvince(string stateorprovincecode);
        StateOrProvince GetByCountryId(int countryId);
        IEnumerable<StateOrProvince> GetAllDivision(int countryId);
        IEnumerable<DBStateOrProvinceOrDivisionDetailModel> GetStateOrProvinceOrDivisionDetail(int cotryId, int statProId, int startRowIndex, string jtSorting, int pageSize, out long TotCount);

        StateOrProvince GetByName(string name);

        IEnumerable<StateOrProvince> GetStateOrProvinceListByFilter(BaseSearchFilter filter);
    }
    public class StateOrProvinceService : IStateOrProvinceService
    {
        private readonly IStateOrProvinceRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        private readonly ICacheManagerService cacheManagerService;


        public StateOrProvinceService(IStateOrProvinceRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
            this.cacheManagerService = cacheManagerService;
        }
        public IEnumerable<StateOrProvince> GetAll()
        {
            var entities = repository.GetAll().Where( c=> c.Status == true).OrderBy(c => c.StateOrProvinceId);
            return entities;
        }

        public StateOrProvince GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public StateOrProvince GetByCountryId(int countryId)
        {
            var entity = repository.Get(e => e.CountryId == countryId);
            return entity;
        }
        public StateOrProvince Create(StateOrProvince objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(StateOrProvince objectToUpdate)
        {
            repository.Update(objectToUpdate);
            Save();
        }

        public void Delete(int id)
        {
            var entity = repository.GetById(id);
            repository.Delete(entity);
            Save();
        }

        public void Save()
        {
            unitOfWork.Commit();
        }

        public bool Inactivate(long id, DateTime? inactiveDate)
        {
            throw new NotImplementedException();
        }

        public bool IsContinued(long id)
        {
            throw new NotImplementedException();
        }        

        public IEnumerable<StateOrProvince> GetAllDivision(int countryId)
        {
            var entities = repository.GetAll().Where(w => w.CountryId == countryId).OrderBy(c => c.StateOrProvinceId);
            return entities;
        }

        IEnumerable<ValidationResult> IStateOrProvinceService.IsValidStateOrProvince(string stateorprovincecode)
        {
            var entity = repository.Get(p => p.CountryShortCode == stateorprovincecode);
            if (entity != null)
            {

                yield return new ValidationResult("StateOrProvinceCode", "Duplicate StateOrProvinceCode Code.");

            }
        }
        public IEnumerable<DBStateOrProvinceOrDivisionDetailModel> GetStateOrProvinceOrDivisionDetail(int cotryId, int statProId, int startRowIndex, string jtSorting, int pageSize, out long TotCount)
        {
            return repository.GetStateOrProvinceOrDivisionDetail(cotryId, statProId, startRowIndex, jtSorting, pageSize, out TotCount);
        }

        public StateOrProvince Get(Expression<Func<StateOrProvince, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<StateOrProvince> GetMany(Expression<Func<StateOrProvince, bool>> where)
        {
            var entities = repository.GetMany(where);
            return entities;
        }

        public StateOrProvince GetByName(string name)
        {
            var single = new StateOrProvince();
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                    return null;

                using (var db = new gHRMDBContext())
                {
                    single = db.StateOrProvinces
                        .FirstOrDefault(f => f.Name.Trim().ToLower() == name.Trim().ToLower());
                }
            }
            catch
            {
                return null;
            }

            return single;
        }

        public IEnumerable<StateOrProvince> GetStateOrProvinceListByFilter(BaseSearchFilter filter)
        {
            var entities = cacheManagerService.GetAllStateOrProvinces();
            if (entities.Any())
            {
                entities = entities.Where(f => f.CountryId == filter.CountryId)
                .OrderBy(o => o.Name);
            }

            return entities;
        }

        #region Asyc
        public virtual async Task<IEnumerable<StateOrProvince>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }


        public virtual async Task<IEnumerable<StateOrProvince>> GetManyAsync(Expression<Func<StateOrProvince, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }



        public virtual async Task<StateOrProvince> GetAsync(Expression<Func<StateOrProvince, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion
    }
}
