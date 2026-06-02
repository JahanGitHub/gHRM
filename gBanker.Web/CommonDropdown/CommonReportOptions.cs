using gHRM.Service.DropDownService;
using gHRM.Service.StoreProcedure;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gHRM.Web.DropDownService
{
    public class CommonReportOptions
    {

        List<SelectListItem> emptyList;
        List<SelectListItem> resultList;
        private CommonDropDownService commonDropDownService = new CommonDropDownService();

        //Empty dropdown with please select
        public List<SelectListItem> GetEmptyList()
        {
            emptyList = new List<SelectListItem>();
            emptyList.Add(new SelectListItem() { Text = "Please Select", Value = "" });
            return emptyList;
        }
     
        // Employee Report Options
        public List<SelectListItem> GetEmployeeReportOptions() {
            resultList = GetEmptyList();
            try
            {                
                resultList.Add(new SelectListItem() { Text = "Employee Wise Product", Value = "1" });
                resultList.Add(new SelectListItem() { Text = "Blood Group Wise All Employee", Value = "2" });
                resultList.Add(new SelectListItem() { Text = "Chart Of Blood Summary", Value = "3" });
                resultList.Add(new SelectListItem() { Text = "Office Name Wise Employee Count Summary", Value = "4" });
                resultList.Add(new SelectListItem() { Text = "Office Type Wise Employee Count Summary", Value = "5" });
                resultList.Add(new SelectListItem() { Text = "Gender Wise Employee", Value = "6" });
                resultList.Add(new SelectListItem() { Text = "All Department Wise Employee", Value = "7" });
                resultList.Add(new SelectListItem() { Text = "Department Wise Total Employee", Value = "8" });
                resultList.Add(new SelectListItem() { Text = "Department Wise Total employee (Graphical View)", Value = "9" });
                resultList.Add(new SelectListItem() { Text = "Payroll Designation Wise Employee", Value = "10" });
                resultList.Add(new SelectListItem() { Text = "Employment Type Wise Count", Value = "11" });
                resultList.Add(new SelectListItem() { Text = "Payroll Designation Wise Insurance", Value = "12" });
                resultList.Add(new SelectListItem() { Text = "Employee experience", Value = "13" });
                resultList.Add(new SelectListItem() { Text = "Employee Demographic Info", Value = "14" });
                resultList.Add(new SelectListItem() { Text = "Employee Pay Slip", Value = "15" });
                resultList.Add(new SelectListItem() { Text = "All Department Wise Employee For Mousumi", Value = "23" });
            }
            catch (Exception e)
            {
                throw;
            }
            return resultList;
        }


        // Blood Groups
        public List<SelectListItem> GetTransferReportOptions()
        {
            resultList = GetEmptyList();
            try
            {
                resultList.Add(new SelectListItem() { Text = "Office Order Report", Value = "1" });
            }

            catch (Exception e)
            {
                throw;
            }
            return resultList;
        }

        // Blood Groups
        public List<SelectListItem> GetBloodGroupList()
        {
            resultList = GetEmptyList();
            try
            {
                resultList.Add(new SelectListItem() { Text = "A+", Value = "A+" });
                resultList.Add(new SelectListItem() { Text = "A-", Value = "A-" });
                resultList.Add(new SelectListItem() { Text = "B+", Value = "B+" });
                resultList.Add(new SelectListItem() { Text = "B-", Value = "B-" });
                resultList.Add(new SelectListItem() { Text = "AB+", Value = "AB+" });
                resultList.Add(new SelectListItem() { Text = "AB-", Value = "AB-" });
                resultList.Add(new SelectListItem() { Text = "O+", Value = "O+" });
                resultList.Add(new SelectListItem() { Text = "O-", Value = "O-" });
                resultList.Add(new SelectListItem() { Text = "All Group", Value = "AG" });
            }
            catch (Exception e)
            {
                throw;
            }
            return resultList;
        }
    }
}