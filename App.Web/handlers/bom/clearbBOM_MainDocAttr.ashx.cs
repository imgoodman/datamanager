using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.Dll;


namespace App.Web.handlers.bom
{
    /// <summary>
    /// Summary description for clearbBOM_MainDocAttr
    /// </summary>
    public class clearbBOM_MainDocAttr : IHttpHandler
    {
        BOMService bms = new BOMService();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            int id = Convert.ToInt32(context.Request.Form["id"]);
            bms.bom.ID = id;
            var r = bms.clearMainDocAttr();
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