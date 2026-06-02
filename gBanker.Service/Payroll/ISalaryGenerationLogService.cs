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
    public interface ISalaryGenerationLogService : IServiceBase<SalaryGenerationLog>
    {
       

    }
    public class SalaryGenerationLogService : ISalaryGenerationLogService
    {
        private readonly ISalaryGenerationLogRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public SalaryGenerationLogService(ISalaryGenerationLogRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        public IEnumerable<SalaryGenerationLog> GetAll()
        {
            var entities = repository.GetAll().Where(c => c.IsActive == true).OrderBy(c => c.ID);
            return entities;
        }

        public SalaryGenerationLog GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public SalaryGenerationLog Create(SalaryGenerationLog objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(SalaryGenerationLog objectToUpdate)
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
        

        public SalaryGenerationLog Get(Expression<Func<SalaryGenerationLog, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<SalaryGenerationLog> GetMany(Expression<Func<SalaryGenerationLog, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }
       
        #region Asyc
        public virtual async Task<IEnumerable<SalaryGenerationLog>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }


        public virtual async Task<IEnumerable<SalaryGenerationLog>> GetManyAsync(Expression<Func<SalaryGenerationLog, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }



        public virtual async Task<SalaryGenerationLog> GetAsync(Expression<Func<SalaryGenerationLog, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion
    }
}
