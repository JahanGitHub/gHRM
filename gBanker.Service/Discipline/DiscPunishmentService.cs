using System;
using gHRM.Core.Common;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Expressions;
using gHRM.Data.CodeFirstMigration.Discipline;
using gHRM.Data.Repository.Discipline;
using gHRM.Data.DBDetailModels.Discipline;

namespace gHRM.Service.Discipline
{
    public interface IDiscPunishmentService : IServiceBase<DiscPunishment>
    {
        IEnumerable<ValidationResult> IsValidPunishment(string PunishmentCode);
        IEnumerable<DBDiscPunishmentDetailsModels> GetDiscPunishmentDetail(string filterColumnName, string filterValue, int startRowIndex, string jtSorting, int pageSize, out long TotCount);

    }
    public class DiscPunishmentService : IDiscPunishmentService
    {
        private readonly IDiscPunishmentRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public DiscPunishmentService(IDiscPunishmentRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<DiscPunishment> GetAll()
        {
            var entities = repository.GetAll().Where(c => c.IsActive == true).OrderBy(c => c.PunishmentCode);
            return entities;
        }

        public DiscPunishment GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public DiscPunishment Create(DiscPunishment objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(DiscPunishment objectToUpdate)
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
        public IEnumerable<DiscPunishment> GetMany(Expression<Func<DiscPunishment, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }

        IEnumerable<ValidationResult> IDiscPunishmentService.IsValidPunishment(string PunishmentCode)
        {
            var entity = repository.Get(p => p.PunishmentCode == PunishmentCode);
            if (entity != null)
            {
                yield return new ValidationResult("PunishmentCode", "Duplicate Punishment Code.");

            }
        }
        public IEnumerable<DBDiscPunishmentDetailsModels> GetDiscPunishmentDetail(string filterColumnName, string filterValue, int startRowIndex, string jtSorting, int pageSize, out long TotCount)
        {
            return repository.GetPunishmentDetail(filterColumnName, filterValue, startRowIndex, jtSorting, pageSize, out TotCount);
        }

        public DiscPunishment Get(Expression<Func<DiscPunishment, bool>> where)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<DiscPunishment>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<DiscPunishment>> GetManyAsync(Expression<Func<DiscPunishment, bool>> where)
        {
            throw new NotImplementedException();
        }

        public Task<DiscPunishment> GetAsync(Expression<Func<DiscPunishment, bool>> where)
        {
            throw new NotImplementedException();
        }
    }
}
