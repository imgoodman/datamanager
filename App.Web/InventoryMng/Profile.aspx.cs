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
    public partial class Profile : System.Web.UI.Page
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
                    return;
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
                InventoryProfile.InventoryObject = d;
                InventoryProfile.Bind();
            }
            else
            {
                this.Page.RegisterStartupScript("alert", "<script>alert('无法读取该清单数据！');</script>");
                return;
            }
        }

        protected void btnEdit_Click(object sender, EventArgs e)
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

        protected void btnDel_Click(object sender, EventArgs e)
        {
            if (ViewState["issaved"] == null)
            {
                int rid = int.Parse(ViewState["rid"].ToString());
                if (DB1.DeleteByID(rid, Session["UserID"].ToInt()))
                {
                    Response.Write(" <script language=javascript>alert('删除成功！');window.window.location.href='List.aspx';</script> ");
                }
                else
                {
                    this.Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script>alert('删除失败！');</script>");
                    //this.Page.RegisterStartupScript("alert", "<script>alert('删除失败！');</script>");
                    return;
                }
            }
            else
            {
                int rid = int.Parse(ViewState["rid"].ToString());
                if (DB1.DeleteByID(rid, Session["UserID"].ToInt()))
                {
                    Response.Write(" <script language=javascript>alert('删除成功！');window.window.location.href='List.aspx?issaved=1';</script> ");
                }
                else
                {
                    this.Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script>alert('删除失败！');</script>");
                    //this.Page.RegisterStartupScript("alert", "<script>alert('删除失败！');</script>");
                    return;
                }
            }
        }

    }
}