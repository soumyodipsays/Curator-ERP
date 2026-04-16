using Microsoft.IdentityModel.Tokens;
using System;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Auth.Filters
{
    public class JwtGuardAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            try
            {
                string authHeader =
                    httpContext.Request.Headers["Authorization"];
                // check if the header is reaching
                System.Diagnostics.Debug.WriteLine(authHeader);

                if (string.IsNullOrEmpty(authHeader))
                    return false;

                if (!authHeader.StartsWith("Bearer "))
                    return false;

                string token = authHeader.Substring(7);

                var secret =
                    ConfigurationManager.AppSettings["JwtSecret"];

                var issuer =
                    ConfigurationManager.AppSettings["JwtIssuer"];

                var tokenHandler = new JwtSecurityTokenHandler();

                var key = Encoding.UTF8.GetBytes(secret);

                tokenHandler.ValidateToken(token,
                    new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = issuer,

                        ValidateAudience = true,
                        ValidAudience = issuer,

                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey =
                            new SymmetricSecurityKey(key),

                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero
                    },
                    out SecurityToken validatedToken
                );

                var jwtToken =
                    (JwtSecurityToken)validatedToken;

                // Save user data for controller use
                httpContext.Items["UserID"] =
                    jwtToken.Claims.First(x =>
                    x.Type.Contains("nameidentifier")).Value;

                httpContext.Items["Email"] =
                    jwtToken.Claims.First(x =>
                    x.Type.Contains("email")).Value;

                httpContext.Items["Role"] =
                    jwtToken.Claims.First(x =>
                    x.Type.Contains("role")).Value;

                return true;
            }
            catch
            {
                return false;
            }
        }

        protected override void HandleUnauthorizedRequest(
            AuthorizationContext filterContext)
        {
            filterContext.Result = new JsonResult
            {
                Data = new
                {
                    success = false,
                    message = "Unauthorized"
                },
                JsonRequestBehavior =
                    JsonRequestBehavior.AllowGet
            };
        }
    }
}