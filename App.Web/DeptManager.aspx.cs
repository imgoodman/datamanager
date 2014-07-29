using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace App.Web
{
    public partial class DeptManager : System.Web.UI.Page
    {
        App.Dll.SysMethod.DBDeptManager DB = new Dll.SysMethod.DBDeptManager();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Bind_ddlParentDept();
                Bind_GridView1();
            }
            plMsg.Visible = false;
        }
        protected void Bind_ddlParentDept()
        {
            ddlParentDept.Items.Clear();
            ddlParentDept.DataSource = DB.GetParentDeptDt();
            ddlParentDept.DataTextField = "DeptName";
            ddlParentDept.DataValueField = "ID";
            ddlParentDept.DataBind();
            ddlParentDept.Items.Insert(0, new ListItem("--请选择父级部门--", "0"));
        }
        protected void ddlParentDept_SelectedIndexChanged(object sender, EventArgs e)
        {
            Bind_GridView1();
        }
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            if (txtDeptName.Text == "")
            {
                plMsg.Visible = true;
                lbMsg.Text = "<font color='red'>部门名称不能为空!</font>";
                return;
            }
            if (DB.AlreadyExist(txtDeptName.Text.Trim(), ddlParentDept.SelectedValue) > 0)
            {
                plMsg.Visible = true;
                lbMsg.Text = "<font color='red'>该部门名称已存在!</font>";
                return;
            }
            DB.InsertToDt(txtDeptName.Text, ddlParentDept.SelectedValue);
            Bind_ddlParentDept();
            Bind_GridView1();
            plMsg.Visible = true;
            lbMsg.Text = "<font color='red'>添加部门成功!</font>";
        }
        protected void btnReset_Click(object sender, EventArgs e)
        {
            Response.Redirect("DeptManager.aspx");
        }
        protected void btnDel_Click(object sender, EventArgs e)
        {
            string deleIDs = "";
            for (int i = 0; i < GridView1.Rows.Count; i++)
            {
                CheckBox cb = (CheckBox)GridView1.Rows[i].FindControl("chkSelect");
                if (cb.Checked)
                {
                    deleIDs += GridView1.DataKeys[i].Value.ToString() + ",";
                    int count = DB.FatherIDCount(GridView1.DataKeys[i].Value.ToString());
                    if (count > 0)
                    {
                        int k = i + 1;
                        plMsg.Visible = true;
                        lbMsg.Text = "<font color='red'>第" + k.ToString() + "行部门包含子部门，请先删除子部门!</font>";
                        return;
                    }
                }
            }
            if (deleIDs != "")
            {
                deleIDs = deleIDs.Remove(deleIDs.Length - 1);
                DB.DeleteDt(deleIDs);
                Bind_GridView1();
                plMsg.Visible = true;
                lbMsg.Text = "<font color='red'>删除成功!</font>";
            }
            else
            {
                plMsg.Visible = true;
                lbMsg.Text = "<font color='red'>没有选中任何项，请选择删除项!</font>";
            }
        }
        protected void Bind_GridView1()
        {
            int recordCount = DB.GetRecordCount(ddlParentDept.SelectedValue);
            AspNetPager1.RecordCount = recordCount;
            GridView1.DataSource = DB.GetDt(ddlParentDept.SelectedValue);
            GridView1.DataBind();
            if (recordCount == 0)
            {
                btnDel.Visible = false;
                AspNetPager1.Visible = false;
                lbRemind.Visible = true;
            }
            else
            {
                btnDel.Visible = true;
                AspNetPager1.Visible = true;
                lbRemind.Visible = false;
            }
        }
        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int rowIndex = -1;
            GridViewRow row = null;
            switch (e.CommandName)
            {
                case "ChooseEdit":
                    rowIndex = Convert.ToInt32(e.CommandArgument);
                    row = GridView1.Rows[rowIndex];
                    string ID = GridView1.DataKeys[row.RowIndex].Value.ToString();
                    txtMdataID.Text = ID;
                    txtMDeptName.Text = DB.GetDeptName(ID);
                    //Bind_ddlMParentDept();
                    //ddlMParentDept.SelectedValue = DB.GetFatherDeptID(ID);
                    break;
            }
            Panel1.Visible = false;
            Panel2.Visible = true;
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
        protected void AspNetPager1_PageChanged(object sender, EventArgs e)
        {
            Bind_GridView1();
        }
        //protected void Bind_ddlMParentDept()
        //{
        //    ddlMParentDept.Items.Clear();
        //    ddlMParentDept.DataSource = DB.GetParentDeptDt();
        //    ddlMParentDept.DataTextField = "DeptName";
        //    ddlMParentDept.DataValueField = "ID";
        //    ddlMParentDept.DataBind();
        //    ddlMParentDept.Items.Insert(0, new ListItem("--请选择--", "0"));
        //}
        protected void btnModify_Click(object sender, EventArgs e)
        {
            if (txtMDeptName.Text == "")
            {
                plMsg.Visible = true;
                lbMsg.Text = "<font color='red'>部门名称不能为空!</font>";
                return;
            }
            else
            {
                int count = DB.AlreadyExist(txtMDeptName.Text.Trim(), txtMdataID.Text);
                if (count > 0)
                {
                    plMsg.Visible = true;
                    lbMsg.Text = "<font color='red'>该部门名称已存在!</font>";
                    return;
                }
            }
            DB.UpdateDeptManager(txtMDeptName.Text.Trim().ToString(), "0", txtMdataID.Text);
            plMsg.Visible = true;
            lbMsg.Text = "<font color='red'>修改部门成功!</font>";
            Bind_ddlParentDept();
            Bind_GridView1();
            Panel1.Visible = true;
            Panel2.Visible = false;
        }
        protected void btnReturn_Click(object sender, EventArgs e)
        {
            Panel1.Visible = true;
            Panel2.Visible = false;
        }
    }
}