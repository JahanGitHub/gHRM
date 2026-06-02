using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.DBDetailModels;
using System.Collections.Generic;
using System.Linq;

namespace gHRM.Data.Repository
{
    public interface IEmployeeDesignationRepository : IRepository<EmployeeDesignation>
    {
        IEnumerable<DBEmployeeDesignationDetailModel> GetDesignationDetail(string filterColumnName, string filterValue, int startRowIndex, string jtSorting, int pageSize, out long TotCount);
        List<EmployeeDesignation> AddDesignationList(List<EmployeeDesignation> objs);
    }
    public class EmployeeDesignationRepository : RepositoryBaseCodeFirst<gHRM.Data.CodeFirstMigration.EmployeeDesignation>, IEmployeeDesignationRepository
    {
        public EmployeeDesignationRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
        public IEnumerable<DBEmployeeDesignationDetailModel> GetDesignationDetail(string filterColumnName, string filterValue, int startRowIndex, string jtSorting, int pageSize, out long TotCount)
        {
            IQueryable<EmployeeDesignation> results = null;
            if (filterColumnName == "DesignationName")
                results = DataContext.EmployeeDesignations.Where(x => x.IsActive == true && x.DesignationName.Contains(filterValue));
            else if (filterColumnName == "DesignationCode")
                results = DataContext.EmployeeDesignations.Where(x => x.IsActive == true && x.DesignationCode.Contains(filterValue));
            else if (filterColumnName == "DesignationType")
                results = DataContext.EmployeeDesignations.Where(x => x.IsActive == true && x.DesignationType.Contains(filterValue));
            else
                results = DataContext.EmployeeDesignations.Where(x=> x.IsActive == true);

            TotCount = results.LongCount();

            var obj = results.OrderBy(o => o.DesignationId).Skip(startRowIndex).Take(pageSize).Select(s => new DBEmployeeDesignationDetailModel()
            {
          
                DesignationId = s.DesignationId,
                DesignationCode = s.DesignationCode,
                DesignationName = s.DesignationName,
                DesignationShortName = s.DesignationShortName,
                DesignationType = s.DesignationType == "" ? "" : s.DesignationType == "RD" ? "Regular Designation" : "Equivalent Designation",
                SalaryScaleId = s.SalaryScaleId,
                // DataFilter               
            });

            if (!string.IsNullOrWhiteSpace(jtSorting))
            {
                if (jtSorting == "DesignationId ASC")
                    return obj.OrderBy(o => o.DesignationId);
                else if (jtSorting == "DesignationId DESC")
                    return obj.OrderByDescending(o => o.DesignationId);
                else if (jtSorting == "DesignationCode ASC")
                    return obj.OrderBy(o => o.DesignationCode);
                else if (jtSorting == "DesignationCode DESC")
                    return obj.OrderByDescending(o => o.DesignationCode);
                else if (jtSorting == "DesignationName ASC")                       //DataSorting
                    return obj.OrderBy(o => o.DesignationName);
                else if (jtSorting == "DesignationName DESC")
                    return obj.OrderByDescending(o => o.DesignationName);
                else if (jtSorting == "DesignationShortName ASC")                                           //DataSorting
                    return obj.OrderBy(o => o.DesignationShortName);
                else if (jtSorting == "DesignationShortName DESC")
                    return obj.OrderByDescending(o => o.DesignationShortName);
                else if (jtSorting == "DesignationType ASC")
                    return obj.OrderBy(o => o.DesignationType);
                else if (jtSorting == "DesignationType DESC")
                    return obj.OrderByDescending(o => o.DesignationType);
                else if (jtSorting == "SalaryScaleId ASC")
                    return obj.OrderBy(o => o.SalaryScaleId);
                else if (jtSorting == "SalaryScaleId DESC")
                    return obj.OrderByDescending(o => o.SalaryScaleId);

                else
                    return obj.OrderBy(o => o.DesignationId);
            }
            else
                return obj.OrderBy(o => o.DesignationId);
        }
        public List<EmployeeDesignation> AddDesignationList(List<EmployeeDesignation> objs)
        {
            DataContext.EmployeeDesignations.AddRange(objs);
            DataContext.SaveChanges();
            return objs;
        }
    }

}
