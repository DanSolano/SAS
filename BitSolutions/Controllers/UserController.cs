﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BitSolutions.Controllers
{
    public class UserController : Controller
    {
        //[Authorize]
        //public RedirectResult Intermediary()
        //{
        //    string role = Session["role_id"].ToString();

        //    switch (role)
        //    {
        //        case "1": //Administrador
        //            RedirectToAction("Index", "User", new { data = "Datos de prueba" });
        //            break;
        //        case "2": //Cliente
        //            RedirectToAction("Index", "User", new { data = "Datos de prueba" });
        //            break;
        //        default:
        //            break;
        //    }

        //    RedirectToAction("Index", "User", new { data = "Datos de prueba" });
        //}

        // GET: User
        [Authorize]
        public ActionResult Index(string data)
        {
            ViewBag.prueba = data;
            return View();
        }

        // GET: User/Details/5
        public ActionResult Details(int id)
        {
            return View();
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
