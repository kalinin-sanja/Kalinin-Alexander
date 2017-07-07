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

        [Route("api/people/create")]
        [HttpPost]
        public async Task<IHttpActionResult> CreatePost(PersonPostViewModel person)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (RepoUoW.PersonRepository.ExistByPhone(person.Phone, person.Id))
                return BadRequest("Phone number is already used");

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

        [Route("api/people/edit")]
        [HttpGet]
        public async Task<IHttpActionResult> Edit(long id)
        {
            var person = await ServiceUoW.PersonPostViewModelService.GetAsync(id);
            if (person == null)
                return NotFound();

            return Ok(person);
        }
        
        [Route("api/people/edit")]
        [HttpPost]
        public async Task<IHttpActionResult> EditPost(PersonPostViewModel person)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (RepoUoW.PersonRepository.ExistByPhone(person.Phone, person.Id))
                return BadRequest("Phone number is already used");

            try
            {
                await ServiceUoW.PersonPostViewModelService.UpdateAsync(person);
            }
            catch (System.Exception)
            {
                return BadRequest();
            }
            return Ok();
        }
    }
}