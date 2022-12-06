using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Model
{
    public class GetQuestionByIdResponse
    {
        public int Id { get; set; }
        public bool IsBefore { get; set; }
        public string Title { get; set; } = null!;
        public DateTime CreateDt { get; set; }
        public DateTime EditDt { get; set; }
        public int EditorId { get; set; }
        public byte EditStatus { get; set; }
        public bool IsEnabled { get; set; }
        public string Content { get; set; } = null!;
        public string? Notes { get; set; }
        public List<Category>? Categories { get; set; }
    }
}
