using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using PhonebookApi.Models;

namespace PhonebookApi.Services
{
    public interface IPersonService : IBaseService<PersonViewModel, PersonFilter, Person>
    {
        int TotalCount(Filter filter);
        Task<int> TotalCountAsync(Filter filter);
    }

    public class PersonService : BaseService<PersonViewModel, PersonFilter, Person>, IPersonService
    {
        public PersonService(ILocator locator) : base(locator)
        {
        }

        public override IList<PersonViewModel> GetRange(PersonFilter filter)
        {
            return GetBaseRangeFor(RepoUoW.PersonRepository.GetRange(filter), filter)
                .ToList()
                .Select(Mapper.Map)
                .ToList();
        }

        public override async Task<IList<PersonViewModel>> GetRangeAsync(PersonFilter filter)
        {
            return (await GetBaseRangeFor(RepoUoW.PersonRepository.GetRange(filter), filter)
                .ToListAsync())
                .Select(Mapper.Map)
                .ToList();
        }

        protected override IQueryable<Person> GetBaseRangeFor(IQueryable<Person> range, PersonFilter filter)
        {
            return GetBaseRange(range, filter);
        }

        protected IQueryable<Person> GetBaseRange(IQueryable<Person> range, PersonFilter filter)
        {
            if (!string.IsNullOrEmpty(filter.Query))
                range = range.Where(x => x.Name.Contains(filter.Query)
                || x.Phone.Contains(filter.Query));

            return range.Skip(filter.Offset).Take(filter.Limit);
        }

        public int TotalCount(Filter filter)
        {
            return GetBaseRangeFor(Repository.GetRange(), new PersonFilter()
            {
                Limit = base.TotalCount(),
                Offset = 0,
                Query = filter.Query
            }).Count();
        }

        public async Task<int> TotalCountAsync(Filter filter)
        {
            return await GetBaseRangeFor(Repository.GetRange(), new PersonFilter()
            {
                Limit = await base.TotalCountAsync(),
                Offset = 0,
                Query = filter.Query
            }).CountAsync();
        }
    }
}