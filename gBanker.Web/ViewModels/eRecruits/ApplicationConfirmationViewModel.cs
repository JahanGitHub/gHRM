using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gHRM.Web.ViewModels.eRecruits
{
    public class ApplicationConfirmationViewModel
    {
        public int ApplicantId { get; set; }
        public string ApplicantName { get; set; }
        public string FatherName { get; set; }
        public string MotherName { get; set; }
        public string NationalId { get; set; }
        public string MobileNo { get; set; }
        public string Email { get; set; }
        public string Nationality { get; set; }
        public string Gender { get; set; }
        public string Religion { get; set; }
        public string DateOfBirth { get; set; }

    }
}