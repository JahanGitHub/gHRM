using gHRM.Data.CodeFirstMigration.Discipline;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.Repository.Discipline
{
    public interface IDiscCaseEnquiryOfficerRepository : IRepository<DiscCaseEnquiryOfficer>
    {

    }
    public class DiscCaseEnquiryOfficerRepository : RepositoryBaseCodeFirst<DiscCaseEnquiryOfficer>, IDiscCaseEnquiryOfficerRepository
    {
        public DiscCaseEnquiryOfficerRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }

    }
}
