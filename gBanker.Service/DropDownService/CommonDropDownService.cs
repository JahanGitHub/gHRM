using gHRM.Data.CodeFirstMigration;
using gHRM.Service.StoreProcedure;
using NUPMS.Service.DropDownService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using gHRM.Data.CodeFirstMigration.Payroll;
using gHRM.Service.ViewsEmployee;

namespace gHRM.Service.DropDownService
{
    public class CommonDropDownService
    {
        private gHRMDBContext db = new gHRMDBContext();
        private EmployeeSPService spService = new EmployeeSPService();

        //If project then Project Departments otherwise HO departments list
        public IEnumerable<DropDownAttribute> GetDepartmentByOfficeTypeId(int OfficeTypeId)
        {

            if (OfficeTypeId != 3)
            {
                OfficeTypeId = 1;
            }
            var list = db.EmployeeDepartments.Where(x => x.OfficeTypeId == OfficeTypeId && x.IsActive == true)
                .Select(a => new DropDownAttribute
                {
                    Id = a.DepartmentId,
                    Name = a.DepartmentName
                });
            return list;
        }
        //Office type list
        public IEnumerable<DropDownAttribute> GetOfficeTypeList()
        {
            var list = db.OfficeTypes.Where(x => x.IsActive == true)
                .Select(a => new DropDownAttribute
                {
                    Id = a.OfficeTypeId,
                    Name = a.OfficeTypeName
                });
            return list;
        }
        // Head office list
        public IEnumerable<DropDownAttribute> GetHeadOfficeList()
        {
            var list = db.Offices.Where(x => x.OfficeTypeId == 1 && x.IsActive == true)
                .Select(a => new DropDownAttribute
                {
                    Id = a.OfficeId,
                    Name = a.OfficeName
                });
            return list;
        }
        //Project Office list
        public IEnumerable<DropDownAttribute> GetProjectOfficeList()
        {
            var list = db.Offices.Where(x => x.OfficeTypeId == 3 && x.IsActive == true)
               .Select(a => new DropDownAttribute
               {
                   Id = a.OfficeId,
                   Name = a.OfficeName
               });
            return list;
        }
        //Zone office list
        public IEnumerable<DropDownAttribute> GetZoneOfficeList()
        {
            var list = db.Offices.Where(x => x.OfficeTypeId == 4 && x.IsActive == true)
                .Select(a => new DropDownAttribute
                {
                    Id = a.OfficeId,
                    Name = a.OfficeName
                });
            return list;
        }
        //Area office by zone office id
        public IEnumerable<DropDownAttribute> GetAreaOfficeListByZoneId(int ZoneId)
        {
            var zoneCode = db.Offices.Where(x => x.OfficeId == ZoneId && x.IsActive == true).First().OfficeCode.Trim();
            var list = db.Offices.Where(x => x.OfficeTypeId == 5 && x.SecondLevel.Trim() == zoneCode && x.IsActive == true)
               .Select(a => new DropDownAttribute
               {
                   Id = a.OfficeId,
                   Name = a.OfficeName
               });
            return list;
        }
        //Unit office by Area office id
        public IEnumerable<DropDownAttribute> GetUnitOfficeListByAreaId(int AreaId)
        {
            var areaCode = db.Offices.Where(x => x.OfficeId == AreaId && x.IsActive == true).First().OfficeCode.Trim();
            var list = db.Offices.Where(x => x.OfficeTypeId == 6 && x.ThirdLevel.Trim() == areaCode && x.IsActive == true)
               .Select(a => new DropDownAttribute
               {
                   Id = a.OfficeId,
                   Name = a.OfficeName
               });
            return list;
        }
        //Get all Office Designation List
        public IEnumerable<DropDownAttribute> GetAllOfficeDesignationList()
        {
            var list = db.OfficeDesignations.Where(x => x.IsActive == true)
                .OrderBy(x => x.OffcDesignName)
              .Select(a => new DropDownAttribute
              {
                  Id = a.OfficeDesignationId,
                  Name = a.OffcDesignName
              });
            return list;
        }

        public IEnumerable<DropDownAttribute> GetAllOfficeDesignationList_SP()
        {
            var list = spService.GetDataWithoutParameter("prl.SP_Get_OfficeEmployeeDesignationList");
            var employeeoffDesgList = list.Tables[0].AsEnumerable().Select(row => new DropDownAttribute()
            {
                Id = row.Field<int>("OfficeDesignationId"),
                Name = row.Field<string>("OffcDesignName")
            }).ToList();
            return employeeoffDesgList;
        }


        #region Office
        public IEnumerable<DropDownAttribute> OfficeLocationList()
        {
            var list = db.OfficeLocation.Where(x => x.IsActive == true)
             .Select(a => new DropDownAttribute
             {
                 Id = a.OfficeLocationId,
                 Name = a.OfficeLocationName

             });
            return list;
        }
        #endregion


        public IEnumerable<DropDownAttribute> GetEmployeeStatusList(bool? IsValid = null)
        {
            var list = db.EmployeeStatus.Where(b => b.IsActive == true);
            if (IsValid != null)
            {
                list = list.Where(b => b.IsValid == IsValid);
            }
            var returnList = list.OrderBy(b => b.ViewOrder).Select(b => new DropDownAttribute()
            {
                Id = b.StatusId,
                Name = b.StatusName,
                NameOther = b.StatusValue
            });

            return returnList;
        }

        //Get Office list by office type
        public IEnumerable<DropDownAttribute> GetOfficeTypeWiseOfficeList(int officeTypeId)
        {
            var list = db.Offices.Where(x => x.IsActive == true && x.OfficeTypeId == officeTypeId)
               .Select(a => new DropDownAttribute
               {
                   Id = a.OfficeId,
                   Name = a.OfficeName
               });
            return list;
        }

        //problem
        public IEnumerable<DropDownAttribute> GetEmployeeListByOffice_Department_OfficeDesignation(int OfficeId, int DepartmentId, int OfficeDesignationId)
        {
            string[] EmpStatus = { "A", "PR", "EPR", "CNT" };

            var param = new { OfficeId = OfficeId, DepartmentId, OfficeDesignationId = OfficeDesignationId };
            var EmpList = spService.GetDataWithParameter(param, "emp.SP_GetEmployeeListByOffice_Department_OfficeDesignation");
            var list = EmpList.Tables[0].AsEnumerable()
                .Select(row => new DropDownAttribute
                {
                    Id = Convert.ToInt32(row.Field<long>("EmployeeId")),
                    Name = row.Field<string>("EmployeeCode") + "-" + row.Field<string>("EmployeeName")
                }).ToList();
            return list;
        }

        public IEnumerable<DropDownAttribute> GetAllActiveDeptList()
        {
            var list = db.EmployeeDepartments.Where(x => x.IsActive == true)
               .Select(a => new DropDownAttribute
               {
                   Id = a.DepartmentId,
                   Name = a.DepartmentName
               });
            return list;
        }

        public IEnumerable<DropDownAttribute> GetLeaveTypeListByEmployeeStatusAndGender(string EmployeeGender, int EmployeeStatusId)
        {
            var genderNotLike = "";
            if (EmployeeGender == "M")
            {
                genderNotLike = "F";
            }
            else
            {
                genderNotLike = "M";
            }

            var list = db.LeaveTypes.Where(x => x.IsActive == true && x.EmployeeStatusId == EmployeeStatusId && x.LeaveGender != genderNotLike)
             .Select(a => new DropDownAttribute
             {
                 Id = a.LeaveTypeId,
                 Name = a.LeaveTypeName,
                 NameOther = a.LeaveCategory
             });
            return list;
        }

        public IEnumerable<DropDownAttribute> GetAllLeaveCategoryList()
        {
            var list = db.LeaveCategory.Where(x => x.IsActive == true)
                .Select(a => new DropDownAttribute
                {
                    Name = a.Value,
                    NameOther = a.Detail
                });
            return list;
        }




        public IEnumerable<DropDownAttribute> GetOfficeAndDepartmentWiseEmployeeList(int OfficeId, int DepartmentId)
        {
            var param = new { OfficeId = OfficeId, DepartmentId = DepartmentId };
            var EmpList = spService.GetDataWithParameter(param, "emp.SP_GET_AllEmployeeByDepartmentandOffice");
            var list = EmpList.Tables[0].AsEnumerable()
                .Select(row => new DropDownAttribute
                {
                    Id = Convert.ToInt32(row.Field<long>("EmployeeId")),
                    Name = row.Field<string>("EmployeeName"),
                    NameOther = row.Field<string>("EmployeeCode")
                }).ToList();
            return list;
        }
        public IEnumerable<DropDownAttribute> GetEmployeeGradeList()
        {
            var list = spService.GetDataWithoutParameter("prl.SP_Get_EmployeeGradeList");
            var employeeGradeList = list.Tables[0].AsEnumerable().Select(row => new DropDownAttribute()
            {
                Id = row.Field<int>("GradeId"),
                Name = row.Field<string>("Grade")
            }).ToList();
            return employeeGradeList;
        }

        public IEnumerable<DropDownAttribute> GetAllPayrollDesignationList()
        {
            var list = db.EmployeeDesignations.Where(x => x.IsActive == true)
              .Select(a => new DropDownAttribute
              {
                  Id = a.DesignationId,
                  Name = a.DesignationName
              });
            return list;
        }
        public IEnumerable<DropDownAttribute> GetSalaryStepXGrade(int? gradeid, int? step)
        {
            List<DropDownAttribute> lst = new List<DropDownAttribute>();
            var lstObj = db.GradeXSalarySteps.Where(x => x.IsActive && x.GradeId == (gradeid ?? 0))
                .GroupBy(t => 1)
                .Select(a => new { min = a.Min(p => p.StepFrom), max = a.Max(p => p.StepTo) });
            if (lstObj.Any())
                for (int y = lstObj.First().min; y <= lstObj.First().max; y++)
                    lst.Add(new DropDownAttribute { Id = y, Name = y.ToString(), DefaultValue = (step ?? 0) });
                    //lst.Add(new DropDownAttribute { Id = y, Name = (y + (y == 1 ? "st" : y == 2 ? "nd" : y == 3 ? "rd" : "th") + " Step").ToString(), DefaultValue = (step ?? 0) });
            return lst;
        }
        public IEnumerable<DropDownAttribute> GetAllPayrollDesignationList_SP()
        {
            var list = spService.GetDataWithoutParameter("prl.SP_Get_EmployeeDesignationList");
            var employeeDesgList = list.Tables[0].AsEnumerable().Select(row => new DropDownAttribute()
            {
                Id = row.Field<int>("DesignationId"),
                Name = row.Field<string>("DesignationName")
            }).ToList();
            return employeeDesgList;
        }


        public IEnumerable<DropDownAttribute> GetEducationDegreeList()
        {
            var list = db.EducationDegree.Where(x => x.IsActive == true)
              .Select(a => new DropDownAttribute
              {
                  Id = a.DegreeId,
                  Name = a.DegreeCode,
                  NameOther = a.DegreeName
              });
            return list;
        }

        public IEnumerable<DropDownAttribute> GetEducationConcentrationListByDegreeCode(string degreeCode)
        {
            var list = db.EducationConcentration.Where(x => x.IsActive == true && x.DegreeCode == degreeCode)
              .Select(a => new DropDownAttribute
              {
                  Id = a.ConcentrationId,
                  Name = a.ConcentrationCode,
                  NameOther = a.ConcentrationName
              });
            return list;
        }

        #region Employee
        public IEnumerable<DropDownAttribute> EmployeeTypeConfig()
        {
            var empTypeConfig = spService.GetDataWithoutParameter("cmm.SP_Get_EmployeeTypeConfig");
            var view_empTypeConfig = empTypeConfig.Tables[0].AsEnumerable()
                .Select(row => new DropDownAttribute()
                {
                    Id = row.Field<int>("EmployeeTypeId"),
                    Name = row.Field<string>("EmployeeTypeName"),

                }).ToList();
            return view_empTypeConfig;
        }


        #endregion Employee

        #region Payroll
        /// <summary>
        /// Other Id = order by View Order
        /// </summary>
        /// <returns></returns>
        public IEnumerable<EmployeeStatusServiceModel> GetAllSalaryStatus()
        {
            var list = db.EmployeeStatus.Where(b => b.IsActive == true && b.IsSalaryApplicable == true).OrderBy(b => b.ViewOrder);
            var rList = list.Select(b => new EmployeeStatusServiceModel()
            {
                StatusId = b.StatusId,
                StatusName = b.StatusName,
                StatusValue = b.StatusValue,
                ViewOrder = b.ViewOrder,
                IsValid = b.IsValid,
                IsSalaryApplicable = b.IsSalaryApplicable

            });
            return rList;
        }
        /// <summary>
        /// True = Salary Applicable Status
        /// False =Not Applicable Status
        /// </summary>
        /// <param name="StatusId"></param>
        /// <returns></returns>
        public bool IsSalaryApplicableForThisStatus(int StatusId)
        {
            var list = db.EmployeeStatus.Where(b => b.IsActive == true && b.IsSalaryApplicable == true
            && b.StatusId == StatusId);
            if (list != null && list.Count() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        public IEnumerable<DropDownAttribute> PayrollComponent()
        {
            var list = db.ComponentPayroll.Where(x => x.IsActive == true).Select(x => new DropDownAttribute()
            {
                Id = x.Id,
                Name = x.ComponentName,
                NameOther = x.ComponentCategory
            }).ToList();
            return list;
        }

        public IEnumerable<DropDownAttribute> PayrollComponentXPRComponent()
        {
            var ids=db.PRComponents.Where(x => x.IsActive).Select(x => x.ComponentPayrollId).Distinct().ToArray();
            if (ids != null)
            {
                var list = db.ComponentPayroll.Where(x => x.IsActive == true && ids.Contains(x.Id)).Select(x => new DropDownAttribute()
                {
                    Id = x.Id,
                    Name = x.ComponentName,
                    NameOther = x.ComponentCategory
                }).ToList();
                return list;
            }
            else  return null;
        }

        public IEnumerable<DropDownAttribute> PayrollComponentIgnoreByCategory(List<string> IgnoreList)
        {
            var list = db.ComponentPayroll.Where(x => x.IsActive == true && !IgnoreList.Contains(x.ComponentCategory)).Select(x => new DropDownAttribute()
            {
                Id = x.Id,
                Name = x.ComponentName,
                NameOther = x.ComponentCategory
            }).ToList();
            return list;
        }
        public IEnumerable<DropDownAttribute> PayrollComponentContainByCategory(List<string> ContainList)
        {
            var list = db.ComponentPayroll.Where(x => x.IsActive == true && ContainList.Contains(x.ComponentCategory)).Select(x => new DropDownAttribute()
            {
                Id = x.Id,
                Name = x.ComponentName,
                NameOther = x.ComponentCategory
            }).ToList();
            return list;
        }

        public IEnumerable<DropDownAttribute> PRComponentGroup_Only_SalaryOrDeduction()
        {
            var list = db.PRComponentGroup.Where(b => b.IsActive == true && (b.PRComponentGroupID == 1 || b.PRComponentGroupID == 6))
                .Select(b => new DropDownAttribute() { Id = b.PRComponentGroupID, Name = b.ComponentGroupName, NameOther = b.ComponentGroupShortName });
            return list;
        }

        public IEnumerable<DropDownAttribute> PayrollComponentName(string value)
        {

            var list = new List<DropDownAttribute>();
            if (value == "Loan")
                list = db.LoanPurposes.Where(x => x.IsActive == true).Select(x => new DropDownAttribute()
                {
                    Id = x.PurposeId,
                    Name = x.PurposeName,
                    NameOther = x.PurposeName
                }).ToList();
            else
                list = db.ComponentPayroll.Where(x => x.IsActive == true && x.ComponentCategory == value).Select(x => new DropDownAttribute()
                {
                    Id = x.Id,
                    Name = x.ComponentName,
                    NameOther = x.ComponentCategory
                }).ToList();

            return list;
        }
        #endregion Payroll

        #region Loan
        public IEnumerable<DropDownAttribute> loanCalculationList()
        {
            var list = db.prlLoanCalculation.Where(x => x.IsActive == true).Select(x => new DropDownAttribute()
            {
                Id = x.LoanCalculationId,
                Name = x.LoanCalculationName

            }).ToList();
            return list;
        }
        #endregion

        #region Bank Name DropDown With Code
        public IEnumerable<DropDownAttribute> PayrollBankNameWithCode()
        {
            var list = db.BankName.Where(x => x.IsActive == true).Select(x => new DropDownAttribute()
            {
                NameOther = x.BankCode,
                Name = x.BankFullName

            }).ToList();
            return list;
        }
        #endregion


        #region Payroll Product Group
        public IEnumerable<DropDownAttribute> GetPayrollProductGroup()
        {
            var list = db.ProductGroup.Where(x => x.IsActive == true).Select(x => new DropDownAttribute()
            {
                Id = x.ProductGroupId,
                Name = x.ProductGroupName

            }).ToList();
            return list;
        }
        public IEnumerable<DropDownAttribute> GetPayrollGroupWiseProductType(int? PayrollGroupProductId)
        {
            var dblist = db.ProductType.Where(x => x.IsActive == true);
            if (PayrollGroupProductId.HasValue == true && PayrollGroupProductId.Value > 0)
            {
                dblist = dblist.Where(b => b.ProductGroupId == PayrollGroupProductId.Value);
            }
            var rList = dblist.Select(x => new DropDownAttribute()
            {
                Id = x.ProductTypeId,
                Name = x.ProductTypeName

            }).ToList();
            return rList;
        }
        public IEnumerable<DropDownAttribute> GetPayrollProductItem()
        {
            var dblist = db.ProductItem.Where(x => x.IsActive == true);

            var rList = dblist.Select(x => new DropDownAttribute()
            {
                Id = x.ProductId,
                Name = x.ProductItemName

            }).ToList();
            return rList;
        }

        //public IEnumerable<DropDownAttribute> GetPayrollProductItemByProductGroupId(int ProductGroupId)
        //{
        //    var dblist = db.ProductItem.Where(x => x.IsActive == true && x.ProductGroupId== ProductGroupId);

        //    var rList = dblist.Select(x => new DropDownAttribute()
        //    {
        //        Id = x.ProductId,
        //        Name = x.ProductItemName

        //    }).ToList();
        //    return rList;
        //}

        public IEnumerable<DropDownAttribute> GetPayrollProductItemByProductTypeId(int ProductTypeId)
        {
            var dblist = db.ProductItem.Where(x => x.IsActive == true && x.ProductTypeId == ProductTypeId);

            var rList = dblist.Select(x => new DropDownAttribute()
            {
                Id = x.ProductId,
                Name = x.ProductItemName

            }).ToList();
            return rList;
        }

        public IEnumerable<DropDownAttribute> GetPromotionTypeList()
        {
            var dblist = db.PromotionType.Where(x => x.IsActive == true);

            var rList = dblist.Select(x => new DropDownAttribute()
            {
                Id = x.PromotionTypeId,
                Name = x.PromotionTypeName

            }).ToList();
            return rList;
        }

        // Nobin
        public IEnumerable<DropDownAttribute> GetCompanyBankListForNobin()
        {
            return db.Database.SqlQuery<DropDownAttribute>("exec sp_CompanyBankInfoForNEP").ToList();
        }

        //public IEnumerable<DropDownAttribute> GetPayrollProductItemByProductItemAndGroup(int ProductGroupId, int ProductTypeId)
        //{
        //    var dblist = db.ProductItem.Where(x => x.IsActive == true && x.ProductGroupId==ProductGroupId && x.ProductTypeId == ProductTypeId);

        //    var rList = dblist.Select(x => new DropDownAttribute()
        //    {
        //        Id = x.ProductId,
        //        Name = x.ProductItemName

        //    }).ToList();
        //    return rList;
        //}
        #endregion


        #region Role Wise Child Menu
        public DataTable getRoleWiseChildMenu(int? Role = 0)
        {
            var parm = new { RoleId = Role };
            var list = spService.GetDataWithParameter(parm, "SP_GET_Rolewise_Child_Menu");
            var employeeGradeList = list.Tables[0];

            return employeeGradeList;
        }
        #endregion



    }
}
