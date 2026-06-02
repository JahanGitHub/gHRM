using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.Repository.Promotion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using gHRM.Data.CodeFirstMigration.Promotion;

namespace gHRM.Service
{
    public interface IPromotionTypeService : IServiceBase<PromotionType>
    {

    }
    public class PromotionTypeService : IPromotionTypeService
    {
        private readonly IPromotionTypeRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public PromotionTypeService(IPromotionTypeRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<PromotionType> GetAll()
        {
            var entities = repository.GetAll().OrderBy(c => c.PromotionTypeId);
            return entities;
        }

        public PromotionType GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public PromotionType Create(PromotionType objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(PromotionType objectToUpdate)
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

        public PromotionType Get(Expression<Func<PromotionType, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<PromotionType> GetMany(Expression<Func<PromotionType, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }

        #region Asyc
        public virtual async Task<IEnumerable<PromotionType>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }


        public virtual async Task<IEnumerable<PromotionType>> GetManyAsync(Expression<Func<PromotionType, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }



        public virtual async Task<PromotionType> GetAsync(Expression<Func<PromotionType, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion

    }
}
