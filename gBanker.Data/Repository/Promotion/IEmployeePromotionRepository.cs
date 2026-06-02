using gHRM.Core.Utilities;
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.EmployeePromotion;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace gHRM.Data.Repository.Promotion
{
    public interface IEmployeePromotionRepository : IRepository<EmployeePromotion>
    {
        EmployeePromotion GetEmployeePromotionByDateRange(long employeeId, DateTime startDate, DateTime endDate);
        BaseResponse ValidatePromotion(long employeeId, int promotionId, DateTime promotionDate);
        void GetDataFromExcelData(string EmployeeCode, string PayrollDesignation, string PromotionType, out long EmployeeId, out int PayrollDesignationId, out int PromotionTypeId);
        bool IsDuplicate(long EmployeeId, long PromotionTypeId, DateTime PromotionDate);

        //
        EmployeePromotion GetByEmpIdLong(long id);

        IEnumerable<PromotionCommonClass> GetAllPromotionScoreCollection();

        IEnumerable<PromotionCommonClass> GetAllPromotionScoreCollectionNotFound();
    }

    public class EmployeePromotionRepository : RepositoryBaseCodeFirst<EmployeePromotion>, IEmployeePromotionRepository
    {
        public EmployeePromotionRepository(IDatabaseFactoryCodeFirst databaseFactory) : base(databaseFactory)
        {

        }

        public EmployeePromotion GetEmployeePromotionByDateRange(long employeeId,DateTime startDate, DateTime endDate)
        {
            var single = new EmployeePromotion();
            try
            {
                single = DataContext.EmployeePromotion.Where(x => x.IsActive && !x.IsReviewed
                                    && x.EmployeeId == employeeId
                                    && (
                                        DbFunctions.TruncateTime(x.PromotionDate) >= DbFunctions.TruncateTime(startDate)
                                        && DbFunctions.TruncateTime(x.PromotionDate) <= DbFunctions.TruncateTime(endDate)
                                    )
                                )
                            .FirstOrDefault();
                return single;
            }
            catch (Exception ex)
            {
                return new EmployeePromotion();
            }           
        }

        public BaseResponse ValidatePromotion(long employeeId ,int promotionId, DateTime promotionDate)
        {
            if(promotionId<=0)
                return new BaseResponse { IsSuccess = true };

            var lastPromotion = DataContext.EmployeePromotion
                        .Where(f => 
                            f.EmployeeId == employeeId &&
                            f.IsActive &&
                            f.NextReviewDate > promotionDate)
                        .OrderByDescending(o => o.NextReviewDate).FirstOrDefault();

            if (lastPromotion != null && (((DateTime)lastPromotion.PromotionDate).ToString("dd-MMM-yyyy") == promotionDate.ToString("dd-MMM-yyyy")))
                return new BaseResponse { IsSuccess = false, Message = "PREVIOUS AND NEW PROMOTION DATE Should not be EQUAL" };

            //if (lastPromotion != null && (lastPromotion.NextReviewDate > promotionDate)) 
            //    return new BaseResponse { IsSuccess = false, Message = "NEW PROMOTION DATE Should not Less Than previous NEXT REVIEW DATE" };
            
            return new BaseResponse { IsSuccess = true };
        }

        public void GetDataFromExcelData(string EmployeeCode, string PayrollDesignation, string PromotionType, out long EmployeeId, out int PayrollDesignationId, out int PromotionTypeId)
        {
            EmployeeId = DataContext.Employees.Where(x => x.IsActive && x.EmployeeCode.Trim().ToLower() == EmployeeCode.Trim().ToLower()).Select(x => x.EmployeeId).FirstOrDefault();
            PayrollDesignationId = DataContext.EmployeeDesignations.Where(x => x.IsActive && x.DesignationName.Trim().ToLower() == PayrollDesignation.Trim().ToLower()).Select(x => x.DesignationId).FirstOrDefault();
            PromotionTypeId = DataContext.PromotionType.Where(x => x.IsActive && x.PromotionTypeName.Trim().ToLower() == PromotionType.Trim().ToLower()).Select(x => x.PromotionTypeId).FirstOrDefault();
        }

        public bool IsDuplicate(long EmployeeId, long PromotionTypeId, DateTime PromotionDate)
        {
            return DataContext.EmployeePromotion.Where(x => x.EmployeeId == EmployeeId && x.PromotionTypeId == PromotionTypeId && x.PromotionDate == PromotionDate).Count() > 0;
        }

         
        public EmployeePromotion GetByEmpIdLong(long id)
        {
            var single = new EmployeePromotion();
            try
            {               
                single = DataContext.EmployeePromotion.Where(x => x.IsActive 
                                    && x.PromotionId == id ).FirstOrDefault();
                return single;
            }
            catch (Exception ex)
            {
                return new EmployeePromotion();
            }
        }

        public IEnumerable<PromotionCommonClass> GetAllPromotionScoreCollection()
        {
            var result = (from Ep in DataContext.EmployeePromotion
                          join E in DataContext.Employees on Ep.EmployeeId equals E.EmployeeId
                          where Ep.IsActive && Ep.AssessmentYear != null

                          select new PromotionCommonClass {
                              AssessmentYear = Ep.AssessmentYear,
                              EmployeeId = Ep.EmployeeId,
                              EmpName = E.EmployeeName,
                              PromotionId = Ep.PromotionId,
                              EmpCode = E.EmployeeCode,
                              Score = Ep.Score,
                              IsActive = Ep.IsActive

                          }).OrderByDescending(x => x.AssessmentYear).ToList();

            return result;

        }

        public IEnumerable<PromotionCommonClass> GetAllPromotionScoreCollectionNotFound()
        {
            var result = (from E in DataContext.Employees
                          join Ep in DataContext.EmployeePromotion on E.EmployeeId equals Ep.EmployeeId 
                          join O in DataContext.Offices on E.OfficeId equals O.OfficeId
                          into ScoreNotFound
                          from Score in ScoreNotFound.DefaultIfEmpty()
                         
                          select new PromotionCommonClass
                          {
                              AssessmentYear = Ep.AssessmentYear,
                              EmployeeId = Ep.EmployeeId,
                              EmpName = E.EmployeeName,
                              PromotionId = Ep.PromotionId,
                              EmpCode = E.EmployeeCode,
                              Score = Ep.Score,
                              IsActive = Ep.IsActive

                          }).Distinct().OrderByDescending(x => x.AssessmentYear).ToList();

            return result;

        }


    }



    public class PromotionCommonClass
    {
        public long PromotionId { get; set; }
        public long EmployeeId { get; set; }
        public int DesignationId { get; set; }
        public int PromotionTypeId { get; set; }
        public DateTime? PromotionDate { get; set; }
        public DateTime? NextReviewDate { get; set; }
        public string Remarks { get; set; }
        public bool IsReviewed { get; set; }
        public bool IsActive { get; set; }
        public long? CreateUser { get; set; }
        public DateTime? CreateDate { get; set; }
        public long? UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string PromotionStatus { get; set; }
        public DateTime? PromotionEffectDate { get; set; }
        public int? AssessmentYear { get; set; }
        public int? Score { get; set; }

        public string EmpCode { get; set; }
        public string EmpName { get; set; }


    }


}
