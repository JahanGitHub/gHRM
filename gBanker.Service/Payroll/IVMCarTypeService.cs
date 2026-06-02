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
    public interface IVMCarTypeService : IServiceBase<VMCarType>
    {


    }
    public class VMCarTypeService : IVMCarTypeService
    {
        private readonly IVMCarTypeRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public VMCarTypeService(IVMCarTypeRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        public IEnumerable<VMCarType> GetAll()
        {
            var entities = repository.GetAll().Where(c => c.IsActive == true).OrderBy(c => c.CarTypeId);
            return entities;
        }

        public VMCarType GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public VMCarType Create(VMCarType objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(VMCarType objectToUpdate)
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

        public VMCarType Get(Expression<Func<VMCarType, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<VMCarType> GetMany(Expression<Func<VMCarType, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }

        #region Asyc
        public virtual async Task<IEnumerable<VMCarType>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }


        public virtual async Task<IEnumerable<VMCarType>> GetManyAsync(Expression<Func<VMCarType, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }



        public virtual async Task<VMCarType> GetAsync(Expression<Func<VMCarType, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion
    }
}
