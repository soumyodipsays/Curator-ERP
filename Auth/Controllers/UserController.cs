using Auth.DAL;
using Auth.DTOs;
using System;
using System.Web.Mvc;

namespace Auth.Controllers
{
    public class UserController : Controller
    {
        private readonly AuthDAL _authDal = new AuthDAL();

        [HttpPost]
        public ActionResult Register(UserRegistrationDto model)
        {
            try
            {
                if (model == null)
                {
                    return Json(new { success = false, message = "Invalid request" });
                }

                if (string.IsNullOrWhiteSpace(model.UserName) ||
                    string.IsNullOrWhiteSpace(model.Password))
                {
                    return Json(new { success = false, message = "Username & Password required" });
                }

                long newUserId = _authDal.InsertUpdateUser(model);

                return Json(new
                {
                    success = true,
                    message = model.UserID == null || model.UserID == 0
                                ? "User registered successfully"
                                : "User updated successfully",
                    userId = newUserId
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }
    }
}