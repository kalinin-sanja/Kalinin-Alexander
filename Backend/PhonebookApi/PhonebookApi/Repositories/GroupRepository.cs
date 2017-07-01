using PhonebookApi.Models;

namespace PhonebookApi.Repositories
{
    public interface IGroupRepository : IRepository<Group>
    {
    }

    public class GroupRepository : Repository<Group>, IGroupRepository
    {
        public GroupRepository(PhonebookDbContext context, IRepositoryUoW repositoryUoW) : base(context, repositoryUoW)
        {
        }
    }
}