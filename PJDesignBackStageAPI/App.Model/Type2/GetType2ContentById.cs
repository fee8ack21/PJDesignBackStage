using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Model
{
    public class GetType2ContentByIdResponse : EditResponseBase
    {
        public int Id { get; set; }
        public int UnitId { get; set; }
        public string ThumbnailUrl { get; set; } = null!;
        public string ImageUrl { get; set; } = null!;
        public bool IsBefore { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime CreateDt { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsFixed { get; set; }
        public string Content { get; set; } = null!;
        public List<Category>? Categories { get; set; }
    }
}
