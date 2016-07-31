using Amoura.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Umbraco.Core.Models;
using Umbraco.Core.Persistence;
using Umbraco.Core.Services;

namespace Amoura.Web.Controllers
{
    public class AccountController : Umbraco.Web.Mvc.SurfaceController
    {
        //
        // GET: /Account/

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ActionName("UpdateCreateAccount")]
        public ActionResult UpdateCreateAccount(AccountModel model)
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

                if (model.MemberId == 0)
                {
                    //create account here

                    var returnUrl = Request.QueryString["returnurl"];
                    if (!string.IsNullOrEmpty(returnUrl))
                    {
                        return this.Redirect(returnUrl);
                    }

                    this.TempData["UserValidated"] = true;
                    this.TempData["Message"] = "Your Account has been created. Please check your email to complete the process.";
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

        public AccountModel GetAccount() 
        {
            AccountModel accountModel = null;

            var memberService = new Umbraco.Core.Services.MemberService(new RepositoryFactory(), new MemberGroupService(new RepositoryFactory()));
            IMember member;
            //member = memberService.GetById(memberModel.MemberId);

            var currentUser = Membership.GetUser(User.Identity.Name, true);
            if (currentUser != null && currentUser.UserName != string.Empty)
            {
                member = memberService.GetByUsername(currentUser.UserName);
                accountModel = new AccountModel
                {
                    MemberId = member.Id,
                    UserName = member.Username,
                    FirstName = member.GetValue("firstName").ToString(),
                    LastName = member.GetValue("lastName").ToString(),
                    Email = member.GetValue("email").ToString(),
                    Password = member.RawPasswordValue
                };
            }

            return accountModel;
        }
    }
}
