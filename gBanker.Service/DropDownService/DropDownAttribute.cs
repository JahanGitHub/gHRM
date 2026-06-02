using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NUPMS.Service.DropDownService
{
    public class DropDownAttribute
    {
        public int Id { get; set; }
        public Int64 Id64 { get; set; }
        public string Name { get; set; }
        public string NameOther { get; set; }
        public int OtherId { get; set; }
        public string OtherString { get; set; }
        public bool Selected { get; set; }
        public bool Disabled { get; set; }
        public string GroupBy { get; set; }
        public int? DefaultValue { get; set; }
    }
}
