
using System;
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using System.Collections.Generic;
using gHRM.Data.CodeFirstMigration.Payroll;

namespace gHRM.Data.Repository.payroll
{
    public interface IPRSalaryRegisterRepository : IRepository<PRSalaryRegister>
    {
        List<PRSalaryRegister> AddEmployeeMonthlySalaryRegister(List<PRSalaryRegister> objs);
    }
    public class PRSalaryRegisterRepository : RepositoryBaseCodeFirst<PRSalaryRegister>, IPRSalaryRegisterRepository
    {
        public PRSalaryRegisterRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }

        public List<PRSalaryRegister> AddEmployeeMonthlySalaryRegister(List<PRSalaryRegister> objs)
        {
            DataContext.PRSalaryRegister.AddRange(objs);
            DataContext.SaveChanges();
            return objs;
        }
    }

  
}
