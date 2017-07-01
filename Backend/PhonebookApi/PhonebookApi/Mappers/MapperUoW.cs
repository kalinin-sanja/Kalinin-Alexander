using System.Linq;

namespace PhonebookApi.Mappers
{
    public interface IMapperUoW
    {
        IPersonMapper PersonMapper { get; }
        IPersonPostViewMapper PersonPostViewMapper { get; }
        IGroupMapper GroupMapper { get; }
        IFullModelMapper<TIn, TOut> GetFullMapper<TIn, TOut>();
    }

    public class MapperUoW : IMapperUoW
    {
        protected ILocator Locator { get; }

        public MapperUoW(ILocator locator)
        {
            Locator = locator;
        }

        public IFullModelMapper<TIn, TOut> GetFullMapper<TIn, TOut>()
        {
            var mapperType = typeof(IFullModelMapper<TIn, TOut>);
            var property = typeof(MapperUoW).GetProperties()
                .FirstOrDefault(x => mapperType.IsAssignableFrom(x.PropertyType));
            return property?.GetValue(this) as IFullModelMapper<TIn, TOut>;
        }

        public IPersonMapper PersonMapper => new PersonMapper(Locator);
        public IPersonPostViewMapper PersonPostViewMapper => new PersonPostViewMapper(Locator);
        public IGroupMapper GroupMapper => new GroupMapper(Locator);
    }
}