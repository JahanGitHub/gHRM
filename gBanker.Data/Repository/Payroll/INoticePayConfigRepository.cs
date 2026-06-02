using System.Text;
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.DBDetailModels;
using System.Collections.Generic;
using System.Linq;
using gHRM.Data.Utility;
using System;
using System.Threading.Tasks;
using gHRM.Core.Filters.Offices;
using System.Data.Entity;
using gHRM.Data.CodeFirstMigration.Payroll;

namespace gHRM.Data.Repository
{
    public interface INoticePayConfigRepository : IRepository<NoticePayConfig>
    {
        bool DeleteNoticePayConfig(long Id, out string Message);
        bool AddNPConfig(NoticePayConfig Config, long LoggedInEmployeeId, out string Message);
        bool IsGenerateAllowed(int OfficeTypeId, int OfficeId, int FromYear, int FromMonth);
    }
    public class NoticePayConfigRepository : RepositoryBaseCodeFirst<NoticePayConfig>, INoticePayConfigRepository
    {
        public NoticePayConfigRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }

        public bool DeleteNoticePayConfig(long Id, out string Message)
        {
            Message = "";
            bool IsDeleteAllowed = DataContext.EmployeeNoticePays.Where(x => x.NoticePayConfigId == Id && x.IsActive
                && (x.IsApproved || (x.IsSendForApproval && !x.IsApproved && !x.IsRejected))
            ).Count() == 0;
            if (IsDeleteAllowed)
            {
                NoticePayConfig _NoticePayConfig = DataContext.NoticePayConfigs.Find(Id);
                _NoticePayConfig.IsActive = false;
                DataContext.Database.ExecuteSqlCommand("UPDATE prl.EmployeeNoticePay SET IsActive = 0 WHERE NoticePayConfigId = {0}", new object[] { Id });
                DataContext.SaveChanges();
                return true;
            }
            Message = "Delete is not allowed. Notice Pay is approved or sent for approval with this configuration.";
            return false;
        }

        public bool AddNPConfig(NoticePayConfig Config, long LoggedInEmployeeId, out string Message)
        {
            Message = "";
            if (!IsValid(Config, out Message)) return false;
            Config.IsActive = true;
            Config.CreateUser = LoggedInEmployeeId;
            Config.CreateDate = DateTime.Now;
            UpdatePrevConfigEffectiveEndDate(Config);
            DataContext.NoticePayConfigs.Add(Config);
            DataContext.SaveChanges();
            return true;
        }

        private void UpdatePrevConfigEffectiveEndDate(NoticePayConfig Config)
        {
            NoticePayConfig PrevConfig = DataContext.NoticePayConfigs.Where(x => x.IsActive && x.EffectiveEndDate == null).FirstOrDefault();
            if (PrevConfig != null) PrevConfig.EffectiveEndDate = Config.EffectiveStartDate.AddDays(-1);
        }

        public bool IsValid(NoticePayConfig Config, out string Message)
        {
            Message = "";
            if (DuplicateConfigExists(Config))
            {
                Message = "Notice Pay Configuration already exists";
                return false;
            }
            return true;
        }

        public bool DuplicateConfigExists(NoticePayConfig Config)
        {
            return DataContext.NoticePayConfigs.Where(x => x.IsActive
                && (
                    (x.EffectiveEndDate != null && x.EffectiveEndDate >= Config.EffectiveStartDate)
                    || (x.EffectiveEndDate == null && x.EffectiveStartDate >= Config.EffectiveStartDate)
                )).Count() > 0;
        }

        public bool IsGenerateAllowed(int OfficeTypeId, int OfficeId, int FromYear, int FromMonth)
        {
            return (from G in DataContext.EmployeeNoticePays
                    join E in DataContext.Employees on G.EmployeeId equals E.EmployeeId
                    join O in DataContext.Offices on E.OfficeId equals O.OfficeId
                    where (OfficeTypeId == 0 || O.OfficeTypeId == OfficeTypeId)
                    && (OfficeId == 0 || E.OfficeId == OfficeId)
                    && (G.ResignDate.Year > FromYear || (G.ResignDate.Year == FromYear && G.ResignDate.Month >= FromMonth))
                    && G.IsActive
                    && (G.IsApproved || (G.IsSendForApproval && !G.IsApproved && !G.IsRejected))
                    select G.Id).Count() == 0;
        }
    }
}
