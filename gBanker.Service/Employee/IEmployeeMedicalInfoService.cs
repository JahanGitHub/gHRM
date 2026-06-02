
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
    public interface IEmployeeMedicalInfoService : IServiceBase<EmployeeMedicalInfo>
    {
        //EmployeeOtherQualification GetByQualificationId(Int64 QualificationId);
        IEnumerable<EmployeeMedicalInfo> GetByEmployeeId(Int64 EmployeeId);

        //EmployeeEducation GetByEducationId(Int64 educationId);
    }
    public class EmployeeMedicalInfoService : IEmployeeMedicalInfoService
    {
        private readonly IEmployeeMedicalInfoRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public EmployeeMedicalInfoService(IEmployeeMedicalInfoRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<EmployeeMedicalInfo> GetAll()
        {
            var entities = repository.GetAll().Where(c => c.IsActive == true).OrderBy(c => c.MedicalInfoId);
            return entities;
        }

        public EmployeeMedicalInfo GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        //public EmployeeEducation GetByEducationId(Int64 educationId)
        //{
        //    var entity = repository.Get(e => e.EducationId == educationId && e.IsActive == true);
        //    return entity;
        //}

        public EmployeeMedicalInfo GetByEmpId(Int64 EmployeeId)
        {
            var entity = repository.Get(e => e.EmployeeId == EmployeeId);
            return entity;
        }

        public EmployeeMedicalInfo Create(EmployeeMedicalInfo objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }
        public IEnumerable<EmployeeMedicalInfo> GetByEmployeeId(Int64 EmployeeId)
        {
            var entity = repository.GetAll().Where(w => w.EmployeeId == EmployeeId && w.IsActive == true);
            return entity;
        }

        public void Update(EmployeeMedicalInfo objectToUpdate)
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

        public EmployeeMedicalInfo Get(Expression<Func<EmployeeMedicalInfo, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<EmployeeMedicalInfo> GetMany(Expression<Func<EmployeeMedicalInfo, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }

        #region Asyc
        public virtual async Task<IEnumerable<EmployeeMedicalInfo>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }


        public virtual async Task<IEnumerable<EmployeeMedicalInfo>> GetManyAsync(Expression<Func<EmployeeMedicalInfo, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }



        public virtual async Task<EmployeeMedicalInfo> GetAsync(Expression<Func<EmployeeMedicalInfo, bool>> where)
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
