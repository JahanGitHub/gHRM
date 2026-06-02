using gHRM.Core.Common;
using gHRM.Core.Utilities;
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.EmployeePromotion;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.DBDetailModels.Promotions;
using gHRM.Data.Repository;
using gHRM.Data.Repository.Promotion;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Service
{
    public interface IEmployeePromotionService : IServiceBase<EmployeePromotion>
    {
        EmployeePromotion GetPromotionInfo(long employeeId);
        EmployeePromotion GetLastPromotionInfo(long employeeId);
        BaseResponse ValidatePromotion(long employeeId, int promotionId, DateTime promotionDate);
        EmployeePromotion GetEmployeePromotionByDateRange(long employeeId, DateTime startDate, DateTime endDate);
        bool BulkPromotionBackLogAdd(List<PromotionBackLogImportModel> promotionBackLogs);
        void GetDataFromExcelData(string EmployeeCode, string PayrollDesignation, string PromotionType, out long EmployeeId, out int PayrollDesignationId, out int PromotionTypeId);
        bool IsDuplicate(long EmployeeId, long PromotionTypeId, DateTime PromotionDate);

        EmployeePromotion GetByEmpIdLong(long id);
        IEnumerable<PromotionCommonClass> GetAllPromotionScoreCollection();
        IEnumerable<PromotionCommonClass> GetAllPromotionScoreCollectionNotFound();
    }
    public class EmployeePromotionService : IEmployeePromotionService
    {
        private readonly IEmployeePromotionRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public EmployeePromotionService(IEmployeePromotionRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<EmployeePromotion> GetAll()
        {
            var entities = repository.GetAll().OrderBy(c => c.PromotionId);
            return entities;
        }

        public EmployeePromotion GetById(int id)
        {
            try
            {
                var entity = repository.GetById(id);
                return entity;
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        public EmployeePromotion GetEmployeePromotionByDateRange(long employeeId, DateTime startDate, DateTime endDate)
        {
            var single = new EmployeePromotion();
            try
            {
                single = repository.GetEmployeePromotionByDateRange(employeeId, startDate, endDate);
                return single;
            }
            catch (Exception ex)
            {
                return new EmployeePromotion();
            }
        }

        public EmployeePromotion GetPromotionInfo(long employeeId)
        {
            var single = new EmployeePromotion();
            using (var db = new gHRMDBContext())
            {
                single = db.EmployeePromotion.OrderByDescending(o => o.PromotionDate).Where(x => x.EmployeeId == employeeId
                                                               && x.IsActive
                                                               && !x.IsReviewed)
                                           .OrderBy(x => x.NextReviewDate)
                                           .FirstOrDefault();
            }
            return single;
        }

        public BaseResponse ValidatePromotion(long employeeId, int promotionId, DateTime promotionDate)
        {
            return repository.ValidatePromotion(employeeId, promotionId, promotionDate);
        }

        public EmployeePromotion GetLastPromotionInfo(long employeeId)
        {
            var single = new EmployeePromotion();
            using (var db = new gHRMDBContext())
            {
                single = db.EmployeePromotion.OrderByDescending(o => o.PromotionDate).Where(x => x.EmployeeId == employeeId
                                                               && x.IsActive).FirstOrDefault();
            }
            return single;
        }

        public EmployeePromotion Create(EmployeePromotion objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(EmployeePromotion objectToUpdate)
        {
            repository.Update(objectToUpdate);
            Save();
        }

        public void Delete(int id)
        {
            var entity = repository.GetById(id);
            repository.Delete(entity);
            Save();
        }

        public void Save()
        {
            unitOfWork.Commit();
        }


        public bool Inactivate(long id, DateTime? inactiveDate)
        {
            throw new NotImplementedException(); ;
        }


        public bool IsContinued(long id)
        {
            throw new NotImplementedException();
        }

        public EmployeePromotion Get(Expression<Func<EmployeePromotion, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<EmployeePromotion> GetMany(Expression<Func<EmployeePromotion, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }

        #region Asyc
        public virtual async Task<IEnumerable<EmployeePromotion>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }


        public virtual async Task<IEnumerable<EmployeePromotion>> GetManyAsync(Expression<Func<EmployeePromotion, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }



        public virtual async Task<EmployeePromotion> GetAsync(Expression<Func<EmployeePromotion, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion


        /// <summary>
        /// add bulk employee
        /// </summary>
        /// <param name="promotionBackLogs"></param>
        /// <returns></returns>
        public bool BulkPromotionBackLogAdd(List<PromotionBackLogImportModel> promotionBackLogs)
        {
            var isAddedSuccess = false;

            if (promotionBackLogs == null || !promotionBackLogs.Any())
                return isAddedSuccess;

            var dt = new DataTable();

            //Add Columns
            dt.Columns.Add("SerialId", typeof(int));
            dt.Columns.Add("EmployeeId", typeof(Int64));
            dt.Columns.Add("DesignationName", typeof(string));
            dt.Columns.Add("PromotionTypeName", typeof(string));
            dt.Columns.Add("PromotionDate", typeof(DateTime));
            dt.Columns.Add("DurationInMonth", typeof(int));
            dt.Columns.Add("NextReviewDate", typeof(DateTime));
            dt.Columns.Add("GrossSalary", typeof(decimal));
            dt.Columns.Add("BasicSalary", typeof(decimal));
            dt.Columns.Add("HouseRent", typeof(decimal));
            dt.Columns.Add("Medical", typeof(decimal));
            dt.Columns.Add("Conveyance", typeof(decimal));
            dt.Columns.Add("Others", typeof(decimal));
            dt.Columns.Add("CreateUser", typeof(Int64));

            //Add rows
            int count = 0;
            int serialId = 1;
            foreach (var model in promotionBackLogs)
            {
                try
                {
                    dt.Rows.Add(
                        serialId,
                        model.EmployeeId,
                        model.PayrollDesignation,
                        model.PromotionType,
                        model.PromotionDate,
                        model.DurationInMonth,
                        model.NextReviewDate,
                        model.GrossSalary,
                        model.BasicSalary,
                        model.HouseRent,
                        model.Medical,
                        model.Conveyance,
                        model.Others,
                        model.CreateUser
                        );

                    count++;
                    serialId++;
                }
                catch (Exception ex)
                {
                    // if error don't continue, fall back
                    var exception = new Exception("Adding data rows: " + ex.Message);
                    return false;
                }

                if (count >= 2000)
                {
                    count = 0;

                    // if error don't continue, fall back
                    isAddedSuccess = AddBulkOfPromotionBackLogs(dt);

                    if (isAddedSuccess == false)
                        return isAddedSuccess;

                    dt.Rows.Clear();
                }
            }

            if (count > 0)
            {
                count = 0;

                // if error don't continue, fall back
                isAddedSuccess = AddBulkOfPromotionBackLogs(dt);

                if (isAddedSuccess == false)
                    return isAddedSuccess;

                dt.Rows.Clear();
            }

            return true;
        }

        public void GetDataFromExcelData(string EmployeeCode, string PayrollDesignation, string PromotionType, out long EmployeeId, out int PayrollDesignationId, out int PromotionTypeId)
        {
            repository.GetDataFromExcelData(EmployeeCode, PayrollDesignation, PromotionType, out EmployeeId, out PayrollDesignationId, out PromotionTypeId);
        }

        public bool IsDuplicate(long EmployeeId, long PromotionTypeId, DateTime PromotionDate)
        {
            return repository.IsDuplicate(EmployeeId, PromotionTypeId, PromotionDate);
        }

        #region Private Method

        /// <summary>
        /// Add a bulk of promotion backlog to database
        /// </summary>
        /// <param name="dt"></param>
        private bool AddBulkOfPromotionBackLogs(DataTable dt)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["gHRMDbContext"].ConnectionString;

            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    var cmd = new SqlCommand("[promo].[EmployeePromotion_InsertBulkForImportPromotionBackLogs]", conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };

                    var dtparam = cmd.Parameters.AddWithValue("@TempBulkPromotionBackLogImport", dt);
                    dtparam.SqlDbType = SqlDbType.Structured;
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (DbEntityValidationException e)
            {
                string err = "";
                string err2 = "";
                foreach (var eve in e.EntityValidationErrors)
                {
                    err = eve.Entry.Entity.GetType().Name + eve.Entry.State;
                    //Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                    //    eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        err2 = "Property Name: " + ve.PropertyName + ", Message:" + ve.ErrorMessage;
                        //Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                        //    ve.PropertyName, ve.ErrorMessage);
                    }
                }
                //throw;
            }
            catch (DbUpdateException ex)
            {
                foreach (var entry in ex.Entries)
                {
                    var errorInProperty = entry;
                }

                return false;
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        public EmployeePromotion GetByEmpIdLong(long id)
        {
            try
            {
                var entity = repository.GetByEmpIdLong(id);
                return entity;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public IEnumerable<PromotionCommonClass> GetAllPromotionScoreCollection()
        {
            return repository.GetAllPromotionScoreCollection();
        }
        public IEnumerable<PromotionCommonClass> GetAllPromotionScoreCollectionNotFound()
        {
            return repository.GetAllPromotionScoreCollectionNotFound();
        }


        #endregion

    }
}
