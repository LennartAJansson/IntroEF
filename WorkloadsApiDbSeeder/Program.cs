using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using WorkloadsDb.Model;

namespace WebApiEFDbSeeder
{
    class Program
    {
        static IEnumerable<Person> people = JsonSerializer.Deserialize<IEnumerable<Person>>(File.ReadAllText("people.json"));
        static IEnumerable<Assignment> assignments = JsonSerializer.Deserialize<IEnumerable<Assignment>>(File.ReadAllText("assignments.json"));

        static async Task Main()
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
                HttpContent httpContent = new StringContent(JsonSerializer.Serialize(person), Encoding.UTF8);
                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var response = await client.PostAsync("https://localhost:5001/people", httpContent);
                var i = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Added {i} status:\n\t{response.StatusCode}");
                person.PersonId = int.Parse(i);
            }
        }

        private static async Task AddAssignmentsAsync(HttpClient client)
        {
            foreach (var assignment in assignments)
            {
                HttpContent httpContent = new StringContent(JsonSerializer.Serialize(assignment), Encoding.UTF8);
                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var response = await client.PostAsync("https://localhost:5001/assignments", httpContent);
                var i = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Added {i} status:\n\t{response.StatusCode}");
                assignment.AssignmentId = int.Parse(i);
            }
        }

        private static async Task AddWorkloads(HttpClient client)
        {
            DateTime startDate = DateTime.Now.AddDays(-21).StartOfWeek(DayOfWeek.Monday);
            DateTime stopDate = DateTime.Now.StartOfWeek(DayOfWeek.Friday);
            Random random = new Random();

            foreach (var person in people)
            {
                for (DateTime day = startDate; day < stopDate; day = day.AddDays(1))
                {
                    if (day.DayOfWeek == DayOfWeek.Saturday || day.DayOfWeek == DayOfWeek.Sunday)
                        continue;

                    Assignment assignment = assignments.ToArray()[random.Next(0, assignments.Count() - 1)];

                    //Start a workload
                    var response = await client.PostAsync($"https://localhost:5001/workloads/{person.PersonId}&{assignment.AssignmentId}&\"Working @ {assignment.Customer}\"", null);
                    var s = await response.Content.ReadAsStringAsync();
                    var i = int.Parse(s);
                    Console.WriteLine($"Started work {i} status:\n\t{response.StatusCode}");

                    //End all workloads except the last for each assignment
                    //Just to have finished and unfinished workloads in the sample data
                    if (i % 8 != 0)
                    {
                        response = await client.PutAsync($"https://localhost:5001/workloads/{i}", null);
                        Console.WriteLine($"Stopped work {i} status:\n\t{response.StatusCode}");
                    }
                }



                //foreach (var assignment in assignments)
                //{
                //    //Start a workload
                //    var response = await client.PostAsync($"https://localhost:5001/workloads/{person.PersonId}&{assignment.AssignmentId}&\"Working @ {assignment.Customer}\"", null);
                //    var s = await response.Content.ReadAsStringAsync();
                //    var i = int.Parse(s);
                //    Console.WriteLine($"Started work {i} status:\n\t{response.StatusCode}");

                //    //End all workloads except the last for each assignment
                //    //Just to have finished and unfinished workloads in the sample data
                //    if (i % 8 != 0)
                //    {
                //        response = await client.PutAsync($"https://localhost:5001/workloads/{i}", null);
                //        Console.WriteLine($"Stopped work {i} status:\n\t{response.StatusCode}");
                //    }
                //}
            }
        }
    }
    public static class DateTimeExtensions
    {
        public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = (7 + (dt.DayOfWeek - startOfWeek)) % 7;
            return dt.AddDays(-1 * diff).Date;
        }
    }
}
