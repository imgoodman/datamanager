using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.Dll;
using System.Web.SessionState;

namespace App.Web.handlers.user
{
    /// <summary>
    /// Summary description for login
    /// </summary>
    public class login : IHttpHandler,IRequiresSessionState
    {
        Dll.SysMethod.DBUserManager bus = new Dll.SysMethod.DBUserManager();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string username = context.Request.Form["username"];
            string password = context.Request.Form["password"];
            var r = bus.CheckLogin(username, password);
            context.Session["user"] = r;
            context.Session["userid"] = r.ID.ToString();
            context.Response.Write((object.Equals(r, null) ? "0" : r.ID.ToString()));
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}