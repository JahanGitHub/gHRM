using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using gHRM.Web.ViewModels;
using Newtonsoft.Json;
using BasicDataAccess;
using System.Data;

namespace gHRM.Web.Helpers
{
    public class DashboardHelper
    {
        private HttpContextBase Context;

        public DashboardHelper(HttpContextBase Context)
        {
            this.Context = Context;
        }

        public HRDashboardViewModel LoadData()
        {
            return GetData();
        }

        private string Path()
        {
            return Context.Server.MapPath("~/");
        }

        private HRDashboardViewModel GetData()
        {
            HRDashboardViewModel Result = new HRDashboardViewModel();
            using (var gbData = new gHRMDataAccess())
            {
                Dictionary<string, int> GenderCountList = new Dictionary<string, int>();
                Dictionary<int, int> Leave6MonthsCountList = new Dictionary<int, int>();

                DataSet Data = gbData.GetDataOnDateset("dbo.SP_Dashboard", new { });
                DataTable DtCommon = Data.Tables[0];
                DataTable DtGender = Data.Tables[1];
                //DataTable DtLeave6MonthsCount = Data.Tables[2];
                DataTable DtAtt = Data.Tables[3];
                DataTable DtTOUR = Data.Tables[4];
                DataTable DtLEAVE = Data.Tables[5];

                Result.TotalBranches = Convert.ToInt32(DtCommon.Rows[0]["TotalBranches"]);
                Result.TotalEmployees = Convert.ToInt32(DtCommon.Rows[0]["TotalEmployees"]);
                Result.TotalEmployeesProbation = Convert.ToInt32(DtCommon.Rows[0]["TotalEmployeesProbation"]);
                Result.TotalJoinedThisMonth = Convert.ToInt32(DtCommon.Rows[0]["TotalJoinedThisMonth"]);
                Result.TotalLeftThisMonth = Convert.ToInt32(DtCommon.Rows[0]["TotalLeftThisMonth"]);
                Result.TotalEmployeesHO = Convert.ToInt32(DtCommon.Rows[0]["TotalEmployeesHO"]);
                Result.TotalEmployeesFO = Convert.ToInt32(DtCommon.Rows[0]["TotalEmployeesFO"]);
                Result.TotalEmployeesHOProbation = Convert.ToInt32(DtCommon.Rows[0]["TotalEmployeesHOProbation"]);
                Result.TotalEmployeesFOProbation = Convert.ToInt32(DtCommon.Rows[0]["TotalEmployeesFOProbation"]);
                Result.TotalJoinedThisMonthHO = Convert.ToInt32(DtCommon.Rows[0]["TotalJoinedThisMonthHO"]);
                Result.TotalJoinedThisMonthFO = Convert.ToInt32(DtCommon.Rows[0]["TotalJoinedThisMonthFO"]);
                Result.TotalLeftThisMonthHO = Convert.ToInt32(DtCommon.Rows[0]["TotalLeftThisMonthHO"]);
                Result.TotalLeftThisMonthFO = Convert.ToInt32(DtCommon.Rows[0]["TotalLeftThisMonthFO"]);
                if (DtAtt.Rows.Count == 3 )
                {
                    Result.Absent = Convert.ToInt32(DtAtt.Rows[0]["TotaAttendanceStatus"]);

                    Result.Late = Convert.ToInt32(DtAtt.Rows[1]["TotaAttendanceStatus"]);

                    Result.Present = Convert.ToInt32(DtAtt.Rows[2]["TotaAttendanceStatus"]);

                    Result.Leave = 0;

                }
                if (DtAtt.Rows.Count == 4 )
                {
                    Result.Absent = Convert.ToInt32(DtAtt.Rows[0]["TotaAttendanceStatus"]);

                    Result.Late = Convert.ToInt32(DtAtt.Rows[1]["TotaAttendanceStatus"]);

                    Result.Leave = Convert.ToInt32(DtAtt.Rows[2]["TotaAttendanceStatus"]);

                    Result.Present = Convert.ToInt32(DtAtt.Rows[3]["TotaAttendanceStatus"]);

                }                  

                Result.Tour = Convert.ToInt32(DtTOUR.Rows[0]["TOUR"]);
                //Result.Leave = Convert.ToInt32(DtLEAVE.Rows[0]["LEAVE"]);


                Result.LastUpdated = DateTime.Now;

                foreach (DataRow DR in DtGender.Rows)
                    GenderCountList[DR["Gender"].ToString()] = Convert.ToInt32(DR["GenderCount"]);

                Result.GenderCountList = GenderCountList;

                //foreach (DataRow DR in DtLeave6MonthsCount.Rows)
                //{
                //    Leave6MonthsCountList[Convert.ToInt32(DR["MonthIndex"])] = Convert.ToInt32(DR["EmpCount"]);
                //}
                //Result.Leave6MonthsCountList = Leave6MonthsCountList;




            }
            return Result;
        }
    }
}