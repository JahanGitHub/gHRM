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
    public class GHealthPatientDetailsViewModel : BaseModel
    {
        public string account_id { get; set; }
        public string role_id { get; set; }

        public string project_id { get; set; }
        public string id { get; set; }
        public string email { get; set; }
        public string username { get; set; }
        public string barcode_id { get; set; }
        public string fullname { get; set; }
        public string guardian_name { get; set; }
        public string dateofbirth { get; set; }
        public string gender { get; set; }
        public string picture { get; set; }
        public string age { get; set; }
        public string mobile { get; set; }
        public string birth_id { get; set; }

        public string create_date { get; set; }
        public string blood_group { get; set; }
        public string address { get; set; }


    }
}