using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Webapi.Interfaces;
using Webapi.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace Webapi.Controllers
{
    public class AuthHelper
    {
        private readonly ILogger _logger;
        private readonly ICookieService _cookieService;
        private readonly IConfiguration _config;
        private User CurrentUser { get; set; }
        public AuthHelper(ILogger logger, ICookieService cookieService, IConfiguration config)
        {
            _logger = logger;
            _cookieService = cookieService;
            _config = config;
        }

        public string generateJWTToken()
        {
            string token = "";
            try
            {
                if (this.CurrentUser != null)
                {
                    IList<Claim> claims = new List<Claim>
                    {
                       new Claim("userName",this.CurrentUser.UserName),
                       new Claim(ClaimTypes.Role,this.CurrentUser.Role),
                       new Claim("GhLevel",this.CurrentUser.GhLevel.ToString()),
                    };
                    string? jwtKey = _config["Jwt:JwtSecret"];
                    if (jwtKey != null)
                    {
                        byte[] bytes = Encoding.UTF8.GetBytes(jwtKey);
                        SymmetricSecurityKey SymmetricSecurityKey = new SymmetricSecurityKey(bytes);
                        SigningCredentials credentials = new SigningCredentials(SymmetricSecurityKey, SecurityAlgorithms.HmacSha256);
                        JwtSecurityToken securityToken = new JwtSecurityToken(
                            issuer: _config["Jwt:Issuer"],
                            audience: _config["Jwt:Audience"],
                            notBefore: DateTime.Now,
                            claims: claims,
                            signingCredentials: credentials,
                            expires: DateTime.Now.AddMinutes(15)
                        );
                        token = new JwtSecurityTokenHandler().WriteToken(securityToken);
                    }
                    return token;
                }
                else
                {
                    throw new Exception("User should b not null field");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return token;
        }

        public void setHttpCookies(User user)
        {
            this.CurrentUser = user;
            string? key = _config["Jwt:CookieName"];
            string token = generateJWTToken();
            if (!string.IsNullOrEmpty(token))
            {
                _cookieService.SetCookie(key, token);
            }
        }
    }
}