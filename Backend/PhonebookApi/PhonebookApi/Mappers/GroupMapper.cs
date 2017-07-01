using PhonebookApi.Models;

namespace PhonebookApi.Mappers
{
    public interface IGroupMapper : IAutoFullModelMapper<Group, GroupViewModel> { }

    public class GroupMapper : AutoFullModelMapper<Group, GroupViewModel>, IGroupMapper
    {
        public GroupMapper(ILocator locator) : base(locator)
        {
        }
    }
}