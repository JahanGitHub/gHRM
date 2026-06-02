using gHRM.Data.CodeFirstMigration;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data
{
    public class UserManagementEntities : IdentityDbContext<ApplicationUser>
    {
        public UserManagementEntities()
            : base("gHRMDbContext", false)
        {

        }
    }
}

