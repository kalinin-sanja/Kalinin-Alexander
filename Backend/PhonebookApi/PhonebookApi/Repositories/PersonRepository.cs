using System.Linq;
using PhonebookApi.Models;

namespace PhonebookApi.Repositories
{
    public interface IPersonRepository : IRepository<Person>
    {
        IQueryable<Person> GetRange(PersonFilter filter);
        bool ExistByPhone(string phone, long? exceptPersonId);
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

        public bool ExistByPhone(string phone, long? exceptPersonId)
        {
            if (exceptPersonId != null)
                return GetRange().Any(x => x.Phone == phone && x.Id != exceptPersonId.Value);

            return GetRange().Any(x => x.Phone == phone);
        }
    }
}