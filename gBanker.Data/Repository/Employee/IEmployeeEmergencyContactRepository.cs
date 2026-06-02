using System;
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using System.Collections.Generic;


namespace gHRM.Data.Repository
{
    public interface IEmployeeEmergencyContactRepository : IRepository<EmployeeEmergencyContact>
    {


    }
    public class EmployeeEmergencyContactRepository : RepositoryBaseCodeFirst<gHRM.Data.CodeFirstMigration.EmployeeEmergencyContact>, IEmployeeEmergencyContactRepository
    {
        public EmployeeEmergencyContactRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }


    }
}
