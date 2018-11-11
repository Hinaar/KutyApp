using KutyApp.Services.Environment.Bll.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace KutyApp.Services.Environment.Api.Controllers
{
    [AllowAnonymous]
    [Route("api/database")]
    public class DatabaseController : BaseController
    {
        private IDatabaseManager DatabaseManager { get; }

        public DatabaseController(IDatabaseManager databaseManager) =>
            DatabaseManager = databaseManager;

        [HttpGet("truncate")]
        public async Task<ActionResult> TruncateDatabase() =>
            await ResultAsync(DatabaseManager.TruncateDatabaseAsync());

        [HttpGet("seed")]
        public async Task<ActionResult> SeedDatabase() =>
            await ResultAsync(DatabaseManager.SeedDatabaseAsync());
    }
}
