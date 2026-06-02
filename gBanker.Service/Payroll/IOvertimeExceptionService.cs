using BasicDataAccess;
using gHRM.Core.Filters;
using gHRM.Core.Utilities;
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.CodeFirstMigration.Payroll;
using gHRM.Data.DBDetailModels.Overtimes;
using gHRM.Data.Repository;
using gHRM.Data.Repository.Payroll;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace gHRM.Service.Payroll
{
    public interface IOvertimeExceptionService
    {
        DataSet GetOveretimeExceptionDataWithParameter<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class;
        IEnumerable<OvertimeExceptionModel> GetListByFilter(BaseSearchFilter filter);
        IEnumerable<OvertimeException> GetAll();
        OvertimeException GetById(int id);
        OvertimeException GetEmployeeOvertimeException(BaseSearchFilter filter);
        BaseResponse IsValidOvertimeException(OvertimeException model);
        BaseResponse IsValidOvertimeExceptionEffectiveDate(OvertimeException model);
        GlobalResponse<OvertimeException> Create(OvertimeException objectToCreate);
        GlobalResponse<OvertimeException> Update(OvertimeException objectToUpdate);
        GlobalResponse<OvertimeException> Delete(OvertimeException MonthlySalaryProcessConfig);
        OvertimeException GetCurrentOvertimeException();
    }
    public class OvertimeExceptionService : IOvertimeExceptionService
    {
        private readonly IOvertimeExceptionRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public OvertimeExceptionService(IOvertimeExceptionRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }


        public DataSet GetOveretimeExceptionDataWithParameter<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class
        {
            using (var gbData = new gHRMDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public IEnumerable<OvertimeExceptionModel> GetListByFilter(BaseSearchFilter filter)
        {
            return repository.GetListingByFilter(filter);
        }

        public DataSet GetOveretimeExceptionDataWithoutParameter(string storeProcedureName)
        {
            using (var gbData = new gHRMDataAccess())
            {
                return gbData.GetDataOnDatesetWithoutParam(storeProcedureName);
            }
        }
        public IEnumerable<OvertimeException> GetAll()
        {
            var listing = new List<OvertimeException>();
            using (var db = new gHRMDBContext())
            {
                listing = db.OvertimeExceptions.Where(f => f.IsActive)
                    .AsParallel().ToList();
            }

            return listing;
        }
        public OvertimeException GetById(int id)
        {
            var single = new OvertimeException();
            using (var db = new gHRMDBContext())
            {
                single = db.OvertimeExceptions
                    .FirstOrDefault(f => f.Id == id);
            }

            return single;
        }

        public OvertimeException GetEmployeeOvertimeException(BaseSearchFilter filter)
        {
            var single = new OvertimeException();
            using (var db = new gHRMDBContext())
            {
                single = db.OvertimeExceptions
                    .FirstOrDefault(f =>
                           f.IsActive 
                        && f.EmployeeId == filter.EmployeeId
                        && (
                            (filter.StartDate == null || filter.EndDate==null) || 
                            (
                                 (filter.StartDate>=f.EffectiveStartDate && filter.StartDate<=f.EffectiveEndDate) 
                              || (filter.EndDate >= f.EffectiveEndDate && filter.EndDate <= f.EffectiveEndDate)
                            )
                           )
                    );
            }

            return single;
        }

        public OvertimeException GetCurrentOvertimeException()
        {
            var single = new OvertimeException();
            using (var db = new gHRMDBContext())
            {
                single = db.OvertimeExceptions
                    .FirstOrDefault(f => f.IsActive);
            }

            return single;
        }

        public BaseResponse IsValidOvertimeException(OvertimeException model)
        {
            var response = new BaseResponse();
            using (var db = new gHRMDBContext())
            {
                var isFound = true;
                if (model.Id > 0)
                    isFound = db.OvertimeExceptions
                        .Any(f => f.Id != model.Id );
                else
                    isFound = db.OvertimeExceptions
                       .Any(f =>f.IsActive);

                response = new BaseResponse
                {
                    IsSuccess = !isFound, //not valid
                    Message = isFound ? $" already exist. Please try again" : "Sucess"
                };
            }

            return response;
        }       
        public BaseResponse IsValidOvertimeExceptionEffectiveDate(OvertimeException model)
        {          
            return repository.IsValidOvertimeExceptionEffectiveDate(model);
        }

        public GlobalResponse<OvertimeException> Create(OvertimeException objectToCreate)
        {
            var response = new GlobalResponse<OvertimeException>();
            var currentDate = DateTime.Now;
            var isOperationSuccess = true;
            try
            {
                using (var db = new gHRMDBContext())
                {                   

                    if (isOperationSuccess)
                    {
                        objectToCreate.CreateDate = currentDate;
                        db.OvertimeExceptions.Add(objectToCreate);

                        db.SaveChanges();                       

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
        public GlobalResponse<OvertimeException> Update(OvertimeException objectToUpdate)
        {
            var response = new GlobalResponse<OvertimeException>();
            var isOperationSuccess = true;
            var currentDate = DateTime.Now;
            try
            {
                using (var db = new gHRMDBContext())
                {                   

                    if (isOperationSuccess)
                    {
                        var updateOvertimeException = db.OvertimeExceptions
                            .FirstOrDefault(f => f.Id == objectToUpdate.Id);

                        if (updateOvertimeException == null)
                        {
                            isOperationSuccess = false;
                            response.IsSuccess = false;
                            response.Message = "Warning, Salary Date Config not exist";
                            response.Result = null;
                        }

                        if (isOperationSuccess)
                        {
                            updateOvertimeException.ExceptionType = objectToUpdate.ExceptionType;
                            updateOvertimeException.EffectiveStartDate = objectToUpdate.EffectiveStartDate;
                            updateOvertimeException.EffectiveEndDate = objectToUpdate.EffectiveEndDate;
                            updateOvertimeException.IsActive = objectToUpdate.IsActive;
                            updateOvertimeException.UpdateUser = objectToUpdate.UpdateUser;
                            updateOvertimeException.UpdateDate = currentDate;

                            db.SaveChanges();                            
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
        public GlobalResponse<OvertimeException> Delete(OvertimeException monthlySalaryProcessConfig)
        {
            var response = new GlobalResponse<OvertimeException>();
            var isOperationSuccess = true;
            var currentDate = DateTime.Now;
            try
            {
                using (var db = new gHRMDBContext())
                {
                    var deleteOvertimeException = db.OvertimeExceptions
                        .FirstOrDefault(f => f.Id == monthlySalaryProcessConfig.Id);

                    if (deleteOvertimeException == null)
                    {
                        isOperationSuccess = false;
                        response.IsSuccess = false;
                        response.Message = "Warning, Salary Date Config not exist";
                        response.Result = null;
                    }

                    if (isOperationSuccess)
                    {
                        deleteOvertimeException.IsActive = false;
                        deleteOvertimeException.UpdateUser = monthlySalaryProcessConfig.UpdateUser;
                        deleteOvertimeException.UpdateDate = currentDate;

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

