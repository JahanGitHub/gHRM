using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.CodeFirstMigration.Payroll;
using gHRM.Data.DBDetailModels.Payroll;
using gHRM.Data.Repository.Payroll;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Service.Payroll
{
    public interface IGradeXSalaryStepService : IServiceBase<GradeXSalaryStep>
    { IEnumerable<GradeXSalaryStepViewModel> GetGradeXSalaryStepList(); }
    public class GradeXSalaryStepService : IGradeXSalaryStepService
    {
        private readonly IGradeXSalaryStepRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public GradeXSalaryStepService(IGradeXSalaryStepRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<GradeXSalaryStep> GetAll()
        {
            var entities = repository.GetMany(x => x.IsActive).OrderBy(c => c.Id);
            return entities;
        }

        public GradeXSalaryStep GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }



        public GradeXSalaryStep Create(GradeXSalaryStep objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(GradeXSalaryStep objectToUpdate)
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


        public GradeXSalaryStep Get(Expression<Func<GradeXSalaryStep, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<GradeXSalaryStep> GetMany(Expression<Func<GradeXSalaryStep, bool>> where)
        {
            var entities = repository.GetMany(where).Where(x=>x.IsActive);
            return entities;
        }
        public IEnumerable<GradeXSalaryStepViewModel> GetGradeXSalaryStepList()
        {
            try
            {
                using (gHRMDBContext db = new gHRMDBContext())
                {
                    return (from step in db.GradeXSalarySteps
                            join grade in db.EmployeeGradeList on step.GradeId equals grade.GradeId
                            where step.IsActive && grade.IsActive
                            select new GradeXSalaryStepViewModel
                            {
                                Id = step.Id,
                                GradeId = step.GradeId,
                                GradeName = grade.GradeName,
                                AmountOrPercent = step.AmountOrPercent,
                                RatioOn = step.RatioOn,
                                StepFrom = step.StepFrom,
                                StepTo = step.StepTo
                            }).OrderBy(x=>new { x.GradeName, x.StepFrom, x.StepTo }).ToList();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            
        }

        #region Asyc
        public virtual async Task<IEnumerable<GradeXSalaryStep>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }


        public virtual async Task<IEnumerable<GradeXSalaryStep>> GetManyAsync(Expression<Func<GradeXSalaryStep, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }



        public virtual async Task<GradeXSalaryStep> GetAsync(Expression<Func<GradeXSalaryStep, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion
    }
}
