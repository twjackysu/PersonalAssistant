using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PersonalAssistant.Data;
using PersonalAssistant.Extension;
using PersonalAssistant.Models.AccountManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PersonalAssistant.Controllers.MaterialTable
{
    [Authorize]
    [ApiController]
    [Route("api/MaterialTable/[controller]")]
    public class StockTransactionsController : Controller
    {
        private readonly ILogger<StockTransactionsController> _logger;
        private readonly ApplicationDbContext _context;

        public StockTransactionsController(ILogger<StockTransactionsController> logger, ApplicationDbContext context)
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

            var allForeignkey = new Dictionary<string, Dictionary<int?, string>>();
            allForeignkey.Add("AccountID", _context.AccountInitialization.Where(x => x.OwnerID == userID).ToDictionary(x => x.ID, x => x.Name));
            allForeignkey.Add("Type", (Enum.GetValues(typeof(StockTransactionType)) as StockTransactionType[]).ToDictionary(x => (int?)x, x => x.ToString()));

            return Ok(new Models.FrontEndDataObject.MaterialTable
            {
                title = typeof(StockTransaction).Name,
                columns = typeof(StockTransaction).GetMaterialTableColumns(allForeignkey),
                data = await _context.StockTransaction.Where(x => x.OwnerID == userID)
                .Select(x => new {
                    x.ID,
                    x.AccountID,
                    EffectiveDate = x.EffectiveDate.ToString("yyyy-MM-dd"),
                    x.Amount,
                    x.Fees,
                    x.Price,
                    x.StockCode,
                    x.TransactionType
                })
                .AsNoTracking().ToArrayAsync()
            });
        }
    }
}
