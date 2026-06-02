using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.CodeFirstMigration.PF;
using gHRM.Data.Repository.PF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Service.PF
{
    public interface IVw_AccChartService : IServiceBase<Vw_AccChart>
    {
      
    }
    public class Vw_AccChartService : IVw_AccChartService
    {
        private readonly IVw_AccChartRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public Vw_AccChartService(IVw_AccChartRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<Vw_AccChart> GetAll()
        {
            var entities = repository.GetAll();
            return entities;
        }
        public Vw_AccChart GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }
        public void Save()
        {
            unitOfWork.Commit();
        }
        public Vw_AccChart Create(Vw_AccChart objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }


        public void Update(Vw_AccChart objectToUpdate)
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
        public bool Inactivate(long id, DateTime? inactiveDate)
        {
            //throw new NotImplementedException();
            var obj = repository.GetById(id);
            if (obj != null)
            {
                //obj.InActiveDate = DateTime.Now;
                //obj.IsActive = false;
                repository.Update(obj);
                Save();
                return true;
            }
            return false;
        }
        public bool IsContinued(long id)
        {
            // throw new NotImplementedException();
            var obj = repository.GetById(id);
            if (obj != null)
            {
                //var isActive = obj.IsActive;
                //if (isActive == false)
                //{
                //    return false;
                //}
            }
            return true;
        }

        #region Asyc
        public virtual async Task<IEnumerable<Vw_AccChart>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }
        public virtual async Task<IEnumerable<Vw_AccChart>> GetManyAsync(Expression<Func<Vw_AccChart, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }
        public virtual async Task<Vw_AccChart> GetAsync(Expression<Func<Vw_AccChart, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion

        public Vw_AccChart Get(Expression<Func<Vw_AccChart, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<Vw_AccChart> GetMany(Expression<Func<Vw_AccChart, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsRemoved == false);
            return entities;
        }
    }
}
