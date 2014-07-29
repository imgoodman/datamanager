using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace App.Web
{
    public partial class UserManager : System.Web.UI.Page
    {
        App.Dll.SysMethod.DBUserManager DB = new App.Dll.SysMethod.DBUserManager();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ViewState["orderByName"] = "ID";
                ViewState["orderByType"] = "desc";
                lb_deptid.Text = "";
                RoleDataBind();
                MRolelDataBind();
                UserDataBind();
            }
            plMsg.Visible = false;
        }

        private void RoleDataBind()
        {
            ddlRole.DataSource = DB.GetRoleNames();
            ddlRole.DataTextField = "RoleName";
            ddlRole.DataValueField = "ID";
            ddlRole.DataBind();
            ddlRole.Items.Insert(0, new ListItem("--请选择--", "0"));
        }

        private void MRolelDataBind()
        {
            ddlMRole.DataSource = DB.GetRoleNames();
            ddlMRole.DataTextField = "RoleName";
            ddlMRole.DataValueField = "ID";
            ddlMRole.DataBind();
        }

        private void UserDataBind()
        {
            string orderByName = ViewState["orderByName"].ToString();
            string orderByType = ViewState["orderByType"].ToString();
            int recordCount = DB.GetRecordCount();
            AspNetPager1.RecordCount = recordCount;
            DataTable dt = DB.GetPagedData(orderByName + " " + orderByType, AspNetPager1.CurrentPageIndex, AspNetPager1.PageSize);
            myGridView.DataSource = dt;
            myGridView.DataBind();
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

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            string username = txtUserName.Text.ToString();
            string name = txtName.Text.ToString();
            string roleID = ddlRole.SelectedValue;
            if (string.IsNullOrEmpty(username))
            {
                plMsg.Visible = true;
                lbMsg.Text = "<font color='red'>用户名不能为空!</font>";
                return;
            }
            if (string.IsNullOrEmpty(name))
            {
                plMsg.Visible = true;
                lbMsg.Text = "<font color='red'>真实姓名不能为空!</font>";
                return;
            }
            if (lb_deptid.Text == "")
            {
                plMsg.Visible = true;
                lbMsg.Text = "<font color='red'>请选择部门!</font>";
                return;
            }
            if (roleID == "0")
            {
                plMsg.Visible = true;
                lbMsg.Text = "<font color='red'>请选择角色!</font>";
                return;
            }
            if (DB.IsExist(username))
            {
                plMsg.Visible = true;
                lbMsg.Text = "<font color='red'>当前用户名已存在!</font>";
                return;
            }
            //int state = 1;
            //string strMsg = DB.GetValidateStr(username, name, ddGender.SelectedItem.Text, role, lb_deptid.Text);
            //if (strMsg != "")
            //{
            //    SU.DisplayErrorMsg(this.Page, strMsg);
            //    return;
            //}
            //else
            //{
            //int sgender;
            //if (ddGender.SelectedItem.Text.ToString() == "男") { sgender = 1; } else { sgender = -1; }
            string encodePwd = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile("111", "MD5").ToLower().Substring(8, 16);
            //string email = "";
            //if (TextBox2.Text.Trim() != "" && (RadioButtonList1.Items[0].Selected == true || RadioButtonList1.Items[1].Selected == true))
            //{
            //    email = TextBox2.Text + RadioButtonList1.SelectedItem.Value.ToString();
            //}
            DB.AddUser(username, name, roleID, 1, lb_deptid.Text, 1, encodePwd, "", "", "");
            plMsg.Visible = true;
            lbMsg.Text = "<font color='red'>添加用户成功!</font>";
            UserDataBind();
            //}
        }

        //单击重置按钮事件
        protected void btnReset_Click(object sender, EventArgs e)
        {
            Response.Redirect("UserManager.aspx");
        }

        protected void myGridView_RowDataBound(object sender, GridViewRowEventArgs e)
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
        protected void myGridView_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int rowIndex = -1;
            GridViewRow row = null;
            switch (e.CommandName)
            {
                case "ChooseEdit": // 模板列 

                    lb_deptid2.Text = "";
                    rowIndex = Convert.ToInt32(e.CommandArgument);
                    row = myGridView.Rows[rowIndex];
                    int dataID = Convert.ToInt32(myGridView.DataKeys[row.RowIndex].Value.ToString());
                    txtMdataID.Text = Convert.ToString(dataID);
                    MtxtUserName.Text = row.Cells[1].Text;
                    MtxtName.Text = row.Cells[2].Text;
                    ////MddGender.SelectedValue = row.Cells[3].Text;
                    category2.SelectedName = row.Cells[3].Text;
                    lb_deptid2.Text = DB.GetDeptID(dataID).ToString();
                    category2.SelectedID = lb_deptid2.Text.ToString();
                    ddlMRole.SelectedItem.Text = row.Cells[4].Text;
                    ddlMRole.SelectedValue = DB.GetRoleID(dataID).ToString();
                    //string mrole = row.Cells[4].Text.ToString();
                    //string[] pp = mrole.Split(',');
                    //for (int i = 0; i < pp.Length; i++)
                    //{
                    //    foreach (ListItem item in MlistRole.Items)
                    //    {
                    //        if (pp[i] == item.Text.ToString())
                    //        {
                    //            item.Selected = true;
                    //        }
                    //    }
                    //}
                    //foreach (ListItem ritem in RadioButtonList2.Items)
                    //{
                    //    ritem.Selected = false;
                    //}
                    //if (row.Cells[6].Text == "" || row.Cells[6].Text == "&nbsp;")
                    //{
                    //    TextBox5.Text = "";
                    //}
                    //else
                    //{
                    //    string email = row.Cells[6].Text;
                    //    string[] ppp = email.Split('@');
                    //    TextBox5.Text = ppp[0].ToString();
                    //    RadioButtonList2.SelectedValue = "@" + ppp[1].ToString();
                    //}
                    //if (row.Cells[7].Text == "" || row.Cells[7].Text == "&nbsp;")
                    //{
                    //    TextBox6.Text = "";
                    //}
                    //else
                    //{
                    //    TextBox6.Text = row.Cells[7].Text;
                    //}
                    //if (row.Cells[8].Text == "" || row.Cells[8].Text == "&nbsp;")
                    //{
                    //    TextBox7.Text = "";
                    //}
                    //else
                    //{
                    //    TextBox7.Text = row.Cells[8].Text;
                    //}
                    Panel1.Visible = false;
                    Panel2.Visible = true;

                    break;
            }
        }
        protected void btnModify_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(txtMdataID.Text);
            //string Mrole = "";
            //foreach (ListItem item in MlistRole.Items)
            //{
            //    if (item.Selected)
            //    {
            //        Mrole += item.Value.ToString() + ",";
            //    }
            //}
            //string strMsg = DB.GetMValidateStr(MtxtUserName.Text, MtxtName.Text, Mrole, MddGender.SelectedValue.ToString(), lb_deptid.Text, id);
            //if (strMsg != "")
            //{
            //    SU.DisplayErrorMsg(this.Page, strMsg);
            //    return;
            //}
            //else
            //{
            //    int sgender;
            //    if (MddGender.SelectedItem.Text == "男") { sgender = 1; } else { sgender = -1; }
            //    Mrole = Mrole.Remove(Mrole.Length - 1);
            //    string email = "";
            //    if (TextBox5.Text.Trim() != "" && (RadioButtonList2.Items[0].Selected == true || RadioButtonList2.Items[1].Selected == true))
            //    {
            //        email = TextBox5.Text + RadioButtonList2.SelectedItem.Value.ToString();
            //    }
            if (MtxtUserName.Text.Trim() == "")
            {
                plMsg.Visible = true;
                lbMsg.Text = "<font color='red'>用户名不能为空!</font>";
                return;
            }
            if (MtxtName.Text == "")
            {
                plMsg.Visible = true;
                lbMsg.Text = "<font color='red'>真实姓名不能为空!</font>";
                return;
            }
            if (DB.IsMExist(MtxtUserName.Text, id))
            {
                plMsg.Visible = true;
                lbMsg.Text = "<font color='red'>当前用户名已存在!</font>";
                return;
            }
            DB.UpdateUser(id, MtxtUserName.Text, MtxtName.Text, ddlMRole.SelectedValue, 1, lb_deptid2.Text, "", "", "");
            plMsg.Visible = true;
            lbMsg.Text = "<font color='red'>修改用户信息成功!</font>";
            UserDataBind();
            Panel1.Visible = true;
            Panel2.Visible = false;
            //}
        }
        protected void btnReturn_Click(object sender, EventArgs e)
        {
            Panel1.Visible = true;
            Panel2.Visible = false;
        }
        protected void btnDel_Click(object sender, EventArgs e)
        {
            string dataIDAll = "";
            for (int i = 0; i < myGridView.Rows.Count; i++)
            {
                CheckBox cb = (CheckBox)myGridView.Rows[i].FindControl("chkSelect");
                if (cb.Checked)
                {
                    dataIDAll += myGridView.DataKeys[i].Value.ToString() + ",";
                }
            }
            if (dataIDAll != "")
            {
                dataIDAll = dataIDAll.Remove(dataIDAll.Length - 1);
                DB.DeleteDt(dataIDAll);
                UserDataBind();
                plMsg.Visible = true;
                lbMsg.Text = "<font color='red'>删除成功!</font>";
            }
            else
            {
                plMsg.Visible = true;
                lbMsg.Text = "<font color='red'>没有选中任何项，请选择删除项!</font>";
            }
        }

        protected void myGridView_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.setViewState(e.SortExpression, this.ViewState["orderByType"].ToString() == "asc" ? "desc" : "asc");
            UserDataBind();
        }
        private void setViewState(string orderByName, string orderByType)
        {
            ViewState["orderByName"] = orderByName;
            ViewState["orderByType"] = orderByType;
        }
        protected void myGridView_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowIndex == -1)
            {
                for (int i = 0; i < myGridView.Columns.Count; i++)
                {
                    if (myGridView.Columns[i].SortExpression == ViewState["orderByName"].ToString())
                    {
                        try
                        {
                            TableCell tableCell = e.Row.Cells[i];
                            Label lblSorted = new Label();
                            lblSorted.Font.Name = "webdings";
                            lblSorted.Width = 20;
                            lblSorted.Text = (ViewState["orderByType"].ToString() == "asc" ? "5" : "6");
                            tableCell.Controls.Add(lblSorted);
                        }
                        catch { }
                    }
                }
            }
        }
        protected void AspNetPager1_PageChanged(object sender, EventArgs e)
        {
            UserDataBind();
        }
        protected void TextChanged1(object sender, EventArgs e)
        {
            lb_deptid.Text = category1.SelectedID;
        }
        protected void TextChanged2(object sender, EventArgs e)
        {
            lb_deptid2.Text = category2.SelectedID;
        }
        protected void lbtnResetPwd_Command(object sender, CommandEventArgs e)
        {
            string id = e.CommandArgument.ToString();
            string encodePwd = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile("111", "MD5").ToLower().Substring(8, 16);
            DB.ResetPwd(id, encodePwd);
            plMsg.Visible = true;
            lbMsg.Text = "<font color='red'>重置密码成功!</font>";
        }
    }
}