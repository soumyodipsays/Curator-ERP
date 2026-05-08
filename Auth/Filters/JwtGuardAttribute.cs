using Microsoft.IdentityModel.Tokens;
using System;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
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

                if (string.IsNullOrWhiteSpace(authHeader))
                    return false;

                if (!authHeader.StartsWith("Bearer "))
                    return false;

                string token = authHeader.Substring(7);

                string secret =
                    ConfigurationManager.AppSettings["JwtSecret"];

                string issuer =
                    ConfigurationManager.AppSettings["JwtIssuer"];

                var tokenHandler = new JwtSecurityTokenHandler();

                var key = Encoding.UTF8.GetBytes(secret);

                var validationParameters =
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
                    };

                var principal = tokenHandler.ValidateToken(
                    token,
                    validationParameters,
                    out SecurityToken validatedToken
                );

                var claims = principal.Claims;

                string userIdValue = claims
                    .FirstOrDefault(x =>
                        x.Type == ClaimTypes.NameIdentifier)
                    ?.Value;

                if (string.IsNullOrWhiteSpace(userIdValue))
                    return false;

                long userId = Convert.ToInt64(userIdValue);

                string email = claims
                    .FirstOrDefault(x =>
                        x.Type == ClaimTypes.Email)
                    ?.Value;

                string userName = claims
                    .FirstOrDefault(x =>
                        x.Type == ClaimTypes.Name)
                    ?.Value;

                string role = claims
                    .FirstOrDefault(x =>
                        x.Type == ClaimTypes.Role)
                    ?.Value ?? "User";

                // Store user info in request context
                httpContext.Items["UserID"] = userId;
                httpContext.Items["Email"] = email;
                httpContext.Items["UserName"] = userName;
                httpContext.Items["Role"] = role;

                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(
                    "JWT ERROR: " + ex.Message
                );

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
                    message = "Invalid or expired token"
                },
                JsonRequestBehavior =
                    JsonRequestBehavior.AllowGet
            };
        }
    }
}