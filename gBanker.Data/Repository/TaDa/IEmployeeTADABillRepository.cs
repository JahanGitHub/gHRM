using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.CodeFirstMigration.TaDa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.Repository.TaDa
{
    public interface IEmployeeTADABillRepository : IRepository<EmployeeTADABill>
    {
        List<EmployeeTADABill> AddTADA(List<EmployeeTADABill> objs);
    }

    public class EmployeeTADABillRepository : RepositoryBaseCodeFirst<EmployeeTADABill>, IEmployeeTADABillRepository
    {
        public EmployeeTADABillRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
        public List<EmployeeTADABill> AddTADA(List<EmployeeTADABill> objs)
        {
            DataContext.EmployeeTADABill.AddRange(objs);
            DataContext.SaveChanges();
            return objs;
        }
    }
}
