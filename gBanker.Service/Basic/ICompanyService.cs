using gHRM.Core.Common;
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.DBDetailModels;
using gHRM.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Service
{

    public interface ICompanyService : IServiceBase<Company>
    {
        Company GetCompanyInfo();
        IEnumerable<ValidationResult> IsValidCompany(int companyId);
        IEnumerable<DBCompanyDetailModel> GetCompanyDetail(string filterColumnName, string filterValue, int startRowIndex, string jtSorting, int pageSize, out long TotCount);
        string GetCompanyNameOtherAndWebsite(out string WebsiteUrl);

        void UpdateCompanyInfo(Company objectToUpdate);
    }
    public class CompanyService : ICompanyService
    {
        private readonly ICompanyRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public CompanyService(ICompanyRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        public Company GetCompanyInfo()
        {
            var single = new Company();
            try
            {
                single = repository.GetCompanyInfo();

                return single;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public IEnumerable<Company> GetAll()
        {
            var entities = repository.GetAll().Where(c => c.IsActive == true).OrderBy(c => c.CompanyId);
            return entities;
        }

        public Company GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public Company Create(Company objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(Company objectToUpdate)
        {
            repository.Update(objectToUpdate);
            Save();
        }

        public void UpdateCompanyInfo(Company objectToUpdate)
        {
            using (var db = new gHRMDBContext())
            {
                var company = db.Companies.FirstOrDefault(f => f.CompanyId == objectToUpdate.CompanyId);

                if (company != null)
                {
                    company.CountryId = objectToUpdate.CountryId;
                    company.CompanyType = objectToUpdate.CompanyType;
                    company.CompanyName = objectToUpdate.CompanyName;
                    company.CompanyAddress = objectToUpdate.CompanyAddress;
                    company.CompanyMobile = objectToUpdate.CompanyMobile;
                    company.CompanyEmail = objectToUpdate.CompanyEmail;
                    company.CompanyPhone = objectToUpdate.CompanyPhone;
                    company.CompanySlogan = objectToUpdate.CompanySlogan;
                    company.WebsiteUrl = objectToUpdate.WebsiteUrl;

                    if (!string.IsNullOrWhiteSpace(objectToUpdate.ImagePath))
                        company.ImagePath = objectToUpdate.ImagePath;

                    if (!string.IsNullOrWhiteSpace(objectToUpdate.CompanySignaturePath))
                        company.CompanySignaturePath = objectToUpdate.CompanySignaturePath;

                    company.UpdateDate = objectToUpdate.UpdateDate;
                    company.UpdateDate = DateTime.Now;

                    db.SaveChanges();
                }

            }
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

        public Company Get(Expression<Func<Company, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<Company> GetMany(Expression<Func<Company, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }

        #region Asyc
        public virtual async Task<IEnumerable<Company>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }


        public virtual async Task<IEnumerable<Company>> GetManyAsync(Expression<Func<Company, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }



        public virtual async Task<Company> GetAsync(Expression<Func<Company, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion
        public bool Inactivate(long id, DateTime? inactiveDate)
        {
            throw new NotImplementedException();
        }
        public bool IsContinued(long id)
        {
            throw new NotImplementedException();
        }
        IEnumerable<ValidationResult> ICompanyService.IsValidCompany(int companyId)
        {
            var entity = repository.Get(p => p.CompanyId == companyId);
            if (entity != null)
            {
                yield return new ValidationResult("CompanyId", "Duplicate Company Id.");

            }
        }
        public IEnumerable<DBCompanyDetailModel> GetCompanyDetail(string filterColumnName, string filterValue, int startRowIndex, string jtSorting, int pageSize, out long TotCount)
        {
            return repository.GetCompanyDetail(filterColumnName, filterValue, startRowIndex, jtSorting, pageSize, out TotCount);
        }

        public string GetCompanyNameOtherAndWebsite(out string WebsiteUrl)
        {
            return repository.GetCompanyNameOtherAndWebsite(out WebsiteUrl);
        }
    }
}
