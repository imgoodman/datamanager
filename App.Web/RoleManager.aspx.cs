using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace App.Web
{
    public partial class RoleManager : System.Web.UI.Page
    {
        App.Dll.SysMethod.DBRoleManager DB = new Dll.SysMethod.DBRoleManager();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["orderByName"] = "RoleName";//ViewState是一个比较复杂的应用，可以上网参考相关的介绍，这里用来记录gridview的用来排序字段信息
                ViewState["orderByType"] = "asc";
                Bind_GridView1();//调用为GridView1绑定数据的函数

            }
            plMsg.Visible = false;
        }
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            if (txtRoleName.Text.Trim() == "")
            {
                plMsg.Visible = true;
                lbMsg.Text = "<font color='red'>角色名称不能为空!</font>";
                return;
            }
            if (DB.IsExist(txtRoleName.Text.Trim()))
            {
                plMsg.Visible = true;
                lbMsg.Text = "<font color='red'>该角色名称已存在!</font>";
                return;
            }
            DB.InsertToDt(txtRoleName.Text);//如果没有错，则添加数据到角色表中
            Bind_GridView1();  //重新绑定gridview
            plMsg.Visible = true;
            lbMsg.Text = "<font color='red'>添加成功!</font>";
            return;
        }
        protected void btnReset_Click(object sender, EventArgs e)
        {
            //点击重置按钮时的事件
            Response.Redirect("RoleManager.aspx");//利用页面跳转函数，跳转到自身页面，即重新加载本页面，实现清空文本框的页面控件初始化的工作
        }
        protected void Bind_GridView1()
        {
            //绑定GridView的函数
            string orderByName = ViewState["orderByName"].ToString();
            string orderByType = ViewState["orderByType"].ToString();

            int recordCount = DB.GetRecordCount();//得到符合要求的数据的条数
            AspNetPager1.RecordCount = recordCount;//将得到的条数值赋给翻页空间AspNetPager1的记录总数属性

            DataTable dt = DB.GetPagedData(orderByName + " " + orderByType, AspNetPager1.CurrentPageIndex, AspNetPager1.PageSize);//调用数据处理函数，满足条件的数据，返回一个数据表
            GridView1.DataSource = dt;//将得到的数据表赋值为gridview的数据源
            GridView1.DataBind();//调用gridview的数据绑定函数，将数据表绑定到gridview

            if (recordCount == 0)//对数据表的数据数量进行判断，如果条数为0，则执行下列代码
            {
                btnDel.Visible = false;//让页面中的删除按钮不看见
                AspNetPager1.Visible = false;//让页面中的翻页控件不可见
                lbRemind.Visible = true;//让页面中的提示控件可见，即在页面显示“当前数据库中没有记录”
            }
            else//如果数据表的数据条数不为0，则执行下面的代码
            {
                btnDel.Visible = true;//让页面中的删除按钮可见
                AspNetPager1.Visible = true;//让页面中的翻页控件可见
                lbRemind.Visible = false;//让页面中的提示控件不可见
            }
        }
        protected void GridView1_Sorting(object sender, GridViewSortEventArgs e)
        {
            //gridview的排序事件，排序是比较深入的应用，可以忽略
            this.setViewState(e.SortExpression, this.ViewState["orderByType"].ToString() == "asc" ? "desc" : "asc");//调用设置ViewState函数，设置新的ViewState
            Bind_GridView1();//重新绑定gridview
        }
        private void setViewState(string orderByName, string orderByType)
        {
            //设置新ViewState的函数
            ViewState["orderByName"] = orderByName;
            ViewState["orderByType"] = orderByType;
        }
        protected void GridView1_RowCreated(object sender, GridViewRowEventArgs e)
        {
            //gridview数据行创建的时候触发的事件，主要是排序时的显示问题，也是比较深入的应用，可以忽略
            if (e.Row.RowIndex == -1)
            {
                for (int i = 0; i < GridView1.Columns.Count; i++)
                {
                    if (GridView1.Columns[i].SortExpression == ViewState["orderByName"].ToString())
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

        protected void btnDel_Click(object sender, EventArgs e)
        {
            //点击删除按钮时的事件
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
                DB.DeleteDt(dataIDAll);
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
        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int rowIndex = -1;
            GridViewRow row = null;
            switch (e.CommandName)
            {
                case "ChooseEdit":

                    rowIndex = Convert.ToInt32(e.CommandArgument);
                    row = GridView1.Rows[rowIndex];
                    string RoleID = GridView1.DataKeys[row.RowIndex].Value.ToString();
                    LabelTraID.Text = RoleID;
                    tbxRename.Text = row.Cells[1].Text;
                    PanelAdd.Visible = false;
                    PanelMod.Visible = true;
                    break;
            }
        }
        protected void btnMod_Click(object sender, EventArgs e)
        {
            int RoleID = Convert.ToInt32(LabelTraID.Text);
            if (DB.IsExist(tbxRename.Text, RoleID))
            {
                plMsg.Visible = true;
                lbMsg.Text = "<font color='red'>该角色名称已存在!</font>";
                return;
            }
            DB.UpdateDt(RoleID, tbxRename.Text);
            PanelAdd.Visible = true;
            PanelMod.Visible = false;
            Bind_GridView1();
            plMsg.Visible = true;
            lbMsg.Text = "<font color='red'>修改部门成功!</font>";
        }
        protected void btnBack_Click(object sender, EventArgs e)
        {
            PanelAdd.Visible = true;
            PanelMod.Visible = false;
        }
        protected void AspNetPager1_PageChanged(object sender, EventArgs e)
        {
            Bind_GridView1();
        }
    }
}