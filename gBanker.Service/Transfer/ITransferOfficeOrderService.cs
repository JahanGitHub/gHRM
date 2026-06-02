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
    public interface ITransferOfficeOrderrService : IServiceBase<TransferOfficeOrder>
    {


    }
    public class TransferOfficeOrderService : ITransferOfficeOrderrService
    {
        private readonly ITransferOfficeOrderRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public TransferOfficeOrderService(ITransferOfficeOrderRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        public IEnumerable<TransferOfficeOrder> GetAll()
        {
            var entities = repository.GetAll().Where(c => c.IsActive == true).OrderBy(c => c.CCForOfficeOrderId);
            return entities;
        }

        public TransferOfficeOrder GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public TransferOfficeOrder Create(TransferOfficeOrder objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(TransferOfficeOrder objectToUpdate)
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

        public TransferOfficeOrder Get(Expression<Func<TransferOfficeOrder, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<TransferOfficeOrder> GetMany(Expression<Func<TransferOfficeOrder, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }

        #region Asyc
        public virtual async Task<IEnumerable<TransferOfficeOrder>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }


        public virtual async Task<IEnumerable<TransferOfficeOrder>> GetManyAsync(Expression<Func<TransferOfficeOrder, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }


        public virtual async Task<TransferOfficeOrder> GetAsync(Expression<Func<TransferOfficeOrder, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion
    }
}
