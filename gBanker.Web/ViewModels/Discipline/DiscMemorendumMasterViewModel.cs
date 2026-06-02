using gHRM.Web.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gHRM.Web.ViewModels.Discipline
{
    public class DiscMemorendumMasterViewModel
    {
        public int MemorendumMasterId { get; set; }
        public string MemorendumNo { get; set; }
        public DateTime MemorendumDate { get; set; }
        public long EmployeeId { get; set; }
        public string DispatchNo { get; set; }
        public int? PunishmentId { get; set; }
        public bool IsPunishmentRunning { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeRank { get; set; }
        public string OfficeName { get; set; }
        public string DesignationName { get; set; }
        public int? CrimeId { get; set; }
        public string Remarks { get; set; }

        public IEnumerable<SelectListItem> PunishmentList { get; set; }
        public IEnumerable<SelectListItem> CrimeList { get; set; }
    }
}