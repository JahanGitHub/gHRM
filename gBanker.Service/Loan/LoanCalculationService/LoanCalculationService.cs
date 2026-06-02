using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.Loan;
using gHRM.Data.CodeFirstMigration.Payroll;
using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Service.Loan.LoanCalculationService
{
    public class LoanCalculationService
    {
        public List<EmployeeMonthlySalary> LoanCalculationForPayrollProcess(DateTime salaryDate, int salaryMonth, int salaryYear, int? officeTypeid)
        {
            var lst = new List<EmployeeMonthlySalary>();

            try
            {
                gHRMDBContext db = new gHRMDBContext();
                var component_lst = (from lp in db.LoanPurposes
                                     join pr in db.PRComponents on lp.PurposeId equals pr.ComponentPayrollId
                                     where lp.IsActive && pr.IsActive && pr.ComponentCategory == "Loan"
                                     select new { lp.PurposeId, pr.PRComponentID, pr.OfficeLocationId, pr.EmployeeStatusId, pr.EmployeeTypeId }
                           ).ToList();
                if (component_lst.Any())
                {
                    var lon_disbLst = (from d in db.LoanDisbursements
                                       join e in db.Employees on d.EmployeeId equals e.EmployeeId
                                       join o in db.Offices on e.OfficeId equals o.OfficeId
                                       where d.IsDeleted == false && d.IsClose == false && e.IsActive && o.OfficeTypeId == (officeTypeid ?? o.OfficeTypeId)
                                       && SqlFunctions.DateAdd("month", d.GracePeriod, d.DisburseDate) <= salaryDate
                                       //&& d.DisburseDate.AddMonths(d.GracePeriod).Date <= salaryDate
                                       orderby new { d.EmployeeId, d.PurposeId }
                                       select new { d.LoanId, d.DisburseDate, d.GracePeriod, e.EmployeeId, e.OfficeId, e.EmployeeTypeId, e.EmployeeStatusId, o.OfficeLocationId, d.PurposeId, d.MethodType, d.DisburseAmount, d.MonthlyInstallment, d.NoOfInstallment, d.InstallmentInterest, d.InstallmentPrincipal, d.InterestCharge, d.IntersetRate, d.LoanPaid, d.InterestPaid }
                                       ).ToList();
                    if (lon_disbLst.Any())
                    {
                        foreach (var l  in lon_disbLst )
                        {
                            var loanObj = new EmployeeMonthlySalary();
                            loanObj.EmployeeId = l.EmployeeId;
                            loanObj.loanId = l.LoanId;
                            loanObj.OfficeId = l.OfficeId ?? 0;
                            loanObj.SalaryDate = salaryDate;
                            loanObj.SalaryMonth = salaryMonth;
                            loanObj.SalaryYear = salaryYear;
                            loanObj.TransactionType = "Dr";
                            loanObj.CreateDate = loanObj.UpdateDate = DateTime.Today;
                            loanObj.IsActive = true;//loanObj.IsApproved=loanObj.IsRejected = false;
                            loanObj.ComponentCategory = "Loan";

                                if (l.MethodType == "D")
                                {
                                    //decimal interestAmt = Interestcharge_decline(l.LoanId, salaryDate, l.DisburseDate/*.AddMonths(l.GracePeriod).Date*/, l.DisburseAmount, l.InterestCharge, l.LoanPaid, l.InterestPaid, l.IntersetRate, salaryMonth, salaryYear, l.InstallmentPrincipal, salaryDate );
                                decimal interestAmt = Interestcharge_decline_sp(l.LoanId, salaryDate, l.DisburseDate/*.AddMonths(l.GracePeriod).Date*/, l.DisburseAmount, l.InterestCharge, l.LoanPaid, l.InterestPaid, l.IntersetRate, salaryMonth, salaryYear, l.InstallmentPrincipal, salaryDate);

                                loanObj.PRComponentAmount = (l.DisburseAmount + l.InterestCharge + interestAmt) - (l.LoanPaid + l.InterestPaid) < l.MonthlyInstallment ? (l.DisburseAmount + l.InterestCharge + interestAmt) - (l.LoanPaid + l.InterestPaid) : l.MonthlyInstallment + interestAmt;

                                    //loanObj.PRComponentAmount = l.InstallmentPrincipal + interestAmt;

                                    if (component_lst.Where(x => x.PurposeId == l.PurposeId && x.EmployeeStatusId == l.EmployeeStatusId && x.EmployeeTypeId == l.EmployeeTypeId && x.OfficeLocationId == l.OfficeLocationId).Any())
                                        loanObj.PRComponentId = component_lst.Where(x => x.PurposeId == l.PurposeId && x.EmployeeStatusId == l.EmployeeStatusId && x.EmployeeTypeId == l.EmployeeTypeId && x.OfficeLocationId == l.OfficeLocationId).First().PRComponentID;
                                    lst.Add(loanObj);

                                }
                                else if (l.MethodType == "F")
                                {
                                    loanObj.PRComponentAmount = (l.DisburseAmount + l.InterestCharge) - (l.LoanPaid + l.InterestPaid) < l.MonthlyInstallment ? (l.DisburseAmount + l.InterestCharge) - (l.LoanPaid + l.InterestPaid) : l.MonthlyInstallment;
                                    if (component_lst.Where(x => x.PurposeId == l.PurposeId && x.EmployeeStatusId == l.EmployeeStatusId && x.EmployeeTypeId == l.EmployeeTypeId && x.OfficeLocationId == l.OfficeLocationId).Any())
                                        loanObj.PRComponentId = component_lst.Where(x => x.PurposeId == l.PurposeId && x.EmployeeStatusId == l.EmployeeStatusId && x.EmployeeTypeId == l.EmployeeTypeId && x.OfficeLocationId == l.OfficeLocationId).First().PRComponentID;
                                    lst.Add(loanObj);
                                }
                            }
                        }
                    }
             
            }
            catch (Exception ex)
            {
                lst = new List<EmployeeMonthlySalary>();
            }

            return lst;
        }


        public List<EmployeeMonthlySalary> LoanCalculationForPayrollProcess2(DateTime salaryDate, int salaryMonth, int salaryYear, int? officeTypeid)
        {
            var lst = new List<EmployeeMonthlySalary>();

            try
            {
                gHRMDBContext db = new gHRMDBContext();
                var component_lst = (from lp in db.LoanPurposes
                                     join pr in db.PRComponents on lp.PurposeId equals pr.ComponentPayrollId
                                     where lp.IsActive && pr.IsActive && pr.ComponentCategory == "Loan"
                                     select new { lp.PurposeId, pr.PRComponentID, pr.OfficeLocationId, pr.EmployeeStatusId, pr.EmployeeTypeId }
                           ).ToList();
                if (component_lst.Any())
                {
                    var lon_disbLst = (from d in db.LoanDisbursements
                                       join e in db.Employees on d.EmployeeId equals e.EmployeeId
                                       join o in db.Offices on e.OfficeId equals o.OfficeId
                                       where d.IsDeleted == false && d.IsClose == false && e.IsActive && o.OfficeTypeId == (officeTypeid ?? o.OfficeTypeId)
                                       && SqlFunctions.DateAdd("month", d.GracePeriod, d.DisburseDate) <= salaryDate
                                       //&& d.DisburseDate.AddMonths(d.GracePeriod).Date <= salaryDate
                                       orderby new { d.EmployeeId, d.PurposeId }
                                       select new { d.LoanId, d.DisburseDate, d.GracePeriod, e.EmployeeId, e.OfficeId, e.EmployeeTypeId, e.EmployeeStatusId, o.OfficeLocationId, d.PurposeId, d.MethodType, d.DisburseAmount, d.MonthlyInstallment, d.NoOfInstallment, d.InstallmentInterest, d.InstallmentPrincipal, d.InterestCharge, d.IntersetRate, d.LoanPaid, d.InterestPaid }
                                       ).ToList();
                    if (lon_disbLst.Any())
                    {
                        foreach (var l in lon_disbLst)
                        {
                            var loanObj = new EmployeeMonthlySalary();
                            loanObj.EmployeeId = l.EmployeeId;
                            loanObj.loanId = l.LoanId;
                            loanObj.OfficeId = l.OfficeId ?? 0;
                            loanObj.SalaryDate = salaryDate;
                            loanObj.SalaryMonth = salaryMonth;
                            loanObj.SalaryYear = salaryYear;
                            loanObj.TransactionType = "Dr";
                            loanObj.CreateDate = loanObj.UpdateDate = DateTime.Today;
                            loanObj.IsActive = true;//loanObj.IsApproved=loanObj.IsRejected = false;
                            loanObj.ComponentCategory = "Loan";

                            if (l.MethodType == "D")
                            {
                                //decimal interestAmt = Interestcharge_decline(l.LoanId, salaryDate, l.DisburseDate/*.AddMonths(l.GracePeriod).Date*/, l.DisburseAmount, l.InterestCharge, l.LoanPaid, l.InterestPaid, l.IntersetRate, salaryMonth, salaryYear, l.InstallmentPrincipal, salaryDate );
                                decimal interestAmt = Interestcharge_decline_sp(l.LoanId, salaryDate, l.DisburseDate/*.AddMonths(l.GracePeriod).Date*/, l.DisburseAmount, l.InterestCharge, l.LoanPaid, l.InterestPaid, l.IntersetRate, salaryMonth, salaryYear, l.InstallmentPrincipal, salaryDate);

                                loanObj.PRComponentAmount = (l.DisburseAmount + l.InterestCharge + interestAmt) - (l.LoanPaid + l.InterestPaid) < l.MonthlyInstallment ? (l.DisburseAmount + l.InterestCharge + interestAmt) - (l.LoanPaid + l.InterestPaid) : l.MonthlyInstallment + interestAmt;

                                //loanObj.PRComponentAmount = l.InstallmentPrincipal + interestAmt;

                                if (component_lst.Where(x => x.PurposeId == l.PurposeId && x.EmployeeStatusId == l.EmployeeStatusId && x.EmployeeTypeId == l.EmployeeTypeId && x.OfficeLocationId == l.OfficeLocationId).Any())
                                    loanObj.PRComponentId = component_lst.Where(x => x.PurposeId == l.PurposeId && x.EmployeeStatusId == l.EmployeeStatusId && x.EmployeeTypeId == l.EmployeeTypeId && x.OfficeLocationId == l.OfficeLocationId).First().PRComponentID;
                                lst.Add(loanObj);

                            }
                            else if (l.MethodType == "F")
                            {
                                loanObj.PRComponentAmount = (l.DisburseAmount + l.InterestCharge) - (l.LoanPaid + l.InterestPaid) < l.MonthlyInstallment ? (l.DisburseAmount + l.InterestCharge) - (l.LoanPaid + l.InterestPaid) : l.MonthlyInstallment;
                                if (component_lst.Where(x => x.PurposeId == l.PurposeId && x.EmployeeStatusId == l.EmployeeStatusId && x.EmployeeTypeId == l.EmployeeTypeId && x.OfficeLocationId == l.OfficeLocationId).Any())
                                    loanObj.PRComponentId = component_lst.Where(x => x.PurposeId == l.PurposeId && x.EmployeeStatusId == l.EmployeeStatusId && x.EmployeeTypeId == l.EmployeeTypeId && x.OfficeLocationId == l.OfficeLocationId).First().PRComponentID;
                                lst.Add(loanObj);
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                lst = new List<EmployeeMonthlySalary>();
            }

            return lst;
        }

        public void EmployeeMonthlySalaryApprovedProcess(List<EmployeeMonthlySalaryModel> loanLst, gHRMDBContext db)
        {
            try
            {
                if (loanLst.Any())
                {
                    foreach (var l in loanLst)
                    {
                        if ((l.LoanId ?? 0) > 0)
                        {
                            var loanDis = db.LoanDisbursements.FirstOrDefault(x => x.LoanId == l.LoanId);
                            decimal coll_Int = 0, coll_Prn = 0, interestCharge = 0;
                            if (loanDis.MethodType == "D")
                            {
                                interestCharge = Interestcharge(l.LoanId.Value, l.SalaryDate, loanDis.DisburseDate/*.AddMonths(loanDis.GracePeriod).Date*/, loanDis.DisburseAmount, loanDis.InterestCharge, loanDis.LoanPaid, loanDis.InterestPaid, loanDis.IntersetRate);
                                loanDis.InterestCharge += interestCharge;

                                var duePrincipla = loanDis.DisburseAmount - loanDis.LoanPaid;
                                var dueInterest = loanDis.InterestCharge - loanDis.InterestPaid;
                                if (duePrincipla >= l.PRComponentAmount) coll_Prn = l.PRComponentAmount;
                                else if (duePrincipla > 0 && (duePrincipla - l.PRComponentAmount) <= 0) coll_Prn =  duePrincipla;

                                coll_Int = l.PRComponentAmount - coll_Prn;
                            }
                            else if (loanDis.MethodType == "F")
                            {
                                coll_Prn = loanDis.InstallmentPrincipal + loanDis.InstallmentInterest == l.PRComponentAmount ? loanDis.InstallmentPrincipal : loanDis.DisburseAmount - loanDis.LoanPaid;
                                coll_Int = loanDis.InstallmentPrincipal + loanDis.InstallmentInterest == l.PRComponentAmount ? loanDis.InstallmentInterest : loanDis.InterestCharge - loanDis.InstallmentInterest;
                            }

                            loanDis.LoanPaid += coll_Prn;
                            loanDis.InterestPaid += coll_Int;
                            if ((loanDis.DisburseAmount + loanDis.InterestCharge) == (loanDis.LoanPaid + loanDis.InterestPaid)) loanDis.IsClose = true;
                            LoanCollection colObj = new LoanCollection()
                            {
                                Coll_InterestAmount = coll_Int,
                                Coll_LoanAmount = coll_Prn,
                                InterestCharge = interestCharge,
                                LoanId = l.LoanId.Value,
                                TransactionDate = l.SalaryDate,
                                TransactionType = "Cr",
                                Comments=l.Comments,
                                CreateDate=DateTime.Now,
                                CreateUser=l.CreateUser,
                            };
                            db.LoanCollections.Add(colObj);
                            db.SaveChanges();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }


        public void EmployeeMonthlySalaryApprovedProcess2(List<EmployeeMonthlySalaryModel> loanLst, gHRMDBContext db)
        {
            if (loanLst == null || !loanLst.Any())
                return;

            try
            {
                foreach (var l in loanLst)
                {
                    if (l.LoanId.GetValueOrDefault(0) <= 0)
                        continue;

                    var loanDis = db.LoanDisbursements.FirstOrDefault(x => x.LoanId == l.LoanId);
                    if (loanDis == null)
                        continue;

                    decimal coll_Int = 0, coll_Prn = 0, interestCharge = 0;

                    if (loanDis.MethodType == "D") // Declining Balance Method
                    {
                        int salaryMonth = l.SalaryDate.Month;
                        int salaryYear = l.SalaryDate.Year;

                        interestCharge = Interestcharge(
                            l.LoanId.Value,
                            l.SalaryDate,
                            loanDis.DisburseDate,
                            loanDis.DisburseAmount,
                            loanDis.InterestCharge,
                            loanDis.LoanPaid,
                            loanDis.InterestPaid,
                            loanDis.IntersetRate
                        );
                        loanDis.InterestCharge += interestCharge;

                        //decimal duePrincipal = loanDis.DisburseAmount - loanDis.LoanPaid;
                        //decimal dueInterest = loanDis.InterestCharge - loanDis.InterestPaid;

                        //if (duePrincipal >= l.PRComponentAmount)
                        //  coll_Prn = l.PRComponentAmount;
                        //else if (duePrincipal > 0)
                        //  coll_Prn = duePrincipal;

                        //coll_Int = Math.Min(l.PRComponentAmount - coll_Prn, dueInterest);

                        coll_Prn = loanDis.InstallmentPrincipal;
                        coll_Int = loanDis.InstallmentInterest;
                    }
                    else if (loanDis.MethodType == "F") // Flat Rate Method
                    {
                        decimal totalInstallment = loanDis.InstallmentPrincipal + loanDis.InstallmentInterest;
                        if (totalInstallment == l.PRComponentAmount)
                        {
                            coll_Prn = loanDis.InstallmentPrincipal;
                            coll_Int = loanDis.InstallmentInterest;
                        }
                        else
                        {
                            coll_Prn = Math.Min(loanDis.DisburseAmount - loanDis.LoanPaid, l.PRComponentAmount);
                            coll_Int = Math.Min(loanDis.InterestCharge - loanDis.InterestPaid, l.PRComponentAmount - coll_Prn);
                        }

                    }

                    // Update loan disbursement
                    loanDis.LoanPaid += coll_Prn;
                    loanDis.InterestPaid += coll_Int;
                    loanDis.IsClose = (loanDis.DisburseAmount + loanDis.InterestCharge) == (loanDis.LoanPaid + loanDis.InterestPaid);

                    // Create collection record
                    var colObj = new LoanCollection
                    {
                        Coll_InterestAmount = interestCharge,       // coll_Int,
                        Coll_LoanAmount = coll_Prn,
                        InterestCharge = interestCharge,
                        LoanId = l.LoanId.Value,
                        TransactionDate = l.SalaryDate,
                        TransactionType = "Cr",
                        Comments = l.Comments ?? $"Loan Deduction For The Month-{l.SalaryDate:MMMM yyyy}",
                        CreateDate = DateTime.Now,
                        CreateUser = l.CreateUser
                    };

                    db.LoanCollections.Add(colObj);
                }

                //db.SaveChanges();
                try
                { // KHALID ADD: 10 March 2026
                    db.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    foreach (var errors in ex.EntityValidationErrors)
                    {
                        foreach (var error in errors.ValidationErrors)
                        {
                            Console.WriteLine(error.PropertyName + ": " + error.ErrorMessage);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                // Log the exception (implement proper logging)
                throw; // Re-throw the exception after logging
            }
        }


        public void EmployeeMonthlySalaryApprovedProcess3(List<EmployeeMonthlySalaryModel> loanLst, gHRMDBContext db)
        {
            if (loanLst == null || !loanLst.Any())
                return;

            try
            {
                foreach (var l in loanLst)
                {
                    if (l.LoanId.GetValueOrDefault(0) <= 0)
                        continue;

                    var loanDis = db.LoanDisbursements.FirstOrDefault(x => x.LoanId == l.LoanId);
                    if (loanDis == null)
                        continue;

                    decimal coll_Int = 0, coll_Prn = 0, interestCharge = 0;

                    // Flat logic will be used for both 'D' and 'F'
                    decimal totalInstallment = loanDis.InstallmentPrincipal + loanDis.InstallmentInterest;
                    if (totalInstallment == l.PRComponentAmount)
                    {
                        coll_Prn = loanDis.InstallmentPrincipal;
                        coll_Int = loanDis.InstallmentInterest;
                    }
                    else
                    {
                        coll_Prn = Math.Min(loanDis.DisburseAmount - loanDis.LoanPaid, l.PRComponentAmount);
                        coll_Int = Math.Min(loanDis.InterestCharge - loanDis.InterestPaid, l.PRComponentAmount - coll_Prn);
                    }

                    // Update loan disbursement
                    loanDis.LoanPaid += coll_Prn;
                    loanDis.InterestPaid += coll_Int;
                    loanDis.IsClose = (loanDis.DisburseAmount + loanDis.InterestCharge) == (loanDis.LoanPaid + loanDis.InterestPaid);

                    // Create collection record
                    var colObj = new LoanCollection
                    {
                        Coll_InterestAmount = coll_Int,
                        Coll_LoanAmount = coll_Prn,
                        InterestCharge = interestCharge,
                        LoanId = l.LoanId.Value,
                        TransactionDate = l.SalaryDate,
                        TransactionType = "Cr",
                        Comments = l.Comments ?? $"Loan Deduction For The Month-{l.SalaryDate:MMMM yyyy}",
                        CreateDate = DateTime.Now,
                        CreateUser = l.CreateUser
                    };

                    db.LoanCollections.Add(colObj);
                }

                db.SaveChanges();
            }
            catch (Exception ex)
            {
                // Log the exception
                throw;
            }
        }



        public decimal Interestcharge
            (int loanid, DateTime collectionDate, DateTime disburseDate, decimal DisburseAmount, decimal InterestCharge, decimal LoanPaid, decimal InterestPaid, decimal IntersetRate)
        {
            int daydiff = 0;
            var TransactionDate = new gHRMDBContext().LoanCollections.Where(x => x.IsDeleted == false && x.LoanId == loanid).Select(x => x.TransactionDate);
            if (TransactionDate.Any())
                daydiff = (int)(collectionDate - TransactionDate.Max(c => c)).TotalDays;
            else daydiff = (int)(collectionDate - disburseDate).TotalDays;

            decimal balance = (DisburseAmount + InterestCharge) - (LoanPaid + InterestPaid);
            return decimal.Round(((balance * IntersetRate * daydiff) / 36500), 2, MidpointRounding.AwayFromZero);
        }


        public decimal Interestcharge_decline(
            int loanid, DateTime collectionDate, DateTime disburseDate, decimal DisburseAmount,
            decimal InterestCharge, decimal LoanPaid, decimal InterestPaid, decimal InterestRate,
            int salaryMonth, int salaryYear, decimal InstallmentPrincipal, DateTime salaryDate)
        {
            int daydiff = 0;
            decimal balance = 0;

            using (var db = new gHRMDBContext())
            {
                var transactionDate = db.LoanCollections
                    .Where(x => x.IsDeleted == false && x.LoanId == loanid)
                    .Select(x => x.TransactionDate)
                    .ToList(); // Ensure this is ToList to avoid deferred execution issues.

                var select = db.LoanDisbursements
                    .Where(x => x.IsDeleted == false && x.LoanId == loanid)
                    .FirstOrDefault();

                if (select == null)
                    throw new Exception("Loan Disbursement record not found.");

                if (transactionDate.Any())
                {
                    if (select.PaidOffDate == null)
                    {
                        daydiff = (int)(salaryDate - disburseDate).TotalDays;
                        balance = select.DisburseAmount;
                    }
                    else
                    {
                        daydiff = (int)(salaryDate - (DateTime)select.PaidOffDate).TotalDays;
                        balance = select.InstallmentPrincipal;

                    }
                }
                else
                {
                    daydiff = (int)(collectionDate - disburseDate).TotalDays;
                }



                // Update loan disbursement record
                var loanDisbursement = db.LoanDisbursements
                    .Where(x => x.IsDeleted == false && x.LoanId == loanid)
                    .FirstOrDefault();

                if (loanDisbursement != null)
                {
                    loanDisbursement.InstallmentPrincipal = loanDisbursement.DisburseAmount - loanDisbursement.MonthlyInstallment;
                    loanDisbursement.PaidOffDate = salaryDate;
                    db.SaveChanges();
                }

                return (balance * InterestRate * daydiff) / 36500;
            }
        }


        public decimal Interestcharge_decline_sp(
        int loanid, DateTime collectionDate, DateTime disburseDate, decimal DisburseAmount,
        decimal InterestCharge, decimal LoanPaid, decimal InterestPaid, decimal InterestRate,
        int salaryMonth, int salaryYear, decimal InstallmentPrincipal, DateTime salaryDate)
        {
            using (var db = new gHRMDBContext())
            {
                var sql = @"EXEC [dbo].[Interestcharge_decline] 
                    @loanid, @collectionDate, @disburseDate, @DisburseAmount, 
                    @InterestCharge, @LoanPaid, @InterestPaid, @InterestRate, 
                    @salaryMonth, @salaryYear, @InstallmentPrincipal, @salaryDate";

                var result = db.Database.SqlQuery<decimal>(
                    sql,
                    new SqlParameter("@loanid", loanid),
                    new SqlParameter("@collectionDate", collectionDate),
                    new SqlParameter("@disburseDate", disburseDate),
                    new SqlParameter("@DisburseAmount", DisburseAmount),
                    new SqlParameter("@InterestCharge", InterestCharge),
                    new SqlParameter("@LoanPaid", LoanPaid),
                    new SqlParameter("@InterestPaid", InterestPaid),
                    new SqlParameter("@InterestRate", InterestRate),
                    new SqlParameter("@salaryMonth", salaryMonth),
                    new SqlParameter("@salaryYear", salaryYear),
                    new SqlParameter("@InstallmentPrincipal", InstallmentPrincipal),
                    new SqlParameter("@salaryDate", salaryDate)
                ).FirstOrDefault();

                return result;
            }
        }


    }





    //        public decimal Interestcharge_decline
    //        (int loanid, DateTime collectionDate, DateTime disburseDate, decimal DisburseAmount, decimal InterestCharge, decimal LoanPaid, decimal InterestPaid, decimal IntersetRate, int salaryMonth, int salaryYear, decimal InstallmentPrincipal, DateTime salaryDate  )
    //        {
    //            int daydiff = 0;
    //            var db = new gHRMDBContext();

    //            var TransactionDate = db.LoanCollections.Where(x => x.IsDeleted == false && x.LoanId == loanid).Select(x => x.TransactionDate);
    //            var select = db.LoanDisbursements.Where(x => x.IsDeleted == false && x.LoanId == loanid).FirstOrDefault();



    //            if (TransactionDate.Any())
    //            {
    //                // daydiff = (int)(collectionDate - TransactionDate.Max(c => c)).TotalDays;
    //                if (select.PaidOffDate == null)
    //                    daydiff = (int)(salaryDate - disburseDate ).TotalDays;
    //                else
    //                    daydiff = (int)(salaryDate - (DateTime)select.PaidOffDate).TotalDays;
    //            }               
    //            else daydiff = (int)(collectionDate - disburseDate).TotalDays;

    //            int total_month = 0;
    //            total_month = daydiff / 30; 

    //            decimal principal_amount = 0;
    //            if (total_month == 1)
    //                principal_amount = DisburseAmount;
    //            else
    //                principal_amount = DisburseAmount - ( (total_month-1) * InstallmentPrincipal );

    //            decimal balance = (principal_amount) - (LoanPaid + InterestPaid);

    //            // update loan disburse table 

    //            var update = db.LoanDisbursements.Where(x => x.IsDeleted == false && x.LoanId == loanid).FirstOrDefault();

    //            update.InstallmentPrincipal = update.DisburseAmount - update.MonthlyInstallment;
    //            update.PaidOffDate = salaryDate;
    //            db.SaveChanges();




    //            return (balance * IntersetRate * daydiff ) / 36500 ;
    //            // return decimal.Round(((balance * IntersetRate * daydiff) / 36500), 2, MidpointRounding.AwayFromZero);
    //        }
    //}



    public class LoanCalculationViewModel
    {
        public long EmployeeId { get; set; }
        public int OfficeId { get; set; }
        public int loanId { get; set; }
        public int PrComponentId { get; set; }
        public string TransactionType { get; set; } = "Dr";
        public decimal Installment { get; set; }
    }
}
