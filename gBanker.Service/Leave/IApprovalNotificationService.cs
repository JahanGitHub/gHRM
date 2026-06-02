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

    public interface IApprovalNotificationService : IServiceBase<ApprovalNotification>
    {

    }
    public class ApprovalNotificationService : IApprovalNotificationService
    {
        private readonly IApprovalNotificationRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public ApprovalNotificationService(IApprovalNotificationRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<ApprovalNotification> GetAll()
        {
            var entities = repository.GetAll().OrderBy(c => c.NotificationId);
            return entities;
        }

        public ApprovalNotification GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public ApprovalNotification Create(ApprovalNotification objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(ApprovalNotification objectToUpdate)
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
        public ApprovalNotification Get(Expression<Func<ApprovalNotification, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<ApprovalNotification> GetMany(Expression<Func<ApprovalNotification, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }

        #region Asyc
        public virtual async Task<IEnumerable<ApprovalNotification>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }


        public virtual async Task<IEnumerable<ApprovalNotification>> GetManyAsync(Expression<Func<ApprovalNotification, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }



        public virtual async Task<ApprovalNotification> GetAsync(Expression<Func<ApprovalNotification, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion

        public bool Inactivate(long id, DateTime? inactiveDate)
        {
            throw new NotImplementedException(); ;
        }


        public bool IsContinued(long id)
        {
            throw new NotImplementedException();
        }


    }
}
