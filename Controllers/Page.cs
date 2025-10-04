using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Webapi.Controllers
{
    [ApiController]
    [Route("api/page")]
    public class Page : ControllerBase
    {
        private readonly ILogger<Page> _logger;

        public Page(ILogger<Page> logger)
        {
            _logger = logger;
        }

        [HttpGet("results")]
        [Authorize]
        public ActionResult GetPageResults([FromQuery] int limit = 10)
        {
            Console.WriteLine(limit);
            return Ok();
        }

        [Authorize(Policy = "adminPolicy")]
        [HttpGet("admin/results")]
        public ActionResult GetAdminPageResults([FromQuery] int limit = 10)
        {
            Console.WriteLine(limit);
            return Ok();
        }

    }
}