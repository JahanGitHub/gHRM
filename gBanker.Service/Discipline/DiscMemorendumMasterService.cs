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
    public interface IDiscMemorendumMasterService : IServiceBase<DiscMemorendumMaster>
    {


    }
    public class DiscMemorendumMasterService : IDiscMemorendumMasterService
    {
        private readonly IDiscMemorendumMasterRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public DiscMemorendumMasterService(IDiscMemorendumMasterRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<DiscMemorendumMaster> GetAll()
        {
            var entities = repository.GetAll().Where(c => c.IsActive == true).OrderBy(c => c.EmployeeId);
            return entities;
        }

        public DiscMemorendumMaster GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }
        public DiscMemorendumMaster Create(DiscMemorendumMaster objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(DiscMemorendumMaster objectToUpdate)
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

        public IEnumerable<DiscMemorendumMaster> GetMany(Expression<Func<DiscMemorendumMaster, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }

        public DiscMemorendumMaster Get(Expression<Func<DiscMemorendumMaster, bool>> where)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<DiscMemorendumMaster>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<DiscMemorendumMaster>> GetManyAsync(Expression<Func<DiscMemorendumMaster, bool>> where)
        {
            throw new NotImplementedException();
        }

        public Task<DiscMemorendumMaster> GetAsync(Expression<Func<DiscMemorendumMaster, bool>> where)
        {
            throw new NotImplementedException();
        }
    }
}
