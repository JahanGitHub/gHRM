//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace gHRM.Service
//{
//    interface IPanelOfficerHistoryHistoryService
//    {
//    }
//}

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
    public interface IPanelOfficerHistoryService : IServiceBase<PanelOfficerHistory>
    {

    }
    public class PanelOfficerHistoryService : IPanelOfficerHistoryService
    {
        private readonly IPanelOfficerHistoryRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public PanelOfficerHistoryService(IPanelOfficerHistoryRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<PanelOfficerHistory> GetAll()
        {
            var entities = repository.GetAll().OrderBy(c => c.HistoryId);
            return entities;
        }

        public PanelOfficerHistory GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public PanelOfficerHistory Create(PanelOfficerHistory objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(PanelOfficerHistory objectToUpdate)
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


        public bool Inactivate(long id, DateTime? inactiveDate)
        {
            throw new NotImplementedException(); ;
        }


        public bool IsContinued(long id)
        {
            throw new NotImplementedException();
        }

        public PanelOfficerHistory Get(Expression<Func<PanelOfficerHistory, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<PanelOfficerHistory> GetMany(Expression<Func<PanelOfficerHistory, bool>> where)
        {
            var entities = repository.GetMany(where);
            return entities;
        }

        #region Asyc
        public virtual async Task<IEnumerable<PanelOfficerHistory>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }


        public virtual async Task<IEnumerable<PanelOfficerHistory>> GetManyAsync(Expression<Func<PanelOfficerHistory, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }



        public virtual async Task<PanelOfficerHistory> GetAsync(Expression<Func<PanelOfficerHistory, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion

    }
}

