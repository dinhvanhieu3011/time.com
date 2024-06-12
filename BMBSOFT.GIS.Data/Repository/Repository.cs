using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BASE.Entity;
using Microsoft.EntityFrameworkCore;

namespace BASE.Data.Repository
{
    public class Repository<TEntity,T> : IRepository<TEntity,T> where TEntity : BaseEntity<T>
    {
        protected DbSet<TEntity> Dbset;
        protected readonly AppDbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public Repository(AppDbContext context)
        {
            _dbContext = context;
            Dbset = context.Set<TEntity>();
        }

        /// <summary>
        /// Function use to get Object flow Id
        /// </summary>
        /// <param name="id">Primary key of Table current</param>
        /// <returns></returns>
        public virtual TEntity GetById(T id)
        {
            return Dbset.Find(id);
        }

        public virtual TEntity GetByIdInclude(T id, params Expression<Func<TEntity, object>>[] includes)
        {
            return Find(x => x.Id.Equals(id), includes);
        }

        public async Task<TEntity> GetAsyncById(T id)
        {
            return await Dbset.FindAsync(id);
        }
        /// <summary>
        /// Get All list Object
        /// </summary>
        /// <returns></returns>
        public IQueryable<TEntity> GetAll()
        {
            return Dbset.AsQueryable();
        }

        /// <summary>
        /// Get All list include next Object
        /// </summary>
        /// <returns></returns>
        public IQueryable<TEntity> GetAllInclude(params Expression<Func<TEntity, object>>[] includes)
        {
            var query = Dbset.AsQueryable();

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }
            return query;
        }

        /// <summary>
        /// Get All list Object
        /// </summary>
        /// <returns></returns>
        //public IQueryable<T> GetCommunceByDistrictId(int districtId)
        //{
        //    return Dbset.AsQueryable();
        //}

        public IList<TEntity> GetListAllAsync()
        {
            return Dbset
                .AsQueryable()
                .ToList();
        }


        /// <summary>
        /// Function use in the case Query have condition
        /// </summary>
        /// <param name="filter">Condition of query</param>
        /// <returns></returns>
        public IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> filter)
        {
            return Dbset.Where(filter);
        }

        /// <summary>
        /// Function use to Update Object 
        /// </summary>
        /// <param name="entity">Object is targer Update</param>
        /// <returns></returns>
        public virtual TEntity Update(TEntity entity)
        {
            var dbEntityEntry = _dbContext.Entry(entity);
            if (dbEntityEntry.State == EntityState.Detached)
            {
                Dbset.Attach(entity);
            }

            dbEntityEntry.State = EntityState.Modified;
            return entity;
        }


        public List<TEntity> UpdateMulti(List<TEntity> listItem)
        {
            foreach (var item in listItem)
            {
                Update(item);
            }
            return listItem;
        }
        

        /// <summary>
        /// Function use to Insert Object 
        /// </summary>
        /// <param name="entity">Object is targer Update</param>
        /// <returns></returns>
        public virtual TEntity Insert(TEntity entity)
        {
            Dbset.Add(entity);

            return entity;
        }

        public virtual async Task<TEntity> InsertAsync(TEntity entity)
        {
            await Dbset.AddAsync(entity);

            return entity;
        }

        public List<TEntity> InsertMulti(List<TEntity> entity)
        {
            foreach (var item in entity)
            {
                Dbset.Add(item);
            }
            return entity;
        }

        /// <summary>
        /// Function use to Remove Object in Database
        /// </summary>
        /// <param name="entity">Object is targer Update</param>
        /// <returns></returns>
        public bool Delete(TEntity entity)
        {
            try
            {
                var entry = _dbContext.Entry(entity);
                entry.State = EntityState.Deleted;
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public virtual bool Delete(T id)
        {
            var entity = GetById(id);
            if (entity == null)
            {
                return false;
            }

            Delete(entity);
            return true;
        }

        public bool DeleteMulti(List<TEntity> entity)
        {
            try
            {
                foreach (var item in entity)
                {
                    Delete(item);
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public TEntity Find(Expression<Func<TEntity, bool>> expression, params Expression<Func<TEntity, object>>[] includes)
        {
            var query = Dbset.AsQueryable();

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }
            query = query.Where(expression);
            return query.FirstOrDefault();
        }

        public List<TEntity> FindAll(Expression<Func<TEntity, bool>> expression, params Expression<Func<TEntity, object>>[] includes)
        {
            var query = Dbset.AsQueryable();
            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }
            if (expression != null)
            {
                query = query.Where(expression);
            }
            return query.ToList();
        }
    }
}
