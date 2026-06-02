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
    public interface IView_EmployeeOfficeTimeExceptionService : IServiceBase<View_EmployeeOfficeTimeException>
    {

        //
    }
    public class View_EmployeeOfficeTimeExceptionService : IView_EmployeeOfficeTimeExceptionService
    {
        private readonly IView_EmployeeOfficeTimeExceptionRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public View_EmployeeOfficeTimeExceptionService(IView_EmployeeOfficeTimeExceptionRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        public IEnumerable<View_EmployeeOfficeTimeException> GetAll()
        {
            var entities = repository.GetAll().Where(c => c.IsActive == true).OrderBy(c => c.RowSl);
            return entities;
        }

        public View_EmployeeOfficeTimeException GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public View_EmployeeOfficeTimeException Create(View_EmployeeOfficeTimeException objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(View_EmployeeOfficeTimeException objectToUpdate)
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

        public View_EmployeeOfficeTimeException Get(Expression<Func<View_EmployeeOfficeTimeException, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<View_EmployeeOfficeTimeException> GetMany(Expression<Func<View_EmployeeOfficeTimeException, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }

        #region Asyc
        public virtual async Task<IEnumerable<View_EmployeeOfficeTimeException>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }


        public virtual async Task<IEnumerable<View_EmployeeOfficeTimeException>> GetManyAsync(Expression<Func<View_EmployeeOfficeTimeException, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }



        public virtual async Task<View_EmployeeOfficeTimeException> GetAsync(Expression<Func<View_EmployeeOfficeTimeException, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion
    }
}
