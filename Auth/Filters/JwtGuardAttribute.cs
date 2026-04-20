using Microsoft.IdentityModel.Tokens;
using System;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Auth.DAL; 

namespace Auth.Filters
{
    public class JwtGuardAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            try
            {
                string authHeader = httpContext.Request.Headers["Authorization"];

                if (string.IsNullOrEmpty(authHeader))
                    return false;

                if (!authHeader.StartsWith("Bearer "))
                    return false;

                string token = authHeader.Substring(7);

                var secret = ConfigurationManager.AppSettings["JwtSecret"];
                var issuer = ConfigurationManager.AppSettings["JwtIssuer"];

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
                        IssuerSigningKey = new SymmetricSecurityKey(key),

                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero
                    },
                    out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;

                long userId = Convert.ToInt64(
                    jwtToken.Claims.First(x =>
                    x.Type.Contains("nameidentifier")).Value
                );

               
                var _authDAL = new AuthDAL();
                var user = _authDAL.GetUserById(userId);

                if (user == null || !user.IsActive)
                {
                    return false;
                }

                // Save fresh DB values
                httpContext.Items["UserID"] = user.UserID;
                httpContext.Items["Email"] = user.Email;
                httpContext.Items["UserName"] = user.UserName;
                httpContext.Items["Role"] = user.UserTypeCode ?? "User";

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
                    message = "Invalid or expired token"
                },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
    }
}