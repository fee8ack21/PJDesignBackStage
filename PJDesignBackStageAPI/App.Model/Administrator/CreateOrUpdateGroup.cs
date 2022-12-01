using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Model
{
    public class CreateOrUpdateGroupRequest
    {
        public int? Id { get; set; }
        public string Name { get; set; } = null!;
        public List<UnitRight>? UnitRights { get; set; }
    }

    public class UnitRight
    {
        public int UnitId { get; set; }
        public int RightId { get; set; }
    }
}
