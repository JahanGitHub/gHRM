using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.DBDetailModels;
using System.Collections.Generic;
using System.Linq;

namespace gHRM.Data.Repository
{
    public interface IEmployeeOfficeDesignationRepository : IRepository<EmployeeOfficeDesignation>
    {
        IEnumerable<DBEmployeeOfficeDesignationDetails> GetEmployeeOfficeDesignationDetails(int startRowIndex, string jtSorting, int pageSize, out long TotCount);
    }
    public class EmployeeOfficeDesignationRepository : RepositoryBaseCodeFirst<EmployeeOfficeDesignation>, IEmployeeOfficeDesignationRepository
    {
        public EmployeeOfficeDesignationRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
        public IEnumerable<DBEmployeeOfficeDesignationDetails> GetEmployeeOfficeDesignationDetails( int startRowIndex, string jtSorting, int pageSize, out long TotCount)
        {
            IQueryable<EmployeeOfficeDesignation> results = null;
                results = DataContext.EmployeeOfficeDesignations.Where(x => x.IsActive == true && x.EndDate == null);

            TotCount = results.LongCount();

            var obj = results.OrderBy(o => o.SartDate).Skip(startRowIndex).Take(pageSize).Select(s => new DBEmployeeOfficeDesignationDetails()
            {
                EmpOfficeDesigId = s.EmpOfficeDesigId,
                EmployeeId = s.EmployeeId,
                EmployeeName = s.employee.EmployeeName,
                OfficeDesignationId = s.OfficeDesignationId,
                OfficeDesignationName = s.officeDesignation.OffcDesignName,
                SartDate = s.SartDate.ToString(),
                EndDate = s.EndDate.ToString(),
                Duration = s.Duration,
            });

            if (!string.IsNullOrWhiteSpace(jtSorting))
            {
                if (jtSorting == "EmpOfficeDesigId ASC")
                    return obj.OrderBy(o => o.EmpOfficeDesigId);
                else if (jtSorting == "EmpOfficeDesigId DESC")
                    return obj.OrderByDescending(o => o.EmpOfficeDesigId);
                else if (jtSorting == "EmployeeId ASC")
                    return obj.OrderBy(o => o.EmployeeId);
                else if (jtSorting == "EmployeeId DESC")
                    return obj.OrderByDescending(o => o.EmployeeId);
                else if (jtSorting == "OfficeDesignationId ASC")
                    return obj.OrderBy(o => o.OfficeDesignationId);
                else if (jtSorting == "OfficeDesignationId DESC")
                    return obj.OrderByDescending(o => o.OfficeDesignationId);
                else
                    return obj.OrderBy(o => o.EmpOfficeDesigId);
            }
            else
                return obj.OrderBy(o => o.EmpOfficeDesigId);
        }
    }
}
