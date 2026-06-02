using gHRM.Core.Common;
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.CodeFirstMigration.Payroll;
using gHRM.Data.Repository;
using gHRM.Data.Repository.payroll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Service.payroll
{
    public interface IPRSalaryRegisterService : IServiceBase<PRSalaryRegister>
    {
        List<PRSalaryRegister> AddEmployeeMonthlySalaryRegister(List<PRSalaryRegister> objs);

    }
    public class PRSalaryRegisterService : IPRSalaryRegisterService
    {
        private readonly IPRSalaryRegisterRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public PRSalaryRegisterService(IPRSalaryRegisterRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        public IEnumerable<PRSalaryRegister> GetAll()
        {
            var entities = repository.GetAll().Where(c => c.IsActive == true).OrderBy(c => c.PRSalaryRegisterID);
            return entities;
        }

        public PRSalaryRegister GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public PRSalaryRegister Create(PRSalaryRegister objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(PRSalaryRegister objectToUpdate)
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

        public List<PRSalaryRegister> AddEmployeeMonthlySalaryRegister(List<PRSalaryRegister> objs)
        {
            repository.AddEmployeeMonthlySalaryRegister(objs);
            return objs;
        }

        public PRSalaryRegister Get(Expression<Func<PRSalaryRegister, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<PRSalaryRegister> GetMany(Expression<Func<PRSalaryRegister, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }

        #region Asyc
        public virtual async Task<IEnumerable<PRSalaryRegister>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }


        public virtual async Task<IEnumerable<PRSalaryRegister>> GetManyAsync(Expression<Func<PRSalaryRegister, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }



        public virtual async Task<PRSalaryRegister> GetAsync(Expression<Func<PRSalaryRegister, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion
    }
}
