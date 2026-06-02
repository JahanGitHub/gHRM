using gHRM.Core.Utilities;
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.Repository.TimeKeeping;
using System;
using System.Collections.Generic;
using System.Linq;

namespace gHRM.Service.TimeKeeping
{
    public interface ITimekeepingAttendanceDeviceService
    {
        IEnumerable<TimekeepingAttendanceDevice> GetAll();
        TimekeepingAttendanceDevice GetById(int id);
        GlobalResponse<TimekeepingAttendanceDevice> Create(TimekeepingAttendanceDevice objectToCreate);
        GlobalResponse<TimekeepingAttendanceDevice> Update(TimekeepingAttendanceDevice objectToUpdate);
        GlobalResponse<TimekeepingAttendanceDevice> Delete(TimekeepingAttendanceDevice staffWelfareFundSetting);
    }
    public class TimekeepingAttendanceDeviceService : ITimekeepingAttendanceDeviceService
    {
        private readonly ITimekeepingAttendanceDeviceRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public TimekeepingAttendanceDeviceService(ITimekeepingAttendanceDeviceRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        public IEnumerable<TimekeepingAttendanceDevice> GetAll()
        {
            var listing = new List<TimekeepingAttendanceDevice>();
            using (var db = new gHRMDBContext())
            {
                listing = db.TimekeepingAttendanceDevices.Where(f => f.IsActive)
                    .AsParallel().ToList();
            }

            return listing;
        }
        public TimekeepingAttendanceDevice GetById(int id)
        {
            var single = new TimekeepingAttendanceDevice();
            using (var db = new gHRMDBContext())
            {
                single = db.TimekeepingAttendanceDevices
                    .FirstOrDefault(f => f.Id == id);
            }

            return single;
        }
        public GlobalResponse<TimekeepingAttendanceDevice> Create(TimekeepingAttendanceDevice objectToCreate)
        {
            var response = new GlobalResponse<TimekeepingAttendanceDevice>();
            var currentDate = DateTime.Now;
            try
            {
                using (var db = new gHRMDBContext())
                {
                    objectToCreate.CreatedDate = currentDate;
                    db.TimekeepingAttendanceDevices.Add(objectToCreate);

                    db.SaveChanges();

                    response.IsSuccess = true;
                    response.Message = "Success, Added Timekeeping Attendance Device";
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
        public GlobalResponse<TimekeepingAttendanceDevice> Update(TimekeepingAttendanceDevice objectToUpdate)
        {
            var response = new GlobalResponse<TimekeepingAttendanceDevice>();
            var isOperationSuccess = true;
            var currentDate = DateTime.Now;
            try
            {
                using (var db = new gHRMDBContext())
                {
                    var updateTimekeepingAttendanceDevice = db.TimekeepingAttendanceDevices
                        .FirstOrDefault(f => f.Id == objectToUpdate.Id);

                    if (updateTimekeepingAttendanceDevice == null)
                    {
                        isOperationSuccess = false;
                        response.IsSuccess = false;
                        response.Message = "Warning, Timekeeping Attendance Device not exist";
                        response.Result = null;
                    }

                    if (isOperationSuccess)
                    {
                        updateTimekeepingAttendanceDevice.DeviceCode = objectToUpdate.DeviceCode;
                        updateTimekeepingAttendanceDevice.DeviceName = objectToUpdate.DeviceName;
                        updateTimekeepingAttendanceDevice.IsActive = objectToUpdate.IsActive;

                        db.SaveChanges();


                        response.IsSuccess = true;
                        response.Message = "Success, Updated Timekeeping Attendance Device";
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
        public GlobalResponse<TimekeepingAttendanceDevice> Delete(TimekeepingAttendanceDevice objectToUpdate)
        {
            var response = new GlobalResponse<TimekeepingAttendanceDevice>();
            var isOperationSuccess = true;
            var currentDate = DateTime.Now;
            try
            {
                using (var db = new gHRMDBContext())
                {
                    var deleteTimekeepingAttendanceDevice = db.TimekeepingAttendanceDevices
                        .FirstOrDefault(f => f.Id == objectToUpdate.Id);

                    if (deleteTimekeepingAttendanceDevice == null)
                    {
                        isOperationSuccess = false;
                        response.IsSuccess = false;
                        response.Message = "Warning, Timekeeping Attendance Device not exist";
                        response.Result = null;
                    }

                    if (isOperationSuccess)
                    {
                        deleteTimekeepingAttendanceDevice.IsActive = false;

                        db.SaveChanges();

                        response.IsSuccess = true;
                        response.Message = "Success, Deleted Timekeeping Attendance Device";
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
    }
}
