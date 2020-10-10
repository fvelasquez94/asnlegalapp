using asnlegalapp.Controllers.Session;
using asnlegalapp.Models;
using CrystalDecisions.CrystalReports.Engine;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace asnlegalapp.Controllers.CRM
{
    public class CotizacionesController : Controller
    {
        private ASN_DBEntities db = new ASN_DBEntities();
        private Cls_session cls_session = new Cls_session();
        private Cls_cookies cls_cookies = new Cls_cookies();
        // GET: CRM
        public ActionResult Historial()
        {
            if (cls_session.checkSession())
            {
                Sys_Usuarios activeuser = Session["activeUser"] as Sys_Usuarios;
                //HEADER
                //ACTIVE PAGES
                ViewData["Menu"] = "CRM";
                ViewData["Page"] = "Cotizaciones";
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


                List<Tb_Cotizaciones> lstcotizaciones = new List<Tb_Cotizaciones>();
                lstcotizaciones = db.Tb_Cotizaciones.ToList();

                return View(lstcotizaciones);
            }
            else
            {
                return RedirectToAction("Login", "Home", new { access = false });
            }
        }

        public ActionResult Imprimir()
        {

                    ReportDocument rd = new ReportDocument();

                    rd.Load(Path.Combine(Server.MapPath("~/Reports"), "rpt_FacturaTest.rpt"));

                    var filePathOriginal = Server.MapPath("/Reports/pdf");
                    Response.Buffer = false;
                    Response.ClearContent();
                    Response.ClearHeaders();

                    //PARA VISUALIZAR
                    Response.AppendHeader("Content-Disposition", "inline; filename=" + "Test.pdf; ");

                    Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                    stream.Seek(0, SeekOrigin.Begin);


                    return File(stream, System.Net.Mime.MediaTypeNames.Application.Pdf);

        }


        public ActionResult Nueva()
        {
            if (cls_session.checkSession())
            {
                Sys_Usuarios activeuser = Session["activeUser"] as Sys_Usuarios;
                //HEADER
                //ACTIVE PAGES
                ViewData["Menu"] = "CRM";
                ViewData["Page"] = "Cotizaciones";
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
                var tb_Clientes = db.Tb_Clientes.Where(c => c.Lead == false);
                ViewBag.lstClientes = tb_Clientes.ToList();
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Home", new { access = false });
            }
        }

        public ActionResult Vista_previa(int id)
        {
            if (cls_session.checkSession())
            {
                Sys_Usuarios activeuser = Session["activeUser"] as Sys_Usuarios;
                //HEADER
                //ACTIVE PAGES
                ViewData["Menu"] = "CRM";
                ViewData["Page"] = "Cotizaciones";
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
                var cotizacion = db.Tb_Cotizaciones.Where(c => c.ID_cotizacion == id).FirstOrDefault();
                return View(cotizacion);
            }
            else
            {
                return RedirectToAction("Login", "Home", new { access = false });
            }
        }
        public ActionResult DeleteConfirmed(int id)
        {
            Tb_Cotizaciones tb_cotizaciones = db.Tb_Cotizaciones.Find(id);
            db.Tb_Cotizaciones.Remove(tb_cotizaciones);
            db.SaveChanges();
            return RedirectToAction("Historial");
        }

        public ActionResult Crear(int idcliente, string nombrecliente, string empresacliente, string telefonocliente, string fecha, List<detallescot> detalles)
        {
            try
            {


                Tb_Cotizaciones nuevaCotizacion = new Tb_Cotizaciones();
                nuevaCotizacion.ID_empresa = 1;
                nuevaCotizacion.Telefono = telefonocliente;
                nuevaCotizacion.No_Cotizacion = "";
                nuevaCotizacion.Empresa = empresacliente;
                nuevaCotizacion.Fecha = Convert.ToDateTime(fecha);
                if (idcliente == 0) {
                    //creamos cliente
                    Tb_Clientes nuevoCliente = new Tb_Clientes();
                    nuevoCliente.Nombre = nombrecliente;
                    nuevoCliente.Apellido = "";
                    nuevoCliente.DUI = "";
                    nuevoCliente.Telefono = telefonocliente;
                    nuevoCliente.Correo = "";
                    nuevoCliente.Direccion = "";
                    nuevoCliente.Contrasena = "";
                    nuevoCliente.Empresa = empresacliente;
                    nuevoCliente.Lead = true;
                    nuevoCliente.ID_empresa = 1;

                    db.Tb_Clientes.Add(nuevoCliente);
                    db.SaveChanges();

                    nuevaCotizacion.ID_cliente = nuevoCliente.ID_cliente;
                }
                else {
                    nuevaCotizacion.ID_cliente = idcliente;
                }

                db.Tb_Cotizaciones.Add(nuevaCotizacion);
                db.SaveChanges();

                //Detalles
                if (detalles.Count > 0) {
                    foreach (var item in detalles)
                    {
                        Tb_Cotizacion_detalle nuevoDetalleCot = new Tb_Cotizacion_detalle();
                        nuevoDetalleCot.Descripcion = item.descripcion;
                        nuevoDetalleCot.Costo = Convert.ToDecimal(item.costo);
                        nuevoDetalleCot.Horas = Convert.ToInt32(item.horas);
                        nuevoDetalleCot.ID_servicio = 0;
                        nuevoDetalleCot.ID_cotizacion = nuevaCotizacion.ID_cotizacion;
                        db.Tb_Cotizacion_detalle.Add(nuevoDetalleCot);
                       
                    }
                    db.SaveChanges();
                }
  
       
                    var result = "SUCCESS";
                    return Json(result, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                var resulterror = ex.Message;
                return Json(resulterror, JsonRequestBehavior.AllowGet);

            }
        }


    }
}