using Microsoft.Extensions.Logging;

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
        private readonly ILogger<WorkloadService> logger;
        private readonly IUnitOfWork unitOfWork;

        public WorkloadService(ILogger<WorkloadService> logger, IUnitOfWork unitOfWork)
        {
            this.logger = logger;
            this.unitOfWork = unitOfWork;
        }

        public Task<IEnumerable<Person>> GetPeopleAsync()
        {
            //TODO! The outcommented is more correct but trashes the (de)serialization in the webapi controller
            //return Task.FromResult(unitOfWork.Repository<Person>().Get(includeProperties: "Workloads,Workloads.Assignment"));
            return Task.FromResult(unitOfWork.Repository<Person>().Get(includeProperties: "Workloads"));
        }

        public async Task<Person> CreatePersonAsync(Person person)
        {
            try
            {
                //TODO Find a better approach. This is only a workaround to make it possible to save without errors due to key violation
                person.Workloads = null;

                await unitOfWork.Repository<Person>().InsertAsync(person);
                await unitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error");
            }

            return person;
        }

        public async Task<Person> UpdatePersonAsync(Person person)
        {
            try
            {
                //TODO Find a better approach. This is only a workaround to make it possible to save without errors due to key violation
                person.Workloads = null;

                unitOfWork.Repository<Person>().Update(person);
                await unitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error");
            }

            return person;
        }

        public Task<IEnumerable<Assignment>> GetAssignmentsAsync()
        {
            //TODO! The outcommented is more correct but trashes the (de)serialization in the webapi controller
            //return Task.FromResult(unitOfWork.Repository<Assignment>().Get(includeProperties: "Workloads,Workloads.Person"));
            return Task.FromResult(unitOfWork.Repository<Assignment>().Get(includeProperties: "Workloads"));
        }

        public async Task<Assignment> CreateAssignmentAsync(Assignment assignment)
        {
            try
            {
                //TODO Find a better approach. This is only a workaround to make it possible to save without errors due to key violation
                assignment.Workloads = null;

                await unitOfWork.Repository<Assignment>().InsertAsync(assignment);
                await unitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error");
            }

            return assignment;
        }

        public async Task<Assignment> UpdateAssignmentAsync(Assignment assignment)
        {
            try
            {
                //TODO Find a better approach. This is only a workaround to make it possible to save without errors due to key violation
                assignment.Workloads = null;

                unitOfWork.Repository<Assignment>().Update(assignment);
                await unitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error");
            }

            return assignment;
        }

        public Task<IEnumerable<Workload>> GetAllWorkloadsAsync()
        {
            //TODO! The outcommented is more correct but trashes the (de)serialization in the webapi controller
            //IEnumerable<Workload> result = unitOfWork.Repository<Workload>().Get(includeProperties: "Person,Assignment");
            IEnumerable<Workload> result = unitOfWork.Repository<Workload>().Get();

            return Task.FromResult(result);
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

        //Nya
        public async Task<Workload> StartWorkloadAsync(Workload workload)
        {
            logger.LogInformation($"Starting {workload.Comment} @ {workload.Start}");

            try
            {
                //TODO Find a better approach. This is only a workaround to make it possible to save without errors due to key violation
                workload.Assignment = null;
                workload.Person = null;
                await unitOfWork.Repository<Workload>().InsertAsync(workload);
                await unitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error");
            }

            return workload;
        }

        public async Task StopWorkloadAsync(Workload workload)
        {
            logger.LogInformation($"Stopping {workload.Comment} @ {workload.Stop.Value.ToString()}");

            try
            {
                //TODO Find a better approach. This is only a workaround to make it possible to save without errors due to key violation
                workload.Assignment = null;
                workload.Person = null;
                unitOfWork.Repository<Workload>().Update(workload);
                await unitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error");
            }
        }
    }
}