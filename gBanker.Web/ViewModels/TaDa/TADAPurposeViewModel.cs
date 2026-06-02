using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace gHRM.Web.ViewModels.TaDa
{
    public class TADAPurposeViewModel
    {
        public int Id { get; set; }
                
        [Display(Name="Purpose")]
        [Required(ErrorMessage ="{0} is Required")]
        [StringLength(150)]
        public string Purpose { get; set; }

        [StringLength(500)]
        [Display(Name="Remarks")]
        public string Remarks { get; set; }        
    }
}