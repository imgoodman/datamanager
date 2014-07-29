using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using App.Dll;
using App.Model;

namespace App.Web.InventoryMng
{
    public partial class List : System.Web.UI.Page
    {
        App.Dll.InventoryMethod.BusinessLayer bl = new App.Dll.InventoryMethod.BusinessLayer();
        App.Dll.InventoryMethod.DBInventoryTempMng DB1 = new App.Dll.InventoryMethod.DBInventoryTempMng();
        App.Dll.InventoryMethod.DBInventoryMng DB2 = new App.Dll.InventoryMethod.DBInventoryMng();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["orderByName"] = "ID";
                ViewState["orderByType"] = "desc";

                ViewState["issaved"] = null;
                string issaved = Request.Params["issaved"];
                if (!string.IsNullOrEmpty(issaved))
                {
                    ViewState["issaved"] = issaved;
                    lbTitle.InnerText = "清单对象列表";
                }

                Bind_myGridView();
            }
        }

        private void Bind_myGridView()
        {
            string orderByName = ViewState["orderByName"].ToString();
            string orderByType = ViewState["orderByType"].ToString();
            string sqlWhere = " IsExpired='false'";
            int recordCount = 0;
            if (ViewState["issaved"] == null)//临时清单列表
            {
                recordCount = DB1.GetRecordCount(sqlWhere);
                AspNetPager1.RecordCount = recordCount;
                myGridView.DataSource = DB1.GetPagedItems(sqlWhere, orderByName + " " + orderByType, AspNetPager1.CurrentPageIndex, AspNetPager1.PageSize);
                myGridView.DataBind();
            }
            else
            {
                recordCount = DB2.GetRecordCount(sqlWhere);
                AspNetPager1.RecordCount = recordCount;
                myGridView.DataSource = DB2.GetPagedItems(sqlWhere, orderByName + " " + orderByType, AspNetPager1.CurrentPageIndex, AspNetPager1.PageSize);
                myGridView.DataBind();
            }
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

                Inventory d = (Inventory)e.Row.DataItem;
                HyperLink hlName = (HyperLink)e.Row.FindControl("hlName");
                hlName.Text = d.Name;
                if (ViewState["issaved"] == null)//临时清单列表
                {
                    hlName.NavigateUrl = "Profile.aspx?rid=" + d.ID;
                }
                else
                {
                    hlName.NavigateUrl = "Profile.aspx?issaved=1&rid=" + d.ID;
                }
            }
        }
        protected void myGridView_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int rowIndex = -1;
            GridViewRow row = null;
            switch (e.CommandName)
            {
                case "ChooseEdit": // 模板列 

                    rowIndex = Convert.ToInt32(e.CommandArgument);
                    row = myGridView.Rows[rowIndex];
                    int dataID = Convert.ToInt32(myGridView.DataKeys[row.RowIndex].Value.ToString());
                    if (ViewState["issaved"] == null)//临时清单列表
                    {
                        Response.Redirect("Add.aspx?rid=" + dataID);
                    }
                    else
                    {
                        Response.Redirect("Add.aspx?issaved=1&rid=" + dataID);
                    }

                    break;
            }
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
                if (ViewState["issaved"] == null)//临时清单列表
                {
                    DB1.DeleteDt(dataIDAll);
                }
                else
                {
                    DB2.DeleteDt(dataIDAll);
                }
                Bind_myGridView();
                this.Page.RegisterStartupScript("alert", "<script>alert('删除成功!');</script>");
            }
            else
            {
                this.Page.RegisterStartupScript("alert", "<script>alert('没有选中任何项，请选择删除项!');</script>");
            }
        }

        protected void myGridView_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.setViewState(e.SortExpression, this.ViewState["orderByType"].ToString() == "asc" ? "desc" : "asc");
            Bind_myGridView();
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
            Bind_myGridView();
        }
    }
}