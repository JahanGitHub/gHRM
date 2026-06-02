//using System;
//using System.Collections.Generic;
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
    public interface IEmployeeFileAttachemntService : IServiceBase<EmployeeFileAttachemnt>
    {

    }
    public class EmployeeFileAttachemntService : IEmployeeFileAttachemntService
    {
        private readonly IEmployeeFileAttachemntRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public EmployeeFileAttachemntService(IEmployeeFileAttachemntRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<EmployeeFileAttachemnt> GetAll()
        {
            var entities = repository.GetAll().OrderBy(c => c.AttachmentId);
            return entities;
        }

        public EmployeeFileAttachemnt GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public EmployeeFileAttachemnt Create(EmployeeFileAttachemnt objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(EmployeeFileAttachemnt objectToUpdate)
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

        public EmployeeFileAttachemnt Get(Expression<Func<EmployeeFileAttachemnt, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<EmployeeFileAttachemnt> GetMany(Expression<Func<EmployeeFileAttachemnt, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }

        #region Asyc
        public virtual async Task<IEnumerable<EmployeeFileAttachemnt>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }


        public virtual async Task<IEnumerable<EmployeeFileAttachemnt>> GetManyAsync(Expression<Func<EmployeeFileAttachemnt, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }



        public virtual async Task<EmployeeFileAttachemnt> GetAsync(Expression<Func<EmployeeFileAttachemnt, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion

    }
}

