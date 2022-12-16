using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Model
{
    public class CreateOrUpdateType1ContentRequest
    {
        public int UnitId { get; set; }
        public string Content { get; set; } = null!;
        public byte EditStatus { get; set; }
        public ReviewNote? Note { get; set; }
    }
}
