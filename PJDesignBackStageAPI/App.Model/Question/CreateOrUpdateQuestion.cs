using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Model
{
    public class CreateOrUpdateQuestionRequest
    {
        public int? Id { get; set; }
        public int? AfterId { get; set; }
        public bool IsEnabled { get; set; }
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
        public byte EditStatus { get; set; }
        public ReviewNote? Note { get; set; }
        public IEnumerable<int>? CategoryIDs { get; set; }
    }
}
