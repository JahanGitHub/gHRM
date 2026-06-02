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
    public interface IEmployeeGradeListService : IServiceBase<EmployeeGradeList>
    {


    }
    public class EmployeeGradeListService : IEmployeeGradeListService
    {
        private readonly IEmployeeGradeListRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public EmployeeGradeListService(IEmployeeGradeListRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        public IEnumerable<EmployeeGradeList> GetAll()
        {
            var entities = repository.GetAll().Where(c => c.IsActive == true).OrderBy(c => c.Id);
            return entities;
        }

        public EmployeeGradeList GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public EmployeeGradeList Create(EmployeeGradeList objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(EmployeeGradeList objectToUpdate)
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

        public EmployeeGradeList Get(Expression<Func<EmployeeGradeList, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<EmployeeGradeList> GetMany(Expression<Func<EmployeeGradeList, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }

        #region Asyc
        public virtual async Task<IEnumerable<EmployeeGradeList>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }


        public virtual async Task<IEnumerable<EmployeeGradeList>> GetManyAsync(Expression<Func<EmployeeGradeList, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }



        public virtual async Task<EmployeeGradeList> GetAsync(Expression<Func<EmployeeGradeList, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion
    }
}
