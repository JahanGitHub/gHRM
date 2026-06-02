using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace gHRM.Web.ViewModels
{
    public class EmployeeGuarantorInformationViewModel:BaseModel
    {
        [Key]
        public int GuarantorId { get; set; }
        public string GuarantorName { get; set; }
        public int? RelationwithemployeeId { get; set; }
        public int? OccupationId { get; set; }
        public string ContactNo { get; set; }
        public string NationalID { get; set; }
        public byte[] GuarantorImage { get; set; }
        public int? PresentCountryId { get; set; }
        public int? PresentDivisionId { get; set; }
        public int? PresentDistrictId { get; set; }
        public int? PresentThanaId { get; set; }
        public int? PresentUnionId { get; set; }
        public string PresentStreetOrHouse { get; set; }
        public string PresentZipCode { get; set; }

        public int? PermanentCountryId { get; set; }

        public int? PermanentDivisionId { get; set; }

        public int? PermanentDistrictId { get; set; }

        public int? PermanentThanaId { get; set; }

        public int? PermanentUnionId { get; set; }
        public string PermanentStreetOrHouse { get; set; }
        public string PermenantZipCode { get; set; }

        public bool? IsActive { get; set; }

        public DateTime? CreateDate { get; set; }

        public DateTime? UpdateDate { get; set; }

        public long? CreateBy { get; set; }

        public long? UpdateBy { get; set; }

        //public string CarTypeName { get; set; }
        //public IEnumerable<SelectListItem> CarTypeNameList { get; set; }
    }
}