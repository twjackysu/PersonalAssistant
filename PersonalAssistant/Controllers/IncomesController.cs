using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PersonalAssistant.Data;
using PersonalAssistant.Extension;
using PersonalAssistant.Models.AccountManager;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PersonalAssistant.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class IncomesController : Controller
    {
        private readonly ILogger<IncomesController> _logger;
        private readonly ApplicationDbContext _context;

        public IncomesController(ILogger<IncomesController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }
        // GET: api/<controller>
        [HttpGet]
        public IActionResult Get()
        {
            var userID = User.GetSID();
            if (userID == null)
                return Forbid();
            return Ok(_context.Income.Where(x => x.OwnerID == userID).ForEach(x => { x.OwnerID = default; }));
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var userID = User.GetSID();
            if (userID == null)
                return Forbid();
            var income = await _context.Income.FindAsync(id);
            if (income == null)
                return NotFound();
            if (userID != income.OwnerID)
                return Unauthorized();
            return Ok(income);
        }

        // POST api/<controller>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Income income)
        {
            var userID = User.GetSID();
            if (userID == null)
                return Forbid();
            income.OwnerID = userID;
            income.ID = null;
            _context.Add(income);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                _logger.LogError(e.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return CreatedAtAction(nameof(Get), new { id = income.ID }, income);
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody]Income income)
        {
            var userID = User.GetSID();
            if (userID == null)
                return Forbid();
            income.OwnerID = userID;
            var oldIncome = await _context.Income.FindAsync(id);
            _context.Entry(oldIncome).State = EntityState.Detached;
            if (oldIncome == null)
            {
                income.ID = null;
                _context.Add(income);
            }
            else
            {
                if (userID != oldIncome.OwnerID)
                    return Unauthorized();
                if (id != oldIncome.ID)
                    return BadRequest();
                _context.Update(income);
            }
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                _logger.LogError(e.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return NoContent();
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var income = await _context.Income.FindAsync(id);
            if (income == null)
                return NotFound();
            var userID = User.GetSID();
            if (userID == null)
                return Forbid();
            if (userID != income.OwnerID)
                return Unauthorized();
            _context.Income.Remove(income);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                _logger.LogError(e.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return NoContent();
        }
    }
}
