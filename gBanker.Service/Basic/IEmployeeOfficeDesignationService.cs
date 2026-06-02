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
    public interface IEmployeeOfficeDesignationService : IServiceBase<EmployeeOfficeDesignation>
    {
        EmployeeOfficeDesignation GetByEmployeeId(long EmployeeId);
        IEnumerable<DBEmployeeOfficeDesignationDetails> GetDBEmployeeOfficeDesignationDetails(int startRowIndex, string jtSorting, int pageSize, out long TotCount);
    }
    public class EmployeeOfficeDesignationService : IEmployeeOfficeDesignationService
    {
        private readonly IEmployeeOfficeDesignationRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public EmployeeOfficeDesignationService(IEmployeeOfficeDesignationRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<EmployeeOfficeDesignation> GetAll()
        {
            var entities = repository.GetAll().Where(c=> c.IsActive==true).OrderBy(c => c.EmpOfficeDesigId);
            return entities;
        }

        public EmployeeOfficeDesignation GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }
        public EmployeeOfficeDesignation GetByEmployeeId(long EmployeeId)
        {
            var entity = repository.GetMany(w => w.EmployeeId == EmployeeId && w.IsActive == true && w.EndDate == null).LastOrDefault();
            return entity;
        }
        public EmployeeOfficeDesignation Create(EmployeeOfficeDesignation objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(EmployeeOfficeDesignation objectToUpdate)
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

        public EmployeeOfficeDesignation Get(Expression<Func<EmployeeOfficeDesignation, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<EmployeeOfficeDesignation> GetMany(Expression<Func<EmployeeOfficeDesignation, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }

        #region Asyc
        public virtual async Task<IEnumerable<EmployeeOfficeDesignation>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }


        public virtual async Task<IEnumerable<EmployeeOfficeDesignation>> GetManyAsync(Expression<Func<EmployeeOfficeDesignation, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }



        public virtual async Task<EmployeeOfficeDesignation> GetAsync(Expression<Func<EmployeeOfficeDesignation, bool>> where)
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
        public IEnumerable<DBEmployeeOfficeDesignationDetails> GetDBEmployeeOfficeDesignationDetails(int startRowIndex, string jtSorting, int pageSize, out long TotCount)
        {
            return repository.GetEmployeeOfficeDesignationDetails(startRowIndex, jtSorting, pageSize, out TotCount);
        }
    }
}
