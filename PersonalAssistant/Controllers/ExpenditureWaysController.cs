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

namespace PersonalAssistant.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ExpenditureWaysController : ControllerBase
    {
        private readonly ILogger<ExpenditureWaysController> _logger;
        private readonly ApplicationDbContext _context;

        public ExpenditureWaysController(ILogger<ExpenditureWaysController> logger, ApplicationDbContext context)
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
            return Ok(_context.ExpenditureWay.Where(x => x.OwnerID == userID).ForEach(x => { x.OwnerID = default; }));
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var userID = User.GetSID();
            if (userID == null)
                return Forbid();
            var expenditureWay = await _context.ExpenditureWay.FindAsync(id);
            if (expenditureWay == null)
                return NotFound();
            if (userID != expenditureWay.OwnerID)
                return Unauthorized();
            return Ok(expenditureWay);
        }

        // POST api/<controller>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ExpenditureWay expenditureWay)
        {
            var userID = User.GetSID();
            if (userID == null)
                return Forbid();
            expenditureWay.OwnerID = userID;
            expenditureWay.ID = null;
            _context.Add(expenditureWay);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                _logger.LogError(e.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return CreatedAtAction(nameof(Get), new { id = expenditureWay.ID }, expenditureWay);
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] ExpenditureWay expenditureWay)
        {
            var userID = User.GetSID();
            if (userID == null)
                return Forbid();
            expenditureWay.OwnerID = userID;
            var oldExpenditureWay = await _context.ExpenditureWay.FindAsync(id);
            _context.Entry(oldExpenditureWay).State = EntityState.Detached;
            if (oldExpenditureWay == null)
            {
                expenditureWay.ID = null;
                _context.Add(expenditureWay);
            }
            else
            {
                if (userID != oldExpenditureWay.OwnerID)
                    return Unauthorized();
                if (id != oldExpenditureWay.ID)
                    return BadRequest();
                _context.Update(expenditureWay);
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
            var expenditureWay = await _context.ExpenditureWay.FindAsync(id);
            if (expenditureWay == null)
                return NotFound();
            var userID = User.GetSID();
            if (userID == null)
                return Forbid();
            if (userID != expenditureWay.OwnerID)
                return Unauthorized();
            _context.ExpenditureWay.Remove(expenditureWay);

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
