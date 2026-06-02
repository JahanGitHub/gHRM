using gHRM.Core.Common;
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.DBDetailModels;
using gHRM.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Service
{
    public interface IEmployeeDesignationService : IServiceBase<EmployeeDesignation>
    {
        // string GetNewDesignationCode(int empdsgId); 
        IEnumerable<DBEmployeeDesignationDetailModel> GetDesignationDetail(string filterColumnName, string filterValue, int startRowIndex, string jtSorting, int pageSize, out long TotCount);
        IEnumerable<ValidationResult> IsValidEmployeeDesignation(string degnationCode);
        //IEnumerable<EmployeeDesignation> SearchEmployeeDesignation();
        IEnumerable<EmployeeDesignation> getDesignationTypeWiseDesignation(string DesignationType);
        List<EmployeeDesignation> AddDesignationList(List<EmployeeDesignation> objs);

    }
    public class EmployeeDesignationService : IEmployeeDesignationService
    {
        private readonly IEmployeeDesignationRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public EmployeeDesignationService(IEmployeeDesignationRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<EmployeeDesignation> GetAll()
        {
            var entities = repository.GetAll().Where( c=> c.IsActive == true).OrderBy(c => c.DesignationId);
            return entities;
        }

        public EmployeeDesignation GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }        
        
        public EmployeeDesignation Create(EmployeeDesignation objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }
        public IEnumerable<EmployeeDesignation> getDesignationTypeWiseDesignation(string DesignationType)
        {
            var entities = repository.GetAll().Where(c => c.DesignationType == DesignationType && c.IsActive == true).OrderBy(o => o.DesignationName);
            return entities;
        }
        //public string GetNewDesignationCode(int empdsgId)
        //{
        //    string NewDesignationCode = "";
        //    var entity = repository.GetAll().Where(w => w.EmployeeDesignationId == empdsgId).OrderBy(o => o.EmployeeDesignationCode).Last();
        //    if (entity != null)
        //    {
        //        NewDesignationCode = (Convert.ToInt32(entity.EmployeeDesignationCode) + 1).ToString().PadLeft(4, '0');
        //        //if (NewThanaCode.Length == 1)
        //        //    NewThanaCode = "000" + NewThanaCode;
        //        //else if (NewThanaCode.Length == 2)
        //        //    NewThanaCode = "00" + NewThanaCode;
        //        //else if (NewThanaCode.Length == 3)
        //        //    NewThanaCode = "0" + NewThanaCode;
        //    }
        //    else
        //        NewDesignationCode = "0001";
        //    return NewDesignationCode;
        //}

        public void Update(EmployeeDesignation objectToUpdate)
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

        public EmployeeDesignation Get(Expression<Func<EmployeeDesignation, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }        

        public IEnumerable<EmployeeDesignation> GetMany(Expression<Func<EmployeeDesignation, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }

        #region Asyc
        public virtual async Task<IEnumerable<EmployeeDesignation>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }


        public virtual async Task<IEnumerable<EmployeeDesignation>> GetManyAsync(Expression<Func<EmployeeDesignation, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }



        public virtual async Task<EmployeeDesignation> GetAsync(Expression<Func<EmployeeDesignation, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion
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
                if (isActive == false)
                {
                    return false;
                }
            }

            return true;
        }
        IEnumerable<ValidationResult> IEmployeeDesignationService.IsValidEmployeeDesignation(string degnationCode)
        {
            var entity = repository.Get(p => p.DesignationCode == degnationCode);
            if (entity != null)
            {

                yield return new ValidationResult("EmployeeDesignationCode", "Duplicate Designation.");

            }
        }
        public IEnumerable<DBEmployeeDesignationDetailModel> GetDesignationDetail(string filterColumnName, string filterValue, int startRowIndex, string jtSorting, int pageSize, out long TotCount)
        {
            return repository.GetDesignationDetail(filterColumnName, filterValue, startRowIndex, jtSorting, pageSize, out TotCount);
        }

        public List<EmployeeDesignation> AddDesignationList(List<EmployeeDesignation> objs)
        {
            repository.AddDesignationList(objs);
            return objs;
        } 
    }
}
