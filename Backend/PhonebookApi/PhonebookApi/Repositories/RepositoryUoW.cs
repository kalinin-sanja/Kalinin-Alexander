using System;
using System.Linq;
using System.Threading.Tasks;
using PhonebookApi.Models;

namespace PhonebookApi.Repositories
{
    public interface IRepositoryUoW : IDisposable
    {
        PhonebookDbContext Context { get; }

        IPersonRepository PersonRepository { get; }
        IGroupRepository GroupRepository { get; }
        void Save();
        IRepository<T> GetRepo<T>() where T : class, IIdentityBase, new();
        IBaseRepository<T> GetBaseRepo<T>() where T : class;
        Task SaveAsync();
    }

    public class RepositoryUoW : IRepositoryUoW
    {
        public PhonebookDbContext Context { get; }
        public IPersonRepository PersonRepository => new PersonRepository(Context, this);
        public IGroupRepository GroupRepository => new GroupRepository(Context, this);

        public RepositoryUoW(PhonebookDbContext context)
        {
            Context = context;
        }

        public void Save()
        {
            Context.SaveChanges();
        }

        public async Task SaveAsync()
        {
            await Context.SaveChangesAsync();
        }

        public IRepository<T> GetRepo<T>() where T : class, IIdentityBase, new()
        {
            var repoType = typeof(IRepository<T>);
            var property = typeof(RepositoryUoW).GetProperties()
                .FirstOrDefault(x => repoType.IsAssignableFrom(x.PropertyType));
            return property?.GetValue(this) as IRepository<T>;
        }

        public IBaseRepository<T> GetBaseRepo<T>() where T : class
        {
            return new BaseRepository<T>(Context, this);
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    Context.Dispose();
                }
            }
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}