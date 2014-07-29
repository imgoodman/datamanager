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
    public partial class InstanceAdd : System.Web.UI.Page
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
                Session["rtnID"] = 0;

                bl.Bind_ddlInventoryName(ddlInventory);
                ddlInventory.Items.Insert(0, new ListItem(" ---请选择清单对象---", "0"));
                ViewState["rid"] = null;
                string rid = Request.Params["rid"];
                if (!string.IsNullOrEmpty(rid))
                {
                    //编辑
                    ViewState["rid"] = rid;
                    if (rid.ToInt() > 0)
                    {
                        LoadData();
                    }
                }
            }
            if (Session["rtnID"].ToString().ToInt() != 0)//弹出窗口有保存
            {
                txtID.Text = Session["rtnID"].ToString();
                ViewState["rid"] = Session["rtnID"].ToString();
                Bind_myGridView();
            }
        }

        protected void LoadData()
        {
            int rid = ViewState["rid"].ToString().ToInt();
            InventoryInstance d = DB.GetInventoryInstanceByID(rid);
            if (d != null)
            {
                ddlInventory.SelectedValue = d.InventoryID.ToString();

                Inventory i = im.GetInventoryByID(d.InventoryID);
                if (i != null)
                {
                    //类别还没有做
                    lbName.Text = i.Name;
                    lbDescription.Text = i.Description;
                    Bind_myGridView();
                }
                else
                {
                    this.Page.RegisterStartupScript("alert", "<script>alert('无法读取该清单对象数据！');</script>");
                    return;
                }
            }
            else
            {
                this.Page.RegisterStartupScript("alert", "<script>alert('无法读取该清单实例数据！');</script>");
                return;
            }
        }

        private void Bind_myGridView()
        {
            string orderByName = ViewState["orderByName"].ToString();
            string orderByType = ViewState["orderByType"].ToString();

            DataSet ds = new DataSet();
            DataTable tb = new DataTable("Table");
            ds.Tables.Add(tb);
            DataColumn col1 = tb.Columns.Add("ID", typeof(string));
            DataColumn col2 = tb.Columns.Add("DocID", typeof(string));
            DataColumn col3 = tb.Columns.Add("DocInstanceIDs", typeof(string));
            DataColumn col5 = tb.Columns.Add("DocName", typeof(string));
            DataColumn col6 = tb.Columns.Add("DocInstanceNames", typeof(string));
            DataColumn col7 = tb.Columns.Add("InventoryID", typeof(string));
            DataColumn col8 = tb.Columns.Add("DocInstanceTranIDs", typeof(string));

            if (ViewState["rid"] == null)//添加
            {
                Inventory i = im.GetInventoryByID(ddlInventory.SelectedValue.ToInt());
                if (i != null)
                {
                    if (i.Docs != null)
                    {
                        for (int j = 0; j < i.Docs.Count(); j++)
                        {
                            DataRow row = ds.Tables["Table"].NewRow();
                            int docid = i.Docs[j].ID;
                            row["ID"] = 0;
                            row["DocID"] = docid;
                            row["DocInstanceIDs"] = 0;
                            row["InventoryID"] = ddlInventory.SelectedValue;
                            row["DocName"] = dcs.getDocumentById(docid).DocName;
                            row["DocInstanceTranIDs"] = 0;
                            ds.Tables["Table"].Rows.Add(row);
                        }
                    }
                }
            }
            else//编辑
            {
                int rid = ViewState["rid"].ToString().ToInt();
                InventoryInstance d = DB.GetInventoryInstanceByID(rid);
                if (d != null && d.InventoryID.ToString() == ddlInventory.SelectedValue)//没有修改过清单对象的才绑定
                {
                    Inventory i = im.GetInventoryByID(d.InventoryID);
                    if (i != null)
                    {
                        if (i.Docs != null)
                        {
                            for (int j = 0; j < i.Docs.Count(); j++)
                            {
                                DataRow row = ds.Tables["Table"].NewRow();
                                int docid = i.Docs[j].ID;
                                string docInstanceIDs = null;
                                if (d.InstanceDocs.Where(p => p.DocID == docid).Count() > 0)
                                {
                                    docInstanceIDs = d.InstanceDocs.Where(p => p.DocID == docid).First().DocInstanceIDs;
                                }
                                row["ID"] = rid;
                                row["DocID"] = docid;
                                row["DocName"] = dcs.getDocumentById(docid).DocName;
                                row["InventoryID"] = ddlInventory.SelectedValue;
                                if (!string.IsNullOrEmpty(docInstanceIDs))
                                {
                                    row["DocInstanceIDs"] = docInstanceIDs;
                                    row["DocInstanceNames"] = GetDocInstanceAttrsByIDs(docInstanceIDs, i.Docs[j].Attrs);
                                    row["DocInstanceTranIDs"] = docInstanceIDs;
                                    //row["DocInstanceTranIDs"] = docInstanceIDs.Replace(',', 'a');
                                }
                                else
                                {
                                    row["DocInstanceIDs"] = 0;
                                    row["DocInstanceTranIDs"] = 0;
                                }
                                ds.Tables["Table"].Rows.Add(row);
                            }
                        }
                    }
                }
            }
            myGridView.DataSource = ds;
            myGridView.DataBind();
        }

        public string GetDocInstanceAttrsByIDs(string docInstanceIDs, List<DocumentAttr> attrs)
        {
            string attrNames = null;
            if (!string.IsNullOrEmpty(docInstanceIDs))
            {
                foreach (var id in docInstanceIDs.Split(','))
                {
                    DocumentInstance d = ds.getDocInstanceById(id.ToInt());
                    if (attrs != null)
                    {
                        string attrName = null;//一行属性值
                        for (int i = 0; i < attrs.Count(); i++)
                        {
                            if (d.Document.Attrs.Where(p => p.ID == attrs[i].ID).Count() > 0)
                            {
                                DocumentAttr attr = d.Document.Attrs.Where(p => p.ID == attrs[i].ID).First();
                                attrName += attr.AttrName + "：" + attr.TranValue + "&nbsp;&nbsp;&nbsp;";
                                attrName += "<br />";//换行
                            }
                        }
                        attrNames += attrName;
                    }
                }
            }
            return attrNames;
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

                //Inventory d = (Inventory)e.Row.DataItem;
                //HyperLink hlName = (HyperLink)e.Row.FindControl("hlName");
                //hlName.Text = d.Name;
                //hlName.NavigateUrl = "Profile.aspx?rid=" + d.ID;
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

        protected void ddlInventory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlInventory.SelectedIndex != 0)
            {
                Inventory i = im.GetInventoryByID(ddlInventory.SelectedValue.ToInt());
                if (i != null)
                {
                    //类别还没有做
                    lbName.Text = i.Name;
                    lbDescription.Text = i.Description;
                    Bind_myGridView();
                }
                else
                {
                    this.Page.RegisterStartupScript("alert", "<script>alert('无法读取该清单对象数据！');</script>");
                    return;
                }
            }
            else
            {
                //类别还没有做
                lbName.Text = "";
                lbDescription.Text = "";
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (ViewState["rid"] == null)//添加
            {

            }
            else//编辑
            {

            }

        }

    }
}