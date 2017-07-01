using PhonebookApi.Models;

namespace PhonebookApi.Mappers
{
    public interface IPersonPostViewMapper : IAutoFullModelMapper<Person, PersonPostViewModel> { }

    public class PersonPostViewMapper : AutoFullModelMapper<Person, PersonPostViewModel>, IPersonPostViewMapper
    {
        public PersonPostViewMapper(ILocator locator) : base(locator)
        {
        }
    }
}