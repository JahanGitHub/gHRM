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
    public interface IEducationDegreeService : IServiceBase<EducationDegree>
    {
        //IEnumerable<ValidationResult> IsValidCountry(string countryCode);
        //IEnumerable<Country> SearchCountry();
        List<Dictionary<string, object>> GetDropdownList();
    }
    public class EducationDegreeService : IEducationDegreeService
    {
        private readonly IEducationDegreeRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;


        public EducationDegreeService(IEducationDegreeRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        public List<Dictionary<string, object>> GetDropdownList()
        {
            return repository.GetDropdownList();
        }

        public IEnumerable<EducationDegree> GetAll()
        {
            var entities = repository.GetAll().Where( c=> c.IsActive == true).OrderBy(c => c.DegreeId);
            return entities;
        }
        

        public EducationDegree GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public EducationDegree Create(EducationDegree objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(EducationDegree objectToUpdate)
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


        public EducationDegree Get(Expression<Func<EducationDegree, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<EducationDegree> GetMany(Expression<Func<EducationDegree, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }

        #region Asyc
        public virtual async Task<IEnumerable<EducationDegree>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }


        public virtual async Task<IEnumerable<EducationDegree>> GetManyAsync(Expression<Func<EducationDegree, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }



        public virtual async Task<EducationDegree> GetAsync(Expression<Func<EducationDegree, bool>> where)
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

