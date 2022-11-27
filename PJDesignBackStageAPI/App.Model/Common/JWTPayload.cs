using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Model
{
    public class JWTPayload
    {
        public int Id { get; set; }
        public string? Account { get; set; }
        public byte Level { get; set; }
        public DateTime ExpiredTime { get; set; }
    }
}
