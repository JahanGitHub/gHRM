using gHRM.Data.CodeFirstMigration.Discipline;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.Repository.Discipline;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace gHRM.Service.Discipline
{
    public interface IDiscEmbezzleService : IServiceBase<DiscEmbezzleInfo>
    {
        //IEnumerable<ValidationResult> IsValidCountry(string countryCode);
        // IEnumerable<Country> SearchCountry();
    }
    public class DiscEmbezzleService : IDiscEmbezzleService
    {
        private readonly IDiscEmbezzleInfoRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;


        public DiscEmbezzleService(IDiscEmbezzleInfoRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<DiscEmbezzleInfo> GetAll()
        {
            var entities = repository.GetAll().Where(c => c.IsActive == true);
            return entities;
        }


        public DiscEmbezzleInfo GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public DiscEmbezzleInfo Create(DiscEmbezzleInfo objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(DiscEmbezzleInfo objectToUpdate)
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
            throw new NotImplementedException();
        }

        public IEnumerable<DiscEmbezzleInfo> GetMany(Expression<Func<DiscEmbezzleInfo, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }

        public DiscEmbezzleInfo Get(Expression<Func<DiscEmbezzleInfo, bool>> where)
        {
            throw new NotImplementedException();
        }


        public Task<IEnumerable<DiscEmbezzleInfo>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<DiscEmbezzleInfo>> GetManyAsync(Expression<Func<DiscEmbezzleInfo, bool>> where)
        {
            throw new NotImplementedException();
        }

        public Task<DiscEmbezzleInfo> GetAsync(Expression<Func<DiscEmbezzleInfo, bool>> where)
        {
            throw new NotImplementedException();
        }
    }
}
