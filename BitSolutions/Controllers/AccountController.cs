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
using BitSolutions.Models.SAS;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace BitSolutions.Controllers
{
    public class AccountController : Controller
    {
        #region Private Properties
        private SAS_2019Entities dbManager;
        #endregion

        public AccountController()
        {
            dbManager = new SAS_2019Entities();
        }

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            try
            {
                // Verification.
                if (Request.IsAuthenticated)
                {
                    return View();
                    // Info.
                    return RedirectToLocal(returnUrl);
                }
            }
            catch (Exception ex){}

            return View();
        }

        /// <summary>
        /// POST: /Account/LogIn
        /// </summary>
        /// <param name="model"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(DB_RRHH_Employee model, string returnUrl)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string encryptPass = Convert.ToString(PassEncrypt(model.Password, "keyPass@0"));
                    var loginInfo =  dbManager.spLoginByUsernamePassword(model.UserName, encryptPass).ToList();

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
            catch (Exception ex){}

            /*Este redirect se ejecuta cuando */
            return RedirectToAction("Index", "Home");
        }

        
    #region cypher

    public static string PassEncrypt(string noEncryptPass, string key)
    {
     
        //Get the Bytes of the password typed
        var bytesToEncrypt = Encoding.UTF8.GetBytes(noEncryptPass);
        var keyBytes = Encoding.UTF8.GetBytes(key);

            //Hash the key with SHA512
            keyBytes = SHA512.Create().ComputeHash(keyBytes);


        var bytesEncrypted = AccountController.Encrypt(bytesToEncrypt, keyBytes);

        return Convert.ToBase64String(bytesEncrypted);

    }

    private static byte[] Encrypt(byte[] bytesToBeEncrypted, byte[] passwordBytes)
    {
        byte[] encryptedBytes = null;

        // Set your salt here, change it to meet your flavor:
        // The salt bytes must be at least 8 bytes.
        var saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };

        using (MemoryStream ms = new MemoryStream())
        {
            using (RijndaelManaged AES = new RijndaelManaged())
            {
                var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);

                AES.KeySize = 256;
                AES.BlockSize = 128;
                AES.Key = key.GetBytes(AES.KeySize / 8);
                AES.IV = key.GetBytes(AES.BlockSize / 8);

                AES.Mode = CipherMode.CBC;

                using (var cs = new CryptoStream(ms, AES.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
                    cs.Close();
                }

                encryptedBytes = ms.ToArray();
            }
        }

        return encryptedBytes;
    }

    #endregion


    }
}