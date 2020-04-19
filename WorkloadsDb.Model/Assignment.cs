using System.Collections.Generic;

namespace WorkloadsDb.Model
{
    public class Assignment : IPOCOClass
    {
        public int AssignmentId { get; set; }
        public string Customer { get; set; }
        public string Description { get; set; }
        public virtual ICollection<Workload> Workloads { get; set; } = new HashSet<Workload>();

        public override string ToString()
        {
            return $"{AssignmentId}-{Customer}({Description})";
        }
    }
}
