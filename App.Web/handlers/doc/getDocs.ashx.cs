using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.Dll;
using Newtonsoft.Json;

namespace App.Web.handlers.doc
{
    /// <summary>
    /// Summary description for getDocs
    /// </summary>
    public class getDocs : IHttpHandler
    {
        DocService ds = new DocService();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            int pageIndex = Convert.ToInt32(context.Request.Form["pageIndex"]);
            int docid = Convert.ToInt32(context.Request.Form["docID"]);
            int userid = int.Parse(context.Request.Form["userid"]);
            var r = ds.getDocInstances(pageIndex, docid,userid);
            if (!object.Equals(r, null))
            {
                int attrCount = MyExtension.ToInt(MyExtension.getAppValue("DisplayDocAttrCount"));
                if (r.First().Document.Attrs.Count > attrCount)
                {
                    foreach (var item in r)
                    {
                        item.Document.Attrs.RemoveRange(attrCount, item.Document.Attrs.Count - attrCount);
                    }
                }
            }
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