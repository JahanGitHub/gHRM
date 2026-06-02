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
    public interface IOfficeRegionRepository : IRepository<OfficeRegion>
    {
        bool Save(OfficeRegion Data, long LoggedInEmployeeId, out string Message);
        void DeleteRegion(int Id);
        string GetNameById(int Id);
        bool SaveMapOffice(OfficeRegionMapping _RegionMap, long LoggedInEmployeeId, out string Message);
        void DeleteMapOffice(int Id);
    }
    public class OfficeRegionRepository : RepositoryBaseCodeFirst<OfficeRegion>, IOfficeRegionRepository
    {
        public OfficeRegionRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }

        public bool Save(OfficeRegion Data, long LoggedInEmployeeId, out string Message)
        {
            if (!IsSaveValid(Data, out Message)) return false;
            OfficeRegion _Region = Data.Id > 0 ? DataContext.OfficeRegions.Find(Data.Id) : new OfficeRegion();
            _Region.Name = Data.Name;

            if (_Region.Id > 0)
            {
                _Region.UpdateDate = DateTime.Now;
                _Region.UpdateUser = LoggedInEmployeeId;
            }
            else
            {
                _Region.IsActive = true;
                _Region.CreateDate = DateTime.Now;
                _Region.CreateUser = LoggedInEmployeeId;
                DataContext.OfficeRegions.Add(_Region);
            }
            DataContext.SaveChanges();
            return true;
        }

        public void DeleteRegion(int Id)
        {
            OfficeRegion _Region = DataContext.OfficeRegions.Find(Id);
            _Region.IsActive = false;
            DataContext.SaveChanges();
        }

        public bool SaveMapOffice(OfficeRegionMapping _RegionMap, long LoggedInEmployeeId, out string Message)
        {
            if (!IsSaveMapOffice(_RegionMap, out Message)) return false;
            _RegionMap.IsActive = true;
            _RegionMap.CreateDate = DateTime.Now;
            _RegionMap.CreateUser = LoggedInEmployeeId;
            DataContext.OfficeRegionMappings.Add(_RegionMap);
            DataContext.SaveChanges();
            return true;
        }

        public void DeleteMapOffice(int Id)
        {
            OfficeRegionMapping _RegionMap = DataContext.OfficeRegionMappings.Find(Id);
            _RegionMap.IsActive = false;
            DataContext.SaveChanges();
        }

        public string GetNameById(int Id)
        {
            return DataContext.OfficeRegions.Where(x => x.Id == Id && x.IsActive).Select(x => x.Name).FirstOrDefault();
        }

        private bool IsSaveValid(OfficeRegion Data, out string Message)
        {
            Message = "";
            string Name = null == Data.Name ? "" : Data.Name.Trim();

            if (Name == "")
            {
                Message = "Name is required";
                return false;
            }
            if ((Data.Id == 0 && DataContext.OfficeRegions.Where(x => x.IsActive && x.Name == Name).Count() > 0)
                || (Data.Id > 0 && DataContext.OfficeRegions.Where(x => x.IsActive && x.Id != Data.Id && x.Name == Name).Count() > 0))
            {
                Message = "Duplicate Name exists";
                return false;
            }
            return true;
        }

        private bool IsSaveMapOffice(OfficeRegionMapping Data, out string Message)
        {
            Message = "";

            if (Data.OfficeId == 0)
            {
                Message = "Office is required";
                return false;
            }
            if (DataContext.OfficeRegionMappings.Where(x => x.RegionId == Data.RegionId && x.OfficeId == Data.OfficeId && x.IsActive).Count() > 0)
            {
                Message = "Office is already under this Region";
                return false;
            }
            if (DataContext.OfficeRegionMappings.Where(x => x.RegionId != Data.RegionId && x.OfficeId == Data.OfficeId && x.IsActive).Count() > 0)
            {
                string RegionName = (from M in DataContext.OfficeRegionMappings
                         join R in DataContext.OfficeRegions on M.RegionId equals R.Id
                         where M.OfficeId == Data.OfficeId && M.IsActive
                         select R.Name).FirstOrDefault();
                Message = "Office is currenty under Region: " + RegionName;
                return false;
            }
            return true;
        }
    }
}

