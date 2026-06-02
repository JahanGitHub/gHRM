using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gHRM.Web.ViewModels.TaDa
{
    public class EmployeeTADABillViewModel
    {
        public int TADABillId { get; set; }
        public long EmployeeId { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string DepartmentName { get; set; }
        public string DesignationName { get; set; }
        public string OfficeName { get; set; }
        public int MemoNo { get; set; }
        public DateTime TravelDate { get; set; }
        public string TravelPlace { get; set; }
        public string TravelPurpose { get; set; }
        public DateTime ApproveDate { get; set; }
        public decimal ClaimAmount { get; set; }
        public decimal ApproveAmount { get; set; }
        public bool? IsAmountPaid { get; set; }
        public string Remark { get; set; }
        public string rowSl { get; set; }

        public string TravelDateMsg { get; set; }
        public string ApproveDateMsg { get; set; }
        public int Year { get; set; }
        public IEnumerable<SelectListItem> YearList { get; set; }
        public int Month { get; set; }
        public IEnumerable<SelectListItem> MonthList { get; set; }
        public int? OfficeTypeId { get; set; }
        public IEnumerable<SelectListItem> OfficeTypeList { get; set; }
        public int EmployeeRank { get; set; }
        public IEnumerable<SelectListItem> EmployeeRankList { get; set; }

        public int TravelPurposeId { get; set; }
        public IEnumerable<SelectListItem> TravelPurposeNameList { get; set; }


        




        [Display(Name = "Zone Name")]
        public string ZoneId { get; set; }
        [Display(Name = "Area Name")]
        public string AreaId { get; set; }
        [Display(Name = "Unit Name")]
        public string UnitId { get; set; }
        public int? HeadOfficeId { get; set; }
        public int? ProjectId { get; set; }
        public int? OfficeId { get; set; }
        public IEnumerable<SelectListItem> ZoneList { get; set; }
        public IEnumerable<SelectListItem> AreaList { get; set; }
        public IEnumerable<SelectListItem> UnitList { get; set; }
        public IEnumerable<SelectListItem> OfficeList { get; set; }
        public IEnumerable<SelectListItem> DesignationNameList { get; set; }
        public IEnumerable<SelectListItem> DepartmentNameList { get; set; }
        public IEnumerable<SelectListItem> EmployeeNameList { get; set; }

        public int? TravelPlaceId { get; set; }
        public int? ZoneOfficeId { get; set; }
        public int? AreaOfficeId { get; set; }
        public int? UnitOfficeId { get; set; }
        
        public int DepartmentId { get; set; }
        public int DesignationId { get; set; }


    }
}