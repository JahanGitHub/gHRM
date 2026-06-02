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
    public interface IDiscCaseAnnexationService : IServiceBase<DiscCaseAnnexation>
    {
        IEnumerable<DiscCaseAnnexation> GetAllByCaseMasterId(int CaseMasterId);
    }
    public class DiscCaseAnnexationService :    IDiscCaseAnnexationService
    {
        private readonly IDiscCaseAnnexationRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public DiscCaseAnnexationService(IDiscCaseAnnexationRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<DiscCaseAnnexation> GetAll()
        {
            var entities = repository.GetAll().Where(c => c.IsActive == true).OrderBy(c => c.AnnexationId);
            return entities;
        }

        public IEnumerable<DiscCaseAnnexation> GetAllByCaseMasterId(int CaseMasterId)
        {
            var entities = repository.GetAll().Where(c => c.IsActive == true && c.CaseMasterId == CaseMasterId).OrderBy(c => c.AnnexationId);
            return entities;
        }
        public DiscCaseAnnexation GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }
        public DiscCaseAnnexation Create(DiscCaseAnnexation objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }
        public void Update(DiscCaseAnnexation objectToUpdate)
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

        public IEnumerable<DiscCaseAnnexation> GetMany(Expression<Func<DiscCaseAnnexation, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }



        #region Asyc
        public virtual async Task<IEnumerable<DiscCaseAnnexation>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }


        public virtual async Task<IEnumerable<DiscCaseAnnexation>> GetManyAsync(Expression<Func<DiscCaseAnnexation, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }



        public virtual async Task<DiscCaseAnnexation> GetAsync(Expression<Func<DiscCaseAnnexation, bool>> where)
        {
            return await repository.GetAsync(where);
        }

        public DiscCaseAnnexation Get(Expression<Func<DiscCaseAnnexation, bool>> where)
        {
            throw new NotImplementedException();
        }
        #endregion






    }
}
