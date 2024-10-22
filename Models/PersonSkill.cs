namespace PeopleSkillsApp.Models
{
    public class PersonSkill
    {
        public int PersonId { get; set; }
        public int SkillId { get; set; }

        public int Level { get; set; }

        public Person Person { get; set; }
        public Skill Skill { get; set; }
    }

}   