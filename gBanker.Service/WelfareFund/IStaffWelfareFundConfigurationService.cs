using gHRM.Core.Utilities;
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.CodeFirstMigration.WelfareFund;
using gHRM.Data.Repository.WelfareFund;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace gHRM.Service.WelfareFund
{
    public interface IStaffWelfareFundConfigurationService
    {
        IEnumerable<StaffWelfareFundConfiguration> GetAll();
        StaffWelfareFundConfiguration GetById(int id);
        GlobalResponse<StaffWelfareFundConfiguration> Create(StaffWelfareFundConfiguration objectToCreate);
      //  GlobalResponse<StaffWelfareFundConfiguration> Update(StaffWelfareFundConfiguration objectToUpdate);
      //  GlobalResponse<StaffWelfareFundConfiguration> Delete(StaffWelfareFundConfiguration staffWelfareFundSetting);  
        BaseResponse ConfigureStaffWelfareFund(string employeeCode, int purposeId, decimal fundAmount, string remarks, long createUser);
    }
    public class StaffWelfareFundConfigurationService : IStaffWelfareFundConfigurationService
    {
        private readonly IStaffWelfareFundConfigurationRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public StaffWelfareFundConfigurationService(IStaffWelfareFundConfigurationRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        
        public IEnumerable<StaffWelfareFundConfiguration> GetAll()
        {
            var listing = new List<StaffWelfareFundConfiguration>();
            using (var db = new gHRMDBContext())
            {
                listing = db.StaffWelfareFundConfigurations.Where(f => f.IsActive)
                    .AsParallel().ToList();
            }

            return listing;
        }
        public StaffWelfareFundConfiguration GetById(int id)
        {
            var single = new StaffWelfareFundConfiguration();
            using (var db = new gHRMDBContext())
            {
                single = db.StaffWelfareFundConfigurations
                    .FirstOrDefault(f=>f.StaffWelfareFundConfigurationId==id);
            }

            return single;
        }
        public GlobalResponse<StaffWelfareFundConfiguration> Create(StaffWelfareFundConfiguration objectToCreate)
        {
            var response = new GlobalResponse<StaffWelfareFundConfiguration>();
            var currentDate = DateTime.Now;
            try
            {
                using (var db = new gHRMDBContext())
                {
                    objectToCreate.CreateDate = currentDate;
                    db.StaffWelfareFundConfigurations.Add(objectToCreate);                    

                    db.SaveChanges();

                    response.IsSuccess = true;
                    response.Message = "Success, Added Staff Welfare Fund Configuration";
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
        //public GlobalResponse<StaffWelfareFundConfiguration> Update(StaffWelfareFundConfiguration objectToUpdate)
        //{
        //    var response = new GlobalResponse<StaffWelfareFundConfiguration>();
        //    var isOperationSuccess = true;
        //    var currentDate = DateTime.Now;
        //    try
        //    {
        //        using (var db = new gHRMDBContext())
        //        {
        //            var updateStaffWelfareFundConfiguration = db.StaffWelfareFundConfigurations
        //                .FirstOrDefault(f=>f.StaffWelfareFundConfigurationId==objectToUpdate.StaffWelfareFundConfigurationId);

        //            if (updateStaffWelfareFundConfiguration == null)
        //            {
        //                isOperationSuccess = false;
        //                response.IsSuccess = false;
        //                response.Message = "Warning, Staff Welfare Fund Configuration not exist";
        //                response.Result = null;
        //            }

        //            if (isOperationSuccess)
        //            {
        //                updateStaffWelfareFundConfiguration.EmployeeId = objectToUpdate.EmployeeId;
        //                updateStaffWelfareFundConfiguration.StaffWelfareFundSettingId = objectToUpdate.StaffWelfareFundSettingId;
        //                updateStaffWelfareFundConfiguration.CollectionAmount = objectToUpdate.CollectionAmount;
        //                updateStaffWelfareFundConfiguration.CollectionDate = objectToUpdate.CollectionDate;
        //                updateStaffWelfareFundConfiguration.IsActive = objectToUpdate.IsActive;
        //                updateStaffWelfareFundConfiguration.UpdateUser = objectToUpdate.UpdateUser;
        //                updateStaffWelfareFundConfiguration.UpdateDate = currentDate;

        //                db.SaveChanges();

        //                response.IsSuccess = true;
        //                response.Message = "Success, Updated Staff Welfare Fund Configuration";
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
        //public GlobalResponse<StaffWelfareFundConfiguration> Delete(StaffWelfareFundConfiguration staffWelfareFundConfiguration)
        //{
        //    var response = new GlobalResponse<StaffWelfareFundConfiguration>();
        //    var isOperationSuccess = true;
        //    var currentDate = DateTime.Now;
        //    try
        //    {
        //        using (var db = new gHRMDBContext())
        //        {
        //            var deleteStaffWelfareFundConfiguration = db.StaffWelfareFundConfigurations
        //                .FirstOrDefault(f => f.StaffWelfareFundConfigurationId == staffWelfareFundConfiguration.StaffWelfareFundConfigurationId);

        //            if (deleteStaffWelfareFundConfiguration == null)
        //            {
        //                isOperationSuccess = false;
        //                response.IsSuccess = false;
        //                response.Message = "Warning, Staff Welfare Fund Configuration not exist";
        //                response.Result = null ;
        //            }

        //            if (isOperationSuccess)
        //            {
        //                deleteStaffWelfareFundConfiguration.IsActive = false;
        //                deleteStaffWelfareFundConfiguration.UpdateUser = staffWelfareFundConfiguration.UpdateUser;
        //                deleteStaffWelfareFundConfiguration.UpdateDate = currentDate;

        //                db.SaveChanges();

        //                response.IsSuccess = true;
        //                response.Message = "Success, Deleted Staff Welfare Fund Configuration";
        //                response.Result = staffWelfareFundConfiguration;
        //            }
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        response.IsSuccess = false;
        //        response.Message = ex.Message;
        //        response.Result = staffWelfareFundConfiguration;
        //    }

        //    return response;
        //}

        public BaseResponse ConfigureStaffWelfareFund(string EmployeeCode, int PurposeId, decimal FundAmount, string remarks,  long createUser)
        {
            var response = new BaseResponse();
            try
            {
                using (var db = new gHRMDBContext())
                {
                    var sqlCommand = $@"[dbo].[StaffWellfareFundConfiguration_ConfigureStaffWelfareFund]
                                    {EmployeeCode},{PurposeId},{FundAmount},{remarks}, {createUser}
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

        public BaseResponse ConfigureStaffWelfareFund(int year, int month, int staffWellfareFundSettingsId, long createUser)
        {
            throw new NotImplementedException();
        }

        #region Private Methods

        #endregion
    }
}
