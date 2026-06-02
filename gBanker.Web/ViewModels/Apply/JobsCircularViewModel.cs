using System;

using System.ComponentModel.DataAnnotations;


namespace gHRM.Web.ViewModels.Apply
{
    public class JobsCircularViewModel : BaseModel
    {
        [Display(Name = "Job Id")]
        [Required(ErrorMessage = "{0} is Required")]
        public Int64 JobId { get; set; }

        [Display(Name = "Post Name")]
        [StringLength(500, ErrorMessage = "Maximum length is {1}")]
        public string PostName { get; set; }

        [Display(Name = "Is Active")]
        public bool? IsActive { get; set; }

        [Display(Name = "Post Description")]
        [StringLength(500, ErrorMessage = "Maximum length is {1}")]
        public string PostDescription { get; set; }

        [Display(Name = "Upload PDF")]
        public string PDF { get; set; }
       public string rowSl { get; set; }

    }
}