using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using WorkloadsDb.Model;

namespace WorkloadsDb.Abstract
{
    public interface IWorkloadService
    {
        Task<IEnumerable<Person>> GetPeopleAsync();
        Task<int> CreatePersonAsync(Person person);

        Task<IEnumerable<Assignment>> GetAssignmentsAsync();
        Task<int> CreateAssignmentAsync(Assignment assignment);

        Task<IEnumerable<Workload>> GetUnfinishedWorkloadsAsync(int personId, int assignmentId);
        Task<int> StartWorkloadAsync(int personId, int assignmentId, string comment, DateTimeOffset start);
        Task StopWorkloadAsync(int workloadId, DateTimeOffset stop);
        string GetCurrentDate();
    }
}
