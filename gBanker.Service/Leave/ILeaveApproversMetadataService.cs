
using gHRM.Core.Common;
using gHRM.Data;
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
    public interface ILeaveApproversMetadataService : IServiceBase<LeaveApproversMetadata>
    {
        List<LeaveApproversMetadata> AddLeaveApproversMetadataList(List<LeaveApproversMetadata> objs);
    }


    public class LeaveApproversMetadataServiceService : ILeaveApproversMetadataService
    {
        private readonly ILeaveApproversMetadataRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;


        public LeaveApproversMetadataServiceService(ILeaveApproversMetadataRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;

        }
        public IEnumerable<LeaveApproversMetadata> GetAll()
        {
            var entities = repository.GetAll();
            return entities;
        }

        public LeaveApproversMetadata GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public LeaveApproversMetadata Create(LeaveApproversMetadata objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(LeaveApproversMetadata objectToUpdate)
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

        public List<LeaveApproversMetadata> AddLeaveApproversMetadataList(List<LeaveApproversMetadata> objs)
        {
            repository.AddLeaveApproversMetadataList(objs);
            return objs;
        }


        public bool Inactivate(long id, DateTime? inactiveDate)
        {
            throw new NotImplementedException(); ;
        }


        public bool IsContinued(long id)
        {
            throw new NotImplementedException();
        }

        public LeaveApproversMetadata Get(Expression<Func<LeaveApproversMetadata, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<LeaveApproversMetadata> GetMany(Expression<Func<LeaveApproversMetadata, bool>> where)
        {
            var entities = repository.GetMany(where);
            return entities;
        }

        #region Asyc
        public virtual async Task<IEnumerable<LeaveApproversMetadata>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }


        public virtual async Task<IEnumerable<LeaveApproversMetadata>> GetManyAsync(Expression<Func<LeaveApproversMetadata, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }



        public virtual async Task<LeaveApproversMetadata> GetAsync(Expression<Func<LeaveApproversMetadata, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion

    }
}

