using System.Text.Json.Serialization;

namespace PeopleSkillsApp.Models
{
    public class Skill
    {
        public int Id { get; set; }
        public string Name { get; set; }
        private int _level;
        public int Level
        {
            get => _level;
            set
            {
                if (value < 1 || value > 5)
                {
                    throw new ArgumentException("Skill level must be between 1 and 5.");
                }
                _level = value;
            }
        }
        [JsonIgnore] // Prevents looping in the response
        public List<PersonSkill>? PersonSkills { get; set; }
    }
}
