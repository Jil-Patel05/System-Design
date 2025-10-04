using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Webapi.Interfaces
{
    public interface ICookieService
    {
        public void SetCookie(string? key, string value);
        public string? GetCookie(string key);
        public void DeleteCookie(string key);
    }
}