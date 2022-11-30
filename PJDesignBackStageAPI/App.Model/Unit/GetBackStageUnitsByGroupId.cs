using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Model
{
    public class GetBackStageUnitsByGroupIdResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Url { get; set; }
        public bool IsAnotherWindow { get; set; }
        public int? Parent { get; set; }
        public int TemplateType { get; set; }
    }
}
