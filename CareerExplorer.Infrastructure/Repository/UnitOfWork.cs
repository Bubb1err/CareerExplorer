using CareerExplorer.Core.Interfaces;
using CareerExplorer.Infrastructure.Data;
using Microsoft.Extensions.DependencyInjection;

namespace CareerExplorer.Infrastructure.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly AppDbContext _db;
        private Dictionary<Type, object> _repositories;

        public UnitOfWork(AppDbContext db, IServiceProvider serviceProvider)
        {
            _db = db;
            _serviceProvider = serviceProvider;
            _repositories = new Dictionary<Type, object>();
        }
        public IRepository<T> GetRepository<T>() where T : class
        {
            if (_repositories.ContainsKey(typeof(T)))
            {
                return (IRepository<T>)_repositories[typeof(T)];
            }

            var repository = _serviceProvider.GetService<IRepository<T>>();
            _repositories.Add(typeof(T), repository);
            return repository;
        }
        public void Dispose()
        {
            _db.Dispose();
        }

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
