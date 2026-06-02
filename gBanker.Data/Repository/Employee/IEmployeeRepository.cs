using gHRM.Core.Filters.Employee;
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.DBDetailModels.Employee;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace gHRM.Data.Repository
{
    public interface IEmployeeRepository : IRepository<Employee>
    {
        Task<IEnumerable<FixedAssetEmployeeModel>> GetFixedAssetEmployeeByOffice(int officeId);
        Task<Employee> GetEmployeeInfo(int employeeId);
        Task<Employee> GetEmployeeInfoByUsername(string username);
        Task<IEnumerable<EmployeeDetailApiModel>> GetEmployeeListByFilter(EmployeeSearchFilter filter);
        DateTime? GetFirstJoiningDateByCode(string Code);
        decimal GetEmployeeBasicSalary(long EmployeeId);
        Dictionary<string, object> GetEmployeeShortInfoByCode(string EmployeeCode);
        bool IsActive(long EmployeeId);
    }

    public class EmployeeRepository : RepositoryBaseCodeFirst<gHRM.Data.CodeFirstMigration.Employee>, IEmployeeRepository
    {
        public EmployeeRepository(IDatabaseFactoryCodeFirst databaseFactory) : base(databaseFactory)
        {

        }

        public async Task<Employee> GetEmployeeInfo(int employeeId)
        {
            var single = new Employee();

            try
            {
                single = await DataContext.Employees.FirstOrDefaultAsync(f => f.EmployeeId == employeeId);

                return single;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<IEnumerable<FixedAssetEmployeeModel>> GetFixedAssetEmployeeByOffice(int officeId)
        {
            var employeeList = new List<FixedAssetEmployeeModel>();

            try
            {
                var sqlCommand = $@"
                    SELECT e.EmployeeId,e.EmployeeName,e.EmployeeCode 
                    FROM Employee e 
                    INNER JOIN EmployeeStatus es ON es.StatusId=e.EmployeeStatusId
                    WHERE e.IsActive=1 AND e.OfficeId={officeId} AND es.IsValid=1
                ";

                employeeList = await DataContext.Database.SqlQuery<FixedAssetEmployeeModel>(sqlCommand)
                    .ToListAsync();

                return employeeList;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<Employee> GetEmployeeInfoByUsername(string username)
        {
            var single = new Employee();

            try
            {
                var sqlCommand = $@"
                        SELECT EmployeeID
                        FROM [dbo].[AspNetUsers]
                        WHERE UserName='{username}'
                        ";
                var employeeId = await DataContext.Database.SqlQuery<Int64>(sqlCommand).FirstOrDefaultAsync();
                if (employeeId > 0)
                {
                    sqlCommand = $@"SELECT *FROM Employee WHERE EmployeeId={employeeId}";
                    single = await DataContext.Database.SqlQuery<Employee>(sqlCommand).FirstOrDefaultAsync();
                }
              
                return single;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<IEnumerable<EmployeeDetailApiModel>> GetEmployeeListByFilter(EmployeeSearchFilter filter)
        {
            var filteredList = new List<EmployeeDetailApiModel>();

            try
            {
                var officeId = filter.OfficeId > 0 ? filter.OfficeId.ToString() : "NULL";
                var filterOfficeCode = !string.IsNullOrEmpty(filter.OfficeCode) ? $"'{filter.OfficeCode}'" : "NULL";

                var employeeId = filter.EmployeeId > 0 ? filter.EmployeeId.ToString() : "NULL";
                var filterEmployeeCode = !string.IsNullOrEmpty(filter.EmployeeCode) ? $"'{filter.EmployeeCode}'" : "NULL";
                var roleId = filter.RoleId > 0 ? filter.RoleId.ToString() : "NULL";

                var sqlCommand = $@"[dbo].[Employee_GetEmployeesByFilter]                                
                                 {officeId},
                                 {filterOfficeCode},
                                 {employeeId},                               
                                 {filterEmployeeCode},                                
                                 {roleId},   
                                 {filter.PageNumber},
                                 {filter.PageSize },
                                '{filter.SortColumn }',
                                '{filter.SortDirection }'
                                ";

                filteredList = await DataContext.Database.SqlQuery<EmployeeDetailApiModel>(sqlCommand).ToListAsync();

                if (filteredList.Any())
                    filter.TotalCount = filteredList[0].TotalCount;
            }
            catch (Exception ex)
            {
                return new List<EmployeeDetailApiModel>();
            }

            return filteredList;
        }

        public DateTime? GetFirstJoiningDateByCode(string Code)
        {
            return DataContext.Employees.Where(x => x.EmployeeCode == Code && x.IsActive).Select(x => x.FirstJoiningDate).FirstOrDefault();
        }

        public decimal GetEmployeeBasicSalary(long EmployeeId)
        {
            decimal ComponentAmount = 0;
            string ComponentName = "Basic Salary";
            DateTime Today = DateTime.Now.Date;
            try
            {
                ComponentAmount = (from SC in DataContext.PRSalaryConfigurations
                    join C in DataContext.PRComponents on SC.PRComponentID equals C.PRComponentID
                    where C.ComponentName == ComponentName && C.IsActive && SC.IsActive
                    && SC.EffectiveStartDate <= Today && SC.EffectiveEndDate >= Today
                    && SC.EmployeeID == EmployeeId
                    select SC.ComponentAmount).FirstOrDefault();
            }
            catch { }
            return ComponentAmount;
        }

        public Dictionary<string, object> GetEmployeeShortInfoByCode(string EmployeeCode)
        {
            Dictionary<string, object> Info = new Dictionary<string, object>();
            var Data = (from E in DataContext.Employees
                     join O in DataContext.Offices on E.OfficeId equals O.OfficeId into c_cd_O
                     from O in c_cd_O.DefaultIfEmpty()
                     join D in DataContext.EmployeeDepartments on E.DepartmentId equals D.DepartmentId into c_cd_D
                     from D in c_cd_D.DefaultIfEmpty()
                     join EDG in DataContext.EmployeeDesignations on E.DesignationId equals EDG.DesignationId into c_cd_EDG
                     from EDG in c_cd_EDG.DefaultIfEmpty()
                     join ODG in DataContext.OfficeDesignations on E.EmployeeRank equals ODG.OfficeDesignationId.ToString() into c_cd_ODG
                     from ODG in c_cd_ODG.DefaultIfEmpty()
                     where E.IsActive && E.EmployeeCode == EmployeeCode
                     select new
                     {
                         E.EmployeeId,
                         E.EmployeeName,
                         O.OfficeName,
                         D.DepartmentName,
                         DesignationName = EDG.DesignationName,
                         ResponsibilityName = ODG.OffcDesignName
                     }).FirstOrDefault();
            Info["EmployeeId"] = null == Data ? 0 : Data.EmployeeId;
            Info["EmployeeName"] = null == Data ? "" : Data.EmployeeName;
            Info["OfficeName"] = null == Data ? "" : Data.OfficeName;
            Info["DepartmentName"] = null == Data ? "" : Data.DepartmentName;
            Info["DesignationName"] = null == Data ? "" : Data.DesignationName;
            Info["ResponsibilityName"] = null == Data ? "" : Data.ResponsibilityName;
            return Info;
        }

        public bool IsActive(long EmployeeId)
        {
            return (from S in DataContext.EmployeeStatus
                     join E in DataContext.Employees on S.StatusId equals E.EmployeeStatusId
                     where E.IsActive && S.IsActive && S.IsValid
                     && E.EmployeeId == EmployeeId
                    select E.EmployeeId).Count() > 0;
        }
    }
}
