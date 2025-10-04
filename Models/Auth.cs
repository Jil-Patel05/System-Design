using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Webapi.Models
{
    public class User
    {
        [Required]
        public string? UserName { get; set; }
        [Required]
        public string? Password { get; set; }
        [Required]
        public string? Role { get; set; }
        [Required]
        public int GhLevel { get; set; }
    }
}