using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gHRM.Web.ViewModels
{
    public class PromotionTypeViewModel : BaseModel
    {
        public int PromotionTypeId { get; set; }
        public int ViewOrder { get; set; }
        public string PromotionTypeName { get; set; }
        public string PromotionTypeValue { get; set; }
    }// End Class
}// End Namespace
