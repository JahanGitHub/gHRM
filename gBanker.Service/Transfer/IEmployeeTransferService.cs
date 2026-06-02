
using gHRM.Core.Common;
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.DBDetailModels.Promotions;
using gHRM.Data.Repository;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Service
{
    public interface IEmployeeTransferService : IServiceBase<EmployeeTransfer>
    {
        EmployeeTransfer GetLastTranserByEmployeeId(long employeeId);
        EmployeeTransfer UpdateEmployeeTransferReleaseDate(EmployeeTransfer updateEmployeeTransfer);

        bool BulkPromotionBackLogAdd(List<TransferBackLogImportModel> promotionBackLogs);
        void GetDataFromExcelData(string EmployeeCode, string OfficeName, string DepartmentName, string SectionName, string ResponsibilityName, out long EmployeeId, out int OfficeId, out int DepartmentId, out int SectionId, out int ResponsibilityId);
    }
    public class EmployeeTransferService : IEmployeeTransferService
    {
        private readonly IEmployeeTransferRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public EmployeeTransferService(IEmployeeTransferRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<EmployeeTransfer> GetAll()
        {
            var entities = repository.GetAll().OrderBy(c => c.Id);
            return entities;
        }

        public EmployeeTransfer GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public EmployeeTransfer GetLastTranserByEmployeeId(long employeeId)
        {
            var single = new EmployeeTransfer { };

            using (var db = new gHRMDBContext())
            {
                single = db.EmployeeTransfer.Where(f => f.IsActive && f.IsApproved && f.EmployeeId == employeeId)
                    .OrderByDescending(d => d.JoiningDate)
                    .FirstOrDefault();
            }

            return single;
        }

        public EmployeeTransfer UpdateEmployeeTransferReleaseDate(EmployeeTransfer updateEmployeeTransfer)
        {
            using (var db = new gHRMDBContext())
            {
                var update = db.EmployeeTransfer.FirstOrDefault(f => f.Id== updateEmployeeTransfer.Id);

                if (update != null)
                {
                    update.ReleaseDate = updateEmployeeTransfer.ReleaseDate;
                    update.UpdateDate = updateEmployeeTransfer.UpdateDate;
                    update.UpdateUser = updateEmployeeTransfer.UpdateUser;

                    db.SaveChanges();
                }                
            }

            return updateEmployeeTransfer;
        }

        public EmployeeTransfer Create(EmployeeTransfer objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(EmployeeTransfer objectToUpdate)
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

        public EmployeeTransfer Get(Expression<Func<EmployeeTransfer, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<EmployeeTransfer> GetMany(Expression<Func<EmployeeTransfer, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }

        #region Asyc
        public virtual async Task<IEnumerable<EmployeeTransfer>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }


        public virtual async Task<IEnumerable<EmployeeTransfer>> GetManyAsync(Expression<Func<EmployeeTransfer, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }



        public virtual async Task<EmployeeTransfer> GetAsync(Expression<Func<EmployeeTransfer, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion

        /// <summary>
        /// add bulk employee
        /// </summary>
        /// <param name="promotionBackLogs"></param>
        /// <returns></returns>
        public bool BulkPromotionBackLogAdd(List<TransferBackLogImportModel> promotionBackLogs)
        {
            var isAddedSuccess = false;

            if (promotionBackLogs == null || !promotionBackLogs.Any())
                return isAddedSuccess;

            var dt = new DataTable();

            //Add Columns
            dt.Columns.Add("SerialId", typeof(int));
            dt.Columns.Add("EmployeeCode", typeof(string));
            dt.Columns.Add("OfficeZone", typeof(string));
            dt.Columns.Add("OfficeArea", typeof(string));
            dt.Columns.Add("OfficeDesignation", typeof(string));
            dt.Columns.Add("OrderNo", typeof(string));
            dt.Columns.Add("OrderDate", typeof(string));
            dt.Columns.Add("ReleaseDate", typeof(string));
            dt.Columns.Add("JoiningDate", typeof(string));
            dt.Columns.Add("CreateUser", typeof(Int64));

            //Add rows
            int count = 0;
            int serialId = 1;
            foreach (var model in promotionBackLogs)
            {
                if (string.IsNullOrWhiteSpace(model.OfficeArea) || string.IsNullOrWhiteSpace(model.OfficeZone))
                    continue;

                try
                {
                    dt.Rows.Add(
                        serialId,
                        model.EmployeeCode,
                        model.OfficeZone,
                        model.OfficeArea,
                        model.OfficeDesignation,
                        model.OrderNo,
                        model.OrderDate.ToString("yyyy-MM-dd HH:mm:ss"),//.ToString("dd-MMM-yyyy",CultureInfo.InvariantCulture),
                        model.ReleaseDate.ToString("yyyy-MM-dd HH:mm:ss"),//.ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture),
                        model.JoiningDate.ToString("yyyy-MM-dd HH:mm:ss"),//.ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture),
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

        public void GetDataFromExcelData(string EmployeeCode, string OfficeName, string DepartmentName, string SectionName, string ResponsibilityName, out long EmployeeId, out int OfficeId, out int DepartmentId, out int SectionId, out int ResponsibilityId)
        {
            repository.GetDataFromExcelData(EmployeeCode, OfficeName, DepartmentName, SectionName, ResponsibilityName, out EmployeeId, out OfficeId, out DepartmentId, out SectionId, out ResponsibilityId);
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

                    var cmd = new SqlCommand("[trns].[EmployeeTransfer_InsertBulkForImportTransferBackLogs]", conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };

                    var dtparam = cmd.Parameters.AddWithValue("@TempBulkTransferBackLogImport", dt);
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
                    foreach (var ve in eve.ValidationErrors)
                    {
                        err2 = "Property Name: " + ve.PropertyName + ", Message:" + ve.ErrorMessage;
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

        #endregion

    }
}
