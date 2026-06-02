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
    public interface IResignNoticeRepository : IRepository<ResignNotice>
    {
        void DeleteResignNotice(long Id);
        bool HasDuplicate(long EmployeeId);
    }
    public class ResignNoticeRepository : RepositoryBaseCodeFirst<ResignNotice>, IResignNoticeRepository
    {
        public ResignNoticeRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }

        public void DeleteResignNotice(long Id)
        {
            ResignNotice _ResignNotice = DataContext.ResignNotices.Find(Id);
            DataContext.ResignNotices.Remove(_ResignNotice);
            DataContext.SaveChanges();
        }

        public bool HasDuplicate(long EmployeeId)
        {
            return DataContext.ResignNotices.Where(x => x.IsActive && x.EmployeeId == EmployeeId).Count() > 0;
        }
    }
}
