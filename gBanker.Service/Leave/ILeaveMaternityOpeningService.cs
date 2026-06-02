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
    public interface ILeaveMaternityOpeningService : IServiceBase<LeaveMaternityOpening>
    {
        LeaveMaternityOpening GetMatrnityByEmpId(Int64 EmpId);
    }
    public class LeaveMaternityOpeningService : ILeaveMaternityOpeningService
    {
        private readonly ILeaveMaternityOpeningRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;
        public LeaveMaternityOpeningService(ILeaveMaternityOpeningRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<LeaveMaternityOpening> GetAll()
        {
            var entities = repository.GetAll();
            return entities;
        }
        public LeaveMaternityOpening GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }
        public LeaveMaternityOpening GetMatrnityByEmpId(Int64 EmpId)
        {
            var entity = repository.Get(e => e.EmployeeId == EmpId);
            return entity;
        }

        public void Save()
        {
            unitOfWork.Commit();
        }
        public LeaveMaternityOpening Create(LeaveMaternityOpening objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(LeaveMaternityOpening objectToUpdate)
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

        public LeaveMaternityOpening Get(Expression<Func<LeaveMaternityOpening, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<LeaveMaternityOpening> GetMany(Expression<Func<LeaveMaternityOpening, bool>> where)
        {
            var entities = repository.GetMany(where);
            return entities;
        }

        #region Asyc
        public virtual async Task<IEnumerable<LeaveMaternityOpening>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }


        public virtual async Task<IEnumerable<LeaveMaternityOpening>> GetManyAsync(Expression<Func<LeaveMaternityOpening, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }



        public virtual async Task<LeaveMaternityOpening> GetAsync(Expression<Func<LeaveMaternityOpening, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion

    }
}
