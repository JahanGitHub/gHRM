using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.CodeFirstMigration.PF
{
    [Table("gcpf.Vw_AccChart")]
    public partial class Vw_AccChart
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int AccID { get; set; }

        [StringLength(100)]
        public string AccCode { get; set; }
        
        [StringLength(100)]
        public string AccName { get; set; }

        public int? AccLevel { get; set; }

        [StringLength(50)]
        public string FirstLevel { get; set; }

        [StringLength(50)]
        public string SecondLevel { get; set; }

        [StringLength(50)]
        public string ThirdLevel { get; set; }

        [StringLength(50)]
        public string FourthLevel { get; set; }

        [StringLength(50)]
        public string FifthLevel { get; set; }

        public int? CategoryID { get; set; }

        [Key]
        [Column(Order = 1)]
        public bool IsTransaction { get; set; }

        [Key]
        [Column(Order = 2)]
        public bool IsRemoved { get; set; }

        [Key]
        [Column(Order = 3)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long CreateBy { get; set; }

        [Key]
        [Column(Order = 4)]
        public DateTime CreateDate { get; set; }

        public long? UpdateBy { get; set; }

        public DateTime? UpdateDate { get; set; }

        [StringLength(100)]
        public string ParentAccCode { get; set; }
        
    }
}
