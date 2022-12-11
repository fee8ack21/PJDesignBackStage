using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Model
{
    public class GetQuestionsResponse
    {
        public int Id { get; set; }
        public int? AfterId { get; set; }
        public bool IsBefore { get; set; }
        public string Title { get; set; } = null!;
        public DateTime CreateDt { get; set; }
        public DateTime EditDt { get; set; }
        public int EditorId { get; set; }
        public byte? EditStatus { get; set; }
        public bool IsEnabled { get; set; }
        public IEnumerable<Category>? Categories { get; set; }
    }
}
