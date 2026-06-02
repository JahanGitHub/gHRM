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
    public interface IEmployeeFamilyInfoApprovalProcessService : IServiceBase<EmployeeFamilyInfoApprovalProcess>
    {

        // na
    }
    public class EmployeeFamilyInfoApprovalProcessService : IEmployeeFamilyInfoApprovalProcessService
    {
        private readonly IEmployeeFamilyInfoApprovalProcessRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public EmployeeFamilyInfoApprovalProcessService(IEmployeeFamilyInfoApprovalProcessRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        public IEnumerable<EmployeeFamilyInfoApprovalProcess> GetAll()
        {
            var entities = repository.GetAll().Where(c => c.IsActive == true).OrderBy(c => c.Id);
            return entities;
        }

        public EmployeeFamilyInfoApprovalProcess GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public EmployeeFamilyInfoApprovalProcess Create(EmployeeFamilyInfoApprovalProcess objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(EmployeeFamilyInfoApprovalProcess objectToUpdate)
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

        public EmployeeFamilyInfoApprovalProcess Get(Expression<Func<EmployeeFamilyInfoApprovalProcess, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<EmployeeFamilyInfoApprovalProcess> GetMany(Expression<Func<EmployeeFamilyInfoApprovalProcess, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }

        #region Asyc
        public virtual async Task<IEnumerable<EmployeeFamilyInfoApprovalProcess>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }


        public virtual async Task<IEnumerable<EmployeeFamilyInfoApprovalProcess>> GetManyAsync(Expression<Func<EmployeeFamilyInfoApprovalProcess, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }



        public virtual async Task<EmployeeFamilyInfoApprovalProcess> GetAsync(Expression<Func<EmployeeFamilyInfoApprovalProcess, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion
    }
}

