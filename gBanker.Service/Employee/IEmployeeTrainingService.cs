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
    public interface IEmployeeTrainingService : IServiceBase<EmployeeTraining>
    {

        bool IsExistEmployeeTraining(EmployeeTraining model);
    }
    public class EmployeeTrainingService : IEmployeeTrainingService
    {
        private readonly IEmployeeTrainingRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public EmployeeTrainingService(IEmployeeTrainingRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        public IEnumerable<EmployeeTraining> GetAll()
        {
            var entities = repository.GetAll().Where(c => c.IsActive == true).OrderBy(c => c.EmployeeTrainingId);
            return entities;
        }

        public EmployeeTraining GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public bool IsExistEmployeeTraining(EmployeeTraining model)
        {
            var isExist = true;
            using (var db = new gHRMDBContext())
            {
                if (model.EmployeeTrainingId > 0)
                    isExist = db.EmployeeTrainings.Any(f => f.IsActive == true && f.EmployeeId==model.EmployeeId && f.TrainingTitle == model.TrainingTitle && f.EmployeeTrainingId != model.EmployeeTrainingId);
                else
                    isExist = db.EmployeeTrainings.Any(f => f.IsActive == true && f.EmployeeId == model.EmployeeId && f.TrainingTitle == model.TrainingTitle);
            }

            return isExist;
        }

        public EmployeeTraining Create(EmployeeTraining objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(EmployeeTraining objectToUpdate)
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

        public EmployeeTraining Get(Expression<Func<EmployeeTraining, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<EmployeeTraining> GetMany(Expression<Func<EmployeeTraining, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }

        #region Asyc
        public virtual async Task<IEnumerable<EmployeeTraining>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }


        public virtual async Task<IEnumerable<EmployeeTraining>> GetManyAsync(Expression<Func<EmployeeTraining, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }



        public virtual async Task<EmployeeTraining> GetAsync(Expression<Func<EmployeeTraining, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion
    }
}
