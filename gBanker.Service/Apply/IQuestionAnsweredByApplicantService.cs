using BasicDataAccess;
using gHRM.Core.Common;
using gHRM.Core.Filters.Offices;
using gHRM.Core.Utilities;
using gHRM.Data.CodeFirstMigration.Apply;
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.DBDetailModels;
using gHRM.Data.Repository;
using gHRM.Data.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Service
{
    public interface IQuestionAnsweredByApplicantService : IServiceBase<QuestionAnsweredByApplicant>
    {
    }
    public class QuestionAnsweredByApplicantService : IQuestionAnsweredByApplicantService
    {
        private readonly IQuestionAnsweredByApplicantRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public QuestionAnsweredByApplicantService(IQuestionAnsweredByApplicantRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        #region Implementation for IServiceBase
        public IEnumerable<QuestionAnsweredByApplicant> GetAll()
        {
            var entities = repository.GetMany(g => g.IsActive == true).OrderBy(c => c.AnsId);
            return entities;
        }

        public QuestionAnsweredByApplicant GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public QuestionAnsweredByApplicant Create(QuestionAnsweredByApplicant objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(QuestionAnsweredByApplicant objectToUpdate)
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
                if (isActive == true)
                {
                    return false;
                }
            }
            return true;
        }

        public QuestionAnsweredByApplicant Get(Expression<Func<QuestionAnsweredByApplicant, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }

        public IEnumerable<QuestionAnsweredByApplicant> GetMany(Expression<Func<QuestionAnsweredByApplicant, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }

        public virtual async Task<IEnumerable<QuestionAnsweredByApplicant>> GetAllAsync()
        {
            return await repository.GetAllAsync();
        }

        public virtual async Task<IEnumerable<QuestionAnsweredByApplicant>> GetManyAsync(Expression<Func<QuestionAnsweredByApplicant, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }

        public virtual async Task<QuestionAnsweredByApplicant> GetAsync(Expression<Func<QuestionAnsweredByApplicant, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion
    }
}

