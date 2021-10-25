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
    public class ExpenditureTypesController : ControllerBase
    {
        private readonly ILogger<ExpenditureTypesController> _logger;
        private readonly ApplicationDbContext _context;

        public ExpenditureTypesController(ILogger<ExpenditureTypesController> logger, ApplicationDbContext context)
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
            return Ok(_context.ExpenditureType.Where(x => x.OwnerID == userID).ForEach(x => { x.OwnerID = default; }));
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var userID = User.GetSID();
            if (userID == null)
                return Forbid();
            var expenditureType = await _context.ExpenditureType.FindAsync(id);
            if (expenditureType == null)
                return NotFound();
            if (userID != expenditureType.OwnerID)
                return Unauthorized();
            return Ok(expenditureType);
        }

        // POST api/<controller>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]ExpenditureType expenditureType)
        {
            var userID = User.GetSID();
            if (userID == null)
                return Forbid();
            expenditureType.OwnerID = userID;
            expenditureType.ID = null;
            _context.Add(expenditureType);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                _logger.LogError(e.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return CreatedAtAction(nameof(Get), new { id = expenditureType.ID }, expenditureType);
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody]ExpenditureType expenditureType)
        {
            var userID = User.GetSID();
            if (userID == null)
                return Forbid();
            expenditureType.OwnerID = userID;
            var oldExpenditureType = await _context.ExpenditureType.FindAsync(id);
            _context.Entry(oldExpenditureType).State = EntityState.Detached;
            if (oldExpenditureType == null)
            {
                expenditureType.ID = null;
                _context.Add(expenditureType);
            }
            else
            {
                if (userID != oldExpenditureType.OwnerID)
                    return Unauthorized();
                if (id != oldExpenditureType.ID)
                    return BadRequest();
                _context.Update(expenditureType);
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
            var expenditureType = await _context.ExpenditureType.FindAsync(id);
            if (expenditureType == null)
                return NotFound();
            var userID = User.GetSID();
            if (userID == null)
                return Forbid();
            if (userID != expenditureType.OwnerID)
                return Unauthorized();
            _context.ExpenditureType.Remove(expenditureType);

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
