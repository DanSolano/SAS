using BitSolutions.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BitSolutions.Controllers
{
    public class UserController : Controller
    {
        #region Properties
        private SAS_2019Entities dbManager;
        #endregion

        public UserController()
        {
            dbManager = new SAS_2019Entities();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult Intermediary(string data = null , string message = null)
        {
            string role = Session["role_id"].ToString();

            switch (role)
            {
                case "1": //Coordinador
                    return RedirectToAction("IndexSASCoordinator", new { data = data, message = message });
                    //break;
                case "2": //Cliente
                    return RedirectToAction("IndexClient");
                    //break;
                default:
                    break;
            }

            return RedirectToAction("");
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public ActionResult IndexSASCoordinator(string data = "", string message = null)
        {
            switch (data)
            {
                case "addSAS":
                    return RedirectToAction("IndexAddSASCoordinator", new { data  = data, message = message });
                case "viewSAS":
                    return RedirectToAction("IndexViewSASCoordinator", new { data = data, message = message });
                default:
                    return View("IndexCoordinator");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public ActionResult IndexAddSASCoordinator(string data = "", string message = null)
        {
            ViewBag.TypeForm = data;
            ViewBag.message = message;

            if (data == "addSAS")
            {
                List<Ticket> ticketList = new List<Ticket>();

                ticketList.Add(new Ticket() { Category = "Bug" });
                ticketList.Add(new Ticket() { Category = "Soporte" });
                ticketList.Add(new Ticket() { Category = "Capacitación" });
                ticketList.Add(new Ticket() { Category = "Nueva funcionalidad" });
                ticketList.Add(new Ticket() { Category = "Otro" });

                //Es usado para asignar datos a dropdownlist
                SelectList sl = new SelectList(ticketList, "Category", "Category"); //1 Id valor por defecto

                TempData["ticketList"] = sl;
                TempData.Keep();
            }

            return View("IndexCoordinator");
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public ActionResult IndexViewSASCoordinator(string data = null, string message = null)
        {
            ViewBag.TypeForm = data;
            ViewBag.message = message;

            List<Ticket> ticketList = dbManager.Tickets.ToList();

            ViewBag.ticketList = ticketList;

            return View("IndexCoordinator");
            //return View("IndexCoordinator");

            //return RedirectToAction("GetInfoTicket","SAS", new { data = data, message  = message });
        }






        // GET: User/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: User/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: User/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: User/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: User/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: User/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
