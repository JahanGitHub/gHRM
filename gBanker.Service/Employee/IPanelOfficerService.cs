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
    public interface IPanelOfficerService : IServiceBase<PanelOfficer>
    {

    }
    public class PanelOfficerService : IPanelOfficerService
    {
        private readonly IPanelOfficerRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public PanelOfficerService(IPanelOfficerRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<PanelOfficer> GetAll()
        {
            var entities = repository.GetAll().OrderBy(c => c.ID);
            return entities;
        }

        public PanelOfficer GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public PanelOfficer Create(PanelOfficer objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(PanelOfficer objectToUpdate)
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

        public PanelOfficer Get(Expression<Func<PanelOfficer, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<PanelOfficer> GetMany(Expression<Func<PanelOfficer, bool>> where)
        {
            var entities = repository.GetMany(where);
            return entities;
        }

        #region Asyc
        public virtual async Task<IEnumerable<PanelOfficer>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }


        public virtual async Task<IEnumerable<PanelOfficer>> GetManyAsync(Expression<Func<PanelOfficer, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }



        public virtual async Task<PanelOfficer> GetAsync(Expression<Func<PanelOfficer, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion

    }
}
