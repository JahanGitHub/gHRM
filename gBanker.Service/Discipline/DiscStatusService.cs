using gHRM.Core.Common;
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.Discipline;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.DBDetailModels;
using gHRM.Data.Repository;
using gHRM.Data.Repository.Discipline;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Service.Discipline
{
    public interface IDiscStatusService : IServiceBase<DiscStatu>
    {
        //IEnumerable<ValidationResult> IsValidCaseMasterDetail(string CaseNo);

    }
    public class DiscStatusService : IDiscStatusService
    {
        private readonly IDiscStatusRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public DiscStatusService(IDiscStatusRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<DiscStatu> GetAll()
        {
            var entities = repository.GetAll().Where(c => c.IsActive == true).OrderBy(c => c.Orders);
            return entities;
        }

        public DiscStatu GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }
        public DiscStatu Create(DiscStatu objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(DiscStatu objectToUpdate)
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

        public IEnumerable<DiscStatu> GetMany(Expression<Func<DiscStatu, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }

        public DiscStatu Get(Expression<Func<DiscStatu, bool>> where)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<DiscStatu>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<DiscStatu>> GetManyAsync(Expression<Func<DiscStatu, bool>> where)
        {
            throw new NotImplementedException();
        }

        public Task<DiscStatu> GetAsync(Expression<Func<DiscStatu, bool>> where)
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
