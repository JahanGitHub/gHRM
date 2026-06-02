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
    public interface IDiscDealingOfficerService : IServiceBase<DiscDealingOfficer>
    {
        DiscDealingOfficer GetByEmployeeIdAndOfficeId(Int64 EmployeeId, int OfficeId);
        IEnumerable<Employee> GetEmployeeByOfficeId(int officeId);
    }
    public class DiscDealingOfficerService : IDiscDealingOfficerService
    {
        private readonly IDiscDealingOfficerRepository repository;
        private readonly IEmployeeRepository empRepository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public DiscDealingOfficerService(IDiscDealingOfficerRepository repository, IUnitOfWorkCodeFirst unitOfWork, IEmployeeRepository empRepository)
        {
            this.repository = repository;
            this.empRepository = empRepository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<DiscDealingOfficer> GetAll()
        {
            var entities = repository.GetAll().Where(s => s.IsActive == true).OrderBy(c => c.DealOfficerId);
            return entities;
        }
        public IEnumerable<Employee> GetEmployeeByOfficeId(int officeId)
        {
            var deal = repository.GetMany(s => s.IsActive == true).Select(S => S.EmployeeId).Distinct();
            var entities = empRepository.GetMany(w => w.IsActive == true && deal.Contains(w.EmployeeId));
            return entities;
        }

        public DiscDealingOfficer GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public DiscDealingOfficer GetByEmployeeIdAndOfficeId(Int64 EmployeeId, int OfficeId)
        {
            var entity = repository.Get(g => g.EmployeeId == EmployeeId && g.OfficeId == OfficeId && g.IsActive == true);
            return entity;
        }

        public DiscDealingOfficer Create(DiscDealingOfficer objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(DiscDealingOfficer objectToUpdate)
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

        public IEnumerable<DiscDealingOfficer> GetMany(Expression<Func<DiscDealingOfficer, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }

        public DiscDealingOfficer Get(Expression<Func<DiscDealingOfficer, bool>> where)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<DiscDealingOfficer>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<DiscDealingOfficer>> GetManyAsync(Expression<Func<DiscDealingOfficer, bool>> where)
        {
            throw new NotImplementedException();
        }

        public Task<DiscDealingOfficer> GetAsync(Expression<Func<DiscDealingOfficer, bool>> where)
        {
            throw new NotImplementedException();
        }
    }
}
