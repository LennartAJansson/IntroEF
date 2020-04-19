using System;
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
        [HttpGet("{personId}&{assignmentId}")]
        public async Task<IEnumerable<Workload>> GetUnfinishedWorkloadsAsync(int personId = 0, int assignmentId = 0)
        {
            logger.LogInformation("Getting workloads");
            return await workloadService.GetUnfinishedWorkloadsAsync(personId, assignmentId);
        }

        [HttpPost("{personId}&{assignmentId}&{comment}&{dateTime}")]
        public async Task<int> StartWorkloadAsync(int personId, int assignmentId, string comment, DateTimeOffset dateTime)
        {
            logger.LogInformation("Starting workload");
            return await workloadService.StartWorkloadAsync(personId, assignmentId, comment, dateTime);
        }

        [HttpPut("{workloadId}&{dateTime}")]
        public async Task StopWorkloadAsync(int workloadId, DateTimeOffset dateTime)
        {
            logger.LogInformation("Stopping workload");
            await workloadService.StopWorkloadAsync(workloadId, dateTime);
        }

        //[HttpDelete()]
        //public void DeleteWorkload([FromBody]Workload workload)
        //{

        //}
    }
}
