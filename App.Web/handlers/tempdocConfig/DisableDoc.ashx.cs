using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.Dll;

namespace App.Web.handlers.tempdocConfig
{
    /// <summary>
    /// Summary description for DisableDoc
    /// </summary>
    public class DisableDoc : IHttpHandler
    {
        TempDocConfigService dcs = new TempDocConfigService();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            int id = Convert.ToInt32(context.Request.Form["ID"]);
            int userid = Convert.ToInt32(context.Request.Form["userid"]);
            var r = dcs.DisableDoc(new Model.Document { ID = id, LastModifier = new Model.User { ID = userid } });
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