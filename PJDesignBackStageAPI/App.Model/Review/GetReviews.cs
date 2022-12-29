using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Model
{
    public class GetReviewsResponse : EditResponseBase
    {
        public int UnitId { get; set; }
        public string UnitName { get; set; } = null!;
        public int ContentId { get; set; }
        public string? Title { get; set; }
        public string? url { get; set; }
    }
}
