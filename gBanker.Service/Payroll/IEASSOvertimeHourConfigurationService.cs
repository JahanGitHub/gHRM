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
    public interface IEASSOvertimeHourConfigurationService : IServiceBase<EASSOvertimeHourConfiguration>
    {

    }
    public class EASSOvertimeHourConfigurationService : IEASSOvertimeHourConfigurationService
    {
        private readonly IEASSOvertimeHourConfigurationRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public EASSOvertimeHourConfigurationService(IEASSOvertimeHourConfigurationRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<EASSOvertimeHourConfiguration> GetAll()
        {
            var entities = repository.GetAll();
            return entities;
        }

        public EASSOvertimeHourConfiguration GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public EASSOvertimeHourConfiguration Create(EASSOvertimeHourConfiguration objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(EASSOvertimeHourConfiguration objectToUpdate)
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


        public EASSOvertimeHourConfiguration Get(Expression<Func<EASSOvertimeHourConfiguration, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<EASSOvertimeHourConfiguration> GetMany(Expression<Func<EASSOvertimeHourConfiguration, bool>> where)
        {
            var entities = repository.GetMany(where);
            return entities;
        }

        #region Asyc
        public virtual async Task<IEnumerable<EASSOvertimeHourConfiguration>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }


        public virtual async Task<IEnumerable<EASSOvertimeHourConfiguration>> GetManyAsync(Expression<Func<EASSOvertimeHourConfiguration, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }



        public virtual async Task<EASSOvertimeHourConfiguration> GetAsync(Expression<Func<EASSOvertimeHourConfiguration, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion
    }
}
