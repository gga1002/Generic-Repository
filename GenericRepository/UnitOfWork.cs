using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace GenericRepository
{
    /// <summary>
    /// This class implements a Generic Singleton pattern for the 
    /// repositories of each entity mapped in the context. 
    /// this pattern avoids creation of more than one instance 
    /// of the same repository.  
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {

        private DbContext _context;
        IDictionary<string, IRepository> _repositories;

        public  UnitOfWork(DbContext context) 
        {
            _context = context;
            _context.Configuration.LazyLoadingEnabled = false;
            _repositories = new Dictionary<string, IRepository>();
        }
        

        /// <summary>
        /// If the instance requested does not exist
        /// it will be created and stored in Repositories
        /// in order to avoid repeated instances of the same repository 
        /// for a given entity.
        /// </summary>
        /// <typeparam name="T">Entity type for the expected repository </typeparam>
        /// <returns>Generic repository for T</returns>
        public Repository<T> Repository<T>() where T : class
        {
            var result = _repositories.Where(r => r.Key == typeof(T).Name)
                            .FirstOrDefault().Value;

            var  repository = (Repository<T>)Convert
                                .ChangeType(result, typeof(Repository<T>));
            if(repository == null)
            {
                repository = new Repository<T>(_context);
                _repositories.Add(typeof(T).Name, repository);
            }
            return repository;
        }
        /// <summary>
        /// Save all the changes in the context once all 
        /// the operation have been performed
        /// </summary>
        public void Complete()
        {
            _context.SaveChanges();
        }
    }
}
