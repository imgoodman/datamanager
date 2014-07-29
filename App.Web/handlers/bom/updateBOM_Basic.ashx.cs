using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.Dll;
using System.Web.SessionState;

namespace App.Web.handlers.bom
{
    /// <summary>
    /// Summary description for updateBOM_Basic
    /// </summary>
    public class updateBOM_Basic : IHttpHandler,IRequiresSessionState
    {
        BOMService bms = new BOMService();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            int id = Convert.ToInt32(context.Request.Form["id"]);
            string name = context.Request.Form["name"];
            string desc = context.Request.Form["desc"];
            bms.bom.ID = id;
            bms.bom.Name = name;
            bms.bom.Description = desc;
            bms.bom.LastModifier = new Model.User {ID=int.Parse(context.Session["userid"].ToString()) };
            var r = bms.updateBOM_Basic();
            context.Response.Write(r ? "1" : "0");
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