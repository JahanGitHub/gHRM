using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.CodeFirstMigration.PF;
using gHRM.Data.Repository.PF;
using gHRM.Service.StoreProcedure;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Service.PF
{
    public interface ICollectionService : IServiceBase<Collection>
    {
        IEnumerable<Collection> GetCollectionByEmpId(string employeeId);
        Collection GetCollectionByCollId(long collectionId);
        IEnumerable<Collection> GetLoanCollectionByLoanId(long loanId);
        //AccountChart GetAccountChartByAccountCode(string accountCode);
        IEnumerable<Collection> GetAllCollection();
        //DataSet GetCollections(long? employeeId, string employeeName, int? transCategoryId, string collectionType);
        DataSet GetCollections(string employeeCode, string employeeName, int? transCategoryId);
    }
    public class CollectionService : ICollectionService
    {
        private readonly ICollectionRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;
        private readonly IEmployeeSPService employeeSPService;

        public CollectionService(ICollectionRepository repository, IUnitOfWorkCodeFirst unitOfWork, IEmployeeSPService employeeSPService)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
            this.employeeSPService = employeeSPService;
        }
        public IEnumerable<Collection> GetAll()
        {
            var entities = repository.GetAll();
            return entities;
        }
        public Collection GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }
        public void Save()
        {
            unitOfWork.Commit();
        }
        public Collection Create(Collection objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(Collection objectToUpdate)
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
                //obj.IsActive = false;
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
                //var isActive = obj.IsActive;
                //if (isActive == false)
                //{
                //    return false;
                //}
            }
            return true;
        }
        public Collection Get(Expression<Func<Collection, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<Collection> GetMany(Expression<Func<Collection, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsDeleted == false);
            return entities;
        }
        #region Asyc
        public virtual async Task<IEnumerable<Collection>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }
        public virtual async Task<IEnumerable<Collection>> GetManyAsync(Expression<Func<Collection, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }
        public virtual async Task<Collection> GetAsync(Expression<Func<Collection, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion
        public IEnumerable<Collection> GetCollectionByEmpId(string employeeId)
        {
            return repository.GetCollectionByEmpId(employeeId);
        }

        public Collection GetCollectionByCollId(long collectionId)
        {
            return repository.GetCollectionByCollId(collectionId);
        }

        public IEnumerable<Collection> GetLoanCollectionByLoanId(long loanId)
        {
            return repository.GetLoanCollectionByLoanId(loanId);
        }

        public IEnumerable<Collection> GetAllCollection()
        {
            return repository.GetAllCollection();
        }
        public DataSet GetCollections(string employeeCode, string employeeName, int? transCategoryId)
        {
            var param = new
            {
                EmployeeCode = employeeCode,
                EmployeeName = employeeName,
                TransCategoryId = transCategoryId
            };
            var dataset = employeeSPService.GetDataWithParameter(param, "gcpf.SP_GetCollections");
            return dataset;
        }

    }
}
