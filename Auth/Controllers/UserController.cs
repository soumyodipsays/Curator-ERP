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
        public async Task<JsonResult> Register(UserRegistrationDto model)
        {
            try
            {
                if (model == null)
                    return Json(new { success = false, message = "Invalid request" });

                if (string.IsNullOrWhiteSpace(model.UserName) ||
                    string.IsNullOrWhiteSpace(model.Password))
                {
                    return Json(new
                    {
                        success = false,
                        message = "Username & Password required"
                    });
                }

                long newUserId = _authDal.InsertUpdateUser(model);

                JsonResult otpResponse = await SendOTP(model.Email);

                dynamic otpData = otpResponse.Data;
                bool isSuccess = (bool?)otpData.GetType()
                                    .GetProperty("success")
                                    ?.GetValue(otpData, null) ?? false;

                if (otpData == null || !isSuccess)
                {
                    return Json(new
                    {
                        success = false,
                        message = "User created but OTP failed."
                    });
                }

                return Json(new
                {
                    success = true,
                    message = "Registration successful",
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

        [HttpPost]
        public async Task<JsonResult> SendOTP(string email)
        {
            // Calling the Email Service project
            var response = await _emailSVC.EmailHandler(email);

            string otpValue = response.GetType().GetProperty("data")?.GetValue(response, null)?.ToString() ?? "";
            bool isSent = (bool?)response.GetType().GetProperty("success")?.GetValue(response, null) ?? false;

            if (isSent)
            {
                var otpDto = new OTP_DTO
                {
                    Email = email,
                    OTP = otpValue
                };
                _authDal.InsertUpdateOTP(otpDto);

                return Json(new { success = true, message = "OTP Sent" });
            }

            return Json(new { success = false, message = "Failed to send email" });
        }

        //public async Task<JsonResult> SendOTP(string email)
        //{
        //    dynamic response = await _emailSVC.EmailHandler(email);
        //    //string otpValue = response?.data?.ToString() ?? string.Empty;

        //    string otpValue = response.GetType()
        //                  .GetProperty("data")
        //                  ?.GetValue(response, null)
        //                  ?.ToString() ?? "";

        //    bool isSent = (bool?)response.GetType()
        //                     .GetProperty("success")
        //                     ?.GetValue(response, null) ?? false;

        //    //bool isSent = response.success ?? false;
        //    if(isSent)
        //    {
        //        var otpDto = new OTP_DTO
        //        {
        //            Email = email,
        //            OTP = otpValue
        //        };
        //        _authDal.InsertUpdateOTP(otpDto);
        //    }

        //    return Json(response, JsonRequestBehavior.AllowGet);
        //}

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