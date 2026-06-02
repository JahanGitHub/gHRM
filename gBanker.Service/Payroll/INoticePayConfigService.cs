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
using gHRM.Data.CodeFirstMigration.Payroll;

namespace gHRM.Service
{
    public interface INoticePayConfigService : IServiceBase<NoticePayConfig>
    {
        bool DeleteNoticePayConfig(long Id, out string Message);
        bool AddNPConfig(NoticePayConfig Config, long LoggedInEmployeeId, out string Message);
        bool GenerateNoticePay(int OfficeTypeId, int OfficeId, int FromYear, int FromMonth, long CreateUser, DateTime ProcessDate, out string Message);
        bool SendGeneratedNoticePayForApproval(long OfficeId, int FromYear, int FromMonth, out string Message);
        bool ApproveNoticePaySendForApproval(int FromYear, int FromMonth, DateTime ApproveDate, long LoggedInEmployeeId, out string Message);
        bool RejectNoticePaySendForApproval(int FromYear, int FromMonth, out string Message);
    }
    public class NoticePayConfigService : INoticePayConfigService
    {
        private readonly INoticePayConfigRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public NoticePayConfigService(INoticePayConfigRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        public bool DeleteNoticePayConfig(long Id, out string Message)
        {
            return repository.DeleteNoticePayConfig(Id, out Message);
        }

        public bool AddNPConfig(NoticePayConfig Config, long LoggedInEmployeeId, out string Message)
        {
            return repository.AddNPConfig(Config, LoggedInEmployeeId, out Message);
        }

        public bool GenerateNoticePay(int OfficeTypeId, int OfficeId, int FromYear, int FromMonth, long CreateUser, DateTime ProcessDate, out string Message)
        {
            Message = "";
            try
            {
                if (!repository.IsGenerateAllowed(OfficeTypeId, OfficeId, FromYear, FromMonth))
                {
                    Message = "Generate is not allowed. Notice Pay is approved or sent for approval.";
                    return false;
                }
                using (var db = new gHRMDBContext())
                {
                    var param = new object[] { OfficeTypeId, OfficeId, FromYear, FromMonth, ProcessDate, CreateUser };
                    var sqlCommand = @"prl.SP_NoticePayProcess {0}, {1}, {2}, {3}, {4}, {5}";
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

        public bool SendGeneratedNoticePayForApproval(long OfficeId, int FromYear, int FromMonth, out string Message)
        {
            Message = "";
            try
            {
                using (var db = new gHRMDBContext())
                {
                    var param = new object[] { OfficeId, FromYear, FromMonth };
                    var sqlCommand = @"prl.SP_SendGeneratedNoticePayForApproval {0}, {1}, {2}";
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

        public bool ApproveNoticePaySendForApproval(int FromYear, int FromMonth, DateTime ApproveDate, long LoggedInEmployeeId, out string Message)
        {
            Message = "";
            try
            {
                using (var db = new gHRMDBContext())
                {
                    var param = new object[] { FromYear, FromMonth, ApproveDate, LoggedInEmployeeId };
                    var sqlCommand = @"prl.SP_ApproveNoticePaySendForApproval {0}, {1}, {2}, {3}";
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

        public bool RejectNoticePaySendForApproval(int FromYear, int FromMonth, out string Message)
        {
            Message = "";
            try
            {
                using (var db = new gHRMDBContext())
                {
                    var param = new object[] { FromYear, FromMonth };
                    var sqlCommand = @"prl.SP_RejectNoticePaySendForApproval {0}, {1}";
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

        #region Implementation for IServiceBase
        public IEnumerable<NoticePayConfig> GetAll()
        {
            var entities = repository.GetMany(g => g.IsActive == true).OrderBy(c => c.Id);
            return entities;
        }

        public NoticePayConfig GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public NoticePayConfig Create(NoticePayConfig objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(NoticePayConfig objectToUpdate)
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

        public NoticePayConfig Get(Expression<Func<NoticePayConfig, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }

        public IEnumerable<NoticePayConfig> GetMany(Expression<Func<NoticePayConfig, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }

        public virtual async Task<IEnumerable<NoticePayConfig>> GetAllAsync()
        {
            return await repository.GetAllAsync();
        }

        public virtual async Task<IEnumerable<NoticePayConfig>> GetManyAsync(Expression<Func<NoticePayConfig, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }

        public virtual async Task<NoticePayConfig> GetAsync(Expression<Func<NoticePayConfig, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion
    }
}
