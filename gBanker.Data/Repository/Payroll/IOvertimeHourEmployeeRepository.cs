using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.CodeFirstMigration.Payroll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.Repository.payroll
{
    public interface IOvertimeHourEmployeeRepository : IRepository<OvertimeHourEmployee>
    {
        List<OvertimeHourEmployee> AddEmployeeOvertimeList(List<OvertimeHourEmployee> objs);

    }

    public class OvertimeHourEmployeeRepository : RepositoryBaseCodeFirst<OvertimeHourEmployee>, IOvertimeHourEmployeeRepository
    {
        public OvertimeHourEmployeeRepository(IDatabaseFactoryCodeFirst databaseFactory) : base(databaseFactory)
        {

        }

        public List<OvertimeHourEmployee> AddEmployeeOvertimeList(List<OvertimeHourEmployee> objs)
        {
            DataContext.OvertimeHourEmployee.AddRange(objs);
            //objs.ForEach(p => DataContext.Entry(p).State = EntityState.Modified);
            //DataContext.Entry(objs).State = System.Data.Entity.EntityState.Added;
            DataContext.SaveChanges();
            return objs;
        }
    }
}
