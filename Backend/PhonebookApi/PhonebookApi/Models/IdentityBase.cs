using System;

namespace PhonebookApi.Models
{
    public interface IIdentityBase : IIdentified
    {
        string Name { get; set; }
        DateTime? CreatingTime { get; set; }
        DateTime? LastEditingTime { get; set; }
    }
}