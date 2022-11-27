using App.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Model
{
    public class RequestBase
    {
    }

    public class ResponseBase<T>
    {
        public string Message { get; set; } = "";
        public T? Entries { get; set; }
        public StatusCode StatusCode { get; set; } = StatusCode.Success;
    }

    public class ListRequestBase : RequestBase
    {
        public int? PageIndex { get; set; }
        public int? PageSize { get; set; }
    }

    public class ListResponseBase<T> : ResponseBase<T>
    {
        public int? TotalItems { get; set; }
    }
}
