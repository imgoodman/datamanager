using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using App.Web.Controls;

namespace App.Web.Controls
{
    public partial class Category : System.Web.UI.UserControl
    {
        App.Dll.BaseClass bc = new Dll.BaseClass();
        public delegate void PostBackEvent(object sender, EventArgs e);
        public event PostBackEvent TextChanged;
        private string selectedid;
        private string selectedname;

        public string SelectedID
        {
            get { return selectedid = lb_id.Text; }
            set
            {
                selectedid = lb_id.Text = value;
                switch (GetDataStyle.ToString())
                {
                    case "Dept":

                        DataTable dt2 = App.Dll.BaseClass.mydataset("select DeptName from DeptManage_Tab where ID=" + lb_id.Text).Tables[0];
                        if (dt2 != null && dt2.Rows.Count > 0)
                        {
                            tbx_name.Text = dt2.Rows[0]["DeptName"].ToString();
                        }
                        else
                        {
                            tbx_name.Text = "";
                        }
                        break;
                    case "DocType":

                        DataTable dt = App.Dll.BaseClass.mydataset("select TypeName from DocumentType_Tab where ID=" + lb_id.Text).Tables[0];
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            tbx_name.Text = dt.Rows[0]["TypeName"].ToString();
                        }
                        else
                        {
                            tbx_name.Text = "";
                        }
                        break;
                    case "InventoryType":

                        DataTable dt3 = App.Dll.BaseClass.mydataset("select Name from InventoryType_Tab where ID=" + lb_id.Text).Tables[0];
                        if (dt3 != null && dt3.Rows.Count > 0)
                        {
                            tbx_name.Text = dt3.Rows[0]["Name"].ToString();
                        }
                        else
                        {
                            tbx_name.Text = "";
                        }
                        break;
                    default:
                        break;
                }

            }
        }
        public string SelectedName
        {
            get { return tbx_name.Text; }
            set { selectedname = value; tbx_name.Text = selectedname; }
        }
        public enum tableStyle
        {
            Dept, DocType, InventoryType
        }
        private tableStyle strGetDataStyle;
        public tableStyle GetDataStyle
        {
            get { return strGetDataStyle; }
            set { strGetDataStyle = value; }
        }
        bool AutoPostBackOn;
        public bool AutoPostBack
        {
            get { return AutoPostBackOn; }
            set { AutoPostBackOn = value; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!IsPostBack)
            //{
            if (TreeView1.Nodes.Count == 0)
            {
                tbx_name.Attributes.Add("onclick", "showCategory('" + datatree.ClientID + "')");
                //tbx_name.Attributes.Add("onblur", "hideCategory('datatree')"); 
                switch (GetDataStyle.ToString())
                {
                    case "Dept":
                        Bind_Category("DeptName");
                        break;
                    case "DocType":
                        Bind_Category("TypeName");
                        break;
                    case "InventoryType":
                        Bind_Category("Name");
                        break;
                }

            }

        }
        protected void Bind_Category(string textField)
        {
            switch (GetDataStyle.ToString())
            {
                case "Dept":
                    DataTable dtdept = App.Dll.BaseClass.mydataset("select ID,DeptName from DeptManage_Tab where FatherDeptID='0' and State<>'-1'").Tables[0];
                    TreeView1.Nodes.Clear();
                    if (dtdept.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtdept.Rows.Count; i++)
                        {
                            TreeNode tNode = new TreeNode();
                            tNode.Text = dtdept.Rows[i][textField].ToString();
                            tNode.Value = dtdept.Rows[i]["ID"].ToString();
                            TreeView1.Nodes.Add(tNode);
                            Create_tvChild(tNode, Convert.ToInt32(tNode.Value.ToString()), "DeptName");
                        }
                    }
                    break;
                case "DocType":
                    DataTable dtdoctype = App.Dll.BaseClass.mydataset("select ID,TypeName from DocumentType_Tab where FatherTypeID='0' and State<>'-1'").Tables[0];
                    TreeView1.Nodes.Clear();
                    if (dtdoctype.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtdoctype.Rows.Count; i++)
                        {
                            TreeNode tNode = new TreeNode();
                            tNode.Text = dtdoctype.Rows[i][textField].ToString();
                            tNode.Value = dtdoctype.Rows[i]["ID"].ToString();
                            TreeView1.Nodes.Add(tNode);
                            Create_tvChild(tNode, Convert.ToInt32(tNode.Value.ToString()), "TypeName");
                        }
                    }
                    break;
                case "InventoryType":
                    DataTable dtinvtype = App.Dll.BaseClass.mydataset("select ID,Name from InventoryType_Tab where FatherID='0' and State<>'-1'").Tables[0];
                    TreeView1.Nodes.Clear();
                    if (dtinvtype.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtinvtype.Rows.Count; i++)
                        {
                            TreeNode tNode = new TreeNode();
                            tNode.Text = dtinvtype.Rows[i][textField].ToString();
                            tNode.Value = dtinvtype.Rows[i]["ID"].ToString();
                            TreeView1.Nodes.Add(tNode);
                            Create_tvChild(tNode, Convert.ToInt32(tNode.Value.ToString()), "Name");
                        }
                    }
                    break;
            }
            //DBXML.GenerateXML((dt,"fatherProdName","ProdGroupName",xmlpath);


        }
        protected void Create_tvChild(TreeNode pNode, int id, string textField)
        {
            switch (GetDataStyle.ToString())
            {
                case "Dept":
                    DataTable dtdept = App.Dll.BaseClass.mydataset("select ID,DeptName from DeptManage_Tab where FatherDeptID='" + id + "' and State<>'-1'").Tables[0];
                    if (dtdept.Rows.Count <= 0)
                    {
                        return;
                    }
                    else
                    {
                        for (int i = 0; i < dtdept.Rows.Count; i++)
                        {
                            TreeNode tNode = new TreeNode();
                            tNode.Text = dtdept.Rows[i][textField].ToString();
                            tNode.Value = dtdept.Rows[i]["ID"].ToString();
                            pNode.ChildNodes.Add(tNode);
                            Create_tvChild(tNode, Convert.ToInt32(tNode.Value.ToString()), "DeptName");
                        }
                    }
                    break;
                case "DocType":
                    DataTable dtdoctype = App.Dll.BaseClass.mydataset("select ID,TypeName from DocumentType_Tab where FatherTypeID='" + id + "' and State<>'-1'").Tables[0];
                    if (dtdoctype.Rows.Count <= 0)
                    {
                        return;
                    }
                    else
                    {
                        for (int i = 0; i < dtdoctype.Rows.Count; i++)
                        {
                            TreeNode tNode = new TreeNode();
                            tNode.Text = dtdoctype.Rows[i][textField].ToString();
                            tNode.Value = dtdoctype.Rows[i]["ID"].ToString();
                            pNode.ChildNodes.Add(tNode);
                            Create_tvChild(tNode, Convert.ToInt32(tNode.Value.ToString()), "TypeName");
                        }
                    }
                    break;
                case "InventoryType":
                    DataTable dtinvtype = App.Dll.BaseClass.mydataset("select ID,Name from InventoryType_Tab where FatherID='" + id + "' and State<>'-1'").Tables[0];
                    if (dtinvtype.Rows.Count <= 0)
                    {
                        return;
                    }
                    else
                    {
                        for (int i = 0; i < dtinvtype.Rows.Count; i++)
                        {
                            TreeNode tNode = new TreeNode();
                            tNode.Text = dtinvtype.Rows[i][textField].ToString();
                            tNode.Value = dtinvtype.Rows[i]["ID"].ToString();
                            pNode.ChildNodes.Add(tNode);
                            Create_tvChild(tNode, Convert.ToInt32(tNode.Value.ToString()), "Name");
                        }
                    }
                    break;
            }

        }
        protected void TreeView1_SelectedNodeChanged(object sender, EventArgs e)
        {
            selectedname = TreeView1.SelectedNode.Text;
            tbx_name.Text = selectedname;
            lb_id.Text = TreeView1.SelectedValue.ToString();
            if (AutoPostBack)
            {
                TextChanged(sender, e);
            }
        }


        protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
        {
            this.Page.RegisterStartupScript("", "<script>hideCategory('" + datatree.ClientID + "')</script>");
        }
    }
}