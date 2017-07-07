using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using PhonebookApi.Mappers;
using PhonebookApi.Models;
using PhonebookApi.Repositories;

namespace PhonebookApi.Services
{
    public interface IBaseService
    {
    }

    public abstract class BaseService : IBaseService
    {
        protected BaseService(ILocator locator)
        {
            ServiceUoW = locator.ServiceUoW;
            RepoUoW = locator.RepoUoW;
            MapperUoW = locator.MapperUoW;
        }

        protected IServiceUoW ServiceUoW { get; }
        public IRepositoryUoW RepoUoW { get; }
        public IMapperUoW MapperUoW { get; }
    }

    public interface IBaseService<T, TFilter> : IBaseService
        where TFilter : Filter
    {
        T Get(long id);
        T GetFull(long id);
        IList<T> GetRange(TFilter filter);
        int TotalCount();
        long Add(T entry);
        void Update(T entry);
        void Remove(long id);
    }

    public abstract class BaseService<T, TFilter> : BaseService, IBaseService<T, TFilter>
        where TFilter : Filter
    {
        public BaseService(ILocator locator) : base(locator)
        {
        }

        public abstract T Get(long id);

        public virtual T GetFull(long id)
        {
            return Get(id);
        }

        public abstract IList<T> GetRange(TFilter filter);
        public abstract int TotalCount();
        public abstract long Add(T entry);
        public abstract void Update(T entry);
        public abstract void Remove(long id);

        protected virtual IQueryable<TDbModel> GetBaseRange<TDbModel>(IQueryable<TDbModel> range, Filter filter)
            where TDbModel : class, IIdentified
        {
            return range
                .Skip(filter.Offset)
                .Take(filter.Limit);
        }
    }

    public interface IBaseService<T, TFilter, TDbEntity> : IBaseService<T, TFilter>
        where TFilter : Filter
        where TDbEntity : class, IIdentityBase
    {
        Task<T> GetAsync(long id);
        Task<T> GetFullAsync(long id);
        Task<IList<T>> GetRangeAsync(TFilter filter);
        Task<int> TotalCountAsync();
        Task<long> AddAsync(T entry);
        Task UpdateAsync(T entry);
        Task RemoveAsync(long id);
    }

    public abstract class BaseService<T, TFilter, TDbEntity> : BaseService<T, TFilter>,
            IBaseService<T, TFilter, TDbEntity>
        where TFilter : Filter
        where TDbEntity : class, IIdentityBase, new()
        where T : class, new()
    {
        public BaseService(ILocator locator) : base(locator)
        {
            Repository = locator.RepoUoW.GetRepo<TDbEntity>();
            Mapper = locator.MapperUoW.GetFullMapper<TDbEntity, T>();
            AutoMapper = new AutoMapper(locator.MapperUoW);
        }

        protected IRepository<TDbEntity> Repository { get; }
        protected IFullModelMapper<TDbEntity, T> Mapper { get; }
        protected IAutoMapper AutoMapper { get; }

        public override T Get(long id)
        {
            var dbEntry = Repository.Get(id);
            if (dbEntry == null)
                return null;
            return Mapper.Map(dbEntry);
        }

        public async Task<T> GetAsync(long id)
        {
            var dbEntry = await Repository.GetAsync(id);
            return Mapper.Map(dbEntry);
        }

        public override T GetFull(long id)
        {
            var dbEntry = Repository.Get(id);
            return Mapper.MapFull(dbEntry);
        }

        public async Task<T> GetFullAsync(long id)
        {
            var dbEntry = await Repository.GetAsync(id);
            return Mapper.MapFull(dbEntry);
        }

        public override IList<T> GetRange(TFilter filter)
        {
            return GetBaseRangeFor(Repository.GetRange(), filter)
                .ToList()
                .Select(Mapper.Map)
                .ToList();
        }

        public virtual async Task<IList<T>> GetRangeAsync(TFilter filter)
        {
            return (await GetBaseRangeFor(Repository.GetRange(), filter)
                    .ToListAsync())
                .Select(Mapper.Map)
                .ToList();
        }

        public override int TotalCount()
        {
            return Repository.GetRange().Count();
        }

        public async Task<int> TotalCountAsync()
        {
            return await Repository.GetRange().CountAsync();
        }

        public override long Add(T entry)
        {
            var dbEntry = Mapper.InverseMap(entry);
            dbEntry = Repository.Add(dbEntry);
            RepoUoW.Save();
            return dbEntry.Id;
        }

        public async Task<long> AddAsync(T entry)
        {
            var dbEntry = Mapper.InverseMap(entry);
            dbEntry.CreatingTime = DateTime.UtcNow;
            dbEntry = Repository.Add(dbEntry);
            await RepoUoW.SaveAsync();
            return dbEntry.Id;
        }

        public override void Update(T entry)
        {
            var dbEntry = Mapper.InverseMap(entry);
            dbEntry.LastEditingTime = DateTime.UtcNow;
            dbEntry = Repository.Update(dbEntry);
            RepoUoW.Save();
        }

        public async Task UpdateAsync(T entry)
        {
            var dbEntry = Mapper.InverseMap(entry);
            dbEntry.LastEditingTime = DateTime.UtcNow;
            dbEntry = await Repository.UpdateAsync(dbEntry);
            await RepoUoW.SaveAsync();
        }

        public override void Remove(long id)
        {
            Repository.Remove(id);
            RepoUoW.Save();
        }

        public async Task RemoveAsync(long id)
        {
            await Repository.RemoveAsync(id);
            await RepoUoW.SaveAsync();
        }

        protected virtual IQueryable<TDbEntity> GetBaseRangeFor(IQueryable<TDbEntity> range, TFilter filter)
        {
            return GetBaseRange(range, filter);
        }

        protected new IQueryable<TDbModel> GetBaseRange<TDbModel>(IQueryable<TDbModel> range, Filter filter)
            where TDbModel : class, IIdentityBase
        {
            if (!string.IsNullOrEmpty(filter.Query))
                range = range.Where(x => x.Id.ToString() == filter.Query || x.Name.Contains(filter.Query));
            return base.GetBaseRange(range, filter);
        }
    }
}