using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.DBDetailModels;
using System.Collections.Generic;
using System.Linq;

namespace gHRM.Data.Repository
{
    public interface IEmployeeDepartmentRepository : IRepository<EmployeeDepartment>
    {
        //IEnumerable<Employee> GetEmployeeInfo(string filterColumnName, string filterValue, int startRowIndex, string jtSorting, int pageSize, out long TotCount);  
        IEnumerable<DBEmployeeDepartmentDetailModel> GetDepartmentDetail(string filterColumnName, string filterValue, int startRowIndex, string jtSorting, int pageSize, out long TotCount);
        List<EmployeeDepartment> AddDepartmentList(List<EmployeeDepartment> objs);
    }
    public class EmployeeDepartmentRepository : RepositoryBaseCodeFirst<EmployeeDepartment>, IEmployeeDepartmentRepository
    {
        public EmployeeDepartmentRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
        public IEnumerable<DBEmployeeDepartmentDetailModel> GetDepartmentDetail(string filterColumnName, string filterValue, int startRowIndex, string jtSorting, int pageSize, out long TotCount)
        {
            IQueryable<EmployeeDepartment> results = null;
            if (filterColumnName == "DepartmentCode")
                results = DataContext.EmployeeDepartments.Where(x => x.IsActive==true && x.DepartmentCode.Contains(filterValue));
            else if (filterColumnName == "DepartmentName")
                results = DataContext.EmployeeDepartments.Where(x => x.IsActive == true && x.DepartmentName.Contains(filterValue));
            else
                results = DataContext.EmployeeDepartments.Where(x=>x.IsActive==true);

            TotCount = results.LongCount();

            var obj = results.OrderBy(o => o.DepartmentId).Skip(startRowIndex).Take(pageSize).Select(s => new DBEmployeeDepartmentDetailModel()
            {
                DepartmentId = s.DepartmentId,
                OfficeTypeId = s.OfficeTypeId,
                OfficeTypeName = s.OfficeType.OfficeTypeName,
                DepartmentCode = s.DepartmentCode,
                DepartmentName = s.DepartmentName,
                DepartmentShortName = s.DepartmentShortName,
                CompanyId=s.CompanyId
                // DataFilter               
            });

            if (!string.IsNullOrWhiteSpace(jtSorting))
            {
                if (jtSorting == "DepartmentId ASC")
                    return obj.OrderBy(o => o.DepartmentId);
                else if (jtSorting == "DepartmentId DESC")
                    return obj.OrderByDescending(o => o.DepartmentId);
                else if (jtSorting == "DepartmentParentId ASC")
                    return obj.OrderBy(o => o.OfficeTypeId);
                else if (jtSorting == "DepartmentParentId DESC")
                    return obj.OrderByDescending(o => o.OfficeTypeId);
                else if (jtSorting == "DepartmentCode ASC")                       //DataSorting
                    return obj.OrderBy(o => o.DepartmentCode);
                else if (jtSorting == "DepartmentCode DESC")
                    return obj.OrderByDescending(o => o.DepartmentCode);
                else if (jtSorting == "DepartmentName ASC")                                           //DataSorting
                    return obj.OrderBy(o => o.DepartmentName);
                else if (jtSorting == "DepartmentName DESC")
                    return obj.OrderByDescending(o => o.DepartmentName);
                else if (jtSorting == "DepartmentShortName ASC")
                    return obj.OrderBy(o => o.DepartmentShortName);
                else if (jtSorting == "DepartmentShortName DESC")
                    return obj.OrderByDescending(o => o.DepartmentShortName);
                else if (jtSorting == "CompanyId ASC")
                    return obj.OrderBy(o => o.CompanyId);
                else if (jtSorting == "CompanyId DESC")
                    return obj.OrderByDescending(o => o.CompanyId);
                else
                    return obj.OrderBy(o => o.DepartmentId);
            }
            else
                return obj.OrderBy(o => o.DepartmentId);
        }
        public List<EmployeeDepartment> AddDepartmentList(List<EmployeeDepartment> objs)
        {
            DataContext.EmployeeDepartments.AddRange(objs);
            DataContext.SaveChanges();
            return objs;
        }
    }
}
