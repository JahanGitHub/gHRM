using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.CodeFirstMigration.PF;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.Repository.PF
{
    public interface IYearEndProcessLogRepository : IRepository<YearEndProcessLog>
    {
        YearEndProcessLog GetLastYearEndProcessLog(out bool opStatus);
        bool IsProcessed(DateTime yearStartDate, DateTime yearEndDate);
        bool IsValidYearForEnding(DateTime yearStartDate, DateTime yearEndDate, out string message);
        bool IsProfitDistributed(DateTime yearStartDate, DateTime yearEndDate, out string message);

    }
    public class YearEndProcessLogRepository : RepositoryBaseCodeFirst<YearEndProcessLog>, IYearEndProcessLogRepository
    {
        public YearEndProcessLogRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {
        }

        public YearEndProcessLog GetLastYearEndProcessLog(out bool opStatus)
        {
            opStatus = true;
            YearEndProcessLog results = null;
            try
            {
                results = DataContext.YearEndProcessLog.Where(x => x.IsDeleted == false).OrderByDescending(x => x.YearStartDate).Take(1).FirstOrDefault();
            }
            catch
            {
                opStatus = false;
            }
            return results;
        }

        public bool IsProcessed(DateTime yearStartDate, DateTime yearEndDate)
        {
            bool isProcessed = false;
            YearEndProcessLog result = null;
            result = DataContext.YearEndProcessLog.Where(x => x.IsDeleted == false && x.YearEndDate.Year == yearEndDate.Year && x.YearEndDate.Year == yearEndDate.Year && x.IsProcessed == true).FirstOrDefault();
            isProcessed = result == null ? false : true;
            return isProcessed;
        }

        public bool IsValidYearForEnding(DateTime yearStartDate, DateTime yearEndDate, out string message)
        {

            bool isValid = false;
            message = string.Empty;
            YearEndProcessLog result = null;

            //Checking Yearr Setup from [gcpf.OrganizationSetup]
            var objOrgSetup = DataContext.OrganizationSetup
                                .FirstOrDefault(x => x.IsDeleted == false && x.IsActive == true 
                                    && DbFunctions.TruncateTime(x.YearStartDate) == DbFunctions.TruncateTime(yearStartDate) && DbFunctions.TruncateTime(x.YearEndDate) == DbFunctions.TruncateTime(x.YearEndDate));

            if (objOrgSetup != null)
            {
                isValid = true;
                message = string.Empty;
            }
            else
            {
                isValid = false;
                message = "Please setup first organization";
                return isValid;
            }

            //Checking Day Status
            var objProcessLog = DataContext.ProcessLog.Where(x => x.IsDeleted == false).OrderByDescending(x => x.StartDate).FirstOrDefault();

            if (objProcessLog == null)
            {
                isValid = false;
                message = "No day is opened";
                return isValid;
            }


            if (objProcessLog != null)
            {
                if (objProcessLog.IsOpen)
                {
                    isValid = false;
                    message = "Day open, please close day";
                    return isValid;
                }

                if (objProcessLog.StartDate != yearEndDate)
                {
                    isValid = false;
                    message = "Transaction date and Closing date is different";
                    return isValid;
                }
            }

            var objProcessedYears = DataContext.YearEndProcessLog.Where(x => x.IsDeleted == false && x.IsProcessed == true);
            if (!objProcessedYears.Any())
            {
                isValid = true;
                message = string.Empty;
            }

            if (objProcessedYears.Any())
            {
                //Aleardy closed
                result = objProcessedYears.OrderByDescending(x => x.YearStartDate).Where(x => yearStartDate.Year <= x.YearStartDate.Year || yearEndDate.Year <= x.YearEndDate.Year).FirstOrDefault();
                if (result != null)
                {
                    isValid = false;
                    message = "Already closed";
                    return isValid;
                }

                result = objProcessedYears.OrderByDescending(x => x.YearStartDate).Where(x => yearStartDate.Year > x.YearStartDate.Year + 1 || yearEndDate.Year > x.YearEndDate.Year + 1).FirstOrDefault();

                if (result != null)
                {
                    isValid = false;
                    message = "You are trying to skip year";
                    return isValid;
                }
              
                result = objProcessedYears.OrderByDescending(x => x.YearStartDate).Where(x => yearStartDate.Year == x.YearStartDate.Year + 1 && yearEndDate.Year == x.YearEndDate.Year + 1).FirstOrDefault();
                
                //Right Year
                if (result != null)
                {
                    isValid = true;
                    message = string.Empty;
                    return isValid;
                }               
            }
            return isValid;
        }

        public bool IsProfitDistributed(DateTime yearStartDate, DateTime yearEndDate, out string message)
        {

            bool isValid = false;
            message = string.Empty;


            //Checking Year Setup
            var objYearEndProcessLogs = DataContext.YearEndProcessLog
                                        .Where(x => x.IsDeleted == false);

            //No Year Ended
            if (objYearEndProcessLogs.Count() == 0)
            {
                isValid = true;
                message = string.Empty;
                return isValid;
            }

            //Check previous Year Ended and profit and induce rate declared properly
            if (objYearEndProcessLogs.Count() > 0)
            {   //Get: Previous year closed or not
                var objYearEndProcessLog = objYearEndProcessLogs
                                            .Where(x => x.YearStartDate == yearStartDate.AddYears(-1) && x.YearEndDate == yearEndDate.AddYears(-1))
                                           .FirstOrDefault();
                ////Previous year closed
                if (objYearEndProcessLog != null)
                {
                    var objProfitDeclaration = DataContext.ProfitDeclaration
                                                .Where(x => x.YearStartDate == yearStartDate.AddYears(-1) && x.YearEndDate == yearEndDate.AddYears(-1))
                                                .OrderByDescending(x => x.YearStartDate).Take(1)
                                                .FirstOrDefault();

                    if (objProfitDeclaration == null)
                    {
                        isValid = false;
                        message = "In previous year ending process- profit information has not been recoreded";
                        return isValid;
                    }
                    else
                    {
                        //if (objProfitDeclaration)
                        //{
                        //    isValid = false;
                        //    message = "Induce rate has not been set by trusty board";
                        //    return isValid;
                        //}
                        //else
                        //{
                            isValid = true;
                            message = string.Empty;
                        //}
                    }
                }
            }

            //Checking Profit distribution

            var objProfitDistribution = DataContext.ProfitDistProcessLog
                                        .Where(x => x.IsDeleted == false && x.IsProcessed == true && x.YearStartDate == yearStartDate.AddYears(-1) && x.YearEndDate == yearEndDate.AddYears(-1))
                                        .OrderByDescending(x => x.YearStartDate).Take(1)
                                        .FirstOrDefault();
            if(objProfitDistribution == null)
            {
                isValid = false;
                message = "Profit of previous year has not been distributed";
                return isValid;
            }
            else
            {
                isValid = true;
                message = string.Empty;
                return isValid;
            }
           // return isValid;
        } 
    }
}
