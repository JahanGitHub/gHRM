using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.DBDetailModels.Apply
{
    public class AddorEditApplicantJobExperience
    {

        public Int64 Id { get; set; }

        public Int64 ApplicantId { get; set; }

        public string CompanyName { get; set; }


        public string CompanyBusiness { get; set; }

        public string Designation { get; set; }


        public string AreaofExperiences { get; set; }


        public string Responsibilities { get; set; }


        public string CompanyLocation { get; set; }

        public DateTime? JobStartDate { get; set; }

        public DateTime? JobEndDate { get; set; }

        public bool active { get; set; }
    }
}
