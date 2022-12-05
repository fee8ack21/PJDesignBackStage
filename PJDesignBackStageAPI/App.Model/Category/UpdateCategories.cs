using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Model
{
    public class UpdateCategoriesRequest
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public bool IsEnabled { get; set; }
    }
}
