using Auth.DAL;
using Auth.DTOs;
using System;
using System.Data.SqlClient;
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
            catch (SqlException sqlex)
            {
                return Json(new
                {
                    success = false,
                    message = sqlex.Message
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

        [HttpPost]
        public ActionResult Login(UserLoginDTO model)
        {
            try
            {
                if (model == null)
                {
                    return Json(new { success = false, message = "Invalid request" });
                }

                if (string.IsNullOrWhiteSpace(model.Email) ||
                    string.IsNullOrWhiteSpace(model.Password))
                {
                    return Json(new { success = false, message = "Email & Password required" });
                }

                _authDal.ValidateUser(model);

                return Json(new
                {
                    success = true,
                    message =  "User logged in successfully", 
                });
            }
            catch (SqlException sqlex)
            {
                return Json(new
                {
                    success = false,
                    message = sqlex.Message
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