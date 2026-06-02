using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.CodeFirstMigration.PF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.Repository.PF
{
    public interface ITempPFCollectionRepository : IRepository<TempPFCollection>
    {
        bool AddBulk(List<TempPFCollection> objs);
    }
    //public class TempPFCollectionRepository : PFRepositoryBaseCodeFirst<TempPFCollection>, ITempPFCollectionRepository
    public class TempPFCollectionRepository : RepositoryBaseCodeFirst<TempPFCollection>, ITempPFCollectionRepository
    {
        public TempPFCollectionRepository(IDatabaseFactoryCodeFirst databaseFactory) : base(databaseFactory)
        { }
        public bool AddBulk(List<TempPFCollection> objs)
        {
            DataContext.TempPFCollections.AddRange(objs);
            DataContext.SaveChanges();
            return true;
        }
    }
}
