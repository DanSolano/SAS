using BitSolutions.Models.SAS;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BitSolutions.Controllers
{
    public class SASController : Controller
    {
        #region Properties
        private SAS_2019Entities dbManager;
        #endregion

        public SASController()
        {
            dbManager = new SAS_2019Entities();
        }
        
        /// <summary>
        /// Genera una soliciitud de atención
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="uploadFile"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public ActionResult CreateSASCoordinator(FormCollection collection, HttpPostedFileBase uploadFile)
        {
            try
            {
                string resultOp = "";

                if (ModelState.IsValid)
                {
                    if (uploadFile != null)
                    {
                        switch (uploadFile.ContentType)
                        {
                            case "text/plain": // Txt
                                resultOp = intermediaryLoadDataInDatabase(collection, uploadFile, "file");
                                break;
                            case "application/msword": // Microsoft Word
                                resultOp =  intermediaryLoadDataInDatabase(collection, uploadFile, "file");
                                break;
                            case "application/pdf": // (PDF)
                                intermediaryLoadDataInDatabase(collection, uploadFile, "file");
                                break;
                            case "application/vnd.ms-powerpoint": // Microsoft PowerPoint
                                resultOp =  intermediaryLoadDataInDatabase(collection, uploadFile, "file");
                                break;
                            case "application/vnd.ms-excel": // Microsoft Excel
                                resultOp =  intermediaryLoadDataInDatabase(collection, uploadFile, "file");
                                break;
                            case "image/x-icon": // Formato Icon
                                resultOp =  intermediaryLoadDataInDatabase(collection, uploadFile, "image");
                                break;
                            case "image/jpeg": // JPEG
                                resultOp =  intermediaryLoadDataInDatabase(collection, uploadFile, "image");
                                break;
                            case "image/svg+xml": // Gráficos Vectoriales (SVG)
                                resultOp =  intermediaryLoadDataInDatabase(collection, uploadFile, "image");
                                break;
                            case "image/webp": //Imágenes WEBP
                                resultOp = intermediaryLoadDataInDatabase(collection, uploadFile, "image");
                                break;
                            case "image/png": //Imágenes PNG
                                resultOp = intermediaryLoadDataInDatabase(collection, uploadFile, "image");
                                break;
                            default: //Formato de imagen no permitido
                                resultOp = "Error !! Formato de imagen no válido";
                                break;
                        }
                    }
                    else
                    {
                        resultOp = intermediaryLoadDataInDatabase(collection, uploadFile, "");
                    }

                    ViewBag.message = resultOp;

                    return RedirectToAction("Intermediary", "User", new { data = "addSAS", message = ViewBag.message });
                }

                return View();
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        /// <summary>
        /// Método intermediario para procesamiento de información de SAS
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="uploadFile"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        private string intermediaryLoadDataInDatabase(FormCollection collection, HttpPostedFileBase uploadFile, string contentType)
        {
            string route = "";
            string result = "";
            string ticketGuid = Guid.NewGuid().ToString();

            if (uploadFile != null)
            {
                route = LoadFileInServer(uploadFile, contentType, ticketGuid);
            }

            result = LoadDataInDatabase(collection, route, ticketGuid, contentType);

            return result;
        }

        /// <summary>
        /// Almacena toda la información del SAS en la bd
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="route"></param>
        /// <param name="ticketGuid"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        private string LoadDataInDatabase(FormCollection collection, string route, string ticketGuid, string contentType)
        {
            DB_RRHH_Employee employee = dbManager.DB_RRHH_Employee.Find(1);
            string description = collection["Description"];
            string username = Session["username"].ToString();
            DateTime now = DateTime.Now;

            ObjectParameter result = new ObjectParameter("result", typeof(string));

            if (contentType == "file")
            { // 1 => Prioridad alta
                dbManager.spAddTicket(username, now, now, "Active", "1", description, "","no route", route, now, result);
            }
            else
            { // 1 => Prioridad alta
                dbManager.spAddTicket(username, now, now, "Active", "1", description, "", route, "no route", now, result);
            }
            
            return result.Value.ToString();
        }

        /// <summary>
        /// Carga el archivo en el servidor
        /// </summary>
        /// <param name="uploadFile"></param>
        /// <param name="contentType"></param>
        /// <param name="ticketGuid"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        private string LoadFileInServer(HttpPostedFileBase uploadFile, string contentType,string ticketGuid)
        {
            string route = "";

            string file = Server.MapPath(".") + "/LoadImages/"+ ticketGuid;

            if (!System.IO.Directory.Exists(file))
                System.IO.Directory.CreateDirectory(file);

            route = file + "\\" + uploadFile.FileName;

            uploadFile.SaveAs(route);

            return route;
        }

        /// <summary>
        /// Busca los ticket de acuerdo al parámetro ingresado
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public ActionResult SearchTicketByParameter(FormCollection collection)
        {
            string searchParameter = "";
            searchParameter = collection["Category"];

            List<Ticket> ticketList = null;
            List<Ticket> tempTicketList = null;

            switch (searchParameter)
            {
                case "Fecha":
                    ticketList = (from ticket in dbManager.Tickets orderby ticket.DateIn descending select ticket).ToList();
                    break;
                case "Estado":
                    ticketList = (from ticket in dbManager.Tickets where ticket.Status != "Closed" orderby ticket.Status ascending select ticket).ToList();

                    tempTicketList = new List<Ticket>();
                    tempTicketList = (from ticket in dbManager.Tickets where ticket.Status == "Closed" select ticket).ToList();

                    foreach (var item in tempTicketList)
                    {
                        ticketList.Add(item);
                    }

                    break;
                default: //Prioridad
                    ticketList = (from ticket in dbManager.Tickets where ticket.Priority != "Low" orderby ticket.Priority ascending select ticket).ToList();

                    tempTicketList = new List<Ticket>();
                    tempTicketList = (from ticket in dbManager.Tickets  where ticket.Priority == "Low" select ticket).ToList();

                    foreach (var item in tempTicketList)
                    {
                        ticketList.Add(item);
                    }
                    break;
            }

            TempData["ticketList"] = ticketList;

            string role = Session["role_id"].ToString();
            string roleName = "";

            switch (role)
            {
                case "1":
                    roleName = "Coordinator";
                    break;
                case "2":
                    roleName = "Client";
                    break;
                case "3":
                    roleName = "Coordinador Mesa";
                    break;
                default:
                    break;
            }

            return RedirectToAction("IndexViewSearchParameterSASCoordinator", "User", new { data = "viewSAS", typeUser = roleName });
        }
    }
}
