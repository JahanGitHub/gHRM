using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.Repository
{
    public interface IEmployeeGuarantorTranInformationRepository : IRepository<EmployeeGuarantorTranInformation>
    {

    }

    public class EmployeeGuarantorTranInformationRepository : RepositoryBaseCodeFirst<EmployeeGuarantorTranInformation>, IEmployeeGuarantorTranInformationRepository
    {
        public EmployeeGuarantorTranInformationRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}

