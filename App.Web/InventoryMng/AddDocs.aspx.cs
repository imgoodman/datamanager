using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using App.Dll;
using App.Model;
using System.Data;

namespace App.Web.InventoryMng
{
    public partial class AddDocs : System.Web.UI.Page
    {
        App.Dll.InventoryMethod.BusinessLayer bl = new App.Dll.InventoryMethod.BusinessLayer();
        App.Dll.InventoryMethod.DBInventoryTempMng DB1 = new App.Dll.InventoryMethod.DBInventoryTempMng();
        App.Dll.InventoryMethod.DBInventoryMng DB2 = new App.Dll.InventoryMethod.DBInventoryMng();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
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
                App.Dll.InventoryMethod.DBInventoryTypeManage idb = new Dll.InventoryMethod.DBInventoryTypeManage();
                lbType.Text = idb.GetDeptName(d.Type.ID.ToString());
                lbName.Text = d.Name;
                lbDescription.Text = d.Description;
                bl.Bind_cblDocsName(cblDocs);
                Bind_Repeater();
                CheckRepeaterCBL(d.Docs);
                if (d.Docs != null)
                {
                    CheckCheckbox(cblDocs, d.Docs);
                    //Bind_gvDocs(d.Docs);
                    //btnAdd.Text = "继续添加";
                }
            }
            else
            {
                this.Page.RegisterStartupScript("alert", "<script>alert('无法读取该清单数据！');</script>");
                return;
            }
        }
        //protected void Bind_gvDocs(List<Document> Docs)
        //{
        //    gvDocs.DataSource = Docs;
        //    gvDocs.DataBind();
        //}
        //protected void gvDocs_RowCommand(object sender, GridViewCommandEventArgs e)
        //{
        //    int rowIndex = -1;
        //    GridViewRow row = null;
        //    switch (e.CommandName)
        //    {
        //        case "ChooseDel":
        //            rowIndex = Convert.ToInt32(e.CommandArgument);
        //            row = gvDocs.Rows[rowIndex];
        //            string ID = gvDocs.DataKeys[row.RowIndex].Value.ToString();
        //            //txtMdataID.Text = ID;
        //            //txtMDeptName.Text = DB.GetDeptName(ID);
        //            //lb_deptid2.Text = category2.SelectedID = DB.GetFatherDeptID(ID);
        //            break;
        //    }
        //}
        protected void CheckCheckbox(CheckBoxList CheckBoxs, List<Document> Docs)
        {
            if (CheckBoxs.Items.Count > 0)
            {
                if (Docs != null)
                {
                    foreach (ListItem list in CheckBoxs.Items)
                    {
                        foreach (var doc in Docs)
                        {
                            if (list.Value == doc.ID.ToString())
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
            if (ViewState["rid"] == null)
            {
                this.Page.RegisterStartupScript("alert", "<script>alert('清单对象不存在！');</script>");
                return;
            }
            //没有保存过的，写入临时表
            if (ViewState["issaved"] == null)
            {
                Inventory edit = DB1.GetInventoryByID(int.Parse(ViewState["rid"].ToString()));
                edit.LastModifier.ID = int.Parse(Session["UserID"].ToString());
                if (DB1.Update(edit))
                {
                    List<Document> Docs = new List<Document>();
                    //string IDs = GetCheckbox(cblDocs);
                    string IDs = GetRepeaterCBL();
                    if (IDs != null)
                    {
                        foreach (var id in IDs.Split(','))
                        {
                            Document doc = new Document();
                            doc.ID = id.ToInt();
                            Docs.Add(doc);
                        }
                    }
                    else
                    {
                        Docs = null;
                    }
                    edit.Docs = Docs;
                    if (DB1.AddInventoryDocs(edit))//修改成功
                    {
                        Response.Redirect("AddDocAttrs.aspx?rid=" + edit.ID);
                    }
                    else
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
            else//保存过的写入正式表
            {
                Inventory edit = DB2.GetInventoryByID(int.Parse(ViewState["rid"].ToString()));
                edit.LastModifier.ID = int.Parse(Session["UserID"].ToString());
                if (DB2.Update(edit))
                {
                    List<Document> Docs = new List<Document>();
                    //string IDs = GetCheckbox(cblDocs);
                    string IDs = GetRepeaterCBL();
                    if (IDs != null)
                    {
                        foreach (var id in IDs.Split(','))
                        {
                            Document doc = new Document();
                            doc.ID = id.ToInt();
                            Docs.Add(doc);
                        }
                    }
                    else
                    {
                        Docs = null;
                    }
                    edit.Docs = Docs;
                    if (DB2.AddInventoryDocs(edit))//修改成功
                    {
                        Response.Redirect("AddDocAttrs.aspx?issaved=1&rid=" + edit.ID);
                    }
                    else
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
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            if (ViewState["issaved"] == null)
            {
                Response.Redirect("Add.aspx?rid=" + ViewState["rid"].ToString());
            }
            else
            {
                Response.Redirect("Add.aspx?issaved=1&rid=" + ViewState["rid"].ToString());
            }
        }

        //protected void btnAdd_Click(object sender, EventArgs e)
        //{
        //    plSearch.Visible = true;
        //    Bind_gvAllDocs();
        //}
        //protected void btnSearch_Click(object sender, EventArgs e)
        //{
        //    string dueSearch = "1=1";
        //    //if (ddlType.SelectedIndex != 0)
        //    //{
        //    //    dueSearch += " and TypeID =" + ddlType.SelectedValue.ToString();
        //    //}
        //    //if (txtDateFrom.Text != "")
        //    //{
        //    //    dueSearch += " and CreateTime>='" + txtDateFrom.Text + "'";
        //    //}
        //    //if (txtDateTo.Text != "")
        //    //{
        //    //    dueSearch += " and CreateTime<='" + txtDateTo.Text + "'";
        //    //}
        //    //if (txtfuzzyProjName.Text != "")
        //    //{
        //    //    dueSearch += " and DocName like '%" + txtfuzzyProjName.Text + "%'";
        //    //}
        //    //Bind_GridView1(dueSearch);
        //}
        //protected void Bind_gvAllDocs()
        //{
        //    App.Dll.DocConfigService d = new DocConfigService();
        //    gvAllDocs.DataSource = d.getBasicDocs();
        //    gvAllDocs.DataBind();
        //}
        //protected void AspNetPager1_PageChanged(object sender, EventArgs e)
        //{
        //    Bind_gvAllDocs();
        //}

        #region 绑定文档
        App.Dll.DocConfigService dcs = new DocConfigService();
        App.Dll.SysMethod.DBDocTypeManager dtm = new Dll.SysMethod.DBDocTypeManager();
        private void Bind_Repeater()
        {
            List<DocType> types = new List<DocType>();
            List<DocType> data = new List<DocType>();
            types = dtm.getTypes(2);
            if (types != null)
            {
                foreach (var type in types)
                {
                    if (types.Where(p => p.FatherTypeID == type.ID).Count() == 0)//根节点
                    {
                        data.Add(type);
                    }
                }
            }
            Repeater1.DataSource = data;
            Repeater1.DataBind();
        }

        protected void Repeater1_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Label lbTypeName = (Label)e.Item.FindControl("lbTypeName");
                CheckBoxList cblDoc = (CheckBoxList)e.Item.FindControl("cblDoc");
                DocType d = (DocType)e.Item.DataItem;
                lbTypeName.Text = d.TypeName;
                Bind_cblDocsNameByTypeID(cblDoc, d.ID);
            }
        }

        protected void Bind_cblDocsNameByTypeID(CheckBoxList cbl, int typeid)
        {
            cbl.Items.Clear();
            var data = GetDocsByTypeID(typeid);
            cbl.DataSource = data;
            cbl.DataTextField = "DocName";
            cbl.DataValueField = "ID";
            cbl.DataBind();
        }

        protected List<Document> GetDocsByTypeID(int typeid)
        {
            string sql = "select id from Document where isexpired='false' and typeid='" + typeid + "' order by id";
            DataTable dt = BaseClass.mydataset(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                List<Document> list = new List<Document>();
                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(dcs.getDocumentById(int.Parse(dr["id"].ToString())));
                }
                return list;
            }
            else
                return null;
        }

        protected void CheckRepeaterCBL(List<Document> Docs)
        {
            if (Repeater1.Items.Count > 0)
            {
                for (int i = 0; i < Repeater1.Items.Count; i++)
                {
                    CheckBoxList cblDoc = (CheckBoxList)Repeater1.Items[i].FindControl("cblDoc");
                    CheckCheckbox(cblDoc, Docs);
                }
            }
        }

        protected string GetRepeaterCBL()
        {
            string ids = null;
            if (Repeater1.Items.Count > 0)
            {
                for (int i = 0; i < Repeater1.Items.Count; i++)
                {
                    CheckBoxList cblDoc = (CheckBoxList)Repeater1.Items[i].FindControl("cblDoc");
                    string id = GetCheckbox(cblDoc);
                    if (id != null)
                        ids += id + ",";
                }
            }
            if (ids != null)
                ids = ids.Remove(ids.Length - 1);
            return ids;
        }
        #endregion
    }
}