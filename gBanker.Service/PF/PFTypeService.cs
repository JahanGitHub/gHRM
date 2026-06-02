using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.CodeFirstMigration.PF;
using gHRM.Data.Repository.PF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Service.PF
{
    public interface IPFTypeService : IServiceBase<PFType>
    {
        IEnumerable<PFType> GetPFTypeByName(string pfTypeName);
        bool UpdatePFType(PFType objPFType);
    }
    public class PFTypeService : IPFTypeService
    {
        private readonly IPFTypeRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public PFTypeService(IPFTypeRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<PFType> GetAll()
        {
            var entities = repository.GetAll();
            return entities;
        }
        public PFType GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }
        public void Save()
        {
            // unitOfWork.Commit();
            unitOfWork.Commit();
        }
        public PFType Create(PFType objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        //Asad added for test
        public bool UpdatePFType(PFType objPFType)
        {
            bool result = false;
            try
            {
                result = repository.UpdatePFType(objPFType);
                Save();
            }
            catch
            {
            }
            return result;
        }

        public void Update(PFType objectToUpdate)
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
                //obj.InActiveDate = DateTime.Now;
                //obj.IsActive = false;
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
                //var isActive = obj.IsActive;
                //if (isActive == false)
                //{
                //    return false;
                //}
            }
            return true;
        }

        public PFType Get(Expression<Func<PFType, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<PFType> GetMany(Expression<Func<PFType, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsDeleted == false);
            return entities;
        }
        #region Asyc
        public virtual async Task<IEnumerable<PFType>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }
        public virtual async Task<IEnumerable<PFType>> GetManyAsync(Expression<Func<PFType, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }
        public virtual async Task<PFType> GetAsync(Expression<Func<PFType, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion
        public IEnumerable<PFType> GetPFTypes(string filterColumnName, string filterValue, int startRowIndex, string jtSorting, int pageSize, out long TotCount)
        {
            return repository.GetPFTypes(filterColumnName, filterValue, startRowIndex, jtSorting, pageSize, out TotCount);
        }
        public IEnumerable<PFType> GetPFTypeByName(string pfTypeName)
        {
            return repository.GetPFTypeByName(pfTypeName);
        }
    }
}
