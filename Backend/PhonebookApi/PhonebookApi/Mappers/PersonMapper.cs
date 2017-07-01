using PhonebookApi.Models;

namespace PhonebookApi.Mappers
{
    public interface IPersonMapper : IAutoFullModelMapper<Person, PersonViewModel> { }

    public class PersonMapper : AutoFullModelMapper<Person, PersonViewModel>, IPersonMapper
    {
        public PersonMapper(ILocator locator) : base(locator)
        {
        }
    }
}