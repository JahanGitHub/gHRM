using gHRM.Web.ViewModels.ScheduleManager;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace gHRM.Web.Controllers.Schedule
{
    public class ScheduleManagerController : Controller
    {
        // GET: ScheduleManager
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetCalendarData()
        {
            JsonResult result = new JsonResult();
            try
            {
                List<PublicHoliday> data = this.LoadData();
                result = this.Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }
            return result;
        }

        private List<PublicHoliday> LoadData()
        {
            List<PublicHoliday> lst = new List<PublicHoliday>();
            lst.Add(new PublicHoliday() { Sr = 1, Desc = "Test1", Title = "Mr. DGM will be on a meeting with Team 1 from 5 pm to 7 pm", Start_Date = DateTime.UtcNow.AddHours(2).ToString(), End_Date = DateTime.UtcNow.AddDays(4).AddHours(3).ToString() });
            lst.Add(new PublicHoliday() { Sr = 1, Desc = "Test2", Title = "Mr. GM will be on a meeting with Team 1 from 8 pm to 10 pm", Start_Date = DateTime.UtcNow.AddDays(1).AddHours(2).ToString(), End_Date = DateTime.UtcNow.AddDays(4).AddHours(3).ToString() });
            lst.Add(new PublicHoliday() { Sr = 1, Desc = "Test3", Title = "Title3", Start_Date = DateTime.UtcNow.AddHours(2).ToString(), End_Date = DateTime.UtcNow.AddDays(2).AddHours(3).ToString() });
            lst.Add(new PublicHoliday() { Sr = 1, Desc = "Test4", Title = "Title4", Start_Date = DateTime.UtcNow.ToString(), End_Date = DateTime.UtcNow.ToString() });

            lst.Add(new PublicHoliday() { Sr = 1, Desc = "Test1", Title = "Mr. DGM will be on a meeting with Team 1 from 5 pm to 7 pm", Start_Date = DateTime.UtcNow.AddHours(2).ToString(), End_Date = DateTime.UtcNow.AddDays(5).AddHours(3).ToString() });
            lst.Add(new PublicHoliday() { Sr = 1, Desc = "Test2", Title = "Mr. GM will be on a meeting with Team 1 from 8 pm to 10 pm", Start_Date = DateTime.UtcNow.AddDays(1).AddHours(2).ToString(), End_Date = DateTime.UtcNow.AddDays(4).AddHours(3).ToString() });
            lst.Add(new PublicHoliday() { Sr = 1, Desc = "Test3", Title = "Title3", Start_Date = DateTime.UtcNow.AddHours(2).ToString(), End_Date = DateTime.UtcNow.AddDays(2).AddHours(3).ToString() });
            lst.Add(new PublicHoliday() { Sr = 1, Desc = "Test4", Title = "Title4", Start_Date = DateTime.UtcNow.ToString(), End_Date = DateTime.UtcNow.ToString() });

            lst.Add(new PublicHoliday() { Sr = 1, Desc = "Test1", Title = "Mr. DGM will be on a meeting with Team 1 from 5 pm to 7 pm", Start_Date = DateTime.UtcNow.AddHours(2).ToString(), End_Date = DateTime.UtcNow.AddDays(3).AddHours(3).ToString() });
            lst.Add(new PublicHoliday() { Sr = 1, Desc = "Test2", Title = "Mr. GM will be on a meeting with Team 1 from 8 pm to 10 pm", Start_Date = DateTime.UtcNow.AddDays(1).AddHours(2).ToString(), End_Date = DateTime.UtcNow.AddDays(4).AddHours(3).ToString() });
            lst.Add(new PublicHoliday() { Sr = 1, Desc = "Test3", Title = "Title3", Start_Date = DateTime.UtcNow.AddHours(2).ToString(), End_Date = DateTime.UtcNow.AddDays(2).AddHours(3).ToString() });
            lst.Add(new PublicHoliday() { Sr = 1, Desc = "Test4", Title = "Title4", Start_Date = DateTime.UtcNow.ToString(), End_Date = DateTime.UtcNow.ToString() });

            lst.Add(new PublicHoliday() { Sr = 1, Desc = "Test1", Title = "Mr. DGM will be on a meeting with Team 1 from 5 pm to 7 pm", Start_Date = DateTime.UtcNow.AddHours(2).ToString(), End_Date = DateTime.UtcNow.AddDays(3).AddHours(3).ToString() });
            lst.Add(new PublicHoliday() { Sr = 1, Desc = "Test2", Title = "Mr. GM will be on a meeting with Team 1 from 8 pm to 10 pm", Start_Date = DateTime.UtcNow.AddDays(1).AddHours(2).ToString(), End_Date = DateTime.UtcNow.AddDays(4).AddHours(3).ToString() });
            lst.Add(new PublicHoliday() { Sr = 1, Desc = "Test3", Title = "Title3", Start_Date = DateTime.UtcNow.AddHours(2).ToString(), End_Date = DateTime.UtcNow.AddDays(2).AddHours(3).ToString() });
            lst.Add(new PublicHoliday() { Sr = 1, Desc = "Test4", Title = "Title4", Start_Date = DateTime.UtcNow.ToString(), End_Date = DateTime.UtcNow.ToString() });

            lst.Add(new PublicHoliday() { Sr = 1, Desc = "Test1", Title = "Mr. DGM will be on a meeting with Team 1 from 5 pm to 7 pm", Start_Date = DateTime.UtcNow.AddHours(2).ToString(), End_Date = DateTime.UtcNow.AddDays(3).AddHours(3).ToString() });
            lst.Add(new PublicHoliday() { Sr = 1, Desc = "Test2", Title = "Mr. GM will be on a meeting with Team 1 from 8 pm to 10 pm", Start_Date = DateTime.UtcNow.AddDays(1).AddHours(2).ToString(), End_Date = DateTime.UtcNow.AddDays(4).AddHours(3).ToString() });
            lst.Add(new PublicHoliday() { Sr = 1, Desc = "Test3", Title = "Title3", Start_Date = DateTime.UtcNow.AddHours(2).ToString(), End_Date = DateTime.UtcNow.AddDays(2).AddHours(3).ToString() });
            lst.Add(new PublicHoliday() { Sr = 1, Desc = "Test4", Title = "Title4", Start_Date = DateTime.UtcNow.ToString(), End_Date = DateTime.UtcNow.ToString() });

            lst.Add(new PublicHoliday() { Sr = 1, Desc = "Test1", Title = "Mr. DGM will be on a meeting with Team 1 from 5 pm to 7 pm", Start_Date = DateTime.UtcNow.AddHours(2).ToString(), End_Date = DateTime.UtcNow.AddDays(3).AddHours(3).ToString() });
            lst.Add(new PublicHoliday() { Sr = 1, Desc = "Test2", Title = "Mr. GM will be on a meeting with Team 1 from 8 pm to 10 pm", Start_Date = DateTime.UtcNow.AddDays(1).AddHours(2).ToString(), End_Date = DateTime.UtcNow.AddDays(4).AddHours(3).ToString() });
            lst.Add(new PublicHoliday() { Sr = 1, Desc = "Test3", Title = "Title3", Start_Date = DateTime.UtcNow.AddHours(2).ToString(), End_Date = DateTime.UtcNow.AddDays(2).AddHours(3).ToString() });
            lst.Add(new PublicHoliday() { Sr = 1, Desc = "Test4", Title = "Title4", Start_Date = DateTime.UtcNow.ToString(), End_Date = DateTime.UtcNow.ToString() });
            return lst;
        }
    }
}