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
    public interface IEASSCompanyService : IServiceBase<EASSCompany>
    {

    }
    public class EASSCompanyService : IEASSCompanyService
    {
        private readonly IEASSCompanyRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public EASSCompanyService(IEASSCompanyRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<EASSCompany> GetAll()
        {
            var entities = repository.GetAll();
            return entities;
        }

        public EASSCompany GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public EASSCompany Create(EASSCompany objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(EASSCompany objectToUpdate)
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


        public EASSCompany Get(Expression<Func<EASSCompany, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<EASSCompany> GetMany(Expression<Func<EASSCompany, bool>> where)
        {
            var entities = repository.GetMany(where);
            return entities;
        }

        #region Asyc
        public virtual async Task<IEnumerable<EASSCompany>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }


        public virtual async Task<IEnumerable<EASSCompany>> GetManyAsync(Expression<Func<EASSCompany, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }



        public virtual async Task<EASSCompany> GetAsync(Expression<Func<EASSCompany, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion
    }
}
