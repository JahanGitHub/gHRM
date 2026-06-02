
using gHRM.Core.Common;
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.DBDetailModels;
using gHRM.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Service
{
    public interface IEmployeeStatusHistoryService : IServiceBase<EmployeeStatusHistory>
    {
        EmployeeStatusHistory GetByEmployeeId(Int64 id);  
    }
    public class EmployeeStatusHistoryService : IEmployeeStatusHistoryService
    {
        private readonly IEmployeeStatusHistoryRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public EmployeeStatusHistoryService(IEmployeeStatusHistoryRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<EmployeeStatusHistory> GetAll()
        {
            var entities = repository.GetAll().Where(b => b.IsActive == true).OrderBy(c => c.HistoryId);
            return entities;
        }

        
        public EmployeeStatusHistory GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }
        public EmployeeStatusHistory GetByEmployeeId(long id)
        {
            //var entities = repository.GetAll().Where(b => b.IsActive == true && b.EmployeeId == id).First();
            //return entities;
            var entity = repository.Get(e => e.EmployeeId == id && e.IsActive == true);
            return entity;
        }
        public EmployeeStatusHistory Create(EmployeeStatusHistory objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(EmployeeStatusHistory objectToUpdate)
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

        public EmployeeStatusHistory Get(Expression<Func<EmployeeStatusHistory, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<EmployeeStatusHistory> GetMany(Expression<Func<EmployeeStatusHistory, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }

        #region Asyc
        public virtual async Task<IEnumerable<EmployeeStatusHistory>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }


        public virtual async Task<IEnumerable<EmployeeStatusHistory>> GetManyAsync(Expression<Func<EmployeeStatusHistory, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }



        public virtual async Task<EmployeeStatusHistory> GetAsync(Expression<Func<EmployeeStatusHistory, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion
        public bool Inactivate(long id, DateTime? inactiveDate)
        {
            throw new NotImplementedException();
        }
        public bool IsContinued(long id)
        {
            throw new NotImplementedException();
        }
    }
}

