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
    public partial class DocInstancesChoose : System.Web.UI.Page
    {
        App.Dll.InventoryMethod.BusinessLayer bl = new App.Dll.InventoryMethod.BusinessLayer();
        App.Dll.DocConfigService dcs = new DocConfigService();
        App.Dll.DocService ds = new DocService();
        App.Dll.InventoryMethod.DBInventoryInstanceMng DB = new Dll.InventoryMethod.DBInventoryInstanceMng();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["orderByName"] = "ID";
                ViewState["orderByType"] = "desc";

                ViewState["rid"] = null;
                ViewState["instancetranids"] = null;
                ViewState["ivtinstanceid"] = null;
                ViewState["inventoryid"] = null;

                string instancetranids = Request.Params["instancetranids"];
                if (!string.IsNullOrEmpty(instancetranids) && instancetranids != "0")
                {
                    ViewState["instancetranids"] = instancetranids;
                }
                string ivtinstanceid = Request.Params["ivtinstanceid"];
                if (!string.IsNullOrEmpty(ivtinstanceid) && ivtinstanceid != "0")
                {
                    ViewState["ivtinstanceid"] = ivtinstanceid;
                }
                string inventoryid = Request.Params["inventoryid"];
                if (!string.IsNullOrEmpty(inventoryid) && inventoryid != "0")
                {
                    ViewState["inventoryid"] = inventoryid;
                }

                string rid = Request.Params["rid"];
                if (!string.IsNullOrEmpty(rid))
                {
                    //编辑
                    ViewState["rid"] = rid;
                    LoadData();
                    Bind_myGridView();
                }
                else
                {
                    this.Page.RegisterStartupScript("alert", "<script>alert('没有文档信息！');</script>");
                }
            }
        }

        protected void LoadData()
        {
            int rid = ViewState["rid"].ToString().ToInt();
            Document d = dcs.getDocumentById(rid);
            if (d != null)
            {
                lbType.Text = d.DocType.TypeName;
                lbName.Text = d.DocName;
                lbDescription.Text = d.Description;
            }
            else
            {
                this.Page.RegisterStartupScript("alert", "<script>alert('无法读取该文档对象数据！');</script>");
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
            DataColumn col5 = tb.Columns.Add("InstanceAttrNames", typeof(string));

            if (ViewState["rid"] != null)
            {
                int rid = ViewState["rid"].ToString().ToInt();
                List<DocumentInstance> instanceList = bl.GetInstancesByDocID(rid);//文档类的所有实例
                if (instanceList != null)
                {
                    for (int i = 0; i < instanceList.Count(); i++)
                    {
                        DataRow row = ds.Tables["Table"].NewRow();
                        int docid = instanceList[i].ID;
                        row["ID"] = instanceList[i].ID;
                        row["InstanceAttrNames"] = bl.GetInstanceAttrNamesByID(instanceList[i]);
                        ds.Tables["Table"].Rows.Add(row);
                    }
                }
                else
                {
                    lbRemind.Visible = true;
                    btnSave.Visible = false;
                }
            }
            myGridView.DataSource = ds;
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

                Label id = (Label)e.Row.FindControl("ID");
                CheckBox chkSelect = (CheckBox)e.Row.FindControl("chkSelect");
                if (ViewState["instancetranids"] != null)
                {
                    string instancetranids = ViewState["instancetranids"].ToString();
                    instancetranids = ',' + instancetranids + ',';
                    if (instancetranids.Contains(',' + id.Text + ','))
                    {
                        chkSelect.Checked = true;
                    }
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

        protected void CheckCheckBox(string instanceids)
        {
            string addedinstanceids = ',' + instanceids + ',';
            for (int i = 0; i < myGridView.Rows.Count; i++)
            {
                CheckBox cb = (CheckBox)myGridView.Rows[i].FindControl("chkSelect");
                if (addedinstanceids.Contains(',' + myGridView.DataKeys[i].Value.ToString() + ','))
                {
                    cb.Checked = true;
                }
            }
        }

        protected string GetCheckBox()
        {
            string dataIDAll = null;
            for (int i = 0; i < myGridView.Rows.Count; i++)
            {
                CheckBox cb = (CheckBox)myGridView.Rows[i].FindControl("chkSelect");
                if (cb.Checked)
                {
                    dataIDAll += myGridView.DataKeys[i].Value.ToString() + ",";
                }
            }
            if (dataIDAll != null)
            {
                dataIDAll = dataIDAll.Remove(dataIDAll.Length - 1);
            }
            return dataIDAll;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            int docid = ViewState["rid"].ToInt();
            string instanceids = GetCheckBox();
            if (ViewState["ivtinstanceid"] != null)//编辑
            {
                InventoryInstance edit = DB.GetInventoryInstanceByID(ViewState["ivtinstanceid"].ToInt());
                edit.LastModifier = new User() { ID = Session["UserID"].ToInt() };
                InventoryInstanceDocs editDoc = new InventoryInstanceDocs();
                if (edit.InstanceDocs.Where(p => p.DocID == docid).Count() > 0)//已经存在该文档
                {
                    editDoc = edit.InstanceDocs.Where(p => p.DocID == docid).First();
                    editDoc.DocInstanceIDs = instanceids;
                    if (!DB.UpdateInstanceDocs(editDoc))
                    {
                        this.Page.RegisterStartupScript("alert", "<script>alert('添加失败！');</script>");
                        return;
                    }
                }
                else//新增
                {
                    List<InventoryInstanceDocs> list = new List<InventoryInstanceDocs>();
                    InventoryInstanceDocs addDoc = new InventoryInstanceDocs();
                    addDoc.DocID = docid;
                    addDoc.DocInstanceIDs = instanceids;
                    list.Add(addDoc);
                    if (!DB.AddInstanceDocs(edit.ID, list))
                    {
                        this.Page.RegisterStartupScript("alert", "<script>alert('添加失败！');</script>");
                        return;
                    }
                }
            }
            else
            {
                InventoryInstance add = new InventoryInstance();
                add.InstanceDocs = new List<InventoryInstanceDocs>();
                add.InventoryID = ViewState["inventoryid"].ToInt();
                add.Creator = new User() { ID = Session["UserID"].ToInt() };
                add.LastModifier = new User() { ID = Session["UserID"].ToInt() };
                InventoryInstanceDocs addDoc = new InventoryInstanceDocs();
                addDoc.DocID = docid;
                addDoc.DocInstanceIDs = instanceids;
                add.InstanceDocs.Add(addDoc);
                int id = DB.Add(add);
                if (id > 0)
                {
                    Session["rtnID"] = id;
                }
                else
                {
                    this.Page.RegisterStartupScript("alert", "<script>alert('添加失败！');</script>");
                    return;
                }
            }

            Response.Write("<script>window.opener.location.reload()</script>");
            Response.Write("<script>window.self.close()</script>");
            //Response.Write("<script>window.opener.document.getElementById(\"txtID\").value=" + docid + "   </script>");
            //ViewState["rtninstanceids"] = instanceids;
            //ClientScript.RegisterStartupScript(ClientScript.GetType(), "myscript", "<script>getSelected(" + docid + ';' + instanceids + ");</script>");
        }

    }
}