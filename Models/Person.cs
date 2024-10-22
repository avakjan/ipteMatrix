using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace PeopleSkillsApp.Models
{
    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int WorkplaceId { get; set; }

        
        [JsonIgnore]
        public Workplace? Workplace { get; set; }

        [JsonIgnore]
        public List<PersonSkill>? PersonSkills { get; set; }

        
        [NotMapped] 
        public List<SkillLevelDto>? SkillLevels { get; set; }

    }


    public class SkillLevelDto
    {
        public int SkillId { get; set; }
        public int Level { get; set; }
    }

}
