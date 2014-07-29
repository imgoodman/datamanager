using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace App.Web.DocConfig
{
    public partial class AttrRelate : System.Web.UI.Page
    {
        App.Dll.DocMethod.DocAttrRelate DB = new Dll.DocMethod.DocAttrRelate();
        App.Dll.DocConfigService DBDoc = new Dll.DocConfigService();
        //App.Dll.DocMethod.DocInstanceImport ddd = new Dll.DocMethod.DocInstanceImport();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                lbRDocTypeID.Text = RDocTypeSelect.SelectedID = "0";
                Bind_ddlRDoc();
                Bind_ddlRDocAttr();
                lbSDocTypeID.Text = SDocTypeSelect.SelectedID = "0";
                Bind_ddlSDoc();
                Bind_ddlSDocAttr();
                //ddd.ImportDocInstanceFromFile("", 37);
            }
            plMsg.Visible = false;
        }

        protected void RDocType_Selected(object sender, EventArgs e)
        {
            lbRDocTypeID.Text = RDocTypeSelect.SelectedID;
            Bind_ddlRDoc();
            Bind_ddlRDocAttr();
        }
        protected void Bind_ddlRDoc()
        {
            ddlRDoc.Items.Clear();
            ddlRDoc.DataSource = DBDoc.getDocsByTypeId(int.Parse(lbRDocTypeID.Text),"");
            ddlRDoc.DataTextField = "DocName";
            ddlRDoc.DataValueField = "ID";
            ddlRDoc.DataBind();
            ddlRDoc.Items.Insert(0, new ListItem("--请选择--", "0"));
        }
        protected void ddlRDoc_SelectedIndexChanged(object sender, EventArgs e)
        {
            Bind_ddlRDocAttr();
        }
        protected void Bind_ddlRDocAttr()
        {
            ddlRDocAttr.Items.Clear();
            ddlRDocAttr.DataSource = DB.getAttrsByDocID(int.Parse(ddlRDoc.SelectedValue));
            ddlRDocAttr.DataTextField = "AttrName";
            ddlRDocAttr.DataValueField = "ID";
            ddlRDocAttr.DataBind();
            ddlRDocAttr.Items.Insert(0, new ListItem("--请选择--", "0"));
        }

        protected void SDocType_Selected(object sender, EventArgs e)
        {
            lbSDocTypeID.Text = SDocTypeSelect.SelectedID;
            Bind_ddlSDoc();
            Bind_ddlSDocAttr();
        }
        protected void Bind_ddlSDoc()
        {
            ddlSDoc.Items.Clear();
            ddlSDoc.DataSource = DBDoc.getDocsByTypeId(int.Parse(lbSDocTypeID.Text),"");
            ddlSDoc.DataTextField = "DocName";
            ddlSDoc.DataValueField = "ID";
            ddlSDoc.DataBind();
            ddlSDoc.Items.Insert(0, new ListItem("--请选择--", "0"));
        }
        protected void ddlSDoc_SelectedIndexChanged(object sender, EventArgs e)
        {
            Bind_ddlSDocAttr();
        }
        protected void Bind_ddlSDocAttr()
        {
            ddlSDocAttr.Items.Clear();
            ddlSDocAttr.DataSource = DB.getAttrsByDocID(int.Parse(ddlSDoc.SelectedValue));
            ddlSDocAttr.DataTextField = "AttrName";
            ddlSDocAttr.DataValueField = "ID";
            ddlSDocAttr.DataBind();
            ddlSDocAttr.Items.Insert(0, new ListItem("--请选择--", "0"));
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            int rdocID = int.Parse(ddlRDoc.SelectedValue);
            int rattrID = int.Parse(ddlRDocAttr.SelectedValue);
            int sdocID = int.Parse(ddlSDoc.SelectedValue);
            int sattrID = int.Parse(ddlSDocAttr.SelectedValue);
            if ((rdocID == 0) || (sdocID == 0))
            {
                plMsg.Visible = true;
                lbMsg.Text = "<font color='red'>请选择文档对象的属性!</font>";
                return;
            }
            if ((rattrID == 0) || (sattrID == 0))
            {
                plMsg.Visible = true;
                lbMsg.Text = "<font color='red'>请选择文档对象的属性!</font>";
                return;
            }
            if (rdocID == sdocID)
            {
                plMsg.Visible = true;
                lbMsg.Text = "<font color='red'>引用文档对象不能与源文档对象相同!</font>";
                return;
            }
            if (!DB.isRelationValid(rdocID, rattrID, sdocID, sattrID))
            {
                plMsg.Visible = true;
                lbMsg.Text = "<font color='red'>文档对象间已存在关联!</font>";
                return;
            }
            if (DB.AddRelation(rdocID, rattrID, sdocID, sattrID) == -1)
            {
                plMsg.Visible = true;
                lbMsg.Text = "<font color='red'>保存关联失败!</font>";
            }
            else
            {
                plSaveResult.Visible = true;
                plAdd.Visible = false;
            }
        }
    }
}