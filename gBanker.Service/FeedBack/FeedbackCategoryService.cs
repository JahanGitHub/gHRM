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
    public interface IFeedbackCategoryService : IServiceBase<FeedbackCategory>
    {
        FeedbackCategory GetByFeedbackCategoryID(Int64 feedbackCategoryID);
    }
    public class FeedbackCategoryService : IFeedbackCategoryService
    {
        private readonly IFeedbackCategoryRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public FeedbackCategoryService(IFeedbackCategoryRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<FeedbackCategory> GetAll()
        {
            var entities = repository.GetAll().Where(w => w.IsActive == true).OrderBy(c => c.FeedbackCategoryID);
            return entities;
        }

        public FeedbackCategory GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public FeedbackCategory GetByFeedbackCategoryID(Int64 feedbackCategoryID)
        {
            var entity = repository.Get(e => e.FeedbackCategoryID == feedbackCategoryID && e.IsActive == true);
            return entity;
        }

        public FeedbackCategory Create(FeedbackCategory objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(FeedbackCategory objectToUpdate)
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

        public IEnumerable<FeedbackCategory> GetMany(Expression<Func<FeedbackCategory, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }

        public FeedbackCategory Get(Expression<Func<FeedbackCategory, bool>> where)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<FeedbackCategory>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<FeedbackCategory>> GetManyAsync(Expression<Func<FeedbackCategory, bool>> where)
        {
            throw new NotImplementedException();
        }

        public Task<FeedbackCategory> GetAsync(Expression<Func<FeedbackCategory, bool>> where)
        {
            throw new NotImplementedException();
        }
    }
}
