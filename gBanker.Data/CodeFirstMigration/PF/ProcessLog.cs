using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.CodeFirstMigration.PF
{
   [Table("gcpf.ProcessLog")]
   public partial class ProcessLog
    {
       [Key] 
       [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
       public long ProcessLogId { get; set; }

       [Required]
       [DataType(DataType.Date)]
       public DateTime StartDate { get; set; }
      
       [Required]
       [DataType(DataType.Date)]
       public DateTime SystemDateAtDayStart { get; set; }

       [DataType(DataType.Date)]
       public DateTime? SystemDateAtDayEnd { get; set; }

       [Required]
       public bool IsOpen { get; set; }

       [NotMapped]
       public DateTime TransactionDate {get;set;}

       [NotMapped]
       public string TransactionDateString { get; set; }

       [NotMapped]
       public string SystemDate { get; set; }
       
       [NotMapped]
       public string DayStatus {get;set;}

       [Required]
       public long CreateUser { get; set; }

       [Column(TypeName = "smalldatetime")]
       public DateTime? CreateDate { get; set; }

       public long? UpdateUser { get; set; }

       [Column(TypeName = "smalldatetime")]
       public DateTime? UpdateDate { get; set; }

       public bool IsDeleted { get; set; }

       public long? DeletedUser { get; set; }

       [Column(TypeName = "smalldatetime")]
       public DateTime? DeleteDate { get; set; }
    }
}
