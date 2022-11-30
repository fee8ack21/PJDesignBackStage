using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Model
{
    public class AuthLoginRequest
    {
        public string Account { get; set; } = "";
        public string Password { get; set; } = "";
    }

    public class AuthLoginResponse
    {
        public string Name { get; set; } = "";
        public string Token { get; set; } = "";
    }
}
