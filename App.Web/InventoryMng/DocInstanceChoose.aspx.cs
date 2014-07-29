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
    public partial class DocInstanceChoose : System.Web.UI.Page
    {
        App.Dll.InventoryMethod.BusinessLayer bl = new App.Dll.InventoryMethod.BusinessLayer();
        App.Dll.InventoryMethod.DBInventoryTempMng DB1 = new App.Dll.InventoryMethod.DBInventoryTempMng();
        App.Dll.InventoryMethod.DBInventoryMng DB2 = new App.Dll.InventoryMethod.DBInventoryMng();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["rid"] = null;
                ViewState["docid"] = null;
                string rid = Request.Params["rid"];
                string docid = Request.Params["docid"];
                if (!string.IsNullOrEmpty(rid))
                {
                    //编辑
                    lbName.Text = "rid=" + rid + ",docid=" + docid;
                    ViewState["rid"] = rid;
                    if (rid.ToInt() > 0)
                    {
                        //LoadData();
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
                lbName.Text = d.Name;
                lbDescription.Text = d.Description;
                bl.Bind_cblDocsName(cblDocs);
                if (d.Docs != null)
                {
                    CheckCheckbox(cblDocs, d.Docs);
                }
            }
            else
            {
                this.Page.RegisterStartupScript("alert", "<script>alert('无法读取该清单数据！');</script>");
                return;
            }
        }

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
                    string IDs = GetCheckbox(cblDocs);
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
                    string IDs = GetCheckbox(cblDocs);
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

    }
}