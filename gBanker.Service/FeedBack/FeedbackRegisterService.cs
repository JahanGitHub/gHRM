//Created by Mansur 14-11-2016 for Entry HRM Feedback Register as per Ataur Bhai's Reuirment with Morshed Bhai

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
    public interface IFeedbackRegisterService : IServiceBase<FeedbackRegister>
    {
        FeedbackRegister GetByFeedbackRegisterID(Int64 feedbackRegisterID);
    }
    public class FeedbackRegisterService : IFeedbackRegisterService
    {
        private readonly IFeedbackRegisterRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public FeedbackRegisterService(IFeedbackRegisterRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<FeedbackRegister> GetAll()
        {
            var entities = repository.GetAll().Where(w => w.IsActive == true).OrderBy(c => c.FeedbackRegisterID);
            return entities;
        }

        public FeedbackRegister GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public FeedbackRegister GetByFeedbackRegisterID(Int64 feedbackRegisterID)
        {
            var entity = repository.Get(e => e.FeedbackRegisterID == feedbackRegisterID && e.IsActive == true);
            return entity;
        }

        public FeedbackRegister Create(FeedbackRegister objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(FeedbackRegister objectToUpdate)
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
            var obj = repository.GetById(id);
            if (obj != null)
            {
                obj.InActiveDate = inactiveDate.HasValue ? inactiveDate : DateTime.Now;
                obj.IsActive = false;
                repository.Update(obj);
                Save();
                return true;
            }
            return false;
        }

        public bool IsContinued(long id)
        {
            var obj = repository.GetById(id);
            if (obj != null)
            {
                var isActive = obj.IsActive;
                if (isActive == false)
                {
                    return false;
                }
            }

            return true;
        }

        public IEnumerable<FeedbackRegister> GetMany(Expression<Func<FeedbackRegister, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }

        public FeedbackRegister Get(Expression<Func<FeedbackRegister, bool>> where)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<FeedbackRegister>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<FeedbackRegister>> GetManyAsync(Expression<Func<FeedbackRegister, bool>> where)
        {
            throw new NotImplementedException();
        }

        public Task<FeedbackRegister> GetAsync(Expression<Func<FeedbackRegister, bool>> where)
        {
            throw new NotImplementedException();
        }
    }
}
