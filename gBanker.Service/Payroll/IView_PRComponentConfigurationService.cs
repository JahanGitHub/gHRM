using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.CodeFirstMigration.Payroll;
using gHRM.Data.Repository.Payroll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Service.Payroll
{
    public interface IView_PRComponentConfigurationService : IServiceBase<View_PRComponentConfiguration>
    {

    }

    public class View_PRComponentConfigurationService : IView_PRComponentConfigurationService
    {
        private readonly IView_PRComponentConfigurationRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public View_PRComponentConfigurationService(IView_PRComponentConfigurationRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        public IEnumerable<View_PRComponentConfiguration> GetAll()
        {
            var entities = repository.GetAll().Where(c => c.IsActive == true).OrderBy(c => c.RowSl);
            return entities;
        }

        public View_PRComponentConfiguration GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public View_PRComponentConfiguration Create(View_PRComponentConfiguration objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(View_PRComponentConfiguration objectToUpdate)
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

        public View_PRComponentConfiguration Get(Expression<Func<View_PRComponentConfiguration, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<View_PRComponentConfiguration> GetMany(Expression<Func<View_PRComponentConfiguration, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }

        #region Asyc
        public virtual async Task<IEnumerable<View_PRComponentConfiguration>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }


        public virtual async Task<IEnumerable<View_PRComponentConfiguration>> GetManyAsync(Expression<Func<View_PRComponentConfiguration, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }



        public virtual async Task<View_PRComponentConfiguration> GetAsync(Expression<Func<View_PRComponentConfiguration, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion
    }
}
