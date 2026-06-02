using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gHRM.Web.ViewModels
{
    public class TransferOfficeOrderViewModel : BaseModel
    {
        public int CCForOfficeOrderId { get; set; }

        public string CCForOfficeOrderName { get; set; }
        public string CCForOfficeOrderNameView { get; set; }

        public int? ViewOrder { get; set; }

        public bool IsActive { get; set; }

        public long CreateUser { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime CreateDate { get; set; }

        public long? UpdateUser { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? UpdateDate { get; set; }
        //public IEnumerable<SelectListItem> EmployeeList { get; set; }


        public string ReportPlacementType { get; set; }
        public IEnumerable<SelectListItem> ReportPlacementList { get; set; }
    }
}