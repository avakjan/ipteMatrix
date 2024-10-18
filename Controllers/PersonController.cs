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
                                            Skills = p.PersonSkills.Select(ps => ps.Skill.Name).ToList()
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
                                        Skills = p.PersonSkills.Select(ps => ps.Skill.Name).ToList()  // List of skill names
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
        [HttpPost]
        public async Task<ActionResult<object>> CreatePerson([FromBody] Person person)
        {
            if (person == null || person.SkillIds == null || person.SkillIds.Count == 0)
            {
                return BadRequest("Invalid person or skills data.");
            }

            // Add the person to the database
            _context.Persons.Add(person);
            await _context.SaveChangesAsync();

            // Associate the person with their skills
            foreach (var skillId in person.SkillIds)
            {
                _context.PersonSkills.Add(new PersonSkill { PersonId = person.Id, SkillId = skillId });
            }

            await _context.SaveChangesAsync();

            // Fetch the workplace name and skill names
            var workplace = await _context.Workplaces.FindAsync(person.WorkplaceId);
            var skillNames = await _context.Skills
                                        .Where(s => person.SkillIds.Contains(s.Id))
                                        .Select(s => s.Name)
                                        .ToListAsync();

            // Return a customized response
            var response = new
            {
                Id = person.Id,
                Name = person.Name,
                Workplace = workplace.Name,  // Workplace name
                Skills = skillNames          // List of skill names
            };

            return CreatedAtAction(nameof(GetPerson), new { id = person.Id }, response);
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
            if (person.SkillIds != null && person.SkillIds.Count > 0)
            {
                // Remove existing skills
                _context.PersonSkills.RemoveRange(existingPerson.PersonSkills);

                // Add new skills
                foreach (var skillId in person.SkillIds)
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