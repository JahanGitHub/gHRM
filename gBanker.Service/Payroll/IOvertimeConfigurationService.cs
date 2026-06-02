using gHRM.Data.CodeFirstMigration;
using gHRM.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using gHRM.Data.Repository;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.Repository.payroll;
using gHRM.Data.CodeFirstMigration.Payroll;

public interface IOvertimeConfigurationService : IServiceBase<OvertimeConfiguration>
{
    OvertimeConfiguration LastOvertimeConfiguration();
    OvertimeConfiguration GetByRank(int rank);
    //IEnumerable<ValidationResult> IsValidEmployee(Employee employee);
    //IEnumerable<Employee> SearchEmployee();
    //IEnumerable<EmployeeAddress> GetByEmployeeId(Int64 EmployeeId);
    //EmployeeAddress GetByAddressId(Int64 addressId);
    //EmployeeAddress GetPresentAddressByEmpId(Int64 employeeId);
}


public class OvertimeConfigurationService : IOvertimeConfigurationService
{
    private readonly IOvertimeRepository repository;

    private readonly IUnitOfWorkCodeFirst unitOfWork;

    public OvertimeConfigurationService(IOvertimeRepository repository, IUnitOfWorkCodeFirst unitOfWork)
    {
        this.repository = repository;
        this.unitOfWork = unitOfWork;
    }

    public OvertimeConfiguration Create(OvertimeConfiguration objectToCreate)
    {
        repository.Add(objectToCreate);
        Save();
        return objectToCreate;
    }

    public void Update(OvertimeConfiguration objectToUpdate)
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

    //throw new NotImplementedException();


    public OvertimeConfiguration Get(Expression<Func<OvertimeConfiguration, bool>> where)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<OvertimeConfiguration> GetAll()
    {
        // var entities = repository.GetMany(g => g.IsActive == true).OrderBy(c => c.OfficeId);
        var entities = repository.GetAll().OrderBy(c => c.OvertimeConfigId);
        return entities;
    }

    public Task<IEnumerable<OvertimeConfiguration>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<OvertimeConfiguration> GetAsync(Expression<Func<OvertimeConfiguration, bool>> where)
    {
        throw new NotImplementedException();
    }

    public OvertimeConfiguration GetById(int id)
    {
        var entity = repository.GetById(id);
        return entity;
    }

    public OvertimeConfiguration LastOvertimeConfiguration()
    {
        var single = new OvertimeConfiguration();
        using (var db = new gHRMDBContext())
        {
            single = db.OvertimeConfiguration.OrderByDescending(f=>f.OvertimeConfigId).FirstOrDefault();
        }

        return single;
    }

    public OvertimeConfiguration GetByRank(int rank)
    {
        var single = new OvertimeConfiguration();
        using (var db = new gHRMDBContext())
        {
            single = db.OvertimeConfiguration.FirstOrDefault(f=>f.Rank==rank);
        }

        return single;
    }

    public IEnumerable<OvertimeConfiguration> GetMany(Expression<Func<OvertimeConfiguration, bool>> where)
    {
        // var entities = repository.GetMany(g => g.IsActive == true).OrderBy(c => c.OfficeId);
        // var entities = repository.GetAll().OrderBy(c => c.OvertimeConfigId);
        // return entities;
        throw new NotImplementedException();
    }

    public Task<IEnumerable<OvertimeConfiguration>> GetManyAsync(Expression<Func<OvertimeConfiguration, bool>> where)
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



}
