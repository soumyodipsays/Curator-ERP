using System.Configuration;
using System.Web.Mvc;

namespace Auth.Controllers
{
    public class LoginController : Controller
    {
        [HttpPost]
        public ActionResult Verify(string orgName, string userEmail)
        {
            return Content($"Auth Project Received! <br/> " +
                           $"Org: {orgName} <br/> " +
                           $"User: {userEmail}");
        }
    }
}