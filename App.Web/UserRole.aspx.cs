using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace App.Web
{
    public partial class UserRole : System.Web.UI.Page
    {
        App.Dll.BaseClass bc = new Dll.BaseClass();
        App.Dll.SysMethod.DBRoleManager DB = new Dll.SysMethod.DBRoleManager();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["orderByName"] = "RoleName";
                ViewState["orderByType"] = "asc";
                Bind_GridView1();
            }
            TreeView1.Attributes.Add("onclick", "OnTreeNodeChecked()");
        }

        protected void Bind_GridView1()
        {
            string orderByName = ViewState["orderByName"].ToString();
            string orderByType = ViewState["orderByType"].ToString();
            int recordCount = DB.GetRecordCount();
            AspNetPager1.RecordCount = recordCount;
            myGridView.DataSource = DB.GetPagedData(orderByName + " " + orderByType, AspNetPager1.CurrentPageIndex, AspNetPager1.PageSize);
            myGridView.DataBind();
            if (recordCount == 0)
            {
                AspNetPager1.Visible = false;
            }
            else
            {
                AspNetPager1.Visible = true;
            }
        }
        protected void myGridView_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.setViewState(e.SortExpression, this.ViewState["orderByType"].ToString() == "asc" ? "desc" : "asc");
            Bind_GridView1();
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
        protected void AspNetPager1_PageChanged(object sender, EventArgs e)
        {
            Bind_GridView1();
        }
        protected void myGridView_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            int rowIndex = -1;
            GridViewRow row = null;
            switch (e.CommandName)
            {
                case "ChooseEdit": // 权限列 

                    txtdataTask.Text = "";
                    rowIndex = Convert.ToInt32(e.CommandArgument);
                    row = myGridView.Rows[rowIndex];
                    int roleID = Convert.ToInt32(myGridView.DataKeys[row.RowIndex].Value.ToString());
                    txtMdataID.Text = Convert.ToString(roleID);
                    Panel1.Visible = false;
                    Panel2.Visible = true;
                    label1.Text = bc.ecScalar("select RoleName from RoleManage_Tab where ID='" + roleID + "'");
                    BindTaskTreeView(TreeView1, true, "0");
                    string checktask = bc.ecScalar("Select TaskList from RoleManage_Tab where ID='" + roleID + "'");
                    txtdataTask.Text = checktask;
                    foreach (TreeNode node in TreeView1.Nodes)
                    {
                        MakeNodeChecked(node, txtdataTask.Text);
                    }    
                    break;
            }
        }
        public void BindTaskTreeView(TreeView treeView, bool isExpanded, string sSelectedData)
        {
            DataTable dataTable = DB.GetTasks().Tables[0];
            treeView.Nodes.Clear();
            DataRow[] rowlist = dataTable.Select("ParentID='0'");
            int roleID = Convert.ToInt32(txtMdataID.Text.ToString());
            if (rowlist.Length <= 0)
            {
                return;
            }
            for (int i = 0; i < rowlist.Length; i++)
            {
                TreeNode rootNode = new TreeNode();
                rootNode.Text = rowlist[i]["TaskName"].ToString();
                rootNode.Value = rowlist[i]["ID"].ToString();
                rootNode.Expanded = isExpanded;
                rootNode.SelectAction = TreeNodeSelectAction.Expand;
                rootNode.Selected = false;
                treeView.Nodes.Add(rootNode);
                CreateChildNode(rootNode, dataTable, isExpanded, sSelectedData);
            }
        }
        private void CreateChildNode(TreeNode parentNode, DataTable dataTable, bool isExpanded, string sSelectedData)
        {
            DataRow[] rowlist = dataTable.Select("ParentID='" + parentNode.Value + "'");
            int roleID = Convert.ToInt32(txtMdataID.Text.ToString());
            foreach (DataRow row in rowlist)
            {
                TreeNode node = new TreeNode();
                node.Text = row["TaskName"].ToString();
                node.Value = row["ID"].ToString();

                node.Expanded = isExpanded;
                node.SelectAction = TreeNodeSelectAction.Expand;
                if (node.Value == sSelectedData)
                {
                    node.Selected = true;
                }
                parentNode.ChildNodes.Add(node);
                CreateChildNode(node, dataTable, isExpanded, sSelectedData);
            }
        }
        private void MakeNodeChecked(TreeNode node, string checktask)
        {
            //string checktask = txtdataTask.Text.ToString();        
            string[] pp = checktask.Split(',');
            for (int i = 0; i < pp.Length; i++)
            {
                if (pp[i] == node.Value.ToString())
                {
                    node.Checked = true;
                    break;
                }
            }
            foreach (TreeNode tn in node.ChildNodes)
            {
                MakeNodeChecked(tn, checktask);
            }
        }

        protected void resetNodeState()
        {
            foreach (TreeNode node in this.TreeView1.Nodes)
            {
                if (node.ChildNodes.Count > 0)
                {
                    node.Checked = GetChildNodeChecked(node);
                }
            }
        }
        protected bool GetChildNodeChecked(TreeNode ParentNode)
        {
            bool checkflag = false;
            foreach (TreeNode node in ParentNode.ChildNodes)
            {
                if (node.ChildNodes.Count > 0)
                {
                    node.Checked = GetChildNodeChecked(node);
                }
                if (node.Checked)
                {
                    checkflag = true;
                }
            }
            return checkflag;
        }

        protected void clickadd(object sender, EventArgs e)
        {
            //重置有子节点的节点选中状态
            resetNodeState();
            txtdataTask.Text = "";
            int roleID = Convert.ToInt32(txtMdataID.Text.ToString());
            string tasklist = "";
            DB.UpdateRole(tasklist, roleID);
            //string sqltxtnull = "update RoleManage_Tab set TaskList='" + tasklist + "' where ID='" + roleID + "'";
            //bc.RunSqlTransaction(sqltxtnull);
            foreach (TreeNode node in this.TreeView1.Nodes)
            {
                AddCheckedNode(node);
            }
            tasklist = txtdataTask.Text.ToString();
            if (tasklist != "")
            {
                tasklist = tasklist.Remove(tasklist.Length - 1);
                //string sqltxtins = "update RoleManage_Tab set TaskList='" + tasklist + "' where ID='" + roleID + "'";
                //bc.RunSqlTransaction(sqltxtins);
            }
            DB.UpdateRole(tasklist, roleID);
            this.Page.RegisterStartupScript("alert", "<script>alert('角色权限配置成功!');</script>");
            return;
        }
        private void AddCheckedNode(TreeNode node)
        {

            if (node.Checked == true)
            {
                txtdataTask.Text = txtdataTask.Text.ToString() + node.Value.ToString() + ",";
                foreach (TreeNode tn in node.ChildNodes)
                {
                    AddCheckedNode(tn);
                }
            }
        }
        protected void clickreturn(object sender, EventArgs e)
        {
            Panel1.Visible = true;
            Panel2.Visible = false;
        }
    }
}