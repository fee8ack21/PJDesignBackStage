using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Model
{
    public class GetPortfolioByIdResponse : EditResponseBase
    {
        public int Id { get; set; }
        public bool IsBefore { get; set; }
        public string Title { get; set; } = null!;
        public string ThumbnailUrl { get; set; } = null!;
        public DateTime CreateDt { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime? Date { get; set; }
        public List<Category>? Categories { get; set; }
        public List<string>? Photos { get; set; }
    }
}
