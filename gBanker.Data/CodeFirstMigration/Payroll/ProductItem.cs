using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.CodeFirstMigration.Payroll
{
    [Table("prl.ProductItem")]
    public class ProductItem
    {
        [Key]
        public int ProductId { get; set; }
        public int ProductGroupId { get; set; }
        public int ProductTypeId { get; set; }
        public string ProductItemName { get; set; }
        public bool IsActive { get; set; }
        public Nullable<bool> IsSerialRequired { get; set; }
        public System.DateTime CreateDate { get; set; }
        public System.DateTime UpdateDate { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
    }
}
