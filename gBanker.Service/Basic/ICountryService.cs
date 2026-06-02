using gHRM.Core.Common;
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Service
{
    public interface ICountryService : IServiceBase<Country>
    {
        IEnumerable<ValidationResult> IsValidCountry(string countryCode);
        IEnumerable<Country> SearchCountry();
        Country GetByName(string name);
    }
    public class CountryService : ICountryService
    {
        private readonly ICountryRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;


        public CountryService(ICountryRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<Country> GetAll()
        {
            var entities = repository.GetAll().Where( c=> c.Status == true).OrderBy(c => c.CountryId);
            return entities;
        }
        

        public Country GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public Country Create(Country objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(Country objectToUpdate)
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


        public Country Get(Expression<Func<Country, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<Country> GetMany(Expression<Func<Country, bool>> where)
        {
            var entities = repository.GetMany(where);
            return entities;
        }

        #region Asyc
        public virtual async Task<IEnumerable<Country>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }


        public virtual async Task<IEnumerable<Country>> GetManyAsync(Expression<Func<Country, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }



        public virtual async Task<Country> GetAsync(Expression<Func<Country, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion
        public bool Inactivate(long id, DateTime? inactiveDate)
        {
            var obj = repository.GetById(id);
            if (obj != null)
            {
                obj.Status = false;
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

        public IEnumerable<Country> SearchCountry()
        {
            //return repository.GetMany(g => g.IsActive == true).OrderBy(g => g.InvestorID);
            return repository.GetMany(g => g.Status == true).OrderBy(o => o.CountryId);
        }

        IEnumerable<ValidationResult> ICountryService.IsValidCountry(string countryCode)
        {
            var entity = repository.Get(p => p.CountryShortCode == countryCode);
            if (entity != null)
            {
                yield return new ValidationResult("CountryCode", "Duplicate Country Code.");

            }
        }

        public Country GetByName(string name)
        {
            var country = new Country();
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                    return null;

                using (var db = new gHRMDBContext())
                {
                    country = db.Countries
                        .FirstOrDefault(f => f.CountryName.Trim().ToLower() == name.Trim().ToLower());
                }
            }
            catch
            {
                return null;
            }

            return country;
        }
    }
}
