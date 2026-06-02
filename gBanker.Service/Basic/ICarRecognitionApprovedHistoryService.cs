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
    public interface ICarRecognitionApprovedHistoryService : IServiceBase<CarRecognitionApprovedHistory>
    {

    }
    public class CarRecognitionApprovedHistoryService : ICarRecognitionApprovedHistoryService
    {
        private readonly ICarRecognitionApprovedHistoryRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public CarRecognitionApprovedHistoryService(ICarRecognitionApprovedHistoryRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        public IEnumerable<CarRecognitionApprovedHistory> GetAll()
        {
            var entities = repository.GetAll().Where(c => c.IsActive == true).OrderBy(c => c.CarRecognitionApprovedHistoryId);
            return entities;
        }

        public CarRecognitionApprovedHistory GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public CarRecognitionApprovedHistory Create(CarRecognitionApprovedHistory objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(CarRecognitionApprovedHistory objectToUpdate)
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

        public CarRecognitionApprovedHistory Get(Expression<Func<CarRecognitionApprovedHistory, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<CarRecognitionApprovedHistory> GetMany(Expression<Func<CarRecognitionApprovedHistory, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }

        #region Asyc
        public virtual async Task<IEnumerable<CarRecognitionApprovedHistory>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }


        public virtual async Task<IEnumerable<CarRecognitionApprovedHistory>> GetManyAsync(Expression<Func<CarRecognitionApprovedHistory, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }



        public virtual async Task<CarRecognitionApprovedHistory> GetAsync(Expression<Func<CarRecognitionApprovedHistory, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion
    }
}
