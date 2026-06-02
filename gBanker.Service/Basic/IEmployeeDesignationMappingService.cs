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
    public interface IEmployeeDesignationMappingService : IServiceBase<EmployeeDesignationMapping>
    {


    }
    public class EmployeeDesignationMappingService : IEmployeeDesignationMappingService
    {
        private readonly IEmployeeDesignationMappingRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public EmployeeDesignationMappingService(IEmployeeDesignationMappingRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        public IEnumerable<EmployeeDesignationMapping> GetAll()
        {
            var entities = repository.GetAll().Where(c => c.IsActive == 1).OrderBy(c => c.DesignationMapId);
            return entities;
        }

        public EmployeeDesignationMapping GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public EmployeeDesignationMapping Create(EmployeeDesignationMapping objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(EmployeeDesignationMapping objectToUpdate)
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

        public bool Inactivate(long id, DateTime? inactiveDate)
        {
            throw new NotImplementedException();
        }

        public bool IsContinued(long id)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            unitOfWork.Commit();
        }

        public EmployeeDesignationMapping Get(Expression<Func<EmployeeDesignationMapping, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<EmployeeDesignationMapping> GetMany(Expression<Func<EmployeeDesignationMapping, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == 1);
            return entities;
        }

        #region Asyc
        public virtual async Task<IEnumerable<EmployeeDesignationMapping>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }


        public virtual async Task<IEnumerable<EmployeeDesignationMapping>> GetManyAsync(Expression<Func<EmployeeDesignationMapping, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }



        public virtual async Task<EmployeeDesignationMapping> GetAsync(Expression<Func<EmployeeDesignationMapping, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion
    }
}
