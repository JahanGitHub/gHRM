using gHRM.Core.Utilities;
using gHRM.Core.Utilities.Constants;
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.DBDetailModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace gHRM.Data.Repository
{
    public interface ILeaveSellRepository : IRepository<LeaveSell>
    {
        BaseResponse UpdateLeaveSellAdviseStatus(string emmployeeCode, int leaveSellId, int status);
        LeaveSellAdviseInfoModel GetEmployeeWithELAdvise(string emmployeeCode);
        LeaveSellAdviseInfoModel GetManualLeaveSellForInactiveInfo(string emmployeeCode);
        bool HasEmployeeEverEncashedDays(long EmployeeId, int Days);
        bool HasEmployeeDoneManualLeaveSell(string EmployeeCode);
        bool WasManualLeaveSellForInactiveDoneForEmployee(long EmployeeId);
        void IfManualLeaveSellAllowManualAgain(int LeaveSellId);
        List<BulkLeaveEncashmentModel> GetBulkEncashmentData();
        void BulkEncash(List<long> ExcludedEmployeeIdList, long LoggedInEmployeeId, int LoginUserOfficeId);
    }

    public class LeaveSellRepository : RepositoryBaseCodeFirst<LeaveSell>, ILeaveSellRepository
    {
        public LeaveSellRepository(IDatabaseFactoryCodeFirst databaseFactory) : base(databaseFactory)
        {

        }

        public BaseResponse UpdateLeaveSellAdviseStatus(string emmployeeCode, int leaveSellId, int status)
        {
            
            try
            {
                var sqlCommand = $@"[leave].[LeaveSellAdvise_UpdateStatus] '{emmployeeCode}',{leaveSellId},{status}";
                var response = DataContext.Database.SqlQuery<BaseResponse>(sqlCommand).FirstOrDefault();

                return response;
            }
            catch 
            {
                return new BaseResponse { IsSuccess=false,Message="Error on updating Leave Sell advise"};
            }
        }

        public LeaveSellAdviseInfoModel GetEmployeeWithELAdvise(string emmployeeCode)
        {
            try
            {
                var sqlCommand = $@"[leave].[LeaveAdvise_GetEmployeeWithELAdvise] '{emmployeeCode}'";
                var single = DataContext.Database.SqlQuery<LeaveSellAdviseInfoModel>(sqlCommand).FirstOrDefault();

                return single;
            }
            catch(Exception ex)
            {
                return new LeaveSellAdviseInfoModel { };
            }
        }

        public LeaveSellAdviseInfoModel GetManualLeaveSellForInactiveInfo(string emmployeeCode)
        {
            try
            {
                var sqlCommand = "leave.SP_GetManualLeaveSellForInactiveInfo {0}";
                return DataContext.Database.SqlQuery<LeaveSellAdviseInfoModel>(sqlCommand, emmployeeCode).FirstOrDefault();
            }
            catch (Exception ex)
            {
                return new LeaveSellAdviseInfoModel { };
            }
        }

        public bool HasEmployeeEverEncashedDays(long EmployeeId, int Days)
        {
            return DataContext.LeaveSells.Where(x => x.IsActive && x.IsApproved && x.EmployeeId == EmployeeId && x.TotalDays == Days).Count() > 0;
        }

        public bool HasEmployeeDoneManualLeaveSell(string EmployeeCode)
        {
            string SQL = "SELECT COUNT(*) AS C FROM leave.LeaveSellAdvise WHERE IDNo = {0} AND LeaveSellStatus = 1";
            return (DataContext.Database.SqlQuery<int>(SQL, EmployeeCode).FirstOrDefault()) > 0;
        }

        public bool WasManualLeaveSellForInactiveDoneForEmployee(long EmployeeId)
        {
            return DataContext.LeaveSells.Where(x => x.EmployeeId == EmployeeId && x.IsManualLeaveSellForInactive).Count() > 0;
        }

        public void IfManualLeaveSellAllowManualAgain(int LeaveSellId)
        {
            string SQL = "" +
@"UPDATE leave.LeaveSellAdvise SET LeaveSellStatus = 0 WHERE LeaveSellId = {0}
SELECT 1";
            DataContext.Database.SqlQuery<int>(SQL, LeaveSellId).FirstOrDefault();
        }

        public List<BulkLeaveEncashmentModel> GetBulkEncashmentData()
        {
            try
            {
                var SQL = "leave.SP_BulkEncashmentData {0}, {1}";
                return DataContext.Database.SqlQuery<BulkLeaveEncashmentModel>(SQL, "General", EncashmentFormulaConstants.HalfIfLessThanMinimum).ToList();
            }
            catch (Exception ex)
            {
                return new List<BulkLeaveEncashmentModel>();
            }
        }

        public void BulkEncash(List<long> ExcludedEmployeeIdList, long LoggedInEmployeeId, int LoginUserOfficeId)
        {
            List<BulkLeaveEncashmentModel> DataList = GetBulkEncashmentData();

            foreach (BulkLeaveEncashmentModel DataItem in DataList)
            {
                if (ExcludedEmployeeIdList.Contains(DataItem.Id)) continue;
                LeaveSell _LeaveSell = new LeaveSell();
                _LeaveSell.EmployeeId = DataItem.Id;
                _LeaveSell.RequestDate = DateTime.Now;
                _LeaveSell.TotalDays = DataItem.Qty;
                _LeaveSell.EncashedAmount = DataItem.Amt;
                _LeaveSell.IsApproved = true;
                _LeaveSell.OrderCreateOfficeId = LoginUserOfficeId;
                _LeaveSell.IsAmountPaid = false;
                _LeaveSell.IsActive = true;
                _LeaveSell.CreateDate = DateTime.UtcNow;
                _LeaveSell.UpdateDate = DateTime.UtcNow;
                _LeaveSell.CreateUser = LoggedInEmployeeId;
                _LeaveSell.UpdateUser = LoggedInEmployeeId;
                _LeaveSell.IsBulkEncashed = true;
                DataContext.LeaveSells.Add(_LeaveSell);
            }
        }
    }
}
