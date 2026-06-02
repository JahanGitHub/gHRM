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
    public interface ILeaveTypeService : IServiceBase<LeaveType>
    {
        IEnumerable<ValidationResult> IsValidLeaveType(string LeaveTypeName, int employeeStatusId, string leaveCategory);
        IEnumerable<ValidationResult> IsValidLeaveTypeEdit(string LeaveTypeName, int employeeStatusId, string leaveCategory, int leaveTypeId);
    }

    public class LeaveTypeService : ILeaveTypeService
    {
        private readonly ILeaveTypeRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public LeaveTypeService(ILeaveTypeRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        public IEnumerable<LeaveType> GetAll()
        {
            var entities = repository.GetAll().Where(w => w.IsActive == true).OrderBy(c => c.LeaveTypeRank);
            return entities;
        }

        public LeaveType GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }
        public LeaveType Create(LeaveType objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(LeaveType objectToUpdate)
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

        IEnumerable<ValidationResult> ILeaveTypeService.IsValidLeaveType(string LeaveTypeName, int employeeStatusId, string leaveCategory)
        {
            var entity = repository.Get(p => 
                                        p.LeaveTypeName == LeaveTypeName && 
                                        p.EmployeeStatusId == employeeStatusId && 
                                        p.IsActive == true);

            if (entity != null)
            {
                yield return new ValidationResult("leaveTypeId", "Duplicate leave Type " + LeaveTypeName + ".");
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

        IEnumerable<ValidationResult> ILeaveTypeService.IsValidLeaveTypeEdit(string LeaveTypeName, int employeeStatusId, string leaveCategory, int leaveTypeId)
        {
            var entity = repository.Get(p => p.LeaveTypeName == LeaveTypeName && p.EmployeeStatusId == employeeStatusId && p.LeaveTypeId != leaveTypeId && p.IsActive ==true);
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

        public LeaveType Get(Expression<Func<LeaveType, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<LeaveType> GetMany(Expression<Func<LeaveType, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }

        #region Asyc
        public virtual async Task<IEnumerable<LeaveType>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }


        public virtual async Task<IEnumerable<LeaveType>> GetManyAsync(Expression<Func<LeaveType, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }



        public virtual async Task<LeaveType> GetAsync(Expression<Func<LeaveType, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion
    }
}
