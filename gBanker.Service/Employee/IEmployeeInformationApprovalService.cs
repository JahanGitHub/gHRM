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
    public interface IEmployeeInformationApprovalService : IServiceBase<EmployeeInformationApproval>
    {


    }
    public class EmployeeInformationApprovalService : IEmployeeInformationApprovalService
    {
        private readonly IEmployeeInformationApprovalRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public EmployeeInformationApprovalService(IEmployeeInformationApprovalRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        public IEnumerable<EmployeeInformationApproval> GetAll()
        {
            var entities = repository.GetAll().Where(c => c.IsActive == true).OrderBy(c => c.Id);
            return entities;
        }

        public EmployeeInformationApproval GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public EmployeeInformationApproval Create(EmployeeInformationApproval objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(EmployeeInformationApproval objectToUpdate)
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

        public EmployeeInformationApproval Get(Expression<Func<EmployeeInformationApproval, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<EmployeeInformationApproval> GetMany(Expression<Func<EmployeeInformationApproval, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }

        #region Asyc
        public virtual async Task<IEnumerable<EmployeeInformationApproval>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }


        public virtual async Task<IEnumerable<EmployeeInformationApproval>> GetManyAsync(Expression<Func<EmployeeInformationApproval, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }



        public virtual async Task<EmployeeInformationApproval> GetAsync(Expression<Func<EmployeeInformationApproval, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion
    }
}
