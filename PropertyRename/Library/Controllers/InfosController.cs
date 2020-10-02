using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Library.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Routing;
using Library.Models;
using System.Data;

namespace Library.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InfosController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly LinkGenerator _linkGenerator;

        public InfosController(IUnitOfWork context, LinkGenerator generator)
        {
            _unitOfWork = context;
            _linkGenerator = generator;
        }

        // GET: api/Infos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Models.HyperMediaControls.APIModelExample>>> GetAPIs()
        {
            ResetInfo().Wait();
            var conn = _unitOfWork.DBContext.Database.GetDbConnection();
            if (conn.State != ConnectionState.Open)
                conn.Open();

            _unitOfWork.DBContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Info ON");
            Info info = new Library.Models.Info() { Id = 1 };
            info.Title = "Info";
            await _unitOfWork.Repository<Info>().InsertEntityAsync(info);

            await _unitOfWork.SaveChangesAsync();
            conn.Close();

            Add(info);

            var list = await _unitOfWork.Repository<Info>().ToListAsync();
            return await _unitOfWork.Repository<Info>().ToListAsync();
        }

        private async Task ResetInfo()
        {
            Info info = await _unitOfWork.Repository<Info>().FirstOrDefault();
            if (info != null)
            {
                _unitOfWork.Repository<Info>().Empty(info);
                await _unitOfWork.SaveChangesAsync();
            }
        }

        public void Add(Models.HyperMediaControls.APIModelExample info)
        {
            info.AddInfo(_linkGenerator, _unitOfWork);
        }
    }

}
