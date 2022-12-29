using App.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Model
{
    public class ResponseBase<T>
    {
        public string Message { get; set; } = "";
        public T? Entries { get; set; }
        public StatusCode StatusCode { get; set; } = StatusCode.Success;
    }

    public abstract class EditRequestBase
    {
        public int? AfterId { get; set; }
        public byte EditStatus { get; set; }
        public ReviewNote? Note { get; set; }
    }

    public abstract class EditResponseBase
    {
        public DateTime EditDt { get; set; }
        public int EditorId { get; set; }
        public string EditorName { get; set; } = null!;
        public byte? EditStatus { get; set; }
        public List<ReviewNote>? Notes { get; set; }
        public int? AfterId { get; set; }
    }
}
