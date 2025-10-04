using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Webapi.Interfaces;

namespace Webapi.Helpers
{
    public class CookieHelper : ICookieService
    {
        private readonly IHttpContextAccessor _httpContext;
        private readonly HttpContext? http;
        public CookieHelper(IHttpContextAccessor httpContext)
        {
            this._httpContext = httpContext;
            this.http = this._httpContext.HttpContext;
        }

        public void SetCookie(string? key, string value)
        {
            DateTimeOffset expirationTime = DateTimeOffset.UtcNow.AddMinutes(10);
            CookieOptions cookie = new CookieOptions
            {
                Expires = expirationTime,
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict
            };
            this.http?.Response.Cookies.Append(key, value, cookie);
        }

        public string? GetCookie(string key)
        {
            return this.http?.Request.Cookies[key];
        }

        public void DeleteCookie(string key)
        {
            this.http?.Response.Cookies.Delete(key);
        }
    }
}