using gHRM.Data.CodeFirstMigration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;

namespace gHRM.Web.CommonDropdown
{
    public class CommonStaticDropDown
    {
        //Empty dropdown with please select
        public List<SelectListItem> ddlInitial()
        {
            List<SelectListItem> InitialItem = new List<SelectListItem>();
            InitialItem.Add(new SelectListItem() { Value = "", Text = "Please Select" });
            return InitialItem;
        }

        public List<SelectListItem> ddlInitial(string DefaultValue = "")
        {
            List<SelectListItem> InitialItem = new List<SelectListItem>();
            InitialItem.Add(new SelectListItem() { Value = DefaultValue, Text = "Please Select" });
            return InitialItem;
        }

        public List<SelectListItem> ddlInitial(bool? isAddPleaseSelect = true, string DefaultValue = "")
        {
            List<SelectListItem> InitialItem = new List<SelectListItem>();
            if (isAddPleaseSelect == true)
            {
                InitialItem.Add(new SelectListItem() { Value = DefaultValue, Text = "Please Select" });
            }
            return InitialItem;
        }


        //Only yes no list with please select
        public List<SelectListItem> GetYesNoList()
        {
            var resultList = ddlInitial();
            try
            {
                resultList.Add(new SelectListItem() { Text = "Yes", Value = "Y" });
                resultList.Add(new SelectListItem() { Text = "No", Value = "N" });
            }
            catch (Exception e)
            {
                throw;
            }
            return resultList;
        }

        public List<SelectListItem> YesNoDropDown_bool(bool? IsInclude_PleaseSelect = true, string PleaseSelect_DefaultValue = "")
        {
            List<SelectListItem> initial;
            if (IsInclude_PleaseSelect != false)
            {
                initial = ddlInitial(PleaseSelect_DefaultValue);
            }
            else
            {
                initial = new List<SelectListItem>();
            }
            initial.Add(new SelectListItem { Text = "No", Value = "false" });
            initial.Add(new SelectListItem { Text = "Yes", Value = "true" });
            return initial;
        }

        public List<SelectListItem> YesNoDropDown_Int(bool? IsInclude_PleaseSelect = true, string PleaseSelect_DefaultValue = "")
        {
            List<SelectListItem> initial;
            if (IsInclude_PleaseSelect != false)
            {
                initial = ddlInitial(PleaseSelect_DefaultValue);
            }
            else
            {
                initial = new List<SelectListItem>();
            }

            initial.Add(new SelectListItem { Text = "Yes", Value = "1" });
            initial.Add(new SelectListItem { Text = "No", Value = "0" });
            return initial;
        }

        public List<SelectListItem> YesNoDropDown_Int(string WhichOneInclude, bool? IsInclude_PleaseSelect = true, string PleaseSelect_DefaultValue = "")
        {
            List<SelectListItem> initial;
            initial = ddlInitial(PleaseSelect_DefaultValue);

            initial.Add(new SelectListItem { Text = "Yes", Value = "1" });
            initial.Add(new SelectListItem { Text = "No", Value = "0" });

            return initial;
        }



        public IEnumerable<SelectListItem> NumberSerialDropDown(int startNum, int Endnum, string InitialValue = "")
        {
            var initial = ddlInitial(InitialValue);
            //for (int y = startNum; y <= Endnum; y++)
            //    initial.Add(new SelectListItem() { Text = y.ToString(), Value = y.ToString() });

            return initial;
        }

        public IEnumerable<SelectListItem> NumberSerialDropDown(int startNum, int Endnum, bool isIncludePleaseSelect, string InitialValue = "")
        {
            var initial = ddlInitial(isIncludePleaseSelect, InitialValue);
            for (int y = startNum; y <= Endnum; y++)
            {
                initial.Add(new SelectListItem() { Text = y.ToString(), Value = y.ToString() });
            }

            return initial;
        }

        // get months name list and number as valye
        public List<SelectListItem> GetMonthListList()
        {
            var resultList = ddlInitial();
            try
            {
                for (var i = 1; i <= 12; i++)
                {
                    resultList.Add(new SelectListItem { Text = DateTimeFormatInfo.CurrentInfo.GetMonthName(i), Value = i.ToString() });
                }
            }
            catch (Exception e)
            {
                throw;
            }
            return resultList;
        }

        //get 1 to 10 number list for level dropdown
        public List<SelectListItem> Get1To10NumberList()
        {
            var resultList = ddlInitial();
            try
            {
                var i = 0;
                for (i = 1; i <= 10; i++)
                {
                    resultList.Add(new SelectListItem() { Text = i.ToString(), Value = i.ToString() });
                }
            }
            catch (Exception e)
            {
                throw;
            }
            return resultList;
        }


        //Period in months up to 60 months
        public List<SelectListItem> GetPeriodInMonthsList()
        {
            var resultList = ddlInitial();
            try
            {
                resultList.Add(new SelectListItem() { Text = "1 Month", Value = "1" });
                resultList.Add(new SelectListItem() { Text = "2 Months", Value = "2" });
                resultList.Add(new SelectListItem() { Text = "3 Months", Value = "3" });
                resultList.Add(new SelectListItem() { Text = "4 Months", Value = "4" });
                resultList.Add(new SelectListItem() { Text = "5 Months", Value = "5" });
                resultList.Add(new SelectListItem() { Text = "6 Months", Value = "6" });
                resultList.Add(new SelectListItem() { Text = "7 Months", Value = "7" });
                resultList.Add(new SelectListItem() { Text = "8 Months", Value = "8" });
                resultList.Add(new SelectListItem() { Text = "9 Months", Value = "9" });
                resultList.Add(new SelectListItem() { Text = "10 Months", Value = "10" });
                resultList.Add(new SelectListItem() { Text = "11 Months", Value = "11" });
                resultList.Add(new SelectListItem() { Text = "1 Year", Value = "12" });
                resultList.Add(new SelectListItem() { Text = "1 Year 6 Months", Value = "18" });
                resultList.Add(new SelectListItem() { Text = "2 Years", Value = "24" });
                resultList.Add(new SelectListItem() { Text = "2 Year 6 Months", Value = "30" });
                resultList.Add(new SelectListItem() { Text = "3 Years", Value = "36" });
                resultList.Add(new SelectListItem() { Text = "5 Years", Value = "60" });
                resultList.Add(new SelectListItem() { Text = "Permanent", Value = "360" });
            }
            catch (Exception e)
            {
                throw;
            }
            return resultList;
        }


        //Religion List Islam selected
        public List<SelectListItem> GetReligionsList()
        {
            var resultList = ddlInitial();
            try
            {
                resultList.Add(new SelectListItem() { Text = "Islam", Value = "Islam" });
                resultList.Add(new SelectListItem() { Text = "Hindu", Value = "Hindu" });
                resultList.Add(new SelectListItem() { Text = "Buddish", Value = "Buddish" });
                resultList.Add(new SelectListItem() { Text = "Christan", Value = "Christan" });
            }
            catch (Exception e)
            {
                throw;
            }
            return resultList;
        }


        //Get Gender list Male seleted
        public List<SelectListItem> GetGendersList()
        {
            var resultList = ddlInitial();
            try
            {
                resultList.Add(new SelectListItem() { Text = "Male", Value = "M" });
                resultList.Add(new SelectListItem() { Text = "Female", Value = "F" });
                resultList.Add(new SelectListItem() { Text = "Common", Value = "C" });
            }
            catch (Exception e)
            {
                throw;
            }
            return resultList;
        }

        // get male female and both gender list for leave
        public List<SelectListItem> GetMaleFemaleAndBothGenderList()
        {
            var resultList = ddlInitial();
            try
            {
                resultList.Add(new SelectListItem() { Text = "Both", Value = "B" });
                resultList.Add(new SelectListItem() { Text = "Male", Value = "M" });
                resultList.Add(new SelectListItem() { Text = "Female", Value = "F" });
            }
            catch (Exception e)
            {
                throw;
            }
            return resultList;
        }

        //Get Marital(Married) Status list
        public List<SelectListItem> GetMaritalStatusList()
        {
            var resultList = ddlInitial();
            try
            {
                resultList.Add(new SelectListItem() { Text = "Married", Value = "M" });
                resultList.Add(new SelectListItem() { Text = "Unmarried", Value = "U" });
                resultList.Add(new SelectListItem() { Text = "Divorced", Value = "D" });
            }
            catch (Exception e)
            {
                throw;
            }
            return resultList;
        }

        //Get Certificate Receive Return List
        public List<SelectListItem> GetCertificateReceiveReturnList()
        {
            var resultList = ddlInitial();
            try
            {
                resultList.Add(new SelectListItem() { Text = "Receive", Value = "Receive" });
                resultList.Add(new SelectListItem() { Text = "Return", Value = "Return" });
            }
            catch (Exception e)
            {
                throw;
            }
            return resultList;
        }



        public List<SelectListItem> ddlSalaryRatio()
        {
            var initial = ddlInitial();
            var ratioOn = new List<SelectListItem>();
            ratioOn.Add(new SelectListItem() { Text = "Gross", Value = "G" });
            ratioOn.Add(new SelectListItem() { Text = "Basic", Value = "B" });
            ratioOn.Add(new SelectListItem() { Text = "Not Required", Value = "NR" });
            initial.AddRange(ratioOn);
            return initial;
        }

        // get employee relation type list
        public List<SelectListItem> GetRelationTypeList()
        {
            var resultList = ddlInitial();
            try
            {
                resultList.Add(new SelectListItem() { Text = "Father", Value = "F" });
                resultList.Add(new SelectListItem() { Text = "Mother", Value = "M" });
                resultList.Add(new SelectListItem() { Text = "Wife", Value = "W" });
                resultList.Add(new SelectListItem() { Text = "Husband", Value = "H" });
                resultList.Add(new SelectListItem() { Text = "Son", Value = "S" });
                resultList.Add(new SelectListItem() { Text = "Daughter", Value = "D" });
                resultList.Add(new SelectListItem() { Text = "Brother", Value = "Br" });
                resultList.Add(new SelectListItem() { Text = "Sister", Value = "Sis" });
                resultList.Add(new SelectListItem() { Text = "Father-in-law", Value = "Finl" });
                resultList.Add(new SelectListItem() { Text = "Mother-in-law", Value = "Minl" });
            }
            catch (Exception e)
            {
                throw;
            }
            return resultList;
        }


        // get blood group type list
        public List<SelectListItem> GetAllBloodGroupTypeList()
        {
            var resultList = ddlInitial();
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
                resultList.Add(new SelectListItem() { Text = "Unknown", Value = "U" });

            }
            catch (Exception e)
            {
                throw;
            }
            return resultList;
        }



        //Get leave Eligible From which date list
        public List<SelectListItem> GetLeaveEligibleDateList()
        {
            var resultList = ddlInitial();
            try
            {
                resultList.Add(new SelectListItem() { Text = "Confirmation Date", Value = "C" });
                resultList.Add(new SelectListItem() { Text = "Joining Date", Value = "J" });
            }
            catch (Exception e)
            {
                throw;
            }
            return resultList;
        }

        //Get leave Laps or Carry forward From which date list
        public List<SelectListItem> GetLeaveLapsOrCarryFoewardStatusList()
        {
            var resultList = ddlInitial();
            try
            {
                resultList.Add(new SelectListItem() { Text = "Laps", Value = "L" });
                resultList.Add(new SelectListItem() { Text = "Carry Forward", Value = "C" });
                resultList.Add(new SelectListItem() { Text = "N/A", Value = "N" });
            }
            catch (Exception e)
            {
                throw;
            }
            return resultList;
        }

        // get education certificate type
        public List<SelectListItem> GetEducationCertificateTypeList()
        {
            var resultList = ddlInitial();
            try
            {
                resultList.Add(new SelectListItem() { Text = "Provisional", Value = "Provisional" });
                resultList.Add(new SelectListItem() { Text = "Original", Value = "Original" });

            }
            catch (Exception e)
            {
                throw;
            }
            return resultList;
        }

        public IEnumerable<SelectListItem> SalaryChangesByComponentList()
        {
            var initial = new List<SelectListItem>();
            initial.Add(new SelectListItem { Value = "N/A", Text = "Not Applicable" });
            initial.Add(new SelectListItem { Value = "Positive", Text = "Increment Effect With Regular Salary" });
            initial.Add(new SelectListItem { Value = "Negative", Text = "Decrement Effect With Regular Salary" });
            return initial;
        }
        
        //private List<SelectListItem> Years()
        //{
        //    List<SelectListItem> items2 = new List<SelectListItem>();
        //    items2.Add(new SelectListItem
        //    {
        //        Text = "Please Select",
        //        Value = "0"
        //    });

        //    int year = DateTime.Now.Year; //Current Year.
        //    int lowYear = year - 5;


        //    for (; year >= lowYear; year--)
        //    {
        //        items2.Add(new SelectListItem
        //        {
        //            Text = Convert.ToString(year),
        //            Value = Convert.ToString(year)
        //        });
        //    }

        //    return items2;
        //}// End of Years

        public IEnumerable<SelectListItem> YearList(int beforeYear = 2, int afterYear = 2)
        {
            int year = DateTime.Now.Year; //Current Year.
            int minYear = year - beforeYear;
            int maxYear = year + afterYear;

            List<SelectListItem> years = new List<SelectListItem>();

            for (; maxYear >= minYear; maxYear--)
            {
                years.Add(new SelectListItem
                {
                    Text = Convert.ToString(maxYear),
                    Value = Convert.ToString(maxYear)
                });
            }

            var initial = ddlInitial();
            initial.AddRange(years);
            return initial;

        }// End of Years

        public IEnumerable<SelectListItem> YearDurationList(int Year)
        {
            var initial = ddlInitial();
            initial.Add(new SelectListItem() { Text = "Half Year", Value = "0.5" });
            initial.Add(new SelectListItem() { Text = "1 Year", Value = "1" });
            for (int y = 2; y <= Year; y++)
            {
                initial.Add(new SelectListItem() { Text = y.ToString() + " Years", Value = y.ToString() });
            }
         //   initial.Add(new SelectListItem() { Text = "Continue", Value = "100" });
            initial.Add(new SelectListItem() { Text = "Other", Value = "Other" });
            return initial;
        }

        public IEnumerable<SelectListItem> SalaryCalculationType()
        {
            var initial = ddlInitial();
            initial.Add(new SelectListItem { Text = "Ratio", Value = "R" });
            initial.Add(new SelectListItem { Text = "Fixed", Value = "F" });
            return initial;
        }

        public IEnumerable<SelectListItem> SalaryAccountTransactionType(string DefaultInitialValue = "")
        {
            var initial = ddlInitial(DefaultInitialValue);
            initial.Add(new SelectListItem { Text = "Addition", Value = "Cr" });
            initial.Add(new SelectListItem { Text = "Deduction", Value = "Dr" });
            return initial;
        }

        public IEnumerable<SelectListItem> SalaryComponentCategory()
        {
            var initial = ddlInitial();
            initial.Add(new SelectListItem { Text = "Salary", Value = "Salary" });
            initial.Add(new SelectListItem { Text = "Bonus", Value = "Bonus" });
            initial.Add(new SelectListItem { Text = "Loan", Value = "Loan" });
            initial.Add(new SelectListItem { Text = "Allowance", Value = "Allowance" });
            initial.Add(new SelectListItem { Text = "Deduction", Value = "Deduction" });
            initial.Add(new SelectListItem { Text = "Deposit", Value = "Deposit" });
            return initial;
        }

        public IEnumerable<SelectListItem> MonthList()
        {
            var monthList = new List<SelectListItem>();
            monthList.Add(new SelectListItem() { Text = "January", Value = "1" });
            monthList.Add(new SelectListItem() { Text = "February", Value = "2" });
            monthList.Add(new SelectListItem() { Text = "March", Value = "3" });
            monthList.Add(new SelectListItem() { Text = "April", Value = "4" });
            monthList.Add(new SelectListItem() { Text = "May", Value = "5" });
            monthList.Add(new SelectListItem() { Text = "June", Value = "6" });
            monthList.Add(new SelectListItem() { Text = "July", Value = "7" });
            monthList.Add(new SelectListItem() { Text = "August", Value = "8" });
            monthList.Add(new SelectListItem() { Text = "September", Value = "9" });
            monthList.Add(new SelectListItem() { Text = "October", Value = "10" });
            monthList.Add(new SelectListItem() { Text = "November", Value = "11" });
            monthList.Add(new SelectListItem() { Text = "December", Value = "12" });
            var initial = ddlInitial();
            initial.AddRange(monthList);
            return initial;
        }

        public IEnumerable<SelectListItem> SalaryGenerationTypeList()
        {
            var generationList = new List<SelectListItem>();

            generationList.Add(new SelectListItem() { Text = "Pay-Scale", Value = "PS" });
            generationList.Add(new SelectListItem() { Text = "Non Pay-Scale", Value = "NPS" });
            var initial = ddlInitial();
            initial.AddRange(generationList);
            return initial;
        }

        public IEnumerable<SelectListItem> SalaryStructuredTypeList()
        {
            var empSalaryType = new List<SelectListItem>();

            empSalaryType.Add(new SelectListItem() { Text = "Structured", Value = "1" });
            empSalaryType.Add(new SelectListItem() { Text = "Unstructured", Value = "2" });
            var initial = ddlInitial();
            initial.AddRange(empSalaryType);
            return initial;
        }

        public List<SelectListItem> GetLeaveDayDurationList()
        {
            var resultList = new List<SelectListItem>();
            try
            {
                resultList.Add(new SelectListItem() { Text = "Full", Value = "Full" });
                resultList.Add(new SelectListItem() { Text = "First Half", Value = "First Half" });
                resultList.Add(new SelectListItem() { Text = "Second Half", Value = "Second Half" });
            }
            catch (Exception e)
            {
                throw;
            }
            return resultList;
        }
    }
}