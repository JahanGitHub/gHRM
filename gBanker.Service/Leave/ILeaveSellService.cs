using gHRM.Core.Utilities;
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.DBDetailModels;
using gHRM.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace gHRM.Service
{
    public interface ILeaveSellService : IServiceBase<LeaveSell>
    {
        int GetTotSellEmpId(Int64 EmployeeId);
        DateTime? GetLastSellDateByEmpId(Int64 EmployeeId);
        LeaveSellAdviseInfoModel GetEmployeeWithELAdvise(string emmployeeCode);
        LeaveSellAdviseInfoModel GetManualLeaveSellForInactiveInfo(string emmployeeCode);
        BaseResponse UpdateLeaveSellAdviseStatus(string emmployeeCode, int leaveSellId, int status);
        bool HasEmployeeEverEncashedDays(long EmployeeId, int Days);
        bool HasEmployeeDoneManualLeaveSell(string EmployeeCode);
        bool WasManualLeaveSellForInactiveDoneForEmployee(long EmployeeId);
        void IfManualLeaveSellAllowManualAgain(int LeaveSellId);
        List<BulkLeaveEncashmentModel> GetBulkEncashmentData();
        void BulkEncash(List<long> ExcludedEmployeeIdList, long LoggedInEmployeeId, int LoginUserOfficeId);
    }

    public class LeaveSellService : ILeaveSellService
    {
        private readonly ILeaveSellRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public LeaveSellService(ILeaveSellRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        public LeaveSellAdviseInfoModel GetEmployeeWithELAdvise(string emmployeeCode)
        {
            try
            {               
                var single = repository.GetEmployeeWithELAdvise(emmployeeCode);

                return single;
            }
            catch
            {
                return new LeaveSellAdviseInfoModel { };
            }
        }

        public LeaveSellAdviseInfoModel GetManualLeaveSellForInactiveInfo(string emmployeeCode)
        {
            try
            {
                var single = repository.GetManualLeaveSellForInactiveInfo(emmployeeCode);

                return single;
            }
            catch
            {
                return new LeaveSellAdviseInfoModel { };
            }
        }

        public BaseResponse UpdateLeaveSellAdviseStatus(string emmployeeCode, int leaveSellId, int status)
        {
            try
            {
                var response = repository.UpdateLeaveSellAdviseStatus(emmployeeCode, leaveSellId, status);

                return response;
            }
            catch
            {
                return new BaseResponse { IsSuccess = false, Message = "Error on updating Leave Sell advise" };
            }
        }

        public bool HasEmployeeEverEncashedDays(long EmployeeId, int Days)
        {
            return repository.HasEmployeeEverEncashedDays(EmployeeId, Days);
        }

        public bool HasEmployeeDoneManualLeaveSell(string EmployeeCode)
        {
            return repository.HasEmployeeDoneManualLeaveSell(EmployeeCode);
        }

        public bool WasManualLeaveSellForInactiveDoneForEmployee(long EmployeeId)
        {
            return repository.WasManualLeaveSellForInactiveDoneForEmployee(EmployeeId);
        }

        public void IfManualLeaveSellAllowManualAgain(int LeaveSellId)
        {
            repository.IfManualLeaveSellAllowManualAgain(LeaveSellId);
        }

        public List<BulkLeaveEncashmentModel> GetBulkEncashmentData()
        {
            return repository.GetBulkEncashmentData();
        }

        public void BulkEncash(List<long> ExcludedEmployeeIdList, long LoggedInEmployeeId, int LoginUserOfficeId)
        {
            repository.BulkEncash(ExcludedEmployeeIdList, LoggedInEmployeeId, LoginUserOfficeId);
        }

        public IEnumerable<LeaveSell> GetAll()
        {
            var entities = repository.GetAll().Where(c => c.IsActive == true).OrderBy(c => c.LeaveSellId);
            return entities;
        }

        public LeaveSell GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public DateTime? GetLastSellDateByEmpId(Int64 EmployeeId)
        {
            DateTime? dt = null;
            var entity = repository.GetAll().Where(w => w.IsActive == true && w.IsApproved == true && w.EmployeeId == EmployeeId).Max(m => m.SaleDate);
            if (entity != null)
                dt = entity.Value;
            return dt;
        }

        public int GetTotSellEmpId(Int64 EmployeeId)
        {
            int Totdays = 0;
            var entity = repository.GetMany(w => w.IsActive == true && w.IsApproved == true && w.EmployeeId == EmployeeId).Sum(m => m.TotalDays);
            Totdays = entity;
            return Totdays;
        }

        public LeaveSell Create(LeaveSell objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(LeaveSell objectToUpdate)
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

        public LeaveSell Get(Expression<Func<LeaveSell, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<LeaveSell> GetMany(Expression<Func<LeaveSell, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }

        #region Asyc
        public virtual async Task<IEnumerable<LeaveSell>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }


        public virtual async Task<IEnumerable<LeaveSell>> GetManyAsync(Expression<Func<LeaveSell, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }



        public virtual async Task<LeaveSell> GetAsync(Expression<Func<LeaveSell, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion
    }
}
