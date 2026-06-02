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
using gHRM.Data.Utility.Gratuity;

namespace gHRM.Data.Repository
{
    public interface IGratuityConfigRepository : IRepository<GratuityGlobalConfig>
    {
        bool AddGConfig(GratuityGlobalConfig Config, long LoggedInEmployeeId, out string Message);
        bool DeleteConfig(int Id, out string Message);
        bool IsGenerateAllowed(int OfficeId, int FromYear, int FromMonth);
        bool IsGenerateAllowed2( int OfficeTypeId, int OfficeId, int FromYear, int FromMonth);
        DateTime? GratuityGeneratedLastDate();
    }
    public class GratuityConfigRepository : RepositoryBaseCodeFirst<GratuityGlobalConfig>, IGratuityConfigRepository
    {
        public GratuityConfigRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }

        public bool AddGConfig(GratuityGlobalConfig Config, long LoggedInEmployeeId, out string Message)
        {
            Message = "";
            if (!IsValid(Config, out Message)) return false;
            Config.IsActive = true;
            Config.CreateUser = LoggedInEmployeeId;
            Config.CreateDate = DateTime.Now;
            UpdatePrevConfigEffectiveEndDate(Config);
            DataContext.GratuityGlobalConfigs.Add(Config);
            DataContext.SaveChanges();
            return true;
        }

        private void UpdatePrevConfigEffectiveEndDate(GratuityGlobalConfig Config)
        {
            GratuityGlobalConfig PrevConfig = DataContext.GratuityGlobalConfigs.Where(x => x.IsActive
                && x.EmployeeStatusId == Config.EmployeeStatusId
                && x.ServiceAgeFrom == Config.ServiceAgeFrom
                && x.ServiceAgeTo == Config.ServiceAgeTo
                && x.EffectiveEndDate == null).FirstOrDefault();
            if (PrevConfig != null) PrevConfig.EffectiveEndDate = Config.EffectiveStartDate.AddDays(-1);
        }

        public bool IsValid(GratuityGlobalConfig Config, out string Message)
        {
            Message = "";
            GratuityHelper _Helper = new GratuityHelper();
            List<GratuityGlobalConfig> ConfigList = DataContext.GratuityGlobalConfigs.Where(x => 
                x.IsActive && x.EmployeeStatusId == Config.EmployeeStatusId).ToList();
            return _Helper.IsValid(Config, ConfigList, out Message);
        }

        public bool DeleteConfig(int Id, out string Message)
        {
            Message = "";
            bool IsDeleteAllowed = DataContext.EmployeeGratuities.Where(x => x.GratuityGlobalConfigId == Id && x.IsActive
                && (x.IsApproved || (x.IsSendForApproval && !x.IsApproved && !x.IsRejected))
            ).Count() == 0;
            if (IsDeleteAllowed)
            {
                GratuityGlobalConfig Config = DataContext.GratuityGlobalConfigs.Find(Id);
                Config.IsActive = false;
                DataContext.SaveChanges();
                return true;
            }
            Message = "Delete is not allowed. Gratuity is approved or sent for approval with this configuration.";
            return false;
        }

        public bool IsGenerateAllowed(int OfficeId, int FromYear, int FromMonth)
        {
            return (from G in DataContext.EmployeeGratuities
                    join E in DataContext.Employees on G.EmployeeId equals E.EmployeeId
                    where E.OfficeId == OfficeId
                    && (G.SalaryDate.Year > FromYear || (G.SalaryDate.Year == FromYear && G.SalaryDate.Month >= FromMonth))
                    && G.IsActive
                    && (G.IsApproved || (G.IsSendForApproval && !G.IsApproved && !G.IsRejected))
                    select G.EmployeeGratuityId).Count() == 0;
        }

        public bool IsGenerateAllowed2(int OfficeTypeId, int OfficeId, int FromYear, int FromMonth)
        {
            return (from G in DataContext.EmployeeGratuities
                    join E in DataContext.Employees on G.EmployeeId equals E.EmployeeId
                    join O in DataContext.Offices on E.OfficeId equals O.OfficeId
                    join T in DataContext.OfficeTypes on O.OfficeTypeId equals T.OfficeTypeId
                    where T.OfficeTypeId == OfficeTypeId // E.OfficeId == OfficeId
                    && G.SalaryDate.Year == FromYear 
                    && G.IsActive
                    && (G.IsApproved || (G.IsSendForApproval && !G.IsApproved && !G.IsRejected))
                    select G.EmployeeGratuityId).Count() == 0;
        }


        public DateTime? GratuityGeneratedLastDate()
        {
            return DataContext.EmployeeGratuities.Where(x => x.IsActive && x.IsSendForApproval && x.IsApproved && !x.IsRejected)
                .Select(x => (DateTime?) x.SalaryDate).FirstOrDefault();
        }
    }
}
