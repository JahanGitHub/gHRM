using gHRM.Core.Common;
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.CodeFirstMigration.Payroll;
using gHRM.Data.Repository;
using gHRM.Data.Repository.Payroll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Service.Payroll
{
    public interface IFestivalBonusCalendarService : IServiceBase<FestivalBonusCalendar>
    {


    }
    public class FestivalBonusCalendarService : IFestivalBonusCalendarService
    {
        private readonly IFestivalBonusCalendarRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public FestivalBonusCalendarService(IFestivalBonusCalendarRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        public IEnumerable<FestivalBonusCalendar> GetAll()
        {
            var entities = repository.GetAll().Where(c => c.IsActive == 1).OrderBy(c => c.Id);
            return entities;
        }

        public FestivalBonusCalendar GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public FestivalBonusCalendar Create(FestivalBonusCalendar objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(FestivalBonusCalendar objectToUpdate)
        {
            repository.Update(objectToUpdate);
            Save();
        }

        public void Delete(int id)
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
        public FestivalBonusCalendar Get(Expression<Func<FestivalBonusCalendar, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<FestivalBonusCalendar> GetMany(Expression<Func<FestivalBonusCalendar, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == 1);
            return entities;
        }

        #region Asyc
        public virtual async Task<IEnumerable<FestivalBonusCalendar>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }


        public virtual async Task<IEnumerable<FestivalBonusCalendar>> GetManyAsync(Expression<Func<FestivalBonusCalendar, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }



        public virtual async Task<FestivalBonusCalendar> GetAsync(Expression<Func<FestivalBonusCalendar, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion
        }
    }

