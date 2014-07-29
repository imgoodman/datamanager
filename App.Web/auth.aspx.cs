using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace App.Web
{
    public partial class auth : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string userid = Request.Params["uid"];
            string rtnurl = Request.Params["rtnUrl"];
            string rattrid = Request.Params["rattrid"];
            Dll.SysMethod.DBUserManager bus = new Dll.SysMethod.DBUserManager();
            var u = bus.getUserByID(int.Parse(userid));
            Session["userid"] = u.ID;
            Session["username"] = u.Name;
            Session["isadmin"] = u.IsAdmin;
            if (string.IsNullOrEmpty(rtnurl))
                Response.Redirect("/default.aspx");
            else
                Response.Redirect(rtnurl + (string.IsNullOrEmpty(rattrid) ? "" : "&rattrid=" + rattrid));
        }
    }
}