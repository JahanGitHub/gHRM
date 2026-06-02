using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.CodeFirstMigration.Payroll;
using gHRM.Data.Repository.Payroll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Service.Payroll
{
    public interface IEASSDesignationService : IServiceBase<EASSDesignation>
    {

    }
    public class EASSDesignationService : IEASSDesignationService
    {
        private readonly IEASSDesignationRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public EASSDesignationService(IEASSDesignationRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<EASSDesignation> GetAll()
        {
            var entities = repository.GetAll();
            return entities;
        }

        public EASSDesignation GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public EASSDesignation Create(EASSDesignation objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(EASSDesignation objectToUpdate)
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


        public EASSDesignation Get(Expression<Func<EASSDesignation, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<EASSDesignation> GetMany(Expression<Func<EASSDesignation, bool>> where)
        {
            var entities = repository.GetMany(where);
            return entities;
        }

        #region Asyc
        public virtual async Task<IEnumerable<EASSDesignation>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }


        public virtual async Task<IEnumerable<EASSDesignation>> GetManyAsync(Expression<Func<EASSDesignation, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }



        public virtual async Task<EASSDesignation> GetAsync(Expression<Func<EASSDesignation, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion
    }
}
