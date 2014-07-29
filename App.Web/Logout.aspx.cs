using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace App.Web
{
    public partial class Logout : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Session.Abandon();
            string rtnUrl = Request.Params["rtnUrl"];
            string rattid = Request.Params["rattrid"];
            Response.Redirect("/login.aspx" + (string.IsNullOrEmpty(rtnUrl) ? "" : "?rtnUrl=" + rtnUrl) + (string.IsNullOrEmpty(rattid) ? "" : "&rattrid=" + rattid));
        }
    }
}