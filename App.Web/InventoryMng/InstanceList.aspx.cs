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
    public partial class InstanceList : System.Web.UI.Page
    {
        App.Dll.InventoryMethod.BusinessLayer bl = new App.Dll.InventoryMethod.BusinessLayer();
        App.Dll.InventoryMethod.DBInventoryMng im = new App.Dll.InventoryMethod.DBInventoryMng();
        App.Dll.InventoryMethod.DBInventoryInstanceMng DB = new Dll.InventoryMethod.DBInventoryInstanceMng();
        App.Dll.DocConfigService dcs = new DocConfigService();
        App.Dll.DocService ds = new DocService();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["orderByName"] = "ID";
                ViewState["orderByType"] = "desc";

                bl.Bind_ddlInventoryName(ddlInventory);
                ddlInventory.Items.Insert(0, new ListItem(" ---请选择清单对象---", "0"));
                Bind_myGridView();
            }
        }

        private void Bind_myGridView()
        {
            string orderByName = ViewState["orderByName"].ToString();
            string orderByType = ViewState["orderByType"].ToString();
            string sqlWhere = " IsExpired='false'";
            if (ddlInventory.SelectedIndex != 0)
            {
                sqlWhere += " and InventoryID='" + ddlInventory.SelectedValue + "'";
            }
            int recordCount = 0;
            recordCount = DB.GetRecordCount(sqlWhere);
            AspNetPager1.RecordCount = recordCount;
            List<InventoryInstance> data = DB.GetPagedItems(sqlWhere, orderByName + " " + orderByType, AspNetPager1.CurrentPageIndex, AspNetPager1.PageSize);
            if (data != null)
            {
                foreach (InventoryInstance d in data)
                {
                    string showNames = GetInventoryInstanceShowNamesByID(d);
                    d.InstanceDocs.First().DocInstanceIDs = showNames;
                }
            }
            myGridView.DataSource = data;
            //myGridView.DataSource = DB.GetPagedItems(sqlWhere, orderByName + " " + orderByType, AspNetPager1.CurrentPageIndex, AspNetPager1.PageSize);
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

        protected string GetInventoryInstanceShowNamesByID(InventoryInstance iInstance)
        {
            string showNames = null;

            Inventory ivtr = im.GetInventoryByID(iInstance.InventoryID);
            for (int i = 0; i < iInstance.InstanceDocs.Count; i++)
            {
                if (iInstance.InstanceDocs[i] != null)
                {
                    Document doc = new Document();
                    if (ivtr.Docs.Where(p => p.ID == iInstance.InstanceDocs[i].DocID).Count() > 0)
                    {
                        doc = ivtr.Docs.Where(p => p.ID == iInstance.InstanceDocs[i].DocID).First();//一类文档
                    }
                    List<DocumentInstance> inslist = bl.GetDocumentInstancesByIDs(iInstance.InstanceDocs[i].DocInstanceIDs);
                    if (doc.Attrs != null)
                    {
                        if (inslist != null)
                        {
                            foreach (var docins in inslist)
                            {
                                string attrName = null;//一行属性值
                                foreach (var attr in doc.Attrs)
                                {

                                    if (docins.Document.Attrs.Where(p => p.ID == attr.ID).Count() > 0)
                                    {
                                        DocumentAttr d = docins.Document.Attrs.Where(p => p.ID == attr.ID).First();
                                        attrName += d.AttrName + "：";
                                        attrName += d.TranValue + "&nbsp;&nbsp;&nbsp;";
                                    }
                                }
                                showNames += attrName + "<br />";
                            }
                        }
                    }
                }
            }
            return showNames;
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

                    rowIndex = Convert.ToInt32(e.CommandArgument);
                    row = myGridView.Rows[rowIndex];
                    int dataID = Convert.ToInt32(myGridView.DataKeys[row.RowIndex].Value.ToString());
                    Response.Redirect("InstanceAdd.aspx?rid=" + dataID);

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
                DB.DeleteDt(dataIDAll);
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

        protected void ddlInventory_SelectedIndexChanged(object sender, EventArgs e)
        {
            Bind_myGridView();
        }
    }
}