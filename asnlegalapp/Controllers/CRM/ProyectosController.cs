using asnlegalapp.Controllers.Session;
using asnlegalapp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace asnlegalapp.Controllers.CRM
{
    public class ProyectosController : Controller
    {
        private ASN_DBEntities db = new ASN_DBEntities();
        private Cls_session cls_session = new Cls_session();
        private Cls_cookies cls_cookies = new Cls_cookies();
        // GET: CRM
        public ActionResult Activos()
        {
            if (cls_session.checkSession())
            {
                Sys_Usuarios activeuser = Session["activeUser"] as Sys_Usuarios;
                //HEADER
                //ACTIVE PAGES
                ViewData["Menu"] = "CRM";
                ViewData["Page"] = "Proyectos";
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
                List<Tb_Proyectos> proyectos = new List<Tb_Proyectos>();
                proyectos = (from a in db.Tb_Proyectos where (a.Activo == true) select a).ToList();

                return View(proyectos);
            }
            else
            {
                return RedirectToAction("Login", "Home", new { access = false });
            }
        }

        public ActionResult Nuevo()
        {
            if (cls_session.checkSession())
            {
                Sys_Usuarios activeuser = Session["activeUser"] as Sys_Usuarios;
                //HEADER
                //ACTIVE PAGES
                ViewData["Menu"] = "CRM";
                ViewData["Page"] = "Proyectos";
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
                var tb_Clientes = db.Tb_Clientes.Where(c => c.Lead == false).Include(t => t.Sys_Empresas);
                ViewBag.lstClientes = tb_Clientes.ToList();
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Home", new { access = false });
            }
        }

        //POST/CREAR/PROYECTO
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Nuevo([Bind(Include = "ID_proyecto,Nombre,Descripcion,Notas,Fecha_creacion,Fecha_finalizacon,Tipo,Prioridad,ID_cliente,Activo,ID_empresa,Estado")] Tb_Proyectos tb_Proyectos)
        {
            tb_Proyectos.Notas = "";
            tb_Proyectos.Fecha_creacion = DateTime.Today;
            tb_Proyectos.Fecha_finalizacon = DateTime.Today;

            tb_Proyectos.Activo = true;
            tb_Proyectos.ID_empresa = 1;

            if (ModelState.IsValid)
            {
                db.Tb_Proyectos.Add(tb_Proyectos);
                db.SaveChanges();
                return RedirectToAction("Activos", "Proyectos");
            }
            else
            {
                return RedirectToAction("Activos", "Proyectos");
            }
        }




        public ActionResult Editar(int? id)
        {
            if (cls_session.checkSession())
            {
                Sys_Usuarios activeuser = Session["activeUser"] as Sys_Usuarios;
                //HEADER
                //ACTIVE PAGES
                ViewData["Menu"] = "CRM";
                ViewData["Page"] = "Proyectos";
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

                var proyecto = (from a in db.Tb_Proyectos where (a.ID_proyecto == id) select a).FirstOrDefault();

                List<Tb_Clientes> lstClientes = new List<Tb_Clientes>();
                lstClientes = (from a in db.Tb_Clientes where (a.Lead == false) select a).ToList();
                ViewBag.lstClientes = lstClientes;



                return View(proyecto);

            }
            else
            {
                return RedirectToAction("Login", "Home", new { access = false });
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar([Bind(Include = "ID_proyecto,Nombre,Descripcion,Notas,Fecha_creacion,Fecha_finalizacon,Tipo,Prioridad,ID_cliente,Activo,ID_empresa,Estado")] Tb_Proyectos tb_Proyectos)
        {
            tb_Proyectos.Notas = "";
            tb_Proyectos.Fecha_creacion = DateTime.Today;
            tb_Proyectos.Fecha_finalizacon = DateTime.Today;

            //tb_Proyectos.Activo = true;
            tb_Proyectos.ID_empresa = 1;

            if (ModelState.IsValid)
            {
                db.Entry(tb_Proyectos).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Activos", "Proyectos");
            }
            else
            {
                return RedirectToAction("Activos", "Proyectos");
            }
        }
        public ActionResult Archivados()
        {
            if (cls_session.checkSession())
            {
                Sys_Usuarios activeuser = Session["activeUser"] as Sys_Usuarios;
                //HEADER
                //ACTIVE PAGES
                ViewData["Menu"] = "CRM";
                ViewData["Page"] = "Proyectos";
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
                List<Tb_Proyectos> proyectos = new List<Tb_Proyectos>();
                proyectos = (from a in db.Tb_Proyectos where (a.Activo == false) select a).ToList();

                return View(proyectos);
               
            }
            else
            {
                return RedirectToAction("Login", "Home", new { access = false });
            }
        }

        public ActionResult DeleteConfirmed(int id)
        {
            Tb_Proyectos tb_Proyectos = db.Tb_Proyectos.Find(id);
            db.Tb_Proyectos.Remove(tb_Proyectos);
            db.SaveChanges();
            return RedirectToAction("Activos");
        }
    }
}