using App.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using System.Linq;

namespace App.PL.Filters
{
    public class JWTFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            try
            {
                bool result = context.HttpContext.Request.Headers.TryGetValue("Authorization", out var _fullToken);
                if (!result || StringValues.IsNullOrEmpty(_fullToken))
                {
                    throw new Exception();
                }

                var fullToken = _fullToken.ToString();
                if (!fullToken.Contains("Bearer "))
                {
                    throw new Exception();
                }

                var jwtToken = fullToken.Substring(7).Trim();
                var payload = JWTHelper.DecodePayload(jwtToken);

                if(payload.ExpiredTime <= DateHelper.GetNowDate())
                {
                    throw new Exception();
                }

                context.HttpContext.Items["jwtPayload"] = payload;

                base.OnActionExecuting(context);
            }
            catch
            {
                context.Result = new UnauthorizedResult();
                return;
            }
        }
    }
}
