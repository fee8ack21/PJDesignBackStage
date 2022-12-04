using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Model
{
    public class GetSettingByUnitIdResponse
    {
        public int UnitId { get; set; }
        public object? Content { get; set; }
        public int? EditorId { get; set; }
        public string? EditorName { get; set; }
        public int? ReviewerId { get; set; }
        public object? Notes { get; set; }
        public byte? Status { get; set; }
        public DateTime CreateDt { get; set; }
    }
}
