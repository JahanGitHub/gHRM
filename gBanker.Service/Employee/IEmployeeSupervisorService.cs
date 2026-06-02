//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace gHRM.Service
//{
//    interface IEmployeeSupervisorService
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
    public interface IEmployeeSupervisorService : IServiceBase<EmployeeSupervisor>
    {

    }
    public class EmployeeSupervisorService : IEmployeeSupervisorService
    {
        private readonly IEmployeeSupervisorRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public EmployeeSupervisorService(IEmployeeSupervisorRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<EmployeeSupervisor> GetAll()
        {
            var entities = repository.GetAll().OrderBy(c => c.Id);
            return entities;
        }

        public EmployeeSupervisor GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public EmployeeSupervisor Create(EmployeeSupervisor objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(EmployeeSupervisor objectToUpdate)
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


        public EmployeeSupervisor Get(Expression<Func<EmployeeSupervisor, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<EmployeeSupervisor> GetMany(Expression<Func<EmployeeSupervisor, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }

        #region Asyc
        public virtual async Task<IEnumerable<EmployeeSupervisor>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }


        public virtual async Task<IEnumerable<EmployeeSupervisor>> GetManyAsync(Expression<Func<EmployeeSupervisor, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }



        public virtual async Task<EmployeeSupervisor> GetAsync(Expression<Func<EmployeeSupervisor, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion
    }
}
