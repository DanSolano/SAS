using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace BitSolutions.Controllers
{
    public class HomeController : Controller
    {
        #region Index method.

        public void Management()
        {
            //string[] roleNames = Roles.GetRolesForUser();
            string g = Session["role_id"].ToString();
            //switch (User.Identity.)
            //{
            //    case ""
            //    default:
            //        break;
            //}
        }

        /// <summary>
        /// Index method.
        /// </summary>
        /// <returns>Returns - Index view</returns>
        public ActionResult Index()
        {
            return View();
        }

        #endregion

        #region Admin Only Link

        /// <summary>
        /// Admin only link method.
        /// </summary>
        /// <returns>Returns - Admin only link view</returns>
        [Authorize(Roles = "1")]
        public ActionResult AdminOnlyLink()
        {
            return View();
        }

        #endregion
    }
}