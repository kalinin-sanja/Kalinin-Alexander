using PhonebookApi.Repositories;

namespace PhonebookApi.Mappers
{
    public interface IModelMapper<in TIn, out TOut>
    {
        TOut Map(TIn dbEntry);
        TOut MapFull(TIn entry);
    }

    public interface IInverseModelMapper<TIn, TOut>
    {
        TOut InverseMap(TIn entry);
    }

    public interface IFullInverseModelMapper<TIn, TOut> : IInverseModelMapper<TIn, TOut>
    {
        TOut InverseMapFull(TIn entry);
    }

    public abstract class ModelMapper<TIn, TOut> : IModelMapper<TIn, TOut>
    {
        public ModelMapper(ILocator locator)
        {
            RepoUoW = locator.RepoUoW;
            MapperUoW = locator.MapperUoW;
        }

        protected IRepositoryUoW RepoUoW { get; }
        protected IMapperUoW MapperUoW { get; }

        public abstract TOut Map(TIn dbEntry);
        public virtual TOut MapFull(TIn entry)
        {
            return Map(entry);
        }
    }

    public interface IFullModelMapper<TIn, TOut> : IModelMapper<TIn, TOut>, IFullInverseModelMapper<TOut, TIn> { }

    public abstract class FullModelMapper<TIn, TOut> : ModelMapper<TIn, TOut>, IFullModelMapper<TIn, TOut>
    {
        public FullModelMapper(ILocator locator) : base(locator)
        {
        }

        public abstract TIn InverseMap(TOut entry);
        public abstract TIn InverseMapFull(TOut entry);
    }

    public interface IAutoFullModelMapper<TIn, TOut> : IFullModelMapper<TIn, TOut>
        where TIn : new()
        where TOut : new()
    {
    }

    public abstract class AutoFullModelMapper<TIn, TOut> : FullModelMapper<TIn, TOut>, IAutoFullModelMapper<TIn, TOut>
        where TIn : class, new()
        where TOut : class, new()
    {
        public AutoFullModelMapper(ILocator locator) : base(locator)
        {
            AutoMapper = new AutoMapper(locator.MapperUoW);
        }

        protected IAutoMapper AutoMapper { get; }

        public override TOut Map(TIn dbEntry)
        {
            return AutoMapper.Map<TIn, TOut>(dbEntry);
        }

        public override TOut MapFull(TIn entry)
        {
            return AutoMapper.MapFull<TIn, TOut>(entry);
        }

        public override TIn InverseMap(TOut entry)
        {
            return AutoMapper.Map<TOut, TIn>(entry, null, "InverseMap");
        }

        public override TIn InverseMapFull(TOut entry)
        {
            return AutoMapper.MapFull<TOut, TIn>(entry, null, "InverseMap");
        }
    }
}