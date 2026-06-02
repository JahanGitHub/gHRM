
using System;
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using System.Collections.Generic;

namespace gHRM.Data.Repository
{
    public interface IEmployeeSignatureDesignationRepository : IRepository<EmployeeSignatureDesignation>
    {

    }
    public class EmployeeSignatureDesignationRepository : RepositoryBaseCodeFirst<EmployeeSignatureDesignation>, IEmployeeSignatureDesignationRepository
    {
        public EmployeeSignatureDesignationRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }


    }
}
