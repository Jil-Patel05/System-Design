using Microsoft.AspNetCore.Mvc;
using Webapi.Interfaces;
using Webapi.Models;

namespace Webapi.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class Auth : ControllerBase
    {
        private readonly ILogger<Auth> _logger;
        private readonly ICookieService _cookieService;
        private readonly IConfiguration _config;

        public Auth(ILogger<Auth> logger, ICookieService cookieService, IConfiguration config)
        {
            _logger = logger;
            _cookieService = cookieService;
            _config = config;
        }

        [HttpPost("login")]
        public ActionResult<User> setHttpCookies(User user)
        {
            AuthHelper helper = new AuthHelper(_logger, _cookieService, _config);
            helper.setHttpCookies(user);
            return Ok(user);
        }
    }
}