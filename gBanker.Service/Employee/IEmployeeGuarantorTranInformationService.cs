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
    public interface IEmployeeGuarantorTranInformationService : IServiceBase<EmployeeGuarantorTranInformation>
    {
        EmployeeGuarantorTranInformation GetByGurId(int GuarantorTranId);
       
    }
    public class EmployeeGuarantorTranInformationService : IEmployeeGuarantorTranInformationService
    {
        private readonly IEmployeeGuarantorTranInformationRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public EmployeeGuarantorTranInformationService(IEmployeeGuarantorTranInformationRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        public IEnumerable<EmployeeGuarantorTranInformation> GetAll()
        {
            var entities = repository.GetAll().Where(c => c.IsRemoved is null || c.IsRemoved == false ).OrderBy(c => c.ID);
            return entities;
        }

        public EmployeeGuarantorTranInformation GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }
        public EmployeeGuarantorTranInformation GetByGurId(string GuarantorTranId)
        {
            var entity = repository.Get(e => e.ID == Convert.ToInt32(GuarantorTranId) );
            return entity;
        }

        public EmployeeGuarantorTranInformation Create(EmployeeGuarantorTranInformation objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(EmployeeGuarantorTranInformation objectToUpdate)
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

        public EmployeeGuarantorTranInformation Get(Expression<Func<EmployeeGuarantorTranInformation, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<EmployeeGuarantorTranInformation> GetMany(Expression<Func<EmployeeGuarantorTranInformation, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsRemoved != null);
            return entities;
        }

        #region Asyc
        public virtual async Task<IEnumerable<EmployeeGuarantorTranInformation>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }


        public virtual async Task<IEnumerable<EmployeeGuarantorTranInformation>> GetManyAsync(Expression<Func<EmployeeGuarantorTranInformation, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }



        public virtual async Task<EmployeeGuarantorTranInformation> GetAsync(Expression<Func<EmployeeGuarantorTranInformation, bool>> where)
        {
            return await repository.GetAsync(where);
        }

        public EmployeeGuarantorTranInformation GetByGurId(int GuarantorTranId)
        {
            var entity = repository.Get(e => e.ID == GuarantorTranId );
            return entity;
        }
        #endregion
    }
}
