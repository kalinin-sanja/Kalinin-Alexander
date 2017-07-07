using System;

namespace PhonebookApi.Models
{
    public class Group : IIdentityBase
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public DateTime? CreatingTime { get; set; }
        public DateTime? LastEditingTime { get; set; }
    }
}