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
    public class WorkloadsController : ControllerBase
    {
        private readonly IWorkloadService workloadService;
        private readonly ILogger<WorkloadsController> logger;

        public WorkloadsController(ILogger<WorkloadsController> logger, IWorkloadService workloadService)
        {
            this.logger = logger;
            this.workloadService = workloadService;
        }

        //Workload CRUD, Create, Read, Update, Delete
        [HttpGet()]
        public async Task<IEnumerable<Workload>> GetAllWorkloadsAsync()
        {
            logger.LogInformation("Getting all workloads");
            return await workloadService.GetAllWorkloadsAsync();
        }

        [HttpGet("{personId}&{assignmentId}")]
        public async Task<IEnumerable<Workload>> GetUnfinishedWorkloadsAsync(int personId = 0, int assignmentId = 0)
        {
            logger.LogInformation($"Getting workloads for PersonId {personId} and AssignmentId {assignmentId}");
            return await workloadService.GetUnfinishedWorkloadsAsync(personId, assignmentId);
        }

        //Nya
        [HttpPost()]
        public async Task<Workload> StartWorkloadAsync([FromBody] Workload workload)
        {
            logger.LogInformation("Starting workload");
            return await workloadService.StartWorkloadAsync(workload);
        }

        [HttpPut()]
        public async Task StopWorkloadAsync([FromBody] Workload workload)
        {
            logger.LogInformation("Stopping workload");
            await workloadService.StopWorkloadAsync(workload);
        }
    }
}