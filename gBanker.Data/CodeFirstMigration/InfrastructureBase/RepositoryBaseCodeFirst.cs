using BasicDataAccess;
using gHRM.Data.CodeFirstMigration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.CodeFirstMigration.InfrastructureBase
{
  
   public abstract class RepositoryBaseCodeFirst<T> where T : class
   {
       private gHRMDBContext dataContext;
       private readonly IDbSet<T> dbset;
       private readonly IDbSet<List<T>> dbsets;
       protected RepositoryBaseCodeFirst(IDatabaseFactoryCodeFirst databaseFactory)
       {
           DatabaseFactory = databaseFactory;
           dbset = DataContext.Set<T>();
           dbsets = DataContext.Set<List<T>>();
       }

       protected IDatabaseFactoryCodeFirst DatabaseFactory
       {
           get;
           private set;
       }

       protected gHRMDBContext DataContext
       {
           get { return dataContext ?? (dataContext = DatabaseFactory.Get()); }
           
       }
       public virtual void Add(T entity)
       {
           dbset.Add(entity);
       }

       public virtual void AddRange(List<T> entities)
       {
           dbsets.Add(entities);
           dataContext.Entry(entities).State = EntityState.Added;
       }
       public virtual void Update(T entity)
       {

           dbset.Attach(entity);
           dataContext.Entry(entity).State = EntityState.Modified;
       }
       public virtual void UpdateConditional(Expression<Func<T, bool>> where)
       {


       }

       public virtual void Delete(T entity)
       {
           dbset.Remove(entity);
       }
       public virtual void Delete(Expression<Func<T, bool>> where)
       {
           IEnumerable<T> objects = dbset.Where<T>(where).AsEnumerable();
           foreach (T obj in objects)
               dbset.Remove(obj);
       }
       public virtual T GetById(long id)
       {
           return dbset.Find(id);
       }
       public virtual T GetById(string id)
       {
           return dbset.Find(id);
       }
       public virtual IEnumerable<T> GetAll()
       {

          return dbset.ToList();
       }


       public virtual IEnumerable<T> GetMany(Expression<Func<T, bool>> where)
       {
           
           return dbset.Where(where).ToList();
       }

      

       public T Get(Expression<Func<T, bool>> where)
       {
           return dbset.Where(where).FirstOrDefault<T>();
       }

        public List<TTarget> GetSqlResult<TTarget, TParam>(string sql, TParam param) where TTarget : class
        {
            var paramList = CreateDbParmetersFromEntity(param);
            return dataContext.Database.SqlQuery<TTarget>(sql, paramList.ToArray()).ToList();
        }

        #region Async
        public async Task<IEnumerable<T>> GetAllAsync()
       {
           return await dbset.ToListAsync();
       }
       public async Task<IEnumerable<T>> GetManyAsync(Expression<Func<T, bool>> where)
       {
           return await dbset.Where(where).ToListAsync();
       }
       public async Task<T> GetAsync(Expression<Func<T, bool>> where)
       {
           return await dbset.Where(where).FirstOrDefaultAsync();
       }
        #endregion Async

        #region SQL Parameters
        protected List<SqlParameter> CreateDbParmetersFromEntity<TTarget>(TTarget target)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                //Get all properties for the given type.
                var props = new List<PropertyInfo>(DataWrapper.GetSourceProperties(typeof(TTarget)));

                //for anonymous type, Object is passed which does not have any properties. So work with the anonymous type itselt.
                if (props.Count == 0)
                    props = new List<PropertyInfo>(DataWrapper.GetSourceProperties(target.GetType()));

                foreach (var p in props)
                {
                    //default property name is column name for the parameter.
                    var columnName = p.Name;
                    var value = p.GetValue(target, null);

                    var attr = p.GetCustomAttributes(true);
                    var added = false;

                    //If still not added in the parameter list, then add as the default input parameter type.
                    if (!added)
                    {
                        if (p.PropertyType.IsValueType || p.PropertyType == typeof(string))
                            parameters.Add(CreateParameter(columnName, value));
                        //else if(IsNullable(value))
                        //{
                        //    parameters.Add(CreateParameter(columnName, value));
                        //}
                    }
                }
                return parameters;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        private bool IsNullable<TType>(TType value)
        {
            return Nullable.GetUnderlyingType(typeof(TType)) != null;
        }

        private SqlParameter CreateParameter(string parameterName, object parameterValue)
        {
            return new SqlParameter { Value = parameterValue, ParameterName = parameterName };
        }

        private SqlParameter CreateParameter(string parameterName, object parameterValue, ParameterDirection direction)
        {
            return new SqlParameter { Value = parameterValue, ParameterName = parameterName, Direction = direction };
        }
        #endregion
    }
}
