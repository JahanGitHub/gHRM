using gHRM.Core.Filters.Payroll;
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
    public interface IPRDepositService : IServiceBase<PRDeposit>
    {
        List<PRDeposit> GetPRDepositsByDataRange(DateTime startDate, DateTime endDate);
        PRDeposit GetSingleComponentByFilter(PRDepositSearchFilter filter);
        PRDeposit GetActivePRComponentById(int prComponentId);
    }
    public class PRDepositService : IPRDepositService
    {
        private readonly IPRDepositRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public PRDepositService(IPRDepositRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<PRDeposit> GetAll()
        {
            var entities = repository.GetAll().OrderBy(c => c.PRComponentId);
            return entities;
        }

        public List<PRDeposit> GetPRDepositsByDataRange(DateTime startDate, DateTime endDate)
        {
            var listing = new List<PRDeposit>();
            using (var db = new gHRMDBContext())
            {
                var sqlCommand = 
                            $@"
                            DECLARE 
	                            @StartDate DATE='{startDate.ToString("dd-MMM-yyyy",CultureInfo.InvariantCulture)}',
	                            @EndDate DATE='{endDate.ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture)}';

                            SELECT 
                            *
                            FROM prl.PRDeposit
                            WHERE 
		                            IsActive=1	 
	                            AND 
	                            (
		                               @StartDate BETWEEN [EffectiveStartDate]  AND [EffectiveEndDate] 
		                            OR @EndDate BETWEEN [EffectiveStartDate]  AND [EffectiveEndDate] 
	                            )
                            ";

                listing = db.Database.SqlQuery<PRDeposit>(sqlCommand).AsParallel().ToList();
            }

            return listing;
        }

        public PRDeposit GetSingleComponentByFilter(PRDepositSearchFilter filter)
        {
            var entity = new PRDeposit();

            using (var db = new gHRMDBContext())
            {
                entity = db.PRDeposit.FirstOrDefault(c => c.IsActive
                                                 && c.OfficeLocationId == filter.OfficeLocationId
                                                 && c.EmployeeType == filter.EmployeeTypeId
                                                 && c.EmployeeStatusId == filter.EmployeeStatusId
                                                 && c.ComponentName.Trim() == filter.ComponentName.Trim()
                                                 && c.ComponentCategory.Trim() == filter.ComponentCategory.Trim());
            }

            return entity;
        }

        public PRDeposit GetActivePRComponentById(int prComponentId)
        {
            var single = new PRDeposit();

            using (var db = new gHRMDBContext())
            {
                single = db.PRDeposit.FirstOrDefault(c => c.IsActive && c.PRComponentId == prComponentId);
            }

            return single;
        }

        public PRDeposit GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public PRDeposit Create(PRDeposit objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(PRDeposit objectToUpdate)
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
            throw new NotImplementedException(); ;
        }


        public bool IsContinued(long id)
        {
            throw new NotImplementedException();
        }

        public PRDeposit Get(Expression<Func<PRDeposit, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<PRDeposit> GetMany(Expression<Func<PRDeposit, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }

        #region Asyc
        public virtual async Task<IEnumerable<PRDeposit>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }


        public virtual async Task<IEnumerable<PRDeposit>> GetManyAsync(Expression<Func<PRDeposit, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }



        public virtual async Task<PRDeposit> GetAsync(Expression<Func<PRDeposit, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion

    }
}
