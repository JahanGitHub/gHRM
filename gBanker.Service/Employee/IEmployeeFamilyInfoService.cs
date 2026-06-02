using gHRM.Core.Common;
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
    public interface IEmployeeFamilyInfoService : IServiceBase<EmployeeFamilyInfo>
    {
        IEnumerable<EmployeeFamilyInfo> GetByEmployeeId(Int64 EmployeeId);

        EmployeeFamilyInfo GetByFamilyInfoId(Int64 familyInfoId);
        EmployeeFamilyInfo GetDefaultEmployeeFamilyInfo(long employeeId);
    }
    public class EmployeeFamilyInfoService : IEmployeeFamilyInfoService
    {
        private readonly IEmployeeFamilyInfoRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public EmployeeFamilyInfoService(IEmployeeFamilyInfoRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<EmployeeFamilyInfo> GetAll()
        {
            var entities = repository.GetAll().Where(c => c.IsActive == true).OrderBy(c => c.FamilyInfoId);
            return entities;
        }

        public EmployeeFamilyInfo GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public EmployeeFamilyInfo GetByFamilyInfoId(Int64 familyInfoId)
        {
            var entity = repository.Get(e => e.FamilyInfoId == familyInfoId && e.IsActive == true);
            return entity;
        }

        public EmployeeFamilyInfo GetDefaultEmployeeFamilyInfo(long employeeId)
        {
            var single = new EmployeeFamilyInfo();
            using (var db = new gHRMDBContext())
            {
                single = db.EmployeeFamilyInfoes
                                .FirstOrDefault(f => 
                                        f.IsActive == true && 
                                        f.EmployeeId == employeeId);
            }

            return single;
        }

        public EmployeeFamilyInfo Create(EmployeeFamilyInfo objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }
        public IEnumerable<EmployeeFamilyInfo> GetByEmployeeId(Int64 EmployeeId)
        {
            var entity = repository.GetMany(w => w.EmployeeId == EmployeeId && w.IsActive == true);
            return entity;
        }

        public void Update(EmployeeFamilyInfo objectToUpdate)
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

        public EmployeeFamilyInfo Get(Expression<Func<EmployeeFamilyInfo, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<EmployeeFamilyInfo> GetMany(Expression<Func<EmployeeFamilyInfo, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }

        #region Asyc
        public virtual async Task<IEnumerable<EmployeeFamilyInfo>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }


        public virtual async Task<IEnumerable<EmployeeFamilyInfo>> GetManyAsync(Expression<Func<EmployeeFamilyInfo, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }



        public virtual async Task<EmployeeFamilyInfo> GetAsync(Expression<Func<EmployeeFamilyInfo, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion
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

    }
}
