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
    public interface IDiscCaseStatusService : IServiceBase<DiscCaseStatu>
    {
        DiscCaseStatu GetByDiscCaseId(Int64 CaseStatusId);

    }
    public class DiscCaseStatusService : IDiscCaseStatusService
    {
        private readonly IDiscCaseStatusRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public DiscCaseStatusService(IDiscCaseStatusRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<DiscCaseStatu> GetAll()
        {
            var entities = repository.GetAll().Where(c => c.IsActive == true).OrderBy(c => c.CaseMasterId);
            return entities;
        }

        public DiscCaseStatu GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }
        public DiscCaseStatu GetByDiscCaseId(Int64 CaseStatusId)
        {
            var entity = repository.Get(e => e.CaseStatusId == CaseStatusId && e.IsActive == true);
            return entity;
        }
        public DiscCaseStatu Create(DiscCaseStatu objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(DiscCaseStatu objectToUpdate)
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
        public IEnumerable<DiscCaseStatu> GetMany(Expression<Func<DiscCaseStatu, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }

        public DiscCaseStatu Get(Expression<Func<DiscCaseStatu, bool>> where)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<DiscCaseStatu>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<DiscCaseStatu>> GetManyAsync(Expression<Func<DiscCaseStatu, bool>> where)
        {
            throw new NotImplementedException();
        }

        public Task<DiscCaseStatu> GetAsync(Expression<Func<DiscCaseStatu, bool>> where)
        {
            throw new NotImplementedException();
        }

        //IEnumerable<ValidationResult> IDiscCaseMasterService.IsValidCaseMasterDetail(string CaseNo)
        //{
        //    var entity = repository.Get(p => p.CaseNo == CaseNo);
        //    if (entity != null)
        //    {
        //        yield return new ValidationResult("OrderId", "Duplicate OrderId Id.");

        //    }
        //}

    }
}
