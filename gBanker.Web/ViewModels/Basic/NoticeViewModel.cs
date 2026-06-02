using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace gHRM.Web.ViewModels.Basic
{
    public class NoticeViewModel
    {
        public int? rowSl { get; set; }
        public int NoticeId { get; set; }
        [AllowHtml]
        public string Title { get; set; }
        [Display(Name = "Notice Details")]
        public string NoticeText { get; set; }

        public DateTime PublishDate { get; set; }
        public DateTime LiveFrom { get; set; }
        public DateTime LiveTo { get; set; }


        [Display(Name = "Publish Date")]
        public string PublishDateMsg { get; set; }

        [Display(Name = "Live From")]
        public string LiveFromMsg { get; set; }

        [Display(Name = "Live To")]
        public string LiveToMsg { get; set; }
        [Display(Name = "Role")]
        public string RoleId { get; set; }
        [Display(Name = "Office Type")]
        public string OfficeTypeId { get; set; }
        public IEnumerable<SelectListItem> RoleList { get; set; }
        public IEnumerable<SelectListItem> OfficeTypeList { get; set; }
    }
}