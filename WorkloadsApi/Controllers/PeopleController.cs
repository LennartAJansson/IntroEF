using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using System.Collections.Generic;
using System.Threading.Tasks;

using WorkloadsDb.Abstract;
using WorkloadsDb.Model;

namespace WorkloadsApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PeopleController : ControllerBase
    {
        private readonly IWorkloadService workloadService;
        private readonly ILogger<PeopleController> logger;

        public PeopleController(ILogger<PeopleController> logger, IWorkloadService workloadService)
        {
            this.logger = logger;
            this.workloadService = workloadService;
        }

        //Person CRUD, Create, Read, Update, Delete
        [HttpGet]
        [Produces("application/json")]
        public async Task<IEnumerable<Person>> GetPeopleAsync()
        {
            logger.LogInformation("Getting people");
            return await workloadService.GetPeopleAsync();
        }

        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<Person> CreatePersonAsync([FromBody] Person person)
        {
            logger.LogInformation("Creating person");
            return await workloadService.CreatePersonAsync(person);
        }

        [HttpPut]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<Person> UpdatePerson([FromBody] Person person)
        {
            logger.LogInformation("Updating person");
            return await workloadService.UpdatePersonAsync(person);
        }

        //[HttpDelete("people")]
        //public void DeletePerson([FromBody]Person person)
        //{
        //}
    }
}