using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

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
        public async Task<int> CreatePersonAsync([FromBody]Person person)
        {
            logger.LogInformation("Creating person");
            return await workloadService.CreatePersonAsync(person);
        }

        //[HttpPut("people")]
        //public void UpdatePerson([FromBody]Person person)
        //{

        //}

        //[HttpDelete("people")]
        //public void DeletePerson([FromBody]Person person)
        //{

        //}
    }
}
