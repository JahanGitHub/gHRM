using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;

namespace gHRM.Data.Repository
{
    public interface IEmployeeMaritalStatusApprovalRepository : IRepository<EmployeeMaritalStatusApproval>
    {

    }
    public class EmployeeMaritalStatusApprovalRepository : RepositoryBaseCodeFirst<EmployeeMaritalStatusApproval>, IEmployeeMaritalStatusApprovalRepository
    {
        public EmployeeMaritalStatusApprovalRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }


    }
}
