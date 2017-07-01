using System.Web.Http;
using PhonebookApi.Repositories;
using PhonebookApi.Services;

namespace PhonebookApi.Controllers
{
    public class BaseApiController : ApiController
    {
        public BaseApiController(ILocator locator)
        {
            ServiceUoW = locator.ServiceUoW;
            RepoUoW = locator.RepoUoW;
        }

        protected IServiceUoW ServiceUoW { get; }
        public IRepositoryUoW RepoUoW { get; }

        protected override void Dispose(bool disposing)
        {
            RepoUoW.Dispose();
            base.Dispose(disposing);
        }
    }
}