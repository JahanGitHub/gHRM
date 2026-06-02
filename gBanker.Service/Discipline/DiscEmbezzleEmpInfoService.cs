using gHRM.Data.CodeFirstMigration.Discipline;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.Repository.Discipline;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace gHRM.Service.Discipline
{
    public interface IDiscEmbezzleEmpInfoService : IServiceBase<DiscEmbezzleEmpInfo>
    {
        //IEnumerable<ValidationResult> IsValidCountry(string countryCode);
        // IEnumerable<Country> SearchCountry();
    }
    public class DiscEmbezzleEmpInfoService : IDiscEmbezzleEmpInfoService
    {
        private readonly IDiscEmbezzleEmpInfoRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;


        public DiscEmbezzleEmpInfoService(IDiscEmbezzleEmpInfoRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<DiscEmbezzleEmpInfo> GetAll()
        {
            var entities = repository.GetAll().Where(c => c.IsActive == true);
            return entities;
        }


        public DiscEmbezzleEmpInfo GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public DiscEmbezzleEmpInfo Create(DiscEmbezzleEmpInfo objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(DiscEmbezzleEmpInfo objectToUpdate)
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

        public IEnumerable<DiscEmbezzleEmpInfo> GetMany(Expression<Func<DiscEmbezzleEmpInfo, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }

        public DiscEmbezzleEmpInfo Get(Expression<Func<DiscEmbezzleEmpInfo, bool>> where)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<DiscEmbezzleEmpInfo>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<DiscEmbezzleEmpInfo>> GetManyAsync(Expression<Func<DiscEmbezzleEmpInfo, bool>> where)
        {
            throw new NotImplementedException();
        }

        public Task<DiscEmbezzleEmpInfo> GetAsync(Expression<Func<DiscEmbezzleEmpInfo, bool>> where)
        {
            throw new NotImplementedException();
        }
    }
}
