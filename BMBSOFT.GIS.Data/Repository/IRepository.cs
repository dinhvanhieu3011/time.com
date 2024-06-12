using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BASE.Data.Repository
{
    public interface IRepository<TEntity,T>
    {
        /// <summary>
        /// Function use to get Object flow Id
        /// </summary>
        /// <param name="id">Primary key of Table current</param>
        /// <returns></returns>
        TEntity GetById(T id);

        TEntity GetByIdInclude(T id, params Expression<Func<TEntity, object>>[] includes);

        Task<TEntity> GetAsyncById(T id);

        /// <summary>
        /// Get All list Object
        /// </summary>
        /// <returns></returns>
        IQueryable<TEntity> GetAll();

        /// <summary>
        /// Get All list include next Object
        /// </summary>
        /// <returns></returns>
        IQueryable<TEntity> GetAllInclude(params Expression<Func<TEntity, object>>[] includes);

        /// <summary>
        /// Get All list Object
        /// </summary>
        /// <returns></returns>
        //IQueryable<T> GetCommunceByDistrictId(int districtId);

        IList<TEntity> GetListAllAsync();

        /// <summary>
        /// Function use in the case Query have condition
        /// </summary>
        /// <param name="filter">Condition of query</param>
        /// <returns></returns>
        IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> filter);

        /// <summary>
        /// Function use to Update Object 
        /// </summary>
        /// <param name="entity">Object is targer Update</param>
        /// <returns></returns>
        TEntity Update(TEntity entity);

        List<TEntity> UpdateMulti(List<TEntity> listItem);

        /// <summary>
        /// Function use to Insert Object 
        /// </summary>
        /// <param name="entity">Object is targer Update</param>
        /// <returns></returns>
        TEntity Insert(TEntity entity);

        Task<TEntity> InsertAsync(TEntity entity);

        /// <summary>
        /// Inserts the multiple entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        List<TEntity> InsertMulti(List<TEntity> entity);

        /// <summary>
        /// Function use to Remove Object in Database
        /// </summary>
        /// <param name="entity">Object is targer Update</param>
        /// <returns></returns>
        bool Delete(TEntity entity);

        /// <summary>
        /// Function use to Remove Object in Database
        /// </summary>
        /// <param name="id">Id is identity</param>
        /// <returns></returns>
        bool Delete(T id);

        /// <summary>
        /// Deletes the mullti.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        bool DeleteMulti(List<TEntity> entity);

        TEntity Find(Expression<Func<TEntity, bool>> expression, params Expression<Func<TEntity, object>>[] includes);
        List<TEntity> FindAll(Expression<Func<TEntity, bool>> expression, params Expression<Func<TEntity, object>>[] includes);
    }
}
