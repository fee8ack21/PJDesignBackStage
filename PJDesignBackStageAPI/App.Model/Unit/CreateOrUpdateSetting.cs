using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Model
{
    public class CreateOrUpdateSettingRequest
    {
        public int UnitId { get; set; }
        public object? Content { get; set; }
        public byte EditStatus { get; set; }
        public SettingNote? Note { get; set; }
    }

    public class SettingNote
    {
        public string Name { get; set; } = null!;
        public DateTime Date { get; set; }
        public string Note { get; set; } = null!;
    }
}
