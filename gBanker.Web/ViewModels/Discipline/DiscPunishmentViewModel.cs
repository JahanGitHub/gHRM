using gHRM.Web.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gHRM.Web.ViewModels.Discipline
{
    public class DiscPunishmentViewModel : BaseModel
    {
        public int PunishmentId { get; set; }
        public int? PunishmentType { get; set; }

        [Display(Name = "Punishment Code (শাস্তি কোড)")]
        public string PunishmentCode { get; set; }

        [Display(Name = "Punishment Name (শাস্তির নাম)")]
        public string PunishmentName { get; set; }
        public int? SeniorityLossDay { get; set; }

        [Display(Name = "Remarks (মন্তব্য)")]
        public string Remarks { get; set; }

        public bool IsActive { get; set; }


        public DateTime? InActiveDate { get; set; }

        public long? CreateUser { get; set; }


        public DateTime? CreateDate { get; set; }

        public long? UpdateUser { get; set; }

        public DateTime? UpdateDate { get; set; }
    }
}