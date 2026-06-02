using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.CodeFirstMigration.Payroll;
using gHRM.Data.CodeFirstMigration.ViewsEmployee;
using gHRM.Data.Repository;
using gHRM.Data.Repository.Payroll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Service.Payroll
{
    public interface IEmployeeShortInfoService : IServiceBase<EmployeeShortInfo>
    {

        // na
    }
    public class EmployeeShortInfoServiceService : IEmployeeShortInfoService
    {
        private readonly IEmployeeShortInfoRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public EmployeeShortInfoServiceService(IEmployeeShortInfoRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        public IEnumerable<EmployeeShortInfo> GetAll()
        {
            var entities = repository.GetAll().OrderBy(c => c.EmployeeId);
            return entities;
        }

        public EmployeeShortInfo GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public EmployeeShortInfo Create(EmployeeShortInfo objectToCreate)
        {
            //repository.Add(objectToCreate);
            //Save();
            return objectToCreate;
        }

        public void Update(EmployeeShortInfo objectToUpdate)
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

        public EmployeeShortInfo Get(Expression<Func<EmployeeShortInfo, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<EmployeeShortInfo> GetMany(Expression<Func<EmployeeShortInfo, bool>> where)
        {
            var entities = repository.GetMany(where);
            return entities;
        }

        #region Asyc
        public virtual async Task<IEnumerable<EmployeeShortInfo>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }


        public virtual async Task<IEnumerable<EmployeeShortInfo>> GetManyAsync(Expression<Func<EmployeeShortInfo, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }



        public virtual async Task<EmployeeShortInfo> GetAsync(Expression<Func<EmployeeShortInfo, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion
    }
}
