using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using gHRM.Core.Common;
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.Repository;


namespace gHRM.Service
{
    public interface ILeaveTypeLedgerService : IServiceBase<LeaveTypeLedger>
    {
        IEnumerable<ValidationResult> IsValidLeaveTypeLedger(string LeaveTypeName, int employeeStatusId, string leaveCategory);
        IEnumerable<ValidationResult> IsValidLeaveTypeLedgerEdit(string LeaveTypeLedgerName, int employeeStatusId, string leaveCategory, int leaveTypeId);
    }

    public class LeaveTypeLedgerService : ILeaveTypeLedgerService
    {
        private readonly ILeaveTypeLedgerRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public LeaveTypeLedgerService(ILeaveTypeLedgerRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        public IEnumerable<LeaveTypeLedger> GetAll()
        {
            var entities = repository.GetAll().Where(w => w.IsActive == true).OrderBy(c => c.Id);
            return entities;
        }

        public LeaveTypeLedger GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }
        public LeaveTypeLedger Create(LeaveTypeLedger objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(LeaveTypeLedger objectToUpdate)
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

        IEnumerable<ValidationResult> ILeaveTypeLedgerService.IsValidLeaveTypeLedger(string LeaveTypeLedgerName, int employeeStatusId, string leaveCategory)
        {
            var entity = repository.Get(p => 
                                        p.LeaveTypeName == LeaveTypeLedgerName && 
                                        p.EmployeeStatusId == employeeStatusId && 
                                        p.IsActive == true);

            if (entity != null)
            {
                yield return new ValidationResult("leaveTypeId", "Duplicate leave Type " + LeaveTypeLedgerName + ".");
            }

            var entityLeave = repository.Get(p => 
                                            p.LeaveCategory == leaveCategory &&
                                            p.EmployeeStatusId == employeeStatusId && 
                                            p.IsActive == true);
            if (entityLeave != null)
            {
                yield return new ValidationResult("leaveTypeId", "Duplicate leave Category " + entityLeave.LeaveTypeName + ".");
            }
        }

        IEnumerable<ValidationResult> ILeaveTypeLedgerService.IsValidLeaveTypeLedgerEdit(string LeaveTypeLedgerName, int employeeStatusId, string leaveCategory, int leaveTypeId)
        {
            var entity = repository.Get(p => p.LeaveTypeName == LeaveTypeLedgerName && p.EmployeeStatusId == employeeStatusId && p.LeaveTypeId != leaveTypeId && p.IsActive ==true);
            if (entity != null)
            {
                yield return new ValidationResult("leaveTypeId", "Duplicate leave Type.");
            }
            var entityLeave = repository.Get(p => p.LeaveCategory == leaveCategory && p.EmployeeStatusId == employeeStatusId && p.LeaveTypeId != leaveTypeId && p.IsActive == true);
            if (entityLeave != null)
            {
                yield return new ValidationResult("leaveTypeId", "Duplicate leave Type.");
            }
        }

        public LeaveTypeLedger Get(Expression<Func<LeaveTypeLedger, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<LeaveTypeLedger> GetMany(Expression<Func<LeaveTypeLedger, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }

        #region Asyc
        public virtual async Task<IEnumerable<LeaveTypeLedger>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }


        public virtual async Task<IEnumerable<LeaveTypeLedger>> GetManyAsync(Expression<Func<LeaveTypeLedger, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }



        public virtual async Task<LeaveTypeLedger> GetAsync(Expression<Func<LeaveTypeLedger, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion
    }
}
