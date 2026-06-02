
using System;
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using System.Collections.Generic;

namespace gHRM.Data.Repository
{
    public interface IEmployeeDesignationMappingRepository : IRepository<EmployeeDesignationMapping>
    {

    }
    public class EmployeeDesignationMappingRepository : RepositoryBaseCodeFirst<EmployeeDesignationMapping>, IEmployeeDesignationMappingRepository
    {
        public EmployeeDesignationMappingRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }


    }
}
