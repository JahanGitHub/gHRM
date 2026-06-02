using gHRM.Web.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gHRM.Web.ViewModels.Discipline
{
    public class DiscMemorendumDetailsViewModel
    {
        public int MemorendumDetailsId { get; set; }
        public int MemorendumMasterId { get; set; }
        public int CrimeId { get; set; }
        public string Remarks { get; set; }
    }
}