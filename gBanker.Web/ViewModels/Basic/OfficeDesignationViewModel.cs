using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gHRM.Web.ViewModels
{
    public class OfficeDesignationViewModel
    {
        public int OfficeDesignationId { get; set; }
        public string OffcDesignName { get; set; }
        public string OffcDesignNameBn { get; set; }
        public string OffcType { get; set; }
        public string OfficeTypeName { get; set; }
        public string Rank { get; set; }
        public int DesignationOrder { get; set; }
        public bool? IsSectionDependent { get; set; }
        public IEnumerable<SelectListItem> OfficeTypeList { get; set; }
        public IEnumerable<SelectListItem> RankList { get; set; }
        public IEnumerable<SelectListItem> SectionDependentList { get; set; }
        public int rowSl { get; set; }

    }
}