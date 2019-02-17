using BitSolutions.Models;
using BitSolutions.Models.Client;
using BitSolutions.Models.SAS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BitSolutions.Controllers
{
    public class HelpDeskController : Controller
    {
        #region Properties
        private SAS_2019Entities dbManagerSAS;
        private DB_ClientEntities dbManagerEntities;
        #endregion

        public HelpDeskController()
        {
            dbManagerSAS = new SAS_2019Entities();
            dbManagerEntities = new DB_ClientEntities();
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

            //Obtiene la lista de los ticket
            var tempTicketList = (from ticket in dbManagerSAS.Tickets select new { ticket.ID, ticket.ID_Client, ticket.Description }).ToList();

            //Obtiene la lista de clientes
            var tempClient = (from rrhhEmployee in dbManagerEntities.DB_RRHH_Employee join blnt in dbManagerEntities.Belong_To on rrhhEmployee.ID equals blnt.ID_Employee join ent in dbManagerEntities.Enterprises on blnt.ID_Enterprise equals ent.ID select new { rrhhEmployee.ID, ent.Name }).ToList();

            List<FullTicket> ticketList = new List<FullTicket>();

            foreach (var index in tempTicketList)
            {
                var temp = tempClient.Where(x => x.ID == index.ID_Client).ToList();

                ticketList.Add(new FullTicket(index.ID.ToString(), temp[0].Name, index.Description));
            }

            ViewBag.TypeForm = data;
            ViewBag.message = ticketList;

            ViewBag.typeUser = typeUser;

            if (typeUser == "HelpDeskCoordinator")
            {
                return RedirectToAction("IndexViewHelpDesk", "User", new { data = "helpDesk", message = ViewBag.message, typeUser = "HelpDeskCoordinator" });
                return View("~/Views/User/IndexUser.cshtml");
            }

            return View("");

            //return RedirectToAction("GetInfoTicket","SAS", new { data = data, message  = message });
        }
    }
}
