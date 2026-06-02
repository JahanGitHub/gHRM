using gHRM.Core.Utilities;
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using System;
using System.Collections.Generic;
using System.Linq;
using gHRM.Data.CodeFirstMigration.Payroll;
using gHRM.Data.Repository.Payroll;

namespace gHRM.Service.Payroll
{
    public interface ICompanyWisePayrollConfigService
    {
        IEnumerable<CompanyWisePayrollConfig> GetAll();
        CompanyWisePayrollConfig GetById(int id);
        CompanyWisePayrollConfig GetByCompanyCode(string companyCode);
        GlobalResponse<CompanyWisePayrollConfig> Create(CompanyWisePayrollConfig newCompanyWisePayrollConfig);
        GlobalResponse<CompanyWisePayrollConfig> Update(CompanyWisePayrollConfig updateCompanyWisePayrollConfig);
        GlobalResponse<CompanyWisePayrollConfig> Delete(CompanyWisePayrollConfig updateCompanyWisePayrollConfig);
        bool IsExistCompanyWisePayrollConfig(CompanyWisePayrollConfig newCompanyWisePayrollConfigViewModel);
    }
    public class CompanyWisePayrollConfigService : ICompanyWisePayrollConfigService
    {
        #region Private Variables
        private readonly ICompanyWisePayrollConfigRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;
        #endregion

        #region ctor
        public CompanyWisePayrollConfigService(ICompanyWisePayrollConfigRepository repository,
            IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        #endregion

        #region Public Methods
        public IEnumerable<CompanyWisePayrollConfig> GetAll()
        {
            var listing = new List<CompanyWisePayrollConfig>();
            using (var db = new gHRMDBContext())
            {
                listing = db.CompanyWisePayrollConfigs.Where(f => f.IsActive)
                    .AsParallel().ToList();
            }

            return listing;
        }
        public CompanyWisePayrollConfig GetById(int id)
        {
            var single = new CompanyWisePayrollConfig();
            using (var db = new gHRMDBContext())
            {
                single = db.CompanyWisePayrollConfigs
                    .FirstOrDefault(f => f.Id == id);
            }

            return single;
        }

        public CompanyWisePayrollConfig GetByCompanyCode(string companyCode)
        {
            var single = new CompanyWisePayrollConfig();
            using (var db = new gHRMDBContext())
            {
                single = db.CompanyWisePayrollConfigs
                    .FirstOrDefault(f => f.CompanyCode == companyCode.Trim());
            }

            return single;
        }

        public bool IsExistCompanyWisePayrollConfig(CompanyWisePayrollConfig model)
        {
            var isExistConfig =true ;
            using (var db = new gHRMDBContext())
            {
                if(model.Id>0)
                    isExistConfig = db.CompanyWisePayrollConfigs
                    .Any(f => f.CompanyCode == model.CompanyCode && f.Id!=model.Id);
                else
                    isExistConfig = db.CompanyWisePayrollConfigs
                                .Any(f => f.CompanyCode == model.CompanyCode );
            }

            return isExistConfig;
        }

        public GlobalResponse<CompanyWisePayrollConfig> Create(CompanyWisePayrollConfig newCompanyWisePayrollConfig)
        {
            var response = new GlobalResponse<CompanyWisePayrollConfig>();
            var currentDate = DateTime.Now;
            try
            {
                using (var db = new gHRMDBContext())
                {
                  
                    newCompanyWisePayrollConfig.CreateDate = currentDate;
                    newCompanyWisePayrollConfig.CreateUser = newCompanyWisePayrollConfig.CreateUser;
                    db.CompanyWisePayrollConfigs.Add(newCompanyWisePayrollConfig);

                    db.SaveChanges();

                    response.IsSuccess = true;
                    response.Message = "Success, Added Company Wise Payroll Config";
                    response.Result = newCompanyWisePayrollConfig;
                }

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
                response.Result = newCompanyWisePayrollConfig;
            }

            return response;
        }
        public GlobalResponse<CompanyWisePayrollConfig> Update(CompanyWisePayrollConfig companyWisePayrollConfig)
        {
            var response = new GlobalResponse<CompanyWisePayrollConfig>();
            var isOperationSuccess = true;
            var currentDate = DateTime.Now;
            try
            {
                using (var db = new gHRMDBContext())
                {
                    var updateCompanyWisePayrollConfig = db.CompanyWisePayrollConfigs
                        .FirstOrDefault(f => f.Id == companyWisePayrollConfig.Id);

                    if (updateCompanyWisePayrollConfig == null)
                    {
                        isOperationSuccess = false;
                        response.IsSuccess = false;
                        response.Message = "Warning, Company Wise Payroll Config not exist";
                        response.Result = null;
                    }
                    if (isOperationSuccess)
                    {
                        //Populate Company Wise Payroll Config for update [CompanyWisePayrollConfig]
                        PopulateCompanyWisePayrollConfigForUpdate(companyWisePayrollConfig, currentDate, updateCompanyWisePayrollConfig);                        
                    }

                    db.SaveChanges();

                    response.IsSuccess = true;
                    response.Message = "Success, Updated Successfully";
                    response.Result = companyWisePayrollConfig;
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
                response.Result = companyWisePayrollConfig;
            }

            return response;
        }

        public GlobalResponse<CompanyWisePayrollConfig> Delete(CompanyWisePayrollConfig companyWisePayrollConfig)
        {
            var response = new GlobalResponse<CompanyWisePayrollConfig>();
            var isOperationSuccess = true;
            var currentDate = DateTime.Now;
            try
            {
                using (var db = new gHRMDBContext())
                {
                    var updateCompanyWisePayrollConfig = db.CompanyWisePayrollConfigs
                        .FirstOrDefault(f => f.Id == companyWisePayrollConfig.Id);

                    if (updateCompanyWisePayrollConfig == null)
                    {
                        isOperationSuccess = false;
                        response.IsSuccess = false;
                        response.Message = "Warning, Company Wise Payroll Config not exist";
                        response.Result = null;
                    }

                    if (isOperationSuccess)
                    {
                        //Populate Company Wise Payroll Config for update [CompanyWisePayrollConfig]
                        updateCompanyWisePayrollConfig.IsActive = false;
                        updateCompanyWisePayrollConfig.UpdateUser = companyWisePayrollConfig.UpdateUser;
                        updateCompanyWisePayrollConfig.UpdateDate = currentDate;                       

                        db.SaveChanges();

                        response.IsSuccess = true;
                        response.Message = "Success, Deleted Company Wise Payroll Config";
                        response.Result = updateCompanyWisePayrollConfig;
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
                response.Result = companyWisePayrollConfig;
            }

            return response;
        }

        #endregion

        #region Private Methods
       
        private void PopulateCompanyWisePayrollConfigForUpdate(CompanyWisePayrollConfig performanceEvaluation, DateTime currentDate, CompanyWisePayrollConfig updateCompanyWisePayrollConfig)
        {
            updateCompanyWisePayrollConfig.CompanyCode = performanceEvaluation.CompanyCode;            
            updateCompanyWisePayrollConfig.PayrollType = performanceEvaluation.PayrollType;
            updateCompanyWisePayrollConfig.Description = performanceEvaluation.Description;           
            updateCompanyWisePayrollConfig.UpdateUser = performanceEvaluation.UpdateUser;
            updateCompanyWisePayrollConfig.UpdateDate = currentDate;
        }
        
        #endregion
    }
}
