using System.Text.Json.Serialization;

namespace PeopleSkillsApp.Models
{
    public class Skill
    {
        public int Id { get; set; }
        public string Name { get; set; }

        [JsonIgnore] // Prevents looping in the response
        public List<PersonSkill>? PersonSkills { get; set; }
    }
}
