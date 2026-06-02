using gHRM.Core.Utilities.Constants;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.CodeFirstMigration.PF;
using gHRM.Data.Repository.PF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Service.PF
{
    public interface IProfitDeclarationService : IServiceBase<ProfitDeclaration>
    {
        //IEnumerable<ProfitDeclaration> GetProfitDeclarationByYear(int declarationYear);
    }
    public class ProfitDeclarationService : IProfitDeclarationService
    {
          private readonly IProfitDeclarationRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public ProfitDeclarationService(IProfitDeclarationRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<ProfitDeclaration> GetAll()
        {
            var entities = repository.GetAll().Where(x => x.DeclarationStatus != ProfitDeclarationConstants.Delete);
            return entities;
        }
        public ProfitDeclaration GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }
        public void Save()
        {
            unitOfWork.Commit();
        }
        public ProfitDeclaration Create(ProfitDeclaration objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(ProfitDeclaration objectToUpdate)
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
        public bool Inactivate(long id, DateTime? inactiveDate)
        {
            //throw new NotImplementedException();
            var obj = repository.GetById(id);
            if (obj != null)
            {
                //obj.InActiveDate = DateTime.Now;
                //obj.IsActive = false;
                repository.Update(obj);
                Save();
                return true;
            }
            return false;
        }
        public bool IsContinued(long id)
        {
            // throw new NotImplementedException();
            var obj = repository.GetById(id);
            if (obj != null)
            {
                //var isActive = obj.IsActive;
                //if (isActive == false)
                //{
                //    return false;
                //}
            }
            return true;
        }

        public ProfitDeclaration Get(Expression<Func<ProfitDeclaration, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<ProfitDeclaration> GetMany(Expression<Func<ProfitDeclaration, bool>> where)
        {
            var entities = repository.GetMany(where).Where(x=>x.DeclarationStatus!= ProfitDeclarationConstants.Delete);
            return entities;
        }
        #region Asyc
        public virtual async Task<IEnumerable<ProfitDeclaration>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }
        public virtual async Task<IEnumerable<ProfitDeclaration>> GetManyAsync(Expression<Func<ProfitDeclaration, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }
        public virtual async Task<ProfitDeclaration> GetAsync(Expression<Func<ProfitDeclaration, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion
        //public IEnumerable<ProfitDeclaration> GetProfitDeclarationByYear(int declarationYear)
        //{
        //    return repository.GetProfitDeclarationByYear(declarationYear);
        //}
    }
}
