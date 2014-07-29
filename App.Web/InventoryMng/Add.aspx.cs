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
    public partial class Add : System.Web.UI.Page
    {
        App.Dll.InventoryMethod.DBInventoryTempMng DB1 = new App.Dll.InventoryMethod.DBInventoryTempMng();
        App.Dll.InventoryMethod.DBInventoryMng DB2 = new App.Dll.InventoryMethod.DBInventoryMng();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                lb_deptid.Text = "";
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
            }
        }

        private void LoadData()
        {
            int rid = int.Parse(ViewState["rid"].ToString());
            Inventory d = new Inventory();
            if (ViewState["issaved"] == null)
                d = DB1.GetInventoryBasicByID(rid);
            else
                d = DB2.GetInventoryBasicByID(rid);

            if (d != null)
            {
                //类别还没有做
                category1.SelectedID = lb_deptid.Text = d.Type.ID.ToString();
                txtName.Text = d.Name;
                txtDescription.Text = d.Description;
            }
            else
            {
                this.Page.RegisterStartupScript("alert", "<script>alert('无法读取该清单数据！');</script>");
                return;
            }
        }
        protected void TextChanged1(object sender, EventArgs e)
        {
            lb_deptid.Text = category1.SelectedID;
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            int pid = 0;
            if (lb_deptid.Text.Trim() == "")
            {
                this.Page.RegisterStartupScript("alert", "<script>alert('请选择清单类别！');</script>");
                return;
            }
            else
                pid = int.Parse(lb_deptid.Text.Trim());
            //没有保存过的，写入临时表
            if (ViewState["issaved"] == null)
            {
                if (ViewState["rid"] == null)//添加
                {
                    Inventory add = new Inventory() { Name = txtName.Text.Trim(), Description = txtDescription.Text.Trim() };
                    add.Type = new App.Model.InventoryType() { ID = pid };
                    add.Creator = new User() { ID = Session["UserID"].ToInt() };
                    add.LastModifier = new User() { ID = Session["UserID"].ToInt() };
                    if (!DB1.IsExistWhenAdd(add))
                    {
                        int id = DB1.Add(add);
                        if (id != 0)//添加成功
                        {
                            Response.Redirect("AddDocs.aspx?rid=" + id);
                        }
                        else
                        {
                            this.Page.RegisterStartupScript("alert", "<script>alert('添加出错！');</script>");
                            return;
                        }
                    }
                    else
                    {
                        this.Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script>alert('该清单已经存在！');</script>");
                        //this.Page.RegisterStartupScript("alert", "<script>alert('该清单已经存在！');</script>");
                        return;
                    }
                }
                else//编辑
                {
                    Inventory edit = DB1.GetInventoryByID(int.Parse(ViewState["rid"].ToString()));
                    edit.Type.ID = pid;
                    edit.Name = txtName.Text.Trim();
                    edit.Description = txtDescription.Text.Trim();
                    edit.LastModifier.ID = int.Parse(Session["UserID"].ToString());

                    if (!DB1.IsExistWhenUpdate(edit))
                    {
                        if (DB1.Update(edit))//修改成功
                        {
                            Response.Redirect("AddDocs.aspx?rid=" + edit.ID);
                        }
                        else
                        {
                            this.Page.RegisterStartupScript("alert", "<script>alert('修改出错！');</script>");
                            return;
                        }
                    }
                    else
                    {
                        this.Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script>alert('该清单已经存在！');</script>");
                        //this.Page.RegisterStartupScript("alert", "<script>alert('该清单已经存在！');</script>");
                        return;
                    }
                }
            }
            else//保存过的写入正式表
            {
                if (ViewState["rid"] == null)//添加
                {
                    Inventory add = new Inventory() { Name = txtName.Text.Trim(), Description = txtDescription.Text.Trim() };
                    add.Type = new App.Model.InventoryType() { ID = pid };
                    add.Creator = new User() { ID = Session["UserID"].ToInt() };
                    add.LastModifier = new User() { ID = Session["UserID"].ToInt() };
                    if (!DB2.IsExistWhenAdd(add))
                    {
                        int id = DB2.Add(add);
                        if (id != 0)//添加成功
                        {
                            Response.Redirect("AddDocs.aspx?issaved=1&rid=" + id);
                        }
                        else
                        {
                            this.Page.RegisterStartupScript("alert", "<script>alert('添加出错！');</script>");
                            return;
                        }
                    }
                    else
                    {
                        this.Page.RegisterStartupScript("alert", "<script>alert('该清单已经存在！');</script>");
                        return;
                    }
                }
                else//编辑
                {
                    Inventory edit = DB2.GetInventoryByID(int.Parse(ViewState["rid"].ToString()));
                    edit.Type.ID = pid;
                    edit.Name = txtName.Text.Trim();
                    edit.Description = txtDescription.Text.Trim();
                    edit.LastModifier.ID = int.Parse(Session["UserID"].ToString());

                    if (!DB2.IsExistWhenUpdate(edit))
                    {
                        if (DB2.Update(edit))//修改成功
                        {
                            Response.Redirect("AddDocs.aspx?issaved=1&rid=" + edit.ID);
                        }
                        else
                        {
                            this.Page.RegisterStartupScript("alert", "<script>alert('修改出错！');</script>");
                            return;
                        }
                    }
                    else
                    {
                        this.Page.RegisterStartupScript("alert", "<script>alert('该清单已经存在！');</script>");
                        return;
                    }
                }
            }
        }
    }
}