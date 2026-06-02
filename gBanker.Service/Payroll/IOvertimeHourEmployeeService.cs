using gHRM.Core.Common;
using gHRM.Data;
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.CodeFirstMigration.Payroll;
using gHRM.Data.Repository;
using gHRM.Data.Repository.payroll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Service.payroll
{
    public interface IOvertimeHourEmployeeService : IServiceBase<OvertimeHourEmployee>
    {
        List<OvertimeHourEmployee> AddEmployeeOvertimeList(List<OvertimeHourEmployee> objs);
        List<OvertimeHourEmployee> GetOvertimeHourEmployeeByYearAndMonth(int year, int month);
    }

    public class OvertimeHourEmployeeService : IOvertimeHourEmployeeService
    {
        private readonly IOvertimeHourEmployeeRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public OvertimeHourEmployeeService(IOvertimeHourEmployeeRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        public IEnumerable<OvertimeHourEmployee> GetAll()
        {
            //var entities = repository.GetAll().Where(c => c.IsActive == 1).OrderBy(c => c.Id);
            var entities = repository.GetAll().OrderBy(c => c.Id);
            return entities;
        }

        public OvertimeHourEmployee GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public OvertimeHourEmployee Create(OvertimeHourEmployee objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(OvertimeHourEmployee objectToUpdate)
        {
            repository.Update(objectToUpdate);
            Save();
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

        public OvertimeHourEmployee Get(Expression<Func<OvertimeHourEmployee, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<OvertimeHourEmployee> GetMany(Expression<Func<OvertimeHourEmployee, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }

        public List<OvertimeHourEmployee> AddEmployeeOvertimeList(List<OvertimeHourEmployee> objs)
        {
            repository.AddEmployeeOvertimeList(objs);
            return objs;
        }

        public List<OvertimeHourEmployee> GetOvertimeHourEmployeeByYearAndMonth(int year, int month)
        {
            var listing = new List<OvertimeHourEmployee>();

            using (var db = new gHRMDBContext())
            {
                listing = db.OvertimeHourEmployee
                            .Where(p => p.Year == year 
                                && p.Month == month 
                                && p.IsActive==true && p.IsSendForApproval == true
                            ).ToList();
            }
            return listing;
        }



        #region Asyc
        public virtual async Task<IEnumerable<OvertimeHourEmployee>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }


        public virtual async Task<IEnumerable<OvertimeHourEmployee>> GetManyAsync(Expression<Func<OvertimeHourEmployee, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }



        public virtual async Task<OvertimeHourEmployee> GetAsync(Expression<Func<OvertimeHourEmployee, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion
    }
}
