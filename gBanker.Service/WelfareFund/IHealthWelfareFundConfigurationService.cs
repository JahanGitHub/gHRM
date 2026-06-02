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
    public interface IHealthWelfareFundConfigurationService
    {
        IEnumerable<HealthWelfareFundConfiguration> GetAll();
        HealthWelfareFundConfiguration GetById(int id);
        GlobalResponse<HealthWelfareFundConfiguration> Create(HealthWelfareFundConfiguration objectToCreate);
        GlobalResponse<HealthWelfareFundConfiguration> Update(HealthWelfareFundConfiguration objectToUpdate);
        GlobalResponse<HealthWelfareFundConfiguration> Delete(HealthWelfareFundConfiguration staffWelfareFundSetting);
        BaseResponse ConfigureHealthWelfareFund(int year, int month, int HealthWelfareFundSettingId, long createUser);
    }
    public class HealthWelfareFundConfigurationService : IHealthWelfareFundConfigurationService
    {
        private readonly IHealthWelfareFundConfigurationRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public HealthWelfareFundConfigurationService(IHealthWelfareFundConfigurationRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        public IEnumerable<HealthWelfareFundConfiguration> GetAll()
        {
            var listing = new List<HealthWelfareFundConfiguration>();
            using (var db = new gHRMDBContext())
            {
                listing = db.HealthWelfareFundConfigurations.Where(f => f.IsActive)
                    .AsParallel().ToList();
            }

            return listing;
        }
        public HealthWelfareFundConfiguration GetById(int id)
        {
            var single = new HealthWelfareFundConfiguration();
            using (var db = new gHRMDBContext())
            {
                single = db.HealthWelfareFundConfigurations
                    .FirstOrDefault(f => f.HealthWelfareFundConfigurationId == id);
            }

            return single;
        }
        public GlobalResponse<HealthWelfareFundConfiguration> Create(HealthWelfareFundConfiguration objectToCreate)
        {
            var response = new GlobalResponse<HealthWelfareFundConfiguration>();
            var currentDate = DateTime.Now;
            try
            {
                using (var db = new gHRMDBContext())
                {
                    objectToCreate.CreateDate = currentDate;
                    db.HealthWelfareFundConfigurations.Add(objectToCreate);

                    db.SaveChanges();

                    response.IsSuccess = true;
                    response.Message = "Success, Added Health Welfare Fund Configuration";
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
        public GlobalResponse<HealthWelfareFundConfiguration> Update(HealthWelfareFundConfiguration objectToUpdate)
        {
            var response = new GlobalResponse<HealthWelfareFundConfiguration>();
            var isOperationSuccess = true;
            var currentDate = DateTime.Now;
            try
            {
                using (var db = new gHRMDBContext())
                {
                    var updateHealthWelfareFundConfiguration = db.HealthWelfareFundConfigurations
                        .FirstOrDefault(f => f.HealthWelfareFundConfigurationId == objectToUpdate.HealthWelfareFundConfigurationId);

                    if (updateHealthWelfareFundConfiguration == null)
                    {
                        isOperationSuccess = false;
                        response.IsSuccess = false;
                        response.Message = "Warning, Health Welfare Fund Configuration not exist";
                        response.Result = null;
                    }

                    if (isOperationSuccess)
                    {
                        updateHealthWelfareFundConfiguration.EmployeeId = objectToUpdate.EmployeeId;
                        updateHealthWelfareFundConfiguration.HealthWelfareFundSettingId = objectToUpdate.HealthWelfareFundSettingId;
                        updateHealthWelfareFundConfiguration.CollectionAmount = objectToUpdate.CollectionAmount;
                        updateHealthWelfareFundConfiguration.CollectionDate = objectToUpdate.CollectionDate;
                        updateHealthWelfareFundConfiguration.IsActive = objectToUpdate.IsActive;
                        updateHealthWelfareFundConfiguration.UpdateUser = objectToUpdate.UpdateUser;
                        updateHealthWelfareFundConfiguration.UpdateDate = currentDate;

                        db.SaveChanges();


                        response.IsSuccess = true;
                        response.Message = "Success, Updated Health Welfare Fund Configuration";
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
        public GlobalResponse<HealthWelfareFundConfiguration> Delete(HealthWelfareFundConfiguration healthWelfareFundConfiguration)
        {
            var response = new GlobalResponse<HealthWelfareFundConfiguration>();
            var isOperationSuccess = true;
            var currentDate = DateTime.Now;
            try
            {
                using (var db = new gHRMDBContext())
                {
                    var deletehealthWelfareFundConfiguration = db.HealthWelfareFundConfigurations
                        .FirstOrDefault(f => f.HealthWelfareFundConfigurationId == healthWelfareFundConfiguration.HealthWelfareFundConfigurationId);

                    if (deletehealthWelfareFundConfiguration == null)
                    {
                        isOperationSuccess = false;
                        response.IsSuccess = false;
                        response.Message = "Warning, Health Welfare Fund Configuration not exist";
                        response.Result = null;
                    }

                    if (isOperationSuccess)
                    {
                        deletehealthWelfareFundConfiguration.IsActive = false;
                        deletehealthWelfareFundConfiguration.UpdateUser = healthWelfareFundConfiguration.UpdateUser;
                        deletehealthWelfareFundConfiguration.UpdateDate = currentDate;

                        db.SaveChanges();


                        response.IsSuccess = true;
                        response.Message = "Success, Deleted Health Welfare Fund Configuration";
                        response.Result = healthWelfareFundConfiguration;
                    }
                }

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
                response.Result = healthWelfareFundConfiguration;
            }

            return response;
        }


        public BaseResponse ConfigureHealthWelfareFund(int year, int month, int HealthWelfareFundSettingId, long createUser)
        {
            var response = new BaseResponse();
            try
            {
                using (var db = new gHRMDBContext())
                {
                    var sqlCommand = $@"[dbo].[HealthWellfareFundConfiguration_ConfigureHealthWelfareFund]
                                    {year},{month},{HealthWelfareFundSettingId},{createUser}
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
