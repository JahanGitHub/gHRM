using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.CodeFirstMigration.Payroll;
using gHRM.Data.Repository.Payroll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Service.Payroll
{
    public interface IEmployeeLoanInstallmentDetailService : IServiceBase<EmployeeLoanInstallmentDetail>
    {
        Task<bool> IsExistRunningLoan(int employeeId);
        Task<bool> IsExistRunningLoan2(int employeeId, int prcomponentid);
        List<EmployeeLoanInstallmentDetail> AddEmployeeLoanInstallmentDetail(List<EmployeeLoanInstallmentDetail> objs); 
    }
    public class EmployeeLoanInstallmentDetailService : IEmployeeLoanInstallmentDetailService
    {
        private readonly IEmployeeLoanInstallmentDetailRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public EmployeeLoanInstallmentDetailService(IEmployeeLoanInstallmentDetailRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        public IEnumerable<EmployeeLoanInstallmentDetail> GetAll()
        {
            var entities = repository.GetAll().Where(c => c.IsActive == true).OrderBy(c => c.LoanDetailId);
            return entities;
        }

        public EmployeeLoanInstallmentDetail GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public EmployeeLoanInstallmentDetail Create(EmployeeLoanInstallmentDetail objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(EmployeeLoanInstallmentDetail objectToUpdate)
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

        public List<EmployeeLoanInstallmentDetail> AddEmployeeLoanInstallmentDetail(List<EmployeeLoanInstallmentDetail> objs)
        {
            repository.AddEmployeeLoanInstallmentDetail(objs);
            return objs;
        }

        public async Task<bool> IsExistRunningLoan(int employeeId)
        {
            return await repository.IsExistRunningLoan(employeeId);
        }

        public async Task<bool> IsExistRunningLoan2(int employeeId, int prcomponentid)
        {
            return await repository.IsExistRunningLoan2(employeeId,prcomponentid);
        }

        public EmployeeLoanInstallmentDetail Get(Expression<Func<EmployeeLoanInstallmentDetail, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<EmployeeLoanInstallmentDetail> GetMany(Expression<Func<EmployeeLoanInstallmentDetail, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }

        #region Asyc
        public virtual async Task<IEnumerable<EmployeeLoanInstallmentDetail>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }


        public virtual async Task<IEnumerable<EmployeeLoanInstallmentDetail>> GetManyAsync(Expression<Func<EmployeeLoanInstallmentDetail, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }



        public virtual async Task<EmployeeLoanInstallmentDetail> GetAsync(Expression<Func<EmployeeLoanInstallmentDetail, bool>> where)
        {
            return await repository.GetAsync(where);
        }

        #endregion
    }
}
