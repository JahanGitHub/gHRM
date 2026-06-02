using gHRM.Data.CodeFirstMigration;
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
    public interface IComponentPayrollService : IServiceBase<ComponentPayroll>
    {
        ComponentPayroll GetByComponentName(string componentName);
    }
    public class ComponentPayrollService : IComponentPayrollService
    {
        private readonly IComponentPayrollRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public ComponentPayrollService(IComponentPayrollRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<ComponentPayroll> GetAll()
        {
            var entities = repository.GetAll().OrderBy(c => c.ComponentName);
            return entities;
        }

        public ComponentPayroll GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public ComponentPayroll GetByComponentName(string componentName)
        {
            var single = new ComponentPayroll();

            using (var db = new gHRMDBContext())
            {
                single = db.ComponentPayroll.FirstOrDefault(f => f.ComponentName == componentName);
            }

            return single;
        }

        public ComponentPayroll Create(ComponentPayroll objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(ComponentPayroll objectToUpdate)
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


        public ComponentPayroll Get(Expression<Func<ComponentPayroll, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<ComponentPayroll> GetMany(Expression<Func<ComponentPayroll, bool>> where)
        {
            var entities = repository.GetMany(where).Where(x=>x.IsActive);
            return entities;
        }

        #region Asyc
        public virtual async Task<IEnumerable<ComponentPayroll>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }


        public virtual async Task<IEnumerable<ComponentPayroll>> GetManyAsync(Expression<Func<ComponentPayroll, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }



        public virtual async Task<ComponentPayroll> GetAsync(Expression<Func<ComponentPayroll, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion
    }
}
