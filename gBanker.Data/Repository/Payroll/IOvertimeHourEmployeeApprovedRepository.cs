
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
    public interface IOvertimeHourEmployeeApprovedRepository : IRepository<OvertimeHourEmployeeApproved>
    {
        List<OvertimeHourEmployeeApproved> AddEmployeeOvertimeApprovedList(List<OvertimeHourEmployeeApproved> objs);

    }



    public class OvertimeHourEmployeeApprovedRepository : RepositoryBaseCodeFirst<OvertimeHourEmployeeApproved>, IOvertimeHourEmployeeApprovedRepository
    {
        public OvertimeHourEmployeeApprovedRepository(IDatabaseFactoryCodeFirst databaseFactory) : base(databaseFactory)
        {

        }

        public List<OvertimeHourEmployeeApproved> AddEmployeeOvertimeApprovedList(List<OvertimeHourEmployeeApproved> objs)
        {
            DataContext.OvertimeHourEmployeeApproved.AddRange(objs);
            //objs.ForEach(p => DataContext.Entry(p).State = EntityState.Modified);
            //DataContext.Entry(objs).State = System.Data.Entity.EntityState.Added;
            DataContext.SaveChanges();
            return objs;
        }
    }
}
