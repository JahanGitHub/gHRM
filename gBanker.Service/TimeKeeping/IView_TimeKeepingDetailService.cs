
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
    public interface IView_TimeKeepingDetailService : IServiceBase<View_TimeKeepingDetail>
    {

    }
    public class View_TimeKeepingDetailService : IView_TimeKeepingDetailService
    {
        private readonly IView_TimeKeepingDetailRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public View_TimeKeepingDetailService(IView_TimeKeepingDetailRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<View_TimeKeepingDetail> GetAll()
        {
            var entities = repository.GetAll().OrderBy(c => c.rowSl);
            return entities;
        }

        public View_TimeKeepingDetail GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public View_TimeKeepingDetail Create(View_TimeKeepingDetail objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(View_TimeKeepingDetail objectToUpdate)
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


        public View_TimeKeepingDetail Get(Expression<Func<View_TimeKeepingDetail, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<View_TimeKeepingDetail> GetMany(Expression<Func<View_TimeKeepingDetail, bool>> where)
        {
            var entities = repository.GetMany(where);
            return entities;
        }

        #region Asyc
        public virtual async Task<IEnumerable<View_TimeKeepingDetail>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }


        public virtual async Task<IEnumerable<View_TimeKeepingDetail>> GetManyAsync(Expression<Func<View_TimeKeepingDetail, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }



        public virtual async Task<View_TimeKeepingDetail> GetAsync(Expression<Func<View_TimeKeepingDetail, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion
    }
}

