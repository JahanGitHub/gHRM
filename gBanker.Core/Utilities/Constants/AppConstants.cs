using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Core.Utilities.Constants
{
    public static class EmployeeFamilyRelationConstants
    {
        public const string Father = "F";
        public const string Mother = "M";
        public const string Wife = "W";
        public const string Husband = "H";
        public const string Son = "S";
        public const string Daughter = "D";
        public const string Brother = "Br";
        public const string Sister = "Sis";
    }

    public static class GenderConstants
    {
        public const string Male = "M";
        public const string Female = "F";
        public const string Common = "C";
    }

    public static class LeaveSellAdviseStatusConstants
    {
        public const int UnSold = 0;
        public const int Sold = 1;
        public const int Cancelled = 2;
    }

    public static class ReportViewModeConstants
    {
        public const string Potrait = "Potrait";
        public const string Landscape = "Landscape";
    }

    public static class BaseResonseConstants
    {
        public const string Success = "1";
        public const string Failed = "2";
    }

    public static class LoanStatusConstants
    {
        public const string Running = "Running";
        public const string Closed = "Closed";

        public static string GetText(string searchTerm)
        {
            return Items.FindItemInList(searchTerm);
        }

        public static IEnumerable<ConstantDropdownItem> Items
        {
            get
            {
                return new List<ConstantDropdownItem>
                {
                    new ConstantDropdownItem {Text = "Running", Value = Running.ToString(), Selected = false},
                    new ConstantDropdownItem {Text = "Closed", Value = Closed.ToString(), Selected = false}
                };
            }
        }
    }

    public static class GradeRatioOnConstants
    {
        public const string Fixed = "Fixed";
        public const string Percentage = "Percentage";

        public static string GetText(string searchTerm)
        {
            return Items.FindItemInList(searchTerm);
        }

        public static IEnumerable<ConstantDropdownItem> Items
        {
            get
            {
                return new List<ConstantDropdownItem>
                {
                    new ConstantDropdownItem {Text = "Fixed", Value = Fixed.ToString(), Selected = true},
                    new ConstantDropdownItem {Text = "Percentage", Value = Percentage.ToString(), Selected = false}
                };
            }
        }
    }

    public static class BloodGroupConstants
    {
        public const string APlus = "A+";
        public const string ANegative = "A-";
        public const string BPlus = "B+";
        public const string BNegative = "B-";

        public const string ABPlus = "AB+";
        public const string ABNegative = "AB-";

        public const string OPlus = "O+";
        public const string ONegative = "O-";

        public const string Unknown = "U";
    }

    public static class ReligionConstants
    {
        public const string Islam = "Islam";
        public const string Hindu = "Hindu";
        public const string Buddish = "Buddish";
        public const string Christan = "Christan";
    }

    public static class UserRoleConstants
    {
        public const string Super_Admin = "Super Admin";
        public const string Human_Resource = "Human Resource";
        public const string Employee = "Employee";
        public const string Audit = "Audit";
        public const string Purchase_entry_user = "Purchase-entry user";
        public const string Accounts_entry_user = "Accounts-entry user";
    }

    public static class EmployeeStatusConstants
    {
        //activew  status
        public const int Regular = 1;
        public const int Contractual = 2;
        public const int Probationary = 3;
        public const int ExtendedProbationary = 4;
        public const int Trainee = 5;
        public const int Intern = 6;
        public const int UnauthorizedAbsent = 7;
        public const int LeaveOnPreparationForRetiredment_L_P_R = 8;
        public const int Lien = 9;
        public const int Suspended = 10;
        public const int OfficerOnSpecialDuty = 11;

        //inactive status
        public const int Resign = 12;

        public static string GetText(string searchTerm)
        {
            return Items.FindItemInList(searchTerm);
        }

        public static IEnumerable<ConstantDropdownItem> Items
        {
            get
            {
                return new List<ConstantDropdownItem>
                {
                    new ConstantDropdownItem {Text = "Regular", Value = Regular.ToString(), Selected = true},
                    new ConstantDropdownItem {Text = "Contractual", Value = Contractual.ToString(), Selected = false},

                    new ConstantDropdownItem {Text = "Probationary", Value = Probationary.ToString(), Selected = false},
                    new ConstantDropdownItem {Text = "ExtendedProbationary", Value = ExtendedProbationary.ToString(), Selected = false},
                    new ConstantDropdownItem {Text = "Trainee", Value = Trainee.ToString(), Selected = false},
                    new ConstantDropdownItem {Text = "Intern", Value = Intern.ToString(), Selected = false},
                    new ConstantDropdownItem {Text = "UnauthorizedAbsent", Value = UnauthorizedAbsent.ToString(), Selected = false},
                    new ConstantDropdownItem {Text = "LeaveOnPreparationForRetiredment_L_P_R", Value = LeaveOnPreparationForRetiredment_L_P_R.ToString(), Selected = false},
                    new ConstantDropdownItem {Text = "Lien", Value = Lien.ToString(), Selected = false},
                    new ConstantDropdownItem {Text = "Suspended", Value = Suspended.ToString(), Selected = false},
                    new ConstantDropdownItem {Text = "OfficerOnSpecialDuty", Value = OfficerOnSpecialDuty.ToString(), Selected = false},
                    new ConstantDropdownItem {Text = "Resign", Value = Resign.ToString(), Selected = false}
                };
            }
        }
    }

    public static class AddressTypeConstants
    {
        public const string PresentAddress = "Pr"; //Present Address
        public const string PermanentAddress = "Pe"; //Permanent Address
    }

    public static class CompanyConstants
    {
        public const int DefaultCompany = 1;
    }

    public static class FamilyInfoTypeConstants
    {
        public const int FatherInfo = 1;
        public const int MotherInfo = 2;
    }

    public static class SalaryCalculationTypeConstants
    {
        public const string Ratio = "R";
        public const string Fixed = "F";

        public static string GetText(string searchTerm)
        {
            return Items.FindItemInList(searchTerm);
        }

        public static IEnumerable<ConstantDropdownItem> Items
        {
            get
            {
                return new List<ConstantDropdownItem>
                {
                    new ConstantDropdownItem {Text = "Ratio", Value = Ratio, Selected = true},
                    new ConstantDropdownItem {Text = "Fixed", Value = Fixed, Selected = false}
                };
            }
        }
    }

    public static class SalaryAccountTransactionTypeConstants
    {
        public const string Addition = "Cr";
        public const string Deduction = "Dr";

        public static string GetText(string searchTerm)
        {
            return Items.FindItemInList(searchTerm);
        }

        public static IEnumerable<ConstantDropdownItem> Items
        {
            get
            {
                return new List<ConstantDropdownItem>
                {
                    new ConstantDropdownItem {Text = "Addition", Value = Addition, Selected = true},
                    new ConstantDropdownItem {Text = "Deduction", Value = Deduction, Selected = false}
                };
            }
        }
    }

    public static class TransactionGroupConstants
    {
        public const string PF = "PF";
        public const string LN = "LN";

        public static string GetText(string searchTerm)
        {
            return Items.FindItemInList(searchTerm);
        }

        public static IEnumerable<ConstantDropdownItem> Items
        {
            get
            {
                return new List<ConstantDropdownItem>
                {
                    new ConstantDropdownItem {Text = "PF", Value = PF, Selected = true},
                    new ConstantDropdownItem {Text = "LN", Value = LN, Selected = false}
                };
            }
        }
    }
    public static class PFDayStatusConstants
    {
        public const string NotStarted = "Not started";
        public const string Close = "Close";
        public const string Open = "Open";

        public static string GetText(string searchTerm)
        {
            return Items.FindItemInList(searchTerm);
        }

        public static IEnumerable<ConstantDropdownItem> Items
        {
            get
            {
                return new List<ConstantDropdownItem>
                {
                    new ConstantDropdownItem {Text = "Not started", Value = NotStarted, Selected = true},
                    new ConstantDropdownItem {Text = "Close", Value = Close, Selected = false},
                    new ConstantDropdownItem {Text = "Open", Value = Open, Selected = false},
                };
            }
        }
    }
    public static class SalaryRatioConstants
    {
        public const string Gross = "G";
        public const string Basic = "B";
        public const string NotRequired = "NR";
        public static string GetText(string searchTerm)
        {
            return Items.FindItemInList(searchTerm);
        }

        public static IEnumerable<ConstantDropdownItem> Items
        {
            get
            {
                return new List<ConstantDropdownItem>
                {
                    new ConstantDropdownItem {Text = "Gross", Value = Gross, Selected = true},
                    new ConstantDropdownItem {Text = "Basic", Value = Basic, Selected = false},
                    new ConstantDropdownItem {Text = "Not Required", Value = NotRequired, Selected = false},
                };
            }
        }
    }

    public static class ComponentPayrollConstants
    {
        public const string Salary_BasicSalary = "Basic Salary";
        public const string Deduction_IncomeTax = "Income Tax";
        public const string Salary_HouseRent = "House Rent";
        public const string Salary_Conveyance = "Conveyance";
        public const string Salary_Medical = "Medical";
        public const string Salary_LFA = "LFA";
        public const string Allowance_TechnicalAllowance = "Technical Allowance";
        public const string Deduction_LeaveWithoutPayment = "Leave Without Payment";
        public const string Allowance_Arrear = "Arrear";
        public const string Allowance_FoodAllowance = "Food Allowance";
        public const string Allowance_Overtime = "Overtime";
        public const string Salary_PFOfficeContribution = "PF Office Contribution";
        public const string Salary_PFEmployeeDeduction = "PF Employee Deduction";
        public const string Salary_PFOfficeDeduction = "PF Office Deduction";
        public const string Deduction_OtherDeduction = "Other Deduction";
        public const string Loan_PFLoan = "PF Loan";
        public const string Deduction_MobileBill = "Mobile Bill";
        public const string Deduction_Vaccine = "Vaccine";
        public const string Deduction_PartialSalaryDeductionNewJoin = "Partial Salary Deduction New Join";
        public const string Decuction_RevenueStamp = "Revenue Stamp";
        public const string Allowance_LeaveEncashment = "Leave Encashment";
        public const string Bonus_EidUlFitrBonus = "Eid-Ul-Fitr Bonus";
        public const string Bonus_IncentiveBonus = "Incentive Bonus";
        public const string Bonus_BoishakhiBonus = "Boishakhi Bonus";
        public const string Bonus_EidUlAzhaBonus = "Eid-Ul-Azha Bonus";
        public const string Salary_DearnessAllowance = "Dearness Allowance";
        public const string Deposit_SalaryDeposit = "Salary Deposit";
        public const string Deposit_SalaryDepositRefund = "Salary Deposit Refund";
        public const string Deduction_LessBranchDeduction = "Less Branch Deduction";
        public const string Allowance_ExtraBranchAllowance = "Extra Branch Allowance";
        public const string Allowance_HardshipAllowance = "Hardship Allowance";
        public const string Allowance_TADAAllowance = "TA/DA Allowance";
        public const string Allowance_OtherAllowance = "Other Allowance";
        public const string Allowance_ArrearDearness = "Arrear Dearness";
        public const string Allowance_ArrearBonus = "Arrear Bonus";
        public const string Allowance_PersonalAllowance = "Personal Allowance";
        public const string Allowance_ArrearPersonalAllowance = "Arrear Personal Allowance";
        public const string Allowance_MobileBill = "Mobile Bill";
        public const string Deduction_AdvanceSalary = "Advance Salary";
        public const string Deduction_PFLoan = "PF Loan";
        public const string Deduction_ModemBill = "Modem Bill";
        public static string GetText(string searchTerm)
        {
            return Items.FindItemInList(searchTerm);
        }

        public static IEnumerable<ConstantDropdownItem> Items
        {
            get
            {
                return new List<ConstantDropdownItem>
                {
                    new ConstantDropdownItem {Text = "Basic Salary", Value = Salary_BasicSalary, Selected = false},
                    new ConstantDropdownItem {Text = "Income Tax", Value = Deduction_IncomeTax, Selected = false},
                    new ConstantDropdownItem {Text = "House Rent", Value = Salary_HouseRent, Selected = false},
                    new ConstantDropdownItem {Text = "Conveyance", Value = Salary_Conveyance, Selected = false},
                    new ConstantDropdownItem {Text = "Medical", Value = Salary_Medical, Selected = false},
                    new ConstantDropdownItem {Text = "LFA", Value = Salary_LFA, Selected = false},
                    new ConstantDropdownItem {Text = "Technical Allowance", Value = Allowance_TechnicalAllowance, Selected = false},
                    new ConstantDropdownItem {Text = "Leave Without Payment", Value = Deduction_LeaveWithoutPayment, Selected = false},
                    new ConstantDropdownItem {Text = "Arrear", Value = Allowance_Arrear, Selected = false},
                    new ConstantDropdownItem {Text = "Food Allowance", Value = Allowance_FoodAllowance, Selected = false},
                    new ConstantDropdownItem {Text = "Overtime", Value = Allowance_Overtime, Selected = false},
                    new ConstantDropdownItem {Text = "PF Office Contribution", Value = Salary_PFOfficeContribution, Selected = false},
                    new ConstantDropdownItem {Text = "PF Employee Deduction", Value = Salary_PFEmployeeDeduction, Selected = false},
                    new ConstantDropdownItem {Text = "PF Office Deduction", Value = Salary_PFOfficeDeduction, Selected = false},
                    new ConstantDropdownItem {Text = "Other Deduction", Value = Deduction_OtherDeduction, Selected = false},
                    new ConstantDropdownItem {Text = "PF Loan", Value = Loan_PFLoan, Selected = false},
                    new ConstantDropdownItem {Text = "Mobile Bill", Value = Deduction_MobileBill, Selected = false},
                    new ConstantDropdownItem {Text = "Vaccine", Value = Deduction_Vaccine, Selected = false},
                    new ConstantDropdownItem {Text = "Partial Salary Deduction New Join", Value = Deduction_PartialSalaryDeductionNewJoin, Selected = false},
                    new ConstantDropdownItem {Text = "Revenue Stamp", Value = Decuction_RevenueStamp, Selected = false},
                    new ConstantDropdownItem {Text = "Leave Encashment", Value = Allowance_LeaveEncashment, Selected = false},
                    new ConstantDropdownItem {Text = "Eid-Ul-Fitr Bonus", Value = Bonus_EidUlFitrBonus, Selected = false},
                    new ConstantDropdownItem {Text = "Incentive Bonus", Value = Bonus_IncentiveBonus, Selected = false},
                    new ConstantDropdownItem {Text = "Boishakhi Bonus", Value = Bonus_BoishakhiBonus, Selected = false},
                    new ConstantDropdownItem {Text = "Eid-Ul-Azha Bonus", Value = Bonus_EidUlAzhaBonus, Selected = false},
                    new ConstantDropdownItem {Text = "Dearness Allowance", Value = Salary_DearnessAllowance, Selected = false},
                    new ConstantDropdownItem {Text = "Salary Deposit", Value = Deposit_SalaryDeposit, Selected = false},
                    new ConstantDropdownItem {Text = "Salary Deposit Refund", Value = Deposit_SalaryDepositRefund, Selected = false},
                    new ConstantDropdownItem {Text = "Less Branch Deduction", Value = Deduction_LessBranchDeduction, Selected = false},
                    new ConstantDropdownItem {Text = "Extra Branch Allowance", Value = Allowance_ExtraBranchAllowance, Selected = false},
                    new ConstantDropdownItem {Text = "Hardship Allowance", Value = Allowance_HardshipAllowance, Selected = false},
                    new ConstantDropdownItem {Text = "TA/DA Allowance", Value = Allowance_TADAAllowance, Selected = false},
                    new ConstantDropdownItem {Text = "Other Allowance", Value = Allowance_OtherAllowance, Selected = false},
                    new ConstantDropdownItem {Text = "Arrear Dearness", Value = Allowance_ArrearDearness, Selected = false},
                    new ConstantDropdownItem {Text = "Arrear Bonus", Value = Allowance_ArrearBonus, Selected = false},
                    new ConstantDropdownItem {Text = "Personal Allowance", Value = Allowance_PersonalAllowance, Selected = false},
                    new ConstantDropdownItem {Text = "Arrear Personal Allowance", Value = Allowance_ArrearPersonalAllowance, Selected = false},
                    new ConstantDropdownItem {Text = "Mobile Bill", Value = Allowance_MobileBill, Selected = false},
                    new ConstantDropdownItem {Text = "Advance Salary", Value = Deduction_AdvanceSalary, Selected = false},
                    new ConstantDropdownItem {Text = "PF Loan", Value = Deduction_PFLoan, Selected = false},
                    new ConstantDropdownItem {Text = "Modem Bill", Value = Deduction_ModemBill, Selected = false},
                };
            }
        }
    }

    public static class EmployeeOthersReportConstants
    {
        public const string DropoutByReason = "DropoutByReason";
        public const string DropoutByReasonForMousumi = "DropoutByReasonForMousumi";
        public const string OfficeWiseActiveEmployeeByDesignation = "OfficeWiseActiveEmployeeByDesignation";
        public const string PersonalInfo = "PersonalInfo";
        public const string MonthWiseConfirmationList = "MonthWiseConfirmationList";
        public const string MonthWiseConfirmationDueList = "MonthWiseConfirmationDueList";


        public static string GetText(string searchTerm)
        {
            return Items.FindItemInList(searchTerm);
        }

        public static IEnumerable<ConstantDropdownItem> Items
        {
            get
            {
                return new List<ConstantDropdownItem>
                {
                    new ConstantDropdownItem {Text = "Dropout By Reason", Value = DropoutByReason},
                    new ConstantDropdownItem {Text = "Dropout By Reason for Mousumi", Value = DropoutByReasonForMousumi},
                    new ConstantDropdownItem {Text = "Office Wise Active Employee By Designation", Value = OfficeWiseActiveEmployeeByDesignation},
                    new ConstantDropdownItem {Text = "Personal Info", Value = PersonalInfo},
                    new ConstantDropdownItem {Text = "Month Wise Confirmation List", Value = MonthWiseConfirmationList},
                    new ConstantDropdownItem {Text = "Month Wise Confirmation Due List", Value = MonthWiseConfirmationDueList},
                };
            }
        }
    }

    public static class ProvidentFundTypeConstants
    {
        public const string NotApplicable = "0";
        public const string CPF = "1";
        public const string GPF = "2";
        public static string GetText(string searchTerm)
        {
            return Items.FindItemInList(searchTerm);
        }

        public static IEnumerable<ConstantDropdownItem> Items
        {
            get
            {
                return new List<ConstantDropdownItem>
                {
                    new ConstantDropdownItem {Text = "Not Applicable", Value = NotApplicable, Selected = true},
                    new ConstantDropdownItem {Text = "CPF[Contributory Provident Fund]", Value = CPF, Selected = false},
                    new ConstantDropdownItem {Text = "GPF[General Provident Fund]", Value = GPF, Selected = false},
                };
            }
        }
    }


    public static class PFTypeConstants
    {
        public const string NotApplicable = "0";
        public const string CPF = "1";
        public const string GPF = "2";
        public static string GetText(string searchTerm)
        {
            return Items.FindItemInList(searchTerm);
        }

        public static IEnumerable<ConstantDropdownItem> Items
        {
            get
            {
                return new List<ConstantDropdownItem>
                {
                    new ConstantDropdownItem {Text = "Not Applicable", Value = NotApplicable, Selected = true},
                    new ConstantDropdownItem {Text = "CPF[Contributory Provident Fund]", Value = CPF, Selected = false},
                    new ConstantDropdownItem {Text = "GPF[General Provident Fund]", Value = GPF, Selected = false},
                };
            }
        }
    }

    public static class SalaryTypeConstants
    {
        public const string Structured = "1";
        public const string Unstructured = "2";
        public static string GetText(string searchTerm)
        {
            return Items.FindItemInList(searchTerm);
        }

        public static IEnumerable<ConstantDropdownItem> Items
        {
            get
            {
                return new List<ConstantDropdownItem>
                {
                    new ConstantDropdownItem {Text = "Structured", Value = Structured, Selected = true},
                    new ConstantDropdownItem {Text = "Unstructured", Value = Unstructured, Selected = false},
                };
            }
        }
    }

    public static class EmployeeTypeConfigConstants
    {
        public const string PayScale = "1";
        public const string NonPayScale = "2";
        public static string GetText(string searchTerm)
        {
            return Items.FindItemInList(searchTerm);
        }

        public static IEnumerable<ConstantDropdownItem> Items
        {
            get
            {
                return new List<ConstantDropdownItem>
                {
                    new ConstantDropdownItem {Text = "PayScale", Value = PayScale, Selected = true},
                    new ConstantDropdownItem {Text = "Non PayScale", Value = NonPayScale, Selected = false},
                };
            }
        }
    }

    public static class ComponentCategoryConstants
    {
        public const string Allowance = "Allowance";
        public const string Deduction = "Deduction";
        public static string GetText(string searchTerm)
        {
            return Items.FindItemInList(searchTerm);
        }

        public static IEnumerable<ConstantDropdownItem> Items
        {
            get
            {
                return new List<ConstantDropdownItem>
                {
                    new ConstantDropdownItem {Text = "Allowance", Value = Allowance, Selected = true},
                    new ConstantDropdownItem {Text = "Deduction", Value = Deduction, Selected = false},
                };
            }
        }
    }

    public static class PromotionTypeConstants
    {
        public const string Increment = "Increment";
        public const string Recontract = "Recontract";
        public const string FirstJoining = "FirstJoining";
        public static string GetText(string searchTerm)
        {
            return Items.FindItemInList(searchTerm);
        }

        public static IEnumerable<ConstantDropdownItem> Items
        {
            get
            {
                return new List<ConstantDropdownItem>
                {
                    new ConstantDropdownItem {Text = "Increment", Value = Increment, Selected = true},
                    new ConstantDropdownItem {Text = "Recontract", Value = Recontract, Selected = false},
                    new ConstantDropdownItem {Text = "First Joining", Value = FirstJoining, Selected = false},
                };
            }
        }
    }
    public static class PromotionTypeValueConstants
    {
        public const string Increment = "1";
        public const string Recontract = "2";
        public const string FirstJoining = "3";
        public static string GetText(string searchTerm)
        {
            return Items.FindItemInList(searchTerm);
        }

        public static IEnumerable<ConstantDropdownItem> Items
        {
            get
            {
                return new List<ConstantDropdownItem>
                {
                    new ConstantDropdownItem {Text = "Increment", Value = Increment, Selected = true},
                    new ConstantDropdownItem {Text = "Recontract", Value = Recontract, Selected = false},
                    new ConstantDropdownItem {Text = "First Joining", Value = FirstJoining, Selected = false},
                };
            }
        }
    }
    public static class ComponentDepositCategoryConstants
    {
        public const string Deposit = "Deposit";
        public static string GetText(string searchTerm)
        {
            return Items.FindItemInList(searchTerm);
        }

        public static IEnumerable<ConstantDropdownItem> Items
        {
            get
            {
                return new List<ConstantDropdownItem>
                {
                    new ConstantDropdownItem {Text = "Deposit", Value = Deposit, Selected = true}
                };
            }
        }
    }

    public static class GHRMPlusCompanyConstants
    {
        public const string AlternativeDevelopmentInitiative = "ADI";
        public const string AIDFoundation = "AID";
        public const string GramBikashKendra = "GBK";

        public const string GrameenCommunications = "GC";
        public const string Ghashful = "GF";
        public const string GrameenKalyan = "GK";

        public const string GrameenMotshoOPashusampadFoundation = "GMPF";
        public const string GrameenShaktiSamajikByaboshaLtd = "GSSB";
        public const string GrameenTelecomTrust = "GTT";
        public const string GT = "GT";

        public const string GUK = "GUK";
        public const string JagoraniChakraFoundation = "JCF";
        public const string PidimFoundation = "Pidim";

        public const string Prottyashi = "Prottyashi";
        public const string Proyas = "Proyas";
        public const string Sangram = "SNG";

        public const string VillageEducationResourceCenter = "VERC";
        public const string YoungPowerinSocialAction = "YPSA";
        public const string NRDS = "NRDS";
        public const string SDC = "SDC";
        public const string Mousumi = "Mousumi";
        public const string NGF = "NGF";
        public const string Ononyo = "ononyo";

        public static string GetText(string searchTerm)
        {
            return Items.FindItemInList(searchTerm);
        }

        public static IEnumerable<ConstantDropdownItem> Items
        {
            get
            {
                return new List<ConstantDropdownItem>
                {
                     new ConstantDropdownItem {Text = "Alternative Development Initiative-ADI", Value = AlternativeDevelopmentInitiative, Selected = false},
                     new ConstantDropdownItem {Text = "AID Foundation-AID", Value = AIDFoundation, Selected = false},
                     new ConstantDropdownItem {Text = "Gram Bikash Kendra-GBK", Value = GramBikashKendra, Selected = false},

                     new ConstantDropdownItem {Text = "Grameen Communications-GC", Value = GrameenCommunications, Selected = false},
                     new ConstantDropdownItem {Text = "Ghashful-GF", Value = Ghashful, Selected = false},
                     new ConstantDropdownItem {Text = "Grameen Kalyan-GK", Value = GrameenKalyan, Selected = false},

                     new ConstantDropdownItem {Text = "Grameen Motsho O Pashusampad Foundation-GMPF", Value = GrameenMotshoOPashusampadFoundation, Selected = false},
                     new ConstantDropdownItem {Text = "Grameen Shakti Samajik Byabosha Ltd-GSSB", Value = GrameenShaktiSamajikByaboshaLtd, Selected = false},
                     new ConstantDropdownItem {Text = "Grameen Telecom Trust", Value = GrameenTelecomTrust, Selected = false},
                     new ConstantDropdownItem {Text = "Grameen Trust", Value = GT, Selected = false},

                     new ConstantDropdownItem {Text = "Gram Unnayan Karma-GUK", Value = GUK, Selected = false},
                     new ConstantDropdownItem {Text = "Jagorani Chakra Foundation-JCF", Value = JagoraniChakraFoundation, Selected = false},
                     new ConstantDropdownItem {Text = "Pidim Foundation-Pidim", Value = PidimFoundation, Selected = false},

                     new ConstantDropdownItem {Text = "Prottyashi", Value = Prottyashi, Selected = false},
                     new ConstantDropdownItem {Text = "Proyas", Value = Proyas, Selected = false},
                     new ConstantDropdownItem {Text = "Sangram", Value = Sangram, Selected = false},

                     new ConstantDropdownItem {Text = "Village Education Resource Center-VERC", Value = VillageEducationResourceCenter, Selected = false},
                     new ConstantDropdownItem {Text = "Young Power in Social Action-YPSA", Value = YoungPowerinSocialAction, Selected = false},

                     new ConstantDropdownItem {Text = "Mousumi", Value = Mousumi, Selected = false},
                     new ConstantDropdownItem {Text = "NGF", Value = NGF, Selected = false},

                     new ConstantDropdownItem {Text = "NRDS", Value = NRDS, Selected = false}
                };
            }
        }
    }

    public static class GBankerCompanyConstants
    {
        public const string GUK = "12";
        public static string GetText(string searchTerm)
        {
            return Items.FindItemInList(searchTerm);
        }

        public static IEnumerable<ConstantDropdownItem> Items
        {
            get
            {
                return new List<ConstantDropdownItem>
                {
                    new ConstantDropdownItem {Text = "Gana Unnayan Kendra", Value = GUK, Selected = true}
                };
            }
        }
    }

    public static class MonthConstants
    {
        public const string January = "1";
        public const string February = "2";
        public const string March = "3";
        public const string April = "4";
        public const string May = "5";
        public const string June = "6";
        public const string July = "7";
        public const string August = "8";
        public const string September = "9";
        public const string October = "10";
        public const string November = "11";
        public const string December = "12";


        public static string GetText(string searchTerm)
        {
            return Items.FindItemInList(searchTerm);
        }

        public static IEnumerable<ConstantDropdownItem> Items
        {
            get
            {
                return new List<ConstantDropdownItem>
                {
                    new ConstantDropdownItem {Text = "January", Value = January, Selected = true},
                    new ConstantDropdownItem {Text = "February", Value = February, Selected = false},
                    new ConstantDropdownItem {Text = "March", Value = March, Selected = false},
                    new ConstantDropdownItem {Text = "April", Value = April, Selected = false},
                    new ConstantDropdownItem {Text = "May", Value = May, Selected = false},
                    new ConstantDropdownItem {Text = "June", Value = June, Selected = false},
                    new ConstantDropdownItem {Text = "July", Value = July, Selected = false},
                    new ConstantDropdownItem {Text = "August", Value = August, Selected = false},
                    new ConstantDropdownItem {Text = "September", Value = September, Selected = false},
                    new ConstantDropdownItem {Text = "October", Value = October, Selected = false},
                    new ConstantDropdownItem {Text = "November", Value = November, Selected = false},
                    new ConstantDropdownItem {Text = "December", Value = December, Selected = false}
                };
            }
        }
    }

    public static class EmploymentTypeConstants
    {
        public const string PayScale = "PS";
        public const string NonPayScale = "NPS";
        public static string GetText(string searchTerm)
        {
            return Items.FindItemInList(searchTerm);
        }

        public static IEnumerable<ConstantDropdownItem> Items
        {
            get
            {
                return new List<ConstantDropdownItem>
                {
                    new ConstantDropdownItem {Text = "Pay-Scale", Value = PayScale, Selected = true},
                    new ConstantDropdownItem {Text = "Non Pay-Scale", Value = NonPayScale, Selected = false}
                };
            }
        }
    }

    public static class RatioBasedOnConstants
    {
        public const string Gross = "G";
        public const string Basic = "B";
        public const string NotRequired = "NR";
        public static string GetText(string searchTerm)
        {
            return Items.FindItemInList(searchTerm);
        }

        public static IEnumerable<ConstantDropdownItem> Items
        {
            get
            {
                return new List<ConstantDropdownItem>
                {
                    new ConstantDropdownItem {Text = "Gross", Value = Gross, Selected = true},
                    new ConstantDropdownItem {Text = "Basic", Value = Basic, Selected = false},
                     new ConstantDropdownItem {Text = "Not Required", Value = NotRequired, Selected = false},
                };
            }
        }
    }

    public static class SalaryStructureTypeConstants
    {
        public const string Structured = "1";
        public const string Unstructured = "2";
        public static string GetText(string searchTerm)
        {
            return Items.FindItemInList(searchTerm);
        }

        public static IEnumerable<ConstantDropdownItem> Items
        {
            get
            {
                return new List<ConstantDropdownItem>
                {
                    new ConstantDropdownItem {Text = "Structured", Value = Structured, Selected = true},
                    new ConstantDropdownItem {Text = "Unstructured", Value = Unstructured, Selected = false}
                };
            }
        }
    }

    public static class LeaveTypeGenderConstants
    {
        public const string Both = "B";
        public const string Male = "M";
        public const string Female = "F";
        public static string GetText(string searchTerm)
        {
            return Items.FindItemInList(searchTerm);
        }

        public static IEnumerable<ConstantDropdownItem> Items
        {
            get
            {
                return new List<ConstantDropdownItem>
                {
                    new ConstantDropdownItem {Text = "Both", Value = Both, Selected = true},
                    new ConstantDropdownItem {Text = "Male", Value = Male, Selected = false},
                    new ConstantDropdownItem {Text = "Female", Value = Female, Selected = false},
                };
            }
        }
    }
    public static class LeaveStatusConstants
    {
        public const string Laps = "L";
        public const string CarryForward = "C";
        public const string NotApplicable = "N";
        public static string GetText(string searchTerm)
        {
            return Items.FindItemInList(searchTerm);
        }

        public static IEnumerable<ConstantDropdownItem> Items
        {
            get
            {
                return new List<ConstantDropdownItem>
                {
                    new ConstantDropdownItem {Text = "Laps", Value = Laps, Selected = true},
                    new ConstantDropdownItem {Text = "Carry Forward", Value = CarryForward, Selected = false},
                    new ConstantDropdownItem {Text = "N/A", Value = NotApplicable, Selected = false},
                };
            }
        }
    }

    public static class AttendanceTerminalConstants
    {
        public const string Terminal_01 = "T01";
        public const string Terminal_02 = "T02";
        public static string GetText(string searchTerm)
        {
            return Items.FindItemInList(searchTerm);
        }

        public static IEnumerable<ConstantDropdownItem> Items
        {
            get
            {
                return new List<ConstantDropdownItem>
                {
                    new ConstantDropdownItem {Text = "Terminal 01", Value = Terminal_01, Selected = true},
                    new ConstantDropdownItem {Text = "Terminal 02", Value = Terminal_02, Selected = false}
                };
            }
        }
    }

    public static class AttendanceResultTypeConstants
    {
        public const string Absence = "Absence";
        public const string AbsenceTrue = "TRUE";
        public const string AbsenceFalse = "";
        public static string GetText(string searchTerm)
        {
            return Items.FindItemInList(searchTerm);
        }

        public static IEnumerable<ConstantDropdownItem> Items
        {
            get
            {
                return new List<ConstantDropdownItem>
                {
                    new ConstantDropdownItem {Text = "Absence", Value = Absence, Selected = true}
                };
            }
        }
    }

    public static class LeaveAdjustTypeConstants
    {
        public const string NonAdjust = "N";
        public const string Adjusted = "A";
        public const string Reject = "R";
        public static string GetText(string searchTerm)
        {
            return Items.FindItemInList(searchTerm);
        }

        public static IEnumerable<ConstantDropdownItem> Items
        {
            get
            {
                return new List<ConstantDropdownItem>
                {
                    new ConstantDropdownItem {Text = "Non-Adjust", Value = NonAdjust, Selected = true},
                    new ConstantDropdownItem {Text = "Adjusted", Value = Adjusted, Selected = false},
                    new ConstantDropdownItem {Text = "Reject", Value = Reject, Selected = false},
                };
            }
        }
    }

    public static class LeaveReasonConstants
    {
        public const string Absent = "Absent";
        public const string OPENING = "OPENING";
        public static string GetText(string searchTerm)
        {
            return Items.FindItemInList(searchTerm);
        }

        public static IEnumerable<ConstantDropdownItem> Items
        {
            get
            {
                return new List<ConstantDropdownItem>
                {
                    new ConstantDropdownItem {Text = "Absent", Value = Absent, Selected = true},
                    new ConstantDropdownItem {Text = "OPENING", Value = OPENING, Selected = true},
                };
            }
        }
    }

    public static class EmailNotificationTypeConstants
    {
        public const string Application = "Application";
        public const string Replacement = "Replacement";
        public const string Rejected = "Rejected";
        public const string Approved = "Approved";
        public static string GetText(string searchTerm)
        {
            return Items.FindItemInList(searchTerm);
        }

        public static IEnumerable<ConstantDropdownItem> Items
        {
            get
            {
                return new List<ConstantDropdownItem>
                {
                    new ConstantDropdownItem {Text = "Application", Value = Application, Selected = true},
                    new ConstantDropdownItem {Text = "Replacement", Value = Replacement, Selected = false},
                    new ConstantDropdownItem {Text = "Rejected", Value = Rejected, Selected = false},
                    new ConstantDropdownItem {Text = "Approved", Value = Approved, Selected = false},
                };
            }
        }
    }

    public static class LeaveCategoryConstants
    {
        public const string Casual = "CL";
        public const string Annual_EL = "AL";
        public const string Maternity = "ML";
        public const string LWP = "LWP";
        public const string Paternity = "PL";
        public const string Other = "OL";
        public const string Medical = "MEL";
        public const string Annual_EL_Laps = "AL_Laps";
        public const string SickLeave = "SL";

        public static string GetText(string searchTerm)
        {
            return Items.FindItemInList(searchTerm);
        }

        public static IEnumerable<ConstantDropdownItem> Items
        {
            get
            {
                return new List<ConstantDropdownItem>
                {
                    new ConstantDropdownItem {Text = "Casual", Value = Casual, Selected = true},
                    new ConstantDropdownItem {Text = "Annual(Earn Leave)", Value = Annual_EL, Selected = false},
                    new ConstantDropdownItem {Text = "Maternity", Value = Maternity, Selected = false},
                    new ConstantDropdownItem {Text = "Leave Without Pay(LWP)", Value = LWP, Selected = false},
                    new ConstantDropdownItem {Text = "Paternity", Value = Paternity, Selected = false},
                    new ConstantDropdownItem {Text = "Medical", Value = Medical, Selected = false},
                    new ConstantDropdownItem {Text = "Annual(Laps)", Value = Annual_EL_Laps, Selected = false},
                    new ConstantDropdownItem {Text = "Sick Leave", Value = SickLeave, Selected = false}
                };
            }
        }
    }

    public static class EmployeeUserConstants
    {
        public const string SuperAdmin = "Super Admin";
        public const string Employee = "Employee";

        public static string GetText(string searchTerm)
        {
            return Items.FindItemInList(searchTerm);
        }

        public static IEnumerable<ConstantDropdownItem> Items
        {
            get
            {
                return new List<ConstantDropdownItem>
                {
                    new ConstantDropdownItem {Text = "Super Admin", Value = SuperAdmin, Selected = true},
                    new ConstantDropdownItem {Text = "Employee", Value = Employee, Selected = false}
                };
            }
        }
    }

    public static class UserTypeConstants
    {
        public const string SuperAdmin = "SA";
        public const string Employee = "E";

        public static string GetText(string searchTerm)
        {
            return Items.FindItemInList(searchTerm);
        }

        public static IEnumerable<ConstantDropdownItem> Items
        {
            get
            {
                return new List<ConstantDropdownItem>
                {
                    new ConstantDropdownItem {Text = "Super Admin", Value = SuperAdmin, Selected = true},
                    new ConstantDropdownItem {Text = "Employee", Value = Employee, Selected = false}
                };
            }
        }
    }

    public static class LeaveImportSheetConstants
    {
        public const string EarnLeaveOpening = "EL_Opening";
        public const string CasualLeaveOpening = "CL_Opening";

        public static string GetText(string searchTerm)
        {
            return Items.FindItemInList(searchTerm);
        }

        public static IEnumerable<ConstantDropdownItem> Items
        {
            get
            {
                return new List<ConstantDropdownItem>
                {
                    new ConstantDropdownItem {Text = "Earn Leave Opening", Value = EarnLeaveOpening, Selected = true},
                    new ConstantDropdownItem {Text = "Casual Leave Opening", Value = CasualLeaveOpening, Selected = false}
                };
            }
        }
    }

    public static class SalaryRoundTypeConstants
    {
        public const string NotApplicable = "N/A";
        public const string RoundUp = "RoundUp";
        public const string RoundDown = "RoundDown";
        public const string RoundNormal = "RoundNormal";
        public static string GetText(string searchTerm)
        {
            return Items.FindItemInList(searchTerm);
        }

        public static IEnumerable<ConstantDropdownItem> Items
        {
            get
            {
                return new List<ConstantDropdownItem>
                {
                    new ConstantDropdownItem {Text = "Not Applicable", Value = NotApplicable, Selected = true},
                    new ConstantDropdownItem {Text = "Round Upper", Value = RoundUp, Selected = false},
                    new ConstantDropdownItem {Text = "Round Down", Value = RoundDown, Selected = false},
                    new ConstantDropdownItem {Text = "Round Normal", Value = RoundNormal, Selected = false},
                };
            }
        }
    }

    public static class PayrollTypeConstants
    {
        public const string CalendarDay = "CalendarDay";
        public const string FixedDays = "FixedDays";
        public static string GetText(string searchTerm)
        {
            return Items.FindItemInList(searchTerm);
        }

        public static IEnumerable<ConstantDropdownItem> Items
        {
            get
            {
                return new List<ConstantDropdownItem>
                {
                    new ConstantDropdownItem {Text = "Calendar Day", Value = CalendarDay, Selected = false},
                    new ConstantDropdownItem {Text = "Fixed Days", Value = FixedDays, Selected = false}
                };
            }
        }
    }

    public static class PayrollDepositTypeConstants
    {
        public const string SalaryDeposit = "Salary Deposit";
        public const string SalaryDepositRefund = "Salary Deposit Refund";
        public static string GetText(string searchTerm)
        {
            return Items.FindItemInList(searchTerm);
        }

        public static IEnumerable<ConstantDropdownItem> Items
        {
            get
            {
                return new List<ConstantDropdownItem>
                {
                    new ConstantDropdownItem {Text = "Salary Deposit", Value = SalaryDeposit, Selected = false},
                    new ConstantDropdownItem {Text = "Salary Deposit Refund", Value = SalaryDepositRefund, Selected = false}
                };
            }
        }
    }

    public static class TimeKeepingTypeConstants
    {
        public const string TimeKeepingException = "TimeKeepingException";
        public const string EmployeeRoaster = "EmployeeRoaster";
        public const string OfficeTimeException = "OfficeTimeException";
        public const string EmployeeLoginLogoutTime = "EmployeeLoginLogoutTime";
        public static string GetText(string searchTerm)
        {
            return Items.FindItemInList(searchTerm);
        }

        public static IEnumerable<ConstantDropdownItem> Items
        {
            get
            {
                return new List<ConstantDropdownItem>
                {
                    new ConstantDropdownItem {Text = "Time Keeping Exception", Value = TimeKeepingException, Selected = false},
                    new ConstantDropdownItem {Text = "Employee Roaster", Value = EmployeeRoaster, Selected = false},
                    new ConstantDropdownItem {Text = "Office Time Exception", Value = OfficeTimeException, Selected = false},
                    new ConstantDropdownItem {Text = "Employee Login/Logout Time", Value = EmployeeLoginLogoutTime, Selected = false},
                };
            }
        }
    }

    public static class AttendanceEventTypeConstants
    {
        public const string InTime = "InTime";
        public const string OutTime = "OutTime";
        public const string CIn = "C/In";
        public const string COut = "C/Out";

        public static string GetText(string searchTerm)
        {
            return Items.FindItemInList(searchTerm);
        }

        public static IEnumerable<ConstantDropdownItem> Items
        {
            get
            {
                return new List<ConstantDropdownItem>
                {
                    new ConstantDropdownItem {Text = "In Time", Value = InTime, Selected = false},
                    new ConstantDropdownItem {Text = "Out Time", Value = OutTime, Selected = false},
                    new ConstantDropdownItem {Text = "C/In", Value = CIn, Selected = false},
                    new ConstantDropdownItem {Text = "C/Out", Value = COut, Selected = false}
                };
            }
        }
    }

    public static class OfficeTypeConstants
    {
        public const string HeadOffice = "HO";
        public const string Project = "PR";
        public const string ZonalOffice = "ZO";
        public const string AreaOffice = "AR";
        public const string Branch_UnitOffice = "BO";

        public static string GetText(string searchTerm)
        {
            return Items.FindItemInList(searchTerm);
        }

        public static IEnumerable<ConstantDropdownItem> Items
        {
            get
            {
                return new List<ConstantDropdownItem>
                {
                    new ConstantDropdownItem {Text = "Head Office", Value = HeadOffice, Selected = false},
                    new ConstantDropdownItem {Text = "Project", Value = Project, Selected = false},
                    new ConstantDropdownItem {Text = "Zonal Office", Value = ZonalOffice, Selected = false},
                    new ConstantDropdownItem {Text = "Area Office", Value = AreaOffice, Selected = false},
                    new ConstantDropdownItem {Text = "Branch Unit Office", Value = Branch_UnitOffice, Selected = false}
                };
            }
        }
    }
    public static class LoanStateConstants
    {
        public const string Running = "1";
        public const string Clossed = "2";
        public const string RunningAndClossed = "3";
        public static string GetText(string searchTerm)
        {
            return Items.FindItemInList(searchTerm);
        }

        public static IEnumerable<ConstantDropdownItem> Items
        {
            get
            {
                return new List<ConstantDropdownItem>
                {
                    new ConstantDropdownItem {Text = "Running", Value = Running, Selected = true},
                    new ConstantDropdownItem {Text = "Clossed", Value = Clossed, Selected = false},
                    new ConstantDropdownItem {Text = "Running and Clossed", Value = RunningAndClossed, Selected = false},
                };
            }
        }
    }

    public static class PayrollConfigurationTypeConstants
    {
        public const string Gross = "GR";
        public const string Basic = "BC";
        public static string GetText(string searchTerm)
        {
            return Items.FindItemInList(searchTerm);
        }

        public static IEnumerable<ConstantDropdownItem> Items
        {
            get
            {
                return new List<ConstantDropdownItem>
                {
                    new ConstantDropdownItem {Text = "Gross", Value = Gross, Selected = true},
                    new ConstantDropdownItem {Text = "Basic", Value = Basic, Selected = false}
                };
            }
        }
    }


    public static class TransactionTypeConstants
    {
        public const string Credit = "Cr";
        public const string Debit = "Dr";
        public static string GetText(string searchTerm)
        {
            return Items.FindItemInList(searchTerm);
        }

        public static IEnumerable<ConstantDropdownItem> Items
        {
            get
            {
                return new List<ConstantDropdownItem>
                {
                    new ConstantDropdownItem {Text = "Credit", Value = Credit, Selected = false},
                    new ConstantDropdownItem {Text = "Debit", Value = Debit, Selected = false}
                };
            }
        }
    }

    public static class EmployeeDocumentTypeConstants
    {
        public const string Signature = "SIG";
        public const string SpecialSymbol = "SPS";
        public const string FingerPrint = "FGR";
        public static string GetText(string searchTerm)
        {
            return Items.FindItemInList(searchTerm);
        }

        public static IEnumerable<ConstantDropdownItem> Items
        {
            get
            {
                return new List<ConstantDropdownItem>
                {
                    new ConstantDropdownItem {Text = "Signature", Value = Signature, Selected = false},
                    new ConstantDropdownItem {Text = "Special Symbol", Value = SpecialSymbol, Selected = false},
                    new ConstantDropdownItem {Text = "Finger Print", Value = FingerPrint, Selected = false}
                };
            }
        }
    }

    //will be removed
    public static class SalaryGenerationTypeConstants
    {
        public const string PayScale = "PS";
        public const string NonPayScale = "NPS";
        public static string GetText(string searchTerm)
        {
            return Items.FindItemInList(searchTerm);
        }

        public static IEnumerable<ConstantDropdownItem> Items
        {
            get
            {
                return new List<ConstantDropdownItem>
                {
                    new ConstantDropdownItem {Text = "Pay-Scale", Value = PayScale, Selected = true},
                    new ConstantDropdownItem {Text = "Non Pay-Scale", Value = NonPayScale, Selected = false}
                };
            }
        }
    }

    public static class PFReportTypeConstants
    {
        public const string IndividualLoanLedger = "1";
        public const string LoanAndInterestCollectionfortheMonth = "2";
        public const string LoanVoucherDetails = "3";
        public const string LoanWiseCollectionList = "4";
        public const string OfficeWiseLoanSummary = "5";
        public const string LoanCollectionDetails = "6";
        public const string LoanStatistics = "7";
        public const string LoanDisbursementSummary = "8";

        public static string GetText(string searchTerm)
        {
            return Items.FindItemInList(searchTerm);
        }

        public static IEnumerable<ConstantDropdownItem> Items
        {
            get
            {
                return new List<ConstantDropdownItem>
                {
                    new ConstantDropdownItem {Text = "Individual Loan Ledger", Value = IndividualLoanLedger, Selected = false},
                    new ConstantDropdownItem {Text = "Loan and Interest Collection for the Month", Value = LoanAndInterestCollectionfortheMonth, Selected = false},
                    new ConstantDropdownItem {Text = "Loan Voucher Details", Value = LoanVoucherDetails, Selected = false},
                    new ConstantDropdownItem {Text = "Loan Wise Collection List", Value = LoanWiseCollectionList, Selected = false},
                    new ConstantDropdownItem {Text = "Office Wise Loan Summary", Value = OfficeWiseLoanSummary, Selected = false},
                    new ConstantDropdownItem {Text = "Loan Collection Details", Value = LoanCollectionDetails, Selected = false},
                    new ConstantDropdownItem {Text = "Loan Statistics", Value = LoanStatistics, Selected = false},
                    new ConstantDropdownItem {Text = "Loan Disbursement Summary", Value = LoanDisbursementSummary, Selected = false},
                };
            }
        }
    }

    public static class OvertimeExceptionTypeConstants
    {
        public const string PublicHoliday = "PublicHoliday";
        public const string Weekend = "Weekend";
        public const string WeekendAndPublicHoliday = "WeekendAndPublicHoliday";
        public const string OnlyWorkingDay = "OnlyWorkingDay";

        public static string GetText(string searchTerm)
        {
            return Items.FindItemInList(searchTerm);
        }

        public static IEnumerable<ConstantDropdownItem> Items
        {
            get
            {
                return new List<ConstantDropdownItem>
                {
                    new ConstantDropdownItem {Text = "Public Holiday", Value = PublicHoliday, Selected = false},
                    new ConstantDropdownItem {Text = "Weekend", Value = Weekend, Selected = false},
                    new ConstantDropdownItem {Text = "Weekend & Public Holiday", Value = WeekendAndPublicHoliday, Selected = true},
                    new ConstantDropdownItem {Text = "Only Working Day", Value = OnlyWorkingDay, Selected = false},
                };
            }
        }
    }

    public static class HolydayTypeConstants
    {
        public const string WeeklyHoliday = "WH";

        public static string GetText(string searchTerm)
        {
            return Items.FindItemInList(searchTerm);
        }

        public static IEnumerable<ConstantDropdownItem> Items
        {
            get
            {
                return new List<ConstantDropdownItem>
                {
                    new ConstantDropdownItem {Text = "Weekly Holiday", Value = WeeklyHoliday, Selected = true}
                };
            }
        }
    }

    public static class GCDesignationConstants
    {
        public const string Messenger = "4";
        public const string Driver = "5";
        public const string Support_Executive_MISITSystem = "6";
        public const string Junior_Executive_MISITSystem = "7";
        public const string Executive_MISITSystem = "8";
        public const string Senior_Executive = "9";
        public const string Deputy_Manager = "10";
        public const string Manager = "11";
        public const string Senior_Manager = "12";
        public const string AGM = "13";
        public const string DGM = "14";
        public const string General_Manager = "15";
        public const string MD = "16";
        public const string ED = "17";
        public const string MIS_Officer = "18";
        public const string Project_Manager = "19";
        public const string Software_Engineer = "20";
        public const string Senior_Software_Engineer = "21";
        public const string ASPnet_Software_Developer = "22";
        public const string Junior_Software_Developer = "23";
        public const string TADA_Bill_Checking_Officer = "24";
        public const string Junior_MIS_Officer = "25";
        public const string Software_Quality_Assurance_Engineer = "26";
        public const string Senior_Network_Engineer = "27";
        public const string Computer_Operator = "28";
        public const string Quality_Monitoring_Officer = "29";
        public const string Program_Officer = "30";
        public const string PHP_Developer = "31";
        public const string Sr_Business_Development__Operation_Manager = "32";
        public const string Health_Assistant = "33";
        public const string Program_Assistant = "34";
        public const string Business_Development_Associate_ = "35";
        public const string System_Analyst = "36";
        public const string Assistant_Coordinator = "37";
        public const string Junior_Programmer = "38";
        public const string IT_Specialist = "39";
        public const string Senior_Officer = "40";
        public const string Hardware_Engineer = "41";
        public const string Senior_MIS_Officer = "42";
        public const string Supervisor_ = "43";
        public const string Junior_Support_Engineer = "44";
        public const string Support_Engineer = "45";
        public const string Senior_Support_Assistant = "46";
        public const string Medical_Assistant = "47";
        public const string Assistant_Software_Engineer_ = "48";
        public const string Local_Coordinator = "49";
        public const string IT_Executive = "50";
        public const string Undefined_DesignationEx_Employees = "51";
        public const string Electrician = "52";

        public static string GetText(string searchTerm)
        {
            return Items.FindItemInList(searchTerm);
        }

        public static IEnumerable<ConstantDropdownItem> Items
        {
            get
            {
                return new List<ConstantDropdownItem>
                {
                    new ConstantDropdownItem {Text = "Messenger", Value = Messenger, Selected = false},
                    new ConstantDropdownItem {Text = "Driver", Value = Driver, Selected = false},
                    new ConstantDropdownItem {Text = "Support Executive (MIS/IT/System)", Value = Support_Executive_MISITSystem, Selected = false},
                    new ConstantDropdownItem {Text = "Junior Executive (MIS/IT/System)", Value = Junior_Executive_MISITSystem, Selected = false},
                    new ConstantDropdownItem {Text = "Executive (MIS/IT/System)", Value = Executive_MISITSystem, Selected = false},
                    new ConstantDropdownItem {Text = "Senior Executive", Value = Senior_Executive, Selected = false},
                    new ConstantDropdownItem {Text = "Deputy Manager", Value = Deputy_Manager, Selected = false},
                    new ConstantDropdownItem {Text = "Manager", Value = Manager, Selected = false},
                    new ConstantDropdownItem {Text = "Senior Manager", Value = Senior_Manager, Selected = false},
                    new ConstantDropdownItem {Text = "AGM", Value = AGM, Selected = false},
                    new ConstantDropdownItem {Text = "DGM", Value = DGM, Selected = false},
                    new ConstantDropdownItem {Text = "General Manager", Value = General_Manager, Selected = false},
                    new ConstantDropdownItem {Text = "MD", Value = MD, Selected = false},
                    new ConstantDropdownItem {Text = "ED", Value = ED, Selected = false},
                    new ConstantDropdownItem {Text = "MIS Officer", Value = MIS_Officer, Selected = false},
                    new ConstantDropdownItem {Text = "Project Manager", Value = Project_Manager, Selected = false},
                    new ConstantDropdownItem {Text = "Software Engineer", Value = Software_Engineer, Selected = false},
                    new ConstantDropdownItem {Text = "Senior Software Engineer", Value = Senior_Software_Engineer, Selected = false},
                    new ConstantDropdownItem {Text = "ASP.net Software Developer", Value = ASPnet_Software_Developer, Selected = false},
                    new ConstantDropdownItem {Text = "Junior Software Developer", Value = Junior_Software_Developer, Selected = false},
                    new ConstantDropdownItem {Text = "TA/DA Bill Checking Officer", Value = TADA_Bill_Checking_Officer, Selected = false},
                    new ConstantDropdownItem {Text = "Junior MIS Officer", Value = Junior_MIS_Officer, Selected = false},
                    new ConstantDropdownItem {Text = "Software Quality Assurance Engineer", Value = Software_Quality_Assurance_Engineer, Selected = false},
                    new ConstantDropdownItem {Text = "Senior Network Engineer", Value = Senior_Network_Engineer, Selected = false},
                    new ConstantDropdownItem {Text = "Computer Operator", Value = Computer_Operator, Selected = false},
                    new ConstantDropdownItem {Text = "Quality Monitoring Officer", Value = Quality_Monitoring_Officer, Selected = false},
                    new ConstantDropdownItem {Text = "Program Officer", Value = Program_Officer, Selected = false},
                    new ConstantDropdownItem {Text = "PHP Developer", Value = PHP_Developer, Selected = false},
                    new ConstantDropdownItem {Text = "Sr. Business Development & Operation Manager", Value = Sr_Business_Development__Operation_Manager, Selected = false},
                    new ConstantDropdownItem {Text = "Health Assistant", Value = Health_Assistant, Selected = false},
                    new ConstantDropdownItem {Text = "Program Assistant", Value = Program_Assistant, Selected = false},
                    new ConstantDropdownItem {Text = "Business Development Associate ", Value = Business_Development_Associate_, Selected = false},
                    new ConstantDropdownItem {Text = "System Analyst", Value = System_Analyst, Selected = false},
                    new ConstantDropdownItem {Text = "Assistant Coordinator", Value = Assistant_Coordinator, Selected = false},
                    new ConstantDropdownItem {Text = "Junior Programmer", Value = Junior_Programmer, Selected = false},
                    new ConstantDropdownItem {Text = "IT Specialist", Value = IT_Specialist, Selected = false},
                    new ConstantDropdownItem {Text = "Senior Officer", Value = Senior_Officer, Selected = false},
                    new ConstantDropdownItem {Text = "Hardware Engineer", Value = Hardware_Engineer, Selected = false},
                    new ConstantDropdownItem {Text = "Senior MIS Officer", Value = Senior_MIS_Officer, Selected = false},
                    new ConstantDropdownItem {Text = "Supervisor ", Value = Supervisor_, Selected = false},
                    new ConstantDropdownItem {Text = "Junior Support Engineer", Value = Junior_Support_Engineer, Selected = false},
                    new ConstantDropdownItem {Text = "Support Engineer", Value = Support_Engineer, Selected = false},
                    new ConstantDropdownItem {Text = "Senior Support Assistant", Value = Senior_Support_Assistant, Selected = false},
                    new ConstantDropdownItem {Text = "Medical Assistant", Value = Medical_Assistant, Selected = false},
                    new ConstantDropdownItem {Text = "Assistant Software Engineer ", Value = Assistant_Software_Engineer_, Selected = false},
                    new ConstantDropdownItem {Text = "Local Coordinator", Value = Local_Coordinator, Selected = false},
                    new ConstantDropdownItem {Text = "IT Executive", Value = IT_Executive, Selected = false},
                    new ConstantDropdownItem {Text = "Undefined Designation/Ex Employees", Value = Undefined_DesignationEx_Employees, Selected = false},
                    new ConstantDropdownItem {Text = "Electrician", Value = Electrician, Selected = false}
                };
            }
        }
    }

    public static class AuthServerConstants
    {
        public static string AUTH_PATH = "AUTHSERVER.APIROUTES";
        public static string CLIENT_ID = "AUTHSERVER.CLIENT_ID";
        public static string ID_CLIENT_HR_APP_DEMO_ASP_MVC_APP = "AUTHSERVER.ID_CLIENT_DEMO_ASP_MVC_APP";
        public static string ID_CLIENT_HEALTH_APP = "AUTHSERVER.ID_CLIENT_HEALTH_APP";
        public static string USERNAME = "AUTHSERVER.USERNAME";
        public static string PASSWORD = "AUTHSERVER.PASSWORD";
        public static string SYNC_API_PATH = "AUTHSERVER.APIROUTE";
    }

    public static class AuthServerClientConstants
    {
        public static string HR_APP = "AUTHSERVER.ID_CLIENT_DEMO_ASP_MVC_APP";
        public static string HEALTH_APP = "AUTHSERVER.ID_CLIENT_HEALTH_APP";
        public static string ACCOUNTING_APP = "AUTHSERVER.ID_CLIENT_ACCOUNTING_APP";
        public static string GetText(string searchTerm)
        {
            return Items.FindItemInList(searchTerm);
        }

        public static IEnumerable<ConstantDropdownItem> Items
        {
            get
            {
                return new List<ConstantDropdownItem>
                {
                    new ConstantDropdownItem {Text = "HR App", Value = HR_APP, Selected = true},
                    new ConstantDropdownItem {Text = "Health App", Value = HEALTH_APP, Selected = false},
                    new ConstantDropdownItem {Text = "Accounting App", Value = ACCOUNTING_APP, Selected = false}
                };
            }
        }

    }

    public static class KeyCloakErrorConstants
    {
        public const string Error = "invalid_grant";
    }

    public static class CookieConstants
    {
        public const string CURRENT_LOGGED_IN_ACCESSTOKEN = "COOKIE_CURRENT_LOGGED_IN_ACCESSTOKEN";
    }

    public static class EncashmentFormulaConstants
    {
        public const string HalfIfLessThanMinimum = "Half if less than Minimum";
    }

    public static class CoOperativeConstants
    {
        public const string ActivityStatus_Active = "A";
        public const string ActivityStatus_Delete = "D";
        public const string ActivityStatus_Close = "C";
        public const string ActivityStatus_Opening = "O";
        // Installment Opening=InsO,Installment=Ins,Installment Payment=InsP,
        // Interest Opening=IntO,Interest=Int,Interest Payment=IntP,
        public const string InstallmentType_Installment_Opening = "InsO";
        public const string InstallmentType_Installment = "Ins";
        public const string InstallmentType_Installment_Payment = "InsP";
        public const string InstallmentType_Interest_Opening = "IntO";
        public const string InstallmentType_Interest = "Int";
        public const string InstallmentType_Interest_Payment = "IntP";

    }
    public static class PFTransactionTypeConstants
    {
        public const string Delete = "D";
        public const string Contribution = "C";
        public const string Profit = "P";
        public const string FinalPayment = "F";
    }
    public static class ProfitDeclarationConstants
    {
        public const string Delete = "D";
        public const string Close = "C";
        public const string Approved = "A";
        public const string Entry = "E";
    }
}
