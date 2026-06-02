using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Service
{

    public interface IAspNetRoleService : IServiceBase<AspNetRole>
    {
        Task<SSORoleMapping> GetSSORoleMapping(int roleId);
        Task<AspNetRole> AddNewRole(AspNetRole aspNetRole);
        Task<SSORoleMapping> AddNewSSORole(SSORoleMapping ssoRoleMapping);
        AspNetRole GetByRoleId(string roleId);
        AspNetRole GetByRoleName(string roleName);
        string GetNameById(string Id);
    }

    public class AspNetRoleService : IAspNetRoleService
    {
        private readonly IAspNetRoleRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public AspNetRoleService(IAspNetRoleRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        public async Task<SSORoleMapping> GetSSORoleMapping(int roleId)
        {
            try
            {
                var ssoRoleMapping = await repository.GetSSORoleMapping(roleId);

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
                var newAspNetRole = await repository.AddNewRole(aspNetRole);

                return newAspNetRole;
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
                return await repository.AddNewSSORole(ssoRoleMapping);
            }
            catch
            {
                return null;
            }
        }

        public IEnumerable<AspNetRole> GetAll()
        {
            var entities = repository.GetAll().OrderBy(c => c.Name);
            return entities;
        }

        public AspNetRole GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public AspNetRole GetByRoleName(string roleName)
        {
            var single = new AspNetRole();
            using (var db = new gHRMDBContext())
            {
                single = db.AspNetRoles.FirstOrDefault(f => f.Name == roleName);
            }                 
            return single;
        }

        public AspNetRole GetByRoleId(string roleId)
        {
            var single = new AspNetRole();
            using (var db = new gHRMDBContext())
            {
                single = db.AspNetRoles.FirstOrDefault(f => f.Id == roleId);
            }
            return single;
        }

        public AspNetRole Create(AspNetRole objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(AspNetRole objectToUpdate)
        {
            repository.Update(objectToUpdate);
            Save();
        }

        public void Delete(int id)
        {
            var entity = repository.GetById(id);
            repository.Delete(entity);
            Save();
        }

        public void Save()
        {
            unitOfWork.Commit();
        }

        public AspNetRole Get(Expression<Func<AspNetRole, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<AspNetRole> GetMany(Expression<Func<AspNetRole, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }

        #region Asyc
        public virtual async Task<IEnumerable<AspNetRole>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }


        public virtual async Task<IEnumerable<AspNetRole>> GetManyAsync(Expression<Func<AspNetRole, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }



        public virtual async Task<AspNetRole> GetAsync(Expression<Func<AspNetRole, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion
        public bool Inactivate(long id, DateTime? inactiveDate)
        {
            throw new NotImplementedException();
        }

        public bool IsContinued(long id)
        {
            throw new NotImplementedException();
        }

        public string GetNameById(string Id)
        {
            return repository.GetNameById(Id);
        }
    }
}
