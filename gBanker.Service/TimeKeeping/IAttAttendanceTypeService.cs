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
    public interface IAttAttendanceTypeService : IServiceBase<AttAttendanceType>
    {


    }
    public class AttAttendanceTypeService : IAttAttendanceTypeService
    {
        private readonly IAttAttendanceTypeRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public AttAttendanceTypeService(IAttAttendanceTypeRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        public IEnumerable<AttAttendanceType> GetAll()
        {
            var entities = repository.GetAll().Where(c => c.IsActive == true).OrderBy(c => c.AttAttendanceTypeId);
            return entities;
        }

        public AttAttendanceType GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public AttAttendanceType Create(AttAttendanceType objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(AttAttendanceType objectToUpdate)
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

        public AttAttendanceType Get(Expression<Func<AttAttendanceType, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<AttAttendanceType> GetMany(Expression<Func<AttAttendanceType, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }

        #region Asyc
        public virtual async Task<IEnumerable<AttAttendanceType>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }


        public virtual async Task<IEnumerable<AttAttendanceType>> GetManyAsync(Expression<Func<AttAttendanceType, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }



        public virtual async Task<AttAttendanceType> GetAsync(Expression<Func<AttAttendanceType, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion
    }
}
