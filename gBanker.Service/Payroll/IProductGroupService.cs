using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.CodeFirstMigration.Payroll;
using gHRM.Data.Repository.Payroll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Service.Payroll
{
    public interface IProductGroupService : IServiceBase<ProductGroup>
    {

    }
    public class ProductGroupService : IProductGroupService
    {
        private readonly IProductGroupRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public ProductGroupService(IProductGroupRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<ProductGroup> GetAll()
        {
            var entities = repository.GetAll().OrderBy(c => c.ProductGroupName);
            return entities;
        }

        public ProductGroup GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public ProductGroup Create(ProductGroup objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(ProductGroup objectToUpdate)
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


        public ProductGroup Get(Expression<Func<ProductGroup, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<ProductGroup> GetMany(Expression<Func<ProductGroup, bool>> where)
        {
            var entities = repository.GetMany(where);
            return entities;
        }

        #region Asyc
        public virtual async Task<IEnumerable<ProductGroup>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }


        public virtual async Task<IEnumerable<ProductGroup>> GetManyAsync(Expression<Func<ProductGroup, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }



        public virtual async Task<ProductGroup> GetAsync(Expression<Func<ProductGroup, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion
    }
}
