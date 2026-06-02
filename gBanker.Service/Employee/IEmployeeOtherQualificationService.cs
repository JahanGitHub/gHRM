
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
    public interface IEmployeeOtherQualificationService : IServiceBase<EmployeeOtherQualification>
    {
        //EmployeeOtherQualification GetByQualificationId(Int64 QualificationId);
        IEnumerable<EmployeeOtherQualification> GetByEmployeeId(long EmployeeId);

        //EmployeeEducation GetByEducationId(Int64 educationId);
    }
    public class EmployeeOtherQualificationService : IEmployeeOtherQualificationService
    {
        private readonly IEmployeeOtherQualificationRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public EmployeeOtherQualificationService(IEmployeeOtherQualificationRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<EmployeeOtherQualification> GetAll()
        {
            var entities = repository.GetAll().Where(c => c.IsActive == true).OrderBy(c => c.QualificationId);
            return entities;
        }

        public EmployeeOtherQualification GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        //public EmployeeEducation GetByEducationId(Int64 educationId)
        //{
        //    var entity = repository.Get(e => e.EducationId == educationId && e.IsActive == true);
        //    return entity;
        //}

        public EmployeeOtherQualification GetByEmpId(long EmployeeId)
        {
            var entity = repository.Get(e => e.EmployeeId == EmployeeId);
            return entity;
        }

        public EmployeeOtherQualification Create(EmployeeOtherQualification objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }
        public IEnumerable<EmployeeOtherQualification> GetByEmployeeId(long EmployeeId)
        {
            var entity = repository.GetMany(w => w.EmployeeId == EmployeeId && w.IsActive == true);
            return entity;
        }

        public void Update(EmployeeOtherQualification objectToUpdate)
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

        public EmployeeOtherQualification Get(Expression<Func<EmployeeOtherQualification, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<EmployeeOtherQualification> GetMany(Expression<Func<EmployeeOtherQualification, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }

        #region Asyc
        public virtual async Task<IEnumerable<EmployeeOtherQualification>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }


        public virtual async Task<IEnumerable<EmployeeOtherQualification>> GetManyAsync(Expression<Func<EmployeeOtherQualification, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }



        public virtual async Task<EmployeeOtherQualification> GetAsync(Expression<Func<EmployeeOtherQualification, bool>> where)
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
