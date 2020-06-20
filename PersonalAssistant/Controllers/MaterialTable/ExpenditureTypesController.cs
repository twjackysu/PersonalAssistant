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

namespace PersonalAssistant.Controllers.MaterialTable
{
    [Authorize]
    [ApiController]
    [Route("api/MaterialTable/[controller]")]
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
        public async Task<IActionResult> Get()
        {
            var userID = User.GetSID();
            if (userID == null)
                return Forbid();

            return Ok(new Models.FrontEndDataObject.MaterialTable
            {
                title = typeof(ExpenditureType).Name,
                columns = typeof(ExpenditureType).GetMaterialTableColumns(null),
                data = await _context.ExpenditureType.Where(x => x.OwnerID == userID)
                .Select(x => new { x.ID, x.TypeName })
                .AsNoTracking().ToArrayAsync()
            });
        }
    }
}
