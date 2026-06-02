using gHRM.Core.Common;
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.Repository;
using gHRM.Data.DBDetailModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using gHRM.Core.Filters;
using gHRM.Service.eRecruit;
using eRecruitment.Infrastructure.Service.CacheManagerServices;

namespace gHRM.Service
{
    public interface ILgThanaService : IServiceBase<LgThana>
    {
        IEnumerable<ValidationResult> IsValidLgThana(string thanaCode);
        string GetNewThanaCode(int distId);
        //IEnumerable<LgThana> SearchThana();
        // LgThana GetByThanaId(Int32 thana_id);
        //LgThana GetByThanaCode(string thana_Code);
        IEnumerable<DBThanaDetailsModel> GetThanaDetail(int DistId, string filterColumnName, string filterValue, int startRowIndex, string jtSorting, int pageSize, out long TotCount);

        LgThana GetByName(string name);
        IEnumerable<LgThana> GetLgThanaListByFilter(BaseSearchFilter filter);
    }
    public class LgThanaService : ILgThanaService
    {
        private readonly ILgThanaRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;
        private readonly ICacheManagerService cacheManagerService;

        public LgThanaService(ILgThanaRepository repository, ICacheManagerService cacheManagerService, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
            this.cacheManagerService = cacheManagerService;
        }
        public IEnumerable<LgThana> GetAll()
        {
            var entities = repository.GetAll().OrderBy(c => c.thana_id);
            return entities;
        }

        //public Employee GetByThanaId(Int32 thana_id)
        //{
        //    var entity = repository.Get(e => e.thana_id  == thana_id );
        //    return entity;
        //}

        public string GetNewThanaCode(int distId)
        {
            string NewThanaCode = "";
            var entity = repository.GetAll().Where(w => w.district_id == distId).OrderBy(o => o.thana_code).Last();
            if (entity != null)
            {
                NewThanaCode = (Convert.ToInt32(entity.thana_code) + 1).ToString().PadLeft(4,'0');
                //if (NewThanaCode.Length == 1)
                //    NewThanaCode = "000" + NewThanaCode;
                //else if (NewThanaCode.Length == 2)
                //    NewThanaCode = "00" + NewThanaCode;
                //else if (NewThanaCode.Length == 3)
                //    NewThanaCode = "0" + NewThanaCode;
            }
            else
                NewThanaCode = "0001";
            return NewThanaCode;
        }
        public LgThana GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public LgThana Create(LgThana objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(LgThana objectToUpdate)
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
        public bool Inactivate(long id, DateTime? inactiveDate)
        {
            throw new NotImplementedException();
        }

        public bool IsContinued(long id)
        {
            throw new NotImplementedException();
        }

        IEnumerable<ValidationResult> ILgThanaService.IsValidLgThana(string thanaCode)
        {
            var entity = repository.Get(p => p.thana_code == thanaCode);
            if (entity != null)
            {

                yield return new ValidationResult("ThanaCode", "Duplicate Thana Code.");

            }
        }
        public IEnumerable<DBThanaDetailsModel> GetThanaDetail(int DistId, string filterColumnName, string filterValue, int startRowIndex, string jtSorting, int pageSize, out long TotCount)
        {
            return repository.GetThanaDetail(DistId, filterColumnName,filterValue, startRowIndex, jtSorting, pageSize, out TotCount);
        }

        public LgThana Get(Expression<Func<LgThana, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<LgThana> GetMany(Expression<Func<LgThana, bool>> where)
        {
            var entities = repository.GetMany(where);
            return entities;
        }

        public LgThana GetByName(string name)
        {
            var single = new LgThana();
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                    return null;

                using (var db = new gHRMDBContext())
                {
                    single = db.LgThanas
                        .FirstOrDefault(f => f.thana_name_eng.Trim().ToLower() == name.Trim().ToLower());
                }
            }
            catch
            {
                return null;
            }

            return single;
        }

        public IEnumerable<LgThana> GetLgThanaListByFilter(BaseSearchFilter filter)
        {
            var entities = cacheManagerService.GetAllLgThanas();
            if (entities.Any())
            {
                entities = entities.Where(f => f.district_id == filter.DistrictId)
                .OrderBy(o => o.thana_name_eng);
            }

            return entities;
        }

        #region Asyc
        public virtual async Task<IEnumerable<LgThana>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }


        public virtual async Task<IEnumerable<LgThana>> GetManyAsync(Expression<Func<LgThana, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }



        public virtual async Task<LgThana> GetAsync(Expression<Func<LgThana, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion
    }
}
