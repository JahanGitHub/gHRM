namespace gHRM.Data.CodeFirstMigration
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("AttHolidayDeclaration")]
    public partial class AttHolidayDeclaration
    {
        public long AttHolidayDeclarationId { get; set; }

        public int HolidayYear { get; set; }

        [Column(TypeName = "date")]
        public DateTime HolidayDate { get; set; }

        public int AttHolidayTypeId { get; set; }

        public bool? IsActive { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? InActiveDate { get; set; }

        public long? CreateUser { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? CreateDate { get; set; }

        public long? UpdateUser { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? UpdateDate { get; set; }
        public int OfficeId { get; set; }
    }
}
