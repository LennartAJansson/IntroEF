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
    class Program
    {
        static readonly IEnumerable<Person> people = JsonSerializer.Deserialize<IEnumerable<Person>>(File.ReadAllText("people.json"), new JsonSerializerOptions { Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(System.Text.Unicode.UnicodeRanges.All) });
        static readonly IEnumerable<Assignment> assignments = JsonSerializer.Deserialize<IEnumerable<Assignment>>(File.ReadAllText("assignments.json"), new JsonSerializerOptions { Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(System.Text.Unicode.UnicodeRanges.All) });

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
                HttpContent httpContent = new StringContent(
                    JsonSerializer.Serialize(person, new JsonSerializerOptions { Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(System.Text.Unicode.UnicodeRanges.All) }),
                    Encoding.UTF8, "application/json");

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
                HttpContent httpContent = new StringContent(
                    JsonSerializer.Serialize(assignment, new JsonSerializerOptions { Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(System.Text.Unicode.UnicodeRanges.All) }),
                    Encoding.UTF8, "application/json");

                var response = await client.PostAsync("https://localhost:5001/assignments", httpContent);
                var i = await response.Content.ReadAsStringAsync();

                Console.WriteLine($"Added {i} status:\n\t{response.StatusCode}");
                assignment.AssignmentId = int.Parse(i);
            }
        }

        private static async Task AddWorkloads(HttpClient client)
        {
            DateTimeOffset startDate = DateTimeOffset.UtcNow.AddDays(-21).StartOfWeek(DayOfWeek.Monday);
            DateTimeOffset stopDate = DateTimeOffset.UtcNow;//.StartOfWeek(DayOfWeek.Friday); //Should be today!!!
            Random random = new Random();

            foreach (var person in people)
            {
                for (DateTimeOffset day = startDate; day.Date <= stopDate.Date; day = day.AddDays(1))
                {
                    if (day.Hour == 0)
                        day = day.AddHours(8);

                    //if (day.DayOfWeek == DayOfWeek.Saturday || day.DayOfWeek == DayOfWeek.Sunday)
                    //    continue;

                    Assignment assignment = assignments.ToArray()[random.Next(0, assignments.Count() - 1)];

                    //Start a workload
                    string comment = $"Working @ {assignment.Customer}";
                    string start = $"{day}";
                    var startUrl = Uri.EscapeUriString($"https://localhost:5001/workloads/{person.PersonId}&{assignment.AssignmentId}&{comment}&{start}");

                    var response = await client.PostAsync(startUrl, null);
                    var s = await response.Content.ReadAsStringAsync();
                    var i = int.Parse(s);

                    Console.WriteLine($"Id: {i} - {response.StatusCode}");

                    if (day.Date < stopDate.Date)
                    {
                        string stop = $"{day.AddHours(9)}";
                        var stopUrl = Uri.EscapeUriString($"https://localhost:5001/workloads/{i}&{stop}");

                        response = await client.PutAsync(stopUrl, null);

                        Console.WriteLine($"Id: {i} - {response.StatusCode}");
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
