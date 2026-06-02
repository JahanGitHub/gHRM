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

    public interface IAttCardIssueService : IServiceBase<AttCardIssue>
    {

    }

    public class AttCardIssueService : IAttCardIssueService
    {
        private readonly IAttCardIssueRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public AttCardIssueService(IAttCardIssueRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<AttCardIssue> GetAll()
        {
            var entities = repository.GetAll().Where(c => c.IsActive == true).OrderBy(c => c.AttCardIssueId);
            return entities;
        }
        public AttCardIssue GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }
        public void Save()
        {
            unitOfWork.Commit();
        }


        public AttCardIssue Get(Expression<Func<AttCardIssue, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<AttCardIssue> GetMany(Expression<Func<AttCardIssue, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }

        #region Asyc
        public virtual async Task<IEnumerable<AttCardIssue>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }


        public virtual async Task<IEnumerable<AttCardIssue>> GetManyAsync(Expression<Func<AttCardIssue, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }



        public virtual async Task<AttCardIssue> GetAsync(Expression<Func<AttCardIssue, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion

        public AttCardIssue Create(AttCardIssue objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(AttCardIssue objectToUpdate)
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
        public bool Inactivate(long id, DateTime? inactiveDate)
        {
            //throw new NotImplementedException();
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
            // throw new NotImplementedException();
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

    }// End of Class
}// End of Namespace
