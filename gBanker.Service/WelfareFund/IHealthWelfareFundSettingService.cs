using gHRM.Core.Utilities;
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.HealthWelfareFund;
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
    public interface IHealthWelfareFundSettingService
    {
        IEnumerable<HealthWelfareFundSetting> GetAll();
        HealthWelfareFundSetting GetById(int id);
        GlobalResponse<HealthWelfareFundSetting> Create(HealthWelfareFundSetting objectToCreate);
        GlobalResponse<HealthWelfareFundSetting> Update(HealthWelfareFundSetting objectToUpdate);
        GlobalResponse<HealthWelfareFundSetting> Delete(HealthWelfareFundSetting staffWelfareFundSetting);
    }
    public class HealthWelfareFundSettingService : IHealthWelfareFundSettingService
    {
        private readonly IHealthWelfareFundSettingRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public HealthWelfareFundSettingService(IHealthWelfareFundSettingRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        public IEnumerable<HealthWelfareFundSetting> GetAll()
        {
            var listing = new List<HealthWelfareFundSetting>();
            using (var db = new gHRMDBContext())
            {
                listing = db.HealthWelfareFundSettings.Where(f => f.IsActive)
                    .AsParallel().ToList();
            }

            return listing;
        }
        public HealthWelfareFundSetting GetById(int id)
        {
            var single = new HealthWelfareFundSetting();
            using (var db = new gHRMDBContext())
            {
                single = db.HealthWelfareFundSettings
                    .FirstOrDefault(f => f.HealthWelfareFundSettingId == id);
            }

            return single;
        }
        public GlobalResponse<HealthWelfareFundSetting> Create(HealthWelfareFundSetting objectToCreate)
        {
            var response = new GlobalResponse<HealthWelfareFundSetting>();
            var currentDate = DateTime.Now;
            try
            {
                using (var db = new gHRMDBContext())
                {
                    objectToCreate.CreateDate = currentDate;
                    db.HealthWelfareFundSettings.Add(objectToCreate);

                    db.SaveChanges();

                    response.IsSuccess = true;
                    response.Message = "Success, Added Health Welfare Fund Setting";
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
        public GlobalResponse<HealthWelfareFundSetting> Update(HealthWelfareFundSetting objectToUpdate)
        {
            var response = new GlobalResponse<HealthWelfareFundSetting>();
            var isOperationSuccess = true;
            var currentDate = DateTime.Now;
            try
            {
                using (var db = new gHRMDBContext())
                {
                    var updateHealthWelfareFundSetting = db.HealthWelfareFundSettings
                        .FirstOrDefault(f => f.HealthWelfareFundSettingId == objectToUpdate.HealthWelfareFundSettingId);

                    if (updateHealthWelfareFundSetting == null)
                    {
                        isOperationSuccess = false;
                        response.IsSuccess = false;
                        response.Message = "Warning, Health Welfare Fund Setting not exist";
                        response.Result = null;
                    }

                    if (isOperationSuccess)
                    {
                        updateHealthWelfareFundSetting.DeductionAmount = objectToUpdate.DeductionAmount;
                        updateHealthWelfareFundSetting.IsPercentage = objectToUpdate.IsPercentage;
                        updateHealthWelfareFundSetting.IsActive = objectToUpdate.IsActive;
                        updateHealthWelfareFundSetting.UpdateUser = objectToUpdate.UpdateUser;
                        updateHealthWelfareFundSetting.UpdateDate = currentDate;

                        db.SaveChanges();


                        response.IsSuccess = true;
                        response.Message = "Success, Updated Health Welfare Fund Setting";
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
        public GlobalResponse<HealthWelfareFundSetting> Delete(HealthWelfareFundSetting objectToUpdate)
        {
            var response = new GlobalResponse<HealthWelfareFundSetting>();
            var isOperationSuccess = true;
            var currentDate = DateTime.Now;
            try
            {
                using (var db = new gHRMDBContext())
                {
                    var deleteHealthWelfareFundSetting = db.HealthWelfareFundSettings
                        .FirstOrDefault(f => f.HealthWelfareFundSettingId == objectToUpdate.HealthWelfareFundSettingId);

                    if (deleteHealthWelfareFundSetting == null)
                    {
                        isOperationSuccess = false;
                        response.IsSuccess = false;
                        response.Message = "Warning, Health Welfare Fund Setting not exist";
                        response.Result = null;
                    }

                    if (isOperationSuccess)
                    {
                        deleteHealthWelfareFundSetting.IsActive = false;
                        deleteHealthWelfareFundSetting.UpdateUser = objectToUpdate.UpdateUser;
                        deleteHealthWelfareFundSetting.UpdateDate = currentDate;

                        db.SaveChanges();

                        response.IsSuccess = true;
                        response.Message = "Success, Deleted Health Welfare Fund Setting";
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
