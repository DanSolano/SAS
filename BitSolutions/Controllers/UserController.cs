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
        public ActionResult Intermediary(string data = null , string message = null, string messageType = null)
        {
            string role = Session["role_id"].ToString();

            switch (role)
            {
                case "1": //Coordinador
                    return RedirectToAction("IndexSAS", new { data = data, message = message, messageType = messageType , typeUser = "Coordinator" });
                case "2": //Cliente
                    return RedirectToAction("IndexSAS", new { data = data, message = message, messageType = messageType , typeUser = "Client" });
                case "3": //Coordinador Mesa
                    return RedirectToAction("IndexSAS", new { data = data, message = message, messageType = messageType , typeUser = "HelpDeskCoordinator" });
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
        public ActionResult IndexSAS(string data = null, string message = null, string messageType = null, string typeUser = "")
        {
            @ViewBag.typeUser = typeUser;

            switch (data)
            {
                case "addSAS":
                    if (typeUser == "Client")
                    {
                        return RedirectToAction("IndexAddSAS", new { data = data, message = message, messageType = messageType, typeUser = "Client" });
                    }
                    break;
                case "viewSAS":    
                    switch (typeUser)
                    {
                        case "Coordinator":
                            return RedirectToAction("IndexViewSAS", new { data = data, message = message, messageType = messageType, typeUser = "Coordinator" });
                        case "HelpDeskCoordinator":
                            return RedirectToAction("IndexViewSAS", new { data = data, message = message, messageType = messageType, typeUser = "HelpDeskCoordinator" });
                        default:
                            break;
                    }
                    break;
                case "helpDesk":
                    if (typeUser == "HelpDeskCoordinator")
                    {
                        return RedirectToAction("IndexViewHelpDesk", "HelpDesk", new { typeUser = "HelpDeskCoordinator" });
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
        public ActionResult IndexAddSAS(string data = "", string message = null, string messageType = null, string typeUser = "")
        {
            ViewBag.TypeForm = data;
            ViewBag.message = message;
            ViewBag.messageType = messageType;

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

            ViewBag.typeUser = typeUser;

            return View("IndexUser");
        }

        /// <summary>
        /// Carga las categorias en la vista para filtrar los ticket
        /// </summary>
        /// <param name="data"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public ActionResult IndexViewSAS(string data = null, string message = null, string messageType = null, string typeUser = "")
        {
            ViewBag.TypeForm = data;
            ViewBag.message = message;
            ViewBag.messageType = messageType;
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

            return View("IndexUser");

            //return RedirectToAction("GetInfoTicket","SAS", new { data = data, message  = message });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="message"></param>
        /// <param name="typeUser"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public ActionResult IndexViewHelpDesk(string data = null, string message = null, string messageType = null, string typeUser = "")
        {
            /*****************************************************************************************************************
             *                                              VALIDACIÓN                                                       *
             * Validar acceso por URL con sesión ya que a este metodo solo pueden acceder los coordinadores de mesa de ayuda *
             *                                                                                                               *
             /****************************************************************************************************************/

            ViewBag.TypeForm = data;
            ViewBag.typeUser = typeUser;
            ViewBag.messageType = messageType;
            ViewBag.ticketList = Session["ticketList"];

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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ticketCode"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public ActionResult ShowCoordinatorList(string ticketCode = null)
        {
            List<DB_RRHH_Employee> CoordinatorList = (from coordinator in dbManager.DB_RRHH_Employee
                                                      join userRole in dbManager.User_Rol on coordinator.ID 
                                                      equals userRole.ID_Employee
                                                      join rol in dbManager.Rols on userRole.ID_Rol equals rol.ID_Rol
                                                      where rol.Name_Rol == "Coordinator" & coordinator.Status == "Active"
                                                      select coordinator).ToList();
           

            ViewBag.TypeForm = "assignTicket";
            ViewBag.typeUser = "HelpDeskCoordinator";
            ViewBag.coordinatorList = CoordinatorList;
            ViewBag.ticketCode = ticketCode;

            return View("IndexUser");
        }

        [Authorize]
        [HttpGet]
        public ActionResult AssignSASToCoordinator(string identificationEmployee = null, string ticketCode = null)
        {
            return RedirectToAction("IndexViewFilterHelpDesk", "HelpDesk", new { typeUser = "HelpDeskCoordinator" });

            int ticketCodeValue = Int32.Parse(ticketCode);

            //Información del cliente que tiene asociado el ticket
            List<DB_Client> ticketClient = (from ticket in dbManager.Tickets join cli in dbManager.DB_Client on ticket.ID_Client equals cli.ID where ticket.ID == ticketCodeValue select cli).ToList();

            //Información del empleado de acuerdo al número de cédula
            List<DB_RRHH_Employee> employee = (from emp in dbManager.DB_RRHH_Employee where emp.identification == identificationEmployee select emp).ToList();

            try
            {
                Employee_Ticket tmpEmployee = new Employee_Ticket();
                tmpEmployee.ID_Employee = employee[0].ID;
                tmpEmployee.ID_Ticket = Int32.Parse(ticketCode);
                tmpEmployee.ID_Client = ticketClient[0].ID;
                tmpEmployee.ID_User_Assign = employee[0].ID;

                dbManager.Employee_Ticket.Add(tmpEmployee);
                dbManager.SaveChanges();

                return RedirectToAction("IndexViewFilterHelpDesk", "HelpDesk", new { typeUser = "HelpDeskCoordinator" });
            }
            catch (Exception ex){}

            return View("IndexUser");




            //List<DB_RRHH_Employee> CoordinatorList = (from coordinator in dbManager.DB_RRHH_Employee
            //                                          join userRole in dbManager.User_Rol on coordinator.ID
            //                                          equals userRole.ID_Employee
            //                                          join rol in dbManager.Rols on userRole.ID_Rol equals rol.ID_Rol
            //                                          where rol.Name_Rol == "Coordinator" & coordinator.Status == "Active"
            //                                          select coordinator).ToList();


            //ViewBag.TypeForm = "assignTicket";
            //ViewBag.typeUser = "HelpDeskCoordinator";
            //ViewBag.coordinatorList = CoordinatorList;
            //ViewBag.ticketCode = ticketCode;

            //return View("IndexUser");
        }
       

    }
}
