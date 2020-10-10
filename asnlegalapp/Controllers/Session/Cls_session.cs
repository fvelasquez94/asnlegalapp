using asnlegalapp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace asnlegalapp.Controllers.Session
{
    public class Cls_session
    {
        private ASN_DBEntities db = new ASN_DBEntities();
        public bool checkSession()
        {
            var flag = false;
            Sys_Usuarios activeuser = HttpContext.Current.Session["activeUser"] as Sys_Usuarios;
            if (activeuser != null)
            {
                flag = true;
            }
            else
            {
                if (HttpContext.Current.Request.Cookies["correo"] != null)
                {
                    //COMO YA EXISTE NO NECESITAMOS RECREARLA Y SOLO VOLVEMOS A INICIAR SESION
                    flag = true;
                    var email = HttpContext.Current.Request.Cookies["correo"].Value;
                    var password = HttpContext.Current.Request.Cookies["pass"].Value;
                    HttpContext.Current.Session["activeUser"] = (from a in db.Sys_Usuarios where (a.Email == email && a.Password == password && a.Activo == true) select a).FirstOrDefault();
                    Sys_Usuarios activeuserAgain = HttpContext.Current.Session["activeUser"] as Sys_Usuarios;
                    if (activeuserAgain != null)
                    {
                        flag = true;
                    }
                    else { flag = false; }


                }
                else
                {
                    flag = false;
                }
            }
            return flag;
        }
    }
}