using gHRM.Core.Common;
using gHRM.Core.Filters.TimeKeepings;
using gHRM.Core.Utilities;
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.Repository;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Service
{
    public interface IView_EmployeeTimeKeepingExceptionService : IServiceBase<View_EmployeeTimeKeepingException>
    {
        List<View_EmployeeTimeKeepingException> GetEmployeeTimeKeepingExceptions();
        BaseResponse UpdateAttendanceForTimekeepingException(TimeKeepingExceptionSearchFilter filter, bool LEAVE_AUTO_ADJUSTMENT_DISABLED);     
    }
    public class View_EmployeeTimeKeepingExceptionService : IView_EmployeeTimeKeepingExceptionService
    {
        private readonly IView_EmployeeTimeKeepingExceptionRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public View_EmployeeTimeKeepingExceptionService(IView_EmployeeTimeKeepingExceptionRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        public IEnumerable<View_EmployeeTimeKeepingException> GetAll()
        {
            var entities = repository.GetAll().Where(c => c.IsActive == true).OrderBy(c => c.RowSl);
            return entities;
        }

        public View_EmployeeTimeKeepingException GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public View_EmployeeTimeKeepingException Create(View_EmployeeTimeKeepingException objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(View_EmployeeTimeKeepingException objectToUpdate)
        {
            repository.Update(objectToUpdate);
            Save();
        }

        public List<View_EmployeeTimeKeepingException> GetEmployeeTimeKeepingExceptions()
        {
            var listing = new List<View_EmployeeTimeKeepingException>();

            using (var db = new gHRMDBContext())
            {
                var sqlCommand = $@"[att].[EmployeeTimeKeepingException_GetTimekeepingException]";
                listing = db.Database.SqlQuery<View_EmployeeTimeKeepingException>(sqlCommand)
                                    .AsParallel().ToList();
            }

            return listing;
        }

        public BaseResponse UpdateAttendanceForTimekeepingException(TimeKeepingExceptionSearchFilter filter, bool LEAVE_AUTO_ADJUSTMENT_DISABLED)
        {
            var response = new BaseResponse();

            try
            {
                using (var db = new gHRMDBContext())
                {
                    int Data_LEAVE_AUTO_ADJUSTMENT_DISABLED = LEAVE_AUTO_ADJUSTMENT_DISABLED ? 1 : 0;
                    var sqlCommand = $@"[att].[SP_TimekeepingException_Update_Attendance]
                                {filter.EmployeeId},
                                {filter.AttendenceTypeId},
                                '{Convert.ToDateTime(filter.AttenDanceDate).ToString("dd-MMM-yyyy hh:mm:ss tt", CultureInfo.InvariantCulture)}',
                                '{Convert.ToDateTime(filter.LoginTime).ToString("dd-MMM-yyyy hh:mm:ss tt", CultureInfo.InvariantCulture)}',
                                '{Convert.ToDateTime(filter.LogoutTime).ToString("dd-MMM-yyyy hh:mm:ss tt", CultureInfo.InvariantCulture)}',
                                '{Convert.ToDateTime(filter.LastLoginTime).ToString("dd-MMM-yyyy hh:mm:ss tt", CultureInfo.InvariantCulture)}',
                                '{filter.Justification}',
                                {filter.CreateUser},
                                {Data_LEAVE_AUTO_ADJUSTMENT_DISABLED}
                                ";

                    response = db.Database.SqlQuery<BaseResponse>(sqlCommand).FirstOrDefault();                       
                }
            }
            catch (Exception ex)
            {
                response = new BaseResponse
                {
                    IsSuccess=false,
                    Message="Error, There was an error while adding time keeping exception!"
                };
            }
            return response;
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

        public View_EmployeeTimeKeepingException Get(Expression<Func<View_EmployeeTimeKeepingException, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<View_EmployeeTimeKeepingException> GetMany(Expression<Func<View_EmployeeTimeKeepingException, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }

        #region Asyc
        public virtual async Task<IEnumerable<View_EmployeeTimeKeepingException>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }


        public virtual async Task<IEnumerable<View_EmployeeTimeKeepingException>> GetManyAsync(Expression<Func<View_EmployeeTimeKeepingException, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }



        public virtual async Task<View_EmployeeTimeKeepingException> GetAsync(Expression<Func<View_EmployeeTimeKeepingException, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion
    }
}
        