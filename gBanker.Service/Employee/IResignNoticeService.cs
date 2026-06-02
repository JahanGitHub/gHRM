using BasicDataAccess;
using gHRM.Core.Common;
using gHRM.Core.Filters.Offices;
using gHRM.Core.Utilities;
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.DBDetailModels;
using gHRM.Data.Repository;
using gHRM.Data.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Service
{
    public interface IResignNoticeService : IServiceBase<ResignNotice>
    {
        void DeleteResignNotice(long Id);
        bool HasDuplicate(long EmployeeId);
    }
    public class ResignNoticeService : IResignNoticeService
    {
        private readonly IResignNoticeRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public ResignNoticeService(IResignNoticeRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        public void DeleteResignNotice(long Id)
        {
            repository.DeleteResignNotice(Id);
        }

        public bool HasDuplicate(long EmployeeId)
        {
            return repository.HasDuplicate(EmployeeId);
        }

        #region Implementation for IServiceBase
        public IEnumerable<ResignNotice> GetAll()
        {
            var entities = repository.GetMany(g => g.IsActive == true).OrderBy(c => c.Id);
            return entities;
        }

        public ResignNotice GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public ResignNotice Create(ResignNotice objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(ResignNotice objectToUpdate)
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
            var obj = repository.GetById(id);
            if (obj != null)
            {
                obj.IsActive = false;
                repository.Update(obj);
                Save();
                return true;
            }
            return false;
        }

        public bool IsContinued(long id)
        {
            var obj = repository.GetById(id);
            if (obj != null)
            {
                var isActive = obj.IsActive;
                if (isActive == true)
                {
                    return false;
                }
            }
            return true;
        }

        public ResignNotice Get(Expression<Func<ResignNotice, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }

        public IEnumerable<ResignNotice> GetMany(Expression<Func<ResignNotice, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }

        public virtual async Task<IEnumerable<ResignNotice>> GetAllAsync()
        {
            return await repository.GetAllAsync();
        }

        public virtual async Task<IEnumerable<ResignNotice>> GetManyAsync(Expression<Func<ResignNotice, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }

        public virtual async Task<ResignNotice> GetAsync(Expression<Func<ResignNotice, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion
    }
}
