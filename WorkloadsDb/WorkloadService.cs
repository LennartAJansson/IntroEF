using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

using WorkloadsDb.Abstract;
using WorkloadsDb.Model;

namespace WorkloadsDb
{
    public class WorkloadService : IWorkloadService
    {
        private readonly IUnitOfWork unitOfWork;

        public WorkloadService(IUnitOfWork unitOfWork) => this.unitOfWork = unitOfWork;

        public Task<IEnumerable<Person>> GetPeopleAsync()
        {
            //TODO! The outcommented is more correct but trashes the (de)serialization in the webapi controller
            //return Task.FromResult(unitOfWork.Repository<Person>().Get(includeProperties: "Workloads,Workloads.Assignment"));
            return Task.FromResult(unitOfWork.Repository<Person>().Get(includeProperties: "Workloads"));
        }
        public async Task<int> CreatePersonAsync(Person person)
        {
            await unitOfWork.Repository<Person>().InsertAsync(person);
            await unitOfWork.SaveAsync();

            return person.PersonId;
        }

        public Task<IEnumerable<Assignment>> GetAssignmentsAsync()
        {
            //TODO! The outcommented is more correct but trashes the (de)serialization in the webapi controller
            //return Task.FromResult(unitOfWork.Repository<Assignment>().Get(includeProperties: "Workloads,Workloads.Person"));
            return Task.FromResult(unitOfWork.Repository<Assignment>().Get(includeProperties: "Workloads"));
        }

        public async Task<int> CreateAssignmentAsync(Assignment assignment)
        {
            await unitOfWork.Repository<Assignment>().InsertAsync(assignment);
            await unitOfWork.SaveAsync();

            return assignment.AssignmentId;
        }

        public Task<IEnumerable<Workload>> GetUnfinishedWorkloadsAsync(int personId = 0, int assignmentId = 0)
        {
            Expression<Func<Workload, bool>> filter = null;

            if (personId == 0)
            {
                if (assignmentId == 0)
                    filter = workload => workload.Stop == null;
                else
                    filter = workload => workload.AssignmentId == assignmentId && workload.Stop == null;
            }
            else
            {
                if (assignmentId == 0)
                    filter = workload => workload.PersonId == personId && workload.Stop == null;
                else
                    filter = workload => workload.PersonId == personId && workload.AssignmentId == assignmentId && workload.Stop == null;
            }

            IEnumerable<Workload> result = unitOfWork.Repository<Workload>().Get(filter, includeProperties: "Person,Assignment");

            return Task.FromResult(result);
        }

        public async Task<int> StartWorkloadAsync(int personId, int assignmentId, string comment, DateTimeOffset start)
        {
            Workload workload = new Workload
            {
                PersonId = personId,
                AssignmentId = assignmentId,
                Comment = comment,
                Start = start,
                Stop = null
            };

            await unitOfWork.Repository<Workload>().InsertAsync(workload);
            await unitOfWork.SaveAsync();

            return workload.WorkloadId;
        }

        public async Task StopWorkloadAsync(int workloadId, DateTimeOffset stop)
        {
            Workload workload = unitOfWork.Repository<Workload>().GetByID(workloadId);

            if (workload != null)
            {
                workload.Stop = stop;
                unitOfWork.Repository<Workload>().Update(workload);
                await unitOfWork.SaveAsync();
            }
        }
        public string GetCurrentDate()
        {
            return DateTime.Now.ToLongDateString();
        }
    }
}
