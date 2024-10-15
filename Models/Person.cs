using System.Text.Json.Serialization;

namespace PeopleSkillsApp.Models
{
    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int WorkplaceId { get; set; }

        // Navigation properties (not required during creation)
        [JsonIgnore]
        public Workplace? Workplace { get; set; }

        [JsonIgnore]
        public List<PersonSkill>? PersonSkills { get; set; }
    }
}