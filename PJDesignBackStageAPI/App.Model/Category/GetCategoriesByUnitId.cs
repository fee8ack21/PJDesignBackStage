using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Model
{
    public class GetCategoriesByUnitIdResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public DateTime CreateDt { get; set; }
        public bool? IsEnabled { get; set; }
    }
}
