using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Common
{
    public class DateHelper
    {
        public static DateTime GetNowDate()
        {
            return DateTime.UtcNow.AddHours(8);
        }
    }
}
