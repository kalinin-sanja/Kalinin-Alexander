using PhonebookApi.Mappers;
using PhonebookApi.Models;
using PhonebookApi.Repositories;
using PhonebookApi.Services;

namespace PhonebookApi
{
    public interface ILocator
    {
        IRepositoryUoW RepoUoW { get; }
        IServiceUoW ServiceUoW { get; }
        IMapperUoW MapperUoW { get; }
    }

    public class Locator : ILocator
    {
        public Locator(PhonebookDbContext context)
        {
            Context = context;
        }

        protected PhonebookDbContext Context { get; }

        public IRepositoryUoW RepoUoW => new RepositoryUoW(Context);
        public IServiceUoW ServiceUoW => new ServiceUoW(this);
        public IMapperUoW MapperUoW => new MapperUoW(this);
    }
}