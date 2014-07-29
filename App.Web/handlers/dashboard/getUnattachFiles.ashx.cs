using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.Dll;
using Newtonsoft.Json;
using System.Web.SessionState;

namespace App.Web.handlers.dashboard
{
    /// <summary>
    /// Summary description for getUnattachFiles
    /// </summary>
    public class getUnattachFiles : IHttpHandler,IRequiresSessionState
    {
        DashService ds = new DashService();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            int pageIndex = Convert.ToInt32(context.Request.Form["pageIndex"]);
            Model.User user = context.Session["user"] as Model.User;
            var r = ds.getUnattachFiles(pageIndex,user.ID.ToString());
            context.Response.Write(JsonConvert.SerializeObject(r));
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