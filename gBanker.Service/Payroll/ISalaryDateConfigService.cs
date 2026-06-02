using gHRM.Core.Utilities;
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.CodeFirstMigration.Payroll;
using gHRM.Data.Repository.Payroll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace gHRM.Service.Payroll
{
    public interface ISalaryDateConfigService
    {
        IEnumerable<SalaryDateConfig> GetAll();
        SalaryDateConfig GetById(int id);
        BaseResponse IsValidSalaryDateConfig(SalaryDateConfig model);
        GlobalResponse<SalaryDateConfig> Create(SalaryDateConfig objectToCreate);
        GlobalResponse<SalaryDateConfig> Update(SalaryDateConfig objectToUpdate);
        GlobalResponse<SalaryDateConfig> Delete(SalaryDateConfig MonthlySalaryProcessConfig);
        SalaryDateConfig GetCurrentSalaryDateConfig();
    }
    public class SalaryDateConfigService : ISalaryDateConfigService
    {
        private readonly ISalaryDateConfigRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public SalaryDateConfigService(ISalaryDateConfigRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        public IEnumerable<SalaryDateConfig> GetAll()
        {
            var listing = new List<SalaryDateConfig>();
            using (var db = new gHRMDBContext())
            {
                listing = db.SalaryDateConfigs.Where(f => f.IsActive)
                    .AsParallel().ToList();
            }

            return listing;
        }
        public SalaryDateConfig GetById(int id)
        {
            var single = new SalaryDateConfig();
            using (var db = new gHRMDBContext())
            {
                single = db.SalaryDateConfigs
                    .FirstOrDefault(f => f.Id == id);
            }

            return single;
        }

        public SalaryDateConfig GetCurrentSalaryDateConfig()
        {
            var single = new SalaryDateConfig();
            using (var db = new gHRMDBContext())
            {
                single = db.SalaryDateConfigs
                    .FirstOrDefault(f => f.IsActive && f.IsCurrentlyUsing);
            }

            return single;
        }

        public BaseResponse IsValidSalaryDateConfig(SalaryDateConfig model)
        {
            var response = new BaseResponse();
            using (var db = new gHRMDBContext())
            {
                var isFound = true;
                if (model.Id > 0)
                    isFound = db.SalaryDateConfigs
                        .Any(f => f.Id != model.Id && f.DayOfMonthlySalary == model.DayOfMonthlySalary);
                else
                    isFound = db.SalaryDateConfigs
                       .Any(f =>f.IsActive && f.DayOfMonthlySalary == model.DayOfMonthlySalary);

                response = new BaseResponse
                {
                    IsSuccess = !isFound, //not valid
                    Message = isFound ? $"{model.DayOfMonthlySalary} already exist. Please try again" : "Sucess"
                };
            }

            return response;
        }
        public GlobalResponse<SalaryDateConfig> Create(SalaryDateConfig objectToCreate)
        {
            var response = new GlobalResponse<SalaryDateConfig>();
            var currentDate = DateTime.Now;
            var isOperationSuccess = true;
            try
            {
                using (var db = new gHRMDBContext())
                {
                    //prevent creation when no item found for is current as true
                    var isFoundAnyCurrentlyUsingConfig = db.SalaryDateConfigs.Any(f => f.IsActive && f.IsCurrentlyUsing);
                    if (!objectToCreate.IsCurrentlyUsing && !isFoundAnyCurrentlyUsingConfig)
                    {
                        response.IsSuccess = false;
                        response.Message = "Warning, Please create atleast one config as is current as true";
                        response.Result = objectToCreate;
                        isOperationSuccess = false;
                    }

                    if (isOperationSuccess)
                    {
                        objectToCreate.CreateDate = currentDate;
                        db.SalaryDateConfigs.Add(objectToCreate);

                        db.SaveChanges();

                        //if is current then update others as is current false.
                        if (objectToCreate.IsCurrentlyUsing)
                        {
                            var listingUpdateSalaryDateConfig = db.SalaryDateConfigs.Where(f => f.IsActive && f.Id != objectToCreate.Id);
                            foreach (var item in listingUpdateSalaryDateConfig)
                            {
                                var updateSalaryDateConfig = db.SalaryDateConfigs.FirstOrDefault(f => f.Id == item.Id);

                                updateSalaryDateConfig.IsCurrentlyUsing = false;
                                updateSalaryDateConfig.UpdateUser = objectToCreate.UpdateUser;
                                updateSalaryDateConfig.UpdateDate = currentDate;
                            }

                            if (listingUpdateSalaryDateConfig.Any())
                                db.SaveChanges();
                        }

                        response.IsSuccess = true;
                        response.Message = "Success, Added Salary Date Config";
                        response.Result = objectToCreate;
                    }
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
        public GlobalResponse<SalaryDateConfig> Update(SalaryDateConfig objectToUpdate)
        {
            var response = new GlobalResponse<SalaryDateConfig>();
            var isOperationSuccess = true;
            var currentDate = DateTime.Now;
            try
            {
                using (var db = new gHRMDBContext())
                {
                    //prevent creation when no item found for is current as true
                    var isFoundAnyCurrentlyUsingConfig = db.SalaryDateConfigs.Any(f => f.IsActive && f.IsCurrentlyUsing && f.Id!=objectToUpdate.Id);
                    if (!objectToUpdate.IsCurrentlyUsing && !isFoundAnyCurrentlyUsingConfig)
                    {
                        response.IsSuccess = false;
                        response.Message = "Warning, Please update atleast one config 'IS CURRENT' as true";
                        response.Result = objectToUpdate;
                        isOperationSuccess = false;
                    }

                    if (isOperationSuccess)
                    {
                        var updateSalaryDateConfig = db.SalaryDateConfigs
                            .FirstOrDefault(f => f.Id == objectToUpdate.Id);

                        if (updateSalaryDateConfig == null)
                        {
                            isOperationSuccess = false;
                            response.IsSuccess = false;
                            response.Message = "Warning, Salary Date Config not exist";
                            response.Result = null;
                        }

                        if (isOperationSuccess)
                        {
                            updateSalaryDateConfig.DayOfMonthlySalary = objectToUpdate.DayOfMonthlySalary;
                            updateSalaryDateConfig.IsCurrentlyUsing = objectToUpdate.IsCurrentlyUsing;
                            updateSalaryDateConfig.IsActive = objectToUpdate.IsActive;
                            updateSalaryDateConfig.UpdateUser = objectToUpdate.UpdateUser;
                            updateSalaryDateConfig.UpdateDate = currentDate;

                            db.SaveChanges();

                            //if is current then update others as is current false.
                            if (objectToUpdate.IsCurrentlyUsing)
                            {
                                var listingUpdateSalaryDateConfig = db.SalaryDateConfigs.Where(f => f.IsActive && f.Id != objectToUpdate.Id);
                                foreach (var item in listingUpdateSalaryDateConfig)
                                {
                                    var itemUpdateSalaryDateConfig = db.SalaryDateConfigs.FirstOrDefault(f => f.Id == item.Id);

                                    itemUpdateSalaryDateConfig.IsCurrentlyUsing = false;
                                    itemUpdateSalaryDateConfig.UpdateUser = objectToUpdate.UpdateUser;
                                    itemUpdateSalaryDateConfig.UpdateDate = currentDate;
                                }

                                if (listingUpdateSalaryDateConfig.Any())
                                    db.SaveChanges();
                            }

                            response.IsSuccess = true;
                            response.Message = "Success, Updated Salary Date Config";
                            response.Result = objectToUpdate;
                        }
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
        public GlobalResponse<SalaryDateConfig> Delete(SalaryDateConfig monthlySalaryProcessConfig)
        {
            var response = new GlobalResponse<SalaryDateConfig>();
            var isOperationSuccess = true;
            var currentDate = DateTime.Now;
            try
            {
                using (var db = new gHRMDBContext())
                {
                    var deleteSalaryDateConfig = db.SalaryDateConfigs
                        .FirstOrDefault(f => f.Id == monthlySalaryProcessConfig.Id);

                    if (deleteSalaryDateConfig == null)
                    {
                        isOperationSuccess = false;
                        response.IsSuccess = false;
                        response.Message = "Warning, Salary Date Config not exist";
                        response.Result = null;
                    }

                    if (isOperationSuccess)
                    {
                        deleteSalaryDateConfig.IsActive = false;
                        deleteSalaryDateConfig.UpdateUser = monthlySalaryProcessConfig.UpdateUser;
                        deleteSalaryDateConfig.UpdateDate = currentDate;

                        db.SaveChanges();

                        response.IsSuccess = true;
                        response.Message = "Success, Deleted Salary Date Config";
                        response.Result = monthlySalaryProcessConfig;
                    }
                }

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
                response.Result = monthlySalaryProcessConfig;
            }

            return response;
        }
    }
}

