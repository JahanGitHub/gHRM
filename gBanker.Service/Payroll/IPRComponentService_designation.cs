using gHRM.Core.Filters.Payroll;
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.CodeFirstMigration.Payroll;
using gHRM.Data.DBDetailModels.Payroll;
using gHRM.Data.Repository.Payroll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Service.Payroll
{
    public interface IPRComponentService_designation : IServiceBase<PRComponent>
    {
        IEnumerable<DBPRComponentViewModel> GetDBPRComponentViewModel(string filterColumnName, string filterValue, int startRowIndex, string jtSorting, int pageSize, out long TotCount);
        
        PRComponent GetSingleComponentByFilter(PRComponentSearchFilter filter);

        bool CheckDuplicateComponent(PRComponent prComponent);
        bool CheckDuplicateComponent_designation(PRComponent newPRComponent);
    }

    public class PRComponentService_designation : IPRComponentService_designation
    {
        private readonly IPRComponent_designationRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public PRComponentService_designation(IPRComponent_designationRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<PRComponent> GetAll()
        {
            var entities = repository.GetAll().Where(c => c.IsActive == true).OrderBy(c => c.PRComponentID);
            return entities;
        }
        public PRComponent GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }
       
        public PRComponent GetSingleComponentByFilter(PRComponentSearchFilter filter)
        {
            var entity = new PRComponent();

            using (var db = new gHRMDBContext())
            {
                entity = db.PRComponents.FirstOrDefault(c => c.IsActive
                                                 && c.OfficeLocationId == filter.OfficeLocationId
                                                 && c.EmployeeTypeId == filter.EmployeeTypeId
                                                 && c.EmployeeStatusId == filter.EmployeeStatusId
                                                 && c.ComponentName.Trim() == filter.ComponentName.Trim()                                                 
                                                 && c.ComponentCategory.Trim() == filter.ComponentCategory.Trim());
            }

            return entity;
        }

        //public bool CheckDuplicateComponent(PRComponent prComponent)
        //{
        //    var isDuplicate = true;

        //    using (var db = new gHRMDBContext())
        //    {
        //        if (prComponent.PRComponentID > 0)
        //        {
        //            isDuplicate = db.PRComponents.Any(c =>
        //                              c.IsActive
        //                           && c.PRComponentID != prComponent.PRComponentID
        //                           && c.ComponentName.Trim() == prComponent.ComponentName.Trim()
        //                           && c.EmployeeStatusId == prComponent.EmployeeStatusId
        //                           && c.EmployeeTypeId == prComponent.EmployeeTypeId
        //                           && c.OfficeLocationId == prComponent.OfficeLocationId
        //                           && c.IsProvidentFundComponent == prComponent.IsProvidentFundComponent
        //                           && c.PFTypeId == prComponent.PFTypeId
        //                           && c.ComponentType == prComponent.ComponentType
        //                           && c.ComponentCategory == prComponent.ComponentCategory
        //                           );
        //        }
        //        else
        //        {
        //            isDuplicate = db.PRComponents.Any(c =>
        //                                   c.IsActive
        //                                && c.ComponentName.Trim() == prComponent.ComponentName.Trim()
        //                                && c.EmployeeStatusId == prComponent.EmployeeStatusId
        //                                && c.EmployeeTypeId == prComponent.EmployeeTypeId
        //                                && c.OfficeLocationId == prComponent.OfficeLocationId
        //                                && c.IsProvidentFundComponent == prComponent.IsProvidentFundComponent
        //                                && c.PFTypeId == prComponent.PFTypeId
        //                                && c.ComponentType == prComponent.ComponentType
        //                                && c.ComponentCategory == prComponent.ComponentCategory
        //                                );
        //        }
        //    }

        //    return isDuplicate;
        //}


        public bool CheckDuplicateComponent(PRComponent prComponent)
        {
            var isDuplicate = true;

            using (var db = new gHRMDBContext())
            {
                if (prComponent.PRComponentID > 0)
                {
                    isDuplicate = db.PRComponents.Any(c =>
                                      c.IsActive
                                   && c.PRComponentID != prComponent.PRComponentID
                                   && c.ComponentName.Trim() == prComponent.ComponentName.Trim()
                                   && c.EmployeeStatusId == prComponent.EmployeeStatusId
                                   && c.EmployeeTypeId == prComponent.EmployeeTypeId
                                   && c.OfficeLocationId == prComponent.OfficeLocationId
                                   && c.IsProvidentFundComponent == prComponent.IsProvidentFundComponent
                                   && c.PFTypeId == prComponent.PFTypeId
                                   && c.ComponentType == prComponent.ComponentType
                                   && c.ComponentCategory == prComponent.ComponentCategory
                                   );
                }
                else
                {
                    isDuplicate = db.PRComponents.Any(c =>
                                           c.IsActive
                                        && c.ComponentName.Trim() == prComponent.ComponentName.Trim()
                                        && c.EmployeeStatusId == prComponent.EmployeeStatusId
                                        && c.EmployeeTypeId == prComponent.EmployeeTypeId
                                        && c.OfficeLocationId == prComponent.OfficeLocationId
                                        );
                }
            }

            return isDuplicate;
        }


        public bool CheckDuplicateComponent_designation(PRComponent prComponent)
        {
            var isDuplicate = true;

            using (var db = new gHRMDBContext())
            {
                if (prComponent.PRComponentID > 0)
                {
                    isDuplicate = db.PRComponents.Any(c =>
                                      c.IsActive
                                   && c.PRComponentID != prComponent.PRComponentID
                                   && c.ComponentName.Trim() == prComponent.ComponentName.Trim()
                                   && c.EmployeeStatusId == prComponent.EmployeeStatusId
                                   && c.EmployeeTypeId == prComponent.EmployeeTypeId
                                   && c.OfficeLocationId == prComponent.OfficeLocationId
                                   && c.IsProvidentFundComponent == prComponent.IsProvidentFundComponent
                                   && c.PFTypeId == prComponent.PFTypeId
                                   && c.ComponentType == prComponent.ComponentType
                                   && c.ComponentCategory == prComponent.ComponentCategory
                                   && c.DesignationId == prComponent.DesignationId
                                   );
                }
                else
                {
                    isDuplicate = db.PRComponents.Any(c =>
                                           c.IsActive
                                        && c.ComponentName.Trim() == prComponent.ComponentName.Trim()
                                        && c.EmployeeStatusId == prComponent.EmployeeStatusId
                                        && c.EmployeeTypeId == prComponent.EmployeeTypeId
                                        && c.OfficeLocationId == prComponent.OfficeLocationId
                                         && c.DesignationId == prComponent.DesignationId
                                        );
                }
            }

            return isDuplicate;
        }

        public void Save()
        {
            unitOfWork.Commit();
        }
        public PRComponent Create(PRComponent objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(PRComponent objectToUpdate)
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
        public bool Inactivate(long id, DateTime? inactiveDate)
        {
            //throw new NotImplementedException();
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
            // throw new NotImplementedException();
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

        public IEnumerable<DBPRComponentViewModel> GetDBPRComponentViewModel(string filterColumnName, string filterValue, int startRowIndex, string jtSorting, int pageSize, out long TotCount)
        {
            return repository.GetDBPRComponentViewModel(filterColumnName, filterValue, startRowIndex, jtSorting, pageSize, out TotCount);
        }

        public PRComponent Get(Expression<Func<PRComponent, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<PRComponent> GetMany(Expression<Func<PRComponent, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }

        #region Asyc
        public virtual async Task<IEnumerable<PRComponent>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }


        public virtual async Task<IEnumerable<PRComponent>> GetManyAsync(Expression<Func<PRComponent, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }



        public virtual async Task<PRComponent> GetAsync(Expression<Func<PRComponent, bool>> where)
        {
            return await repository.GetAsync(where);
        }
 
        #endregion
    }//
}
