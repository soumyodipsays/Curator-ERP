using Auth.DAL;
using Auth.DTOs;
using EmailService.Controllers;
using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Web.Mvc;
using Auth.Filters;

namespace Auth.Controllers
{
    public class UserController : Controller
    {
        private readonly AuthDAL _authDal = new AuthDAL();
        private readonly EmailController _emailSVC = new EmailController();

        [HttpPost]
        public JsonResult Register(UserRegistrationDto model)
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
        public JsonResult Login(UserLoginDTO model)
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

                var user = _authDal.ValidateUser(model);

                if (user == null)
                {
                    return Json(new
                    {
                        success = false,
                        message = "Invalid credentials"
                    });
                }

                var jwt = new JwtService();

                string token = jwt.GenerateToken(
                    user.UserID,
                    user.Email,
                    user.UserTypeCode
                );
                return Json(new
                {
                    success = true,
                    message = "User logged in successfully",
                    accessToken = token,
                    user = user
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

        public async Task<JsonResult> SendOTP(string email)
        {
            dynamic response = await _emailSVC.EmailHandler(email);
            _authDal.InsertUpdateOTP(response.data.otp);
            return Json(response);
        }

        public JsonResult ValidateOTP(OTP_DTO model)
        {
            try
            {
                if (model == null)
                {
                    return Json(new { success = false, message = "Invalid request" });
                }
                if (string.IsNullOrWhiteSpace(model.Email) ||
                    string.IsNullOrWhiteSpace(model.OTP))
                {
                    return Json(new { success = false, message = "Email & OTP required" });
                }
                _authDal.ValidateOTP(model);
                return Json(new
                {
                    success = true,
                    message = "OTP validated successfully",
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

        [JwtGuard]
        [HttpPost]
        public JsonResult GetProfile()
        {
            long userId =
                Convert.ToInt64(HttpContext.Items["UserID"]);

            string email =
                HttpContext.Items["Email"].ToString();

            return Json(new
            {
                success = true,
                userId = userId,
                email = email
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ResetPassword(UserLoginDTO model)
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
                _authDal.ResetPassword(model);
                return Json(new
                {
                    success = true,
                    message = "Password reset successfully",
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