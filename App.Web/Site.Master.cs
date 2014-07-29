using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace App.Web
{
    public partial class Site : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (object.Equals(Session["userid"], null))
            {
                var backUrl = Request.RawUrl;
                Response.Redirect("/logout.aspx?rtnUrl="+backUrl);
            }
            else
            {
                lbUsername.Text = Session["username"].ToString();
                userid.Value = Session["userid"].ToString();
                isadmin.Value = Session["isadmin"].ToString();
            }
        }
    }
}