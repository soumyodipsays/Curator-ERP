using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Auth.Filters
{
    public class RoleGuardAttribute : AuthorizeAttribute
    {
        private readonly string _role;

        public RoleGuardAttribute(string role)
        {
            _role = role;
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var role = httpContext.Items["Role"]?.ToString();

            return role == _role;
        }

        protected override void HandleUnauthorizedRequest(
            AuthorizationContext filterContext)
        {
            filterContext.Result = new JsonResult
            {
                Data = new
                {
                    success = false,
                    message = "Forbidden"
                },
                JsonRequestBehavior =
                    JsonRequestBehavior.AllowGet
            };
        }
    }
}