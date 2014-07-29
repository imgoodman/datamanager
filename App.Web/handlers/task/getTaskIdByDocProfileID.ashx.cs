using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.Dll;

namespace App.Web.handlers.task
{
    /// <summary>
    /// Summary description for getTaskIdByDocProfileID
    /// </summary>
    public class getTaskIdByDocProfileID : IHttpHandler
    {
        DocService ds = new DocService();
        TaskService ts = new TaskService();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            int docinstanceid = Convert.ToInt32(context.Request.Form["id"].ToString());
            var r = ds.getDocInstanceById(docinstanceid);
            string url = "Doc/List.aspx?ID=" + r.Document.ID;
            var tid = ts.getTaskIDByUrl(url);
            context.Response.Write(tid + "_" + r.Document.ID + "_" + r.Document.DocName);
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