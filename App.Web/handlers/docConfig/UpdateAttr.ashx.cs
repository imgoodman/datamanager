using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.Model;
using App.Dll;

namespace App.Web.handlers.docConfig
{
    /// <summary>
    /// Summary description for UpdateAttr
    /// </summary>
    public class UpdateAttr : IHttpHandler
    {
        DocConfigService dcs = new DocConfigService();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            int docID = Convert.ToInt32(context.Request.Form["docID"]);
            int attrid = Convert.ToInt32(context.Request.Form["attrID"]);
            string attrname = context.Request.Form["attrName"];
            string attrtype = context.Request.Form["attrType"];
            string desc = context.Request.Form["desc"];
            bool isrequired = Convert.ToBoolean(context.Request.Form["isRequired"]);
            bool isSearch = Convert.ToBoolean(context.Request.Form["isSearch"]);
            bool isRepeat = Convert.ToBoolean(context.Request.Form["isrepeat"]);
            int userid = Convert.ToInt32(context.Request.Form["userid"]);
            var r = dcs.UpdateAttr(new Document { ID = docID, Attrs = new List<DocumentAttr>() { new DocumentAttr { ID = attrid, AttrName = attrname, AttrType = dcs.getAttrType(attrtype), IsRequired = isrequired, IsSearch = isSearch, IsRepeat=isRepeat, Description = desc, LastModifier=new User{ID=userid} } } });
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