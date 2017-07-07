using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace PhonebookApi.Models
{
    public class Person : IIdentityBase
    {
        public long Id { get; set; }
        [Required]
        [MinLength(3)]
        [MaxLength(100)]
        public string Name { get; set; }
        [Index(IsUnique = true)]
        [MaxLength(16)]
        public string Phone { get; set; }
        public bool Sex { get; set; }
        public bool IsDraft { get; set; }
        public DateTime Birthday { get; set; }
        public long GroupId { get; set; }
        public virtual Group Group { get; set; }
        public DateTime? CreatingTime { get; set; }
        public DateTime? LastEditingTime { get; set; }
    }
}