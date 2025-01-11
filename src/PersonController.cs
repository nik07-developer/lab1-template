using Microsoft.AspNetCore.Mvc;

namespace Lab
{
    [ApiController]
    [Route("/api/v1")]
    public class PersonController : ControllerBase
    {
        private readonly ILogger<PersonController> _logger;

        public PersonController(ILogger<PersonController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "/persons/")]
        public IEnumerable<Person> Get()
        {
            return [new Person()];
            var repo = new PostgresRepository();

            repo.Add(new Person());
            return repo.GetAll();
        }


    }
}
