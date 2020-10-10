using asnlegalapp.Controllers.Session;
using asnlegalapp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace asnlegalapp.Controllers
{
    public class AppController : Controller
    {
        private ASN_DBEntities db = new ASN_DBEntities();
        private Cls_session cls_session = new Cls_session();
        private Cls_cookies cls_cookies = new Cls_cookies();
        // GET: App
        public ActionResult Index()
        {
            if (cls_session.checkSession())
            {
                Sys_Usuarios activeuser = Session["activeUser"] as Sys_Usuarios;
                //HEADER
                //ACTIVE PAGES
                ViewData["Menu"] = "App";
                ViewData["Page"] = "Index";
                ViewBag.menunameid = "";
                ViewBag.submenunameid = "";
                //AUTH
                List<string> s = new List<string>(activeuser.Departamentos.Split(new string[] { "," }, StringSplitOptions.None));
                ViewBag.lstDepartments = JsonConvert.SerializeObject(s);
                List<string> r = new List<string>(activeuser.Roles.Split(new string[] { "," }, StringSplitOptions.None));
                ViewBag.lstRoles = JsonConvert.SerializeObject(r);
                //NOTIFICATIONS
                List<Sys_Notificaciones> lstAlerts = (from a in db.Sys_Notificaciones where (a.ID_usuario == activeuser.ID_usuario && a.Activo == true) select a).OrderByDescending(x => x.Fecha).Take(4).ToList();
                ViewBag.notifications = lstAlerts;
                //DATA VIEW
                ViewBag.activeuser = activeuser;
                //END HEADER

                return View();
            }
            else
            {
                return RedirectToAction("Login", "Home", new { access = false });
            }
        }
    }
}