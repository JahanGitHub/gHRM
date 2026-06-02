using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.CodeFirstMigration.Payroll;

namespace gHRM.Data.Repository.Payroll
{
    public interface IEmployeeSalaryBonusRepository: IRepository<EmployeeSalaryBonus>
    {
        List<EmployeeSalaryBonus> AddEmployeeMonthlySalaryBonusList(List<EmployeeSalaryBonus> objs);
    }

    public class EmployeeSalaryBonusRepository : RepositoryBaseCodeFirst<EmployeeSalaryBonus>,
        IEmployeeSalaryBonusRepository
    {
        public EmployeeSalaryBonusRepository(IDatabaseFactoryCodeFirst databaseFactory) : base(databaseFactory)
        {
            
        }

        public List<EmployeeSalaryBonus> AddEmployeeMonthlySalaryBonusList(List<EmployeeSalaryBonus> objs)
        {
            DataContext.EmployeeSalaryBonus.AddRange(objs);            
            DataContext.SaveChanges();
            return objs;
        }
    }

    
}
