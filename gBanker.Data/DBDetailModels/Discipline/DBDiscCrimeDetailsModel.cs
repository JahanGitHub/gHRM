using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.DBDetailModels.Discipline
{
    public class DBDiscCrimeDetailsModel
    {

        public int CrimeId { get; set; }


        public string CrimeCode { get; set; }


        public string CrimeName { get; set; }


        public string Remarks { get; set; }

        public bool IsActive { get; set; }


        public DateTime? InActiveDate { get; set; }

        public long? CreateUser { get; set; }


        public DateTime? CreateDate { get; set; }

        public long? UpdateUser { get; set; }


        public DateTime? UpdateDate { get; set; }
    }
}
