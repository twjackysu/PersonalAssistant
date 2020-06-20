using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PersonalAssistant.Data;
using PersonalAssistant.Extension;
using PersonalAssistant.Models.AccountManager;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PersonalAssistant.Controllers.MaterialTable
{
    [Authorize]
    [ApiController]
    [Route("api/MaterialTable/[controller]")]
    public class AccountInitializationsController : Controller
    {
        private readonly ILogger<AccountInitializationsController> _logger;
        private readonly ApplicationDbContext _context;

        public AccountInitializationsController(ILogger<AccountInitializationsController> logger, ApplicationDbContext context)
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
                title = typeof(AccountInitialization).Name,
                columns = typeof(AccountInitialization).GetMaterialTableColumns(null),
                data = await _context.AccountInitialization.Where(x => x.OwnerID == userID)
                .Select(x => new
                {
                    x.ID,
                    x.Name,
                    x.Balance,
                    EffectiveDate = x.EffectiveDate.ToString("yyyy-MM-dd")
                })//remove user sid to reduce json size
                .AsNoTracking().ToArrayAsync()
            });
        }
    }
}
