using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Model
{
    public class GetAdministratorByIdResponse
    {
        public int Id { get; set; }
        public string Account { get; set; } = null!;
        public string Name { get; set; } = null!;
        public int GroupId { get; set; }
        public bool IsEnabled { get; set; }
    }
}
