using System.Collections.Generic;

namespace PhonebookApi.Models
{
    public class PersonIndex
    {
        public long TotalCount { get; set; }
        public int Limit { get; set; }
        public int Offset { get; set; }
        public IList<PersonViewModel> People { get; set; }
    }
}