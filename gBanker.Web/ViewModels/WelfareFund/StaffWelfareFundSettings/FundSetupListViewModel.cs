using gHRM.Data.CodeFirstMigration.WelfareFund;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace gHRM.Web.ViewModels.WelfareFund.StaffWelfareFundSettings
{
    public class FundSetupListViewModel
    {
        public IEnumerable<FundSetup> StaffWelfareFundSettingList { get; set; }  
    }
}