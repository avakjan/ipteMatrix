using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace PeopleSkillsApp.Models
{
    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int WorkplaceId { get; set; }

        // Navigation properties
        [JsonIgnore]
        public Workplace? Workplace { get; set; }

        [JsonIgnore]
        public List<PersonSkill>? PersonSkills { get; set; }

        // New property for creating the person with skills
        [NotMapped] // This tells EF Core to ignore this property when mapping to the database
        public List<int>? SkillIds { get; set; }
    }
}
