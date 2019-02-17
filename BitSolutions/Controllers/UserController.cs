using BitSolutions.Models;
using BitSolutions.Models.SAS;
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
        /// Método intermediario que redirecciona a una vista de acuerdo al role del usuario
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
                    return RedirectToAction("IndexSAS", new { data = data, message = message, typeUser = "Coordinator" });
                case "2": //Cliente
                    return RedirectToAction("IndexSAS", new { data = data, message = message, typeUser = "Client" });
                case "3": //Coordinador Mesa
                    return RedirectToAction("IndexSAS", new { data = data, message = message, typeUser = "HelpDeskCoordinator" });
                default:
                    return RedirectToAction("");
            }
        }

        /// <summary>
        /// Método que carga las vista del usuario Coordinador
        /// </summary>
        /// <param name="data"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public ActionResult IndexSAS(string data = null, string message = null, string typeUser = "")
        {
            @ViewBag.typeUser = typeUser;

            switch (data)
            {
                case "addSAS":
                    switch (typeUser)
                    {
                        case "Coordinator":
                            return RedirectToAction("IndexAddSAS", new { data = data, message = message, typeUser = "Coordinator" });
                        case "Client":
                            return RedirectToAction("IndexAddSAS", new { data = data, message = message, typeUser = "Client" });
                        default:
                            break;
                    }

                    break;
                case "viewSAS":    
                    switch (typeUser)
                    {
                        case "Coordinator":
                            return RedirectToAction("IndexViewSAS", new { data = data, message = message, typeUser = "Coordinator" });
                        case "HelpDeskCoordinator":
                            return RedirectToAction("IndexViewSAS", new { data = data, message = message, typeUser = "HelpDeskCoordinator" });
                        default:
                            break;
                    }
                    break;
                case "helpDesk":
                    if (typeUser == "HelpDeskCoordinator")
                    {
                        return RedirectToAction("IndexViewHelpDesk", "HelpDesk", new { data = data, message = message, typeUser = "HelpDeskCoordinator" });
                    }
                    break;
                default:
                    return View("IndexUser");
            }

            return View("IndexUser");
        }

        /// <summary>
        /// Carga datos en la vista de coordinador de agregar SAS
        /// </summary>
        /// <param name="data"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public ActionResult IndexAddSAS(string data = "", string message = null, string typeUser = "")
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

            switch (typeUser)
            {
                case "Coordinator":
                    ViewBag.typeUser = "Coordinator";

                    return View("IndexUser");
                case "Client":
                    ViewBag.typeUser = "Client";

                    return View("IndexUser");
                case "Coordinador Mesa":
                    ViewBag.typeUser = "Coordinador Mesa";

                    return View("IndexUser");
                default:
                    break;
            }

            return View("");
        }

        /// <summary>
        /// Carga las categorias en la vista para filtrar los ticket
        /// </summary>
        /// <param name="data"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public ActionResult IndexViewSAS(string data = null, string message = null, string typeUser = "")
        {
            ViewBag.TypeForm = data;
            ViewBag.message = message;

            ViewBag.typeUser = typeUser;

            List<Ticket> ticketList = (from ticket in dbManager.Tickets orderby ticket.Priority descending select ticket).ToList();

            List<Category> categoryList = new List<Category>();

            categoryList.Add(new Category() { CategoryData = "Fecha" });
            categoryList.Add(new Category() { CategoryData = "Estado" });
            categoryList.Add(new Category() { CategoryData = "Prioridad" });

            //Es usado para asignar datos a dropdownlist
            SelectList sl = new SelectList(categoryList, "CategoryData", "CategoryData"); //1 Id valor por defecto

            TempData["categoryList"] = sl;
            TempData.Keep();

            ViewBag.ticketList = ticketList;

            switch (typeUser)
            {
                case "Coordinator":
                    return View("IndexUser");
                case "Client":
                    return View("IndexUser");
                case "Coordinador Mesa":
                    return View("IndexUser");
                default:
                    break;
            }

            return View("");

            //return RedirectToAction("GetInfoTicket","SAS", new { data = data, message  = message });
        }





        [Authorize]
        [HttpGet]
        public ActionResult IndexViewHelpDesk(string data = null, string message = null, string typeUser = "")
        {
            /*****************************************************************************************************************
             *                                              VALIDACIÓN                                                       *
             * Validar acceso por URL con sesión ya que a este metodo solo pueden acceder los coordinadores de mesa de ayuda *
             *                                                                                                               *
             /****************************************************************************************************************/


            ViewBag.TypeForm = data;
            ViewBag.message = message;

            ViewBag.typeUser = typeUser;

            if (typeUser == "HelpDeskCoordinator")
            {
                return View("IndexUser");
            }

            return View("");

            //return RedirectToAction("GetInfoTicket","SAS", new { data = data, message  = message });
        }








        /// <summary>
        /// Método que es accedido desde SASController para cargar los datos de los ticket
        /// </summary>
        /// <param name="ticketList"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public ActionResult IndexViewSearchParameterSASCoordinator(string data = null, string typeUser = "")
        {
            ViewBag.TypeForm = data;

            List<Ticket> ticketList = (List<Ticket>) TempData["ticketList"];
            List<Category> categoryList = new List<Category>();

            categoryList.Add(new Category() { CategoryData = "Fecha" });
            categoryList.Add(new Category() { CategoryData = "Estado" });
            categoryList.Add(new Category() { CategoryData = "Prioridad" });

            //Es usado para asignar datos a dropdownlist
            SelectList sl = new SelectList(categoryList, "CategoryData", "CategoryData"); //1 Id valor por defecto

            TempData["categoryList"] = sl;
            TempData.Keep();

            ViewBag.TypeForm = "viewSAS";
            ViewBag.typeUser = typeUser;
            ViewBag.ticketList = ticketList;

            return View("IndexUser");
        }

    }
}
