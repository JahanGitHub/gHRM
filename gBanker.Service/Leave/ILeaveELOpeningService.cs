using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.Repository;
using System.Linq.Expressions;

namespace gHRM.Service
{
    public interface ILeaveELOpeningService : IServiceBase<LeaveELOpening>
    {
        LeaveELOpening GetByEmployeeId(long empId);
        List<LeaveELOpening> AddELOpeningList(List<LeaveELOpening> objs);
    }

    public class LeaveELOpeningService : ILeaveELOpeningService
    {
        private readonly ILeaveELOpeningRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public LeaveELOpeningService(ILeaveELOpeningRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<LeaveELOpening> GetAll()
        {
            var entities = repository.GetAll().Where(c => c.IsActive == true).OrderBy(c => c.ELOpeningId);
            return entities;
        }

        public LeaveELOpening GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }
        public LeaveELOpening GetByEmployeeId(long empId)
        {
            var entity = repository.Get(e => e.EmployeeId == empId && e.IsActive == true);
            return entity;
        }

        public LeaveELOpening Create(LeaveELOpening objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(LeaveELOpening objectToUpdate)
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
            throw new NotImplementedException();
        }
        public bool IsContinued(long id)
        {
            throw new NotImplementedException();
        }

        public List<LeaveELOpening> AddELOpeningList(List<LeaveELOpening> objs)
        {
            repository.AddELOpeningList(objs);
            return objs;
        }

        public LeaveELOpening Get(Expression<Func<LeaveELOpening, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<LeaveELOpening> GetMany(Expression<Func<LeaveELOpening, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }

        #region Asyc
        public virtual async Task<IEnumerable<LeaveELOpening>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }


        public virtual async Task<IEnumerable<LeaveELOpening>> GetManyAsync(Expression<Func<LeaveELOpening, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }



        public virtual async Task<LeaveELOpening> GetAsync(Expression<Func<LeaveELOpening, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion
    }
}
