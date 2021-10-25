using PersonalAssistant.Models;
using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PersonalAssistant.Models.AccountManager;

namespace PersonalAssistant.Data
{
    public class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUser>
    {
        public ApplicationDbContext(
            DbContextOptions options,
            IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
        {
        }
        public DbSet<ExpenditureType> ExpenditureType { get; set; }
        public DbSet<ExpenditureWay> ExpenditureWay { get; set; }
        public DbSet<AccountInitialization> AccountInitialization { get; set; }
        public DbSet<Expenditure> Expenditure { get; set; }
        public DbSet<Income> Income { get; set; }
        public DbSet<InternalTransfer> InternalTransfer { get; set; }
        public DbSet<StockInitialization> StockInitialization { get; set; }
        public DbSet<StockTransaction> StockTransaction { get; set; }
    }
}
