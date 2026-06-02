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
    public interface IView_EmployeeDesignationService : IServiceBase<View_EmployeeDesignation>
    {


    }
    public class View_EmployeeDesignationService : IView_EmployeeDesignationService
    {
        private readonly IView_EmployeeDesignationRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public View_EmployeeDesignationService(IView_EmployeeDesignationRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        public IEnumerable<View_EmployeeDesignation> GetAll()
        {
            var entities = repository.GetAll().Where(c => c.IsActive == true).OrderBy(c => c.RowSl);
            return entities;
        }

        public View_EmployeeDesignation GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public View_EmployeeDesignation Create(View_EmployeeDesignation objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(View_EmployeeDesignation objectToUpdate)
        {
            repository.Update(objectToUpdate);
            Save();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public bool Inactivate(long id, DateTime? inactiveDate)
        {
            throw new NotImplementedException();
        }

        public bool IsContinued(long id)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            unitOfWork.Commit();
        }

        public View_EmployeeDesignation Get(Expression<Func<View_EmployeeDesignation, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<View_EmployeeDesignation> GetMany(Expression<Func<View_EmployeeDesignation, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }

        #region Asyc
        public virtual async Task<IEnumerable<View_EmployeeDesignation>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }


        public virtual async Task<IEnumerable<View_EmployeeDesignation>> GetManyAsync(Expression<Func<View_EmployeeDesignation, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }



        public virtual async Task<View_EmployeeDesignation> GetAsync(Expression<Func<View_EmployeeDesignation, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion
    }
}
