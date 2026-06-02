using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using gHRM.Data.CodeFirstMigration.Basic;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.Repository.Basic;

namespace gHRM.Service.Basic
{
    public interface INoticeService : IServiceBase<Notice>
    {
        //IEnumerable<ValidationResult> IsValidCountry(string countryCode);
        // IEnumerable<Country> SearchCountry();
    }
    public class NoticeService : INoticeService
    {
        private readonly INoticeRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public NoticeService(INoticeRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<Notice> GetAll()
        {
            var entities = repository.GetAll().Where(c => c.IsActive == true);
            return entities;
        }

        public Notice GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public Notice Create(Notice objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(Notice objectToUpdate)
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

        public IEnumerable<Notice> GetMany(Expression<Func<Notice, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }

        public Notice Get(Expression<Func<Notice, bool>> where)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Notice>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Notice>> GetManyAsync(Expression<Func<Notice, bool>> where)
        {
            throw new NotImplementedException();
        }

        public Task<Notice> GetAsync(Expression<Func<Notice, bool>> where)
        {
            throw new NotImplementedException();
        }
    }
}

