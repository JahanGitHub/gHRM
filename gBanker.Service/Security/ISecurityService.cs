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
   
    public interface ISecurityService : IServiceBase<AspNetSecurityModule>
    {
        IEnumerable<AspNetSecurityModule> GetAllPrentModule();
        IEnumerable<AspNetSecurityModule> GetAllModulesForParent(int parentModuleId, int roleId);
        void CreateSecurityRole(List<AspNetRoleModule> roleModules);
        IEnumerable<AspNetSecurityModule> GeAllRoleModules(int roleId);
    }
   public class SecurityService : ISecurityService
    {
        private readonly ISecurityRepository repository;
        private readonly IAspNetSecurityModuleRepository nRepository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;


        public SecurityService(ISecurityRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;           
        }


        public IEnumerable<AspNetSecurityModule> GetAllPrentModule()
        {
            return repository.GetAllPrentModule();
        }

        public IEnumerable<AspNetSecurityModule> GetAllModulesForParent(int parentModuleId, int roleId)
        {
            return repository.GetAllModulesForParent(parentModuleId, roleId);
        }

        public IEnumerable<AspNetSecurityModule> GetAll()
        {
            throw new NotImplementedException();
        }

        public AspNetSecurityModule GetById(int id)
        {
            throw new NotImplementedException();
        }

        public AspNetSecurityModule Create(AspNetSecurityModule objectToCreate)
        {
            throw new NotImplementedException();
        }

        public void Update(AspNetSecurityModule objectToUpdate)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public bool Inactivate(long id, DateTime? inactiveDate)
        {
            throw new NotImplementedException();
        }

        public bool IsContinued(long id)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            unitOfWork.Commit();
        }


        public void CreateSecurityRole(List<AspNetRoleModule> roleModules)
        {
            repository.CreateSecurityRole(roleModules);
            Save();
        }


        public IEnumerable<AspNetSecurityModule> GeAllRoleModules(int roleId)
        {
            return repository.GeAllRoleModules(roleId);
        }

        public AspNetSecurityModule Get(Expression<Func<AspNetSecurityModule, bool>> where)
        {
            var entities = nRepository.Get(where);
            return entities;
        }
        public IEnumerable<AspNetSecurityModule> GetMany(Expression<Func<AspNetSecurityModule, bool>> where)
        {
            var entities = nRepository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }

        #region Asyc
        public virtual async Task<IEnumerable<AspNetSecurityModule>> GetAllAsync()
        {

            return await nRepository.GetAllAsync();
        }


        public virtual async Task<IEnumerable<AspNetSecurityModule>> GetManyAsync(Expression<Func<AspNetSecurityModule, bool>> where)
        {
            return await nRepository.GetManyAsync(where);
        }



        public virtual async Task<AspNetSecurityModule> GetAsync(Expression<Func<AspNetSecurityModule, bool>> where)
        {
            return await nRepository.GetAsync(where);
        }
        #endregion
    }
}
