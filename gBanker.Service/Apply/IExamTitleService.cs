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

    public interface IExamTitleService : IServiceBase<ExamTitle>
    {

    }

    public class ExamTitleService : IExamTitleService
    {
        private readonly IExamTitleRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public ExamTitleService(IExamTitleRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        public ExamTitle Create(ExamTitle objectToCreate)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public ExamTitle Get(Expression<Func<ExamTitle, bool>> where)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ExamTitle> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ExamTitle>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ExamTitle> GetAsync(Expression<Func<ExamTitle, bool>> where)
        {
            throw new NotImplementedException();
        }

        public ExamTitle GetById(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ExamTitle> GetMany(Expression<Func<ExamTitle, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }

        public Task<IEnumerable<ExamTitle>> GetManyAsync(Expression<Func<ExamTitle, bool>> where)
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

        public void Update(ExamTitle objectToUpdate)
        {
            throw new NotImplementedException();
        }
    }
}
