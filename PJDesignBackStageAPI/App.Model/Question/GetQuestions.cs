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
        public bool IsBefore { get; set; }
        public string Name { get; set; } = null!;
        public int Categories { get; set; }
        public DateTime CreateDt { get; set; }
        public DateTime EditDt { get; set; }
        public int EditorId { get; set; }
        public string? Content { get; set; };
        public byte Status { get; set; }
    }
}
