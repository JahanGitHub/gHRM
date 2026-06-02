using gHRM.Core.Utilities;
using gHRM.Core.Utilities.Constants;
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.Repository;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace gHRM.Service.TimeKeeping
{
    public interface IRoasterEmployeeScheduleService
    {
        IEnumerable<RoasterEmployeeSchedule> GetAll();
        RoasterEmployeeSchedule GetById(int id);
        GlobalResponse<RoasterEmployeeSchedule> Create(RoasterEmployeeSchedule objectToCreate);
        GlobalResponse<RoasterEmployeeSchedule> Update(RoasterEmployeeSchedule objectToUpdate);
        GlobalResponse<RoasterEmployeeSchedule> Delete(RoasterEmployeeSchedule roasterEmployeeSchedule);
        RoasterEmployeeSchedule GetRoasterEmployeeScheduleByEmployeeAndRosterId(int employeeId, int roasterId);
        IEnumerable<EmployeeRoasterScheduleModel> GetRoasterEmployeeSchedulesByEmployeeId(int employeeId);
        BaseResponse IsCurrentlyUsedInAttendance(int? employeeId, DateTime effectiveStartDate, DateTime effectiveEndDate, string timeKeepingType);
        RoasterEmployeeSchedule GetByTimeKeepingRoasterId(int timeKeepingRoasterId);
    }
    public class RoasterEmployeeScheduleService : IRoasterEmployeeScheduleService
    {
        private readonly IRoasterEmployeeScheduleRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public RoasterEmployeeScheduleService(IRoasterEmployeeScheduleRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        public IEnumerable<RoasterEmployeeSchedule> GetAll()
        {
            var listing = new List<RoasterEmployeeSchedule>();
            using (var db = new gHRMDBContext())
            {
                listing = db.RoasterEmployeeSchedules.Where(f => f.IsActive)
                    .AsParallel().ToList();
            }

            return listing;
        }

        public IEnumerable<EmployeeRoasterScheduleModel> GetRoasterEmployeeSchedulesByEmployeeId(int employeeId)
        {
            var listing = new List<EmployeeRoasterScheduleModel>();
            using (var db = new gHRMDBContext())
            {
                var sqlCommand = $"[att].[RoasterSchedule_GetRoasterScheduleByEmployee] {employeeId}";

                listing = db.Database.SqlQuery<EmployeeRoasterScheduleModel>(sqlCommand)
                                    .AsParallel().ToList();
            }

            return listing;
        }
        public RoasterEmployeeSchedule GetRoasterEmployeeScheduleByEmployeeAndRosterId(int employeeId, int roasterId)
        {
            var single = new RoasterEmployeeSchedule();
            using (var db = new gHRMDBContext())
            {
                single = db.RoasterEmployeeSchedules
                    .FirstOrDefault(f => f.IsActive
                                    && f.EmployeeId == employeeId
                                    && f.RoasterId == roasterId);
            }

            return single;
        }
        public RoasterEmployeeSchedule GetById(int id)
        {
            var single = new RoasterEmployeeSchedule();
            using (var db = new gHRMDBContext())
            {
                single = db.RoasterEmployeeSchedules
                    .FirstOrDefault(f => f.Id == id);
            }

            return single;
        }
        public RoasterEmployeeSchedule GetByTimeKeepingRoasterId(int timeKeepingRoasterId)
        {
            var single = new RoasterEmployeeSchedule();
            using (var db = new gHRMDBContext())
            {
                single = db.RoasterEmployeeSchedules
                    .FirstOrDefault(f => f.RoasterId == timeKeepingRoasterId && f.IsActive);
            }

            return single;
        }

        public GlobalResponse<RoasterEmployeeSchedule> Create(RoasterEmployeeSchedule objectToCreate)
        {
            var response = new GlobalResponse<RoasterEmployeeSchedule>();
            var currentDate = DateTime.Now;
            try
            {
                using (var db = new gHRMDBContext())
                {
                    objectToCreate.CreateDate = currentDate;
                    db.RoasterEmployeeSchedules.Add(objectToCreate);

                    db.SaveChanges();

                    response.IsSuccess = true;
                    response.Message = "Success, Added Employee Roaster Schedule";
                    response.Result = objectToCreate;
                }

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
                response.Result = objectToCreate;
            }

            return response;
        }
        public GlobalResponse<RoasterEmployeeSchedule> Update(RoasterEmployeeSchedule objectToUpdate)
        {
            var response = new GlobalResponse<RoasterEmployeeSchedule>();
            var isOperationSuccess = true;
            var currentDate = DateTime.Now;
            try
            {
                using (var db = new gHRMDBContext())
                {
                    var updateRoasterEmployeeSchedule = db.RoasterEmployeeSchedules
                        .FirstOrDefault(f => f.Id == objectToUpdate.Id);

                    if (updateRoasterEmployeeSchedule == null)
                    {
                        isOperationSuccess = false;
                        response.IsSuccess = false;
                        response.Message = "Warning, Employee Roaster Schedule not exist";
                        response.Result = null;
                    }

                    if (isOperationSuccess)
                    {
                        updateRoasterEmployeeSchedule.RoasterName = objectToUpdate.RoasterName;
                        updateRoasterEmployeeSchedule.LoginTime = objectToUpdate.LoginTime;
                        updateRoasterEmployeeSchedule.LastLoginTime = objectToUpdate.LastLoginTime;
                        updateRoasterEmployeeSchedule.LogoutTime = objectToUpdate.LogoutTime;
                        updateRoasterEmployeeSchedule.EffectiveStartDate = objectToUpdate.EffectiveStartDate;
                        updateRoasterEmployeeSchedule.EffectiveEndDate = objectToUpdate.EffectiveEndDate;

                        updateRoasterEmployeeSchedule.UpdateBy = objectToUpdate.UpdateBy;
                        updateRoasterEmployeeSchedule.UpdateDate = currentDate;

                        db.SaveChanges();

                        response.IsSuccess = true;
                        response.Message = "Success, Updated Employee Roaster Schedule";
                        response.Result = objectToUpdate;
                    }
                }

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
                response.Result = objectToUpdate;
            }

            return response;
        }
        public GlobalResponse<RoasterEmployeeSchedule> Delete(RoasterEmployeeSchedule roasterEmployeeSchedule)
        {
            var response = new GlobalResponse<RoasterEmployeeSchedule>();
            var isOperationSuccess = true;
            var currentDate = DateTime.Now;
            try
            {
                using (var db = new gHRMDBContext())
                {
                    var deleteRoasterEmployeeSchedule = db.RoasterEmployeeSchedules
                        .FirstOrDefault(f => f.Id == roasterEmployeeSchedule.Id);

                    if (deleteRoasterEmployeeSchedule == null)
                    {
                        isOperationSuccess = false;
                        response.IsSuccess = false;
                        response.Message = "Warning, Employee Roaster Schedule not exist";
                        response.Result = null;
                    }

                    if (isOperationSuccess)
                    {
                        //check used in attendance                       
                        var responseValidity=IsCurrentlyUsedInAttendance(roasterEmployeeSchedule.EmployeeId,
                                                    roasterEmployeeSchedule.EffectiveStartDate,
                                                    roasterEmployeeSchedule.EffectiveEndDate,
                                                    TimeKeepingTypeConstants.EmployeeRoaster);

                        if (!responseValidity.IsSuccess)
                        {
                            isOperationSuccess = false;
                            response.Message = responseValidity.Message;
                        }

                        if (isOperationSuccess)
                        {
                            deleteRoasterEmployeeSchedule.IsActive = false;
                            deleteRoasterEmployeeSchedule.UpdateDate = currentDate;
                            deleteRoasterEmployeeSchedule.UpdateBy = roasterEmployeeSchedule.UpdateBy;
                            db.SaveChanges();

                            response.IsSuccess = true;
                            response.Message = "Success, Deleted Employee Roaster Schedule";
                            response.Result = roasterEmployeeSchedule;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
                response.Result = roasterEmployeeSchedule;
            }

            return response;
        }


        public BaseResponse IsCurrentlyUsedInAttendance(int? employeeId, DateTime effectiveStartDate, DateTime effectiveEndDate, string timeKeepingType)
        {
            var response = new BaseResponse();
            try
            {
                using (var db = new gHRMDBContext())
                {

                    //check used in attendance
                    var sqlCommand = $@"[att].[AttAttendance_CheckAttendanceIsInUsed]
                                          { employeeId}
                                        ,'{ effectiveStartDate.ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture)}'
                                        ,'{ effectiveEndDate.ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture)}'
                                        ,'{timeKeepingType}'
                                        ";

                    response = db.Database.SqlQuery<BaseResponse>(sqlCommand).FirstOrDefault();

                }

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
