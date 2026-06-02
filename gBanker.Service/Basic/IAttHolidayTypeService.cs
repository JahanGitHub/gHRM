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

    public interface IAttHolidayTypeService : IServiceBase<AttHolidayType>
    {

    }

    public class AttHolidayTypeService : IAttHolidayTypeService
    {
        private readonly IAttHolidayTypeRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public AttHolidayTypeService(IAttHolidayTypeRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<AttHolidayType> GetAll()
        {
            var entities = repository.GetAll().Where(c => c.IsActive == true).OrderBy(c => c.AttHolidayTypeId);
            return entities;
        }
        public AttHolidayType GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }
        public void Save()
        {
            unitOfWork.Commit();
        }


        public AttHolidayType Get(Expression<Func<AttHolidayType, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<AttHolidayType> GetMany(Expression<Func<AttHolidayType, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }

        #region Asyc
        public virtual async Task<IEnumerable<AttHolidayType>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }


        public virtual async Task<IEnumerable<AttHolidayType>> GetManyAsync(Expression<Func<AttHolidayType, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }



        public virtual async Task<AttHolidayType> GetAsync(Expression<Func<AttHolidayType, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion

        public AttHolidayType Create(AttHolidayType objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(AttHolidayType objectToUpdate)
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
            throw new NotImplementedException();
        }
        public bool IsContinued(long id)
        {
            // throw new NotImplementedException();
            var obj = repository.GetById(id);
            if (obj != null)
            {
                var isActive = obj.IsActive;
                if (isActive == false)
                {
                    return false;
                }
            }

            return true;
        }


    } //End of Class
}//End of Namespace
