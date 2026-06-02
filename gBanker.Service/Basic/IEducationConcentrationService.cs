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
    public interface IEducationConcentrationService : IServiceBase<EducationConcentration>
    {
        //IEnumerable<ValidationResult> IsValidCountry(string countryCode);
        //IEnumerable<Country> SearchCountry();
    }
    public class EducationConcentrationService : IEducationConcentrationService
    {
        private readonly IEducationConcentrationRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;


        public EducationConcentrationService(IEducationConcentrationRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<EducationConcentration> GetAll()
        {
            var entities = repository.GetAll().Where( c=> c.IsActive == true).OrderBy(c => c.ConcentrationId);
            return entities;
        }
        

        public EducationConcentration GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public EducationConcentration Create(EducationConcentration objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(EducationConcentration objectToUpdate)
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

        public bool Inactivate(long id, DateTime? inactiveDate)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            unitOfWork.Commit();
        }

        public EducationConcentration Get(Expression<Func<EducationConcentration, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<EducationConcentration> GetMany(Expression<Func<EducationConcentration, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }

        #region Asyc
        public virtual async Task<IEnumerable<EducationConcentration>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }


        public virtual async Task<IEnumerable<EducationConcentration>> GetManyAsync(Expression<Func<EducationConcentration, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }



        public virtual async Task<EducationConcentration> GetAsync(Expression<Func<EducationConcentration, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion

        public bool Inactivate(int id, DateTime? inactiveDate)
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

        //public IEnumerable<EducationDegree> SearchCountry()
        //{
        //    //return repository.GetMany(g => g.IsActive == true).OrderBy(g => g.InvestorID);
        //    return repository.GetMany(g => g.Status == true).OrderBy(o => o.CountryId);
        //}

        //IEnumerable<ValidationResult> ICountryService.IsValidCountry(string countryCode)
        //{
        //    var entity = repository.Get(p => p.CountryShortCode == countryCode);
        //    if (entity != null)
        //    {
        //        yield return new ValidationResult("CountryCode", "Duplicate Country Code.");

        //    }
        //}
        

    }
}

