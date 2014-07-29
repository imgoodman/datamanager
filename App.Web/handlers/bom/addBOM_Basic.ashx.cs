using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.Dll;
using System.Web.SessionState;

namespace App.Web.handlers.bom
{
    /// <summary>
    /// Summary description for addBOM_Basic
    /// </summary>
    public class addBOM_Basic : IHttpHandler,IRequiresSessionState
    {
        BOMService bms = new BOMService();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string name = context.Request.Form["name"];
            string desc = context.Request.Form["desc"];
            bms.bom.Name = name;
            bms.bom.Description = desc;
            bms.bom.Creator = new Model.User {ID=int.Parse(context.Session["userid"].ToString()) };
            var id = bms.addBOM_Basic();
            context.Response.Write(id);
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