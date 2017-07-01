using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using PhonebookApi.Mappers;
using PhonebookApi.Models;

namespace PhonebookApi.Repositories
{
    public interface IBaseRepository<T> where T : class
    {
        IQueryable<T> GetRange();
        T Add(T entry);
        ICollection<T> AddRange(ICollection<T> entries);
        T Update(T entry);
        ICollection<T> UpdateRange(ICollection<T> entries);
        void Remove(T entry);
        void RemoveRange(ICollection<T> entries);
    }

    public class BaseRepository<T> : IBaseRepository<T>
        where T : class
    {
        public BaseRepository(PhonebookDbContext context, IRepositoryUoW repositoryUoW)
        {
            Context = context;
            RepoUoW = repositoryUoW;
        }

        protected PhonebookDbContext Context { get; }
        protected IRepositoryUoW RepoUoW { get; }

        public virtual IQueryable<T> GetRange()
        {
            return Context.Set<T>().AsQueryable();
        }

        public virtual T Add(T entry)
        {
            return Context.Set<T>().Add(entry);
        }

        public ICollection<T> AddRange(ICollection<T> entries)
        {
            return entries.Select(Add).ToList();
        }

        public virtual T Update(T entry)
        {
            var dbEntry = Context.Set<T>().Attach(entry);
            Context.Entry(entry).State = EntityState.Modified;
            return dbEntry;
        }

        public ICollection<T> UpdateRange(ICollection<T> entries)
        {
            return entries.Select(Update).ToList();
        }

        public virtual void Remove(T entry)
        {
            Context.Set<T>().Remove(entry);
        }

        public void RemoveRange(ICollection<T> entries)
        {
            foreach (var entry in entries)
            {
                Remove(entry);
            }
        }
    }

    public interface IRepository<T> : IBaseRepository<T>
        where T : class, IIdentityBase, new()
    {
        T Get(long id);
        void Remove(long id);
        IQueryable<T> FromSql(string sql, params object[] parameters);
        Task<T> GetAsync(long id);
        Task<T> UpdateAsync(T entry);
        Task RemoveAsync(long id);
        Task RemoveAsync(T entry);
    }

    public class Repository<T> : BaseRepository<T>, IRepository<T>
        where T : class, IIdentityBase, new()
    {
        public Repository(PhonebookDbContext context, IRepositoryUoW repositoryUoW) : base(context, repositoryUoW)
        {
        }

        protected AutoDbMapper AutoMapper => new AutoDbMapper();

        public override IQueryable<T> GetRange()
        {
            return base.GetRange().OrderBy(x => x.Id);
        }

        public virtual T Get(long id)
        {
            return Context.Set<T>().SingleOrDefault(x => x.Id == id);
        }

        public virtual Task<T> GetAsync(long id)
        {
            return Context.Set<T>().SingleOrDefaultAsync(x => x.Id == id);
        }

        public override T Add(T entry)
        {
            entry.CreatingTime = DateTime.UtcNow;
            return base.Add(entry);
        }

        public override T Update(T entry)
        {
            var dbEntry = Get(entry.Id);
            dbEntry = AutoMapper.MapForDb(entry, dbEntry);
            return base.Update(dbEntry);
        }

        public async Task<T> UpdateAsync(T entry)
        {
            var dbEntry = await GetAsync(entry.Id);
            dbEntry = AutoMapper.MapForDb(entry, dbEntry);
            return base.Update(dbEntry);
        }

        public virtual void Remove(long id)
        {
            var entry = Get(id);
            base.Remove(entry);
            Context.SaveChanges();
        }

        public virtual async Task RemoveAsync(long id)
        {
            var entry = await GetAsync(id);
            base.Remove(entry);
            await Context.SaveChangesAsync();
        }

        public IQueryable<T> FromSql(string sql, params object[] parameters)
        {
            return Context.Set<T>().SqlQuery(sql, parameters).AsQueryable();
        }

        public override void Remove(T entry)
        {
            var dbEntry = Get(entry.Id);
            base.Remove(dbEntry);
        }

        public async Task RemoveAsync(T entry)
        {
            var dbEntry = await GetAsync(entry.Id);
            base.Remove(dbEntry);
        }
    }
}