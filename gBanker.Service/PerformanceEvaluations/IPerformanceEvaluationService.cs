using gHRM.Core.Filters.PerformanceEvaluations;
using gHRM.Core.Utilities;
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.CodeFirstMigration.PerformanceEvaluations;
using gHRM.Data.DBDetailModels.PerformanceEvaluations;
using gHRM.Data.Repository.PerformanceEvaluations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace gHRM.Service.PerformanceEvaluations
{
    public interface IPerformanceEvaluationService
    {
        IEnumerable<PerformanceEvaluation> GetAll();
        PerformanceEvaluation GetById(int id);
        PerformanceEvaluation GetByYearMonthAndEmployeeId(int year, int month, long employeeId);
        IEnumerable<PerformanceEvaluationModel> GetByPerformanceEvaluationByFilter(PerformanceEvaluationSearchFilter filter);
        GlobalResponse<PerformanceEvaluation> Create(PerformanceEvaluation newPerformanceEvaluation);
        GlobalResponse<PerformanceEvaluation> Update(PerformanceEvaluation updatePerformanceEvaluation);
        GlobalResponse<PerformanceEvaluation> Delete(PerformanceEvaluation updatePerformanceEvaluation);
    }
    public class PerformanceEvaluationService : IPerformanceEvaluationService
    {
        #region Private Variables
        private readonly IPerformanceEvaluationRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;
        #endregion

        #region ctor
        public PerformanceEvaluationService(IPerformanceEvaluationRepository repository,
            IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        #endregion

        #region Public Methods
        public IEnumerable<PerformanceEvaluation> GetAll()
        {
            var listing = new List<PerformanceEvaluation>();
            using (var db = new gHRMDBContext())
            {
                listing = db.PerformanceEvaluations.Where(f => f.IsActive)
                    .AsParallel().ToList();
            }

            return listing;
        }
        public PerformanceEvaluation GetById(int id)
        {
            var single = new PerformanceEvaluation();
            using (var db = new gHRMDBContext())
            {
                single = db.PerformanceEvaluations
                    .FirstOrDefault(f => f.PerformanceEvaluationId == id);
            }

            return single;
        }

        public PerformanceEvaluation GetByYearMonthAndEmployeeId(int year, int month, long employeeId)
        {
            var single = new PerformanceEvaluation();
            using (var db = new gHRMDBContext())
            {
                single = db.PerformanceEvaluations
                        .FirstOrDefault(f => f.EvaluationYear == year
                                            && f.EvaluationMonth == month
                                            && f.EmployeeId == employeeId
                                            && f.IsActive
                                            );
            }

            return single;
        }

        public IEnumerable<PerformanceEvaluationModel> 
            GetByPerformanceEvaluationByFilter(PerformanceEvaluationSearchFilter filter)
        {
            var listing = new List<PerformanceEvaluationModel>();

            var employeeCode = string.IsNullOrWhiteSpace(filter.EmployeeCode) ? "NULL" : "'"+filter.EmployeeCode+"'";

            var officeId = filter.OfficeId>0 ? filter.OfficeId.ToString() : "NULL";
            var branchId =filter.BranchId>0 ? filter.BranchId.ToString() : "NULL";

            using (var db = new gHRMDBContext())
            {
                var sqlCommand = $@"[dbo].[PerformanceEvaluation_GetPerformanceEvaluationsByFilter]
                                {filter.Year},
                                {filter.Month},
                                {employeeCode},
                                {officeId},
                                {branchId}
                                ";

                listing = db.Database.SqlQuery<PerformanceEvaluationModel>(sqlCommand)
                    .AsParallel().ToList();
            }

            return listing;
        }
        public GlobalResponse<PerformanceEvaluation> Create(PerformanceEvaluation newPerformanceEvaluation)
        {
            var response = new GlobalResponse<PerformanceEvaluation>();
            var currentDate = DateTime.Now;
            try
            {
                using (var db = new gHRMDBContext())
                {
                  
                    newPerformanceEvaluation.CreateDate = currentDate;
                    newPerformanceEvaluation.EvaluationDate = currentDate;
                    db.PerformanceEvaluations.Add(newPerformanceEvaluation);

                    db.SaveChanges();

                    response.IsSuccess = true;
                    response.Message = "Success, Added Performance Evaluation";
                    response.Result = newPerformanceEvaluation;
                }

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
                response.Result = newPerformanceEvaluation;
            }

            return response;
        }
        public GlobalResponse<PerformanceEvaluation> Update(PerformanceEvaluation performanceEvaluation)
        {
            var response = new GlobalResponse<PerformanceEvaluation>();
            var isOperationSuccess = true;
            var currentDate = DateTime.Now;
            try
            {
                using (var db = new gHRMDBContext())
                {
                    var updatePerformanceEvaluation = db.PerformanceEvaluations
                        .FirstOrDefault(f => f.PerformanceEvaluationId == performanceEvaluation.PerformanceEvaluationId
                        && f.EmployeeId == performanceEvaluation.EmployeeId);

                    if (updatePerformanceEvaluation == null)
                    {
                        isOperationSuccess = false;
                        response.IsSuccess = false;
                        response.Message = "Warning, Performance Evaluation not exist";
                        response.Result = null;
                    }

                     var performanceEvaluationHistory = updatePerformanceEvaluation;

                    if (isOperationSuccess)
                    {
                        //Populate performance evaluation for update [PerformanceEvaluation]
                        PopulatePerformanceEvaluationForUpdate(performanceEvaluation, currentDate, updatePerformanceEvaluation);

                        //populate performance evaluation history for new insert [PerformanceEvaluationHistory]
                        var newPerformanceEvaluationHistory = PopulatePerformanceEvaluationHistory(performanceEvaluation, currentDate, updatePerformanceEvaluation, performanceEvaluationHistory);

                        //let's add new for [PerformanceEvaluationHistory]
                        db.PerformanceEvaluationHistories.Add(newPerformanceEvaluationHistory);

                        db.SaveChanges();

                        response.IsSuccess = true;
                        response.Message = "Success, Updated Performance Evaluation";
                        response.Result = updatePerformanceEvaluation;
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
                response.Result = performanceEvaluation;
            }

            return response;
        }

        public GlobalResponse<PerformanceEvaluation> Delete(PerformanceEvaluation performanceEvaluation)
        {
            var response = new GlobalResponse<PerformanceEvaluation>();
            var isOperationSuccess = true;
            var currentDate = DateTime.Now;
            try
            {
                using (var db = new gHRMDBContext())
                {
                    var updatePerformanceEvaluation = db.PerformanceEvaluations
                        .FirstOrDefault(f => f.PerformanceEvaluationId == performanceEvaluation.PerformanceEvaluationId
                        && f.EmployeeId == performanceEvaluation.EmployeeId);

                    if (updatePerformanceEvaluation == null)
                    {
                        isOperationSuccess = false;
                        response.IsSuccess = false;
                        response.Message = "Warning, Performance Evaluation not exist";
                        response.Result = null;
                    }

                    if (isOperationSuccess)
                    {
                        //Populate performance evaluation for update [PerformanceEvaluation]
                        updatePerformanceEvaluation.IsActive = false;
                        updatePerformanceEvaluation.UpdatedBy = performanceEvaluation.UpdatedBy;
                        updatePerformanceEvaluation.UpdateDate = currentDate;                       

                        db.SaveChanges();

                        response.IsSuccess = true;
                        response.Message = "Success, Deleted Performance Evaluation";
                        response.Result = updatePerformanceEvaluation;
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
                response.Result = performanceEvaluation;
            }

            return response;
        }

        #endregion

        #region Private Methods
        private PerformanceEvaluationHistory PopulatePerformanceEvaluationHistory(PerformanceEvaluation performanceEvaluation, 
            DateTime currentDate, PerformanceEvaluation updatePerformanceEvaluation, 
            PerformanceEvaluation performanceEvaluationHistory)
        {
            return new PerformanceEvaluationHistory
            {
                OfficeId = updatePerformanceEvaluation.OfficeId,
                PerformanceEvaluationId = updatePerformanceEvaluation.PerformanceEvaluationId,
                EvaluationHistoryDate = currentDate,
                TotalSamity = performanceEvaluationHistory.TotalSamity,
                TotalMember = performanceEvaluationHistory.TotalMember,
                TotalLoanee = performanceEvaluationHistory.TotalLoanee,
                OSP = performanceEvaluationHistory.OSP,
                SpecialSavings = performanceEvaluationHistory.SpecialSavings,
                GeneralSavings = performanceEvaluationHistory.GeneralSavings,
                LoanDisburse = performanceEvaluationHistory.LoanDisburse,
                LoanRepaid = performanceEvaluationHistory.LoanRepaid,
                LoanOutstanding = performanceEvaluationHistory.LoanOutstanding,
                CurrentDueNo = performanceEvaluationHistory.CurrentDueNo,
                CurrentDue = performanceEvaluationHistory.CurrentDue,
                OverDueNo = performanceEvaluationHistory.OverDueNo,
                OverDue = performanceEvaluationHistory.OverDue,
                CreateDate = currentDate,
                CreatedBy = Convert.ToInt64(performanceEvaluation.UpdatedBy)
            };
        }

        private void PopulatePerformanceEvaluationForUpdate(PerformanceEvaluation performanceEvaluation, DateTime currentDate, PerformanceEvaluation updatePerformanceEvaluation)
        {
            updatePerformanceEvaluation.OfficeId = performanceEvaluation.OfficeId;
            updatePerformanceEvaluation.EvaluationYear = performanceEvaluation.EvaluationYear;
            updatePerformanceEvaluation.EvaluationMonth = performanceEvaluation.EvaluationMonth;
            updatePerformanceEvaluation.TotalSamity = performanceEvaluation.TotalSamity;
            updatePerformanceEvaluation.TotalMember = performanceEvaluation.TotalMember;
            updatePerformanceEvaluation.TotalLoanee = performanceEvaluation.TotalLoanee;
            updatePerformanceEvaluation.OSP = performanceEvaluation.OSP;
            updatePerformanceEvaluation.SpecialSavings = performanceEvaluation.SpecialSavings;
            updatePerformanceEvaluation.GeneralSavings = performanceEvaluation.GeneralSavings;
            updatePerformanceEvaluation.LoanDisburse = performanceEvaluation.LoanDisburse;
            updatePerformanceEvaluation.LoanRepaid = performanceEvaluation.LoanRepaid;
            updatePerformanceEvaluation.LoanOutstanding = performanceEvaluation.LoanOutstanding;
            updatePerformanceEvaluation.CurrentDueNo = performanceEvaluation.CurrentDueNo;
            updatePerformanceEvaluation.OverDueNo = performanceEvaluation.OverDueNo;
            updatePerformanceEvaluation.CurrentDue = performanceEvaluation.CurrentDue;
            updatePerformanceEvaluation.OverDue = performanceEvaluation.OverDue;
            updatePerformanceEvaluation.UpdatedBy = performanceEvaluation.UpdatedBy;
            updatePerformanceEvaluation.UpdateDate = currentDate;
        }

        #endregion
    }
}
