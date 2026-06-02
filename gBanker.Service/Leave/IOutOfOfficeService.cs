using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Service
{
    public  interface IOutOfOfficeService : IServiceBase<OutOfOffice>
    {

    }


    public class OutOfOfficeService : IOutOfOfficeService
    {

        private readonly IOutOfOfficeRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public OutOfOfficeService(IOutOfOfficeRepository repository,  IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
    }


        public OutOfOffice Create(OutOfOffice objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public OutOfOffice Get(Expression<Func<OutOfOffice, bool>> where)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<OutOfOffice> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<OutOfOffice>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<OutOfOffice> GetAsync(Expression<Func<OutOfOffice, bool>> where)
        {
            throw new NotImplementedException();
        }

        public OutOfOffice GetById(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<OutOfOffice> GetMany(Expression<Func<OutOfOffice, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }

        public Task<IEnumerable<OutOfOffice>> GetManyAsync(Expression<Func<OutOfOffice, bool>> where)
        {
            throw new NotImplementedException();
        }

        public bool Inactivate(long id, DateTime? inactiveDate)
        {
            throw new NotImplementedException();
        }

        public bool IsContinued(long id)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            unitOfWork.Commit();
        }

        public void Update(OutOfOffice objectToUpdate)
        {
            throw new NotImplementedException();
        }
    }
}
