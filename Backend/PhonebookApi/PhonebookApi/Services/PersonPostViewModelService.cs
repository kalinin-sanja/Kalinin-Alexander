using PhonebookApi.Models;

namespace PhonebookApi.Services
{
    public interface IPersonPostViewModelService : IBaseService<PersonPostViewModel, Filter, Person>
    {
    }

    public class PersonPostViewModelService : BaseService<PersonPostViewModel, Filter, Person>, IPersonPostViewModelService
    {
        public PersonPostViewModelService(ILocator locator) : base(locator)
        {
        }
    }
}