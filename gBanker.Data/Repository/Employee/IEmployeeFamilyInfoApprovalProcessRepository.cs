using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;

namespace gHRM.Data.Repository
{
    public interface IEmployeeFamilyInfoApprovalProcessRepository : IRepository<EmployeeFamilyInfoApprovalProcess>
    {

    }
    public class EmployeeFamilyInfoApprovalProcessRepository : RepositoryBaseCodeFirst<EmployeeFamilyInfoApprovalProcess>, IEmployeeFamilyInfoApprovalProcessRepository
    {
        public EmployeeFamilyInfoApprovalProcessRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }


    }
}