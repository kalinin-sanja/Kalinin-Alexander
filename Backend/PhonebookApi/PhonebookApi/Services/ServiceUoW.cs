namespace PhonebookApi.Services
{
    public interface IServiceUoW
    {
        IPersonService PersonService { get; }
        IPersonPostViewModelService PersonPostViewModelService { get; }
        IGroupService GroupService { get; }
    }

    public class ServiceUoW : IServiceUoW
    {
        protected ILocator Locator;
        public IPersonService PersonService => new PersonService(Locator);
        public IPersonPostViewModelService PersonPostViewModelService => new PersonPostViewModelService(Locator);
        public IGroupService GroupService => new GroupService(Locator);

        public ServiceUoW(ILocator locator)
        {
            Locator = locator;
        }
    }
}