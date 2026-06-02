using gHRM.Data.CodeFirstMigration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gHRM.Web.ViewModels.Loan
{
    public class LoanConfigCommonDropdown
    {
        public List<SelectListItem> LoanType(string loanType)
        {
            List<SelectListItem> lst = new List<SelectListItem>();
            lst.Add(new SelectListItem { Text = "Provident Fund Loan", Value = "PFL", Selected = (loanType == "PFL" ? true : false) });
            lst.Add(new SelectListItem { Text = "Co-Operative Loan", Value = "COL", Selected = (loanType == "COL" ? true : false) });
            lst.Add(new SelectListItem { Text = "Company Loan", Value = "CL", Selected = (loanType == "CL" ? true : false) });
            return lst;
        }

        public List<SelectListItem> LoanType2(string loanType)
        {
            List<SelectListItem> lst = new List<SelectListItem>();
            lst.Add(new SelectListItem { Text = "Provident Fund Loan", Value = "PFL", Selected = (loanType == "PFL" ? true : false) });
            lst.Add(new SelectListItem { Text = "House Loan", Value = "HL", Selected = (loanType == "HL" ? true : false) });
            lst.Add(new SelectListItem { Text = "Motor Loan", Value = "ML", Selected = (loanType == "ML" ? true : false) });
            return lst;
        }

        // Tazdik
        public List<SelectListItem> LoanType3(string loanType)
        {
            List<SelectListItem> lst = new List<SelectListItem>();
            lst.Add(new SelectListItem { Text = "Provident Fund Loan", Value = "PFL", Selected = (loanType == "PFL" ? true : false) });
            //.Add(new SelectListItem { Text = "Co-Operative Loan", Value = "COL", Selected = (loanType == "COL" ? true : false) });
            //lst.Add(new SelectListItem { Text = "Company Loan", Value = "CL", Selected = (loanType == "CL" ? true : false) });
            return lst;
        }

        public List<SelectListItem> GracePeriod(int? graceperiod)
        {
            List<SelectListItem> lst = new List<SelectListItem>();
            for(int i = 0; i <= 12; i++)
                lst.Add(new SelectListItem { Text = i.ToString()+(i<=1?" Month": " Months"), Value =i.ToString(), Selected = ((graceperiod??0) == i ? true : false) });
            return lst;
        }
        public List<SelectListItem> MethodType(string methodType)
        {
            List<SelectListItem> lst = new List<SelectListItem>();
            lst.Add(new SelectListItem { Text = "Decline Method", Value = "D", Selected = (methodType == "D" ? true : false) });
            lst.Add(new SelectListItem { Text = "Flat Method", Value = "F", Selected = (methodType == "F" ? true : false) });
            return lst;
        }
        public List<SelectListItem> CollectionFormat(string collectionFormat)
        {
            List<SelectListItem> lst = new List<SelectListItem>();
            lst.Add(new SelectListItem { Text = "First Principal then Interest Collection", Value = "PI", Selected = (collectionFormat == "PI" ? true : false) });
            lst.Add(new SelectListItem { Text = "Installment wise Collection", Value = "IC", Selected = (collectionFormat == "IC" ? true : false) });
            return lst;
        }
        public List<SelectListItem> PFContribution(string pfContribution)
        {
            List<SelectListItem> lst = new List<SelectListItem>();
            lst.Add(new SelectListItem { Text = "Both", Value = "Both", Selected = (pfContribution == "Both" ? true : false) });
            lst.Add(new SelectListItem { Text = "Self", Value = "Self", Selected = (pfContribution == "Self" ? true : false) });
            return lst;
        }

        public List<SelectListItem> FormName(string value)
        {
            List<SelectListItem> lst = new List<SelectListItem>();
            lst.Add(new SelectListItem { Value = "ACC", Text = "Accounts",Selected= (value== "ACC" ? true:false) });
            lst.Add(new SelectListItem { Value = "DAP", Text = "Disburse Application", Selected = (value == "DAP" ? true : false) });
            return lst;
        }
        public List<SelectListItem> TypeXPropose(string loanType, int id)
        {
            gHRMDBContext db = new gHRMDBContext();
            List<SelectListItem> lst = new List<SelectListItem>();
            lst.Add(new SelectListItem { Text = "Please Select", Value = "0", Selected = (id == 0 ? true : false) });

            lst.AddRange(db.LoanPurposes.Where(x => x.IsActive == true && x.LoanType == loanType).Select(s => new SelectListItem()
            {
                Text = s.PurposeName,
                Value = s.PurposeId.ToString(),
                Selected = (s.PurposeId == id ? true : false)
            }));
            return lst;
        }
    }

}