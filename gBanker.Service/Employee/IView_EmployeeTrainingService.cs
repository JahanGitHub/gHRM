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
    public interface IView_EmployeeTrainingService : IServiceBase<View_EmployeeTraining>
    {

        //
    }
    public class View_EmployeeTrainingService : IView_EmployeeTrainingService
    {
        private readonly IView_EmployeeTrainingRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public View_EmployeeTrainingService(IView_EmployeeTrainingRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        public IEnumerable<View_EmployeeTraining> GetAll()
        {
            var entities = repository.GetAll().Where(c => c.IsActive == true).OrderBy(c => c.RowSl);
            return entities;
        }

        public View_EmployeeTraining GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public View_EmployeeTraining Create(View_EmployeeTraining objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(View_EmployeeTraining objectToUpdate)
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

        public View_EmployeeTraining Get(Expression<Func<View_EmployeeTraining, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<View_EmployeeTraining> GetMany(Expression<Func<View_EmployeeTraining, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }

        #region Asyc
        public virtual async Task<IEnumerable<View_EmployeeTraining>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }


        public virtual async Task<IEnumerable<View_EmployeeTraining>> GetManyAsync(Expression<Func<View_EmployeeTraining, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }



        public virtual async Task<View_EmployeeTraining> GetAsync(Expression<Func<View_EmployeeTraining, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion
    }
}

