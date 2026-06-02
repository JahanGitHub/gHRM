using BasicDataAccess;
using gHRM.Core.Common;
using gHRM.Core.Filters.Offices;
using gHRM.Core.Utilities;
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.DBDetailModels;
using gHRM.Data.Repository;
using gHRM.Data.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Service
{
    public interface IGratuityConfigService : IServiceBase<GratuityGlobalConfig>
    {
        bool AddGConfig(GratuityGlobalConfig Config, long LoggedInEmployeeId, out string Message);
        bool GenerateGratuity(int OfficeId, int FromYear, int FromMonth, long CreateUser, DateTime ProcessDate, out string Message);
        bool GenerateGratuity2(int OfficeTypeId, int OfficeId, int FromYear, int FromMonth, long CreateUser, DateTime ProcessDate, out string Message);
        DataSet GetDataWithParameter<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class;
        bool SendGeneratedGratuityForApproval(long OfficeId, int FromYear, int FromMonth, out string Message);
        bool SendGeneratedGratuityForApproval2(long OfficeId, long OfficeTypeId, int FromYear, int FromMonth, out string Message);
        bool ApproveGratuitySendForApproval(int FromYear, int FromMonth, DateTime ApproveDate, long LoggedInEmployeeId, out string Message);

        bool RejectGratuitySendForApproval(int FromYear, int FromMonth, out string Message);
        bool DeleteConfig(int Id, out string Message);
        DateTime? GratuityGeneratedLastDate();
    }
    public class GratuityConfigService : IGratuityConfigService
    {
        private readonly IGratuityConfigRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public GratuityConfigService(IGratuityConfigRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        public bool AddGConfig(GratuityGlobalConfig Config, long LoggedInEmployeeId, out string Message)
        {
            return repository.AddGConfig(Config, LoggedInEmployeeId, out Message);
        }

        public bool GenerateGratuity(int OfficeId, int FromYear, int FromMonth, long CreateUser, DateTime ProcessDate, out string Message)
        {
            Message = "";
            try
            {
                if (!repository.IsGenerateAllowed(OfficeId, FromYear, FromMonth))
                {
                    Message = "Generate is not allowed. Gratuity is approved or sent for approval.";
                    return false;
                }
                using (var db = new gHRMDBContext())
                {
                    var param = new object[] { OfficeId, FromYear, FromMonth, ProcessDate, CreateUser };
                    var sqlCommand = @"gr.SP_GratuityProcess {0}, {1}, {2}, {3}, {4}";
                    db.Database.ExecuteSqlCommand(sqlCommand, param);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                return false;
            }
        }

        public bool GenerateGratuity2(int OfficeTypeId,  int OfficeId, int FromYear, int FromMonth, long CreateUser, DateTime ProcessDate, out string Message)
        {
            Message = "";
            try
            {
                if (!repository.IsGenerateAllowed2(OfficeTypeId, OfficeId, FromYear, FromMonth))
                {
                    Message = "Generate is not allowed. Gratuity is approved or sent for approval.";
                    return false;
                }
                using (var db = new gHRMDBContext())
                {
                    var param = new object[] {OfficeTypeId, OfficeId, FromYear, FromMonth, ProcessDate, CreateUser };
                    var sqlCommand = @"gr.SP_GratuityProcess2 {0}, {1}, {2}, {3}, {4}, {5}";
                    db.Database.ExecuteSqlCommand(sqlCommand, param);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                return false;
            }
        }

        public DataSet GetDataWithParameter<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class
        {
            using (var gbData = new gHRMDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public bool SendGeneratedGratuityForApproval(long OfficeId, int FromYear, int FromMonth, out string Message)
        {
            Message = "";
            try
            {
                using (var db = new gHRMDBContext())
                {
                    var param = new object[] { OfficeId, FromYear, FromMonth };
                    var sqlCommand = @"gr.SP_SendGeneratedGratuityForApproval {0}, {1}, {2}";
                    db.Database.ExecuteSqlCommand(sqlCommand, param);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                return false;
            }
        }

        public bool SendGeneratedGratuityForApproval2(long OfficeId, long OfficeTypeId, int FromYear, int FromMonth, out string Message)
        {
            Message = "";
            try
            {
                using (var db = new gHRMDBContext())
                {
                    var param = new object[] { OfficeTypeId, FromYear, FromMonth };
                    var sqlCommand = @"gr.SP_SendGeneratedGratuityForApproval2 {0}, {1}, {2}";
                    db.Database.ExecuteSqlCommand(sqlCommand, param);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                return false;
            }
        }


        public bool ApproveGratuitySendForApproval(int FromYear, int FromMonth, DateTime ApproveDate, long LoggedInEmployeeId, out string Message)
        {
            Message = "";
            try
            {
                using (var db = new gHRMDBContext())
                {
                    var param = new object[] { FromYear, FromMonth, ApproveDate, LoggedInEmployeeId };
                    var sqlCommand = @"gr.SP_ApproveGratuitySendForApproval {0}, {1}, {2}, {3}";
                    db.Database.ExecuteSqlCommand(sqlCommand, param);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                return false;
            }
        }

        public bool RejectGratuitySendForApproval(int FromYear, int FromMonth, out string Message)
        {
            Message = "";
            try
            {
                using (var db = new gHRMDBContext())
                {
                    var param = new object[] { FromYear, FromMonth };
                    var sqlCommand = @"gr.SP_RejectGratuitySendForApproval {0}, {1}";
                    db.Database.ExecuteSqlCommand(sqlCommand, param);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                return false;
            }
        }

        public bool DeleteConfig(int Id, out string Message)
        {
            return repository.DeleteConfig(Id, out Message);
        }

        public DateTime? GratuityGeneratedLastDate()
        {
            return repository.GratuityGeneratedLastDate();
        }

        #region Implementation for IServiceBase
        public IEnumerable<GratuityGlobalConfig> GetAll()
        {
            var entities = repository.GetMany(g => g.IsActive == true).OrderBy(c => c.GratuityGlobalConfigId);
            return entities;
        }

        public GratuityGlobalConfig GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public GratuityGlobalConfig Create(GratuityGlobalConfig objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(GratuityGlobalConfig objectToUpdate)
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
            var obj = repository.GetById(id);
            if (obj != null)
            {
                obj.IsActive = false;
                repository.Update(obj);
                Save();
                return true;
            }
            return false;
        }

        public bool IsContinued(long id)
        {
            var obj = repository.GetById(id);
            if (obj != null)
            {
                var isActive = obj.IsActive;
                if (isActive == true)
                {
                    return false;
                }
            }
            return true;
        }

        public GratuityGlobalConfig Get(Expression<Func<GratuityGlobalConfig, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }

        public IEnumerable<GratuityGlobalConfig> GetMany(Expression<Func<GratuityGlobalConfig, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }

        public virtual async Task<IEnumerable<GratuityGlobalConfig>> GetAllAsync()
        {
            return await repository.GetAllAsync();
        }

        public virtual async Task<IEnumerable<GratuityGlobalConfig>> GetManyAsync(Expression<Func<GratuityGlobalConfig, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }

        public virtual async Task<GratuityGlobalConfig> GetAsync(Expression<Func<GratuityGlobalConfig, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion
    }
}
