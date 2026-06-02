using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.CodeFirstMigration.Payroll;
using gHRM.Data.DBDetailModels.Payroll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.Repository.Payroll
{
    public interface IPRComponent_designationRepository : IRepository<PRComponent>
    {
        IEnumerable<DBPRComponentViewModel> GetDBPRComponentViewModel(string filterColumnName, string filterValue, int startRowIndex, string jtSorting, int pageSize, out long TotCount);

    }
    public class PRComponent_designationRepository : RepositoryBaseCodeFirst<PRComponent>, IPRComponent_designationRepository
    {
        public PRComponent_designationRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {


        }

        public IEnumerable<DBPRComponentViewModel> GetDBPRComponentViewModel(string filterColumnName, string filterValue, int startRowIndex, string jtSorting, int pageSize, out long TotCount)
        {
            IQueryable<PRComponent> results = null;
            results = DataContext.PRComponents.Where(x => x.IsActive == true);

            TotCount = results.LongCount();

            var obj = results.OrderBy(o => o.PRComponentID).Select(s => new DBPRComponentViewModel()
            {
                ComponentName = s.ComponentName,
                ComponentType = s.ComponentType,
                ComponentAmount = s.ComponentAmount,
                TransactionType = s.TransactionType,
                AccountCode = s.SalaryAccCode,
                EffectiveStartDate = s.EffectiveStartDate,
                EffectiveEndDate = s.EffectiveEndDate,
                PRComponentGroupID = s.PRComponentGroupID,
                ComponentCategory = s.ComponentCategory,
                IsActive = s.IsActive

            });

            return obj.OrderBy(o => o.PRComponentID);
        } // End of Method


    }// End Class

}
