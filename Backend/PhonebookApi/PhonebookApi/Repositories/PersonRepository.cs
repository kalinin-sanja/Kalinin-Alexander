using System.Linq;
using PhonebookApi.Models;

namespace PhonebookApi.Repositories
{
    public interface IPersonRepository : IRepository<Person>
    {
        IQueryable<Person> GetRange(PersonFilter filter);
    }

    public class PersonRepository : Repository<Person>, IPersonRepository
    {
        public PersonRepository(PhonebookDbContext context, IRepositoryUoW repositoryUoW) : base(context, repositoryUoW)
        {
        }

        public IQueryable<Person> GetRange(PersonFilter filter)
        {
            if (filter.OrderByDesc)
                return Context.Set<Person>().AsQueryable().OrderByDescending(x => x.Name);

            return Context.Set<Person>().AsQueryable().OrderBy(x => x.Name);

        }
    }
}