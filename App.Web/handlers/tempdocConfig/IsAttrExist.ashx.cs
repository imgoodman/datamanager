using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.Dll;

namespace App.Web.handlers.tempdocConfig
{
    /// <summary>
    /// Summary description for IsAttrExist
    /// </summary>
    public class IsAttrExist : IHttpHandler
    {
        TempDocConfigService dcs = new TempDocConfigService();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            int docid = Convert.ToInt32(context.Request.Form["docID"]);
            string name = context.Request.Form["attrName"];
            var r = dcs.IsAttrExist(new Model.Document { ID = docid, Attrs = new List<Model.DocumentAttr>() { new Model.DocumentAttr { AttrName = name } } });
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