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
    public interface ICarRecognitionService : IServiceBase<CarRecognition>
    {

    }
    public class CarRecognitionService : ICarRecognitionService
    {
        private readonly ICarRecognitionRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public CarRecognitionService(ICarRecognitionRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        public IEnumerable<CarRecognition> GetAll()
        {
            var entities = repository.GetAll().Where(c => c.IsActive == true).OrderBy(c => c.CarRecognitionId);
            return entities;
        }

        public CarRecognition GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public CarRecognition Create(CarRecognition objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(CarRecognition objectToUpdate)
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

        public CarRecognition Get(Expression<Func<CarRecognition, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<CarRecognition> GetMany(Expression<Func<CarRecognition, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }

        #region Asyc
        public virtual async Task<IEnumerable<CarRecognition>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }


        public virtual async Task<IEnumerable<CarRecognition>> GetManyAsync(Expression<Func<CarRecognition, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }



        public virtual async Task<CarRecognition> GetAsync(Expression<Func<CarRecognition, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion
    }
}
