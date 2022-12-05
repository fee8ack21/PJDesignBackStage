using Ganss.Xss;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Common
{
    public class HtmlHelper
    {
        public static string Sanitize(string htmlString)
        {
            var sanitizer = new HtmlSanitizer();
            sanitizer.AllowedSchemes.Add("data");
            return sanitizer.Sanitize(htmlString); ;
        }
    }
}
