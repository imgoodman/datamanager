using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using App.Dll;

namespace App.Web.DocConfig
{
    public partial class AttrRelationList : System.Web.UI.Page
    {
        DBGridViewList DB = new DBGridViewList();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["orderByName"] = "ID";
                ViewState["orderByType"] = "asc";
                Bind_GridView1("1=1");
                lbRDocTypeID.Text = RDocTypeSelect.SelectedID = "0";
                RDocTypeSelect.SelectedName = "选择引用文档对象的类别";
                Bind_ddlRDoc();
                lbSDocTypeID.Text = SDocTypeSelect.SelectedID = "0";
                SDocTypeSelect.SelectedName = "选择源文档对象的类别";
                Bind_ddlSDoc();
            }
            plMsg.Visible = false;
        }

        protected void Bind_GridView1(string searchStr)
        {
            string orderByName = ViewState["orderByName"].ToString();
            string orderByType = ViewState["orderByType"].ToString();
            string sqlwhere = searchStr;
            int recordCount = DB.GetRecordCount(sqlwhere);
            AspNetPager1.RecordCount = recordCount;
            GridView1.DataSource = DB.GetPagedData(sqlwhere, orderByName + " " + orderByType, AspNetPager1.CurrentPageIndex, AspNetPager1.PageSize);
            GridView1.DataBind();
            if (recordCount == 0)
            {
                plBottom.Visible = false;
            }
            else
            {
                plBottom.Visible = true;
            }
        }
        protected void GridView1_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.setViewState(e.SortExpression, this.ViewState["orderByType"].ToString() == "asc" ? "desc" : "asc");
            Bind_GridView1("1=1");
        }
        private void setViewState(string orderByName, string orderByType)
        {
            ViewState["orderByName"] = orderByName;
            ViewState["orderByType"] = orderByType;
        }
        protected void GridView1_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowIndex == -1)
            {
                for (int i = 0; i < GridView1.Columns.Count; i++)
                {
                    if (GridView1.Columns[i].SortExpression == ViewState["orderByName"].ToString())
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
        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            foreach (TableCell tc in e.Row.Cells)
            {
                tc.Attributes["class"] = "gvTd";
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["onmouseover"] = "javascript:c=this.style.backgroundColor;this.style.background='" + "#C8DDF3" + "'";
                e.Row.Attributes["onmouseout"] = "javascript:this.style.background=c";
            }
        }
        protected void AspNetPager1_PageChanged(object sender, EventArgs e)
        {
            Bind_GridView1("1=1");
        }
        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //int rowIndex = -1;
            //GridViewRow row = null;
            //switch (e.CommandName)
            //{
            //    case "ChooseEdit":
            //        rowIndex = Convert.ToInt32(e.CommandArgument);
            //        row = GridView1.Rows[rowIndex];
            //        string ID = GridView1.DataKeys[row.RowIndex].Value.ToString();
            //        Response.Redirect("/DocConfig/AttrRelate.aspx?ID=" + ID);
            //        break;
            //    default:
            //        break;
            //}
        }
        protected void btnDel_Click(object sender, EventArgs e)
        {
            string dataIDAll = "";
            for (int i = 0; i < GridView1.Rows.Count; i++)
            {
                CheckBox cb = (CheckBox)GridView1.Rows[i].FindControl("chkSelect");
                if (cb.Checked)
                {
                    dataIDAll += GridView1.DataKeys[i].Value.ToString() + ",";
                }
            }
            if (dataIDAll != "")
            {
                dataIDAll = dataIDAll.Remove(dataIDAll.Length - 1);
                if (DB.DeleteObjs(dataIDAll) == 0)
                {
                    Bind_GridView1("1=1");
                    plMsg.Visible = true;
                    lbMsg.Text = "<font color='red'>删除成功!</font>";
                }
                else
                {
                    plMsg.Visible = true;
                    lbMsg.Text = "<font color='red'>删除失败!</font>";
                }
            }
            else
            {
                plMsg.Visible = true;
                lbMsg.Text = "<font color='red'>没有选中任何项，请选择删除项!</font>";
            }
        }
        /// <summary>
        /// 搜索控件相关
        /// </summary>
        protected void RDocType_Selected(object sender, EventArgs e)
        {
            lbRDocTypeID.Text = RDocTypeSelect.SelectedID;
            Bind_ddlRDoc();
        }
        public void Bind_ddlRDoc()
        {
            App.Dll.DocConfigService DB = new DocConfigService();
            ddlRDoc.Items.Clear();
            ddlRDoc.DataSource = DB.getDocsByTypeId(int.Parse(lbRDocTypeID.Text),"");
            ddlRDoc.DataTextField = "DocName";
            ddlRDoc.DataValueField = "ID";
            ddlRDoc.DataBind();
            ddlRDoc.Items.Insert(0, new ListItem("--选择引用文档对象--", "0"));
        }
        protected void SDocType_Selected(object sender, EventArgs e)
        {
            lbSDocTypeID.Text = SDocTypeSelect.SelectedID;
            Bind_ddlSDoc();
        }
        public void Bind_ddlSDoc()
        {
            App.Dll.DocConfigService DB = new DocConfigService();
            ddlSDoc.Items.Clear();
            ddlSDoc.DataSource = DB.getDocsByTypeId(int.Parse(lbSDocTypeID.Text),"");
            ddlSDoc.DataTextField = "DocName";
            ddlSDoc.DataValueField = "ID";
            ddlSDoc.DataBind();
            ddlSDoc.Items.Insert(0, new ListItem("--选择源文档对象--", "0"));
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string dueSearch = "1=1";
            if (ddlRDoc.SelectedIndex != 0)
            {
                dueSearch += " and RDocID =" + ddlRDoc.SelectedValue;
            }
            if (ddlSDoc.SelectedIndex != 0)
            {
                dueSearch += " and SDocID =" + ddlSDoc.SelectedValue;
            }
            Bind_GridView1(dueSearch);
        }
        protected void btnShowAll_Click(object sender, EventArgs e)
        {
            Response.Redirect("/DocConfig/AttrRelationList.aspx");
        }

        public class DBGridViewList
        {
            BaseClass bc = new BaseClass();
            public int GetRecordCount(string sqlWhere)
            {
                int recordCount = bc.intScalar("select count(0) from DocAttrRelate_Tab where " + sqlWhere);
                return recordCount;
            }
            public DataTable GetPagedData(string sqlWhere, string orderStr, int pageIndex, int pageSize)
            {
                int aa = (pageIndex - 1) * pageSize;
                DataTable dt = BaseClass.mydataset("select top " + pageSize + " * from DocAttrRelate_Tab where " + sqlWhere + " and ID not in (select top " + aa + " ID from DocAttrRelate_Tab where " + sqlWhere + " order by " + orderStr + ") order by " + orderStr).Tables[0];
                dt.Columns.Add("RDocName");
                dt.Columns.Add("RAttrName");
                dt.Columns.Add("SDocName");
                dt.Columns.Add("SAttrName");
                foreach (DataRow dr in dt.Rows)
                {
                    dr["RDocName"] = bc.ecScalar("select DocName from Document where ID='" + dr["RDocID"].ToString() + "'");
                    dr["RAttrName"] = bc.ecScalar("select AttrName from DocumentAttr where ID='" + dr["RAttrID"].ToString() + "'");
                    dr["SDocName"] = bc.ecScalar("select DocName from Document where ID='" + dr["SDocID"].ToString() + "'");
                    dr["SAttrName"] = bc.ecScalar("select AttrName from DocumentAttr where ID='" + dr["SAttrID"].ToString() + "'");
                }
                return dt;
            }
            public int DeleteObjs(string ids)
            {
                return bc.RunSqlTransaction("delete from DocAttrRelate_Tab where ID in (" + ids + ")");
            }
        }
    }
}