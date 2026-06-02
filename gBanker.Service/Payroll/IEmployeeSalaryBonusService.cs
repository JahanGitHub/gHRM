using BasicDataAccess;
using gHRM.Core.Common;
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.CodeFirstMigration.Payroll;
using gHRM.Data.Repository;
using gHRM.Data.Repository.Payroll;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Service.Payroll
{
    public interface IEmployeeSalaryBonusService : IServiceBase<EmployeeSalaryBonus>
    {
        List<EmployeeSalaryBonus> AddEmployeeMonthlySalaryBonusList(List<EmployeeSalaryBonus> objs);
        DataSet GetDataWithParameter<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class;
    }
    public class EmployeeSalaryBonusService : IEmployeeSalaryBonusService
    {
        private readonly IEmployeeSalaryBonusRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public EmployeeSalaryBonusService(IEmployeeSalaryBonusRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        public DataSet GetDataWithParameter<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class
        {
            using (var gbData = new gHRMDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public IEnumerable<EmployeeSalaryBonus> GetAll()
        {
            var entities = repository.GetAll().Where(c => c.IsActive == 1).OrderBy(c => c.ESBonusId);
            return entities;
        }

        public EmployeeSalaryBonus GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public EmployeeSalaryBonus Create(EmployeeSalaryBonus objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(EmployeeSalaryBonus objectToUpdate)
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

        public List<EmployeeSalaryBonus> AddEmployeeMonthlySalaryBonusList(List<EmployeeSalaryBonus> objs)
        {
            repository.AddEmployeeMonthlySalaryBonusList(objs);
            return objs;
        }

        public EmployeeSalaryBonus Get(Expression<Func<EmployeeSalaryBonus, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<EmployeeSalaryBonus> GetMany(Expression<Func<EmployeeSalaryBonus, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == 1);
            return entities;
        }

        #region Asyc
        public virtual async Task<IEnumerable<EmployeeSalaryBonus>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }


        public virtual async Task<IEnumerable<EmployeeSalaryBonus>> GetManyAsync(Expression<Func<EmployeeSalaryBonus, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }



        public virtual async Task<EmployeeSalaryBonus> GetAsync(Expression<Func<EmployeeSalaryBonus, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion
    }
}
