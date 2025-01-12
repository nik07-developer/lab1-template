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

        [IgnoreAntiforgeryToken]
        [HttpGet("/api/v1/persons/{personId}")]
        public ActionResult<Person> Get([FromRoute] int personId)
        {
            var repo = new PostgresRepository();

            if (repo.TryGet(personId, out var person))
            {
                return Ok(person);
            }

            return NotFound();
        }

        [IgnoreAntiforgeryToken]
        [HttpGet("/api/v1/persons/")]
        public ActionResult<List<Person>> GetAll()
        {
            var repo = new PostgresRepository();
            return Ok(repo.GetAll());
        }

        [IgnoreAntiforgeryToken]
        [HttpPost("/api/v1/persons/")]
        public ActionResult Post([FromBody] PersonRequestDto person)
        {
            var repo = new PostgresRepository();
            var id = repo.Add(person);


            return new CreatedResult($"http://localhost:8080/api/v1/persons/{id}", null);
        }

        [IgnoreAntiforgeryToken]
        [HttpDelete("/api/v1/persons/{personId}")]
        public ActionResult Delete([FromRoute]int personId)
        {
            var repo = new PostgresRepository();
            repo.Delete(personId);

            return Ok();
        }

        [IgnoreAntiforgeryToken]
        [HttpPatch("/api/v1/persons/{personId}")]
        public ActionResult Patch([FromRoute] int personId, [FromBody] PersonRequestDto person)
        {
            var repo = new PostgresRepository();
            repo.Replace(personId, person);

            return Ok();
        }
    }
}
