using gHRM.Core.Common;
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.Discipline;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
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
    public interface IDiscCaseEnquiryOfficerService : IServiceBase<DiscCaseEnquiryOfficer>
    {

    }
    public class DiscCaseEnquiryOfficerService : IDiscCaseEnquiryOfficerService
    {
        private readonly IDiscCaseEnquiryOfficerRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;


        public DiscCaseEnquiryOfficerService(IDiscCaseEnquiryOfficerRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<DiscCaseEnquiryOfficer> GetAll()
        {
            var entities = repository.GetAll().Where(c => c.IsActive == true).OrderBy(c => c.EmployeeId);
            return entities;
        }


        public DiscCaseEnquiryOfficer GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public DiscCaseEnquiryOfficer Create(DiscCaseEnquiryOfficer objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(DiscCaseEnquiryOfficer objectToUpdate)
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

        public bool IsContinued(long id)
        {
            throw new NotImplementedException();
        }

        public bool Inactivate(long id, DateTime? inactiveDate)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DiscCaseEnquiryOfficer> GetMany(Expression<Func<DiscCaseEnquiryOfficer, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }

        public DiscCaseEnquiryOfficer Get(Expression<Func<DiscCaseEnquiryOfficer, bool>> where)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<DiscCaseEnquiryOfficer>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<DiscCaseEnquiryOfficer>> GetManyAsync(Expression<Func<DiscCaseEnquiryOfficer, bool>> where)
        {
            throw new NotImplementedException();
        }

        public Task<DiscCaseEnquiryOfficer> GetAsync(Expression<Func<DiscCaseEnquiryOfficer, bool>> where)
        {
            throw new NotImplementedException();
        }
    }
}
