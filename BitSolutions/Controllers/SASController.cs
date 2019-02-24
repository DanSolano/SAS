using BitSolutions.Models.SAS;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Xml;

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
        public ActionResult CreateSAS(FormCollection collection, HttpPostedFileBase uploadFile)
        {
            try
            {
                string resultOp = "";

                if (ModelState.IsValid)
                {
                    if (collection["Description"] == "")
                    {
                        ViewBag.message = "Description is required.";
                        ViewBag.messageType = "Warning";

                        return RedirectToAction("Intermediary", "User", new { data = "addSAS", message = ViewBag.message, messageType = ViewBag.messageType });
                    }
                    else
                    {
                        if (uploadFile != null)
                        {
                            switch (uploadFile.ContentType)
                            {
                                case "text/plain": // Txt
                                    resultOp = intermediaryLoadDataInDatabase(collection, uploadFile, "file");
                                    break;
                                case "application/msword": // Microsoft Word
                                    resultOp = intermediaryLoadDataInDatabase(collection, uploadFile, "file");
                                    break;
                                case "application/pdf": // (PDF)
                                    intermediaryLoadDataInDatabase(collection, uploadFile, "file");
                                    break;
                                case "application/vnd.ms-powerpoint": // Microsoft PowerPoint
                                    resultOp = intermediaryLoadDataInDatabase(collection, uploadFile, "file");
                                    break;
                                case "application/vnd.ms-excel": // Microsoft Excel
                                    resultOp = intermediaryLoadDataInDatabase(collection, uploadFile, "file");
                                    break;
                                case "image/x-icon": // Formato Icon
                                    resultOp = intermediaryLoadDataInDatabase(collection, uploadFile, "image");
                                    break;
                                case "image/jpeg": // JPEG
                                    resultOp = intermediaryLoadDataInDatabase(collection, uploadFile, "image");
                                    break;
                                case "image/svg+xml": // Gráficos Vectoriales (SVG)
                                    resultOp = intermediaryLoadDataInDatabase(collection, uploadFile, "image");
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

                        if (!resultOp.Contains("Error"))
                        {
                            SendEmailToUser(resultOp);

                            return RedirectToAction("Intermediary", "User", new { data = "addSAS", message = resultOp, messageType = "Error" });
                        }
                        else
                        {
                            return RedirectToAction("Intermediary", "User", new { data = "addSAS", message = resultOp, messageType = "Success" });
                        }

                        
                    }   
                }

                return View();
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Authorize]
        private string SendEmailToUser(string idTicket)
        {
            string path = "", userName = "", password = "";

            XmlDocument doc = new XmlDocument();

            path = Server.MapPath(".");
            doc.Load((path + "\\Configurations\\SMTPConf.xml"));

            foreach (XmlNode node in doc.DocumentElement.ChildNodes)
            {
                if (node.Name.Trim() == "userName")
                {
                    userName = node.InnerText.Trim(); //
                }
                else
                {
                    password = node.InnerText.Trim(); //
                }
            }

            try
            {
                SmtpClient server = new SmtpClient("smtp.gmail.com", 587);

                server.Credentials = new System.Net.NetworkCredential(userName, password);
                server.EnableSsl = true;

                MailMessage mnsj = new MailMessage();

                mnsj.Subject = "Request for attention";

                mnsj.To.Add(new MailAddress(Session["email"].ToString()));
                mnsj.From = new MailAddress(userName, "BitSolutions");

                mnsj.IsBodyHtml = true;

                mnsj.Body = "<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'><html xmlns='http://www.w3.org/1999/xhtml' xmlns:v='urn:schemas-microsoft-com:vml' xmlns:o='urn:schemas-microsoft-com:office:office'><head><meta http-equiv='Content-Type' content='text/html; charset=UTF-8' /><meta name='viewport' content='width=device-width, initial-scale=1.0'/><title>Proof of service request</title><style type='text/css'>@import url(https://fonts.googleapis.com/css?family=Lato:400,700,400italic,700italic&subset=latin,latin-ext);table{font-family:'Lato',Arial,sans-serif;-webkit-font-smoothing:antialiased;-moz-font-smoothing:antialiased;font-smoothing:antialiased}@media only screen and (max-width: 700px){.full-width-container{padding:0 !important}.container{width:100% !important}.header td{padding:30px 15px 30px 15px !important}.projects-list{display:block !important}.projects-list tr{display:block !important}.projects-list td{display:block !important}.projects-list tbody{display:block !important}.projects-list img{margin:0 auto 25px auto}.half-block{display:block !important}.half-block tr{display:block !important}.half-block td{display:block !important}.half-block__image{width:100% !important;background-size:cover}.half-block__content{width:100% !important;box-sizing:border-box;padding:25px 15px 25px 15px !important}.hero-subheader__title{padding:80px 15px 15px 15px !important;font-size:35px !important}.hero-subheader__content{padding:0 15px 90px 15px !important}.title-block{padding:0 15px 0 15px}.paragraph-block__content{padding:25px 15px 18px 15px !important}.info-bullets{display:block !important}.info-bullets tr{display:block !important}.info-bullets td{display:block !important}.info-bullets tbody{display:block}.info-bullets__icon{text-align:center;padding:0 0 15px 0 !important}.info-bullets__content{text-align:center}.info-bullets__block{padding:25px !important}.cta-block__title{padding:35px 15px 0 15px !important}.cta-block__content{padding:20px 15px 27px 15px !important}.cta-block__button{padding:0 15px 0 15px !important}}</style><!--[if gte mso 9]><xml> <o:OfficeDocumentSettings> <o:AllowPNG/> <o:PixelsPerInch>96</o:PixelsPerInch> </o:OfficeDocumentSettings> </xml><![endif]--></head><body style='padding: 0; margin: 0;' bgcolor='#eeeeee'> <span style='color:transparent !important; overflow:hidden !important; display:none !important; line-height:0px !important; height:0 !important; opacity:0 !important; visibility:hidden !important; width:0 !important; mso-hide:all;'>This is your preheader text for this email (Read more about email preheaders here - https://goo.gl/e60hyK)</span><table class='full-width-container' border='0' cellpadding='0' cellspacing='0' height='100%' width='100%' bgcolor='#eeeeee' style='width: 100%; height: 100%; padding: 30px 0 30px 0;'><tr><td align='center' valign='top'><table class='container' border='0' cellpadding='0' cellspacing='0' width='700' bgcolor='#ffffff' style='width: 700px;'><tr><td align='center' valign='top'><table class='container header' border='0' cellpadding='0' cellspacing='0' width='620' style='width: 620px;'><tr><td style='padding: 30px 0 30px 0; border-bottom: solid 1px #eeeeee;' align='left'> <a href='#' style='font-size: 30px; text-decoration: none; color: #000000;'><strong>Dear</strong> Danny Zúñiga Barrantes</a></td></tr></table><table class='container hero-subheader' border='0' cellpadding='0' cellspacing='0' width='620' style='width: 620px;'><tr><td class='hero-subheader__title' style='font-size: 43px; font-weight: bold; padding: 80px 0 15px 0;' align='left'>Information</td></tr><tr><td class='hero-subheader__content' style='font-size: 16px; line-height: 27px; color: #969696; padding: 0 60px 90px 0;' align='left'>You are informed that a request for attention has been made to your name, please check the following section.</td></tr></table><table class='container title-block' border='0' cellpadding='0' cellspacing='0' width='100%'><tr><td align='center' valign='top'><table class='container' border='0' cellpadding='0' cellspacing='0' width='620' style='width: 620px;'><tr><td style='border-bottom: solid 1px #eeeeee; padding: 35px 0 18px 0; font-size: 26px;' align='left'>Ticket number: <strong>1</strong></td></tr></table></td></tr></table><table class='container title-block' border='0' cellpadding='0' cellspacing='0' width='100%'><tr><td align='center' valign='top'><table class='container' border='0' cellpadding='0' cellspacing='0' width='620' style='width: 620px;'><tr><td style='border-bottom: solid 1px #eeeeee; padding: 35px 0 18px 0; font-size: 26px;' align='left'>Description</td></tr></table></td></tr></table><table class='container paragraph-block' border='0' cellpadding='0' cellspacing='0' width='100%'><tr><td align='center' valign='top'><table class='container' border='0' cellpadding='0' cellspacing='0' width='620' style='width: 620px;'><tr><td class='paragraph-block__content' style='padding: 25px 0 18px 0; font-size: 16px; line-height: 27px; color: #969696;' align='left'>Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.</td></tr></table></td></tr></table><table class='container' border='0' cellpadding='0' cellspacing='0' width='100%' style='padding-top: 25px;' align='center'><tr><td align='center'><table class='container' border='0' cellpadding='0' cellspacing='0' width='620' align='center' style='border-bottom: solid 1px #eeeeee; width: 620px;'><tr><td align='center'>&nbsp;</td></tr></table></td></tr></table><table class='container cta-block' border='0' cellpadding='0' cellspacing='0' width='100%'><tr><td align='center' valign='top'><table class='container' border='0' cellpadding='0' cellspacing='0' width='620' style='width: 620px;'><tr><td class='cta-block__title' style='padding: 35px 0 0 0; font-size: 26px; text-align: center;'>About Us</td></tr><tr><td class='cta-block__content' style='padding: 20px 0 27px 0; font-size: 16px; line-height: 27px; color: #969696; text-align: center;'>Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.</td></tr></table></td></tr></table><table class='container' border='0' cellpadding='0' cellspacing='0' width='100%' style='padding-top: 25px;' align='center'><tr><td align='center'><table class='container' border='0' cellpadding='0' cellspacing='0' width='620' align='center' style='border-bottom: solid 1px #eeeeee; width: 620px;'><tr><td align='center'>&nbsp;</td></tr></table></td></tr></table><table class='container info-bullets' border='0' cellpadding='0' cellspacing='0' width='100%' align='center'><tr><td align='center'><table class='container' border='0' cellpadding='0' cellspacing='0' width='620' align='center' style='width: 620px;'><tr><td class='info-bullets__block' style='padding: 30px 30px 15px 30px;' align='center'><table class='container' border='0' cellpadding='0' cellspacing='0' align='center'><tr><td class='info-bullets__icon' style='padding: 0 15px 0 0;'> <img src='img/img13.png'></td><td class='info-bullets__content' style='color: #969696; font-size: 16px;'>contact@example.com</td></tr></table></td></tr></table></td></tr></table><table class='container' border='0' cellpadding='0' cellspacing='0' width='100%' align='center'><tr><td align='center'><table class='container' border='0' cellpadding='0' cellspacing='0' width='620' align='center' style='border-top: 1px solid #eeeeee; width: 620px;'><tr><td style='text-align: center; padding: 50px 0 10px 0;'> <a href='#' style='font-size: 28px; text-decoration: none; color: #d5d5d5;'>MailPortfolio</a></td></tr><tr><td align='middle'><table width='60' height='2' border='0' cellpadding='0' cellspacing='0' style='width: 60px; height: 2px;'><tr><td align='middle' width='60' height='2' style='background-color: #eeeeee; width: 60px; height: 2px; font-size: 1px;'><img src='img/img16.jpg'></td></tr></table></td></tr><tr><td style='color: #d5d5d5; text-align: center; font-size: 15px; padding: 10px 0 60px 0; line-height: 22px;'>Copyright &copy; 2019 <a target='_blank' style='text-decoration: none; border-bottom: 1px solid #d5d5d5; color: #d5d5d5;'>BitSolutions</a>. <br />All rights reserved.</td></tr></table></td></tr></table></td></tr></table></td></tr></table></body></html>";

                /* Enviar */
                server.Send(mnsj);

                return "Success";
            }
            catch (Exception ex)
            {
                return "Error "+ ex.Message;
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
                    roleName = "HelpDeskCoordinator";
                    break;
                default:
                    break;
            }

            return RedirectToAction("IndexViewSearchParameterSASCoordinator", "User", new { data = "viewSAS", typeUser = roleName });
        }
    }
}
