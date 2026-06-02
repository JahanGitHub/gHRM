using gHRM.Core.Common;
using gHRM.Data.CodeFirstMigration.Discipline;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.Repository.Discipline;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Service.Discipline
{
    public interface IDiscCaseMasterService : IServiceBase<DiscCaseMaster>
    {
        IEnumerable<ValidationResult> IsValidCaseMasterDetail(string CaseNo);

    }
    public class DiscCaseMasterService : IDiscCaseMasterService
    {
        private readonly IDiscCaseMasterRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public DiscCaseMasterService(IDiscCaseMasterRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<DiscCaseMaster> GetAll()
        {
            var entities = repository.GetAll().Where(c => c.IsActive == true).OrderBy(c => c.CaseNo);
            return entities;
        }

        public DiscCaseMaster GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }
        public DiscCaseMaster Create(DiscCaseMaster objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(DiscCaseMaster objectToUpdate)
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
            throw new NotImplementedException();
        }
        public bool IsContinued(long id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DiscCaseMaster> GetMany(Expression<Func<DiscCaseMaster, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }

        IEnumerable<ValidationResult> IDiscCaseMasterService.IsValidCaseMasterDetail(string CaseNo)
        {
            var entity = repository.Get(p => p.CaseNo == CaseNo);
            if (entity != null)
            {
                yield return new ValidationResult("OrderId", "Duplicate OrderId Id.");

            }
        }

        public DiscCaseMaster Get(Expression<Func<DiscCaseMaster, bool>> where)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<DiscCaseMaster>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<DiscCaseMaster>> GetManyAsync(Expression<Func<DiscCaseMaster, bool>> where)
        {
            throw new NotImplementedException();
        }

        public Task<DiscCaseMaster> GetAsync(Expression<Func<DiscCaseMaster, bool>> where)
        {
            throw new NotImplementedException();
        }
    }
}
