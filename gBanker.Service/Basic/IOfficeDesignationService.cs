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
    public interface IOfficeDesignationService : IServiceBase<OfficeDesignation>
    {
        IEnumerable<OfficeDesignation> getDesignationTypeWiseDesignation(string DesignationType);
        List<OfficeDesignation> AddOfficeDesignationList(List<OfficeDesignation> objs);
    }
    public class OfficeDesignationService : IOfficeDesignationService
    {
        private readonly IOfficeDesignationRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public OfficeDesignationService(IOfficeDesignationRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<OfficeDesignation> GetAll()
        {
            var entities = repository.GetAll().Where(c => c.IsActive ==true).OrderBy(c => c.OfficeDesignationId);
            return entities;
        }

        public OfficeDesignation GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public OfficeDesignation Create(OfficeDesignation objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(OfficeDesignation objectToUpdate)
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
        public IEnumerable<OfficeDesignation> getDesignationTypeWiseDesignation(string DesignationType)
        {
            var entities = repository.GetAll().Where(c => c.IsActive == true).OrderBy(o => o.DesignationOrder);
            return entities;
        }

        public List<OfficeDesignation> AddOfficeDesignationList(List<OfficeDesignation> objs)
        {
            repository.AddOfficeDesignationList(objs);
            return objs;
        }


        public void Save()
        {
            unitOfWork.Commit();
        }
        public bool Inactivate(long id, DateTime? inactiveDate)
        {
            throw new NotImplementedException();
        }
        public bool IsContinued(long id)
        {
            throw new NotImplementedException();
        }

        public OfficeDesignation Get(Expression<Func<OfficeDesignation, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<OfficeDesignation> GetMany(Expression<Func<OfficeDesignation, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }

        #region Asyc
        public virtual async Task<IEnumerable<OfficeDesignation>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }


        public virtual async Task<IEnumerable<OfficeDesignation>> GetManyAsync(Expression<Func<OfficeDesignation, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }



        public virtual async Task<OfficeDesignation> GetAsync(Expression<Func<OfficeDesignation, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion
    }
}
