using gHRM.Core.Common;
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.Basic;
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
    public interface ICarRecognitionApprovalService : IServiceBase<CarRecognitionApproval>
    {

    }
    public class CarRecognitionApprovalService : ICarRecognitionApprovalService
    {
        private readonly ICarRecognitionApprovalRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public CarRecognitionApprovalService(ICarRecognitionApprovalRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        public IEnumerable<CarRecognitionApproval> GetAll()
        {
            var entities = repository.GetAll().Where(c => c.IsActive == true).OrderBy(c => c.ApprovalId);
            return entities;
        }

        public CarRecognitionApproval GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public CarRecognitionApproval Create(CarRecognitionApproval objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(CarRecognitionApproval objectToUpdate)
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

        public CarRecognitionApproval Get(Expression<Func<CarRecognitionApproval, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<CarRecognitionApproval> GetMany(Expression<Func<CarRecognitionApproval, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }

        #region Asyc
        public virtual async Task<IEnumerable<CarRecognitionApproval>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }


        public virtual async Task<IEnumerable<CarRecognitionApproval>> GetManyAsync(Expression<Func<CarRecognitionApproval, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }



        public virtual async Task<CarRecognitionApproval> GetAsync(Expression<Func<CarRecognitionApproval, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion
    }
}

