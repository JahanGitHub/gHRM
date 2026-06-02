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
    public interface IEmployeeReportOptionJCFService : IServiceBase<EmployeeReportOptionJCF>
    {


    }
    public class EmployeeReportOptionJCFService : IEmployeeReportOptionJCFService
{
        private readonly IEmployeeReportOptionJCFRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public EmployeeReportOptionJCFService(IEmployeeReportOptionJCFRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        public IEnumerable<EmployeeReportOptionJCF> GetAll()
        {
            var entities = repository.GetAll().Where(c => c.IsActive == true).OrderBy(c => c.Id);
            return entities;
        }

        public EmployeeReportOptionJCF GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public EmployeeReportOptionJCF Create(EmployeeReportOptionJCF objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(EmployeeReportOptionJCF objectToUpdate)
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

        public EmployeeReportOptionJCF Get(Expression<Func<EmployeeReportOptionJCF, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<EmployeeReportOptionJCF> GetMany(Expression<Func<EmployeeReportOptionJCF, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }

        #region Asyc
        public virtual async Task<IEnumerable<EmployeeReportOptionJCF>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }


        public virtual async Task<IEnumerable<EmployeeReportOptionJCF>> GetManyAsync(Expression<Func<EmployeeReportOptionJCF, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }



        public virtual async Task<EmployeeReportOptionJCF> GetAsync(Expression<Func<EmployeeReportOptionJCF, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion
    }
}

