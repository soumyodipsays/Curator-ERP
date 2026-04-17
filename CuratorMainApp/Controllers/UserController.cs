using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CuratorMainApp.Controllers
{
    public class UserController : Controller
    {
        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }

        public ActionResult ResetPassword()
        {

            return View();
        }

        [HttpGet]
        public ActionResult VerifyOTP()
        {
            return View();
        }
    }
}