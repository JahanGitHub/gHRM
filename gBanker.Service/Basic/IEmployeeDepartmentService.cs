using gHRM.Core.Common;
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.DBDetailModels;
using gHRM.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Service
{

    public interface IEmployeeDepartmentService : IServiceBase<EmployeeDepartment>
    {
        IEnumerable<ValidationResult> IsValidDepartment(int employeeDepartmentId);
        IEnumerable<DBEmployeeDepartmentDetailModel> GetDepartmentDetail(string filterColumnName, string filterValue, int startRowIndex, string jtSorting, int pageSize, out long TotCount);
        IEnumerable<EmployeeDepartment> getEmployeeDepartment(int? OfficeType);
        //bool Inactivate(int id);

        List<EmployeeDepartment> AddDepartmentList(List<EmployeeDepartment> objs);
    }
    public class EmployeeDepartmentService : IEmployeeDepartmentService
    {
        private readonly IEmployeeDepartmentRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public EmployeeDepartmentService(IEmployeeDepartmentRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<EmployeeDepartment> GetAll()
        {
            var entities = repository.GetAll().Where(c => c.IsActive == true).OrderBy(c => c.DepartmentId);
            return entities;
        }

        public EmployeeDepartment GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public EmployeeDepartment Create(EmployeeDepartment objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(EmployeeDepartment objectToUpdate)
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

        public EmployeeDepartment Get(Expression<Func<EmployeeDepartment, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<EmployeeDepartment> GetMany(Expression<Func<EmployeeDepartment, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }

        #region Asyc
        public virtual async Task<IEnumerable<EmployeeDepartment>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }


        public virtual async Task<IEnumerable<EmployeeDepartment>> GetManyAsync(Expression<Func<EmployeeDepartment, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }



        public virtual async Task<EmployeeDepartment> GetAsync(Expression<Func<EmployeeDepartment, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion
        public bool Inactivate(long id, DateTime? inactiveDate)
        {
            throw new NotImplementedException();
        }
        public bool IsContinued(long id)
        {
            throw new NotImplementedException();
        }
        IEnumerable<ValidationResult> IEmployeeDepartmentService.IsValidDepartment(int employeeDepartmentId)
        {
            var entity = repository.Get(p => p.DepartmentId == employeeDepartmentId);
            if (entity != null)
            {
                yield return new ValidationResult("DeparmentId", "Duplicate Department Id.");

            }
        }
        public IEnumerable<EmployeeDepartment> getEmployeeDepartment(int? OfficeType)
        {
         var list = repository.GetAll().Where(c => c.IsActive == true).OrderBy(o => o.DepartmentName);
            
            if (OfficeType.HasValue)
            {
                int _officeType = Convert.ToInt32(OfficeType);
             var  _list = list.Where(b => b.OfficeTypeId == _officeType);
             return _list;
            }
            return list;
        }
        public IEnumerable<DBEmployeeDepartmentDetailModel> GetDepartmentDetail(string filterColumnName, string filterValue, int startRowIndex, string jtSorting, int pageSize, out long TotCount)
        {
            return repository.GetDepartmentDetail(filterColumnName, filterValue, startRowIndex, jtSorting, pageSize, out TotCount);
        }

        public List<EmployeeDepartment> AddDepartmentList(List<EmployeeDepartment> objs)
        {
            repository.AddDepartmentList(objs);
            return objs;
        }

    }
}
