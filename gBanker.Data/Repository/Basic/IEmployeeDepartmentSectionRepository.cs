
using System;
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using System.Collections.Generic;

namespace gHRM.Data.Repository
{
    public interface IEmployeeDepartmentSectionRepository : IRepository<EmployeeDepartmentSection>
    {

    }
    public class EmployeeDepartmentSectionRepository : RepositoryBaseCodeFirst<EmployeeDepartmentSection>, IEmployeeDepartmentSectionRepository
    {
        public EmployeeDepartmentSectionRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }


    }
}
