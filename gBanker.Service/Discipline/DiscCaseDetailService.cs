using gHRM.Core.Common;
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.Discipline;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.DBDetailModels;
using gHRM.Data.Repository;
using gHRM.Data.Repository.Discipline;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Service.Discipline
{
    public interface IDiscCaseDetailService : IServiceBase<DiscCaseDetail>
    {
        IEnumerable<DiscCaseDetail> SaveCaseDetail(IEnumerable<DiscCaseDetail> CaseDetails);
        IEnumerable<ValidationResult> IsValidCaseDetail(int CrimeDetail);
        IEnumerable<DiscCaseDetail> GetAllByCaseMasterId(int CaseMasterId);

    }
    public class DiscCaseDetailService : IDiscCaseDetailService
    {
        private readonly IDiscCaseDetailRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public DiscCaseDetailService(IDiscCaseDetailRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        public IEnumerable<DiscCaseDetail> SaveCaseDetail(IEnumerable<DiscCaseDetail> CaseDetails)
        {

            if (CaseDetails != null && CaseDetails.Count() > 0)
            {
                foreach (var detail in CaseDetails)
                {
                    //var dbLoan = repository.GetById(loan.DailyLoanTrxID);
                    //if (dbLoan != null)
                    //{
                    //    dbLoan.LoanPaid = loan.LoanPaid;
                    //    dbLoan.IntPaid = loan.IntPaid;
                    //    dbLoan.TotalPaid = loan.TotalPaid;
                    //    repository.Update(dbLoan);
                    //}
                    Create(detail);
                }
            }
            //Save();
            return CaseDetails;
        }

        public IEnumerable<DiscCaseDetail> GetAll()
        {
            var entities = repository.GetAll().Where(c => c.IsActive == true).OrderBy(c => c.CaseDetailsId);
            return entities;
        }

        public IEnumerable<DiscCaseDetail> GetAllByCaseMasterId(int CaseMasterId)
        {
            var entities = repository.GetAll().Where(c => c.IsActive == true && c.CaseMasterId == CaseMasterId).OrderBy(c => c.CaseDetailsId);
            return entities;
        }
        public DiscCaseDetail GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }
        public DiscCaseDetail Create(DiscCaseDetail objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(DiscCaseDetail objectToUpdate)
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
            throw new NotImplementedException();
        }
        public bool IsContinued(long id)
        {
            throw new NotImplementedException();
        }
        IEnumerable<ValidationResult> IDiscCaseDetailService.IsValidCaseDetail(int CrimeDetail)
        {
            var entity = repository.Get(p => p.CaseDetailsId == CrimeDetail);
            if (entity != null)
            {
                yield return new ValidationResult("OrderId", "Duplicate OrderId Id.");

            }
        }

        public IEnumerable<DiscCaseDetail> GetMany(Expression<Func<DiscCaseDetail, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }

        public DiscCaseDetail Get(Expression<Func<DiscCaseDetail, bool>> where)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<DiscCaseDetail>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<DiscCaseDetail>> GetManyAsync(Expression<Func<DiscCaseDetail, bool>> where)
        {
            throw new NotImplementedException();
        }

        public Task<DiscCaseDetail> GetAsync(Expression<Func<DiscCaseDetail, bool>> where)
        {
            throw new NotImplementedException();
        }
    }
}
