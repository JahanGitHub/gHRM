using gHRM.Web.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;


namespace gHRM.Web.ViewModels
{

    public class GHealthPatientDetailsBasicCheckUpParentViewModel 
    {
        [JsonProperty("GHealthPatientDetailsBasicCheckUpViewModel")]
        public List<GHealthPatientDetailsBasicCheckUpViewModel> checkup_info { get; set; }
    }

    public class GHealthPatientDetailsBasicCheckUpViewModel 
    {
        public string checkup_date { get; set; }

        public string height { get; set; }
        public string weight { get; set; }
        public string bmi { get; set; }
        public string waist { get; set; }
        public string hip { get; set; }
        public string waist_hip_ratio { get; set; }
        public string temperature { get; set; }
        public string oxygen_of_blood { get; set; }
        public string bp_sys { get; set; }
        public string bp_dia { get; set; }
        public string blood_glucose { get; set; }
        public string blood_glucose_type { get; set; }
        public string blood_hemoglobin { get; set; }

        public string urinary_glucose { get; set; }
        public string urinary_protein { get; set; }
        public string urinary_urobilinogen { get; set; }

        public string urinary_ph { get; set; }
        public string pulse_rate { get; set; }
        public string arrhythmia { get; set; }
        public string cholesterol { get; set; }
        public string uric_acid { get; set; }
        public string hbsag { get; set; }
        public string color_status { get; set; }
  

    }
}