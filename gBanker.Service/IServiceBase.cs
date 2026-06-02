using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Service
{
    public interface IServiceBase<T> where T : class
    {
       
        IEnumerable<T> GetAll();
        T GetById(int id);
        T Create(T objectToCreate);
        void Update(T objectToUpdate);
        T Get(Expression<Func<T, bool>> where);
        IEnumerable<T> GetMany(Expression<Func<T, bool>> where);
        void Delete(int id);
        bool Inactivate(long id, DateTime? inactiveDate);
        bool IsContinued(long id);
        void Save();
       // T GetByIdLong(long id);
        #region Async
        Task<IEnumerable<T>> GetAllAsync();

        Task<IEnumerable<T>> GetManyAsync(Expression<Func<T, bool>> where);
        Task<T> GetAsync(Expression<Func<T, bool>> where);
        #endregion Async
    }
}
