using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.Repository
{
    public interface IEmployeeOfficeMappingRepository : IRepository<EmployeeOfficeMapping>
    {
        //IEnumerable<EmployeeOfficeMapping> GetEmployeeOfficeMappings(string employeeCode);
        void CreateEmployeeOfficeMapping(string employeeCode, List<EmployeeOfficeMapping> mappings);

    }
    public class EmployeeOfficeMappingRepository : RepositoryBaseCodeFirst<EmployeeOfficeMapping>, IEmployeeOfficeMappingRepository
    {
        public EmployeeOfficeMappingRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }

        //public IEnumerable<EmployeeOfficeMapping> GetEmployeeOfficeMappings(string employeeCode)
        //{
        //    var mappings = DataContext.EmployeeOfficeMappings.Where(w => w.Employee.EmployeeCode == employeeCode && w.IsActive == true && w.Employee.IsActive == true);
        //    return mappings;
        //}

        public void CreateEmployeeOfficeMapping(string employeeCode, List<EmployeeOfficeMapping> mappings)
        {
            var employee = DataContext.Employees.Where(w => w.EmployeeCode == employeeCode).FirstOrDefault();
            if (employee != null)
            {
                foreach (var map in mappings)
                {
                    var existingMapping = DataContext.EmployeeOfficeMappings.Where(e => e.EmployeeId == employee.EmployeeId && e.OfficeID == map.OfficeID).FirstOrDefault();
                    if (existingMapping != null )
                    {
                        existingMapping.IsActive = map.IsActive;
                        existingMapping.CreateUser = map.CreateUser;
                        Update(existingMapping);
                    }
                    else
                    {
                        map.EmployeeId = 1;//employee.EmployeeId;
                        map.IsActive = true;

                        Add(map);
                    }
                }
            }
        }
    }
}
