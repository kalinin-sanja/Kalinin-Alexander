using System.Threading.Tasks;
using System.Web.Http;
using PhonebookApi.ActionFilters;
using PhonebookApi.Models;

namespace PhonebookApi.Controllers
{
    public class PeopleController : BaseApiController
    {
        public PeopleController(ILocator locator) : base(locator)
        {
        }

        [RangeFilter(5, 50)]
        [HttpGet]
        public async Task<IHttpActionResult> Index([FromUri] PersonFilter filter)
        {
            var range = await ServiceUoW.PersonService.GetRangeAsync(filter);
            var model = new PersonIndex()
            {
                Limit = filter.Limit,
                Offset = filter.Offset,
                TotalCount = await ServiceUoW.PersonService.TotalCountAsync(filter),
                People = range
            };
            return Ok(model);
        }

        [HttpPost]
        public async Task<IHttpActionResult> Post(PersonPostViewModel person)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await ServiceUoW.PersonPostViewModelService.AddAsync(person);
            }
            catch (System.Exception)
            {
                return BadRequest();
            }
            return Ok();
        }
    }
}