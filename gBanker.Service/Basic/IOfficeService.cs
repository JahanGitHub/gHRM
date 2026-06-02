using gHRM.Core.Common;
using gHRM.Core.Filters.Offices;
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.DBDetailModels;
using gHRM.Data.Repository;
using gHRM.Data.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;


namespace gHRM.Service
{
    public interface IOfficeService : IServiceBase<Office>
    {
        Task<Office> GetOfficeByFilter(OfficeSearchFilter filter);
        Task<IEnumerable<DBOfficeDetailModel>> GetOfficeListByFilter(OfficeSearchFilter filter);
        IEnumerable<Office> GetOfficeAndRelatedOffices(string officeCode);
        IEnumerable<ValidationResult> IsValidOffice(Office office);        
        IEnumerable<Office> SearchOffice();
        Office GetByOfficeCode(string OfficeCode);
        IEnumerable<DBOfficeDetailModel> GetOfficeDetail();
        IEnumerable<Office> GetOfficeByType(int TypeId);
        IEnumerable<DBOfficeDetailModel> GetAllZoneOffice(string headofficeCode);
        IEnumerable<DBOfficeDetailModel> GetAllZoneOffice(string headofficeCode, int? orgiD);
        IEnumerable<DBOfficeDetailModel> GetAllAreaOffice();
        IEnumerable<DBOfficeDetailModel> GetAllAreaOfficeForZone(string headofficeCode, string zoneCode);
        IEnumerable<DBOfficeDetailModel> GetAllBranchesForArea(string headofficeCode, string zoneCode, string areaCode);
        int GetAllOfficeCount();
        IEnumerable<DBOfficeDetailModel> GetOfficeDetailInformation(string filterColumnName, string filterValue, int startRowIndex, string jtSorting, int pageSize, out long TotCount);
        IEnumerable<Office> GetAOOfc(string zoCode);
        IEnumerable<Office> GetBOOfc(string aoCode);
        IEnumerable<Office> GetBOOfcByZO(string zoCode);
        List<Office> AddOfficeList(List<Office> officeListings);
        IEnumerable<DropDownAttribute> getOfficeTypeWiseOfficeList(int OfficeTypeId);
        Office GetByOfficeOrgID(int Office_Id, int Org_Id);
        List<Dictionary<string, object>> GetAllZonalOfficeList();
    }
    public class OfficeService : IOfficeService
    {
        private readonly IOfficeRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public OfficeService(IOfficeRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public async Task<Office> GetOfficeByFilter(OfficeSearchFilter filter)
        {
            var single = new Office();

            try
            {
                single = await repository.GetOfficeByFilter(filter);
                return single;
            }
            catch (Exception ex)
            {
                return new Office();
            }
        }

        public async Task<IEnumerable<DBOfficeDetailModel>> GetOfficeListByFilter(OfficeSearchFilter filter)
        {
            try
            {
                return await repository.GetOfficeListByFilter(filter);
            }
            catch (Exception ex)
            {
                return new List<DBOfficeDetailModel>();
            }            
        }

        public IEnumerable<Office> GetAll()
        {
            var entities = repository.GetMany(g => g.IsActive == true).OrderBy(c => c.OfficeId);
            return entities;
        }

        public IEnumerable<Office> GetOfficeAndRelatedOffices(string officeCode)
        {
            try
            {                
                var officeList = repository.GetOfficeAndRelatedOffices(officeCode);

                return officeList;
            }
            catch (Exception ex)
            {
                return new List<Office>();
            }
        }
        public Office GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }
        public IEnumerable<Office> GetOfficeByType(int TypeId)
        {
            var entities = repository.GetAll().Where(c => c.OfficeTypeId == TypeId && c.IsActive == true).OrderBy(o => o.OfficeName);
            return entities;
        }
        public Office GetByOfficeCode(string OfficeCode)
        {
            var entity = repository.Get(p => p.OfficeCode == OfficeCode);
            return entity;
        }

        public List<Office> AddOfficeList(List<Office> officeListings)
        {
            using (var db = new gHRMDBContext())
            {
                db.Offices.AddRange(officeListings);
                db.SaveChanges();            
            }
            return officeListings;
        }

        public Office Create(Office objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(Office objectToUpdate)
        {
            repository.Update(objectToUpdate);
            Save();
        }

        public void Delete(int id)
        {
            var entity = repository.GetById(id);
            repository.Delete(entity);
            Save();
        }

        public void Save()
        {
            //throw new NotImplementedException();
            unitOfWork.Commit();
        }

        public bool Inactivate(long id, DateTime? inactiveDate)
        {
            var obj = repository.GetById(id);
            if (obj != null)
            {
                obj.InActiveDate = inactiveDate.HasValue ? inactiveDate : DateTime.Now;
                obj.IsActive = false;
                repository.Update(obj);
                Save();
                return true;
            }
            return false;
        }


        public bool IsContinued(long id)
        {
            var obj = repository.GetById(id);
            if (obj != null)
            {
                var isActive = obj.IsActive;
                if (isActive == true)
                {
                    return false;
                }
            }

            return true;
        }
        //public bool IsValidOffice(Office office, out string msg)
        //{
        //    var entity = repository.Get(p => p.OfficeCode == office.OfficeCode);
        //    msg = "test";
        //    return entity == null ? true : false;
        //}

        IEnumerable<ValidationResult> IOfficeService.IsValidOffice(Office office)
        {
            var entity = repository.Get(p => p.OfficeCode == office.OfficeCode);
            if (entity != null)
            {

                yield return new ValidationResult("OfficeCode", "Duplicate Office.");

            }
        }
        public IEnumerable<Office> SearchOffice()
        {
            return repository.GetMany(g => g.IsActive == true).OrderBy(g => g.OfficeCode);
        }

        public IEnumerable<DBOfficeDetailModel> GetOfficeDetail()
        {
            return repository.GetOfficeDetail();
        }

        public IEnumerable<DBOfficeDetailModel> GetAllAreaOffice()
        {
            return repository.GetAllAreaOffice();
        }

        public IEnumerable<DBOfficeDetailModel> GetAllZoneOffice(string headofficeCode)
        {
            return repository.GetAllZoneOffice(headofficeCode);
        }
        public IEnumerable<DBOfficeDetailModel> GetAllZoneOffice(string headofficeCode, int? orgiD)
        {
            return repository.GetAllZoneOffice(headofficeCode, orgiD);
        }
        public IEnumerable<DBOfficeDetailModel> GetAllAreaOfficeForZone(string headofficeCode, string zoneCode)
        {
           return repository.GetAllAreaOfficeForZone(headofficeCode, zoneCode);
        }

        public IEnumerable<DBOfficeDetailModel> GetAllBranchesForArea(string headofficeCode, string zoneCode, string areaCode)
        {
            return repository.GetAllBranchesForArea(headofficeCode, zoneCode, areaCode);
        }


        public int GetAllOfficeCount()
        {
            return repository.GetAllOfficeCount();
        }
        public IEnumerable<DBOfficeDetailModel> GetOfficeDetailInformation(string filterColumnName, string filterValue, int startRowIndex, string jtSorting, int pageSize, out long TotCount)
        {
            return repository.GetOfficeDetailInformation(filterColumnName, filterValue, startRowIndex, jtSorting, pageSize, out TotCount);
        }

        public IEnumerable<Office> GetAOOfc(string zoCode)
        {
            var entities = repository.GetAll().Where(O => Convert.ToInt32(O.SecondLevel) == Convert.ToInt32(zoCode) && O.OfficeTypeId == 4);
            return entities;
        }
        public IEnumerable<Office> GetBOOfc(string aoCode)
        {
            
            var entities = repository.GetAll().Where(O => Convert.ToInt32(O.ThirdLevel) == Convert.ToInt32(aoCode) && O.OfficeTypeId == 5);
            return entities;
        }
        public IEnumerable<Office> GetBOOfcByZO(string zoCode)
        {
            var entities = repository.GetAll().Where(O => Convert.ToInt32(O.SecondLevel) == Convert.ToInt32(zoCode) && O.OfficeTypeId == 5);
            return entities;
        }

        public Office Get(Expression<Func<Office, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<Office> GetMany(Expression<Func<Office, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }
        public Office GetByOfficeOrgID(int Office_Id, int Org_Id)
        {
            var entity = repository.Get(p => p.CompanyId == Org_Id && p.OfficeId == Office_Id);
            return entity;
        }

        public List<Dictionary<string, object>> GetAllZonalOfficeList()
        {
            return repository.GetAllZonalOfficeList();
        }
        #region Asyc
        public virtual async Task<IEnumerable<Office>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }


        public virtual async Task<IEnumerable<Office>> GetManyAsync(Expression<Func<Office, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }



        public virtual async Task<Office> GetAsync(Expression<Func<Office, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion

        public IEnumerable<DropDownAttribute> getOfficeTypeWiseOfficeList(int OfficeTypeId)
        {
            return repository.getOfficeTypeWiseOfficeList(OfficeTypeId);
        }

        //public IEnumerable<DropDownAttribute> getAreaByZoneId(int? ZoneId)
        //{
        //    return repository.getAreaByZoneId(ZoneId);
        //}
        //public IEnumerable<SelectListItem> getAreaByZoneId(int? ZoneId)
        //{
        //    List<SelectListItem> dropDownFirstElement = new List<SelectListItem>();
        //    dropDownFirstElement.Add(new SelectListItem() { Text = @NUPMS.Web.Resource.PleaseSelect, Value = "" });
        //    var param = new { ZoneId = ZoneId };
        //    var dataList = spService.GetDataWithParameter(param, "SP_GETAreaByZone");
        //    List<SelectListItem> BankListData = new List<SelectListItem>();
        //    var bankList = dataList.Tables[0].AsEnumerable()
        //            .Select(row => new SelectListItem
        //            {
        //                Value = Convert.ToString(row.Field<int>("AreaId")),
        //                Text = row.Field<string>("AreaName"),

        //            }).ToList();

        //    dropDownFirstElement.AddRange(bankList);
        //    return dropDownFirstElement;
        //}

    }
}
