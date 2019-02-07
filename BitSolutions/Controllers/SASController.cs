using BitSolutions.Models;
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
        /// 
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
        /// 
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
                route = LoadFileInDatabase(uploadFile, contentType, ticketGuid);
            }

            result = LoadDataInDatabase(collection, route, ticketGuid, contentType);

            return result;
        }

        /// <summary>
        /// 
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
            string category = collection["Category"];
            string username = Session["username"].ToString();
            DateTime now = DateTime.Now;

            ObjectParameter result = new ObjectParameter("result", typeof(string));

            if (contentType == "file")
            {
                dbManager.spAddTicket(username, now, now, "Active", "High", description, category,"no route", route, now, result);
            }
            else
            {
                dbManager.spAddTicket(username, now, now, "Active", "High", description, category, route, "no route", now, result);
            }
            
            return result.Value.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="uploadFile"></param>
        /// <param name="contentType"></param>
        /// <param name="ticketGuid"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        private string LoadFileInDatabase(HttpPostedFileBase uploadFile, string contentType,string ticketGuid)
        {
            string route = "";

            string file = Server.MapPath(".") + "/LoadImages/"+ ticketGuid;

            if (!System.IO.Directory.Exists(file))
                System.IO.Directory.CreateDirectory(file);

            route = file + "\\" + uploadFile.FileName;

            uploadFile.SaveAs(route);

            return route;
        }


        //public ActionResult GetInfoTicket(string data = null, string message = null)
        //{
        //    ViewBag.TypeForm = data;
        //    ViewBag.message = message;
            
        //    List<Ticket> ticketList = dbManager.Tickets.ToList();

        //    ViewBag.ticketList = ticketList;

        //    return View("~/Views/User/IndexCoordinator.cshtml");
        //}




        [Authorize]
        [HttpPost]
        public ActionResult ShowRequestForAttention()
        {
            return View();
        }

        // POST: SAS/Edit/5
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

        // GET: SAS/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: SAS/Delete/5
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
