using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Client.Data;
using Client.Models;

namespace Client.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Level1Controller : ControllerBase
    {
        private readonly ClientContext _context;

        public Level1Controller(ClientContext context)
        {
            _context = context;
        }

        // GET: api/Level1
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Level1>>> GetLevel1()
        {
            Level1 level1 = new Level1();
            level1.Name = "Test";
            _context.Level1.Add(level1);
            await _context.SaveChangesAsync();
            return await _context.Level1.ToListAsync();
        }
    }
}
