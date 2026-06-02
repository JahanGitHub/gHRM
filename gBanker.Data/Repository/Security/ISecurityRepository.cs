using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.Repository
{

    public interface ISecurityRepository : IRepository<AspNetRoleModule>
    {
        IEnumerable<AspNetSecurityModule> GetAllPrentModule();

        IEnumerable<AspNetSecurityModule> GetAllModulesForParent(int parentModuleId, int roleId);
        void CreateSecurityRole(List<AspNetRoleModule> roleModules);
        IEnumerable<AspNetSecurityModule> GeAllRoleModules(int roleId);
    }
    public class SecurityRepository : RepositoryBaseCodeFirst<AspNetRoleModule>, ISecurityRepository
    {
        public SecurityRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }

        public IEnumerable<AspNetSecurityModule> GetAllPrentModule()
        {
            var moduels = DataContext.AspNetSecurityModules.Where(w => !w.ParentModuleId.HasValue && (w.IsActive??true));
            return moduels;
        }

        public IEnumerable<AspNetSecurityModule> GetAllModulesForParent(int parentModuleId, int roleId)
        {

            var allModles = DataContext.AspNetSecurityModules.Where(p => p.ParentModuleId.Value == parentModuleId && (p.IsActive ?? true)).ToList();
            var query = DataContext.AspNetRoleModules.Where(w => w.RoleId == roleId.ToString() && (w.IsActive));
            var roleModles = query.ToList();
            foreach (var m in allModles)
            {
                var securityExists = roleModles.Where(rm => rm.ModuleId == m.AspNetSecurityModuleId).FirstOrDefault();
                if (securityExists != null)
                {
                    m.RoleId = roleId;
                    m.IsSelectedForRole = true;
                    m.SecurityLevelId = securityExists.SecurityLevelId;
                }
                else
                {
                    m.IsSelectedForRole = false;
                    m.SecurityLevelId = 1;
                }
            }

            return allModles;
        }


        public void CreateSecurityRole(List<AspNetRoleModule> roleModules)
        {
            foreach (var roleModule in roleModules)
            {
                var existingRoleModule = DataContext.AspNetRoleModules.Where(w => w.RoleId == roleModule.RoleId && w.ModuleId == roleModule.ModuleId).FirstOrDefault();
                if (roleModule.IsSelectedForRole)
                {
                    if (existingRoleModule == null)
                    {
                        roleModule.CreateDate = DateTime.Now;
                        Add(roleModule);
                    }
                    else
                    {
                        existingRoleModule.SecurityLevelId = roleModule.SecurityLevelId;
                        Update(existingRoleModule);
                    }
                }
                else
                {
                    if (existingRoleModule != null)
                        Delete(existingRoleModule);
                }
            }
        }


        public IEnumerable<AspNetSecurityModule> GeAllRoleModules(int roleId)
        {
            var allModles = DataContext.AspNetSecurityModules.Where(p => p.ParentModuleId.HasValue).ToList();
            var query = DataContext.AspNetRoleModules.Where(w => w.RoleId == roleId.ToString());
            var roleModles = query.ToList();
            var newList = new List<AspNetSecurityModule>();
            foreach (var m in allModles)
            {
                var securityExists = roleModles.Where(rm => rm.ModuleId == m.AspNetSecurityModuleId).FirstOrDefault();
                if (securityExists != null)
                {
                    m.RoleId = roleId;
                    m.IsSelectedForRole = true;
                    m.SecurityLevelId = securityExists.SecurityLevelId;
                    newList.Add(m);
                }
            }

            return newList;
        }
      

    }
}
