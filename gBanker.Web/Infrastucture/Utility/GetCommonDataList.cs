using gHRM.Service.DropDownService;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gHRM.Web.Infrastucture.Utility
{
    public class GetCommonDataList
    {
        List<SelectListItem> emptyList;
        List<SelectListItem> resultList;
        private CommonDropDownService commonDropDownService = new CommonDropDownService();
        //Empty dropdown List
        public List<SelectListItem> GetEmptyList()
        {
            emptyList = new List<SelectListItem>();
            return emptyList;
        }
        //Empty dropdown with please select
        public List<SelectListItem> GetEmptyListWithPleaseSelect()
        {
            emptyList = new List<SelectListItem>();
            emptyList.Add(new SelectListItem() { Text = "Please Select", Value = "" });
            return emptyList;
        }
        //Only yes no list with please select
        public List<SelectListItem> GetYesNoList()
        {
            resultList = GetEmptyListWithPleaseSelect();
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
        //Period in months up to 60 months
        //public List<SelectListItem> GetPeriodInMonthsList()
        //{
        //    resultList = GetEmptyListWithPleaseSelect();
        //    try
        //    {
        //        resultList.Add(new SelectListItem() { Text = "1 Month", Value = "1" });
        //        resultList.Add(new SelectListItem() { Text = "2 Months", Value = "2" });
        //        resultList.Add(new SelectListItem() { Text = "3 Months", Value = "3" });
        //        resultList.Add(new SelectListItem() { Text = "4 Months", Value = "4" });
        //        resultList.Add(new SelectListItem() { Text = "5 Months", Value = "5" });
        //        resultList.Add(new SelectListItem() { Text = "6 Months", Value = "6" });
        //        resultList.Add(new SelectListItem() { Text = "7 Months", Value = "7" });
        //        resultList.Add(new SelectListItem() { Text = "8 Months", Value = "8" });
        //        resultList.Add(new SelectListItem() { Text = "9 Months", Value = "9" });
        //        resultList.Add(new SelectListItem() { Text = "10 Months", Value = "10" });
        //        resultList.Add(new SelectListItem() { Text = "11 Months", Value = "11" });
        //        resultList.Add(new SelectListItem() { Text = "1 Year", Value = "12" });
        //        resultList.Add(new SelectListItem() { Text = "1 Year 6 Months", Value = "18" });
        //        resultList.Add(new SelectListItem() { Text = "2 Years", Value = "24" });
        //        resultList.Add(new SelectListItem() { Text = "2 Year 6 Months", Value = "30" });
        //        resultList.Add(new SelectListItem() { Text = "3 Years", Value = "36" });
        //        resultList.Add(new SelectListItem() { Text = "5 Years", Value = "60" });
        //    }
        //    catch (Exception e)
        //    {
        //        throw;
        //    }
        //    return resultList;
        //}
        ////Religion List Islam selected
        //public List<SelectListItem> GetReligionsList()
        //{
        //    resultList = new List<SelectListItem>();
        //    try
        //    {
        //        resultList.Add(new SelectListItem() { Text = "Islam", Value = "Islam" });
        //        resultList.Add(new SelectListItem() { Text = "Hindu", Value = "Hindu" });
        //        resultList.Add(new SelectListItem() { Text = "Buddish", Value = "Buddish" });
        //        resultList.Add(new SelectListItem() { Text = "Christan", Value = "Christan" });
        //    }
        //    catch (Exception e)
        //    {
        //        throw;
        //    }
        //    return resultList;
        //}
        ////Get Gender list Male seleted
        //public List<SelectListItem> GetGendersList()
        //{
        //    resultList = new List<SelectListItem>();
        //    try
        //    {
        //        resultList.Add(new SelectListItem() { Text = "Male", Value = "M" });
        //        resultList.Add(new SelectListItem() { Text = "Female", Value = "F" });
        //        resultList.Add(new SelectListItem() { Text = "Common", Value = "C" });
        //    }
        //    catch (Exception e)
        //    {
        //        throw;
        //    }
        //    return resultList;
        //}
        ////Get Marital(Married) Status list
        //public List<SelectListItem> GetMaritalStatusList()
        //{
        //    resultList = new List<SelectListItem>();
        //    try
        //    {
        //        resultList.Add(new SelectListItem() { Text = "Married", Value = "M" });
        //        resultList.Add(new SelectListItem() { Text = "Unmarried", Value = "U" });
        //    }
        //    catch (Exception e)
        //    {
        //        throw;
        //    }
        //    return resultList;
        //}
        ////Get Certificate Receive Return List
        //public List<SelectListItem> GetCertificateReceiveReturnList()
        //{
        //    resultList = new List<SelectListItem>();
        //    try
        //    {
        //        resultList.Add(new SelectListItem() { Text = "Receive", Value = "Receive" });
        //        resultList.Add(new SelectListItem() { Text = "Return", Value = "Return" });
        //    }
        //    catch (Exception e)
        //    {
        //        throw;
        //    }
        //    return resultList;
        //}
        ////Employee Status List all
        //public List<SelectListItem> GetAllEmployeeStatusList()
        //{
        //    resultList = GetEmptyListWithPleaseSelect();
        //    try
        //    {
        //        var employeeStatusList = commonDropDownService.GetAllEmployeeStatusList()
        //            .Select(obj => new SelectListItem
        //            {
        //                Text = obj.Name,
        //                Value = obj.Id.ToString()
        //            });
        //        resultList.AddRange(employeeStatusList);
        //    }
        //    catch (Exception e)
        //    {
        //        throw;
        //    }
        //    return resultList;
        //}
        ////Employee Status List "A","PR","EPR","CNT"
        //public List<SelectListItem> GetActiveEmployeeStatusList()
        //{
        //    resultList = GetEmptyList();
        //    try
        //    {
        //        var employeeStatusList = commonDropDownService.GetActiveEmployeeStatusList()
        //            .Select(obj => new SelectListItem
        //            {
        //                Text = obj.Name,
        //                Value = obj.Id.ToString()
        //            });
        //        resultList.AddRange(employeeStatusList);
        //    }
        //    catch (Exception e)
        //    {
        //        throw;
        //    }
        //    return resultList;
        //}
        ////If project then Project Departments otherwise HO departments
        //public List<SelectListItem> GetDepartmentByOfficeTypeId(int OfficeTypeId)
        //{
        //    resultList = GetEmptyListWithPleaseSelect();
        //    try
        //    {
        //        var list = commonDropDownService.GetDepartmentByOfficeTypeId(OfficeTypeId)
        //            .Select(obj => new SelectListItem
        //            {
        //                Text = obj.Name,
        //                Value = obj.Id.ToString()
        //            });
        //        resultList.AddRange(list);
        //    }
        //    catch (Exception e)
        //    {
        //        throw;
        //    }
        //    return resultList;
        //}
        ////Office type list
        //public List<SelectListItem> GetOfficeTypeList()
        //{
        //    resultList = GetEmptyListWithPleaseSelect();
        //    try
        //    {
        //        var officeTypeList = commonDropDownService.GetOfficeTypeList()
        //            .Select(obj => new SelectListItem
        //            {
        //                Text = obj.Name,
        //                Value = obj.Id.ToString()
        //            });
        //        resultList.AddRange(officeTypeList);
        //    }
        //    catch (Exception e)
        //    {
        //        throw;
        //    }
        //    return resultList;
        //}
        ////Head office list
        //public List<SelectListItem> GetHeadOfficeList()
        //{
        //    resultList = GetEmptyListWithPleaseSelect();
        //    try
        //    {
        //        var headOfficeList = commonDropDownService.GetHeadOfficeList()
        //            .Select(obj => new SelectListItem
        //            {
        //                Text = obj.Name,
        //                Value = obj.Id.ToString()
        //            });
        //        resultList.AddRange(headOfficeList);
        //    }
        //    catch (Exception e)
        //    {
        //        throw;
        //    }
        //    return resultList;
        //}
        ////Project office list
        //public List<SelectListItem> GetProjectOfficeList()
        //{
        //    resultList = GetEmptyListWithPleaseSelect();
        //    try
        //    {
        //        var headOfficeList = commonDropDownService.GetProjectOfficeList()
        //            .Select(obj => new SelectListItem
        //            {
        //                Text = obj.Name,
        //                Value = obj.Id.ToString()
        //            });
        //        resultList.AddRange(headOfficeList);
        //    }
        //    catch (Exception e)
        //    {
        //        throw;
        //    }
        //    return resultList;
        //}
        ////Zone office list
        //public List<SelectListItem> GetZoneOfficeList()
        //{
        //    resultList = GetEmptyListWithPleaseSelect();
        //    try
        //    {
        //        var zoneOfficeList = commonDropDownService.GetZoneOfficeList()
        //            .Select(obj => new SelectListItem
        //            {
        //                Text = obj.Name,
        //                Value = obj.Id.ToString()
        //            });
        //        resultList.AddRange(zoneOfficeList);
        //    }
        //    catch (Exception e)
        //    {
        //        throw;
        //    }
        //    return resultList;
        //}
        ////Area office list by Zone office
        //public List<SelectListItem> GetAreaOfficeListByZoneId(int ZoneId)
        //{
        //    resultList = GetEmptyListWithPleaseSelect();
        //    try
        //    {
        //        var areaOfficeList = commonDropDownService.GetAreaOfficeListByZoneId(ZoneId)
        //            .Select(obj => new SelectListItem
        //            {
        //                Text = obj.Name,
        //                Value = obj.Id.ToString()
        //            });
        //        resultList.AddRange(areaOfficeList);
        //    }
        //    catch (Exception e)
        //    {
        //        throw;
        //    }
        //    return resultList;
        //}
        ////Unit/Branch office list
        //public List<SelectListItem> GetUnitOfficeListByAreaId(int AreaId)
        //{
        //    resultList = GetEmptyListWithPleaseSelect();
        //    try
        //    {
        //        var unitOfficeList = commonDropDownService.GetUnitOfficeListByAreaId(AreaId)
        //            .Select(obj => new SelectListItem
        //            {
        //                Text = obj.Name,
        //                Value = obj.Id.ToString()
        //            });
        //        resultList.AddRange(unitOfficeList);
        //    }
        //    catch (Exception e)
        //    {
        //        throw;
        //    }
        //    return resultList;
        //}
        ////All Employee Designations/ Payroll designations list
        //public List<SelectListItem> GetAllPayrollDesignationList()
        //{
        //    resultList = GetEmptyListWithPleaseSelect();
        //    try
        //    {
        //        var payrollDesignationList = commonDropDownService.GetAllPayrollDesignationList()
        //            .Select(obj => new SelectListItem
        //            {
        //                Text = obj.Name,
        //                Value = obj.Id.ToString()
        //            });
        //        resultList.AddRange(payrollDesignationList);
        //    }
        //    catch (Exception e)
        //    {
        //        throw;
        //    }
        //    return resultList;
        //}
        ////All Office Designations/ Ranks/ Ornamental designations list
        //public List<SelectListItem> GetAllOfficeDesignationList()
        //{
        //    resultList = GetEmptyListWithPleaseSelect();
        //    try
        //    {
        //        var officeDesignationList = commonDropDownService.GetAllOfficeDesignationList()
        //            .Select(obj => new SelectListItem
        //            {
        //                Text = obj.Name,
        //                Value = obj.Id.ToString()
        //            });
        //        resultList.AddRange(officeDesignationList);
        //    }
        //    catch (Exception e)
        //    {
        //        throw;
        //    }
        //    return resultList;
        //}
        ////get 1 to 10 number list for level dropdown
        //public List<SelectListItem> Get1To10NumberList()
        //{
        //    resultList = GetEmptyListWithPleaseSelect();
        //    try
        //    {
        //        var i = 0;
        //        for (i = 1; i <= 10; i++)
        //        {
        //            resultList.Add(new SelectListItem() { Text = i.ToString(), Value = i.ToString() });
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        throw;
        //    }
        //    return resultList;
        //}
        ////get office type wise office list
        //public List<SelectListItem> GetOfficeTypeWiseOfficeList(int officeTypeId)
        //{
        //    resultList = GetEmptyListWithPleaseSelect();
        //    try
        //    {
        //        var officeList = commonDropDownService.GetOfficeTypeWiseOfficeList(officeTypeId);//officeService.GetOfficeByType(officeTypeId);
        //        var ViewOfficeList = officeList.AsEnumerable().Select(row => new SelectListItem()
        //        {
        //            Value = row.Id.ToString(),
        //            Text = row.Name
        //        }).ToList();
        //        resultList.AddRange(ViewOfficeList);
        //    }
        //    catch (Exception e)
        //    {
        //        throw;
        //    }
        //    return resultList;
        //}
        ////get employee list by officeId departmentId and employeeRank/officeDesignation/ornamentalDesignation



        //public List<SelectListItem> GetEmployeeListByOffice_Department_OfficeDesignation(int OfficeId, int DepartmentId, int OfficeDesignationId)
        //{
        //    resultList = GetEmptyListWithPleaseSelect();
        //    try
        //    {
        //        var employeeList = commonDropDownService.GetEmployeeListByOffice_Department_OfficeDesignation(OfficeId, DepartmentId, OfficeDesignationId);
        //        var ViewOfficeList = employeeList.AsEnumerable().Select(row => new SelectListItem()
        //        {
        //            Value = row.Id.ToString(),
        //            Text = row.Name
        //        }).ToList();
        //        resultList.AddRange(ViewOfficeList);
        //    }
        //    catch (Exception e)
        //    {
        //        throw;
        //    }
        //    return resultList;
        //}
        //// get months name list and number as valye
        //public List<SelectListItem> GetMonthListList()
        //{
        //    resultList = GetEmptyListWithPleaseSelect();
        //    try
        //    {
        //        for (var i = 1; i <= 12; i++)
        //        {
        //            resultList.Add(new SelectListItem { Text = DateTimeFormatInfo.CurrentInfo.GetMonthName(i), Value = i.ToString() });
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        throw;
        //    }
        //    return resultList;
        //}
        ////Get all active Department list
        //public List<SelectListItem> GetAllActiveDepartmentList()
        //{
        //    resultList = GetEmptyListWithPleaseSelect();
        //    try
        //    {
        //        var list = commonDropDownService.GetAllActiveDeptList()
        //            .Select(obj => new SelectListItem
        //            {
        //                Text = obj.Name,
        //                Value = obj.Id.ToString()
        //            });
        //        resultList.AddRange(list);
        //    }
        //    catch (Exception e)
        //    {
        //        throw;
        //    }
        //    return resultList;
        //}
        ////get all active leave Category list
        //public List<SelectListItem> GetAllLeaveCategoryList()
        //{
        //    resultList = GetEmptyListWithPleaseSelect();
        //    try
        //    {
        //        var list = commonDropDownService.GetAllLeaveCategoryList()
        //            .Select(obj => new SelectListItem
        //            {
        //                Text = obj.NameOther,
        //                Value = obj.Name
        //            });
        //        resultList.AddRange(list);
        //    }
        //    catch (Exception e)
        //    {
        //        throw;
        //    }
        //    return resultList;
        //}
        ////get leave type by employee status and gender
        //public List<SelectListItem> GetLeaveTypeListByEmployeeStatusAndGender(string EmployeeGender, string EmployeeStatus)
        //{
        //    resultList = GetEmptyListWithPleaseSelect();
        //    try
        //    {
        //        var list = commonDropDownService.GetLeaveTypeListByEmployeeStatusAndGender(EmployeeGender, EmployeeStatus)
        //            .Select(obj => new SelectListItem
        //            {
        //                Text = obj.Name + '-' + obj.NameOther,
        //                Value = obj.Id.ToString()
        //            });
        //        resultList.AddRange(list);
        //    }
        //    catch (Exception e)
        //    {
        //        throw;
        //    }
        //    return resultList;
        //}
        ////Get leave Eligible From which date list
        //public List<SelectListItem> GetLeaveEligibleDateList()
        //{
        //    resultList = GetEmptyListWithPleaseSelect();
        //    try
        //    {
        //        resultList.Add(new SelectListItem() { Text = "Confirmation Date", Value = "C" });
        //        resultList.Add(new SelectListItem() { Text = "Joining Date", Value = "J" });
        //    }
        //    catch (Exception e)
        //    {
        //        throw;
        //    }
        //    return resultList;
        //}
        ////Get leave Laps or Carry forward From which date list
        //public List<SelectListItem> GetLeaveLapsOrCarryFoewardStatusList()
        //{
        //    resultList = GetEmptyListWithPleaseSelect();
        //    try
        //    {
        //        resultList.Add(new SelectListItem() { Text = "Laps", Value = "L" });
        //        resultList.Add(new SelectListItem() { Text = "Carry Forward", Value = "C" });
        //        resultList.Add(new SelectListItem() { Text = "N/A", Value = "N" });
        //    }
        //    catch (Exception e)
        //    {
        //        throw;
        //    }
        //    return resultList;
        //}
        //// get male female and both gender list for leave
        //public List<SelectListItem> GetMaleFemaleAndBothGenderList()
        //{
        //    resultList = GetEmptyListWithPleaseSelect();
        //    try
        //    {
        //        resultList.Add(new SelectListItem() { Text = "Both", Value = "B" });
        //        resultList.Add(new SelectListItem() { Text = "Male", Value = "M" });
        //        resultList.Add(new SelectListItem() { Text = "Female", Value = "F" });
        //    }
        //    catch (Exception e)
        //    {
        //        throw;
        //    }
        //    return resultList;
        //}

        ////get office wise all employee of a department
        //public List<SelectListItem> GetOfficeAndDepartmentWiseEmployeeList(int OfficeId, int DepartmentId)
        //{
        //    resultList = GetEmptyListWithPleaseSelect();
        //    try
        //    {
        //        var employeeList = commonDropDownService.GetOfficeAndDepartmentWiseEmployeeList(OfficeId, DepartmentId);
        //        var ViewOfficeList = employeeList.AsEnumerable().Select(row => new SelectListItem()
        //        {
        //            Value = row.Id.ToString(),
        //            Text = row.NameOther + "-" + row.Name
        //        }).ToList();
        //        resultList.AddRange(ViewOfficeList);
        //    }
        //    catch (Exception e)
        //    {
        //        throw;
        //    }
        //    return resultList;
        //}
        //// get education certificate type
        //public List<SelectListItem> GetEducationCertificateTypeList()
        //{
        //    resultList = GetEmptyListWithPleaseSelect();
        //    try
        //    {
        //        resultList.Add(new SelectListItem() { Text = "Provisional", Value = "Provisional" });
        //        resultList.Add(new SelectListItem() { Text = "Original", Value = "Original" });

        //    }
        //    catch (Exception e)
        //    {
        //        throw;
        //    }
        //    return resultList;
        //}
        //// get employee relation type list
        //public List<SelectListItem> GetRelationTypeList()
        //{
        //    resultList = GetEmptyListWithPleaseSelect();
        //    try
        //    {
        //        resultList.Add(new SelectListItem() { Text = "Father", Value = "F" });
        //        resultList.Add(new SelectListItem() { Text = "Mother", Value = "M" });
        //        resultList.Add(new SelectListItem() { Text = "Wife", Value = "W" });
        //        resultList.Add(new SelectListItem() { Text = "Husband", Value = "H" });
        //        resultList.Add(new SelectListItem() { Text = "Son", Value = "S" });
        //        resultList.Add(new SelectListItem() { Text = "Daughter", Value = "D" });
        //        resultList.Add(new SelectListItem() { Text = "Brother", Value = "Br" });
        //        resultList.Add(new SelectListItem() { Text = "Sister", Value = "Sis" });
        //    }
        //    catch (Exception e)
        //    {
        //        throw;
        //    }
        //    return resultList;
        //}
        //// get blood group type list
        //public List<SelectListItem> GetAllBloodGroupTypeList()
        //{
        //    resultList = GetEmptyListWithPleaseSelect();
        //    try
        //    {
        //        resultList.Add(new SelectListItem() { Text = "A+", Value = "A+" });
        //        resultList.Add(new SelectListItem() { Text = "A-", Value = "A-" });
        //        resultList.Add(new SelectListItem() { Text = "B+", Value = "B+" });
        //        resultList.Add(new SelectListItem() { Text = "B-", Value = "B-" });
        //        resultList.Add(new SelectListItem() { Text = "AB+", Value = "AB+" });
        //        resultList.Add(new SelectListItem() { Text = "AB-", Value = "AB-" });
        //        resultList.Add(new SelectListItem() { Text = "O+", Value = "O+" });
        //        resultList.Add(new SelectListItem() { Text = "O-", Value = "O-" });
        //        resultList.Add(new SelectListItem() { Text = "Unknown", Value = "U" });

        //    }
        //    catch (Exception e)
        //    {
        //        throw;
        //    }
        //    return resultList;
        //}
    }
}
