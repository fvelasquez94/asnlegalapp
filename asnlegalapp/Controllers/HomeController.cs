using asnlegalapp.Controllers.Session;
using asnlegalapp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace asnlegalapp.Controllers
{
    public class HomeController : Controller
    {
        private ASN_DBEntities db = new ASN_DBEntities();
        private Cls_session cls_session = new Cls_session();
        private Cls_cookies cls_cookies = new Cls_cookies();
        public ActionResult Login(bool access = true, int? logpage = 0)
        {
            if (access == false)
            {
                if (logpage == 0)
                {
                    TempData["advertencia"] = "Sesion expirada.";
                }
                else if (logpage == 1)
                {
                    TempData["advertencia"] = "Correo o contrasena erronea.";
                }

            }

            HttpCookie aCookieCorreo = Request.Cookies["correo"];
            HttpCookie aCookiePassword = Request.Cookies["pass"];
            HttpCookie aCookieRememberme = Request.Cookies["rememberme"];

            try
            {
                var correo = Server.HtmlEncode(aCookieCorreo.Value).ToString();
                var pass = Server.HtmlEncode(aCookiePassword.Value).ToString();
                int remember = Convert.ToInt32(Server.HtmlEncode(aCookieRememberme.Value));

                if (remember == 1) { ViewBag.remember = true; } else { ViewBag.remember = false; }
                ViewBag.correo = correo;
                ViewBag.pass = pass;

            }
            catch
            {
                ViewBag.remember = false;

            }
            return View("~/Views/Home/Login.cshtml");
        }


        public ActionResult Log_in(string email, string password, bool? rememberme = true)
        {
            if (rememberme == null) { rememberme = false; }
            Session["activeUser"] = (from a in db.Sys_Usuarios where (a.Email == email && a.Password == password && a.Activo == true) select a).FirstOrDefault();
            if (Session["activeUser"] != null)
            {
                //Validamos cookies
                cls_cookies.Check_cookies(email, password, rememberme);

               Sys_Usuarios activeuser = Session["activeUser"] as Sys_Usuarios;
                List<string> r = new List<string>(activeuser.Roles.Split(new string[] { "," }, StringSplitOptions.None));


                return RedirectToAction("Index", "App", null);
                
            }

            return RedirectToAction("Login", "Home", new { access = false, logpage = 1 });
        }

        public ActionResult Log_out()
        {
            Session.RemoveAll();
            //Global_variables.active_user.Name = null;
            //Global_variables.active_Departments = null;
            //Global_variables.active_Roles = null;
            if (Request.Cookies["correo"] != null)
            {
                var c = new HttpCookie("correo");
                c.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(c);
            }
            if (Request.Cookies["pass"] != null)
            {
                var c = new HttpCookie("pass");
                c.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(c);
            }
            if (Request.Cookies["rememberme"] != null)
            {
                var c = new HttpCookie("rememberme");
                c.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(c);
            }



            return RedirectToAction("Login", "Home", new { access = true });
        }


    }
}