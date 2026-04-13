using EmailService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Services.Description;
using static System.Net.WebRequestMethods;

namespace EmailService.Controllers
{
    public class EmailController : Controller
    {
        // GET: Email
        public ActionResult Index()
        {
            return View();
        }

        public string GetHTMLBody(string OTP)
        {
            string htmlBody = @"
                        <table width=""600"" cellpadding=""0"" cellspacing=""0"" style=""margin:0 auto; font-family:-apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, Arial, sans-serif;"">
                  <tr>
                    <td>
                      <table width=""100%"" cellpadding=""0"" cellspacing=""0"" style=""background:#ffffff; border:1px solid #e5e7eb;"">

                        <!-- Header -->
                        <tr>
                          <td style=""padding:32px 40px; border-bottom:1px solid #e5e7eb;"">
                            <h1 style=""margin:0; font-size:20px; font-weight:600; color:#111827;"">
                              Verification Code
                            </h1>
                          </td>
                        </tr>

                        <!-- Body -->
                        <tr>
                          <td style=""padding:40px;"">
            
                            <p style=""font-size:15px; line-height:1.6; color:#374151; margin:0 0 24px 0;"">
                              Hello,
                            </p>

                            <p style=""font-size:15px; line-height:1.6; color:#374151; margin:0 0 32px 0;"">
                              Your verification code is:
                            </p>

                            <!-- OTP Code -->
                            <table width=""100%"" cellpadding=""0"" cellspacing=""0"">
                              <tr>
                                <td style=""padding:0 0 32px 0;"">
                                  <div style=""background:#f9fafb; border:1px solid #d1d5db; padding:20px; text-align:center;"">
                                    <span style=""font-size:28px; font-weight:600; letter-spacing:6px; color:#111827; font-family:monospace;"">
                                      {{OTP_CODE}}
                                    </span>
                                  </div>
                                </td>
                              </tr>
                            </table>

                            <p style=""font-size:14px; line-height:1.6; color:#6b7280; margin:0 0 16px 0;"">
                              This code will expire in 10 minutes.
                            </p>

                            <p style=""font-size:14px; line-height:1.6; color:#6b7280; margin:0;"">
                              If you did not request this code, please ignore this email.
                            </p>

                          </td>
                        </tr>

                        <!-- Footer -->
                        <tr>
                          <td style=""padding:24px 40px; border-top:1px solid #e5e7eb; background:#f9fafb;"">
                            <p style=""font-size:13px; line-height:1.5; color:#9ca3af; margin:0; text-align:center;"">
                              © 2026 Curator
                            </p>
                          </td>
                        </tr>

                      </table>
                    </td>
                  </tr>
                </table>
                ";

            string finalBody = htmlBody.Replace("{{OTP_CODE}}", OTP.ToString());
            return finalBody;
        }

        public static string GenerateOtp()
        {
            Random random = new Random();
            int otp = random.Next(100000, 999999); 
            return otp.ToString();
        }

        [HttpPost]
        public async Task<object> SendEmail(EmailModel emailModel)
        {
            try
            {
                var message = new MailMessage();

                message.To.Add(new MailAddress(
                    emailModel.NameTo + " <" + emailModel.EmailTo + ">"));

                message.From = new MailAddress(
                    emailModel.NameFrom + " <" + emailModel.EmailFrom + ">");

                message.Subject = emailModel.EmailSubject;
                emailModel.OTP = GenerateOtp();
                message.Body = GetHTMLBody(emailModel.OTP);
                message.IsBodyHtml = true;

                using (var smtp = new SmtpClient())
                {
                    smtp.Send(message);
                }

                return new
                {
                    success = true,
                    message = "Mail sent successfully",
                    data = new
                    {
                        otp = emailModel.OTP
                    },
                    error = ""
                };
            }
            catch (Exception ex)
            {
                return new
                {
                    success = false,
                    message = "Mail sent failed",
                    data = new { },
                    error = ex.Message
                };
            }
        }

        public async Task<object> EmailHandler(string email)
        {
            EmailModel model = new EmailModel
            {
                EmailFrom = "subhradeepbasu2305@gmail.com",
                EmailTo = email,
                //OTP = "123456",
                //EmailBody = "Here is your OTP: 123456",
                EmailSubject = "Don't Reply. Just a Test mail.",
                NameFrom = "Curator",
                //NameTo = "Client"
            };

            dynamic response = await SendEmail(model);

            return response;
        }
    }
}
