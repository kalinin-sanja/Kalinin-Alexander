using System;
using System.Collections.Generic;
using System.Data.Entity;

namespace PhonebookApi.Models
{
    public class PhonebookDatabaseInitializer : CreateDatabaseIfNotExists<PhonebookDbContext>
    {
        protected override void Seed(PhonebookDbContext context)
        {
            base.Seed(context);

            var groups = new List<Group>();

            groups.Add(new Group()
            {
                Name = "Friends",
                CreatingTime = DateTime.UtcNow
            });

            groups.Add(new Group()
            {
                Name = "Colleagues",
                CreatingTime = DateTime.UtcNow
            });

            groups.Add(new Group()
            {
                Name = "Relatives",
                CreatingTime = DateTime.UtcNow
            });

            var people = new List<Person>();

            people.Add(new Person()
            {
                Name = "Ella Warner",
                Birthday = new DateTime(1988, 4, 8),
                Phone = "+7(912)873-99-99",
                Group = groups[0],
                Sex = false,
                IsDraft = false,
                CreatingTime = DateTime.UtcNow
            });

            people.Add(new Person()
            {
                Name = "Fernando Benson",
                Birthday = new DateTime(1982, 7, 28),
                Phone = "+7(912)873-45-99",
                Group = groups[2],
                Sex = true,
                IsDraft = false,
                CreatingTime = DateTime.UtcNow
            });

            people.Add(new Person()
            {
                Name = "Antonio Mason",
                Birthday = new DateTime(1972, 5, 23),
                Phone = "+7(912)873-12-16",
                Group = groups[1],
                Sex = true,
                IsDraft = false,
                CreatingTime = DateTime.UtcNow
            });

            people.Add(new Person()
            {
                Name = "Annie Andrews",
                Birthday = new DateTime(1974, 12, 3),
                Phone = "+7(912)843-99-09",
                Group = groups[1],
                Sex = false,
                IsDraft = false,
                CreatingTime = DateTime.UtcNow
            });

            people.Add(new Person()
            {
                Name = "Kimberly Fisher",
                Birthday = new DateTime(1968, 2, 16),
                Phone = "+7(912)810-12-99",
                Group = groups[0],
                Sex = false,
                IsDraft = false,
                CreatingTime = DateTime.UtcNow
            });

            people.Add(new Person()
            {
                Name = "Alberto	Burke",
                Birthday = new DateTime(1969, 4, 19),
                Phone = "+7(945)123-65-99",
                Group = groups[2],
                Sex = true,
                IsDraft = false,
                CreatingTime = DateTime.UtcNow
            });

            people.Add(new Person()
            {
                Name = "Forrest Francis",
                Birthday = new DateTime(1970, 1, 9),
                Phone = "+7(916)742-44-99",
                Group = groups[0],
                Sex = true,
                IsDraft = false,
                CreatingTime = DateTime.UtcNow
            });

            people.Add(new Person()
            {
                Name = "Catherine Carson",
                Birthday = new DateTime(1961, 7, 5),
                Phone = "+7(932)782-12-45",
                Group = groups[0],
                Sex = false,
                IsDraft = false,
                CreatingTime = DateTime.UtcNow
            });

            people.Add(new Person()
            {
                Name = "Homer Flores",
                Birthday = new DateTime(1991, 5, 13),
                Phone = "+7(789)164-11-42",
                Group = groups[1],
                Sex = true,
                IsDraft = false,
                CreatingTime = DateTime.UtcNow
            });

            people.Add(new Person()
            {
                Name = "Sammy Bridges",
                Birthday = new DateTime(1984, 10, 7),
                Phone = "+7(941)374-65-87",
                Group = groups[2],
                Sex = true,
                IsDraft = false,
                CreatingTime = DateTime.UtcNow
            });

            people.Add(new Person()
            {
                Name = "Lora Kelly",
                Birthday = new DateTime(1986, 11, 24),
                Phone = "+7(936)456-17-15",
                Group = groups[1],
                Sex = false,
                IsDraft = true,
                CreatingTime = DateTime.UtcNow
            });

            groups.ForEach(x => context.Groups.Add(x));
            people.ForEach(x => context.People.Add(x));
            context.SaveChanges();
        }
    }
}