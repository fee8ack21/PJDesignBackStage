using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Model
{
    public class CreateOrUpdateUnitRequest
    {
        public int? Id { get; set; }
        public int? Parent { get; set; }
        public string Name { get; set; } = null!;
        public int TemplateType { get; set; }
        public string? Url { get; set; }
        public bool IsAnotherWindow { get; set; }
        public bool IsEnabled { get; set; }
    }
}
