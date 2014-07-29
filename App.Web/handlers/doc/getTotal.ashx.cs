using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.Dll;
using App.Model;

namespace App.Web.handlers.doc
{
    /// <summary>
    /// Summary description for getTotal
    /// </summary>
    public class getTotal : IHttpHandler
    {
        DocService ds = new DocService();
        DocConfigService dcs = new DocConfigService();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string attrids = context.Request.Form["attrIds"];
            string attrVals = context.Request.Form["attrVals"];
            int docid = Convert.ToInt32(context.Request.Form["docid"]);
            string[] idArray = attrids.Split('$');
            string[] valArray = attrVals.Split('$');
            int userid = 0;
            List<DocumentAttr> attrs = new List<DocumentAttr>();
            if (!string.IsNullOrEmpty(attrids))
            {
                for (int i = 0; i < idArray.Length; i++)
                {
                    if (idArray[i] != "0")
                    {
                        var attr = dcs.getAttrById(int.Parse(idArray[i]));
                        attr.Value = valArray[i].ToLower();
                        if (attr.AttrType == AttrType.Date)
                        {
                            string dFrom = valArray[i].Split('_')[0];
                            string dTo = valArray[i].Split('_')[1];
                            attr.Value = (dFrom == "NA" ? "" : dFrom);
                            attr.TranValue = (dTo == "NA" ? "" : dTo);
                        }
                        attrs.Add(attr);
                    }
                    else
                        userid = int.Parse(valArray[i]);
                }
            }
            var r = ds.getTotal(userid, attrs,docid);
            context.Response.Write(r);
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