using gHRM.Core.Common;
using gHRM.Core.Filters.Offices;
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.Apply;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.DBDetailModels;
using gHRM.Data.Repository;
using gHRM.Data.Repository.Apply;
using gHRM.Data.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;


namespace gHRM.Service.Apply
{

    public interface ILevelofEducationService : IServiceBase<LevelofEducation>
    {

    }

    public class LevelofEducationService : ILevelofEducationService
    {
        private readonly ILevelofEducationRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public LevelofEducationService(ILevelofEducationRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        public LevelofEducation Create(LevelofEducation objectToCreate)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public LevelofEducation Get(Expression<Func<LevelofEducation, bool>> where)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<LevelofEducation> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<LevelofEducation>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<LevelofEducation> GetAsync(Expression<Func<LevelofEducation, bool>> where)
        {
            throw new NotImplementedException();
        }

        public LevelofEducation GetById(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<LevelofEducation> GetMany(Expression<Func<LevelofEducation, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }

        public Task<IEnumerable<LevelofEducation>> GetManyAsync(Expression<Func<LevelofEducation, bool>> where)
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
            throw new NotImplementedException();
        }

        public void Update(LevelofEducation objectToUpdate)
        {
            throw new NotImplementedException();
        }
    }
}
