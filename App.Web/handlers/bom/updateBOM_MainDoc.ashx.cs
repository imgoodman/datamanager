using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.Dll;
using System.Web.SessionState;

namespace App.Web.handlers.bom
{
    /// <summary>
    /// Summary description for updateBOM_MainDoc
    /// </summary>
    public class updateBOM_MainDoc : IHttpHandler, IRequiresSessionState
    {
        BOMService bms = new BOMService();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            int id = Convert.ToInt32(context.Request.Form["id"]);
            int maindocid = Convert.ToInt32(context.Request.Form["maindocid"]);
            bms.bom.ID = id;
            bms.bom.MainDoc = new Model.BOMDocument();
            bms.bom.MainDoc.ID = maindocid;
            bms.bom.LastModifier = new Model.User { ID = Convert.ToInt32(context.Session["userid"].ToString()) };
            var r = bms.updateBOM_MainDoc();
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