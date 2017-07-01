using System.Threading.Tasks;
using System.Web.Http;
using PhonebookApi.ActionFilters;
using PhonebookApi.Models;

namespace PhonebookApi.Controllers
{
    public class GroupsController : BaseApiController
    {
        public GroupsController(ILocator locator) : base(locator)
        {
        }

        [RangeFilter(5, 50)]
        [HttpGet]
        public async Task<IHttpActionResult> Index([FromUri] Filter filter)
        {
            var groups = await ServiceUoW.GroupService.GetRangeAsync(filter);
            return Ok(groups);
        }
    }
}