namespace gHRM.Data.CodeFirstMigration
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class View_EmployeeGuarantorInformation
    {
        [Key]
        public int? RowSl { get; set; }
        public int GuarantorId { get; set; }
        public string GuarantorName { get; set; }
        public long EmployeeId { get; set; }

        public int? GuarantorRelationshipId { get; set; }

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
        public string GuarantorRelationshipName { get; set; }

        public string OccupationName { get; set; }

        public string CountryName { get; set; }

        public string Name { get; set; }
        public string district_name_eng { get; set; }
        public string thana_name_eng { get; set; }
        public string union_name_eng { get; set; }
        public string EmployeeName { get; set; }
        public string PresentAddressDetail { get; set; }
        public string PermanentAddressDetail { get; set; }
        public string GRType { get; set; }
        public decimal GuaranteeMoney { get; set; }
        public string ReferenceORGuarantorDetail { get; set; }
    }
}
