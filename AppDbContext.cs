using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace PeopleSkillsApp.Models
{
    public class AppDbContext : DbContext
    {
        public DbSet<Person> Persons { get; set; }
        public DbSet<Workplace> Workplaces { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<PersonSkill> PersonSkills { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            
            modelBuilder.Entity<PersonSkill>()
                .HasKey(ps => new { ps.PersonId, ps.SkillId });

            modelBuilder.Entity<PersonSkill>()
                .HasOne(ps => ps.Person)
                .WithMany(p => p.PersonSkills)
                .HasForeignKey(ps => ps.PersonId);

            modelBuilder.Entity<PersonSkill>()
                .HasOne(ps => ps.Skill)
                .WithMany(s => s.PersonSkills)
                .HasForeignKey(ps => ps.SkillId);

            
            var estonia = new Workplace { Id = 1, Name = "Estonia" };
            var germany = new Workplace { Id = 2, Name = "Germany" };
            var usa = new Workplace { Id = 3, Name = "USA" };

            modelBuilder.Entity<Workplace>().HasData(estonia, germany, usa);

            
            var csharp = new Skill { Id = 1, Name = "C#" };
            var java = new Skill { Id = 2, Name = "Java" };
            var excel = new Skill { Id = 3, Name = "Excel" };
            var python = new Skill { Id = 4, Name = "Python" };
            var powerpoint = new Skill { Id = 5, Name = "PowerPoint" };

            modelBuilder.Entity<Skill>().HasData(csharp, java, excel, python, powerpoint);

            
            var german = new Person { Id = 1, Name = "German", WorkplaceId = estonia.Id };
            var mark = new Person { Id = 2, Name = "Mark", WorkplaceId = germany.Id };
            var daniel = new Person { Id = 3, Name = "Daniel", WorkplaceId = usa.Id };

            modelBuilder.Entity<Person>().HasData(german, mark, daniel);

            
            modelBuilder.Entity<PersonSkill>().HasData(
                new PersonSkill { PersonId = 1, SkillId = 1 }, 
                new PersonSkill { PersonId = 1, SkillId = 2 }, 
                new PersonSkill { PersonId = 2, SkillId = 1 }, 
                new PersonSkill { PersonId = 2, SkillId = 3 }, 
                new PersonSkill { PersonId = 3, SkillId = 4 }, 
                new PersonSkill { PersonId = 3, SkillId = 5 }, 
                new PersonSkill { PersonId = 3, SkillId = 3 }  
            );
        }
    }
}