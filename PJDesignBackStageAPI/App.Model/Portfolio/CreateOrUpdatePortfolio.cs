using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Model
{
    public class CreateOrUpdatePortfolioRequest
    {
        public int? Id { get; set; }
        public int? AfterId { get; set; }
        public bool IsEnabled { get; set; }
        public string Title { get; set; } = null!;
        public DateTime? Date { get; set; }
        public byte EditStatus { get; set; }
        public ReviewNote? Note { get; set; }
        public IEnumerable<int>? CategoryIDs { get; set; }
        public IEnumerable<string>? Photos { get; set; }
    }
}
