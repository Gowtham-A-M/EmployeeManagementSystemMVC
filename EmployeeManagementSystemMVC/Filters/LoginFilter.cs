using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EmployeeManagementSystemMVC.Filters
{
    public class LoginFilter : IActionFilter
    {

        public void OnActionExecuting(
            ActionExecutingContext context)
        {

            var session =
                context.HttpContext.Session;


            var admin =
                session.GetString("Admin");


            if (string.IsNullOrEmpty(admin))
            {
                context.Result =
                    new RedirectToActionResult(
                        "Index",
                        "Login",
                        null
                    );
            }

        }



        public void OnActionExecuted(
            ActionExecutedContext context)
        {

        }

    }
}