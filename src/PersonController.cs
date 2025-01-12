using Microsoft.AspNetCore.Mvc;

namespace Lab
{
    [ApiController]
    [Route("/api/v1/persons")]
    public class PersonController : ControllerBase
    {
        private readonly ILogger<PersonController> _logger;

        public PersonController(ILogger<PersonController> logger)
        {
            _logger = logger;
        }

        [HttpGet( "/api/v1/persons/{personId}")]
        public ActionResult<Person> Get([FromRoute] int personId)
        {
            var repo = new PostgresRepository();

            if (repo.TryGet(personId, out var person))
            {
                return Ok(person);
            }

            return NotFound();
        }

        [HttpGet("/api/v1/persons/")]
        public ActionResult<List<Person>> GetAll()
        {
            var repo = new PostgresRepository();
            return Ok(repo.GetAll());
        }

        [HttpPost("/api/v1/persons/")]
        public ActionResult Post([FromBody] Person person)
        {
            var repo = new PostgresRepository();
            var id = repo.Add(person);

            return new CreatedResult($"/api/v1/persons/{id}", null);
        }

        [HttpDelete("/api/v1/persons/{personId}")]
        public ActionResult Delete([FromRoute]int personId)
        {
            var repo = new PostgresRepository();
            repo.Delete(personId);

            return Ok();
        }

        [HttpPatch("/api/v1/persons/{personId}")]
        public ActionResult Patch([FromRoute] int personId, [FromBody] Person person)
        {
            var repo = new PostgresRepository();
            repo.Replace(personId, person);

            return Ok();
        }
    }
}
