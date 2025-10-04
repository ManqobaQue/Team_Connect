using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;
using CompanyPhonebook.Data;
using CompanyPhonebook.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace CompanyPhonebook.Filters

{
    public class AdminLoggingFilter : IActionFilter
    {

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var user = context.HttpContext.User;

            if (user.Identity != null && user.Identity.IsAuthenticated && user.IsInRole("Admin"))
            {
                var db = context.HttpContext.RequestServices.GetRequiredService<PhonebookContext>();

                var log = new AdminLog
                {
                    Adminusername = user.Identity.Name,
                    ControllerName = context.RouteData.Values["controller"]?.ToString(),
                    ActionName = context.RouteData.Values["action"]?.ToString(),
                    IPAdress = context.HttpContext.Connection.RemoteIpAddress?.ToString(),
                    TimeStamp = DateTime.UtcNow
                };

               // db.AdminLogs.Add(log);
                db.SaveChanges();
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            // No post-action logic
        }

    }
}
