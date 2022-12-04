using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Model
{
    public class AuthLoginRequest
    {
        public string Account { get; set; } = null!;
        public string Password { get; set; } = null!;
    }

    public class AuthLoginResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Token { get; set; } = null!;
    }
}
