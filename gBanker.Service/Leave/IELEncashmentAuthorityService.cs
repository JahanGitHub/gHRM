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
    public interface IELEncashmentAuthorityService : IServiceBase<ELEncashmentAuthority>
    {
        bool IsEmployeeAuthorizedForEncashment(long EmployeeId);
    }
    public class ELEncashmentAuthorityService : IELEncashmentAuthorityService
    {
        private readonly IELEncashmentAuthorityRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public ELEncashmentAuthorityService(IELEncashmentAuthorityRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        public bool IsEmployeeAuthorizedForEncashment(long EmployeeId)
        {
            return repository.IsEmployeeAuthorizedForEncashment(EmployeeId);
        }

        public IEnumerable<ELEncashmentAuthority> GetAll()
        {
            var entities = repository.GetAll().OrderBy(c => c.Id);
            return entities;
        }

        public ELEncashmentAuthority GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public ELEncashmentAuthority Create(ELEncashmentAuthority objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(ELEncashmentAuthority objectToUpdate)
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

        public ELEncashmentAuthority Get(Expression<Func<ELEncashmentAuthority, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<ELEncashmentAuthority> GetMany(Expression<Func<ELEncashmentAuthority, bool>> where)
        {
            var entities = repository.GetMany(where);
            return entities;
        }

        #region Asyc
        public virtual async Task<IEnumerable<ELEncashmentAuthority>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }
        public virtual async Task<IEnumerable<ELEncashmentAuthority>> GetManyAsync(Expression<Func<ELEncashmentAuthority, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }
        public virtual async Task<ELEncashmentAuthority> GetAsync(Expression<Func<ELEncashmentAuthority, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion

    }
}

