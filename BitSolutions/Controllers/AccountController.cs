using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using BitSolutions.Models;
using System.Collections.Generic;

namespace BitSolutions.Controllers
{
    public class AccountController : Controller
    {
        #region Private Properties

        private SAS_2019Entities dbManager;

        #endregion

        #region Default Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountController" /> class.
        /// </summary>
        public AccountController()
        {
            dbManager = new SAS_2019Entities();
        }

        #endregion

        #region Login methods

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            try
            {
                // Verification.
                if (Request.IsAuthenticated)
                {
                    // Info.
                    return RedirectToLocal(returnUrl);
                }
            }
            catch (Exception ex){}

            return View();
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(DB_RRHH_Employee model, string returnUrl)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var loginInfo =  dbManager.LoginByUsernamePassword(model.UserName, model.Password).ToList();

                    // Verification.
                    if (loginInfo != null && loginInfo.Count() > 0)
                    {
                        var logindetails = loginInfo.First();

                        SignInUser(logindetails.UserName, logindetails.ID_Rol, false);

                        Session["role_id"] = logindetails.ID_Rol;
                        Session["username"] = model.UserName;

                        //return RedirectToAction("Intermediary", "User", new { data = "Datos de prueba" });
                        return RedirectToAction("Intermediary", "User");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Invalid username or password.");
                    }
                }
            }
            catch (Exception ex){}

            return View(model);
        }

        #endregion

        #region Log Out method.

        /// <summary>
        /// POST: /Account/LogOff
        /// </summary>
        /// <returns>Return log off action</returns>
        [HttpPost]
        [Authorize]
        public ActionResult LogOff()
        {
            try
            {
                // Setting.
                var ctx = Request.GetOwinContext();
                var authenticationManager = ctx.Authentication;

                // Sign Out.
                authenticationManager.SignOut();
            }
            catch (Exception ex){}

            // Info.
            return RedirectToAction("Login", "Account");
        }

        #endregion

        #region Helpers

        #region Sign In method.

        /// <summary>
        /// Sign In User method.
        /// </summary>
        /// <param name="username">Username parameter.</param>
        /// <param name="role_id">Role ID parameter</param>
        /// <param name="isPersistent">Is persistent parameter.</param>
        [Authorize]
        private void SignInUser(string username, int role_id, bool isPersistent)
        {
            // Initialization.
            var claims = new List<Claim>();

            try
            {
                // Setting
                claims.Add(new Claim(ClaimTypes.Name, username));
                claims.Add(new Claim(ClaimTypes.Role, role_id.ToString()));
                var claimIdenties = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);
                var ctx = Request.GetOwinContext();
                var authenticationManager = ctx.Authentication;

                // Sign In.
                authenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, claimIdenties);
            }
            catch (Exception ex){}
        }

        #endregion

        #region Redirect to local method.

        /// <summary>
        /// Redirect to local method.
        /// </summary>
        /// <param name="returnUrl">Return URL parameter.</param>
        /// <returns>Return redirection action</returns>
        [Authorize]
        private ActionResult RedirectToLocal(string returnUrl)
        {
            try
            {
                // Verification.
                if (Url.IsLocalUrl(returnUrl))
                {
                    // Info.
                    return Redirect(returnUrl);
                }
            }
            catch (Exception ex)
            {
                // Info
                throw ex;
            }

            /*Este redirect se ejecuta cuando */
            return RedirectToAction("Index", "Home");
            //return RedirectToAction("Management", "Home");
        }

        #endregion

        #endregion
    }
}