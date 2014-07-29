using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace App.Web
{
    public partial class DocumentType : System.Web.UI.Page
    {
        App.Dll.SysMethod.DBDocTypeManager DB = new Dll.SysMethod.DBDocTypeManager();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                lb_deptid.Text = "";
                Bind_GridView1();
            }
            plMsg.Visible = false;
        }
        protected void TextChanged1(object sender, EventArgs e)
        {
            lb_deptid.Text = category1.SelectedID;
            Bind_GridView1();
        }
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            string pid = string.Empty;
            if (lb_deptid.Text.Trim() == "")
                pid = "0";
            else
                pid = lb_deptid.Text.Trim();
            if (txtDeptName.Text.Trim() == "")
            {
                plMsg.Visible = true;
                lbMsg.Text = "<font color='red'>类型名称不能为空!</font>";
                return;
            }
            if (DB.AlreadyExist(txtDeptName.Text.Trim(), pid) > 0)
            {
                plMsg.Visible = true;
                lbMsg.Text = "<font color='red'>该类型名称已存在!</font>";
                return;
            }
            DB.InsertToDt(txtDeptName.Text, pid);
            Bind_GridView1();
            plMsg.Visible = true;
            lbMsg.Text = "<font color='red'>添加成功!</font>";
        }
        protected void btnReset_Click(object sender, EventArgs e)
        {
            Response.Redirect("DocumentType.aspx");
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
                        lbMsg.Text = "<font color='red'>第" + k.ToString() + "行类型包含子类型，请先删除子类型!</font>";
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
            string pid = string.Empty;
            if (lb_deptid.Text.Trim() == "")
                pid = "0";
            else
                pid = lb_deptid.Text.Trim();
            int recordCount = DB.GetRecordCount(pid);
            AspNetPager1.RecordCount = recordCount;
            GridView1.DataSource = DB.GetDt(pid);
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
                    lb_deptid2.Text = category2.SelectedID = DB.GetFatherDeptID(ID);
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
        protected void TextChanged2(object sender, EventArgs e)
        {
            lb_deptid2.Text = category2.SelectedID;
        }
        protected void btnModify_Click(object sender, EventArgs e)
        {
            string pid = string.Empty;
            if (lb_deptid2.Text.Trim() == "")
                pid = "0";
            else
                pid = lb_deptid2.Text.Trim();
            if (txtMDeptName.Text == "")
            {
                plMsg.Visible = true;
                lbMsg.Text = "<font color='red'>文档类型名称不能为空!</font>";
                return;
            }
            string childids = DB.GetChildTypeIDs(int.Parse(txtMdataID.Text));
            if (!string.IsNullOrEmpty(childids))
            {
                childids = childids.Remove(childids.Length - 1);
                string[] cid = childids.Split(',');
                for (int i = 0; i < cid.Length; i++)
                {
                    if (cid[i] == pid)
                    {
                        plMsg.Visible = true;
                        lbMsg.Text = "<font color='red'>当前文档类型存在子类!</font>";
                        return;
                    }
                }
            }
            int count = DB.AlreadyExist(txtMDeptName.Text.Trim(), pid, txtMdataID.Text);
            if (count > 0)
            {
                plMsg.Visible = true;
                lbMsg.Text = "<font color='red'>该文档类型名称已存在!</font>";
                return;
            }
            DB.UpdateTypeManager(txtMDeptName.Text.Trim().ToString(), pid, txtMdataID.Text);
            plMsg.Visible = true;
            lbMsg.Text = "<font color='red'>修改文档类型成功!</font>";
            Bind_GridView1();
            Panel1.Visible = true;
            Panel2.Visible = false;
        }
        protected void btnReturn_Click(object sender, EventArgs e)
        {
            Panel1.Visible = true;
            Panel2.Visible = false;
        }
        protected void btnSetRoot_Click(object sender, EventArgs e)
        {
            category2.SelectedName = "--根节点--";
            lb_deptid2.Text = "";
        }
    }
}