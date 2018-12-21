using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NHLStats.Core.Models;
using NHLStats.Data;
using Serilog;
using System.Diagnostics;
using System.Threading.Tasks;

namespace NHLStats.Api.Controllers.Rest
{
    [Route("[controller]")]
    public class RestController : Controller
    {
        private readonly NHLStatsContext _ctx;
        private readonly ILogger _logger;

        public RestController(NHLStatsContext ctx, ILogger logger)
        {
            _ctx = ctx;
            _logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPlayerWithAddresses(int id)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            //_logger.Information($"GetPlayerWithAddresses started");

            Player result = await _ctx.Players
                                    .Include(p => p.Addresses)
                                    .AsNoTracking()
                                    .FirstAsync(p => p.Id == id);
            PlayerDto resultDto = result.Adapt<PlayerDto>();

            sw.Stop();
            //_logger.Information($"GetPlayerWithAddresses finished after {sw.ElapsedMilliseconds}ms");
            _logger.Information($"GetPlayerWithAddresses: {sw.ElapsedMilliseconds}ms");

            return Ok(resultDto);
        }
    }
}