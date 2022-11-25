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
        public T? Entries { get; set; }
    }
}
