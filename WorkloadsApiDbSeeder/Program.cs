using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using WorkloadsDb.Model;

namespace WebApiEFDbSeeder
{
    internal class Program
    {
        private static readonly IEnumerable<Person> people = JsonSerializer.Deserialize<IEnumerable<Person>>(File.ReadAllText("people.json"));
        private static readonly IEnumerable<Assignment> assignments = JsonSerializer.Deserialize<IEnumerable<Assignment>>(File.ReadAllText("assignments.json"));
        private static readonly string peopleUrl = Uri.EscapeUriString($"https://localhost:5001/people");
        private static readonly string assignmentsUrl = Uri.EscapeUriString($"https://localhost:5001/assignments");
        private static readonly string workloadsUrl = Uri.EscapeUriString($"https://localhost:5001/workloads");

        private static readonly JsonSerializerOptions options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        private static async Task Main()
        {
            using var client = new HttpClient();
            await AddPeopleAsync(client);
            await AddAssignmentsAsync(client);
            await AddWorkloads(client);
        }

        private static async Task AddPeopleAsync(HttpClient client)
        {
            foreach (var person in people)
            {
                string json = JsonSerializer.Serialize(person);
                HttpContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(peopleUrl, httpContent);
                json = await response.Content.ReadAsStringAsync();
                Person result = JsonSerializer.Deserialize<Person>(json, options);

                Console.WriteLine($"Added {result} status: {response.StatusCode}");
                person.PersonId = result.PersonId;
            }
        }

        private static async Task AddAssignmentsAsync(HttpClient client)
        {
            foreach (var assignment in assignments)
            {
                string json = JsonSerializer.Serialize(assignment);
                HttpContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(assignmentsUrl, httpContent);
                json = await response.Content.ReadAsStringAsync();
                Assignment result = JsonSerializer.Deserialize<Assignment>(json, options);

                Console.WriteLine($"Added {result} status: {response.StatusCode}");
                assignment.AssignmentId = result.AssignmentId;
            }
        }

        private static async Task AddWorkloads(HttpClient client)
        {
            DateTimeOffset startDate = DateTimeOffset.UtcNow.AddDays(-21).StartOfWeek(DayOfWeek.Monday);
            DateTimeOffset stopDate = DateTimeOffset.UtcNow;
            Random random = new Random();

            foreach (Person person in people)
            {
                for (DateTimeOffset day = startDate; day.Date <= stopDate.Date; day = day.AddDays(1))
                {
                    if (day.Hour == 0)
                        day = day.AddHours(8);

                    Assignment assignment = assignments.ToArray()[random.Next(0, assignments.Count() - 1)];

                    //Start a workload
                    Workload workload = new Workload
                    {
                        AssignmentId = assignment.AssignmentId,
                        PersonId = person.PersonId,
                        Comment = $"Working @ {assignment.Customer}",
                        Start = day
                    };
                    string json = JsonSerializer.Serialize(workload);
                    HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await client.PostAsync(workloadsUrl, content);
                    json = await response.Content.ReadAsStringAsync();
                    Workload result = JsonSerializer.Deserialize<Workload>(json, options);
                    workload.WorkloadId = result.WorkloadId;

                    //Fix result for displaying
                    result.Person = person;
                    result.PersonId = person.PersonId;
                    result.Assignment = assignment;
                    result.AssignmentId = assignment.AssignmentId;

                    Console.WriteLine($"Started {result} status: {response.StatusCode}");

                    if (day.Date < stopDate.Date)
                    {
                        workload.Stop = day.AddHours(9);
                        json = JsonSerializer.Serialize(workload);
                        content = new StringContent(json, Encoding.UTF8, "application/json");

                        response = await client.PutAsync(workloadsUrl, content);
                        response.EnsureSuccessStatusCode();

                        //Fix workload for displaying
                        workload.Person = person;
                        workload.Assignment = assignment;

                        Console.WriteLine($"Stopped {workload} status: {response.StatusCode}");
                    }
                }
            }
        }
    }

    public static class DateTimeExtensions
    {
        public static DateTimeOffset StartOfWeek(this DateTimeOffset dt, DayOfWeek startOfWeek)
        {
            int diff = (7 + (dt.DayOfWeek - startOfWeek)) % 7;
            return dt.AddDays(-1 * diff).Date;
        }
    }
}