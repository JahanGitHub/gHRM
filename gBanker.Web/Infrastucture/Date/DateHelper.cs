using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace gHRM.Web.Infrastructure.Date
{

    public static class DateHelper
    {
        public static List<SelectListItem> GetYears(int startBeforeCurrentYear = 0, int endYear = 10)
        {
            List<SelectListItem> yearList = new List<SelectListItem>();
            startBeforeCurrentYear = startBeforeCurrentYear > 0 ? startBeforeCurrentYear : 0;
            endYear = endYear > 0 ? endYear : 10; 
           
            for (var i = -startBeforeCurrentYear; i <= endYear; i++)
            {
                yearList.Add(new SelectListItem
                {
                    Text = (DateTime.Today.Year + i).ToString(),
                    Value = (DateTime.Today.Year + i).ToString()
                });
            }

            return yearList;
        }

        public static List<SelectListItem> GetMonths()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem
            {
                Text = "January",
                Value = "1"
            });
            items.Add(new SelectListItem
            {
                Text = "February",
                Value = "2"
            });
            items.Add(new SelectListItem
            {
                Text = "March",
                Value = "3"
            });
            items.Add(new SelectListItem
            {
                Text = "April",
                Value = "4"
            });
            items.Add(new SelectListItem
            {
                Text = "May",
                Value = "5"
            });
            items.Add(new SelectListItem
            {
                Text = "June",
                Value = "6"
            });
            items.Add(new SelectListItem
            {
                Text = "July",
                Value = "7"
            });
            items.Add(new SelectListItem
            {
                Text = "August",
                Value = "8"
            });
            items.Add(new SelectListItem
            {
                Text = "September",
                Value = "9"
            });
            items.Add(new SelectListItem
            {
                Text = "October",
                Value = "10"
            });
            items.Add(new SelectListItem
            {
                Text = "November",
                Value = "11"
            });
            items.Add(new SelectListItem
            {
                Text = "December",
                Value = "12"
            });

            return items;
        }
    }
}
