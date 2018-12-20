using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NHLStats.Core.Models;
using NHLStats.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NHLStats.Api.Controllers
{
    [Route("[controller]")]
    public class RestController : Controller
    {
        private readonly NHLStatsContext _ctx;

        public RestController(NHLStatsContext ctx)
        {
            this._ctx = ctx;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPlayerWithAddresses(int id)
        {
            var result = await _ctx.Players.Include(p => p.Addresses).FirstAsync(p => p.Id == id);

            return Ok(result);
        }

        //[HttpGet("{id}")]
        //public Task<Player> GetPlayerWithAddresses(int id)
        //{
        //    return _ctx.Players.Include(p => p.Addresses).FirstAsync(p => p.Id == id);
        //}
    }
}
