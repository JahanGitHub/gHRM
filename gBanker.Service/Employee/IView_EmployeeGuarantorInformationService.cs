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
    public interface IView_EmployeeGuarantorInformationService : IServiceBase<View_EmployeeGuarantorInformation>
    {

        //
    }
    public class View_EmployeeGuarantorInformationService : IView_EmployeeGuarantorInformationService
    {
        private readonly IView_EmployeeGuarantorInformationRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public View_EmployeeGuarantorInformationService(IView_EmployeeGuarantorInformationRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        public IEnumerable<View_EmployeeGuarantorInformation> GetAll()
        {
            var entities = repository.GetAll().Where(c => c.IsActive == true).OrderBy(c => c.RowSl);
            return entities;
        }

        public View_EmployeeGuarantorInformation GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public View_EmployeeGuarantorInformation Create(View_EmployeeGuarantorInformation objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(View_EmployeeGuarantorInformation objectToUpdate)
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

        public View_EmployeeGuarantorInformation Get(Expression<Func<View_EmployeeGuarantorInformation, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<View_EmployeeGuarantorInformation> GetMany(Expression<Func<View_EmployeeGuarantorInformation, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }

        #region Asyc
        public virtual async Task<IEnumerable<View_EmployeeGuarantorInformation>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }


        public virtual async Task<IEnumerable<View_EmployeeGuarantorInformation>> GetManyAsync(Expression<Func<View_EmployeeGuarantorInformation, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }



        public virtual async Task<View_EmployeeGuarantorInformation> GetAsync(Expression<Func<View_EmployeeGuarantorInformation, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion
    }
}
