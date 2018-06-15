using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace GenericRepository
{
    public class Repository<T> : IRepository where T : class

    {
        private DbContext _context;

        // constructor should be internal in order to keep the class from 
        // being instantiated outside the assembly.
        internal Repository(DbContext context)
        {
            _context = context;
        }

        public void Add(T entity)
        {
            _context.Set<T>().Add(entity);
        }

        public IEnumerable<T> Get(Expression<Func<T, bool>> expr = null,
                string include = null)
        {
            var query = _context.Set<T>().AsQueryable();
            if(!string.IsNullOrEmpty(include))
            {
                query =  query.Include(include);
            }
            return query.AsEnumerable();
        }

        public T GetById(Expression<Func<T, bool>> expr, 
                string include = null)
        {
            return Get(expr, include)
                    .FirstOrDefault();
        }

        public void Remove(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public void Update(T entity)
        {
            _context.Entry<T>(entity).State = EntityState.Modified;
        }
    }
}
