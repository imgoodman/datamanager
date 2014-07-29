using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using App.Dll;
using App.Model;

namespace App.Web.InventoryMng
{
    public partial class AddDocAttrs : System.Web.UI.Page
    {
        App.Dll.InventoryMethod.BusinessLayer bl = new App.Dll.InventoryMethod.BusinessLayer();
        App.Dll.InventoryMethod.DBInventoryTempMng DB1 = new App.Dll.InventoryMethod.DBInventoryTempMng();
        App.Dll.InventoryMethod.DBInventoryMng DB2 = new App.Dll.InventoryMethod.DBInventoryMng();
        App.Dll.DocConfigService dcs = new DocConfigService();
        int attrCount = MyExtension.ToInt(MyExtension.getAppValue("InventoryDocAttrCount"));//文档可以勾选的属性个数

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["orderByName"] = "ID";
                ViewState["orderByType"] = "desc";
                ViewState["rid"] = null;
                ViewState["issaved"] = null;
                string issaved = Request.Params["issaved"];
                if (!string.IsNullOrEmpty(issaved))
                {
                    ViewState["issaved"] = issaved;
                }
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
                else
                {
                    this.Page.RegisterStartupScript("alert", "<script>alert('没有清单信息！');</script>");
                }
                lbTitle.InnerText = "选择清单对象文档需要显示的属性（每个文档最多勾选" + attrCount + "个属性）";
            }
        }

        private void LoadData()
        {
            int rid = int.Parse(ViewState["rid"].ToString());
            Inventory d = new Inventory();
            if (ViewState["issaved"] == null)
                d = DB1.GetInventoryByID(rid);
            else
                d = DB2.GetInventoryByID(rid);

            if (d != null)
            {
                //类别还没有做
                lbName.Text = d.Name;
                lbDescription.Text = d.Description;

                if (d.Docs == null)
                {
                    lbRemind.Visible = true;
                }
                else
                {
                    lbRemind.Visible = false;
                    Bind_myGridView(d.Docs);
                }
            }
            else
            {
                this.Page.RegisterStartupScript("alert", "<script>alert('无法读取该清单数据！');</script>");
            }
        }

        private void Bind_myGridView(List<Document> data)
        {
            string orderByName = ViewState["orderByName"].ToString();
            string orderByType = ViewState["orderByType"].ToString();

            myGridView.DataSource = data;
            myGridView.DataBind();
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

                Document d = (Document)e.Row.DataItem;
                Document doc = dcs.getDocumentById(d.ID);
                if (doc != null)
                {
                    CheckBoxList cblDocAttrs = (CheckBoxList)e.Row.FindControl("cblDocAttrs");
                    if (doc.Attrs != null)
                    {
                        cblDocAttrs.DataSource = doc.Attrs;
                        cblDocAttrs.DataTextField = "AttrName";
                        cblDocAttrs.DataValueField = "ID";
                        cblDocAttrs.DataBind();
                    }
                    List<DocumentAttr> docattrlist = new List<DocumentAttr>();
                    if (ViewState["issaved"] == null)
                        docattrlist = DB1.GetInventoryDocAttrsByDocID(ViewState["rid"].ToString().ToInt(), d.ID);
                    else
                        docattrlist = DB2.GetInventoryDocAttrsByDocID(ViewState["rid"].ToString().ToInt(), d.ID);
                    CheckCheckbox(cblDocAttrs, docattrlist);
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

        protected void CheckCheckbox(CheckBoxList CheckBoxs, List<DocumentAttr> data)
        {
            if (CheckBoxs.Items.Count > 0)
            {
                if (data != null)
                {
                    foreach (ListItem list in CheckBoxs.Items)
                    {
                        foreach (var d in data)
                        {
                            if (list.Value == d.ID.ToString())
                            {
                                list.Selected = true;
                            }
                        }
                    }
                }
            }
        }

        protected string GetCheckbox(CheckBoxList CheckBoxs)
        {
            string IDs = null;
            if (CheckBoxs.Items.Count > 0)
            {
                foreach (ListItem list in CheckBoxs.Items)
                {
                    if (list.Selected)
                    {
                        IDs += list.Value + ",";
                    }
                }
                if (IDs != null)
                {
                    IDs = IDs.Remove(IDs.Length - 1);
                }
            }
            return IDs;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            //没有保存过的，写入临时表
            if (ViewState["issaved"] == null)
            {
                Inventory edit = DB1.GetInventoryByID(int.Parse(ViewState["rid"].ToString()));
                edit.LastModifier.ID = int.Parse(Session["UserID"].ToString());
                if (edit.Docs != null)//表格不为空
                {
                    if (DB1.Update(edit))
                    {
                        for (int i = 0; i < myGridView.Rows.Count; i++)
                        {
                            Document doc = new Document();
                            doc.Attrs = new List<DocumentAttr>();
                            doc.ID = myGridView.DataKeys[i].Value.ToInt();
                            CheckBoxList cblDocAttrs = (CheckBoxList)myGridView.Rows[i].FindControl("cblDocAttrs");
                            string IDs = GetCheckbox(cblDocAttrs);
                            if (IDs != null)
                            {
                                if (IDs.Split(',').Count() > attrCount)
                                {
                                    this.Page.RegisterStartupScript("alert", "<script>alert('每个文档最多勾选'" + attrCount + "'个属性！');</script>");
                                    return;
                                }
                                else
                                {
                                    foreach (var id in IDs.Split(','))
                                    {
                                        DocumentAttr da = new DocumentAttr();
                                        da.ID = id.ToInt();
                                        doc.Attrs.Add(da);
                                    }
                                }
                            }
                            edit.Docs.Where(p => p.ID == myGridView.DataKeys[i].Value.ToInt()).First().Attrs.Clear();
                            edit.Docs.Where(p => p.ID == myGridView.DataKeys[i].Value.ToInt()).First().Attrs = doc.Attrs;
                        }
                        if (!DB1.AddInventoryDocAttrs(edit))
                        {
                            this.Page.RegisterStartupScript("alert", "<script>alert('添加出错！');</script>");
                            return;
                        }
                    }
                    else
                    {
                        this.Page.RegisterStartupScript("alert", "<script>alert('更新清单信息出错！');</script>");
                        return;
                    }
                }
                Response.Redirect("AddSave.aspx?rid=" + edit.ID);
            }
            else//保存过的写入正式表
            {
                Inventory edit = DB2.GetInventoryByID(int.Parse(ViewState["rid"].ToString()));
                edit.LastModifier.ID = int.Parse(Session["UserID"].ToString());
                if (edit.Docs != null)//表格不为空
                {
                    if (DB2.Update(edit))
                    {

                        for (int i = 0; i < myGridView.Rows.Count; i++)
                        {
                            Document doc = new Document();
                            doc.Attrs = new List<DocumentAttr>();
                            doc.ID = myGridView.DataKeys[i].Value.ToInt();
                            CheckBoxList cblDocAttrs = (CheckBoxList)myGridView.Rows[i].FindControl("cblDocAttrs");
                            string IDs = GetCheckbox(cblDocAttrs);
                            if (IDs != null)
                            {
                                if (IDs.Split(',').Count() > attrCount)
                                {
                                    this.Page.RegisterStartupScript("alert", "<script>alert('每个文档最多勾选'" + attrCount + "'个属性！');</script>");
                                    return;
                                }
                                else
                                {
                                    foreach (var id in IDs.Split(','))
                                    {
                                        DocumentAttr da = new DocumentAttr();
                                        da.ID = id.ToInt();
                                        doc.Attrs.Add(da);
                                    }
                                }
                            }
                            edit.Docs.Where(p => p.ID == myGridView.DataKeys[i].Value.ToInt()).First().Attrs.Clear();
                            edit.Docs.Where(p => p.ID == myGridView.DataKeys[i].Value.ToInt()).First().Attrs = doc.Attrs;
                        }
                        if (!DB2.AddInventoryDocAttrs(edit))
                        {
                            this.Page.RegisterStartupScript("alert", "<script>alert('添加出错！');</script>");
                            return;
                        }
                    }
                    else
                    {
                        this.Page.RegisterStartupScript("alert", "<script>alert('更新清单信息出错！');</script>");
                        return;
                    }
                }
                Response.Redirect("Profile.aspx?issaved=1&rid=" + edit.ID);
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            if (ViewState["issaved"] == null)
            {
                Response.Redirect("AddDocs.aspx?rid=" + ViewState["rid"].ToString());
            }
            else
            {
                Response.Redirect("AddDocs.aspx?issaved=1&rid=" + ViewState["rid"].ToString());
            }
        }

    }
}