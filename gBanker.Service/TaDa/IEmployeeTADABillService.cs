using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.CodeFirstMigration.TaDa;
using gHRM.Data.Repository.TaDa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Service.TaDa
{
    public interface IEmployeeTADABillService : IServiceBase<EmployeeTADABill>
    {
        List<EmployeeTADABill> AddTADA(List<EmployeeTADABill> objs);
    }
    public class EmployeeTADABillService : IEmployeeTADABillService
    {
        private readonly IEmployeeTADABillRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public EmployeeTADABillService(IEmployeeTADABillRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<EmployeeTADABill> GetAll()
        {
            var entities = repository.GetAll().OrderBy(c => c.TADABillId);
            return entities;
        }

        public EmployeeTADABill GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public EmployeeTADABill Create(EmployeeTADABill objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(EmployeeTADABill objectToUpdate)
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
            throw new NotImplementedException(); ;
        }


        public bool IsContinued(long id)
        {
            throw new NotImplementedException();
        }

        public EmployeeTADABill Get(Expression<Func<EmployeeTADABill, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<EmployeeTADABill> GetMany(Expression<Func<EmployeeTADABill, bool>> where)
        {
            var entities = repository.GetMany(where);
            return entities;
        }

        #region Asyc
        public virtual async Task<IEnumerable<EmployeeTADABill>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }


        public virtual async Task<IEnumerable<EmployeeTADABill>> GetManyAsync(Expression<Func<EmployeeTADABill, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }



        public virtual async Task<EmployeeTADABill> GetAsync(Expression<Func<EmployeeTADABill, bool>> where)
        {
            return await repository.GetAsync(where);
        }

        public List<EmployeeTADABill> AddTADA(List<EmployeeTADABill> objs)
        {
            repository.AddTADA(objs);
            return objs;
        }

        #endregion

    }
}
