using Amoura.Web.Data;
using Amoura.Web.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Amoura.Extensions.Email;
using umbraco.NodeFactory;
using Iomer.Umbraco.Extensions;
using Amoura.Web.Email;
using Umbraco.Core.Persistence;
using Umbraco.Core.Services;
using Umbraco.Core.Models;

namespace Amoura.Web.Controllers
{
    public class MembershipController : Umbraco.Web.Mvc.SurfaceController
    {
        protected string MemberGroup = "Members";
        protected string MemberType = "Member";

        [HttpPost]
        [ActionName("MemberLogin")]
        public ActionResult MemberLoginPost(MemberLoginModel model)
        {
            if (!this.TempData.ContainsKey("UserValidated"))
            {
                TempData.Add("UserValidated", false);
            }
            if (!this.TempData.ContainsKey("Message"))
            {
                TempData.Add("Message", string.Empty);
            }

            if (ModelState.IsValid)
            {
                var userName = model.Username ?? string.Empty;
                var password = model.Password ?? string.Empty;

                if (Membership.ValidateUser(userName, password))
                {
                    TempData["UserValidated"] = null;
                    FormsAuthentication.SetAuthCookie(userName, model.RememberMe);

                    var returnUrl = this.RedirectUrl();
                    if (returnUrl != string.Empty)
                    {
                        return this.Redirect(returnUrl);
                    }
                    else
                    {
                        return this.Redirect("/members/");
                    }
                }
                else
                {
                    this.TempData["UserValidated"] = false;
                    this.TempData["Message"] = "Invalid Login Credentials.<br /><span>Contact <a href=\"mailto:info@msamoura.com\" target=_blank>info@msamoura.com</a> for assistance.</span>";
                }
            }
            else
            {
                this.TempData["UserValidated"] = false;
                this.TempData["Message"] = "Invalid Login Credentials.<br /><span>Contact <a href=\"mailto:info@msamoura.com\" target=_blank>info@msamoura.com</a> for assistance.</span>";
                
            }
            //error message, add custom error to viewstate model and return the page.
            ModelState.AddModelError("CustomError", new Exception(this.TempData["Message"].ToString()));
            return CurrentUmbracoPage();

        }

        [HttpGet]
        [ActionName("MemberLogout")]
        public ActionResult MemberLogoutPost(MemberLoginModel model)
        {
            Session.Clear();
            FormsAuthentication.SignOut();
            var defaultLoginPage = ConfigurationManager.AppSettings["defaultLoginPage"];
            return Redirect(defaultLoginPage);
        }

        public string MemberLogout()
        {
            //HttpContext.Session.Clear();
            FormsAuthentication.SignOut();
            var defaultLoginPage = ConfigurationManager.AppSettings["defaultLoginPage"];
            return defaultLoginPage;
        }

        [HttpPost]
        [ActionName("ForgotPassword")]
        public ActionResult ForgotPassword(MemberLoginModel model)
        {
            if (!this.TempData.ContainsKey("Message"))
            {
                TempData["Message"] = "";
            }
            this.TempData["Message"] = string.Format("<span class=\"success\">Account information has been sent to <strong>{0}</strong></span>", model.Username);
            if (model.Username != string.Empty)
            {
                var emailStatus = SendForgotPasswordEmail(model, false);
                if (!emailStatus.EmailSent && emailStatus.ErrorMessage != string.Empty)
                {
                    var errorMessage = string.Format("<span class=\"failure\">{0}</span>", emailStatus.ErrorMessage);
                    this.TempData["Message"] = errorMessage;

                    //error message, add custom error to viewstate model and return the page.
                    ModelState.AddModelError("CustomError", new Exception(errorMessage));
                    return CurrentUmbracoPage();
                }
            }
            return RedirectToCurrentUmbracoPage();
        }

        [HttpPost]
        [ActionName("ChangePassword")]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            string errMsg = "";
            if (!TempData.ContainsKey("Message"))
            {
                TempData.Add("Message", string.Empty);
            }
            if (ModelState.IsValid)
            {
                // ChangePassword will throw an exception rather
                // than return false in certain failure scenarios.
                bool changePasswordSucceeded;
                try
                {
                    MembershipUser currentUser = Membership.GetUser(User.Identity.Name, true /* userIsOnline */);
                    changePasswordSucceeded = currentUser.ChangePassword(model.OldPassword, model.NewPassword);
                }
                catch (Exception e)
                {
                    errMsg = e.Message;
                    ModelState.AddModelError("CustomError", new Exception(errMsg));
                    return CurrentUmbracoPage();
                }

                if (changePasswordSucceeded)
                {
                    TempData["Message"] = "Your password has been updated";
                    return RedirectToCurrentUmbracoPage();
                }
                errMsg = "The 'Current Password' you have entered is incorrect.";
                this.ModelState.AddModelError("CustomError", new Exception(errMsg));
                return this.CurrentUmbracoPage();
            }
            if (model.NewPassword.Length > 100 || model.NewPassword.Length < 6)
            {
                errMsg = "Password length must be greater than 6 and less than 100";
            }
            if (model.NewPassword != model.ConfirmPassword)
            {
                errMsg = "Confirm password does not match new password";
            }
            this.ModelState.AddModelError("CustomError", new Exception(errMsg));
            return this.CurrentUmbracoPage();
            
        }

        [HttpPost]
        [ActionName("CreateUpdateAccount")]
        public ActionResult CreateUpdateAccount(AccountModel model)
        {
            if (!this.TempData.ContainsKey("UserValidated"))
            {
                TempData.Add("UserValidated", false);
            }
            if (!this.TempData.ContainsKey("Message"))
            {
                TempData.Add("Message", string.Empty);
            }
            if (ModelState.IsValid)
            {
                var accountExists = this.AccountExists(model.Email);
                var currentUser = Membership.GetUser();
                if (!accountExists || model.MemberId > 0)
                {
                    //create account here
                    var mode = SaveMemberToUmbraco(model);
                    
                    var returnUrl = Request.QueryString["returnurl"];
                    if (!string.IsNullOrEmpty(returnUrl))
                    {
                        return this.Redirect(returnUrl);
                    }

                    this.TempData["UserValidated"] = true;
                    if (mode == 0)
                    {
                        this.TempData["Message"] = "Your Profile has been created. Please click <a href=\"/members/\">here</a> to login.";
                    }
                    else if (mode == 1)
                    {
                        this.TempData["Message"] = "Your Profile has been updated.";
                    }
                }
                else
                {
                    this.TempData["UserValidated"] = false;
                    this.TempData["Message"] = "An account with that email already exists.";
                }

                if (!Convert.ToBoolean(this.TempData["UserValidated"]))
                {
                    //error message, add custom error to viewstate model and return the page.
                    ModelState.AddModelError("CustomError", new Exception(this.TempData["Message"].ToString()));
                    return CurrentUmbracoPage();
                }
            }
            return RedirectToCurrentUmbracoPage();
        }

        public bool AccountExists(string email)
        {
            var accountExists = false;

            var userId = GetUserId(email);
            if (userId > 0)
            {
                accountExists = true;
            }
            return accountExists;
        }

        protected int SaveMemberToUmbraco(AccountModel memberModel)
        {
            var mode = -1;   //0 = added. 1 = updated

            var memberService = new Umbraco.Core.Services.MemberService(new RepositoryFactory(), new MemberGroupService(new RepositoryFactory()));
            IMember member;

            memberModel.UserName = memberModel.Email;     //username will be email
            if (memberModel.MemberId == 0)
            {
                var fullname = string.Format("{0} {1}", memberModel.FirstName, memberModel.LastName);
                member = memberService.CreateMember(memberModel.UserName, memberModel.Email, fullname, MemberType);
                memberService.Save(member);
                member = memberService.GetByUsername(memberModel.UserName);
                memberService.SavePassword(member, memberModel.Password);
                mode = 0;
            }
            else
            {
                member = memberService.GetById(memberModel.MemberId);
                //memberService.SavePassword(member, memberModel.Password);
                mode = 1;
            }

            member.SetValue(MemberFields.firstName.ToString(), memberModel.FirstName);
            member.SetValue(MemberFields.lastName.ToString(), memberModel.LastName);
            member.SetValue(MemberFields.bio.ToString(), memberModel.Bio);

            //member.SetValue(MemberFields.employerCity.ToString(), memberModel.PracticeSetting);
            memberService.Save(member);

            if (!Roles.IsUserInRole(memberModel.UserName, MemberGroup))
            {
                Roles.AddUserToRole(memberModel.UserName, MemberGroup);
            }

            SendCreateMemberEmail(memberModel, false);
            SendCreateMemberEmail(memberModel, true);

            return mode;
        }

        public AccountModel GetAccountModel(string userName)
        {
            var accountModel = new AccountModel();
            var memberService = new Umbraco.Core.Services.MemberService(new RepositoryFactory(), new MemberGroupService(new RepositoryFactory()));
            var memberModel = memberService.GetByUsername(userName);
            if (memberModel != null) 
            {
                accountModel.MemberId = memberModel.Id;
                accountModel.FirstName = memberModel.GetValue(MemberFields.firstName.ToString()).ToString();
                accountModel.LastName = memberModel.GetValue(MemberFields.lastName.ToString()).ToString();
                accountModel.Bio = memberModel.GetValue(MemberFields.bio.ToString()).ToString();
                accountModel.Email = memberModel.Email;
            }
            return accountModel;
        }

        public int GetUserId(string username)
        {
            var memberService = new Umbraco.Core.Services.MemberService(new RepositoryFactory(), new MemberGroupService(new RepositoryFactory()));
            var member = memberService.GetByUsername(username);
            var memberId = member != null ? member.Id : 0;
            return memberId;
        }

        public Amoura.Extensions.Email.Email.EmailStatus SendCreateMemberEmail(AccountModel model, bool admin = false)
        {
            
            // first get email parameters
            var currentNode = Node.GetCurrent();
            var notificationTemplateId = currentNode.GetNodeValue(DocumentFields.notificationTemplate.ToString())
                                         != string.Empty
                                             ? int.Parse(currentNode.GetNodeValue(DocumentFields.notificationTemplate.ToString()))
                                             : 0;

            var emailStatus = new Amoura.Extensions.Email.Email.EmailStatus { EmailSent = false, ErrorMessage = string.Empty };

            if (notificationTemplateId > 0)
            {
                var notificationNode = new Node(notificationTemplateId);
                var emailSubject = notificationNode.GetNodeValue(DocumentFields.emailSubject.ToString());
                var emailTo = notificationNode.GetNodeValue(DocumentFields.emailAdmin.ToString());
                var emailFrom = notificationNode.GetNodeValue(DocumentFields.emailFrom.ToString());
                var emailMessage = notificationNode.GetNodeValue(DocumentFields.emailMessage.ToString());
                var emailBcc = string.Empty;
                const string EmailTemplate = "/Email/StandardTemplate.html";

                if (!admin)
                {
                    //client email
                    emailTo = model.Email;
                }
                else
                {
                    emailBcc = notificationNode.GetNodeValue(DocumentFields.emailBcc.ToString());
                }

                //var accountController = new AccountController();
                //var accountModel = accountController.GetAccount();
                //var membershipModel = CacheItemController.CachedUserInfo;

                if (model != null)
                {
                    if (emailTo != string.Empty)
                    {

                        var replacements = DictionaryReplacement.NewMember(model);

                        emailStatus = Amoura.Extensions.Email.Email.SendEmail(
                            emailTo,
                            emailFrom,
                            emailSubject, emailMessage,
                            EmailTemplate,
                            replacements, emailBcc);
                    }
                }
                else
                {
                    emailStatus.EmailSent = false;
                    emailStatus.ErrorMessage = "Your account could not be found in the system";
                }
            }

            return emailStatus;
        }

        public Amoura.Extensions.Email.Email.EmailStatus SendForgotPasswordEmail(MemberLoginModel model, bool admin = false)
        {
            // first get email parameters
            var currentNode = Node.GetCurrent();
            var notificationTemplateId = currentNode.GetNodeValue(DocumentFields.notificationTemplate.ToString())
                                         != string.Empty
                                             ? int.Parse(currentNode.GetNodeValue(DocumentFields.notificationTemplate.ToString()))
                                             : 0;

            var emailStatus = new Amoura.Extensions.Email.Email.EmailStatus { EmailSent = false, ErrorMessage = string.Empty };

            if (notificationTemplateId > 0)
            {
                var notificationNode = new Node(notificationTemplateId);
                var emailSubject = notificationNode.GetNodeValue(DocumentFields.emailSubject.ToString());
                var emailTo = notificationNode.GetNodeValue(DocumentFields.emailAdmin.ToString());
                var emailFrom = notificationNode.GetNodeValue(DocumentFields.emailFrom.ToString());
                var emailMessage = notificationNode.GetNodeValue(DocumentFields.emailMessage.ToString());
                var emailBcc = string.Empty;
                const string EmailTemplate = "/Email/StandardTemplate.html";

                if (!admin)
                {
                    //client email
                    emailTo = model.Username;
                }
                else
                {
                    emailBcc = notificationNode.GetNodeValue(DocumentFields.emailBcc.ToString());
                }

                var accountController = new AccountController();
                var accountModel = accountController.GetAccount();
                //var membershipModel = CacheItemController.CachedUserInfo;

                if (accountModel != null)
                {
                    if (emailTo != string.Empty)
                    {

                        var replacements = DictionaryReplacement.RecoverPassword(accountModel);

                        emailStatus = Amoura.Extensions.Email.Email.SendEmail(
                            emailTo,
                            emailFrom,
                            emailSubject, emailMessage,
                            EmailTemplate,
                            replacements, emailBcc);
                    }
                }
                else
                {
                    emailStatus.EmailSent = false;
                    emailStatus.ErrorMessage = "Your account could not be found in the system";
                }
            }

            return emailStatus;
        }

        public string RedirectUrl()
        {
            var returnRawUrl = Request.RawUrl;
            var returnUrl = string.Empty;
            if (Request.QueryString["returnurl"] != null && Request.QueryString["returnurl"] != string.Empty)
            {
                returnUrl = Request.QueryString["returnurl"];
            }
            else if (Url.IsLocalUrl(returnRawUrl) && returnRawUrl.Length > 1 && returnRawUrl.StartsWith("/")
                && !returnRawUrl.StartsWith("//") && !returnRawUrl.StartsWith("/\\"))
            {
                returnUrl = returnRawUrl;
            }
            return returnUrl;
        }

    }
}
