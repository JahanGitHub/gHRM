using gHRM.Core.Common;
using gHRM.Core.Filters.Offices;
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.Apply;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.DBDetailModels;
using gHRM.Data.Repository;
using gHRM.Data.Repository.Apply;
using gHRM.Data.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;



namespace gHRM.Service.Apply
{
    public interface IApplicantAccademicService : IServiceBase<ApplicantAccademic>
    {
        
    }
    public class ApplicantAccademicService : IApplicantAccademicService
    {
        private readonly IApplicantAccademicRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public ApplicantAccademicService(IApplicantAccademicRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }


        public ApplicantAccademic Create(ApplicantAccademic objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }



        public void Delete(int id)
        {
            var entity = repository.GetById(id);
            repository.Delete(entity);
            Save();
        }

        public ApplicantAccademic Get(Expression<Func<ApplicantAccademic, bool>> where)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ApplicantAccademic> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ApplicantAccademic>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ApplicantAccademic> GetAsync(Expression<Func<ApplicantAccademic, bool>> where)
        {
            throw new NotImplementedException();
        }

        public ApplicantAccademic GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
            throw new NotImplementedException();
        }


        public IEnumerable<ApplicantAccademic> GetMany(Expression<Func<ApplicantAccademic, bool>> where)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ApplicantAccademic>> GetManyAsync(Expression<Func<ApplicantAccademic, bool>> where)
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
            //throw new NotImplementedException();
            unitOfWork.Commit();
        }

        public void Update(ApplicantAccademic objectToUpdate)
        {
            repository.Update(objectToUpdate);
            Save();
        }
    }
}
