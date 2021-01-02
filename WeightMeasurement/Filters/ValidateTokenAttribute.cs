using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeightMeasurement.Data;

namespace WeightMeasurement.Filters
{
    public class ValidateTokenAttribute : Attribute, IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            var http = context.HttpContext;
            var db = http.RequestServices.GetService(typeof(ApplicationDbContext)) as ApplicationDbContext;
            var sessionToken = http.Request.Headers["Authorization"][0];

            Guid.TryParse(sessionToken, out Guid token);

            void setUnauthorize()
            {
                http.Response.StatusCode = 401;
                context.Result = new ContentResult()
                {
                    StatusCode = 401,
                    Content = "Unauthorized"
                };
            }

            try
            {
                if (token == Guid.Empty)
                {
                    setUnauthorize();
                    return;
                }

                var accessToken = db.UserTokens.SingleOrDefault(m => m.Token == token);

                if (accessToken == null || DateTime.UtcNow > accessToken?.Expiration)
                {
                    setUnauthorize();
                }
            }
            catch (Exception)
            {
                setUnauthorize();
            }
        }

        public void OnActionExecuted(ActionExecutedContext context) { }
    }
}
