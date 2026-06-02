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
    public interface IOfficeRepository : IRepository<Office>
    {
        Task<Office> GetOfficeByOfficeId(int officeId);
        IEnumerable<Office> GetOfficeAndRelatedOffices(string officeCode);
        Task<IEnumerable<DBOfficeDetailModel>> GetOfficeListByFilter(OfficeSearchFilter filter);
        Task<Office> GetOfficeByFilter(OfficeSearchFilter filter);
        IEnumerable<DBOfficeDetailModel> GetOfficeDetail();
        IEnumerable<DBOfficeDetailModel> GetAllAreaOffice();
        IEnumerable<DBOfficeDetailModel> GetAllZoneOffice(string headofficeCode, int? orgiD);
        IEnumerable<DBOfficeDetailModel> GetAllAreaOfficeForZone(string headofficeCode, string zoneCode);
        IEnumerable<DBOfficeDetailModel> GetAllBranchesForArea(string headofficeCode, string zoneCode, string areaCode);
        int GetAllOfficeCount();
        IEnumerable<DBOfficeDetailModel> GetOfficeDetailInformation(string filterColumnName, string filterValue, int startRowIndex, string jtSorting, int pageSize, out long TotCount);

        IEnumerable<DropDownAttribute> getOfficeTypeWiseOfficeList(int OfficeTypeId);
        IEnumerable<DBOfficeDetailModel> GetAllZoneOffice(string headofficeCode);
        List<Dictionary<string, object>> GetAllZonalOfficeList();
    }
    public class OfficeRepository : RepositoryBaseCodeFirst<Office>, IOfficeRepository
    {
        public OfficeRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }

        public async Task<IEnumerable<DBOfficeDetailModel>> GetOfficeListByFilter(OfficeSearchFilter filter)
        {
            var filteredList = new List<DBOfficeDetailModel>();

            try
            {
                var officeId = filter.OfficeId > 0 ? filter.OfficeId.ToString() : "NULL";
                var filterOfficeCode = !string.IsNullOrEmpty(filter.OfficeCode) ? $"'{filter.OfficeCode}'" : "NULL";
                var officeTypeId = filter.OfficeTypeId > 0 ? filter.OfficeTypeId.ToString() : "NULL";

                var sqlCommand = $@"[dbo].[Office_GetOfficesByFilter]                                
                                 {officeId},
                                 {filterOfficeCode},
                                 {officeTypeId},   
                                 {filter.PageNumber},
                                 {filter.PageSize },
                                '{filter.SortColumn }',
                                '{filter.SortDirection }'
                                ";

                filteredList = await DataContext.Database.SqlQuery<DBOfficeDetailModel>(sqlCommand).ToListAsync();

                if (filteredList.Any())
                    filter.TotalCount = filteredList[0].TotalCount;
            }
            catch (Exception ex)
            {
                return new List<DBOfficeDetailModel>();
            }

            return filteredList;
        }
        
        public async Task<Office> GetOfficeByFilter(OfficeSearchFilter filter)
        {
            var single = new Office();

            try
            {
                IQueryable<Office> query = DataContext.Offices.Where(f =>
                                           (filter.OfficeCode == string.Empty || filter.OfficeCode == null && f.OfficeCode == filter.OfficeCode)
                                        && (filter.OfficeId == 0 || filter.OfficeId == null && f.OfficeId == filter.OfficeId)
                                    );

                single = await query.FirstOrDefaultAsync();

                return single;
            }
            catch (Exception ex)
            {
                return new Office();
            }
        }

        public async Task<Office> GetOfficeByOfficeId(int officeId)
        {
            var single = new Office();

            try
            {
                var sqlCommand = $@"SELECT *FROM Office WHERE OfficeId={officeId}";
                single = await DataContext.Database.SqlQuery<Office>(sqlCommand).FirstOrDefaultAsync();

                return single;
            }
            catch (Exception ex)
            {
                return new Office();
            }
        }

        public IEnumerable<DBOfficeDetailModel> GetOfficeDetail()
        {
            var obj = DataContext.Offices.Where(x => x.IsActive == true)
                .Select(s => new DBOfficeDetailModel()
                {
                    OfficeID = s.OfficeId,
                    OfficeCode = s.OfficeCode,
                    OfficeName = s.OfficeName,
                    OfficeLevel = s.OfficeLevel,
                    FirstLevel = s.FirstLevel,
                    SecondLevel = s.SecondLevel,
                    ThirdLevel = s.ThirdLevel,
                    FourthLevel = s.FourthLevel,
                    OperationStartDate = s.OperationStartDate,
                    OfficeAddress = s.OfficeAddress,
                    PostCode = s.PostCode,
                    //GeoLocationID = s.GeoLocationID,
                    //LocationName = s.GeoLocation == null ? "" : s.GeoLocation.LocationName,
                    Email = s.Email,
                    Phone = s.Phone
                });

            return obj;
        }

        public IEnumerable<Office> GetOfficeAndRelatedOffices(string officeCode)
        {
            try
            {
                var sqlCommand = $"[dbo].[Office_GetOfficeAndRelatedOffices] '{officeCode}'";
                var officeList = DataContext.Database.SqlQuery<Office>(sqlCommand).AsParallel().ToList();

                return officeList;
            }
            catch (Exception ex)
            {
                return new List<Office>();
            }
        }

        public IEnumerable<DBOfficeDetailModel> GetAllAreaOffice()
        {
            var AreaOffice = DataContext.Offices.Where(x => x.IsActive == true && x.OfficeLevel == 4)
                .Select(s => new DBOfficeDetailModel()
                {
                    OfficeID = s.OfficeId,
                    OfficeCode = s.OfficeCode,
                    OfficeName = s.OfficeName,
                    OfficeLevel = s.OfficeLevel,
                    FirstLevel = s.FirstLevel,
                    SecondLevel = s.SecondLevel,
                    ThirdLevel = s.ThirdLevel,
                    FourthLevel = s.FourthLevel,
                    OperationStartDate = s.OperationStartDate,
                    OfficeAddress = s.OfficeAddress,
                    PostCode = s.PostCode,
                    //GeoLocationID = s.GeoLocationID,
                    //LocationName = s.GeoLocation == null ? "" : s.GeoLocation.LocationName,
                    Email = s.Email,
                    Phone = s.Phone
                });
            return AreaOffice;
        }

        public IEnumerable<DBOfficeDetailModel> GetAllZoneOffice(string headofficeCode)
        {
            var zoneOffices = DataContext.Offices.Where(x => x.IsActive == true && x.OfficeLevel == 2)
                .Select(s => new DBOfficeDetailModel()
                {
                    OfficeID = s.OfficeId,
                    OfficeCode = s.OfficeCode,
                    OfficeName = s.OfficeName,
                    OfficeLevel = s.OfficeLevel,
                    FirstLevel = s.FirstLevel,
                    SecondLevel = s.SecondLevel,
                    ThirdLevel = s.ThirdLevel,
                    FourthLevel = s.FourthLevel,
                    OperationStartDate = s.OperationStartDate,
                    OfficeAddress = s.OfficeAddress,
                    PostCode = s.PostCode,
                    //GeoLocationID = s.GeoLocationID,
                    //LocationName = s.GeoLocation == null ? "" : s.GeoLocation.LocationName,
                    Email = s.Email,
                    Phone = s.Phone
                });
            return zoneOffices;
        }

        public IEnumerable<DBOfficeDetailModel> GetAllAreaOfficeForZone(string headofficeCode, string zoneCode)
        {
            var areaOffices = DataContext.Offices.Where(x => x.IsActive == true && x.SecondLevel == zoneCode && x.OfficeLevel == 3)
                .Select(s => new DBOfficeDetailModel()
                {
                    OfficeID = s.OfficeId,
                    OfficeCode = s.OfficeCode,
                    OfficeName = s.OfficeName,
                    OfficeLevel = s.OfficeLevel,
                    FirstLevel = s.FirstLevel,
                    SecondLevel = s.SecondLevel,
                    ThirdLevel = s.ThirdLevel,
                    FourthLevel = s.FourthLevel,
                    OperationStartDate = s.OperationStartDate,
                    OfficeAddress = s.OfficeAddress,
                    PostCode = s.PostCode,
                    //GeoLocationID = s.GeoLocationID,
                    //LocationName = s.GeoLocation == null ? "" : s.GeoLocation.LocationName,
                    Email = s.Email,
                    Phone = s.Phone
                });
            return areaOffices;
        }

        public IEnumerable<DBOfficeDetailModel> GetAllBranchesForArea(string headofficeCode, string zoneCode, string areaCode)
        {
            var branchOffices = DataContext.Offices.Where(x => x.IsActive == true && x.SecondLevel == zoneCode && x.ThirdLevel == areaCode && x.OfficeLevel == 4)
                 .Select(s => new DBOfficeDetailModel()
                 {
                     OfficeID = s.OfficeId,
                     OfficeCode = s.OfficeCode,
                     OfficeName = s.OfficeName,
                     OfficeLevel = s.OfficeLevel,
                     FirstLevel = s.FirstLevel,
                     SecondLevel = s.SecondLevel,
                     ThirdLevel = s.ThirdLevel,
                     FourthLevel = s.FourthLevel,
                     OperationStartDate = s.OperationStartDate,
                     OfficeAddress = s.OfficeAddress,
                     PostCode = s.PostCode,
                     //GeoLocationID = s.GeoLocationID,
                     //LocationName = s.GeoLocation == null ? "" : s.GeoLocation.LocationName,
                     Email = s.Email,
                     Phone = s.Phone
                 });
            return branchOffices; ;
        }

        public int GetAllOfficeCount()
        {
            return DataContext.Offices.Where(x => x.IsActive == true && x.OfficeLevel == 4).Count();
        }

        public IEnumerable<DBOfficeDetailModel> GetOfficeDetailInformation(string filterColumnName, string filterValue, int startRowIndex, string jtSorting, int pageSize, out long TotCount)
        {//
            IQueryable<Office> results = null;
            if (filterColumnName == "OfficeName")
                results = DataContext.Offices.Where(x => x.IsActive == true && x.OfficeName.Contains(filterValue));
            else if (filterColumnName == "OfficeCode")
                results = DataContext.Offices.Where(x => x.IsActive == true && x.OfficeCode.Contains(filterValue));
            else
                results = DataContext.Offices.Where(x => x.IsActive == true);

            TotCount = results.LongCount();
            var SlNo = 0;
            var obj = results.OrderBy(o => o.OfficeId).Skip(startRowIndex).Take(pageSize).Select(s => new DBOfficeDetailModel()
            {
                SlNo = SlNo + 1,
                OfficeID = s.OfficeId,
                OfficeCode = s.OfficeCode,
                OfficeName = s.OfficeName,
                OfficeTypeId = s.OfficeTypeId, 
                OfficeLevel = s.OfficeLevel,
                FirstLevel = s.FirstLevel,
                SecondLevel = s.SecondLevel,
                ThirdLevel = s.ThirdLevel,
                FourthLevel = s.FourthLevel,
                OperationStartDate = s.OperationStartDate,
                OfficeAddress = s.OfficeAddress,
                PostCode = s.PostCode,
                //GeoLocationID = s.GeoLocationID,               
                Email = s.Email,
                Phone = s.Phone
                // DataFilter               
            });

            if (!string.IsNullOrWhiteSpace(jtSorting))
            {
                if (jtSorting == "OfficeID ASC")
                    return obj.OrderBy(o => o.OfficeID);
                else if (jtSorting == "OfficeID DESC")
                    return obj.OrderByDescending(o => o.OfficeID);
                else if (jtSorting == "OfficeCode ASC")
                    return obj.OrderBy(o => o.OfficeCode);
                else if (jtSorting == "OfficeCode DESC")
                    return obj.OrderByDescending(o => o.OfficeCode);
                else if (jtSorting == "OfficeName ASC")                       //DataSorting
                    return obj.OrderBy(o => o.OfficeName);
                else if (jtSorting == "OfficeName DESC")
                    return obj.OrderByDescending(o => o.OfficeName);
                else if (jtSorting == "OfficeAddress ASC")                                           //DataSorting
                    return obj.OrderBy(o => o.OfficeAddress);
                else if (jtSorting == "OfficeAddress DESC")
                    return obj.OrderByDescending(o => o.OfficeAddress);
                else if (jtSorting == "OfficeLevel ASC")
                    return obj.OrderBy(o => o.OfficeLevel);
                else if (jtSorting == "OfficeLevel DESC")
                    return obj.OrderByDescending(o => o.OfficeLevel);

                else
                    return obj.OrderBy(o => o.OfficeID);
            }
            else
                return obj.OrderBy(o => o.OfficeID);
        }

        public IEnumerable<DropDownAttribute> getOfficeTypeWiseOfficeList(int OfficeTypeId)
        {
            var list = DataContext.Offices
                 .Where(b => b.IsActive == true && b.OfficeTypeId == OfficeTypeId)
                 .Select(b => new DropDownAttribute
                 {
                     Id = b.OfficeId,
                     Name = b.OfficeName,
                     NameOther = b.OfficeNameBn,
                     OtherString = b.OfficeCode
                 }).ToList();
            return list;
        }

        public IEnumerable<DBOfficeDetailModel> GetAllZoneOffice(string headofficeCode, int? orgiD)
        {
            throw new NotImplementedException();
        }

        public List<Dictionary<string, object>> GetAllZonalOfficeList()
        {
            List<Dictionary<string, object>> OfficeList = new List<Dictionary<string, object>>();
            var DataList = (from O in DataContext.Offices
                     join OT in DataContext.OfficeTypes on O.OfficeTypeId equals OT.OfficeTypeId
                     where O.IsActive && OT.IsActive && OT.OfficeTypeCode == "ZO"
                     orderby O.OfficeName
                     select new { Id = O.OfficeId, Name = O.OfficeName }).ToList();
            foreach (var DataItem in DataList)
            {
                Dictionary<string, object> Item = new Dictionary<string, object>();
                Item["Id"] = DataItem.Id;
                Item["Name"] = DataItem.Name;
                OfficeList.Add(Item);
            }
            return OfficeList;
        }
    }
}
