using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.DBDetailModels.Apply
{
    public class AddorEditApplicantMasterInfo
    {

        public Int64 ID { get; set; }


        public string FirstName { get; set; }


        public string LastName { get; set; }


        public string FatherName { get; set; }


        public string MotherName { get; set; }


        public string GuardianName { get; set; }


        public System.DateTime DateofBirth { get; set; }

        public string Gender { get; set; }

        public string Religion { get; set; }


        public string MaritalStatus { get; set; }


        public string Nationality { get; set; }


        public decimal? NationalId { get; set; }


        public decimal? PassportNumber { get; set; }


        public DateTime? PassportIssueDate { get; set; }


        public string PrimaryMobile { get; set; }


        public string SecondaryMobile { get; set; }


        public string PrimaryEmail { get; set; }

        public string BloodGroup { get; set; }


        public string CareerObjective { get; set; }


        public decimal? PresentSalary { get; set; }


        public decimal? ExpectedSalary { get; set; }


        public string LookingforJob_Level { get; set; }


        public string Availablefor { get; set; }

        public string CareerSummary { get; set; }


        public string SpecialQualification { get; set; }


        public string Image { get; set; }

        public bool? active { get; set; }

        public string QualificationKeyword { get; set; }

        public string KindsOfDisability { get; set; }

        public decimal? DisabilityId { get; set; }

        public Int64? UserId { get; set; }
    }
}

