using gHRM.Core.Common;
using gHRM.Core.Utilities.Constants;
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace gHRM.Service
{
    public interface IEmployeeAddressService : IServiceBase<EmployeeAddress>
    {

        EmployeeAddress GetDefaultEmployeeAddress(long employeeId);

        IEnumerable<EmployeeAddress> GetByEmployeeId(Int64 EmployeeId);
        EmployeeAddress GetByAddressId(Int64 addressId);
        EmployeeAddress GetPresentAddressByEmpId(Int64 employeeId);
    }
    public class EmployeeAddressService : IEmployeeAddressService
    {
        private readonly IEmployeeAddressRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public EmployeeAddressService(IEmployeeAddressRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<EmployeeAddress> GetAll()
        {
            var entities = repository.GetAll().Where(w => w.IsActive == true).OrderBy(c => c.AddressId);
            return entities;
        }

        public EmployeeAddress GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public EmployeeAddress GetByAddressId(Int64 addressId)
        {
            var entity = repository.Get(e => e.AddressId == addressId && e.IsActive == true);
            return entity;
        }
        public EmployeeAddress GetPresentAddressByEmpId(Int64 employeeId)
        {
            var entity = repository.Get(e => e.EmployeeId == employeeId && e.IsActive == true && e.AddressType == "Pr");
            return entity;
        }
        public IEnumerable<EmployeeAddress> GetByEmployeeId(Int64 EmployeeId)
        {
            var entity = repository.GetMany(w => w.EmployeeId == EmployeeId && w.IsActive == true);
            return entity;
        }
        public EmployeeAddress Create(EmployeeAddress objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(EmployeeAddress objectToUpdate)
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


        public EmployeeAddress Get(Expression<Func<EmployeeAddress, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<EmployeeAddress> GetMany(Expression<Func<EmployeeAddress, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }

        public EmployeeAddress GetDefaultEmployeeAddress(long employeeId)
        {
            var single = new EmployeeAddress();
            using (var db = new gHRMDBContext())
            {                
                single = db.EmployeeAddresses
                    .Include(f=>f.Country)
                    .Include(f => f.StateOrProvince)
                    .FirstOrDefault(f =>
                                        f.IsActive == true && f.EmployeeId == employeeId &&
                                        f.AddressType == AddressTypeConstants.PermanentAddress);

                if (single == null)
                {
                    single = db.EmployeeAddresses
                        .Include(f => f.Country)
                        .Include(f => f.StateOrProvince)
                        .FirstOrDefault(f =>
                                        f.IsActive == true && f.EmployeeId == employeeId &&
                                        f.AddressType == AddressTypeConstants.PresentAddress);
                }
            }

            return single;
        }

        #region Asyc
        public virtual async Task<IEnumerable<EmployeeAddress>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }


        public virtual async Task<IEnumerable<EmployeeAddress>> GetManyAsync(Expression<Func<EmployeeAddress, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }



        public virtual async Task<EmployeeAddress> GetAsync(Expression<Func<EmployeeAddress, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion
        //public IEnumerable<Employee> SearchEmployee()
        //{
        //    return repository.GetMany(g => g.IsActive == true).OrderBy(g => g.EmployeeId);
        //}


        public bool Inactivate(long id, DateTime? inactiveDate)
        {
            var obj = repository.GetById(id);
            if (obj != null)
            {
                obj.InActiveDate = inactiveDate.HasValue ? inactiveDate : DateTime.Now;
                obj.IsActive = false;
                repository.Update(obj);
                Save();
                return true;
            }
            return false;
        }


        public bool IsContinued(long id)
        {
            var obj = repository.GetById(id);
            if (obj != null)
            {
                var isActive = obj.IsActive;
                if (isActive == false)
                {
                    return false;
                }
            }

            return true;
        }



        //IEnumerable<ValidationResult> IEmployeeService.IsValidEmployee(Employee employee)
        //{
        //    var entity = repository.Get(p => p.EmployeeCode == employee.EmployeeCode);
        //    if (entity != null)
        //    {

        //        yield return new ValidationResult("EmployeeCode", "Duplicate Employee.");

        //    }
        //}
    }
}
