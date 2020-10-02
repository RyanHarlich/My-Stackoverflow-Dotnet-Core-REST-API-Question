using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.DependencyInjection;
using Library.Models.HyperMediaControls;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Library.Data
{
    public interface IUnitOfWork
    {
        IRepository<T> Repository<T>() where T : class;
        Task SaveChangesAsync();
        DbContext DBContext { get; }
        APIModelExample.MasterNode StartMasterNode { get; }
    }

    internal class UnitOfWork : IUnitOfWork
    {
        private Hashtable _repositories;

        public UnitOfWork(DbContext dbContext,  APIModelExample.MasterNode startMasterNode)
        {
            DBContext = dbContext;
            StartMasterNode = startMasterNode;
        }

        public DbContext DBContext { get; protected set; }

        public APIModelExample.MasterNode StartMasterNode { get; protected set; }

        public IRepository<T> Repository<T>() where T : class
        {
            if (_repositories == null)
                _repositories = new Hashtable();

            var type = typeof(T).Name;

            if (!_repositories.ContainsKey(type))
            {
                var repositoryType = typeof(Repository<>);

                var repositoryInstance =
                    Activator.CreateInstance(repositoryType.MakeGenericType(typeof(T)), DBContext);

                _repositories.Add(type, repositoryInstance);
            }

            return (IRepository<T>)_repositories[type];
        }

        public async Task SaveChangesAsync()
        {
            await DBContext.SaveChangesAsync();
        }
    }

    public interface IRepository<TEntity> where TEntity : class
    {
        Task InsertEntityAsync(TEntity entity);
        Task<List<TEntity>> ToListAsync();
        Task<TEntity> FirstOrDefault();
        void Empty(TEntity entity);
    }

    internal class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly DbContext _dbContext;
        
        public Repository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task InsertEntityAsync(TEntity entity) => await _dbContext.Set<TEntity>().AddAsync(entity);

        public async Task<List<TEntity>> ToListAsync() => await _dbContext.Set<TEntity>().ToListAsync();

        public async Task<TEntity> FirstOrDefault() => await _dbContext.Set<TEntity>().FirstOrDefaultAsync();

        public void Empty(TEntity entity) => _dbContext.Set<TEntity>().Remove(entity);
    }

    public static class ServiceCollectionExtensions
    {
        public static void RegisterYourLibrary(this IServiceCollection services, DbContext dbContext,  APIModelExample.MasterNode startMasterNode)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            services.AddScoped<IUnitOfWork, UnitOfWork>(uow => new UnitOfWork(dbContext, startMasterNode));
        }
    }
}
