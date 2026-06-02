using gHRM.Core.Utilities;
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.CodeFirstMigration.WelfareFund;
using gHRM.Data.Repository.WelfareFund;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace gHRM.Service.WelfareFund
{
    public interface IStaffWelfareFundSettingService
    {
        IEnumerable<StaffWelfareFundSetting> GetAll();
        StaffWelfareFundSetting GetById(int id);
        GlobalResponse<StaffWelfareFundSetting> Create(StaffWelfareFundSetting objectToCreate);
        //GlobalResponse<StaffWelfareFundSetting> Update(StaffWelfareFundSetting objectToUpdate);
        GlobalResponse<StaffWelfareFundSetting> Delete(StaffWelfareFundSetting staffWelfareFundSetting);
    }
    public class StaffWelfareFundSettingService : IStaffWelfareFundSettingService
    {
        private readonly IStaffWelfareFundSettingRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public StaffWelfareFundSettingService(IStaffWelfareFundSettingRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        
        public IEnumerable<StaffWelfareFundSetting> GetAll()
        {
            var listing = new List<StaffWelfareFundSetting>();
            using (var db = new gHRMDBContext())
            {
                listing = db.StaffWelfareFundSettings.Where(f => f.IsActive)
                    .AsParallel().ToList();
            }

            return listing;
        }
        public StaffWelfareFundSetting GetById(int id)
        {
            var single = new StaffWelfareFundSetting();
            using (var db = new gHRMDBContext())
            {
                single = db.StaffWelfareFundSettings
                    .FirstOrDefault(f=>f.StaffWelfareFundSettingId==id);
            }

            return single;
        }
        public GlobalResponse<StaffWelfareFundSetting> Create(StaffWelfareFundSetting objectToCreate)
        {
            var response = new GlobalResponse<StaffWelfareFundSetting>();
            var currentDate = DateTime.Now;
            try
            {
                using (var db = new gHRMDBContext())
                {
                    objectToCreate.CreateDate = currentDate;
                    db.StaffWelfareFundSettings.Add(objectToCreate);                    

                    db.SaveChanges();

                    response.IsSuccess = true;
                    response.Message = "Success, Added Fund Setting";
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
        //public GlobalResponse<StaffWelfareFundSetting> Update(StaffWelfareFundSetting objectToUpdate)
        //{
        //    var response = new GlobalResponse<StaffWelfareFundSetting>();
        //    var isOperationSuccess = true;
        //    var currentDate = DateTime.Now;
        //    try
        //    {
        //        using (var db = new gHRMDBContext())
        //        {
        //            var updateStaffWelfareFundSetting = db.StaffWelfareFundSettings
        //                .FirstOrDefault(f=>f.StaffWelfareFundSettingId==objectToUpdate.StaffWelfareFundSettingId);

        //            if (updateStaffWelfareFundSetting == null)
        //            {
        //                isOperationSuccess = false;
        //                response.IsSuccess = false;
        //                response.Message = "Warning, Staff Welfare Fund Setting not exist";
        //                response.Result = null;
        //            }

        //            if (isOperationSuccess)
        //            {
        //                updateStaffWelfareFundSetting.DeductionAmount = objectToUpdate.DeductionAmount;                        
        //                updateStaffWelfareFundSetting.IsPercentage = objectToUpdate.IsPercentage;
        //                updateStaffWelfareFundSetting.IsActive = objectToUpdate.IsActive;
        //                updateStaffWelfareFundSetting.UpdateUser = objectToUpdate.UpdateUser;
        //                updateStaffWelfareFundSetting.UpdateDate = currentDate;

        //                db.SaveChanges();

        //                response.IsSuccess = true;
        //                response.Message = "Success, Updated Staff Welfare Fund Setting";
        //                response.Result = objectToUpdate;
        //            }
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        response.IsSuccess = false;
        //        response.Message = ex.Message;
        //        response.Result = objectToUpdate;
        //    }

        //    return response;
        //}
        public GlobalResponse<StaffWelfareFundSetting> Delete(StaffWelfareFundSetting staffWelfareFundSetting)
        {
            var response = new GlobalResponse<StaffWelfareFundSetting>();
            var isOperationSuccess = true;
            var currentDate = DateTime.Now;
            try
            {
                using (var db = new gHRMDBContext())
                {
                    var deleteStaffWelfareFundSetting = db.StaffWelfareFundSettings
                        .FirstOrDefault(f => f.StaffWelfareFundSettingId == staffWelfareFundSetting.StaffWelfareFundSettingId);

                    if (deleteStaffWelfareFundSetting == null)
                    {
                        isOperationSuccess = false;
                        response.IsSuccess = false;
                        response.Message = "Warning,  Fund Setting not exist";
                        response.Result = null;
                    }

                    if (isOperationSuccess)
                    {
                        deleteStaffWelfareFundSetting.IsActive = false;
                        
                        db.SaveChanges();

                        response.IsSuccess = true;
                        response.Message = "Success, Inactive  Fund Setting";
                        response.Result = staffWelfareFundSetting;
                    }
                }

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
                response.Result = staffWelfareFundSetting;
            }

            return response;
        }
    }
}
