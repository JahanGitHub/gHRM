using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using gHRM.Core.Common;
using gHRM.Data.DBDetailModels.Discipline;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.CodeFirstMigration.Discipline;
using System;
using gHRM.Data.Repository.Discipline;

namespace gHRM.Service.Discipline
{
    public interface IDisCrimeService : IServiceBase<DiscCrime>
    {
        IEnumerable<ValidationResult> IsValidCrime(string CrimeCode);
        IEnumerable<DBDiscCrimeDetailsModel> GetDiscCrimeDetail(string filterColumnName, string filterValue, int startRowIndex, string jtSorting, int pageSize, out long TotCount);

    }
    public class DiscCrimeService : IDisCrimeService
    {
        private readonly IDiscCrimeRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public DiscCrimeService(IDiscCrimeRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<DiscCrime> GetAll()
        {
            var entities = repository.GetAll().Where(c => c.IsActive == true).OrderBy(c => c.CrimeCode);
            return entities;
        }

        public DiscCrime GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public DiscCrime Create(DiscCrime objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(DiscCrime objectToUpdate)
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
        IEnumerable<ValidationResult> IDisCrimeService.IsValidCrime(string CrimeCode)
        {
            var entity = repository.Get(p => p.CrimeCode == CrimeCode);
            if (entity != null)
            {
                yield return new ValidationResult("CrimeId", "Duplicate Crime Id.");

            }
        }

        public IEnumerable<DiscCrime> GetMany(Expression<Func<DiscCrime, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }

        public IEnumerable<DBDiscCrimeDetailsModel> GetDiscCrimeDetail(string filterColumnName, string filterValue, int startRowIndex, string jtSorting, int pageSize, out long TotCount)
        {
            return repository.GetCrimeDetail(filterColumnName, filterValue, startRowIndex, jtSorting, pageSize, out TotCount);
        }

        public DiscCrime Get(Expression<Func<DiscCrime, bool>> where)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<DiscCrime>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<DiscCrime>> GetManyAsync(Expression<Func<DiscCrime, bool>> where)
        {
            throw new NotImplementedException();
        }

        public Task<DiscCrime> GetAsync(Expression<Func<DiscCrime, bool>> where)
        {
            throw new NotImplementedException();
        }
    }
}
