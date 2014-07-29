using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace App.Web
{
    public partial class PwdManager : System.Web.UI.Page
    {
        App.Dll.BaseClass bc = new Dll.BaseClass();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) 
            {
            
            }
            plMsg.Visible = false;
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            if (TextBox1.Text.Trim() != "")
            {
                if (Session["UserID"] == null)
                {
                    plMsg.Visible = true;
                    lbMsg.Text = "<font color='red'>请登录系统!</font>";
                    return;
                }
                string encodeOldPwd = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(TextBox1.Text.Trim(), "MD5").ToLower().Substring(8, 16);
                string userID = Session["UserID"].ToString();
                if (CheckOldPwd(userID, encodeOldPwd) == true)
                {
                    if (TextBox2.Text.Trim() == TextBox3.Text.Trim() && TextBox2.Text.Trim() != "" && TextBox3.Text.Trim() != "")
                    {
                        string newPwd = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(TextBox2.Text.Trim(), "MD5").ToLower().Substring(8, 16);
                        ChangePwd(userID, newPwd);
                        plMsg.Visible = true;
                        lbMsg.Text = "<font color='red'>密码修改成功!</font>";
                        TextBox1.Text = "";
                        TextBox2.Text = "";
                        TextBox3.Text = "";
                    }
                    else
                    {
                        plMsg.Visible = true;
                        lbMsg.Text = "<font color='red'>两次输入的新密码不一致，请重新输入!</font>";
                        TextBox3.Text = "";
                        TextBox2.Text = "";
                        return;
                    }
                }
                else
                {
                    plMsg.Visible = true;
                    lbMsg.Text = "<font color='red'>输入的旧密码不正确，请重新输入!</font>";
                    TextBox3.Text = "";
                    TextBox2.Text = "";
                    TextBox1.Text = "";
                }
            }
            else
            {
                plMsg.Visible = true;
                lbMsg.Text = "<font color='red'>请输入旧密码!</font>";
                return;
            }
        }
        protected void Button2_Click(object sender, EventArgs e)
        {
            Response.Redirect("PwdManage.aspx");
        }

        private bool CheckOldPwd(string userID, string pwd)
        {
            bool flag = false;
            if (App.Dll.BaseClass.mydataset("select * from Users_Tab where ID='" + Convert.ToInt32(userID) + "' and Password='" + pwd + "'").Tables[0].Rows.Count > 0)
            {
                flag = true;
            }
            return flag;
        }
        private void ChangePwd(string userID, string newPwd)
        {
            string sqltxt = "update Users_Tab set Password='" + newPwd + "' where ID='" + Convert.ToInt32(userID) + "'";
            bc.RunSqlTransaction(sqltxt);
        }
    }
}