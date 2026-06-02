using System;
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using System.Collections.Generic;


namespace gHRM.Data.Repository
{
    public interface IEmployeeOtherQualificationRepository : IRepository<EmployeeOtherQualification>
    {


    }
    public class EmployeeOtherQualificationRepository : RepositoryBaseCodeFirst<gHRM.Data.CodeFirstMigration.EmployeeOtherQualification>, IEmployeeOtherQualificationRepository
    {
        public EmployeeOtherQualificationRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }


    }
}
