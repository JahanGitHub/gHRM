using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.DBDetailModels.Security
{
    public class SSOUserRegistrationModel
    {
        public long EmployeeID { get; set; }
        public string EmployeeCode { get; set; }
        public int OfficeID { get; set; }
        public string OfficeCode { get; set; }
        public string EmpName { get; set; }
        public string EmpNameBen { get; set; }
        public string GuardianName { get; set; }
        public string EmpAddress { get; set; }
        public string PhoneNo { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Designation { get; set; }
        public DateTime JoiningDate { get; set; }        
        public DateTime? ReleaseDate { get; set; }
        public int OrganizationId { get; set; }
        public bool IsActive { get; set; }
        public DateTime? InActiveDate { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateUser { get; set; }

        //aspnet user
        public string ASPNETUserId { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ProfilePicUrl { get; set; }       
        public DateTime DateCreated { get; set; }
        public DateTime? LastLoginTime { get; set; }       
        public bool Activated { get; set; }        
        public string RoleName { get; set; }        
        public bool EmailConfirmed { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public string PhoneNumber { get; set; }       
        public bool PhoneNumberConfirmed { get; set; }        
        public bool TwoFactorEnabled { get; set; }
        public DateTime? LockoutEndDateUtc { get; set; }       
        public bool LockoutEnabled { get; set; }        
        public int AccessFailedCount { get; set; }
    }
}
