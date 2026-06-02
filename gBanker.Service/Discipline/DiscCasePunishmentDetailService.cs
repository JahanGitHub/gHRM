using gHRM.Data.CodeFirstMigration.Discipline;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.Repository.Discipline;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace gHRM.Service.Discipline
{
    public interface IDiscCasePunishmentDetailService : IServiceBase<DiscCasePunishmentDetail>
    {

    }
    public class DiscCasePunishmentDetailService : IDiscCasePunishmentDetailService
    {
        private readonly IDiscCasePunishmentDetailRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public DiscCasePunishmentDetailService(IDiscCasePunishmentDetailRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<DiscCasePunishmentDetail> GetAll()
        {
            var entities = repository.GetAll().Where(c => c.IsActive == true).OrderBy(c => c.PunishmentDetailId);
            return entities;
        }

        public DiscCasePunishmentDetail GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }



        public DiscCasePunishmentDetail Create(DiscCasePunishmentDetail objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(DiscCasePunishmentDetail objectToUpdate)
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

        public IEnumerable<DiscCasePunishmentDetail> GetMany(Expression<Func<DiscCasePunishmentDetail, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }

        public DiscCasePunishmentDetail Get(Expression<Func<DiscCasePunishmentDetail, bool>> where)
        {
            throw new NotImplementedException();
        }


        public Task<IEnumerable<DiscCasePunishmentDetail>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<DiscCasePunishmentDetail>> GetManyAsync(Expression<Func<DiscCasePunishmentDetail, bool>> where)
        {
            throw new NotImplementedException();
        }

        public Task<DiscCasePunishmentDetail> GetAsync(Expression<Func<DiscCasePunishmentDetail, bool>> where)
        {
            throw new NotImplementedException();
        }
    }
}
