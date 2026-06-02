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
    public interface IEmployeeRosterScheduleService : IServiceBase<EmployeeRosterSchedule>
    {


    }
    public class EmployeeRosterScheduleService : IEmployeeRosterScheduleService
    {
        private readonly IEmployeeRosterScheduleRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public EmployeeRosterScheduleService(IEmployeeRosterScheduleRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        public IEnumerable<EmployeeRosterSchedule> GetAll()
        {
            var entities = repository.GetAll().Where(c => c.IsActive == true).OrderBy(c => c.EmployeeRosterScheduleId);
            return entities;
        }

        public EmployeeRosterSchedule GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public EmployeeRosterSchedule Create(EmployeeRosterSchedule objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(EmployeeRosterSchedule objectToUpdate)
        {
            repository.Update(objectToUpdate);
            Save();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
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

        public EmployeeRosterSchedule Get(Expression<Func<EmployeeRosterSchedule, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<EmployeeRosterSchedule> GetMany(Expression<Func<EmployeeRosterSchedule, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }

        #region Asyc
        public virtual async Task<IEnumerable<EmployeeRosterSchedule>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }


        public virtual async Task<IEnumerable<EmployeeRosterSchedule>> GetManyAsync(Expression<Func<EmployeeRosterSchedule, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }



        public virtual async Task<EmployeeRosterSchedule> GetAsync(Expression<Func<EmployeeRosterSchedule, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion
    }
}
