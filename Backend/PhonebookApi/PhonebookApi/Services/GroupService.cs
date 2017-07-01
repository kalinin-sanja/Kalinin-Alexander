using PhonebookApi.Models;

namespace PhonebookApi.Services
{
    public interface IGroupService : IBaseService<GroupViewModel, Filter, Group>
    {
    }

    public class GroupService : BaseService<GroupViewModel, Filter, Group>, IGroupService
    {
        public GroupService(ILocator locator) : base(locator)
        {
        }
    }
}