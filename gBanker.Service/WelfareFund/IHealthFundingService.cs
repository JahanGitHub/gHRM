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
    public interface IHealthFundingService
    {
        IEnumerable<HealthFunding> GetAll();
        HealthFunding GetById(int id);
        GlobalResponse<HealthFunding> Create(HealthFunding objectToCreate);
        //  GlobalResponse<HealthFunding> Update(HealthFunding objectToUpdate);
        GlobalResponse<HealthFunding> Delete(HealthFunding staffWelfareFundSetting);  
        BaseResponse ConfigureStaffWelfareFund(string employeeCode, int purposeId, decimal fundAmount, string remarks, long createUser);
    }
    public class HealthFundingService : IHealthFundingService
    {
        private readonly IHealthFundingRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public HealthFundingService(IHealthFundingRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        public IEnumerable<HealthFunding> GetAll()
        {
            var listing = new List<HealthFunding>();
            using (var db = new gHRMDBContext())
            {
                listing = db.HealthFundings.Where(f => f.IsActive)
                    .AsParallel().ToList();
            }

            return listing;
        }
        public HealthFunding GetById(int id)
        {
            var single = new HealthFunding();
            using (var db = new gHRMDBContext())
            {
                single = db.HealthFundings
                    .FirstOrDefault(f => f.Id == id);
            }

            return single;
        }
        public GlobalResponse<HealthFunding> Create(HealthFunding objectToCreate)
        {
            var response = new GlobalResponse<HealthFunding>();
            var currentDate = DateTime.Now;
            try
            {
                using (var db = new gHRMDBContext())
                {
                    objectToCreate.CreateDate = currentDate;
                    db.HealthFundings.Add(objectToCreate);

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
        //public GlobalResponse<HealthFunding> Update(HealthFunding objectToUpdate)
        //{
        //    var response = new GlobalResponse<HealthFunding>();
        //    var isOperationSuccess = true;
        //    var currentDate = DateTime.Now;
        //    try
        //    {
        //        using (var db = new gHRMDBContext())
        //        {
        //            var updateHealthFunding = db.HealthFundings
        //                .FirstOrDefault(f=>f.HealthFundingId==objectToUpdate.HealthFundingId);

        //            if (updateHealthFunding == null)
        //            {
        //                isOperationSuccess = false;
        //                response.IsSuccess = false;
        //                response.Message = "Warning, Staff Welfare Fund Configuration not exist";
        //                response.Result = null;
        //            }

        //            if (isOperationSuccess)
        //            {
        //                updateHealthFunding.EmployeeId = objectToUpdate.EmployeeId;
        //                updateHealthFunding.StaffWelfareFundSettingId = objectToUpdate.StaffWelfareFundSettingId;
        //                updateHealthFunding.CollectionAmount = objectToUpdate.CollectionAmount;
        //                updateHealthFunding.CollectionDate = objectToUpdate.CollectionDate;
        //                updateHealthFunding.IsActive = objectToUpdate.IsActive;
        //                updateHealthFunding.UpdateUser = objectToUpdate.UpdateUser;
        //                updateHealthFunding.UpdateDate = currentDate;

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
        public GlobalResponse<HealthFunding> Delete(HealthFunding HealthFunding)
        {
            var response = new GlobalResponse<HealthFunding>();
            var isOperationSuccess = true;
            var currentDate = DateTime.Now;
            try
            {
                using (var db = new gHRMDBContext())
                {
                    var deleteHealthFunding = db.HealthFundings
                        .FirstOrDefault(f => f.Id == HealthFunding.Id);

                    if (deleteHealthFunding == null)
                    {
                        isOperationSuccess = false;
                        response.IsSuccess = false;
                        response.Message = "Warning, Health Funding not exist";
                        response.Result = null;
                    }

                    if (isOperationSuccess)
                    {
                        deleteHealthFunding.IsActive = false;
                        //deleteHealthFunding.UpdateUser = HealthFunding.UpdateUser;
                        //deleteHealthFunding.UpdateDate = currentDate;

                        db.SaveChanges();

                        response.IsSuccess = true;
                        response.Message = "Success, Inactive Health Funding";
                        response.Result = HealthFunding;
                    }
                }

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
                response.Result = HealthFunding;
            }

            return response;
        }

        public BaseResponse ConfigureStaffWelfareFund(string EmployeeCode, int PurposeId, decimal FundAmount, string remarks, long createUser)
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
