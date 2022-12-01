using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Model
{
    public class GetUnitRightsByGroupIdRequest
    {
        public int Id { get; set; }
    }

    public class GetUnitRightsByGroupIdResponse
    {
        public int UnitId { get; set; }
        public int RightId { get; set; }
    }
}
