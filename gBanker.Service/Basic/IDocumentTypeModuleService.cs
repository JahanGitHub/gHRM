using gHRM.Core.Common;
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
    public interface IDocumentTypeModuleService : IServiceBase<DocumentTypeModule>
    {

    }
    public class DocumentTypeModuleService : IDocumentTypeModuleService
    {
        private readonly IDocumentTypeModuleRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public DocumentTypeModuleService(IDocumentTypeModuleRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<DocumentTypeModule> GetAll()
        {
            var entities = repository.GetAll().OrderBy(c => c.DocumentTypeModuleId);
            return entities;
        }

        public DocumentTypeModule GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public DocumentTypeModule Create(DocumentTypeModule objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(DocumentTypeModule objectToUpdate)
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


        public bool Inactivate(long id, DateTime? inactiveDate)
        {
            throw new NotImplementedException(); ;
        }


        public bool IsContinued(long id)
        {
            throw new NotImplementedException();
        }

        public DocumentTypeModule Get(Expression<Func<DocumentTypeModule, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<DocumentTypeModule> GetMany(Expression<Func<DocumentTypeModule, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }

        #region Asyc
        public virtual async Task<IEnumerable<DocumentTypeModule>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }


        public virtual async Task<IEnumerable<DocumentTypeModule>> GetManyAsync(Expression<Func<DocumentTypeModule, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }



        public virtual async Task<DocumentTypeModule> GetAsync(Expression<Func<DocumentTypeModule, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion

    }
}













