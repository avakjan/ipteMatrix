using System.Text.Json.Serialization;

namespace PeopleSkillsApp.Models
{
    public class Workplace
    {
        public int Id { get; set; }
        public string Name { get; set; }

        [JsonIgnore] // Prevents looping in the response
        public List<Person> Persons { get; set; }
    }
}
