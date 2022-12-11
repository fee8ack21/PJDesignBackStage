using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Model
{
    public class ReviewNote
    {
        public string Name { get; set; } = null!;
        public string Note { get; set; } = null!;
        public DateTime Date { get; set; }
    }
}
