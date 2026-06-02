using gHRM.Data;
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using gHRM.Data.CodeFirstMigration.Payroll;
using gHRM.Data.Repository.payroll;

namespace gHRM.Service.payroll
{


    public interface IOvertimeHourEmployeeApprovedService : IServiceBase<OvertimeHourEmployeeApproved>
    {
        List<OvertimeHourEmployeeApproved> AddEmployeeOvertimeApprovedList(List<OvertimeHourEmployeeApproved> objs);
        List<OvertimeHourEmployeeApproved> GetOvertimeHourEmployeeApprovedByYearAndMonth(int year, int month);
    }

    public class OvertimeHourEmployeeApprovedService : IOvertimeHourEmployeeApprovedService
    {
        private readonly IOvertimeHourEmployeeApprovedRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public OvertimeHourEmployeeApprovedService(IOvertimeHourEmployeeApprovedRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        public IEnumerable<OvertimeHourEmployeeApproved> GetAll()
        {
            //var entities = repository.GetAll().Where(c => c.IsActive == 1).OrderBy(c => c.Id);
            var entities = repository.GetAll();
            return entities;
        }

        public OvertimeHourEmployeeApproved GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public OvertimeHourEmployeeApproved Create(OvertimeHourEmployeeApproved objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(OvertimeHourEmployeeApproved objectToUpdate)
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

        public List<OvertimeHourEmployeeApproved> AddEmployeeOvertimeApprovedList(List<OvertimeHourEmployeeApproved> objs)
        {
            repository.AddEmployeeOvertimeApprovedList(objs);
            return objs;
        }

        public OvertimeHourEmployeeApproved Get(Expression<Func<OvertimeHourEmployeeApproved, bool>> where)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<OvertimeHourEmployeeApproved> GetMany(Expression<Func<OvertimeHourEmployeeApproved, bool>> where)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<OvertimeHourEmployeeApproved>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<OvertimeHourEmployeeApproved>> GetManyAsync(Expression<Func<OvertimeHourEmployeeApproved, bool>> where)
        {
            throw new NotImplementedException();
        }

        public Task<OvertimeHourEmployeeApproved> GetAsync(Expression<Func<OvertimeHourEmployeeApproved, bool>> where)
        {
            throw new NotImplementedException();
        }

        public List<OvertimeHourEmployeeApproved> GetOvertimeHourEmployeeApprovedByYearAndMonth(int year, int month)
        {
            var listing = new List<OvertimeHourEmployeeApproved>();

            using (var db = new gHRMDBContext())
            {
                listing = db.OvertimeHourEmployeeApproved
                            .Where(p => p.Year == year
                                && p.Month == month
                                && p.IsActive == true
                            ).ToList();
            }
            return listing;
        }
    }
}

