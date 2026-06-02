using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.CodeFirstMigration.Payroll;
using gHRM.Data.DBDetailModels.Payroll;
using gHRM.Data.Repository.Payroll;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Service.Payroll
{
    public interface IPRSalaryConfigurationService : IServiceBase<PRSalaryConfiguration>
    {
        List<PRSalaryConfiguration> GetByEmployeeId(long id);
        List<PRSalaryConfigurationModel> GetPREmployeeSalaryCurrentConfigurationAllowanceAndDeduction(
            long employeeId, DateTime effecitveStartDate, DateTime effecitveEndDate);

        bool ExisstPRSalaryConfigurationByEmployeeId(long employeeId);
    }
    public class PRSalaryConfigurationService : IPRSalaryConfigurationService
    {
        private readonly IPRSalaryConfigurationRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;


        public PRSalaryConfigurationService(IPRSalaryConfigurationRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<PRSalaryConfiguration> GetAll()
        {
            var entities = repository.GetAll().Where(c => c.IsActive == true).OrderBy(c => c.PRSalaryConfigurationID);
            return entities;
        }

        public List<PRSalaryConfiguration> GetByEmployeeId(long id)
        {
            var entity = repository.GetAll().Where(p => p.EmployeeID == id & p.IsActive == true).ToList();
            return entity;
        }

        public bool ExisstPRSalaryConfigurationByEmployeeId(long employeeId)
        {
            bool isExist = true;
            using (var db = new gHRMDBContext())
            {
                isExist = db.PRSalaryConfigurations.Any(p =>
                                                       p.EmployeeID == employeeId 
                                                    && p.IsActive == true);
            }
                           
            return isExist;
        }

        public PRSalaryConfiguration GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public List<PRSalaryConfigurationModel> 
            GetPREmployeeSalaryCurrentConfigurationAllowanceAndDeduction(long employeeId,DateTime effecitveStartDate, DateTime effecitveEndDate)
        {
            var listing = new List<PRSalaryConfigurationModel>();

            using (var db = new gHRMDBContext())
            {
                var sqlCommand = $@"prl.SP_GET_PREmployeeSalaryCurrentConfigurationAllowanceAndDeduction 
                                {employeeId},'{effecitveStartDate.ToString("dd-MMM-yyyy",CultureInfo.InvariantCulture)}','{effecitveEndDate.ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture)}'";

                listing = db.Database.SqlQuery<PRSalaryConfigurationModel>(sqlCommand)
                                    .AsParallel().ToList();
            }

            return listing;
        }

        public PRSalaryConfiguration Create(PRSalaryConfiguration objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(PRSalaryConfiguration objectToUpdate)
        {
            repository.Update(objectToUpdate);
            Save();
        }

        public void Delete(int id)
        {
            var entity = repository.GetById(id);
            repository.Delete(entity);
            Save();
        }

        public void Save()
        {
            unitOfWork.Commit();
        }


        public bool Inactivate(long id, DateTime? inactiveDate)
        {
            var obj = repository.GetById(id);
            if (obj != null)
            {
                obj.IsActive = false;
                repository.Update(obj);
                Save();
                return true;
            }
            return false;
        }


        public bool IsContinued(long id)
        {
            throw new NotImplementedException();
        }

        public PRSalaryConfiguration Get(Expression<Func<PRSalaryConfiguration, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<PRSalaryConfiguration> GetMany(Expression<Func<PRSalaryConfiguration, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }

        #region Asyc
        public virtual async Task<IEnumerable<PRSalaryConfiguration>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }


        public virtual async Task<IEnumerable<PRSalaryConfiguration>> GetManyAsync(Expression<Func<PRSalaryConfiguration, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }



        public virtual async Task<PRSalaryConfiguration> GetAsync(Expression<Func<PRSalaryConfiguration, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion
    }
}
