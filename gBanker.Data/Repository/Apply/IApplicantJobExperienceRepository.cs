using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.Apply;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.DBDetailModels;
using System.Collections.Generic;
using System.Linq;
using gHRM.Data.Utility;
using System;
using System.Threading.Tasks;
using gHRM.Core.Filters.Offices;
using System.Data.Entity;
using System.Linq.Expressions;

namespace gHRM.Data.Repository.Apply
{
    public interface IApplicantJobExperienceRepository : IRepository<ApplicantJobExperience>
    {

    }
    public class ApplicantJobExperienceRepository : RepositoryBaseCodeFirst<ApplicantJobExperience>, IApplicantJobExperienceRepository
    {
        public ApplicantJobExperienceRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }

    }
}
