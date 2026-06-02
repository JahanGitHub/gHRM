using gHRM.Core.Common;
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.Repository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Service
{
    public interface ITimeKeepingRosterService : IServiceBase<TimeKeepingRoster>
    {
        TimeKeepingRoster GetActiveRoasterById(int id);
        List<TimeKeepingRoster> GetTimeKeepingRosterByDate(DateTime fromDate);
        bool ValidateEmployeeRoasterByDateRange(int employeeId, int id, DateTime effectiveStartDate, DateTime effectiveEndDate);
    }
    public class TimeKeepingRosterService : ITimeKeepingRosterService
    {
        private readonly ITimeKeepingRosterRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public TimeKeepingRosterService(ITimeKeepingRosterRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        public IEnumerable<TimeKeepingRoster> GetAll()
        {
            var entities = repository.GetAll().Where(c => c.IsActive == true).OrderBy(c => c.TimeKeepingRosterId);
            return entities;
        }

        public TimeKeepingRoster GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public TimeKeepingRoster GetActiveRoasterById(int id)
        {
            var single = new TimeKeepingRoster();

            using (var db = new gHRMDBContext())
            {
                single = db.TimeKeepingRoster.FirstOrDefault(f => f.IsActive && f.TimeKeepingRosterId == id);
            }

            return single;
        }

        public List<TimeKeepingRoster> GetTimeKeepingRosterByDate(DateTime fromDate)
        {
            var listing = new List<TimeKeepingRoster>();

            using (var db = new gHRMDBContext())
            {
                listing = db.TimeKeepingRoster.Where(f => f.IsActive 
                                                    && ( DbFunctions.TruncateTime(fromDate) <= DbFunctions.TruncateTime(f.EffectiveEndDate))
                                                    ).ToList();
            }

            return listing;
        }
        public bool ValidateEmployeeRoasterByDateRange(int employeeId, int id,DateTime effectiveStartDate, DateTime effectiveEndDate)
        {
            bool isValid = true;

            using (var db = new gHRMDBContext())
            {                
                var sqlCommand = $@"[att].[TimeKeepingRoaster_GetTimekeepingroasterListingsByDateRange] 
                                      {employeeId}  
                                    , {id}
                                    ,'{effectiveStartDate.ToString("dd-MMM-yyyy",CultureInfo.InvariantCulture)}'
                                    ,'{effectiveEndDate.ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture)}'
                                ";
                int count = db.Database.SqlQuery<int>(sqlCommand).FirstOrDefault();
                if (count > 0)
                    isValid = false;
            }

            return isValid;
        }

        public TimeKeepingRoster Create(TimeKeepingRoster objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(TimeKeepingRoster objectToUpdate)
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

        public TimeKeepingRoster Get(Expression<Func<TimeKeepingRoster, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<TimeKeepingRoster> GetMany(Expression<Func<TimeKeepingRoster, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }

        #region Asyc
        public virtual async Task<IEnumerable<TimeKeepingRoster>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }


        public virtual async Task<IEnumerable<TimeKeepingRoster>> GetManyAsync(Expression<Func<TimeKeepingRoster, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }



        public virtual async Task<TimeKeepingRoster> GetAsync(Expression<Func<TimeKeepingRoster, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion
    }
}

