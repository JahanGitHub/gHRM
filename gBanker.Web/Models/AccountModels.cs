using gHRM.Web.ViewModels.Dashboard;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Globalization;
using System.Web.Security;

namespace gHRM.Web.Models
{
    public class UsersContext : DbContext
    {
        public UsersContext()
            : base("gHRMDbContext")
        {
        }

        public DbSet<UserProfile> UserProfiles { get; set; }
    }

    [Table("UserProfile")]
    public class UserProfile
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }
        public string UserName { get; set; }
    }

    public class RegisterExternalLoginModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        public string ExternalLoginData { get; set; }
    }

    public class LocalPasswordModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class LoginModel
    {
        public LoginModel()
        {
            this.SSOInstances = new List<SSOInstanceViewModel>();
        }

        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }

        public string ReturnUrl { get; set; }
        public bool LogoutOperation { get; set; }
        public bool CanShowLoginPanel { get; set; }

        public List<SSOInstanceViewModel> SSOInstances { get; set; }
    }

    public class RegisterModel
    {
        public string Id { get; set; }
        [Required]
        [Display(Name = "Login Name (লগ-ইন নাম)")]
        public string UserName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password (পাসওয়ার্ড)")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password (পাসওয়ার্ড নিশ্চিত করুন)")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [Display(Name = "Security Role (নিরাপত্তা ভূমিকা)")]
        [Range(1, 10000, ErrorMessage = "Security Role is required.")]
        public int RoleId { get; set; }


        public long EmployeeId { get; set; }
        public string FirstName { get; set; }       
    }    

    public class ExternalLogin
    {
        public string Provider { get; set; }
        public string ProviderDisplayName { get; set; }
        public string ProviderUserId { get; set; }
    }
}
