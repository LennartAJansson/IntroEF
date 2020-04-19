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
    public class AssignmentsController : ControllerBase
    {
        private readonly IWorkloadService workloadService;
        private readonly ILogger<AssignmentsController> logger;

        public AssignmentsController(ILogger<AssignmentsController> logger, IWorkloadService workloadService)
        {
            this.logger = logger;
            this.workloadService = workloadService;
        }

        //Assignment CRUD, Create, Read, Update, Delete
        [HttpGet]
        public async Task<IEnumerable<Assignment>> GetAssignmentsAsync()
        {
            logger.LogInformation("Getting assignment");
            return await workloadService.GetAssignmentsAsync();
        }

        [HttpPost]
        public async Task<int> CreateAssignmentAsync([FromBody]Assignment assignment)
        {
            logger.LogInformation("Creating assignment");
            return await workloadService.CreateAssignmentAsync(assignment);
        }

        //[HttpPut("assignment")]
        //public void UpdateAssignment([FromBody]Assignment assignment)
        //{
        //  logger.LogInformation("");
        //}

        //[HttpDelete("assignment")]
        //public void DeleteAssignment([FromBody]Assignment assignment)
        //{
        //  logger.LogInformation("");
        //}
    }
}
