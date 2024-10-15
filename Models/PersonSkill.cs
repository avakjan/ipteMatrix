namespace PeopleSkillsApp.Models
{
    public class PersonSkill
    {
        public int PersonId { get; set; } // Foreign key to the Person model
        public Person Person { get; set; }

        public int SkillId { get; set; } // Foreign key to the Skill model
        public Skill Skill { get; set; }
    }
}