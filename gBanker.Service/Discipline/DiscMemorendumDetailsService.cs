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
    public interface IDiscMemorendumDetailsService : IServiceBase<DiscMemorendumDetail>
    {


    }
    public class DiscMemorendumDetailsService : IDiscMemorendumDetailsService
    {
        private readonly IDiscMemorendumDetailsRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public DiscMemorendumDetailsService(IDiscMemorendumDetailsRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<DiscMemorendumDetail> GetAll()
        {
            var entities = repository.GetAll().Where(c => c.IsActive == true).OrderBy(c => c.MemorendumDetailsId);
            return entities;
        }

        public DiscMemorendumDetail GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }
        public DiscMemorendumDetail Create(DiscMemorendumDetail objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(DiscMemorendumDetail objectToUpdate)
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

        public IEnumerable<DiscMemorendumDetail> GetMany(Expression<Func<DiscMemorendumDetail, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }

        public DiscMemorendumDetail Get(Expression<Func<DiscMemorendumDetail, bool>> where)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<DiscMemorendumDetail>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<DiscMemorendumDetail>> GetManyAsync(Expression<Func<DiscMemorendumDetail, bool>> where)
        {
            throw new NotImplementedException();
        }

        public Task<DiscMemorendumDetail> GetAsync(Expression<Func<DiscMemorendumDetail, bool>> where)
        {
            throw new NotImplementedException();
        }
    }
}
