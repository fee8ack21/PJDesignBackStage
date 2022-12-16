using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Model
{
    public class GetType1ContentByUnitIdResponse
    {
        public int? Id { get; set; }
        public int? UnitId { get; set; }
        public bool IsBefore { get; set; }
        public DateTime CreateDt { get; set; }
        public DateTime EditDt { get; set; }
        public int EditorId { get; set; }
        public string EditorName { get; set; } = null!;
        public byte? EditStatus { get; set; }
        public string Content { get; set; } = null!;
        public List<ReviewNote>? Notes { get; set; }
    }
}
