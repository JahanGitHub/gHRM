using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.Discipline;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.DBDetailModels;
using System;
using System.Collections.Generic;
using System.Linq;


namespace gHRM.Data.Repository.Discipline
{
    public interface IDiscCaseAnnexationRepository : IRepository<DiscCaseAnnexation>
    {

    }
    public class DiscCaseAnnexationRepository : RepositoryBaseCodeFirst<DiscCaseAnnexation>, IDiscCaseAnnexationRepository
    {
        public DiscCaseAnnexationRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }

    }
}
