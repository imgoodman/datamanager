﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.Dll;
using Newtonsoft.Json;

namespace App.Web.handlers.tempdocConfig
{
    /// <summary>
    /// Summary description for getAttrById
    /// </summary>
    public class getAttrById : IHttpHandler
    {
        TempDocConfigService dcs = new TempDocConfigService();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            int id = Convert.ToInt32(context.Request.Form["ID"]);
            var r = dcs.getAttrById(id);
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