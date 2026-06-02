using gHRM.Core.Utilities;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.CodeFirstMigration.Payroll;
using gHRM.Data.DBDetailModels.Payroll;
using gHRM.Data.Repository.Payroll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Service.Payroll
{
    public interface ILoanInstallmentDetailService : IServiceBase<LoanInstallmentDetail>
    {
        Task<BaseResponse> UpdatePreviousLoanAsClosed(UpdatePreviousLoanAsClosedModel model);
        List<LoanInstallmentDetail> AddLoanExcel(List<LoanInstallmentDetail> objs);//

    }
    public class LoanInstallmentDetailService : ILoanInstallmentDetailService
    {
        private readonly ILoanInstallmentDetailRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public List<LoanInstallmentDetail> AddLoanExcel(List<LoanInstallmentDetail> objs)
        {
            repository.AddLoanExcel(objs);
            return objs;
        }
        public LoanInstallmentDetailService(ILoanInstallmentDetailRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        public IEnumerable<LoanInstallmentDetail> GetAll()
        {
            var entities = repository.GetAll().Where(c => c.IsActive == true).OrderBy(c => c.Id);
            return entities;
        }

        public LoanInstallmentDetail GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public async Task<BaseResponse> UpdatePreviousLoanAsClosed(UpdatePreviousLoanAsClosedModel model)
        {
            return await repository.UpdatePreviousLoanAsClosed(model);            
        }

        public LoanInstallmentDetail Create(LoanInstallmentDetail objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(LoanInstallmentDetail objectToUpdate)
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

        public LoanInstallmentDetail Get(Expression<Func<LoanInstallmentDetail, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<LoanInstallmentDetail> GetMany(Expression<Func<LoanInstallmentDetail, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }

        #region Asyc
        public virtual async Task<IEnumerable<LoanInstallmentDetail>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }


        public virtual async Task<IEnumerable<LoanInstallmentDetail>> GetManyAsync(Expression<Func<LoanInstallmentDetail, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }



        public virtual async Task<LoanInstallmentDetail> GetAsync(Expression<Func<LoanInstallmentDetail, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion
    }
}
