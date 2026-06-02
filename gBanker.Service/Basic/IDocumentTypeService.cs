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
    public interface IDocumentTypeService : IServiceBase<DocumentType>
    {


    }
    public class DocumentTypeService : IDocumentTypeService
    {
        private readonly IDocumentTypeRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public DocumentTypeService(IDocumentTypeRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        public IEnumerable<DocumentType> GetAll()
        {
            var entities = repository.GetAll().Where(c => c.IsActive == true).OrderBy(c => c.DocumentTypeId);
            return entities;
        }

        public DocumentType GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public DocumentType Create(DocumentType objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(DocumentType objectToUpdate)
        {
            repository.Update(objectToUpdate);
            Save();
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

        public DocumentType Get(Expression<Func<DocumentType, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<DocumentType> GetMany(Expression<Func<DocumentType, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }

        #region Asyc
        public virtual async Task<IEnumerable<DocumentType>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }


        public virtual async Task<IEnumerable<DocumentType>> GetManyAsync(Expression<Func<DocumentType, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }



        public virtual async Task<DocumentType> GetAsync(Expression<Func<DocumentType, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion
    }
}
