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
    public interface IUnionService : IServiceBase<LgUnion>
    {
        LgUnion GetByName(string name);
        IEnumerable<LgUnion> GetLgUnionListByFilter(BaseSearchFilter filter);
        IEnumerable<ValidationResult> IsValidUnion(string unionCode);
    }
    public class UnionService : IUnionService
    {
        private readonly IUnionRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;
        private readonly ICacheManagerService cacheManagerService;

        public UnionService(IUnionRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
            this.cacheManagerService = cacheManagerService;

        }
        public IEnumerable<LgUnion> GetAll()
        {
            var entities = repository.GetAll().OrderBy(c => c.union_id);
            return entities;
        }

        public LgUnion GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public LgUnion Create(LgUnion objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(LgUnion objectToUpdate)
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
            throw new NotImplementedException(); ;
        }


        public bool IsContinued(long id)
        {
            throw new NotImplementedException();
        }

        public LgUnion Get(Expression<Func<LgUnion, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<LgUnion> GetMany(Expression<Func<LgUnion, bool>> where)
        {
            var entities = repository.GetMany(where);
            return entities;
        }

        public LgUnion GetByName(string name)
        {
            var single = new LgUnion();
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                    return null;

                using (var db = new gHRMDBContext())
                {
                    single = db.LgUnions
                        .FirstOrDefault(f => f.union_name_eng.Trim().ToLower() == name.Trim().ToLower());
                }
            }
            catch
            {
                return null;
            }

            return single;
        }

        public IEnumerable<LgUnion> GetLgUnionListByFilter(BaseSearchFilter filter)
        {
            var entities = cacheManagerService.GetAllLgUnions();
            if (entities.Any())
            {
                entities = entities.Where(f => f.thana_id == filter.ThanaId)
                .OrderBy(o => o.union_name_eng);
            }

            return entities;
        }
        IEnumerable<ValidationResult> IsValidUnion(string unionCode)
        {
            var entity = repository.Get(p => p.union_code == unionCode);
            if (entity != null)
            {

                yield return new ValidationResult("ThanaCode", "Duplicate Thana Code.");

            }
        }        
        #region Asyc
        public virtual async Task<IEnumerable<LgUnion>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }


        public virtual async Task<IEnumerable<LgUnion>> GetManyAsync(Expression<Func<LgUnion, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }



        public virtual async Task<LgUnion> GetAsync(Expression<Func<LgUnion, bool>> where)
        {
            return await repository.GetAsync(where);
        }

        IEnumerable<ValidationResult> IUnionService.IsValidUnion(string unionCode)
        {
            throw new NotImplementedException();
        }
        #endregion

    }
}
