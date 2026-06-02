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
    public interface IView_TimeKeepingRosterService : IServiceBase<View_TimeKeepingRoster>
    {

    }

    public class View_TimeKeepingRosterService : IView_TimeKeepingRosterService
    {
        private readonly IView_TimeKeepingRosterRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public View_TimeKeepingRosterService(IView_TimeKeepingRosterRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        public IEnumerable<View_TimeKeepingRoster> GetAll()
        {
            var entities = repository.GetAll().Where(c => c.IsActive == true).OrderBy(c => c.RowSl);
            return entities;
        }

        public View_TimeKeepingRoster GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public View_TimeKeepingRoster Create(View_TimeKeepingRoster objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(View_TimeKeepingRoster objectToUpdate)
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

        public View_TimeKeepingRoster Get(Expression<Func<View_TimeKeepingRoster, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<View_TimeKeepingRoster> GetMany(Expression<Func<View_TimeKeepingRoster, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }

        #region Asyc
        public virtual async Task<IEnumerable<View_TimeKeepingRoster>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }


        public virtual async Task<IEnumerable<View_TimeKeepingRoster>> GetManyAsync(Expression<Func<View_TimeKeepingRoster, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }



        public virtual async Task<View_TimeKeepingRoster> GetAsync(Expression<Func<View_TimeKeepingRoster, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion
    }
}

