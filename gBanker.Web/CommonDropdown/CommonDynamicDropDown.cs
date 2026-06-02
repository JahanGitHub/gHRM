using gHRM.Core.Utilities.Constants;
using gHRM.Data.CodeFirstMigration;
using gHRM.Service.DropDownService;
using gHRM.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;

namespace gHRM.Web.CommonDropdown
{
    public class CommonDynamicDropDown
    {
        private CommonDropDownService commonDropDownService = new CommonDropDownService();

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

        #region Security

        public List<AspNetSecurityModule> getRoleWiseChildMenu(int? Role = 0)
        {
            var list = commonDropDownService.getRoleWiseChildMenu(Role).AsEnumerable().Select(row => new AspNetSecurityModule
            {
                AspNetSecurityModuleId = Convert.ToInt32(row.Field<int>("AspNetSecurityModuleId")),
                SecurityModuleCode = row.Field<string>("SecurityModuleCode"),
                LinkText = row.Field<string>("LinkText"),
                ControllerName = row.Field<string>("ControllerName"),
                ActionName = row.Field<string>("ActionName"),
                ParentModuleId = row.Field<int?>("ParentModuleId"),
                IsActive = row.Field<bool?>("IsActive"),
                IsMenuItem = row.Field<bool?>("IsMenuItem"),
                RoleId = Convert.ToInt32(row.Field<string>("RoleId")),
            });
            return list.ToList();
        }
        #endregion

        #region Basic

        public List<SelectListItem> ddlEmployeeType()
        {
            var list = commonDropDownService.EmployeeTypeConfig()
                .Select(b => new SelectListItem() { Value = b.Id.ToString(), Text = b.Name });

            var initial = ddlInitial();
            initial.AddRange(list);
            return initial;

        }

        //Employee Status List all
        public List<SelectListItem> ddlEmployeeStatusList(bool? IsValid = null)
        {
            var resultList = ddlInitial();
            try
            {
                var employeeStatusList = commonDropDownService.GetEmployeeStatusList(IsValid)
                    .Select(obj => new SelectListItem
                    {
                        Text = obj.Name,
                        Value = obj.Id.ToString()
                    });
                resultList.AddRange(employeeStatusList);
            }
            catch (Exception e)
            {
                throw;
            }
            return resultList;
        }


        public IEnumerable<SelectListItem> OfficeLocationList(bool? isAddPleaseSelect = true)
        {
            var list = commonDropDownService.OfficeLocationList()
                .Select(b => new SelectListItem() { Value = b.Id.ToString(), Text = b.Name });

            var initial = ddlInitial(false);
            initial.AddRange(list);
            return initial;
        }


        //If project then Project Departments otherwise HO departments
        public List<SelectListItem> GetDepartmentByOfficeTypeId(int OfficeTypeId)
        {
            var resultList = ddlInitial();
            try
            {
                var list = commonDropDownService.GetDepartmentByOfficeTypeId(OfficeTypeId)
                    .Select(obj => new SelectListItem
                    {
                        Text = obj.Name,
                        Value = obj.Id.ToString()
                    });
                resultList.AddRange(list);
            }
            catch (Exception e)
            {
                throw;
            }
            return resultList;
        }

        //Office type list
        public List<SelectListItem> GetOfficeTypeList()
        {
            var resultList = ddlInitial();
            try
            {
                var officeTypeList = commonDropDownService.GetOfficeTypeList()
                    .Select(obj => new SelectListItem
                    {
                        Text = obj.Name,
                        Value = obj.Id.ToString()
                    });
                resultList.AddRange(officeTypeList);
            }
            catch (Exception e)
            {
                throw;
            }
            return resultList;
        }

        //Head office list
        public List<SelectListItem> GetHeadOfficeList()
        {
            var resultList = ddlInitial();
            try
            {
                var headOfficeList = commonDropDownService.GetHeadOfficeList()
                    .Select(obj => new SelectListItem
                    {
                        Text = obj.Name,
                        Value = obj.Id.ToString()
                    });
                resultList.AddRange(headOfficeList);
            }
            catch (Exception e)
            {
                throw;
            }
            return resultList;
        }

        //Project office list
        public List<SelectListItem> GetProjectOfficeList()
        {
            var resultList = ddlInitial();
            try
            {
                var headOfficeList = commonDropDownService.GetProjectOfficeList()
                    .Select(obj => new SelectListItem
                    {
                        Text = obj.Name,
                        Value = obj.Id.ToString()
                    });
                resultList.AddRange(headOfficeList);
            }
            catch (Exception e)
            {
                throw;
            }
            return resultList;
        }

        //Zone office list
        public List<SelectListItem> GetZoneOfficeList()
        {
            var resultList = ddlInitial();
            try
            {
                var zoneOfficeList = commonDropDownService.GetZoneOfficeList()
                    .Select(obj => new SelectListItem
                    {
                        Text = obj.Name,
                        Value = obj.Id.ToString()
                    });
                resultList.AddRange(zoneOfficeList);
            }
            catch (Exception e)
            {
                throw;
            }
            return resultList;
        }

        //Area office list by Zone office
        public List<SelectListItem> GetAreaOfficeListByZoneId(int ZoneId)
        {
            var resultList = ddlInitial();
            try
            {
                var areaOfficeList = commonDropDownService.GetAreaOfficeListByZoneId(ZoneId)
                    .Select(obj => new SelectListItem
                    {
                        Text = obj.Name,
                        Value = obj.Id.ToString()
                    });
                resultList.AddRange(areaOfficeList);
            }
            catch (Exception e)
            {
                throw;
            }
            return resultList;
        }
        //Unit/Branch office list
        public List<SelectListItem> GetUnitOfficeListByAreaId(int AreaId)
        {
            var resultList = ddlInitial();
            try
            {
                var unitOfficeList = commonDropDownService.GetUnitOfficeListByAreaId(AreaId)
                    .Select(obj => new SelectListItem
                    {
                        Text = obj.Name,
                        Value = obj.Id.ToString()
                    });
                resultList.AddRange(unitOfficeList);
            }
            catch (Exception e)
            {
                throw;
            }
            return resultList;
        }

        //All Employee Designations/ Payroll designations list
        public List<SelectListItem> GetAllPayrollDesignationList()
        {
            var resultList = ddlInitial();
            try
            {
                var payrollDesignationList = commonDropDownService.GetAllPayrollDesignationList()
                    .Select(obj => new SelectListItem
                    {
                        Text = obj.Name,
                        Value = obj.Id.ToString()
                    });
                resultList.AddRange(payrollDesignationList);
            }
            catch (Exception e)
            {
                throw;
            }
            return resultList;
        }

        public List<SelectListItem> GetSalaryStepXGrade(int? gradeid, int? step)
        {
            var resultList = ddlInitial();
            try
            {
                var lst = commonDropDownService.GetSalaryStepXGrade(gradeid, step)
                    .Select(obj => new SelectListItem
                    {
                        Text = obj.Name,
                        Value = obj.Id.ToString()
                    });
                resultList.AddRange(lst);
            }
            catch (Exception e)
            {
                throw;
            }
            return resultList;
        }
        public List<SelectListItem> GetAllPayrollDesignationList_SP()
        {
            var resultList = ddlInitial();
            try
            {
                var payrollDesignationList = commonDropDownService.GetAllPayrollDesignationList_SP()
                    .Select(obj => new SelectListItem
                    {
                        Text = obj.Name,
                        Value = obj.Id.ToString()
                    });
                resultList.AddRange(payrollDesignationList);
            }
            catch (Exception e)
            {
                throw;
            }
            return resultList;
        }



        //All Office Designations/ Ranks/ Ornamental designations list
        public List<SelectListItem> GetAllOfficeDesignationList()
        {
            var resultList = ddlInitial();
            try
            {
                var officeDesignationList = commonDropDownService.GetAllOfficeDesignationList()
                    .Select(obj => new SelectListItem
                    {
                        Text = obj.Name,
                        Value = obj.Id.ToString()
                    });
                resultList.AddRange(officeDesignationList);
            }
            catch (Exception e)
            {
                throw;
            }
            return resultList;
        }

        public List<SelectListItem> GetAllOfficeDesignationList_SP()
        {
            var resultList = ddlInitial();
            try
            {
                var officeDesignationList = commonDropDownService.GetAllOfficeDesignationList_SP()
                    .Select(obj => new SelectListItem
                    {
                        Text = obj.Name,
                        Value = obj.Id.ToString()
                    });
                resultList.AddRange(officeDesignationList);
            }
            catch (Exception e)
            {
                throw;
            }
            return resultList;
        }

        //get office type wise office list
        public List<SelectListItem> GetOfficeTypeWiseOfficeList(int officeTypeId)
        {
            var resultList = ddlInitial();
            try
            {
                var officeList = commonDropDownService.GetOfficeTypeWiseOfficeList(officeTypeId);//officeService.GetOfficeByType(officeTypeId);
                var ViewOfficeList = officeList.AsEnumerable().Select(row => new SelectListItem()
                {
                    Value = row.Id.ToString(),
                    Text = row.Name
                }).ToList();
                resultList.AddRange(ViewOfficeList);
            }
            catch (Exception e)
            {
                throw;
            }
            return resultList;
        }
        //get employee list by officeId departmentId and employeeRank/officeDesignation/ornamentalDesignation


        //Get all active Department list
        public List<SelectListItem> GetAllActiveDepartmentList()
        {
            var resultList = ddlInitial();
            try
            {
                var list = commonDropDownService.GetAllActiveDeptList()
                    .Select(obj => new SelectListItem
                    {
                        Text = obj.Name,
                        Value = obj.Id.ToString()
                    });
                resultList.AddRange(list);
            }
            catch (Exception e)
            {
                throw;
            }
            return resultList;
        }
        //get office wise all employee of a department
        public List<SelectListItem> GetOfficeAndDepartmentWiseEmployeeList(int OfficeId, int DepartmentId)
        {
            var resultList = ddlInitial();
            try
            {
                var employeeList = commonDropDownService.GetOfficeAndDepartmentWiseEmployeeList(OfficeId, DepartmentId);
                var ViewOfficeList = employeeList.AsEnumerable().Select(row => new SelectListItem()
                {
                    Value = row.Id.ToString(),
                    Text = row.NameOther + "-" + row.Name
                }).ToList();
                resultList.AddRange(ViewOfficeList);
            }
            catch (Exception e)
            {
                throw;
            }
            return resultList;
        }

        public IEnumerable<SelectListItem> GetEmployeeGradeList()
        {
            var initial = ddlInitial();
            var EmpGrade = commonDropDownService.GetEmployeeGradeList().Select(b => new SelectListItem()
            {
                Value = b.Id.ToString(),
                Text = b.Name
            });

            initial.AddRange(EmpGrade);
            return initial;
        }


        public List<SelectListItem> GetEducationDegreeList()
        {
            var resultList = ddlInitial();
            try
            {
                var employeeList = commonDropDownService.GetEducationDegreeList();
                var ViewOfficeList = employeeList.AsEnumerable().Select(row => new SelectListItem()
                {
                    Value = row.Name,
                    Text = row.NameOther
                }).ToList();
                resultList.AddRange(ViewOfficeList);
            }
            catch (Exception e)
            {
                throw;
            }
            return resultList;
        }

        public List<SelectListItem> GetEducationConcentrationListByDegreeCode(string degreeCode)
        {
            var resultList = ddlInitial();
            try
            {
                var employeeList = commonDropDownService.GetEducationConcentrationListByDegreeCode(degreeCode);
                var ViewOfficeList = employeeList.AsEnumerable().Select(row => new SelectListItem()
                {
                    Value = row.Name,
                    Text = row.NameOther
                }).ToList();
                resultList.AddRange(ViewOfficeList);
            }
            catch (Exception e)
            {
                throw;
            }
            return resultList;
        }



        // Grade List 



        #endregion

        #region Employee

        public List<SelectListItem> GetEmployeeListByOffice_Department_OfficeDesignation(int OfficeId, int DepartmentId, int OfficeDesignationId)
        {
            var resultList = ddlInitial();
            try
            {
                var employeeList = commonDropDownService.GetEmployeeListByOffice_Department_OfficeDesignation(OfficeId, DepartmentId, OfficeDesignationId);
                var ViewOfficeList = employeeList.AsEnumerable().Select(row => new SelectListItem()
                {
                    Value = row.Id.ToString(),
                    Text = row.Name
                }).ToList();
                resultList.AddRange(ViewOfficeList);
            }
            catch (Exception e)
            {
                throw;
            }
            return resultList;
        }


        #endregion

        #region leave

        //get all active leave Category list
        public List<SelectListItem> GetAllLeaveCategoryList()
        {
            var resultList = ddlInitial();
            try
            {
                var list = commonDropDownService.GetAllLeaveCategoryList()
                    .Select(obj => new SelectListItem
                    {
                        Text = obj.NameOther,
                        Value = obj.Name
                    });
                resultList.AddRange(list);
            }
            catch (Exception e)
            {
                throw;
            }
            return resultList;
        }

        //get leave type by employee status and gender
        public List<SelectListItem> GetLeaveTypeListByEmployeeStatusAndGender(string EmployeeGender, int EmployeeStatusId)
        {
            var resultList = ddlInitial();
            try
            {
                var list = commonDropDownService.GetLeaveTypeListByEmployeeStatusAndGender(EmployeeGender, EmployeeStatusId)
                    .Select(obj => new SelectListItem
                    {
                        Text = obj.Name + '-' + obj.NameOther,
                        Value = obj.Id.ToString()
                    });
                resultList.AddRange(list);
            }
            catch (Exception e)
            {
                throw;
            }
            return resultList;
        }

        #endregion

        #region payroll

        public IEnumerable<SelectListItem> PRSalaryRoundType()
        {
            List<SelectListItem> salaryRoundTypes = new List<SelectListItem>();
            salaryRoundTypes.Add(new SelectListItem() { Value = SalaryRoundTypeConstants.NotApplicable, Text = SalaryRoundTypeConstants.GetText(SalaryRoundTypeConstants.NotApplicable) });
            salaryRoundTypes.Add(new SelectListItem() { Value = SalaryRoundTypeConstants.RoundUp, Text = SalaryRoundTypeConstants.GetText(SalaryRoundTypeConstants.RoundUp) });
            salaryRoundTypes.Add(new SelectListItem() { Value = SalaryRoundTypeConstants.RoundDown, Text = SalaryRoundTypeConstants.GetText(SalaryRoundTypeConstants.RoundDown) });
            salaryRoundTypes.Add(new SelectListItem() { Value = SalaryRoundTypeConstants.RoundNormal, Text = SalaryRoundTypeConstants.GetText(SalaryRoundTypeConstants.RoundNormal) });
            return salaryRoundTypes;
        }
        public IEnumerable<SelectListItem> CommoninitialOption()
        {
            return ddlInitial();
        }

        public IEnumerable<SelectListItem> PayrollComponent()
        {
            var initial = ddlInitial();
            var status = commonDropDownService.PayrollComponent().Select(b => new SelectListItem()
            {
                Value = b.Id.ToString(),
                Text = b.Name
            });

            initial.AddRange(status);
            return initial;
        }
        public IEnumerable<SelectListItem> PayrollComponentXPRComponent()
        {
            var initial = ddlInitial();
            var lst = commonDropDownService.PayrollComponentXPRComponent();
            if (lst != null)
            {
                var objLst = lst.Select(b => new SelectListItem()
                {
                    Value = b.Id.ToString(),
                    Text = b.Name
                });
                initial.AddRange(objLst);
            }
            return initial;
        }

        public IEnumerable<SelectListItem> PayrollComponentIgnoreByCategory(List<string> ignorList)
        {
            var initial = ddlInitial();
            var status = commonDropDownService.PayrollComponentIgnoreByCategory(ignorList).Select(b => new SelectListItem()
            {
                Value = b.Id.ToString(),
                Text = b.Name
            });

            initial.AddRange(status);
            return initial;
        }

        public IEnumerable<SelectListItem> PayrollComponentName(string value)
        {
            var initial = ddlInitial();
            var componentNames = commonDropDownService.PayrollComponentName(value).Select(b => new SelectListItem()
            {
                Value = b.Id.ToString(),
                Text = b.Name
            });
            initial.AddRange(componentNames);
            return initial;
        }

        public IEnumerable<SelectListItem> PayrollComponentContainByCategory(List<string> ContainList)
        {
            var initial = ddlInitial();
            var status = commonDropDownService.PayrollComponentContainByCategory(ContainList).Select(b => new SelectListItem()
            {
                Value = b.Id.ToString(),
                Text = b.Name
            });

            initial.AddRange(status);
            return initial;
        }

        public IEnumerable<SelectListItem> PRComponentGroup_Only_SalaryOrDeduction()
        {
            var initial = ddlInitial();
            var list = commonDropDownService.PRComponentGroup_Only_SalaryOrDeduction()
                .Select(b => new SelectListItem() { Text = b.Name, Value = b.Id.ToString() });
            initial.AddRange(list);
            return initial;
        }


        public IEnumerable<SelectListItem> PayrollBankNameWithCode()
        {
            var initial = ddlInitial();
            var list = commonDropDownService.PayrollBankNameWithCode()
                .Select(b => new SelectListItem() { Text = b.Name, Value = b.NameOther });
            initial.AddRange(list);
            return initial;
        }

        public IEnumerable<SelectListItem> GetPayrollProductGroup()
        {
            var initial = ddlInitial();
            var list = commonDropDownService.GetPayrollProductGroup().Select(x => new SelectListItem()
            {
                Value = x.Id.ToString(),
                Text = x.Name

            }).ToList();
            initial.AddRange(list);
            return initial;
        }

        public IEnumerable<SelectListItem> GetPayrollGroupWiseProductType(int? PayrollGroupProductId)
        {
            var initial = ddlInitial();
            var dblist = commonDropDownService.GetPayrollGroupWiseProductType(PayrollGroupProductId).Select(x => new SelectListItem()
            {
                Value = x.Id.ToString(),
                Text = x.Name

            }).ToList();
            initial.AddRange(dblist);
            return initial;
        }

        public IEnumerable<SelectListItem> GetPayrollProductItem()
        {
            var initial = ddlInitial();
            var rList = commonDropDownService.GetPayrollProductItem().Select(x => new SelectListItem()
            {
                Value = x.Id.ToString(),
                Text = x.Name

            }).ToList();
            initial.AddRange(rList);
            return initial;
        }

        //public IEnumerable<SelectListItem> GetPayrollProductItemByProductGroupId(int ProductGroupId)
        //{
        //    var initial = ddlInitial();
        //    var dblist = commonDropDownService.GetPayrollProductItemByProductGroupId(ProductGroupId).Select(x => new SelectListItem()
        //    {
        //        Value = x.Id.ToString(),
        //        Text = x.Name

        //    }).ToList();
        //    initial.AddRange(dblist);
        //    return initial;
        //}

        public IEnumerable<SelectListItem> GetPayrollProductItemByProductTypeId(int ProductTypeId)
        {
            var initial = ddlInitial();
            var dblist = commonDropDownService.GetPayrollProductItemByProductTypeId(ProductTypeId).Select(x => new SelectListItem()
            {
                Value = x.Id.ToString(),
                Text = x.Name

            }).ToList();
            initial.AddRange(dblist);
            return initial;
        }

        //public IEnumerable<SelectListItem> GetPayrollProductItemByProductItemAndGroup(int ProductGroupId, int ProductTypeId)
        //{
        //    var initial = ddlInitial();
        //    var dblist = commonDropDownService.GetPayrollProductItemByProductItemAndGroup(ProductGroupId, ProductTypeId).Select(x => new SelectListItem()
        //    {
        //        Value = x.Id.ToString(),
        //        Text = x.Name

        //    }).ToList();
        //    initial.AddRange(dblist);
        //    return initial;
        //}

        public IEnumerable<EmployeeStatusViewModel> GetAllSalaryStatus()
        {
            var list = commonDropDownService.GetAllSalaryStatus().Select(b => new EmployeeStatusViewModel()
            {
                StatusId = b.StatusId,
                StatusName = b.StatusName,
                StatusValue = b.StatusValue,
                ViewOrder = b.ViewOrder,
                IsSalaryApplicable = b.IsSalaryApplicable
            });
            return list;
        }

        public bool IsSalaryApplicableForThisStatus(int EmployeeStatusId)
        {
            var res = commonDropDownService.IsSalaryApplicableForThisStatus(EmployeeStatusId);
            return res;
        }

        #endregion

        #region Loan

        public IEnumerable<SelectListItem> loanCalculationList()
        {
            var initial = ddlInitial("0");
            var list = commonDropDownService.loanCalculationList()
                .Select(b => new SelectListItem() { Text = b.Name, Value = b.Id.ToString() });
            initial.AddRange(list);
            return initial;
        }

        #endregion


        #region ProvidentFund

        public IEnumerable<SelectListItem> ProvidentFundType()
        {
            var initial = ddlInitial();
            initial.Add(new SelectListItem() { Value = "0", Text = "Not Applicable" });
            initial.Add(new SelectListItem() { Value = "1", Text = "CPF[Contributory Provident Fund]" });
            initial.Add(new SelectListItem() { Value = "2", Text = "GPF[General Provident Fund]" });
            return initial;
        }

        #endregion

        #region Promotion

        public IEnumerable<SelectListItem> PromotionTypeList()
        {
            var dblist = commonDropDownService.GetPromotionTypeList().Select(x => new SelectListItem()
            {
                Value = x.Id.ToString(),
                Text = x.Name

            }).ToList();

            var promotionTypelist = new List<SelectListItem>();
            promotionTypelist.Add(new SelectListItem() { Text = "Please Select", Value = "0" });
            promotionTypelist.AddRange(dblist);
            return promotionTypelist;
        }


        #endregion

        public IEnumerable<SelectListItem> GetCompanyBankListForNobin()
        {
            var initial = ddlInitial();
            var list = commonDropDownService.GetCompanyBankListForNobin()
                .Select(b => new SelectListItem() { Text = b.Name, Value = b.Id.ToString() });
            initial.AddRange(list);
            return initial;
        }
    }
}