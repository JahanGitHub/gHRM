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

namespace gHRM.Data.Repository
{
    public interface IManualOvertimeConfigurationRepository : IRepository<ManualOvertimeConfiguration>
    {
        bool IsManualConfigSaveValid(ManualOvertimeConfiguration Config, out string Message);
        void DeleteConfiguration(long Id);
        void DisablePreviousConfig(ManualOvertimeConfiguration Config);
    }
    public class ManualOvertimeConfigurationRepository : RepositoryBaseCodeFirst<ManualOvertimeConfiguration>, IManualOvertimeConfigurationRepository
    {
        public ManualOvertimeConfigurationRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }

        public bool IsManualConfigSaveValid(ManualOvertimeConfiguration Config, out string Message)
        {
            Message = "";

            if (IsDuplicate(Config))
            {
                Message = "Duplicate data exists with Effective Start Date: " + Config.EffectiveStartDate.ToString("dd-MMM-yyyy");
                return false;
            }
            if (IsAllowed(Config))
            {
                Message = "Configuration entry is not allowed with Effective Start Date: " + Config.EffectiveStartDate.ToString("dd-MMM-yyyy");
                return false;
            }
            if (null == Config.EmployeeDesignationId && null == Config.EmployeeId)
            {
                Message = "Configuration can be either with Payroll Designation or Employee Code";
                return false;
            }
            return true;
        }

        public void DeleteConfiguration(long Id)
        {
            ManualOvertimeConfiguration Config = DataContext.ManualOvertimeConfigurations.Find(Id);
            Config.IsActive = false;
        }

        public void DisablePreviousConfig(ManualOvertimeConfiguration Config)
        {
            ManualOvertimeConfiguration OldConfig = DataContext.ManualOvertimeConfigurations.Where(x => x.IsActive
                && (
                    (null == Config.EmployeeDesignationId && x.EmployeeId == Config.EmployeeId)
                    || (null != Config.EmployeeDesignationId && x.EmployeeDesignationId == Config.EmployeeDesignationId)
                )
                && x.EffectiveEndDate == null).FirstOrDefault();
            if (null != OldConfig)
            {
                OldConfig.EffectiveEndDate = Config.EffectiveStartDate.AddDays(-1);
            }
        }

        private bool IsDuplicate(ManualOvertimeConfiguration Config)
        {
            return DataContext.ManualOvertimeConfigurations.Where(x => x.IsActive
                && (
                    (null == Config.EmployeeDesignationId && x.EmployeeId == Config.EmployeeId)
                    || (null != Config.EmployeeDesignationId && x.EmployeeDesignationId == Config.EmployeeDesignationId)
                )
                && x.EffectiveStartDate == Config.EffectiveStartDate
            ).Count() > 0;
        }

        private bool IsAllowed(ManualOvertimeConfiguration Config)
        {
            return DataContext.ManualOvertimeConfigurations.Where(x => x.IsActive
                && (
                    (null == Config.EmployeeDesignationId && x.EmployeeId == Config.EmployeeId)
                    || (null != Config.EmployeeDesignationId && x.EmployeeDesignationId == Config.EmployeeDesignationId)
                )
                && x.EffectiveStartDate > Config.EffectiveStartDate
            ).Count() > 0;
        }
    }
}

