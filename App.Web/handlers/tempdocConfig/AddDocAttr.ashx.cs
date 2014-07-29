using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.Model;
using App.Dll;

namespace App.Web.handlers.tempdocConfig
{
    /// <summary>
    /// Summary description for AddDocAttr
    /// </summary>
    public class AddDocAttr : IHttpHandler
    {
        TempDocConfigService dcs = new TempDocConfigService();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            int docid = Convert.ToInt32(context.Request.Form["docID"]);
            string attrname = context.Request.Form["attrName"];
            string attrtype = context.Request.Form["attrType"];
            string desc = context.Request.Form["desc"];
            int userid = Convert.ToInt32(context.Request.Form["userid"]);
            bool isrequired = Convert.ToBoolean(context.Request.Form["isRequired"]);
            bool issearch = Convert.ToBoolean(context.Request.Form["isSearch"]);
            int order = Convert.ToInt32(context.Request.Form["vOrder"]);
            int id = dcs.AddDocAttr(new Document { ID = docid, Attrs = new List<DocumentAttr>() { new DocumentAttr { AttrName = attrname, AttrType = dcs.getAttrType(attrtype), Description = desc, Creator = new User { ID = userid }, IsRequired = isrequired, IsSearch = issearch, VerticalOrder = order, HorizontalOrder=0 } } });
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