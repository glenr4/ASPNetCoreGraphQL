using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NHLStats.Core.Models;
using NHLStats.Data;
using System.Threading.Tasks;

namespace NHLStats.Api.Controllers.Rest
{
    [Route("[controller]")]
    public class RestController : Controller
    {
        private readonly NHLStatsContext _ctx;

        public RestController(NHLStatsContext ctx)
        {
            _ctx = ctx;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPlayerWithAddresses(int id)
        {
            Player result = await _ctx.Players.Include(p => p.Addresses).FirstAsync(p => p.Id == id);

            return Ok(result.Adapt<PlayerDto>());
        }
    }
}