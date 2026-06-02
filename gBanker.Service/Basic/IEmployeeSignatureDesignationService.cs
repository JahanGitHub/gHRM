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
    public interface IEmployeeSignatureDesignationService : IServiceBase<EmployeeSignatureDesignation>
    {


    }
    public class EmployeeSignatureDesignationService : IEmployeeSignatureDesignationService
    {
        private readonly IEmployeeSignatureDesignationRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public EmployeeSignatureDesignationService(IEmployeeSignatureDesignationRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        public IEnumerable<EmployeeSignatureDesignation> GetAll()
        {
            var entities = repository.GetAll().Where(c => c.IsActive == true).OrderBy(c => c.SignatureId);
            return entities;
        }

        public EmployeeSignatureDesignation GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public EmployeeSignatureDesignation Create(EmployeeSignatureDesignation objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(EmployeeSignatureDesignation objectToUpdate)
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

        public EmployeeSignatureDesignation Get(Expression<Func<EmployeeSignatureDesignation, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<EmployeeSignatureDesignation> GetMany(Expression<Func<EmployeeSignatureDesignation, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }

        #region Asyc
        public virtual async Task<IEnumerable<EmployeeSignatureDesignation>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }


        public virtual async Task<IEnumerable<EmployeeSignatureDesignation>> GetManyAsync(Expression<Func<EmployeeSignatureDesignation, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }



        public virtual async Task<EmployeeSignatureDesignation> GetAsync(Expression<Func<EmployeeSignatureDesignation, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion
    }
}

