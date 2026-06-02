
using System;
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using System.Collections.Generic;

namespace gHRM.Data.Repository
{
    public interface IEmployeeInformationApprovalRepository : IRepository<EmployeeInformationApproval>
    {

    }
    public class EmployeeInformationApprovalRepository : RepositoryBaseCodeFirst<EmployeeInformationApproval>, IEmployeeInformationApprovalRepository
    {
        public EmployeeInformationApprovalRepository(IDatabaseFactoryCodeFirst databaseFactory)  //check
            : base(databaseFactory)
        {

        }


    }
}
