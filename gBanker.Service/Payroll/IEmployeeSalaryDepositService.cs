using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.CodeFirstMigration.Payroll;
using gHRM.Data.Repository.Payroll;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Service.Payroll
{
    public interface IEmployeeSalaryDepositService : IServiceBase<EmployeeSalaryDeposit>
    {
        List<EmployeeSalaryDeposit> GetEmployeeSalaryDepositsByDataRange(DateTime startDate, DateTime endDate);

        EmployeeSalaryDeposit GetDepositedSalaryDepositByEmployeeId(int empoyeeId);
    }
    public class EmployeeSalaryDepositService : IEmployeeSalaryDepositService
    {
        private readonly IEmployeeSalaryDepositRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public EmployeeSalaryDepositService(IEmployeeSalaryDepositRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        public IEnumerable<EmployeeSalaryDeposit> GetAll()
        {
            var entities = repository.GetAll().Where(c => c.IsActive == true).OrderBy(c => c.PRComponentId);
            return entities;
        }

        public EmployeeSalaryDeposit GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public EmployeeSalaryDeposit GetDepositedSalaryDepositByEmployeeId(int empoyeeId)
        {
            var single = new EmployeeSalaryDeposit();

            using (var db = new gHRMDBContext())
            {
                single = db.EmployeeSalaryDeposit.FirstOrDefault(f=>f.IsActive && f.DepositDone && !f.RefundDone && f.EmployeeId==empoyeeId);
            }

            return single;
        }

        public List<EmployeeSalaryDeposit> GetEmployeeSalaryDepositsByDataRange(DateTime startDate, DateTime endDate)
        {
            var listing = new List<EmployeeSalaryDeposit>();
            using (var db = new gHRMDBContext())
            {
                var sqlCommand = $@"
                        DECLARE 
	                        @StartDate DATE='{startDate.ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture)}',
	                        @EndDate DATE='{endDate.ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture)}';

                        SELECT 
                        *
                        FROM prl.EmployeeSalaryDeposit
                        WHERE 
		                        IsActive=1	 
	                        AND 
	                        (
		                           @StartDate BETWEEN [EffectiveStartDate]  AND [EffectiveEndDate] 
		                        OR @EndDate BETWEEN [EffectiveStartDate]  AND [EffectiveEndDate] 
	                        )
                        ";

                listing = db.Database.SqlQuery<EmployeeSalaryDeposit>(sqlCommand).AsParallel().ToList();
            }

            return listing;
        }

        public EmployeeSalaryDeposit Create(EmployeeSalaryDeposit objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(EmployeeSalaryDeposit objectToUpdate)
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

        public EmployeeSalaryDeposit Get(Expression<Func<EmployeeSalaryDeposit, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<EmployeeSalaryDeposit> GetMany(Expression<Func<EmployeeSalaryDeposit, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }

        #region Asyc
        public virtual async Task<IEnumerable<EmployeeSalaryDeposit>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }


        public virtual async Task<IEnumerable<EmployeeSalaryDeposit>> GetManyAsync(Expression<Func<EmployeeSalaryDeposit, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }



        public virtual async Task<EmployeeSalaryDeposit> GetAsync(Expression<Func<EmployeeSalaryDeposit, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion
    }
}
