using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace App.Web.util
{
    public partial class correctCreators : System.Web.UI.Page
    {
        App.Dll.DocService ds = new Dll.DocService();
        protected void Page_Load(object sender, EventArgs e)
        {
            Label1.Text = ds.getBadCreators().ToString();

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            ds.correctCreator();
        }
    }
}