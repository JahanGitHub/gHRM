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
    public interface IDiscEnqueryOfficerService : IServiceBase<DiscEnqueryOfficer>
    {
        DiscEnqueryOfficer GetByEmployeeIdAndOfficeId(Int64 EmployeeId, int OfficeId);
        IEnumerable<Employee> GetEmployeeByOfficeId(int officeId);
    }
    public class DiscEnqueryOfficerService : IDiscEnqueryOfficerService
    {
        private readonly IDiscEnqueryOfficerRepository repository;
        private readonly IEmployeeRepository empRepository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public DiscEnqueryOfficerService(IDiscEnqueryOfficerRepository repository, IUnitOfWorkCodeFirst unitOfWork, IEmployeeRepository empRepository)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
            this.empRepository = empRepository;
        }
        public IEnumerable<DiscEnqueryOfficer> GetAll()
        {
            var entities = repository.GetAll().Where(s => s.IsActive == true).OrderBy(c => c.EnqueryOfficerId);
            return entities;
        }

        public IEnumerable<Employee> GetEmployeeByOfficeId(int officeId)
        {
            var enquery = repository.GetMany(s => s.IsActive == true && s.OfficeId == officeId).Select(S => S.EmployeeId);
            var entities = empRepository.GetMany(w => w.IsActive == true && enquery.Contains(w.EmployeeId));
            return entities;
        }

        public DiscEnqueryOfficer GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public DiscEnqueryOfficer GetByEmployeeIdAndOfficeId(Int64 EmployeeId, int OfficeId)
        {
            var entity = repository.Get(g => g.EmployeeId == EmployeeId && g.OfficeId == OfficeId && g.IsActive == true);
            return entity;
        }

        public DiscEnqueryOfficer Create(DiscEnqueryOfficer objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(DiscEnqueryOfficer objectToUpdate)
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

        public IEnumerable<DiscEnqueryOfficer> GetMany(Expression<Func<DiscEnqueryOfficer, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }

        public DiscEnqueryOfficer Get(Expression<Func<DiscEnqueryOfficer, bool>> where)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<DiscEnqueryOfficer>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<DiscEnqueryOfficer>> GetManyAsync(Expression<Func<DiscEnqueryOfficer, bool>> where)
        {
            throw new NotImplementedException();
        }

        public Task<DiscEnqueryOfficer> GetAsync(Expression<Func<DiscEnqueryOfficer, bool>> where)
        {
            throw new NotImplementedException();
        }
    }
}
