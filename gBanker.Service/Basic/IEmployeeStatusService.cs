//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace gHRM.Service
//{
//    interface IEmployeeStatusService
//    {
//    }
//}
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
    public interface IEmployeeStatusService : IServiceBase<EmployeeStatus>
    {
    }
    public class EmployeeStatusService : IEmployeeStatusService
    {
        private readonly IEmployeeStatusRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public EmployeeStatusService(IEmployeeStatusRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        public IEnumerable<EmployeeStatus> GetAll()
        {
            var entities = repository.GetAll().OrderBy(c => c.StatusName);
            return entities;
        }

        public EmployeeStatus GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public EmployeeStatus Create(EmployeeStatus objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(EmployeeStatus objectToUpdate)
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

        public EmployeeStatus Get(Expression<Func<EmployeeStatus, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<EmployeeStatus> GetMany(Expression<Func<EmployeeStatus, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }

        #region Asyc
        public virtual async Task<IEnumerable<EmployeeStatus>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }


        public virtual async Task<IEnumerable<EmployeeStatus>> GetManyAsync(Expression<Func<EmployeeStatus, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }



        public virtual async Task<EmployeeStatus> GetAsync(Expression<Func<EmployeeStatus, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion

    }
}
