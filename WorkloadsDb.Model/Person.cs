using System;
using System.Collections.Generic;

namespace WorkloadsDb.Model
{
    public class Person : IPOCOClass
    {
        public int PersonId { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Postalcode { get; set; }
        public string City { get; set; }
        public string Phone { get; set; }
        public virtual ICollection<Workload> Workloads { get; set; } = new HashSet<Workload>();

        public override string ToString()
        {
            return $"{PersonId}-{Firstname} {Lastname}";
        }
    }
}
