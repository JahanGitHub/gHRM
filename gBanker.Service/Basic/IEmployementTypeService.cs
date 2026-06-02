using gHRM.Core.Common;
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Service
{
    public interface IEmployementTypeService : IServiceBase<EmployementType>
    {

    }
    public class EmployementTypeService : IEmployementTypeService
    {
        private readonly IEmployementTypeRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public EmployementTypeService(IEmployementTypeRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<EmployementType> GetAll()
        {
            var entities = repository.GetAll().OrderBy(c => c.ViewOrder);
            return entities;
        }

        public EmployementType GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public EmployementType Create(EmployementType objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(EmployementType objectToUpdate)
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

        public EmployementType Get(Expression<Func<EmployementType, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<EmployementType> GetMany(Expression<Func<EmployementType, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }

        #region Asyc
        public virtual async Task<IEnumerable<EmployementType>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }


        public virtual async Task<IEnumerable<EmployementType>> GetManyAsync(Expression<Func<EmployementType, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }



        public virtual async Task<EmployementType> GetAsync(Expression<Func<EmployementType, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion

    }
}
