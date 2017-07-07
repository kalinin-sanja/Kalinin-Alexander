using System;
using System.ComponentModel.DataAnnotations;

namespace PhonebookApi.Models
{
    public class PersonPostViewModel
    {
        public long Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [RegularExpression(@"[\+]\d{1}[\(]\d{3}[\)]\d{3}[\-]\d{2}[\-]\d{2}")]
        public string Phone { get; set; }
        public bool Sex { get; set; }
        public bool IsDraft { get; set; }
        [DataType(DataType.Date)]
        public DateTime Birthday { get; set; }
        public long GroupId { get; set; }
        public DateTime? CreatingTime { get; set; }
        public DateTime? LastEditingTime { get; set; }
    }
}