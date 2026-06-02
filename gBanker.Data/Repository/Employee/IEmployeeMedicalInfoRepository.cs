
using System;
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using System.Collections.Generic;


namespace gHRM.Data.Repository
{
    public interface IEmployeeMedicalInfoRepository : IRepository<EmployeeMedicalInfo>
    {


    }
    public class EmployeeMedicalInfoRepository : RepositoryBaseCodeFirst<gHRM.Data.CodeFirstMigration.EmployeeMedicalInfo>, IEmployeeMedicalInfoRepository
    {
        public EmployeeMedicalInfoRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }


    }
}
