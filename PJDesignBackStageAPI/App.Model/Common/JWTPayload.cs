using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Model
{
    public class JwtPayload
    {
        public int Id { get; set; }
        public string? Account { get; set; }
        public int GroupId { get; set; }
        public DateTime ExpiredTime { get; set; }
    }
}
