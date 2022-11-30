using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Model
{
    public class CreateGroupRequest
    {
        public string Name { get; set; } = "";
    }

    public class CreateGroupResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
    }
}
