using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.CodeFirstMigration.Payroll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.Repository.Payroll
{
    public interface IGradeXSalaryStepRepository : IRepository<GradeXSalaryStep>
    { }
    public class GradeXSalaryStepRepository : RepositoryBaseCodeFirst<GradeXSalaryStep>, IGradeXSalaryStepRepository
    {
        public GradeXSalaryStepRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        { }
    }
}
