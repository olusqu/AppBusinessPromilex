using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;

namespace WebBusinessPromilexApp.Filters
{
    public class AdminOnlyAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var isLoggedIn = context.HttpContext.Session.GetString("IsAdminLoggedIn");

            Debug.WriteLine($"🛡️ [AdminOnly] IsAdminLoggedIn = '{isLoggedIn}'");
            Debug.WriteLine($"🛡️ [AdminOnly] Controller: {context.Controller.GetType().Name}");
            Debug.WriteLine($"🛡️ [AdminOnly] Action: {context.ActionDescriptor.DisplayName}");

            if (isLoggedIn != "true")
            {
                Debug.WriteLine(" [AdminOnly] Dostęp odrzucony - przekierowanie do logowania");
                context.Result = new RedirectToActionResult("Login", "Admin", null);
            }
            else
            {
                Debug.WriteLine(" [AdminOnly] Dostęp dozwolony");
            }

            base.OnActionExecuting(context);
        }
    }
}