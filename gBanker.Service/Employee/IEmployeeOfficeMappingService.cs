using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Service
{


    public interface IEmployeeOfficeMappingService : IServiceBase<EmployeeOfficeMapping>
    {

        //IEnumerable<EmployeeOfficeMapping> GetEmployeeOfficeMappings(string employeeCode);

        void CreateEmployeeOfficeMapping(string employeeCode, List<EmployeeOfficeMapping> mappings);

    }
    public class EmployeeOfficeMappingService : IEmployeeOfficeMappingService
    {
        private readonly IEmployeeOfficeMappingRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;
        public EmployeeOfficeMappingService(IEmployeeOfficeMappingRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;

        }


        //public IEnumerable<EmployeeOfficeMapping> GetEmployeeOfficeMappings(string employeeCode)
        //{
        //    return repository.GetEmployeeOfficeMappings(employeeCode);
        //}

        public IEnumerable<EmployeeOfficeMapping> GetAll()
        {
            throw new NotImplementedException();
        }

        public EmployeeOfficeMapping GetById(int id)
        {
            throw new NotImplementedException();
        }

        public EmployeeOfficeMapping Create(EmployeeOfficeMapping objectToCreate)
        {
            throw new NotImplementedException();
        }

        public void Update(EmployeeOfficeMapping objectToUpdate)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
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
            unitOfWork.Commit();
        }

        public EmployeeOfficeMapping Get(Expression<Func<EmployeeOfficeMapping, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<EmployeeOfficeMapping> GetMany(Expression<Func<EmployeeOfficeMapping, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }

        #region Asyc
        public virtual async Task<IEnumerable<EmployeeOfficeMapping>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }


        public virtual async Task<IEnumerable<EmployeeOfficeMapping>> GetManyAsync(Expression<Func<EmployeeOfficeMapping, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }



        public virtual async Task<EmployeeOfficeMapping> GetAsync(Expression<Func<EmployeeOfficeMapping, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion

        public void CreateEmployeeOfficeMapping(string employeeCode, List<EmployeeOfficeMapping> mappings)
        {
            repository.CreateEmployeeOfficeMapping(employeeCode, mappings);
            Save();
        }
    }
}
