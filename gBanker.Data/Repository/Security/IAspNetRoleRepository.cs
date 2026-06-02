using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using System;
using System.Linq;
using System.Data.Entity;
using System.Threading.Tasks;

namespace gHRM.Data.Repository
{
   

    public interface IAspNetRoleRepository : IRepository<AspNetRole>
    {
        Task<SSORoleMapping> GetSSORoleMapping(int roleId);
        Task<AspNetRole> AddNewRole(AspNetRole aspNetRole);

        Task<SSORoleMapping> AddNewSSORole(SSORoleMapping ssoRoleMapping);
        string GetNameById(string Id);
    }
    public class AspNetRoleRepository : RepositoryBaseCodeFirst<AspNetRole>, IAspNetRoleRepository
    {
        public AspNetRoleRepository(IDatabaseFactoryCodeFirst databaseFactory): base(databaseFactory)
        {

        }

        public async Task<SSORoleMapping> GetSSORoleMapping(int roleId)
        {
            try
            {
                var ssoRoleMapping = await DataContext.SSORoleMappings.FirstOrDefaultAsync(r=>r.RoleId== roleId);
                
                return ssoRoleMapping;
            }
            catch 
            {
                return null;
            }
        }

        public async Task<AspNetRole> AddNewRole(AspNetRole aspNetRole)
        {
            try
            {
                DataContext.AspNetRoles.Add(aspNetRole);
                await DataContext.SaveChangesAsync();

                return aspNetRole;
            }
            catch
            {
                return null;
            }
        }

        public async Task<SSORoleMapping> AddNewSSORole(SSORoleMapping ssoRoleMapping)
        {
            try
            {
                DataContext.SSORoleMappings.Add(ssoRoleMapping);
                await DataContext.SaveChangesAsync();

                return ssoRoleMapping;
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        public string GetNameById(string Id)
        {
            return DataContext.AspNetRoles.Where(x => x.Id == Id).Select(x => x.Name).FirstOrDefault();
        }
    }
}
