using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Model
{
    public class GetUnitsRequest
    {
        public int? StageType { get; set; }
        public int? GroupId { get; set; }
        public int? TemplateType { get; set; }
    }

    public class GetUnitsResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? BackStageUrl { get; set; }
        public int TemplateType { get; set; }
        public string? FrontStageUrl { get; set; }
        public bool IsAnotherWindow { get; set; }
        public bool? IsEnabled { get; set; }
        public DateTime CreateDt { get; set; }
        public int? Parent { get; set; }
        public int StageType { get; set; }
        public byte? Sort { get; set; }
    }
}
