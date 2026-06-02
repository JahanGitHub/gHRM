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
    public interface IProductItemService : IServiceBase<ProductItem>
    {

    }
    public class ProductItemService : IProductItemService
    {
        private readonly IProductItemRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public ProductItemService(IProductItemRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<ProductItem> GetAll()
        {
            var entities = repository.GetAll().OrderBy(c => c.ProductItemName);
            return entities;
        }

        public ProductItem GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public ProductItem Create(ProductItem objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(ProductItem objectToUpdate)
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


        public ProductItem Get(Expression<Func<ProductItem, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<ProductItem> GetMany(Expression<Func<ProductItem, bool>> where)
        {
            var entities = repository.GetMany(where);
            return entities;
        }

        #region Asyc
        public virtual async Task<IEnumerable<ProductItem>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }


        public virtual async Task<IEnumerable<ProductItem>> GetManyAsync(Expression<Func<ProductItem, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }



        public virtual async Task<ProductItem> GetAsync(Expression<Func<ProductItem, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion
    }
}
