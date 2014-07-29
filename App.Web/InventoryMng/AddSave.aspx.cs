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
    public partial class AddSave : System.Web.UI.Page
    {
        App.Dll.InventoryMethod.BusinessLayer bl = new App.Dll.InventoryMethod.BusinessLayer();
        App.Dll.InventoryMethod.DBInventoryTempMng DB1 = new App.Dll.InventoryMethod.DBInventoryTempMng();
        App.Dll.InventoryMethod.DBInventoryMng DB2 = new App.Dll.InventoryMethod.DBInventoryMng();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
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
                else
                {
                    this.Page.RegisterStartupScript("alert", "<script>alert('没有清单信息！');</script>");
                    return;
                }
            }
        }

        private void LoadData()
        {
            int rid = int.Parse(ViewState["rid"].ToString());
            Inventory d = DB1.GetInventoryByID(rid);

            if (d != null)
            {
                InventoryProfile.InventoryObject = d;
                InventoryProfile.Bind();
            }
            else
            {
                this.Page.RegisterStartupScript("alert", "<script>alert('无法读取该清单数据！');</script>");
                return;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (ViewState["rid"] == null)
            {
                this.Page.RegisterStartupScript("alert", "<script>alert('清单对象不存在！');</script>");
                return;
            }
            //一定是没有保存过的清单
            Inventory add = DB1.GetInventoryByID(int.Parse(ViewState["rid"].ToString()));
            add.LastModifier.ID = int.Parse(Session["UserID"].ToString());
            if (DB1.Delete(add))
            {
                if (!DB2.IsExistWhenAdd(add))
                {
                    int id = DB2.Add(add);
                    if (id > 0)
                    {
                        add.ID = id;
                        if (DB2.AddInventoryDocs(add) && DB2.AddInventoryDocAttrs(add))
                        {
                            Response.Write(" <script language=javascript>alert('保存成功！');window.window.location.href='List.aspx?issaved=1';</script> ");
                        }
                        else
                        {
                            this.Page.RegisterStartupScript("alert", "<script>alert('保存出错！');</script>");
                            return;
                        }
                    }
                    else
                    {
                        this.Page.RegisterStartupScript("alert", "<script>alert('保存出错！');</script>");
                        return;
                    }
                }
                else
                {
                    this.Page.RegisterStartupScript("alert", "<script>alert('该清单已经存在！');</script>");
                    return;
                }
            }
            else
            {
                this.Page.RegisterStartupScript("alert", "<script>alert('删除临时清单出错！');</script>");
                return;
            }

        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("AddDocAttrs.aspx?rid=" + ViewState["rid"].ToString());
        }

    }
}