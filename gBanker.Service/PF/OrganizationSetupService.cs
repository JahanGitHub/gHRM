using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.CodeFirstMigration.PF;
using gHRM.Data.Repository.PF;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Data.Entity;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Service.PF
{
    public interface IOrganizationSetupService : IServiceBase<OrganizationSetup>
    {
        IEnumerable<OrganizationSetup> GetOrganization();
        IEnumerable<OrganizationSetup> GetOrgSetupByName(string orgName);
        bool ValidateEmployeeRoasterByDateRange(int id, DateTime effectiveStartDate, DateTime effectiveEndDate);
        OrganizationSetup GetOrganizationSetupById(int id);
    }
    public class OrganizationSetupService : IOrganizationSetupService
    {
        private readonly IOrganizationSetupRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public OrganizationSetupService(IOrganizationSetupRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<OrganizationSetup> GetAll()
        {
            var entities = repository.GetAll();
            return entities;
        }
        public OrganizationSetup GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public OrganizationSetup GetOrganizationSetupById(int id)
        {
            var single = new OrganizationSetup();
            using (var db = new gHRMDBContext())
            {
                single = db.OrganizationSetup
                        .Include(i => i.PFType)
                        .FirstOrDefault(f => !f.IsDeleted && f.IsActive && f.Id == id);
            }

            return single;
        }

        public bool ValidateEmployeeRoasterByDateRange(int id, DateTime effectiveStartDate, DateTime effectiveEndDate)
        {
            bool isValid = true;

            using (var db = new gHRMDBContext())
            {
                var sqlCommand = $@"[prl].[OrganizationSetup_IsAvailableByDateRange]                                     
                                      {id}
                                    ,'{effectiveStartDate.ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture)}'
                                    ,'{effectiveEndDate.ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture)}'
                                ";
                int count = db.Database.SqlQuery<int>(sqlCommand).FirstOrDefault();
                if (count > 0)
                    isValid = false;
            }

            return isValid;
        }
        public void Save()
        {
            unitOfWork.Commit();
        }
        public OrganizationSetup Create(OrganizationSetup objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(OrganizationSetup objectToUpdate)
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
            //throw new NotImplementedException();
            var obj = repository.GetById(id);
            if (obj != null)
            {
                //obj.InActiveDate = DateTime.Now;
                obj.IsActive = false;
                repository.Update(obj);
                Save();
                return true;
            }
            return false;
        }
        public bool IsContinued(long id)
        {
            // throw new NotImplementedException();
            var obj = repository.GetById(id);
            if (obj != null)
            {
                var isActive = obj.IsActive;
                if (isActive == false)
                {
                    return false;
                }
            }
            return true;
        }
        public OrganizationSetup Get(Expression<Func<OrganizationSetup, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<OrganizationSetup> GetMany(Expression<Func<OrganizationSetup, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsDeleted == false);
            return entities;
        }
        #region Asyc
        public virtual async Task<IEnumerable<OrganizationSetup>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }
        public virtual async Task<IEnumerable<OrganizationSetup>> GetManyAsync(Expression<Func<OrganizationSetup, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }
        public virtual async Task<OrganizationSetup> GetAsync(Expression<Func<OrganizationSetup, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion
        public IEnumerable<OrganizationSetup> GetOrganization()
        {
            return repository.GetOrganization();
        }

        public IEnumerable<OrganizationSetup> GetOrgSetupByName(string orgName)
        {
            return repository.GetOrgSetupByName(orgName);
        }

        //public IEnumerable<PFType> GetPFTypeByName(string pfTypeName)
        //{
        //    return repository.GetPFTypeByName(pfTypeName);
        //}
    }
}
