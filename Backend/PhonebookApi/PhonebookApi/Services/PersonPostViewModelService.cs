using PhonebookApi.Models;

namespace PhonebookApi.Services
{
    public interface IPersonPostViewModelService : IBaseService<PersonPostViewModel, Filter, Person>
    {
        bool ExistByPhone(string phone, long? exceptPersonId);
    }

    public class PersonPostViewModelService : BaseService<PersonPostViewModel, Filter, Person>, IPersonPostViewModelService
    {
        public PersonPostViewModelService(ILocator locator) : base(locator)
        {
        }

        public bool ExistByPhone(string phone, long? exceptPersonId)
        {
            return RepoUoW.PersonRepository.ExistByPhone(phone, exceptPersonId);
        }
    }
}