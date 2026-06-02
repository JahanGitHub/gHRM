
#region Using

using gHRM.Core.Utilities.Constants;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;

#endregion

namespace gHRM.Web.Infrastructure.Html
{
    public static class DropdownListHelper
    {
        public static IEnumerable<SelectListItem> GetDropdownList(DropdownListTypes type, string selected = "")
        {
            var items = new List<ConstantDropdownItem>();

            switch (type)
            {
                case DropdownListTypes.SalaryCalculationType:
                    items = SalaryCalculationTypeConstants.Items.ToList();
                    break;

                case DropdownListTypes.SalaryAccountTransactionType:
                    items = SalaryAccountTransactionTypeConstants.Items.ToList();
                    break;
                case DropdownListTypes.ProvidentFundType:
                    items = ProvidentFundTypeConstants.Items.ToList();
                    break;
                case DropdownListTypes.ComponentCategory:
                    items = ComponentCategoryConstants.Items.ToList();
                    break;
                case DropdownListTypes.EmployeeStatus:
                    items = EmployeeStatusConstants.Items.ToList();
                    break;
                case DropdownListTypes.SalaryGenerationType:
                    items = EmploymentTypeConstants.Items.ToList();
                    break;
                case DropdownListTypes.SalaryStructureType:
                    items = SalaryStructureTypeConstants.Items.ToList();
                    break;
                case DropdownListTypes.LeaveTypeGender:
                    items = LeaveTypeGenderConstants.Items.ToList();
                    break;
                case DropdownListTypes.LeaveStatus:
                    items = LeaveStatusConstants.Items.ToList();
                    break;
                case DropdownListTypes.AttendanceTerminal:
                    items = AttendanceTerminalConstants.Items.ToList();
                    break;
                case DropdownListTypes.LeaveAdjustType:
                    items = LeaveAdjustTypeConstants.Items.ToList();
                    break;
                case DropdownListTypes.LeaveReason:
                    items = LeaveReasonConstants.Items.ToList();
                    break;
                case DropdownListTypes.EmailNotificationType:
                    items = EmailNotificationTypeConstants.Items.ToList();
                    break;
                case DropdownListTypes.LeaveCategory:
                    items = LeaveCategoryConstants.Items.ToList();
                    break;
                case DropdownListTypes.EmployeeUser:
                    items = EmployeeUserConstants.Items.ToList();
                    break;
                case DropdownListTypes.UserType:
                    items = UserTypeConstants.Items.ToList();
                    break;
                case DropdownListTypes.LeaveImportSheet:
                    items = LeaveImportSheetConstants.Items.ToList();
                    break;
                case DropdownListTypes.GHRMPlusCompany:
                    items = GHRMPlusCompanyConstants.Items.ToList();
                    break;
                case DropdownListTypes.PayrollType:
                    items = PayrollTypeConstants.Items.ToList();
                    break;
                case DropdownListTypes.TimeKeepingType:
                    items = TimeKeepingTypeConstants.Items.ToList();
                    break;
                case DropdownListTypes.AttendanceEventType:
                    items = AttendanceEventTypeConstants.Items.ToList();
                    break;
                case DropdownListTypes.ComponentPayroll:
                    items = ComponentPayrollConstants.Items.ToList();
                    break;
                case DropdownListTypes.LoanState:
                    items = LoanStateConstants.Items.ToList();
                    break;
                case DropdownListTypes.TransactionType:
                    items = TransactionTypeConstants.Items.ToList();
                    break;
                case DropdownListTypes.EmployeeDocumentType:
                    items = EmployeeDocumentTypeConstants.Items.ToList();
                    break;
                case DropdownListTypes.PFReportType:
                    items = PFReportTypeConstants.Items.ToList();
                    break; 
                case DropdownListTypes.OvertimeExceptionType:
                    items = OvertimeExceptionTypeConstants.Items.ToList();
                    break;
                case DropdownListTypes.EmployeeOthersReport:
                    items = EmployeeOthersReportConstants.Items.ToList();
                    break;
                case DropdownListTypes.LoanStatus:
                    items = LoanStatusConstants.Items.ToList();
                    break;
                case DropdownListTypes.GradeRatioOn:
                    items = GradeRatioOnConstants.Items.ToList();
                    break;
                case DropdownListTypes.GradeName:
                    items = GradeRatioOnConstants.Items.ToList();
                    break;
            }

            return items.Select(
                i => new SelectListItem
                {
                    Value = i.Value,
                    Text = i.Text,
                    Selected = !string.IsNullOrWhiteSpace(selected) ? selected == i.Value : i.Selected
                }).ToList();
        }

        public static IEnumerable<ConstantDropdownItem> GetCreditCardYears()
        {
            var years = new List<ConstantDropdownItem>();

            for (var i = 1; i <= 15; i++)
            {
                var year = Convert.ToString(DateTime.Now.Year + i);
                years.Add(new ConstantDropdownItem
                {
                    Text = year,
                    Value = year,
                });
            }
            return years;
        }

        public static IEnumerable<ConstantDropdownItem> GetCreditCardMonths()
        {
            var months = new List<ConstantDropdownItem>();

            for (var i = 1; i <= 12; i++)
            {
                var text = (i < 10)
                                  ? "0" + i.ToString(CultureInfo.InvariantCulture)
                                  : i.ToString(CultureInfo.InvariantCulture);

                months.Add(new ConstantDropdownItem
                {
                    Text = text,
                    Value = i.ToString(CultureInfo.InvariantCulture),
                });
            }

            return months;
        }

        public static IEnumerable<ConstantDropdownItem> GetNumbersList()
        {
            var items = new List<ConstantDropdownItem>();

            for (var i = 1; i <= 31; i++)
            {
                var text = i.ToString(CultureInfo.InvariantCulture);

                items.Add(new ConstantDropdownItem
                {
                    Text = text,
                    Value = text,
                });
            }

            return items;
        }
    }
}