using gHRM.Core.Common;
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.DBDetailModels;
using gHRM.Data.Repository;
using gHRM.Data.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Service
{
    public interface IOfficeTypeService : IServiceBase<OfficeType>
    {
        IEnumerable<DropDownAttribute> getOfficeTypeList();
    }
  public class OfficeTypeService : IOfficeTypeService
    {
       private readonly IOfficeTypeRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public OfficeTypeService(IOfficeTypeRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<OfficeType> GetAll()
        {
            var entities = repository.GetAll().Where(b=>b.IsActive==true).OrderBy(c => c.OfficeTypeId);
            return entities;
        }

        public OfficeType GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }
        public OfficeType Create(OfficeType objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(OfficeType objectToUpdate)
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
            throw new NotImplementedException();
        }
        public bool IsContinued(long id)
        {
            throw new NotImplementedException();
        }

        public OfficeType Get(Expression<Func<OfficeType, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<OfficeType> GetMany(Expression<Func<OfficeType, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }

        #region Asyc
        public virtual async Task<IEnumerable<OfficeType>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }


        public virtual async Task<IEnumerable<OfficeType>> GetManyAsync(Expression<Func<OfficeType, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }



        public virtual async Task<OfficeType> GetAsync(Expression<Func<OfficeType, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion

      public IEnumerable<DropDownAttribute> getOfficeTypeList()
        {
            return repository.getOfficeTypeList();
        }
    }
}
