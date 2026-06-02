using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.DBDetailModels.Payroll
{
    public class DBPRComponentViewModel
    {
        public int PRComponentID { get; set; }

        public string ComponentName { get; set; }

        public string ComponentType { get; set; }

        public decimal ComponentAmount { get; set; }

        public string TransactionType { get; set; }

        public string AccountCode { get; set; }

        public DateTime EffectiveStartDate { get; set; }

        public DateTime? EffectiveEndDate { get; set; }

        public int PRComponentGroupID { get; set; }

        public string ComponentCategory { get; set; }

        public bool IsActive { get; set; }


    }//
}
