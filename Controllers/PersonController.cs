using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeopleSkillsApp.Models;

namespace PeopleSkillsApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PersonController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Person
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetPersons()
        {
            var persons = await _context.Persons
                                        .Include(p => p.Workplace)
                                        .Include(p => p.PersonSkills)
                                        .ThenInclude(ps => ps.Skill)
                                        .Select(p => new 
                                        {
                                            p.Id,
                                            p.Name,
                                            Workplace = p.Workplace.Name,
                                            Skills = p.PersonSkills.Select(ps => ps.Skill.Name).ToList(),
                                            Levels = p.PersonSkills.Select(ps => ps.Level).ToList()
                                        })
                                        .ToListAsync();

            return Ok(persons);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetPerson(int id)
        {
            var person = await _context.Persons
                                    .Include(p => p.Workplace)
                                    .Include(p => p.PersonSkills)
                                    .ThenInclude(ps => ps.Skill)
                                    .Select(p => new
                                    {
                                        p.Id,
                                        p.Name,
                                        Workplace = p.Workplace.Name,  // Include the workplace name
                                        Skills = p.PersonSkills.Select(ps => ps.Skill.Name).ToList(),  // List of skill names
                                        Levels = p.PersonSkills.Select(ps => ps.Level).ToList()  // List of skill levels
                                    })
                                    .FirstOrDefaultAsync(p => p.Id == id);

            if (person == null)
            {
                return NotFound();
            }

            return Ok(person);
        }

        [HttpGet("skill/{skillName}")]
        public async Task<ActionResult<IEnumerable<object>>> GetPersonsBySkill(string skillName)
        {
            var persons = await _context.Persons
                                        .Include(p => p.Workplace)
                                        .Include(p => p.PersonSkills)
                                        .ThenInclude(ps => ps.Skill)
                                        .Where(p => p.PersonSkills.Any(ps => ps.Skill.Name.ToLower() == skillName.ToLower()))
                                        .Select(p => new
                                        {
                                            p.Id,
                                            p.Name,
                                            Workplace = p.Workplace.Name, // Include workplace name
                                            Skills = p.PersonSkills.Select(ps => ps.Skill.Name).ToList() // List of skill names
                                        })
                                        .ToListAsync();

            if (persons == null || persons.Count == 0)
            {
                return NotFound($"No persons found with skill: {skillName}");
            }

            return Ok(persons);
        }



        // POST: api/Person
        [HttpPost("add")]
        public IActionResult AddPerson([FromBody] Person person)
        {
            // Validate the incoming person object
            if (person == null || person.SkillLevels == null || person.SkillLevels.Count == 0)
            {
                return BadRequest("Invalid person or skills data.");
            }

            // Validate that skill levels are within the range of 1 to 5
            foreach (var skillLevel in person.SkillLevels)
            {
                if (skillLevel.Level < 1 || skillLevel.Level > 5)
                {
                    return BadRequest($"Skill level for SkillId {skillLevel.SkillId} must be between 1 and 5.");
                }
            }

            // Create new PersonSkills entries for each skill and level
            person.PersonSkills = person.SkillLevels.Select(sl => new PersonSkill
            {
                PersonId = person.Id,
                SkillId = sl.SkillId,
                Level = sl.Level
            }).ToList();

            // Add the person to the database
            _context.Persons.Add(person);
            _context.SaveChanges();

            return Ok($"Person {person.Name} added successfully with {person.SkillLevels.Count} skills!");
        }






        // PUT: api/Person/1
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePerson(int id, [FromBody] Person person)
        {
            if (person == null)
            {
                return BadRequest("Invalid person data.");
            }

            var existingPerson = await _context.Persons
                                            .Include(p => p.PersonSkills)
                                            .FirstOrDefaultAsync(p => p.Id == id);

            if (existingPerson == null)
            {
                return NotFound();
            }

            // Update person details
            existingPerson.Name = person.Name;
            existingPerson.WorkplaceId = person.WorkplaceId;

            // Update skills if provided
            if (person.SkillLevels != null && person.SkillLevels.Count > 0)
            {
                // Remove existing skills
                _context.PersonSkills.RemoveRange(existingPerson.PersonSkills);

                // Add new skills
                foreach (var skillId in person.SkillLevels.Select(sl => sl.SkillId))
                {
                    existingPerson.PersonSkills.Add(new PersonSkill { PersonId = existingPerson.Id, SkillId = skillId });
                }
            }

            await _context.SaveChangesAsync();

            return NoContent(); // Return 204 - No Content for successful update
        }



        // DELETE: api/Person/1
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePerson(int id)
        {
            var person = await _context.Persons.FindAsync(id);
            if (person == null)
            {
                return NotFound();
            }

            _context.Persons.Remove(person);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}