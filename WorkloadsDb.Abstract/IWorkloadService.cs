using System.Collections.Generic;
using System.Threading.Tasks;

using WorkloadsDb.Model;

namespace WorkloadsDb.Abstract
{
    public interface IWorkloadService
    {
        Task<IEnumerable<Person>> GetPeopleAsync();

        Task<Person> CreatePersonAsync(Person person);

        Task<Person> UpdatePersonAsync(Person person);

        Task<IEnumerable<Assignment>> GetAssignmentsAsync();

        Task<Assignment> CreateAssignmentAsync(Assignment assignment);

        Task<Assignment> UpdateAssignmentAsync(Assignment assignment);

        Task<IEnumerable<Workload>> GetAllWorkloadsAsync();

        Task<IEnumerable<Workload>> GetUnfinishedWorkloadsAsync(int personId, int assignmentId);

        //Nya
        Task<Workload> StartWorkloadAsync(Workload workload);

        Task StopWorkloadAsync(Workload workload);
    }
}