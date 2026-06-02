using gHRM.Core.Common;
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.Discipline;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.DBDetailModels;
using gHRM.Data.Repository;
using gHRM.Data.Repository.Discipline;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Service.Discipline
{
    public interface IDiscCasePunishmentMasterService : IServiceBase<DiscCasePunishmentMaster>
    {

    }
    public class DiscCasePunishmentMasterService : IDiscCasePunishmentMasterService
    {
        private readonly IDiscCasePunishmentMasterRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public DiscCasePunishmentMasterService(IDiscCasePunishmentMasterRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<DiscCasePunishmentMaster> GetAll()
        {
            var entities = repository.GetAll().Where(c => c.IsActive == true).OrderBy(c => c.PunishmentMasterId);
            return entities;
        }

        public DiscCasePunishmentMaster GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }



        public DiscCasePunishmentMaster Create(DiscCasePunishmentMaster objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(DiscCasePunishmentMaster objectToUpdate)
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
            throw new NotImplementedException();
        }
        public bool IsContinued(long id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DiscCasePunishmentMaster> GetMany(Expression<Func<DiscCasePunishmentMaster, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }

        public DiscCasePunishmentMaster Get(Expression<Func<DiscCasePunishmentMaster, bool>> where)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<DiscCasePunishmentMaster>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<DiscCasePunishmentMaster>> GetManyAsync(Expression<Func<DiscCasePunishmentMaster, bool>> where)
        {
            throw new NotImplementedException();
        }

        public Task<DiscCasePunishmentMaster> GetAsync(Expression<Func<DiscCasePunishmentMaster, bool>> where)
        {
            throw new NotImplementedException();
        }
    }
}
