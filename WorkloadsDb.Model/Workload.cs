using System;

namespace WorkloadsDb.Model
{
    public class Workload : IPOCOClass
    {
        public int WorkloadId { get; set; }
        public virtual Person Person { get; set; }
        public int PersonId { get; set; }
        public virtual Assignment Assignment { get; set; }
        public int AssignmentId { get; set; }
        public string Comment { get; set; }
        public DateTimeOffset Start { get; set; }
        public DateTimeOffset? Stop { get; set; }

        public override string ToString()
        {
            return $"{WorkloadId}-[{Start:yyyy-MM-dd hh.mm}] ({Person.Firstname} {Person.Lastname}, {Assignment.Customer}({Assignment.Description}))";
        }
    }
}
