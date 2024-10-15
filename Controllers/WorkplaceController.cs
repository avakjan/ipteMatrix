using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeopleSkillsApp.Models;

namespace PeopleSkillsApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkplaceController : ControllerBase
    {
        private readonly AppDbContext _context;

        public WorkplaceController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Workplace
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Workplace>>> GetWorkplaces()
        {
            return await _context.Workplaces.ToListAsync();
        }

        // GET: api/Workplace/1
        [HttpGet("{id}")]
        public async Task<ActionResult<Workplace>> GetWorkplace(int id)
        {
            var workplace = await _context.Workplaces.FindAsync(id);

            if (workplace == null)
            {
                return NotFound();
            }

            return workplace;
        }

        [HttpGet("skill/{skillName}")]
        public async Task<ActionResult<object>> GetWorkplaceWithMostPeopleBySkill(string skillName)
        {
            var workplacesWithPeopleCount = await _context.Workplaces
                .Select(w => new
                {
                    WorkplaceName = w.Name,
                    PeopleCount = w.Persons
                                .Count(p => p.PersonSkills
                                            .Any(ps => ps.Skill.Name.ToLower() == skillName.ToLower()))
                })
                .OrderByDescending(w => w.PeopleCount)
                .ToListAsync();

            // Check if there are any workplaces with people having that skill
            if (workplacesWithPeopleCount == null || workplacesWithPeopleCount.Count == 0 || workplacesWithPeopleCount.First().PeopleCount == 0)
            {
                return NotFound($"No workplaces found with people having the skill: {skillName}");
            }

            // Return the workplace(s) with the most people
            var maxPeopleCount = workplacesWithPeopleCount.First().PeopleCount;
            var workplacesWithMaxPeople = workplacesWithPeopleCount
                                        .Where(w => w.PeopleCount == maxPeopleCount)
                                        .ToList();

            return Ok(workplacesWithMaxPeople);
        }


        // POST: api/Workplace
        [HttpPost]
        public async Task<ActionResult<Workplace>> CreateWorkplace([FromBody] Workplace workplace)
        {
            _context.Workplaces.Add(workplace);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetWorkplace), new { id = workplace.Id }, workplace);
        }

        // DELETE: api/Workplace/1
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWorkplace(int id)
        {
            var workplace = await _context.Workplaces.FindAsync(id);
            if (workplace == null)
            {
                return NotFound();
            }

            _context.Workplaces.Remove(workplace);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}