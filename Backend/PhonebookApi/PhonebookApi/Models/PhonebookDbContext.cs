namespace PhonebookApi.Models
{
    using System.Data.Entity;

    public interface IIdentified
    {
        long Id { get; set; }
    }

    public class PhonebookDbContext : DbContext
    {
        public PhonebookDbContext()
            : base("name=PhonebookDbContext")
        {
        }

        public virtual DbSet<Person> People { get; set; }
        public virtual DbSet<Group> Groups { get; set; }
    }
}