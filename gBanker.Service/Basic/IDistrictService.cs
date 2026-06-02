using eRecruitment.Infrastructure.Service.CacheManagerServices;
using gHRM.Core.Common;
using gHRM.Core.Filters;
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.DBDetailModels;
using gHRM.Data.Repository;
using gHRM.Service.eRecruit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Service
{
    public interface IDistrictService : IServiceBase<District>
    {
        string GetNewDistrictCode();
        IEnumerable<ValidationResult> IsValidDistrict(string districtCode);
        IEnumerable<DBDistrictDetailModel> GetDistrictDetail(int DivId, int DistId, int startRowIndex, string jtSorting, int pageSize, out long TotCount);
        District GetByName(string name);
        IEnumerable<District> GetDistrictListByFilter(BaseSearchFilter filter);
    }
    public class DistrictService : IDistrictService
    {
        private readonly IDistrictRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;
        private readonly ICacheManagerService cacheManagerService;


        public DistrictService(IDistrictRepository repository, IUnitOfWorkCodeFirst unitOfWork, ICacheManagerService cacheManagerService )
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<District> GetAll()
        {
            var entities = repository.GetAll().OrderBy(c => c.district_id);
            return entities;
        }

        public District GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }
        public string GetNewDistrictCode()
        {
            string NewDistrictCode = "";
            var entity = repository.GetAll().OrderBy(o => o.district_code).Last();
          //  var entity = repository.GetAll().Where(w => w.district_id == distId).OrderBy(o => o.thana_code).Last();
            if (entity != null)
            {
                NewDistrictCode = (Convert.ToInt32(entity.district_code) + 1).ToString().PadLeft(3, '0');
                //if (NewThanaCode.Length == 1)
                //    NewThanaCode = "000" + NewThanaCode;
                //else if (NewThanaCode.Length == 2)
                //    NewThanaCode = "00" + NewThanaCode;
                //else if (NewThanaCode.Length == 3)
                //    NewThanaCode = "0" + NewThanaCode;
            }
            else
                NewDistrictCode = "001";
            return NewDistrictCode;
        }
        public District Create(District objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(District objectToUpdate)
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
            unitOfWork.Commit();
        }

        public District Get(Expression<Func<District, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<District> GetMany(Expression<Func<District, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }

        public IEnumerable<District> GetDistrictListByFilter(BaseSearchFilter filter)
        {
            var entities = cacheManagerService.GetAllDistricts();
            if (entities.Any())
            {
                entities = entities.Where(f => f.division_Id == filter.StateOrProvinceId)
                .OrderBy(o => o.district_name_eng);
            }

            return entities;
        }

        #region Asyc
        public virtual async Task<IEnumerable<District>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }


        public virtual async Task<IEnumerable<District>> GetManyAsync(Expression<Func<District, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }



        public virtual async Task<District> GetAsync(Expression<Func<District, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion
        public bool Inactivate(long id, DateTime? inactiveDate)
        {
            throw new NotImplementedException();
        }
        public bool IsContinued(long id)
        {
            throw new NotImplementedException();
        }
        IEnumerable<ValidationResult> IDistrictService.IsValidDistrict(string districtCode)
        {
            var entity = repository.Get(p => p.district_code == districtCode);
            if (entity != null)
            {
                yield return new ValidationResult("DistrictCode", "Duplicate District Code.");

            }
        }
        public IEnumerable<DBDistrictDetailModel> GetDistrictDetail(int DivId, int DistId, int startRowIndex, string jtSorting, int pageSize, out long TotCount)
        {
            return repository.GetDistrictDetail(DivId, DistId, startRowIndex, jtSorting, pageSize, out TotCount);
        }
        public District GetByName(string name)
        {
            var single = new District();
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                    return null;

                using (var db = new gHRMDBContext())
                {
                    single = db.Districts
                        .FirstOrDefault(f => f.district_name_eng.Trim().ToLower() == name.Trim().ToLower());
                }
            }
            catch
            {
                return null;
            }

            return single;
        }

       

    }
}
