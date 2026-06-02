using gHRM.Core.Utilities.Constants;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.CodeFirstMigration.PF;
using System;
using System.Data.Entity.Infrastructure;
using System.Globalization;
using System.Linq;

namespace gHRM.Data.Repository.PF
{
    public interface IProcessLogRepository : IRepository<ProcessLog>
    {
        ProcessLog GetLastProcessLog();
        bool IsDayOpen();
        ProcessLog GetDayStatus();
        ProcessLog GetCustomDayStatus();
    }
    public class ProcessLogRepository : RepositoryBaseCodeFirst<ProcessLog>, IProcessLogRepository
    {
        public static ProcessLog ProcessLog { get; set; }
        public ProcessLogRepository(IDatabaseFactoryCodeFirst databaseFactory): base(databaseFactory)
        {
        }
        public ProcessLog GetLastProcessLog()
        {
            ProcessLog results = null;
            results = DataContext.ProcessLog.Where(x => x.IsDeleted == false).OrderByDescending(x => x.StartDate).Take(1).FirstOrDefault();
            return results;
        }

        public bool IsDayOpen()
        {
            bool isOpen = false;
            ProcessLog result = null;

            result = DataContext.ProcessLog.Where(x => x.IsDeleted == false).OrderByDescending(x => x.StartDate).Take(1).FirstOrDefault();

            if (result != null)
               isOpen = (result.IsOpen == true) ? true : false;

            return isOpen;
        }

        public ProcessLog GetDayStatus()
        {
            var context = ((IObjectContextAdapter)DataContext).ObjectContext;
            ProcessLog model = new ProcessLog();

            //get process log from [gcpf.ProcessLog]
            var objProcessLog = DataContext.ProcessLog.Where(x => x.IsDeleted == false).OrderByDescending(x => x.StartDate).Take(1).FirstOrDefault();
            if (objProcessLog != null)    
                context.Refresh(System.Data.Entity.Core.Objects.RefreshMode.StoreWins, objProcessLog);

            //get organization wise pf setup from [gcpf.OrganizationSetup]
            var objOrgSetup = DataContext.OrganizationSetup.FirstOrDefault(x => x.IsDeleted == false && x.IsActive == true);
            if (objOrgSetup != null)
                context.Refresh(System.Data.Entity.Core.Objects.RefreshMode.StoreWins, objOrgSetup);
            
            if (objProcessLog == null)
            {
                model.DayStatus = PFDayStatusConstants.NotStarted;
                model.TransactionDateString = string.Empty;
                model.IsOpen = false; //as close
            }

            if (objProcessLog != null && objOrgSetup != null)
            {      
                //if process date is out of organization wise pf setup then day initialization not started.        
                if (!(objProcessLog.StartDate >= objOrgSetup.YearStartDate && objProcessLog.StartDate <= objOrgSetup.YearEndDate))
                {
                    model.DayStatus = PFDayStatusConstants.NotStarted;
                    model.TransactionDateString = string.Empty;
                    model.IsOpen = false; //as close
                }

                //if process log is not open and process date is in organization wise pf date range then day status is closed.
                if (!objProcessLog.IsOpen && objProcessLog.StartDate >= objOrgSetup.YearStartDate && objProcessLog.StartDate <= objOrgSetup.YearEndDate)
                {
                    model.TransactionDate = objProcessLog.StartDate;  //Clossed Date
                    model.TransactionDateString = Convert.ToDateTime(objProcessLog.StartDate).ToString("dd-MMM-yyyy",CultureInfo.InvariantCulture);
                    model.DayStatus = PFDayStatusConstants.Close;
                    model.IsOpen = objProcessLog.IsOpen; ////as close
                }

                //if process log is open and process date is in organization wise pf date range then day status is open
                if (objProcessLog.IsOpen && objProcessLog.StartDate >= objOrgSetup.YearStartDate && objProcessLog.StartDate <= objOrgSetup.YearEndDate)
                {
                    model.TransactionDate = objProcessLog.StartDate;  //Open Date
                    model.TransactionDateString = Convert.ToDateTime(objProcessLog.StartDate).ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture);
                    model.DayStatus = PFDayStatusConstants.Open;
                    model.IsOpen = objProcessLog.IsOpen;  //open
                }
            }
            return model;
        }

        public ProcessLog GetCustomDayStatus()
        {
            //Same:yyy
            ProcessLog objProcessLog = new ProcessLog();
            var processLog = GetDayStatus();
            if (processLog != null)
            {
                objProcessLog.IsOpen = processLog.IsOpen;
                objProcessLog.SystemDate = DateTime.Now.ToString("dd-MMM-yyyy",CultureInfo.InvariantCulture);
                if (processLog.IsOpen)
                {
                    objProcessLog.DayStatus = "Open";
                    objProcessLog.IsOpen = processLog.IsOpen;
                    objProcessLog.TransactionDateString = processLog.TransactionDate != null ? processLog.TransactionDate.ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture) : string.Empty;
                }
                else
                {
                    objProcessLog.DayStatus = "Close";
                    objProcessLog.IsOpen = processLog.IsOpen;
                    objProcessLog.TransactionDateString = string.Empty;
                }
            }
            return objProcessLog;
        }
    }
}
