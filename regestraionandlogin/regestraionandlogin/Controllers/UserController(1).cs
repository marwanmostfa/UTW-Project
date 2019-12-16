using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Mail;
using System.Net;
using System.Web.Security;
using regestraionandlogin.Models;

namespace regestraionandlogin.Controllers
{
    public class UserController : Controller
    {
        // registration action
        [HttpGet]
        public ActionResult Registration()
        {
            return View();

        }

        // reg. post action

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registration([Bind(Exclude = "IsEmailverified ,ActivationCode")] User user)
        {
            bool Status = false;
            string message = "";
            // model validation
            if (ModelState.IsValid)
            {
                #region // email is already exist 
                var isExist = IsEmailExist(user.EmailID);
                if (isExist)
                {
                    ModelState.AddModelError("EmailExist", "email is already exist");
                    return View(user);
                }


                #endregion

                #region  generate activTION code
                user.ActivationCode = Guid.NewGuid();
                #endregion
                #region password hashing
                user.Password = Crypto.Hash(user.Password);
                user.ConfirmPassword = Crypto.Hash(user.ConfirmPassword); 
                #endregion 
                user.IsEmailverified = false;
                #region  save data to database
                using (MyDatabaseEntities3 dc = new MyDatabaseEntities3())
                {
                    dc.Users.Add(user);
                    dc.SaveChanges();

                    //send email to user
                    SendVerificationLinkEmail(user.EmailID, user.ActivationCode.ToString());
                    message = "Registration Successfully done.Account Activation link " +
                        "has been sent to your email :" + user.EmailID;
                    Status = true;
                }

                #endregion
            }
            else
            {
                message = "invalid request";
            }



            ViewBag.message = message;
            ViewBag.Status = Status;
            return View(user);
        }

        // verify account
        [HttpGet]
        public ActionResult VerifyAccount(string id)
        {
            bool Status = false;
            using (MyDatabaseEntities3 dc = new MyDatabaseEntities3())
            {
                dc.Configuration.ValidateOnSaveEnabled = false;   // this line i have added to avoid confirm pass that doesnot match issue on save changes
                var v = dc.Users.Where(a => a.ActivationCode == new Guid(id)).FirstOrDefault();
                if (v != null)
                {
                    v.IsEmailverified = true;
                    dc.SaveChanges();
                    Status = true;

                }
                else
                {
                    ViewBag.Message = "Request is invalid";
                }
            }
            ViewBag.Status = Status;
            return View();

        }




        // login
        [HttpGet]
        public ActionResult Login()
        {
            return View();

        }


        // login post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UserLogin login ,string returnUrl)
        {
            string message = "";
            using (MyDatabaseEntities3 dc = new MyDatabaseEntities3())
            {
                var v = dc.Users.Where(a => a.EmailID == login.EmailID).FirstOrDefault();
                if (v != null)
                {
                    if (string.Compare(Crypto.Hash(login.Password), v.Password) == 0)
                    {
                        int timeout = login.RememberMe ? 525600 : 20; //525600min =1 year
                        var ticket = new FormsAuthenticationTicket(login.EmailID, login.RememberMe, timeout);
                        string encrypted = FormsAuthentication.Encrypt(ticket);
                        var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrypted);
                        cookie.Expires = DateTime.Now.AddMinutes(timeout);
                        cookie.HttpOnly= true;
                        Response.Cookies.Add(cookie);
                        if (Url.IsLocalUrl(returnUrl))
                        {
                            return Redirect(returnUrl);
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }

                    }


                    else
                    {
                        message = "invalid credintial provided";
                    }
                }
                else
                {
                    message = "invalid credintial provided";
                }

            }

            ViewBag.Message = message;
            return View();

        }

        // logout
        [Authorize]
        [HttpPost]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "User");
        }






        [NonAction]
        public bool IsEmailExist(string emailID)
        {
            using (MyDatabaseEntities3 dc =new MyDatabaseEntities3())
            {
                var v = dc.Users.Where(a => a.EmailID == emailID).FirstOrDefault();
                return v != null;
            }
        }
        [NonAction]
        public void SendVerificationLinkEmail(string emailID,string activationCode, string emailFor = "VerifyAccount")
        {
            var verifyUrl = "/User/"+emailFor+"/" + activationCode;
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);
            var fromemail = new MailAddress("mohamed.hassan9623@gmail.com", "mohamed hassan");
            var toemail = new MailAddress(emailID);
            var fromemailpassword = "mohamed.9623"; // replace with actual password
            string subject = "";
            string body = "";

            if (emailFor== "VerifyAccount")

            {
                 subject = "Your account is successfully created!";
                 body = " <br/><br/> we are excited to tell you that your .net awesome account is" +
                     "Successfully created. Please click on the below link to verify account" + "<br/><br/><a href='" + link + "'>" + link + "</a>";
            }
            else if (emailFor == "ResetPassword")
            {
                subject = "Reset Password";
                body = "Hi,<br/><br/> we got request for reset your acoount password. please click on the below link to reset your Password" +
                    "<br/><br/> <a href=" + link + ">Reset Password link </a> ";
            }
           
            var smpt = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl=true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials=false,
                Credentials=new NetworkCredential(fromemail.Address,fromemailpassword) 

            };
            using (var message = new MailMessage(fromemail, toemail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
                smpt.Send(message);
      
        }

        // forget pass
        public ActionResult ForgetPassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ForgetPassword (string EmailID)
        {
            //verify email id
            // generate reset password link 
            //send email
            string message = "";
            bool status = false;
            using(MyDatabaseEntities3 dc=new MyDatabaseEntities3())
            {
                var account = dc.Users.Where(a => a.EmailID == EmailID).FirstOrDefault();
                if (account!= null)
                {
                    //send email for reset pass
                    string resetCode = Guid.NewGuid().ToString();
                    SendVerificationLinkEmail(account.EmailID, resetCode, "ResetPassword");
                    account.ResetPasswordCode = resetCode;
                    // this line to avoid confirm pass not match issue...
                    dc.Configuration.ValidateOnSaveEnabled = false;
                    dc.SaveChanges();
                    message = "reset password link has been sent to your Email.";


                }
                else
                {
                    message = "account not found";
                }
            }
            ViewBag.Message = message;
            return View();
        }
        public ActionResult ResetPassword(string id)
        {
            // verify reset password link 
            // find accont associated with this link
            // redirect to reset password page

            using(MyDatabaseEntities3 dc =new MyDatabaseEntities3())
            {
                var user = dc.Users.Where(a => a.ResetPasswordCode == id).FirstOrDefault();
                if(user!= null)
                {
                    ResetPasswordModel model = new ResetPasswordModel();
                    model.ResetCode = id;
                    return View(model);
                }
                else
                {
                    return HttpNotFound();
                }
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword (ResetPasswordModel model)
        {
            var message = "";
            if (ModelState.IsValid)
            {
                using(MyDatabaseEntities3 dc=new MyDatabaseEntities3())
                {
                    var user = dc.Users.Where(a => a.ResetPasswordCode == model.ResetCode).FirstOrDefault();
                    if (user != null)
                    {
                        user.Password = Crypto.Hash(model.NewPassword);
                        user.ResetPasswordCode = "";
                        dc.Configuration.ValidateOnSaveEnabled = false;
                        dc.SaveChanges();
                        message = "New Password Updated Successfully.";
                    }
                }

            }

            else
            {
                message = "something invalid";
            }
            ViewBag.Message = message;
            return View(model);
        }
    }
}