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

        /// <summary>
        /// Obtiene todos los datos de los ticket
        /// </summary>
        /// <param name="data"></param>
        /// <param name="message"></param>
        /// <param name="typeUser"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public ActionResult IndexViewHelpDesk(string typeUser = "")
        {
            /*****************************************************************************************************************
             *                                              VALIDACIÓN                                                       *
             * Validar acceso por URL con sesión ya que a este metodo solo pueden acceder los coordinadores de mesa de ayuda *
             *                                                                                                               *
             /****************************************************************************************************************/

            List<Ticket> ticketList = (from ticket in dbManagerSAS.Tickets select ticket).ToList();

            List<Ticket> ticketsListOfAssociatedWithUser = (from ticket in dbManagerSAS.Tickets
                                                            join employeeTicket in dbManagerSAS.Employee_Ticket on ticket.ID
                                                            equals employeeTicket.ID_Ticket
                                                            join db_rr_employee in dbManagerSAS.DB_RRHH_Employee
                                                            on employeeTicket.ID_Employee equals db_rr_employee.ID
                                                            select ticket).ToList();
            List<Ticket> freeTicket = new List<Ticket>();

            //Se obtienen los tickets que no han sido asigandos a un usuario
            foreach (var ticket in ticketList)
            {
                bool flag = false;

                foreach (var ticketUser in ticketsListOfAssociatedWithUser)
                {
                    if (ticket.ID == ticketUser.ID)
                    {
                        flag = true;
                        break;
                    }
                }

                if (!flag)
                {
                    freeTicket.Add(ticket);
                }
            }

            //Obtiene la lista de clientes
            var tempClient = (from rrhhEmployee in dbManagerEntities.DB_RRHH_Employee join blnt in dbManagerEntities.Belong_To 
                              on rrhhEmployee.ID equals blnt.ID_Employee join ent in dbManagerEntities.Enterprises 
                              on blnt.ID_Enterprise equals ent.ID select new { rrhhEmployee.ID, ent.Name }).ToList();

            List<FullTicket> fullTicketList = new List<FullTicket>();

            foreach (var index in freeTicket)
            {
                var temp = tempClient.Where(x => x.ID == index.ID_Client).ToList();

                fullTicketList.Add(new FullTicket(index.ID.ToString(), temp[0].Name, index.Description));
            }

            if (typeUser == "HelpDeskCoordinator")
            {
                Session["ticketList"] = fullTicketList.ToList();
                return RedirectToAction("IndexViewHelpDesk", "User", new { data = "helpDesk", typeUser = "HelpDeskCoordinator" });
            }

            return View("");

            //return RedirectToAction("GetInfoTicket","SAS", new { data = data, message  = message });
        }

        /// <summary>
        /// Obtiene los datos de la mesa de ayuda de los ticket que no han sido asignados a un coordinador
        /// </summary>
        /// <param name="data"></param>
        /// <param name="message"></param>
        /// <param name="typeUser"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public ActionResult IndexViewFilterHelpDesk(string typeUser = "")
        {
            /*****************************************************************************************************************
             *                                              VALIDACIÓN                                                       *
             * Validar acceso por URL con sesión ya que a este metodo solo pueden acceder los coordinadores de mesa de ayuda *
             *                                                                                                               *
             /****************************************************************************************************************/

            List<Ticket> ticketList = (from ticket in dbManagerSAS.Tickets select ticket).ToList();

            List<Ticket> ticketsListOfAssociatedWithUser = (from ticket in dbManagerSAS.Tickets
                                                            join employeeTicket in dbManagerSAS.Employee_Ticket on ticket.ID
                                                            equals employeeTicket.ID_Ticket
                                                            join db_rr_employee in dbManagerSAS.DB_RRHH_Employee
                                                            on employeeTicket.ID_Employee equals db_rr_employee.ID
                                                            select ticket).ToList();
            List<Ticket> freeTicket = new List<Ticket>();

            //Se obtienen los tickets que no han sido asigandos a un usuario
            foreach (var ticket in ticketList)
            {
                bool flag = false;

                foreach (var ticketUser in ticketsListOfAssociatedWithUser)
                {
                    if (ticket.ID == ticketUser.ID)
                    {
                        flag = true;
                        break;
                    }
                }

                if (!flag)
                {
                    freeTicket.Add(ticket);
                }
            }


            //Obtiene la lista de clientes
            var tempClient = (from rrhhEmployee in dbManagerEntities.DB_RRHH_Employee join blnt in dbManagerEntities.Belong_To on rrhhEmployee.ID equals blnt.ID_Employee join ent in dbManagerEntities.Enterprises on blnt.ID_Enterprise equals ent.ID select new { rrhhEmployee.ID, ent.Name }).ToList();

            //Lista de ticket
            List<FullTicket> filterTicketList = new List<FullTicket>();

            //Obtiene la lista de los Employee_ticket
            List<Employee_Ticket> employeeTicketList = (from employeeTicket in dbManagerSAS.Employee_Ticket select employeeTicket).ToList();

            foreach (var tick in freeTicket)
            {
                bool flag = false;

                foreach (var empl in employeeTicketList)
                {
                    if (tick.ID == empl.ID_Ticket)
                    {
                        flag = true;
                        break;
                    }
                }

                if (!flag)
                {
                    var temp = tempClient.Where(x => x.ID == tick.ID_Client).ToList();

                    filterTicketList.Add(new FullTicket(tick.ID.ToString(), temp[0].Name, tick.Description));
                }
            }

            if (typeUser == "HelpDeskCoordinator")
            {
                Session["ticketList"] = filterTicketList.ToList();
                return RedirectToAction("IndexViewHelpDesk", "User", new { data = "helpDesk", typeUser = "HelpDeskCoordinator" });
            }

            return View("");

            //return RedirectToAction("GetInfoTicket","SAS", new { data = data, message  = message });
        }
    }
}
