using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace App.Web.Doc
{
    public partial class DocInstanceImport : System.Web.UI.Page
    {
        App.Dll.DocConfigService DBDoc = new Dll.DocConfigService();
        App.Dll.DocMethod.DocInstanceImport DB = new Dll.DocMethod.DocInstanceImport();

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
            FU.Save();
            if (!string.IsNullOrEmpty(FU.FileName))
            {
                lbFilePath.Text = FU.ServerFilePath;
                pl1.Visible = false;
                pl2.Visible = true;
                Init_pn2();
            }
            else
            {
                if (!string.IsNullOrEmpty(lbFilePath.Text))
                {
                    pl1.Visible = false;
                    pl2.Visible = true;
                    Init_pn2();
                }
                else
                {
                    plMsg.Visible = true;
                    lbMsg.Text = "<font color='red'>请选择导入文件!</font>";
                }
            }
        }
        private void Init_pn2()
        {
            if (DB.CheckTemplate(lbFilePath.Text, int.Parse(ddlDoc.SelectedValue)))
                btnNext2.Enabled = true;
            else
                btnNext2.Enabled = false;                
            Bind_GridView1();
        }
        protected void Bind_GridView1()
        {
            GridView1.DataSource = DB.getDocAttrColNameDt(lbFilePath.Text, int.Parse(ddlDoc.SelectedValue));
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
        protected void btnNext2_Click(object sender, EventArgs e)
        {
            CreategvData();
            pl2.Visible = false;
            pl3.Visible = true;
        }
        protected void btnReturn2_Click(object sender, EventArgs e)
        {
            pl1.Visible = true;
            pl2.Visible = false;
        }
        private void CreategvData()
        {
            DataTable dt = DB.DisplayDocInstanceFromFile(lbFilePath.Text, int.Parse(ddlDoc.SelectedValue));
            GridView gv = gvData;
            gv.DataSource = dt;
            gv.Columns.Clear();
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                BoundField bc = new BoundField();
                if (i == 0)
                    bc.HeaderText = "行号";
                else
                {
                    if (i == dt.Columns.Count - 1)
                        bc.HeaderText = "数据校对";
                    else
                        bc.HeaderText = dt.Columns[i].Caption;
                }
                bc.DataField = dt.Columns[i].ToString();
                bc.HtmlEncode = false;
                gv.Columns.Add(bc);
            }
            gv.DataBind();
        }
        protected void gvData_RowDataBound(object sender, GridViewRowEventArgs e)
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
            string msg = DB.ImportDocInstanceFromFile(lbFilePath.Text, int.Parse(ddlDoc.SelectedValue));
            plMsg.Visible = true;
            lbMsg.Text = "<font color='red'>" + msg + "!</font>";

            FU.Initialize();
            lbFilePath.Text = "";
            pl1.Visible = true;
            pl3.Visible = false;
        }
        protected void btnReturn3_Click(object sender, EventArgs e)
        {
            pl1.Visible = true;
            pl3.Visible = false;
        }


    }
}