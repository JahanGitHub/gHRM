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
    public interface ILeaveAdjustmentAuthorityService : IServiceBase<LeaveAdjustmentAuthority>
    {
        bool IsExistLeaveAdjustmentAuthority(long employeeId);
    }
    public class LeaveAdjustmentAuthorityService : ILeaveAdjustmentAuthorityService
    {
        private readonly ILeaveAdjustmentAuthorityRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public LeaveAdjustmentAuthorityService(ILeaveAdjustmentAuthorityRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<LeaveAdjustmentAuthority> GetAll()
        {
            var entities = repository.GetAll().OrderBy(c => c.Id);
            return entities;
        }

        public LeaveAdjustmentAuthority GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public bool IsExistLeaveAdjustmentAuthority(long employeeId)
        {
            bool isExist = true;
            using (var db = new gHRMDBContext())
            {
                isExist = db.LeaveAdjustmentAuthority.Any(a => a.IsActive == true && a.EmployeeId == employeeId);
            }
            return isExist;
        }

        public LeaveAdjustmentAuthority Create(LeaveAdjustmentAuthority objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(LeaveAdjustmentAuthority objectToUpdate)
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

        public LeaveAdjustmentAuthority Get(Expression<Func<LeaveAdjustmentAuthority, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<LeaveAdjustmentAuthority> GetMany(Expression<Func<LeaveAdjustmentAuthority, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }

        #region Asyc
        public virtual async Task<IEnumerable<LeaveAdjustmentAuthority>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }


        public virtual async Task<IEnumerable<LeaveAdjustmentAuthority>> GetManyAsync(Expression<Func<LeaveAdjustmentAuthority, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }



        public virtual async Task<LeaveAdjustmentAuthority> GetAsync(Expression<Func<LeaveAdjustmentAuthority, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion

    }
}

