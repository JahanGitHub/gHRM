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
    public interface IEmployeeGuarantorInformationService : IServiceBase<EmployeeGuarantorInformation>
    {

        EmployeeGuarantorInformation GetByGurId(int GuarantorId);
    }
    public class EmployeeGuarantorInformationService : IEmployeeGuarantorInformationService
    {
        private readonly IEmployeeGuarantorInformationRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public EmployeeGuarantorInformationService(IEmployeeGuarantorInformationRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        public IEnumerable<EmployeeGuarantorInformation> GetAll()
        {
            var entities = repository.GetAll().Where(c => c.IsActive == true).OrderBy(c => c.GuarantorId);
            return entities;
        }

        public EmployeeGuarantorInformation GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }
        public EmployeeGuarantorInformation GetByGurId(int GuarantorId)
        {
            var entity = repository.Get(e => e.GuarantorId == GuarantorId && e.IsActive == true);
            return entity;
        }

        public EmployeeGuarantorInformation Create(EmployeeGuarantorInformation objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(EmployeeGuarantorInformation objectToUpdate)
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

        public EmployeeGuarantorInformation Get(Expression<Func<EmployeeGuarantorInformation, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<EmployeeGuarantorInformation> GetMany(Expression<Func<EmployeeGuarantorInformation, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }

        #region Asyc
        public virtual async Task<IEnumerable<EmployeeGuarantorInformation>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }


        public virtual async Task<IEnumerable<EmployeeGuarantorInformation>> GetManyAsync(Expression<Func<EmployeeGuarantorInformation, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }



        public virtual async Task<EmployeeGuarantorInformation> GetAsync(Expression<Func<EmployeeGuarantorInformation, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion
    }
}
