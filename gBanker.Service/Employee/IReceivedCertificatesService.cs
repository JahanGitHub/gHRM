
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
    public interface IReceivedCertificatesService : IServiceBase<ReceivedCertificates>
    {

    }
    public class ReceivedCertificatesService : IReceivedCertificatesService
    {
        private readonly IReceivedCertificatesRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public ReceivedCertificatesService(IReceivedCertificatesRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<ReceivedCertificates> GetAll()
        {
            var entities = repository.GetAll().OrderBy(c => c.Id);
            return entities;
        }

        public ReceivedCertificates GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public ReceivedCertificates Create(ReceivedCertificates objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(ReceivedCertificates objectToUpdate)
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

        public ReceivedCertificates Get(Expression<Func<ReceivedCertificates, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<ReceivedCertificates> GetMany(Expression<Func<ReceivedCertificates, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }

        #region Asyc
        public virtual async Task<IEnumerable<ReceivedCertificates>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }


        public virtual async Task<IEnumerable<ReceivedCertificates>> GetManyAsync(Expression<Func<ReceivedCertificates, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }



        public virtual async Task<ReceivedCertificates> GetAsync(Expression<Func<ReceivedCertificates, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion

    }
}

