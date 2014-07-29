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
    public partial class GridViewList : System.Web.UI.Page
    {
        DBGridViewList DB = new DBGridViewList();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["orderByName"] = "ID";
                ViewState["orderByType"] = "asc";
                Bind_ddlType();
                Bind_GridView1("1=1");
            }
        }

        protected void Bind_GridView1(string searchStr)
        {
            string orderByName = ViewState["orderByName"].ToString();
            string orderByType = ViewState["orderByType"].ToString();
            string sqlwhere = searchStr + " and IsExpired='false'";
            int recordCount = DB.GetRecordCount(sqlwhere);
            AspNetPager1.RecordCount = recordCount;
            GridView1.DataSource = DB.GetPagedData(sqlwhere, orderByName + " " + orderByType, AspNetPager1.CurrentPageIndex, AspNetPager1.PageSize);
            GridView1.DataBind();
            if (recordCount == 0)
            {
                AspNetPager1.Visible = false;
            }
            else
            {
                AspNetPager1.Visible = true;
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
        /// <summary>
        /// 搜索控件相关
        /// </summary>
        public void Bind_ddlType()
        {
            App.Dll.SysMethod.DBDocTypeManager DB = new Dll.SysMethod.DBDocTypeManager();
            ddlType.Items.Clear();
            ddlType.DataSource = DB.getTypes(2);
            ddlType.DataTextField = "TypeName";
            ddlType.DataValueField = "ID";
            ListItem choose = new ListItem();
            choose.Text = "--请选择所属类别--";
            choose.Value = "0";
            ddlType.Items.Add(choose);
            ddlType.DataBind();
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string dueSearch = "1=1";
            if (ddlType.SelectedIndex != 0)
            {
                dueSearch += " and TypeID =" + ddlType.SelectedValue.ToString();
            }
            if (txtDateFrom.Text != "")
            {
                dueSearch += " and CreateTime>='" + txtDateFrom.Text + "'";
            }
            if (txtDateTo.Text != "")
            {
                dueSearch += " and CreateTime<='" + txtDateTo.Text + "'";
            }
            if (txtfuzzyProjName.Text != "")
            {
                dueSearch += " and DocName like '%" + txtfuzzyProjName.Text + "%'";
            }
            Bind_GridView1(dueSearch);
        }
        protected void btnShowAll_Click(object sender, EventArgs e)
        {
            Bind_GridView1("1=1");
        }

        public class DBGridViewList
        {
            BaseClass bc = new BaseClass();
            public int GetRecordCount(string sqlWhere)
            {
                int recordCount = bc.intScalar("select count(0) from Document where " + sqlWhere);
                return recordCount;
            }
            public DataTable GetPagedData(string sqlWhere, string orderStr, int pageIndex, int pageSize)
            {
                int aa = (pageIndex - 1) * pageSize;
                DataTable dt = BaseClass.mydataset("select top " + pageSize + " * from Document where " + sqlWhere + " and ID not in (select top " + aa + " ID from Document where " + sqlWhere + " order by " + orderStr + ") order by " + orderStr).Tables[0];

                dt.Columns.Add("Attrs");
                string attrs = string.Empty;
                DataTable dtAttr = null;

                dt.Columns.Add("FatherTypeName");
                int topfathertypeID = 0;
                dt.Columns.Add("TopFatherTypeName");
                dt.Columns.Add("TopFatherTypeID");

                dt.Columns.Add("Num");
                int i = 0;

                foreach (DataRow dr in dt.Rows)
                {
                    attrs = "";
                    dtAttr = BaseClass.mydataset("select AttrName from DocumentAttr where DocID='" + dr["ID"].ToString() + "' and IsExpired='false' order by ID asc").Tables[0];
                    foreach (DataRow drAttr in dtAttr.Rows)
                    {
                        attrs += drAttr["AttrName"] + ",";
                    }
                    if (!string.IsNullOrEmpty(attrs))
                    {
                        attrs = attrs.Remove(attrs.Length - 1);
                    }
                    dr["Attrs"] = attrs;

                    dr["FatherTypeName"] = bc.ecScalar("select TypeName from DocumentType_Tab where ID='" + dr["TypeID"].ToString() + "' and State<>'-1'");
                    topfathertypeID = GetTopFatherDocTypeID(int.Parse(dr["TypeID"].ToString()));
                    dr["TopFatherTypeID"] = topfathertypeID.ToString();
                    dr["TopFatherTypeName"] = bc.ecScalar("select TypeName from DocumentType_Tab where ID='" + topfathertypeID + "' and State<>'-1'");

                    i = i + 1;
                    dr["Num"] = i.ToString();
                }
                return dt;
            }
            private int GetTopFatherDocTypeID(int doctypeID)
            {
                int pid = bc.intScalar("select FatherTypeID from DocumentType_Tab where ID='" + doctypeID + "'");
                if (pid == 0)
                    return doctypeID;
                else
                    return GetTopFatherDocTypeID(pid);
            }
        }
    }
}