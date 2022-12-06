using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Model
{
    public class GetFrontStageUnitsResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int TemplateType { get; set; }
        public bool IsAnotherWindow { get; set; }
        public bool? IsEnabled { get; set; }
        public DateTime CreateDt { get; set; }
        public int? Parent { get; set; }
        public int StageType { get; set; }
        public byte? Sort { get; set; }
    }
}
