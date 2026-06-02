using gHRM.Data.CodeFirstMigration.Discipline;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.DBDetailModels.Discipline;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.Repository.Discipline
{
    public interface IDiscPunishmentRepository : IRepository<DiscPunishment>
    {
        IEnumerable<DBDiscPunishmentDetailsModels> GetPunishmentDetail(string filterColumnName, string filterValue, int startRowIndex, string jtSorting, int pageSize, out long TotCount);
    }
    public class DiscPunishmentRepository : RepositoryBaseCodeFirst<DiscPunishment>, IDiscPunishmentRepository
    {
        public DiscPunishmentRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
        public IEnumerable<DBDiscPunishmentDetailsModels> GetPunishmentDetail(string filterColumnName, string filterValue, int startRowIndex, string jtSorting, int pageSize, out long TotCount)
        {
            IQueryable<DiscPunishment> results = null;
            if (filterColumnName == "PunishmentCode")
                results = DataContext.DiscPunishments.Where(x => x.IsActive == true && x.PunishmentCode.Contains(filterValue));
            else if (filterColumnName == "PunishmentName")
                results = DataContext.DiscPunishments.Where(x => x.IsActive == true && x.PunishmentName.Contains(filterValue));
            else
                results = DataContext.DiscPunishments.Where(x => x.IsActive == true);

            TotCount = results.LongCount();

            var obj = results.OrderBy(o => o.PunishmentId).Skip(startRowIndex).Take(pageSize).Select(s => new DBDiscPunishmentDetailsModels()
            {
                PunishmentId = s.PunishmentId,
                PunishmentCode = s.PunishmentCode,
                PunishmentName = s.PunishmentName,
                Remarks = s.Remarks,                                                       // DataFilter               
            });

            if (!string.IsNullOrWhiteSpace(jtSorting))
            {
                if (jtSorting == "PunishmentCode ASC")
                    return obj.OrderBy(o => o.PunishmentCode);
                else if (jtSorting == "PunishmentCode DESC")
                    return obj.OrderByDescending(o => o.PunishmentCode);
                else if (jtSorting == "PunishmentName ASC")
                    return obj.OrderBy(o => o.PunishmentName);
                else if (jtSorting == "PunishmentName DESC")
                    return obj.OrderByDescending(o => o.PunishmentName);
                else
                    return obj.OrderBy(o => o.PunishmentId);
            }
            else
                return obj.OrderBy(o => o.PunishmentId);
        }
    }
}
