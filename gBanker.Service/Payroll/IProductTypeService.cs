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
    public interface IProductTypeService : IServiceBase<ProductType>
    {

    }
    public class ProductTypeService : IProductTypeService
    {
        private readonly IProductTypeRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public ProductTypeService(IProductTypeRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<ProductType> GetAll()
        {
            var entities = repository.GetAll().OrderBy(c => c.ProductTypeName);
            return entities;
        }

        public ProductType GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public ProductType Create(ProductType objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(ProductType objectToUpdate)
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


        public ProductType Get(Expression<Func<ProductType, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<ProductType> GetMany(Expression<Func<ProductType, bool>> where)
        {
            var entities = repository.GetMany(where);
            return entities;
        }

        #region Asyc
        public virtual async Task<IEnumerable<ProductType>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }


        public virtual async Task<IEnumerable<ProductType>> GetManyAsync(Expression<Func<ProductType, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }



        public virtual async Task<ProductType> GetAsync(Expression<Func<ProductType, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion
    }
}
