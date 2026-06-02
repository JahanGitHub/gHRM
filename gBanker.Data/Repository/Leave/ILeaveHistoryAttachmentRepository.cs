using System.Text;
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.DBDetailModels;
using System.Collections.Generic;
using System.Linq;
using gHRM.Data.Utility;
using System;
using System.Threading.Tasks;
using gHRM.Core.Filters.Offices;
using System.Data.Entity;

namespace gHRM.Data.Repository
{
    public interface ILeaveHistoryAttachmentRepository : IRepository<LeaveHistoryAttachment>
    {
        List<Dictionary<string, object>> GetAttachmentList(long LeaveHistoryId);
    }
    public class LeaveHistoryAttachmentRepository : RepositoryBaseCodeFirst<LeaveHistoryAttachment>, ILeaveHistoryAttachmentRepository
    {
        public LeaveHistoryAttachmentRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }

        public List<Dictionary<string, object>> GetAttachmentList(long LeaveHistoryId)
        {
            List<Dictionary<string, object>> DataList = new List<Dictionary<string, object>>();
            var Data = DataContext.LeaveHistoryAttachments.Where(x => x.IsActive && x.LeaveHistoryId == LeaveHistoryId)
                .OrderBy(x => x.FileName).Select(x => new { x.FileName, x.FileLocation }).ToList();
            foreach (var DataItem in Data)
            {
                var Item = new Dictionary<string, object>();
                Item["Name"] = DataItem.FileName;
                Item["Location"] = DataItem.FileLocation;
                DataList.Add(Item);
            }
            return DataList;
        }
    }
}

