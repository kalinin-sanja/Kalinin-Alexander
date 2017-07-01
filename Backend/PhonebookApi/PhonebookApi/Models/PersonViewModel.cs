using System;

namespace PhonebookApi.Models
{
    public class PersonViewModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public bool Sex { get; set; }
        public bool IsDraft { get; set; }
        public DateTime Birthday { get; set; }
        public GroupViewModel Group { get; set; }
    }

    public class PersonFilter : Filter
    {
        public bool OrderByDesc { get; set; }
    }
}