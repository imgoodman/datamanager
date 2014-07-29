using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace App.Web.Doc
{
    public partial class DocRedundantCheck : System.Web.UI.Page
    {
        App.Dll.DocConfigService DBDoc = new Dll.DocConfigService();
        App.Dll.DocMethod.DocRedundantCheck DBDocRe = new Dll.DocMethod.DocRedundantCheck();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                lbDocTypeID.Text = DocTypeSelect.SelectedID = "0";
                Bind_ddlDoc();
            }
            plMsg.Visible = false;
        }

        protected void DocType_Selected(object sender, EventArgs e)
        {
            lbDocTypeID.Text = DocTypeSelect.SelectedID;
            Bind_ddlDoc();
        }
        protected void Bind_ddlDoc()
        {
            ddlDoc.Items.Clear();
            ddlDoc.DataSource = DBDoc.getDocsByTypeId(int.Parse(lbDocTypeID.Text), "");
            ddlDoc.DataTextField = "DocName";
            ddlDoc.DataValueField = "ID";
            ddlDoc.DataBind();
            ddlDoc.Items.Insert(0, new ListItem("--请选择--", "0"));
        }
        protected void btnNext_Click(object sender, EventArgs e)
        {
            if (ddlDoc.SelectedValue == "0")
            {
                plMsg.Visible = true;
                lbMsg.Text = "<font color='red'>请选择文档对象!</font>";
                return;
            }
            pl1.Visible = false;
            pl2.Visible = true;
            Bind_GridView1();
        }

        protected void btnReturn_Click(object sender, EventArgs e)
        {
            pl1.Visible = true;
            pl2.Visible = false;
        }
        protected void Bind_GridView1()
        {
            GridView1.DataSource = DBDocRe.GetDocRedundantDt(int.Parse(ddlDoc.SelectedValue));
            GridView1.DataBind();
        }
        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            foreach (TableCell tc in e.Row.Cells)
            {
                tc.Attributes["class"] = "gvTd";
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["onmouseover"] = "javascript:c=this.style.backgroundColor;this.style.background='" + "#C8DDF3" + "'";
                e.Row.Attributes["onmouseout"] = "javascript:this.style.background=c";
            }
        }
        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            string dataIDAll = "";
            for (int i = 0; i < GridView1.Rows.Count; i++)
            {
                CheckBox cb = (CheckBox)GridView1.Rows[i].FindControl("chkSelect");
                if (cb.Checked)
                {
                    dataIDAll += GridView1.DataKeys[i].Value.ToString() + ",";
                }
            }
            if (dataIDAll != "")
            {
                dataIDAll = dataIDAll.Remove(dataIDAll.Length - 1);
                string msg = DBDocRe.DeleteDocInstanceRedu(dataIDAll, int.Parse(Session["UserID"].ToString()));
                plMsg.Visible = true;
                lbMsg.Text = "<font color='red'>" + msg + "!</font>";
                Bind_GridView1();
            }
            else
            {
                plMsg.Visible = true;
                lbMsg.Text = "<font color='red'>没有选中任何项，请选择删除项!</font>";
            }
        }

    }
}