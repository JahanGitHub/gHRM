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
    public interface IFundSetupService
    {
        IEnumerable<FundSetup> GetAll();
        FundSetup GetById(int id);
        GlobalResponse<FundSetup> Create(FundSetup objectToCreate);
        //GlobalResponse<StaffWelfareFundSetting> Update(StaffWelfareFundSetting objectToUpdate);
        GlobalResponse<FundSetup> Delete(FundSetup fundSetup);
    }
    public class FundSetupService : IFundSetupService
    {
        private readonly IFundSetupRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public FundSetupService(IFundSetupRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        
        public IEnumerable<FundSetup> GetAll()
        {
            var listing = new List<FundSetup>();
            using (var db = new gHRMDBContext())
            {
                listing = db.FundSetups.Where(f => f.IsActive)
                    .AsParallel().ToList();
            }

            return listing;
        }
        public FundSetup GetById(int id)
        {
            var single = new FundSetup();
            using (var db = new gHRMDBContext())
            {
                single = db.FundSetups
                    .FirstOrDefault(f=>f.Id==id);
            }

            return single;
        }
        public GlobalResponse<FundSetup> Create(FundSetup objectToCreate)
        {
            var response = new GlobalResponse<FundSetup>();
            var currentDate = DateTime.Now;
            try
            {
                using (var db = new gHRMDBContext())
                {
                    objectToCreate.CreateDate = currentDate;
                    db.FundSetups.Add(objectToCreate);                    

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
        public GlobalResponse<FundSetup> Update(FundSetup objectToUpdate)
        {
            var response = new GlobalResponse<FundSetup>();
            var isOperationSuccess = true;
            var currentDate = DateTime.Now;
            try
            {
                using (var db = new gHRMDBContext())
                {
                    var updateStaffWelfareFundSetting = db.FundSetups
                        .FirstOrDefault(f => f.Id == objectToUpdate.Id);

                    if (updateStaffWelfareFundSetting == null)
                    {
                        isOperationSuccess = false;
                        response.IsSuccess = false;
                        response.Message = "Warning, Fund Setting not exist";
                        response.Result = null;
                    }

                    if (isOperationSuccess)
                    {
                        updateStaffWelfareFundSetting.FundType = objectToUpdate.FundType;
                        updateStaffWelfareFundSetting.PRComponentId = objectToUpdate.PRComponentId;
                        updateStaffWelfareFundSetting.IsActive = objectToUpdate.IsActive;
                        //updateStaffWelfareFundSetting.UpdateUser = objectToUpdate.UpdateUser;
                        //updateStaffWelfareFundSetting.UpdateDate = currentDate;

                        db.SaveChanges();

                        response.IsSuccess = true;
                        response.Message = "Success, Updated Staff Welfare Fund Setting";
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
        public GlobalResponse<FundSetup> Delete(FundSetup staffWelfareFundSetting)
        {
            var response = new GlobalResponse<FundSetup>();
            var isOperationSuccess = true;
            var currentDate = DateTime.Now;
            try
            {
                using (var db = new gHRMDBContext())
                {
                    var deleteStaffWelfareFundSetting = db.FundSetups
                        .FirstOrDefault(f => f.Id == staffWelfareFundSetting.Id);

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
