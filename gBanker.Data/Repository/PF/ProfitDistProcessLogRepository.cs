using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.CodeFirstMigration.PF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.Repository.PF
{

    public interface IProfitDistProcessLogRepository : IRepository<ProfitDistProcessLog>
    {
        bool IsValidYearForprofitDist(DateTime yearStartDate, DateTime yearEndDate, out string message);
    }
    public class ProfitDistProcessLogRepository : RepositoryBaseCodeFirst<ProfitDistProcessLog>, IProfitDistProcessLogRepository
    {
        public ProfitDistProcessLogRepository(IDatabaseFactoryCodeFirst databaseFactory) : base(databaseFactory)
        {
        }

        public bool IsValidYearForprofitDist(DateTime yearStartDate, DateTime yearEndDate, out string message)
        {
            bool isValid = false;
            message = string.Empty;

            //Org Setup checking for [gcpf.OrganizationSetup]
            var objOrgSetupExist = DataContext.OrganizationSetup
                    .Any(x => !x.IsDeleted && x.IsActive && x.YearStartDate.Year - 1 == yearStartDate.Year 
                                                         && x.YearEndDate.Year - 1 == x.YearEndDate.Year);
            if (objOrgSetupExist)
                isValid = true;

            //Checking Profit Declaration for [gcpf.ProfitDeclaration]
            var objProfitDeclaration = DataContext.ProfitDeclaration
                            .FirstOrDefault(x => x.YearStartDate.Year == yearStartDate.Year 
                                                                && x.YearEndDate.Year == yearEndDate.Year);
            if (objProfitDeclaration == null)
            {
                isValid = false; message = "Profit has not been calculated";
                return isValid;
            }
            
            //if (!objProfitDeclaration.IsDeclared)
            //{
            //    isValid = false; message = "Profit has not been declared yet";
            //    return isValid;
            //}

            //Checking year closing for [gcpf.YearEndProcessLog]
            var objYearEndProcessLog = DataContext.YearEndProcessLog
                .Where(x => !x.IsDeleted).OrderByDescending(x => x.YearStartDate).Take(1).FirstOrDefault();

            if (objYearEndProcessLog == null)
            {
                isValid = false; message = "Year has not been ended yet";
                return isValid;
            }
           
            if (yearStartDate.Year < objYearEndProcessLog.YearStartDate.Year || yearEndDate.Year < objYearEndProcessLog.YearEndDate.Year)
            {
                isValid = false; message = "Profit already distributed for " + yearStartDate.Year + "-" + yearEndDate.Year;
                return isValid;
            }

            if (yearStartDate.Year > objYearEndProcessLog.YearStartDate.Year || yearEndDate.Year > objYearEndProcessLog.YearEndDate.Year)
            {
                isValid = false; message = "You are trying to skip distribution year";
                return isValid;
            }

            if (yearStartDate.Year == objYearEndProcessLog.YearStartDate.Year && yearEndDate.Year == objYearEndProcessLog.YearEndDate.Year)            
                isValid = true;

            //Checking profit distribution process log for [gcpf.ProfitDistProcessLog]
            var processedYearsExist = DataContext.ProfitDistProcessLog.Any(x => !x.IsDeleted && x.IsProcessed);
            if (!processedYearsExist)
                isValid = true;

            if (processedYearsExist)
            {
                ProfitDistProcessLog objProfitDistProcessLog = null;

                //get process log for [gcpf.ProfitDistProcessLog]
                objProfitDistProcessLog = DataContext.ProfitDistProcessLog
                                                .Where(x => x.IsProcessed && !x.IsDeleted)
                                                    .OrderByDescending(x => x.YearStartDate).Take(1).FirstOrDefault();

                if (yearStartDate.Year < objProfitDistProcessLog.YearStartDate.Year || yearEndDate.Year < objProfitDistProcessLog.YearEndDate.Year)
                {
                    isValid = false;  message = "Profit already distributed for " + yearStartDate.Year + "-" + yearEndDate.Year;
                    return isValid;
                }
                if (yearStartDate.Year > objProfitDistProcessLog.YearStartDate.Year + 1 || yearEndDate.Year > objProfitDistProcessLog.YearEndDate.Year + 1)
                {
                    isValid = false; message = "You are trying to skip distribution year";
                    return isValid;
                }

                if (yearStartDate.Year == objProfitDistProcessLog.YearStartDate.Year && yearEndDate.Year == objProfitDistProcessLog.YearEndDate.Year)
                {
                    isValid = false; message = "Profit distribution of this year already completed";
                    return isValid;
                }

                if (yearStartDate.Year == objProfitDistProcessLog.YearStartDate.Year + 1 && yearEndDate.Year == objProfitDistProcessLog.YearEndDate.Year + 1)                
                    isValid = true;                
            }

            return isValid;
        }
    }
}
