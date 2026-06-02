using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using System.Collections.Generic;

namespace gHRM.Data.Repository
{
    public interface IOfficeDesignationRepository : IRepository<OfficeDesignation>
    {
        List<OfficeDesignation> AddOfficeDesignationList(List<OfficeDesignation> objs);
    }
    public class OfficeDesignationRepository : RepositoryBaseCodeFirst<gHRM.Data.CodeFirstMigration.OfficeDesignation>, IOfficeDesignationRepository
    {
        public OfficeDesignationRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }

        public List<OfficeDesignation> AddOfficeDesignationList(List<OfficeDesignation> objs)
        {
            DataContext.OfficeDesignations.AddRange(objs);
            DataContext.SaveChanges();
            return objs;
        }
    }
}
