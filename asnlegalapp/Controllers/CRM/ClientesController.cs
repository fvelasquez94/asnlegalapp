using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using asnlegalapp.Controllers.Session;
using asnlegalapp.Models;
using Newtonsoft.Json;

namespace asnlegalapp.Controllers.CRM
{
    public class ClientesController : Controller
    {
        private ASN_DBEntities db = new ASN_DBEntities();
        private Cls_session cls_session = new Cls_session();
        private Cls_cookies cls_cookies = new Cls_cookies();

        // GET: Clientes
        public ActionResult Activos()
        {
            if (cls_session.checkSession())
            {
                Sys_Usuarios activeuser = Session["activeUser"] as Sys_Usuarios;
                //HEADER
                //ACTIVE PAGES
                ViewData["Menu"] = "CRM";
                ViewData["Page"] = "Clientes";
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

                var tb_Clientes = db.Tb_Clientes.Where(c=>c.Lead==false).Include(t => t.Sys_Empresas);
                return View(tb_Clientes.ToList());
            }
            else
            {
                return RedirectToAction("Login", "Home", new { access = false });
            }
        
        }
        public ActionResult Potenciales()
        {
            if (cls_session.checkSession())
            {
                Sys_Usuarios activeuser = Session["activeUser"] as Sys_Usuarios;
                //HEADER
                //ACTIVE PAGES
                ViewData["Menu"] = "CRM";
                ViewData["Page"] = "Clientes";
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

                var tb_Clientes = db.Tb_Clientes.Where(c => c.Lead == true).Include(t => t.Sys_Empresas);
                return View(tb_Clientes.ToList());
            }
            else
            {
                return RedirectToAction("Login", "Home", new { access = false });
            }

        }
        // GET: Clientes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tb_Clientes tb_Clientes = db.Tb_Clientes.Find(id);
            if (tb_Clientes == null)
            {
                return HttpNotFound();
            }
            return View(tb_Clientes);
        }

        // GET: Clientes/Create
        public ActionResult Nuevo()
        {
            if (cls_session.checkSession())
            {
                Sys_Usuarios activeuser = Session["activeUser"] as Sys_Usuarios;
                //HEADER
                //ACTIVE PAGES
                ViewData["Menu"] = "CRM";
                ViewData["Page"] = "Clientes";
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

        // POST: Clientes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Nuevo([Bind(Include = "ID_cliente,Nombre,Apellido,DUI,Telefono,Correo,Direccion,Contrasena,Empresa,Lead,ID_empresa")] Tb_Clientes tb_Clientes)
        {
            tb_Clientes.ID_empresa = 1;
            tb_Clientes.Contrasena = "";
            tb_Clientes.Lead = false;
            if (tb_Clientes.Empresa == null) { tb_Clientes.Empresa = ""; }
            if (ModelState.IsValid)
            {
                db.Tb_Clientes.Add(tb_Clientes);
                db.SaveChanges();
                return RedirectToAction("Activos", "Clientes");
            }
            else {
                return RedirectToAction("Activos", "Clientes");
            }
        }

        // GET: Clientes/Edit/5
        public ActionResult Editar(int? id)
        {
            if (cls_session.checkSession())
            {
                Sys_Usuarios activeuser = Session["activeUser"] as Sys_Usuarios;
                //HEADER
                //ACTIVE PAGES
                ViewData["Menu"] = "CRM";
                ViewData["Page"] = "Clientes";
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
                Tb_Clientes tb_Clientes = db.Tb_Clientes.Find(id);
                ViewBag.ID_empresa = new SelectList(db.Sys_Empresas, "ID_empresa", "Nombre", tb_Clientes.ID_empresa);
                return View(tb_Clientes);
            }
            else
            {
                return RedirectToAction("Login", "Home", new { access = false });
            }

          
        }

        // POST: Clientes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar([Bind(Include = "ID_cliente,Nombre,Apellido,DUI,Telefono,Correo,Direccion,Contrasena,Empresa,Lead,ID_empresa")] Tb_Clientes tb_Clientes)
        {
            tb_Clientes.ID_empresa = 1;
            if (tb_Clientes.Empresa == null) { tb_Clientes.Empresa = ""; }
            tb_Clientes.Contrasena = "";
            if (ModelState.IsValid)
            {
                db.Entry(tb_Clientes).State = EntityState.Modified;
                db.SaveChanges();
               
            }
            return RedirectToAction("Activos", "Clientes");
        }



        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Tb_Clientes tb_Clientes = db.Tb_Clientes.Find(id);
                db.Tb_Clientes.Remove(tb_Clientes);
                db.SaveChanges();
            }
            catch {

            }

            return RedirectToAction("Activos");
        }
        public ActionResult DeleteConfirmedPotenciales(int id)
        {
            Tb_Clientes tb_Clientes = db.Tb_Clientes.Find(id);
            db.Tb_Clientes.Remove(tb_Clientes);
            db.SaveChanges();
            return RedirectToAction("Potenciales");
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
