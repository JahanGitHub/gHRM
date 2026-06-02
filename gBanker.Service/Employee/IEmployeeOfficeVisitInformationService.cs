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
    public interface IEmployeeOfficeVisitInformationService : IServiceBase<EmployeeOfficeVisitInformation>
    {


    }
    public class EmployeeOfficeVisitInformationService : IEmployeeOfficeVisitInformationService
    {
        private readonly IEmployeeOfficeVisitInformationRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public EmployeeOfficeVisitInformationService(IEmployeeOfficeVisitInformationRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        public IEnumerable<EmployeeOfficeVisitInformation> GetAll()
        {
            var entities = repository.GetAll().Where(c => c.IsActive == true).OrderBy(c => c.EmpOfficeVisitId);
            return entities;
        }

        public EmployeeOfficeVisitInformation GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public EmployeeOfficeVisitInformation Create(EmployeeOfficeVisitInformation objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(EmployeeOfficeVisitInformation objectToUpdate)
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

        public EmployeeOfficeVisitInformation Get(Expression<Func<EmployeeOfficeVisitInformation, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<EmployeeOfficeVisitInformation> GetMany(Expression<Func<EmployeeOfficeVisitInformation, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }

        #region Asyc
        public virtual async Task<IEnumerable<EmployeeOfficeVisitInformation>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }


        public virtual async Task<IEnumerable<EmployeeOfficeVisitInformation>> GetManyAsync(Expression<Func<EmployeeOfficeVisitInformation, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }



        public virtual async Task<EmployeeOfficeVisitInformation> GetAsync(Expression<Func<EmployeeOfficeVisitInformation, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion
    }
}
