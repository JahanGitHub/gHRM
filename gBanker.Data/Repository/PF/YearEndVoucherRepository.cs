using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.CodeFirstMigration.PF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.Repository.PF
{
    public interface IYearEndVoucherRepository : IRepository<YearEndVoucher>
    {
       
    }
    public class YearEndVoucherRepository : RepositoryBaseCodeFirst<YearEndVoucher>, IYearEndVoucherRepository
    {
        public YearEndVoucherRepository(IDatabaseFactoryCodeFirst databaseFactory): base(databaseFactory)
        {
            
        }

       
    }
}
