using System;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using App.Model;

namespace App.Dll.InventoryMethod
{
    public class BusinessLayer
    {
        BaseClass bc = new BaseClass();
        SysCommon SC = new SysCommon();
        DocConfigService dcs = new DocConfigService();
        DBInventoryMng im = new DBInventoryMng();
        DBInventoryInstanceMng iim = new DBInventoryInstanceMng();
        DocService ds = new DocService();


        /// <summary>
        /// 绑定文档名
        /// </summary>
        /// <param name="ddl"></param>
        public void Bind_ddlDocsName(DropDownList ddl)
        {
            ddl.Items.Clear();
            var data = dcs.getBasicDocs();
            ddl.DataSource = data;
            ddl.DataTextField = "DocName";
            ddl.DataValueField = "ID";
            ddl.DataBind();
        }

        /// <summary>
        /// 绑定文档名
        /// </summary>
        /// <param name="ddl"></param>
        public void Bind_cblDocsName(CheckBoxList cbl)
        {
            cbl.Items.Clear();
            var data = dcs.getBasicDocs();
            cbl.DataSource = data;
            cbl.DataTextField = "DocName";
            cbl.DataValueField = "ID";
            cbl.DataBind();
        }

        /// <summary>
        /// 绑定清单
        /// </summary>
        /// <param name="ddl"></param>
        public void Bind_ddlInventoryName(DropDownList ddl)
        {
            ddl.Items.Clear();
            var data = im.GetAllBasicInventory(" IsExpired='0'", " ID");
            ddl.DataSource = data;
            ddl.DataTextField = "Name";
            ddl.DataValueField = "ID";
            ddl.DataBind();
        }

        /// <summary>
        /// 获得文档的属性名串
        /// </summary>
        /// <returns></returns>
        public string GetAttrNamesByDoc(Document doc)
        {
            string attrNames = null;
            if (doc.Attrs != null)
            {
                foreach (var attr in doc.Attrs)
                {
                    attrNames += bc.ecScalar("select AttrName from DocumentAttr where ID=" + attr.ID) + ",";
                }
            }
            if (attrNames != null)
                attrNames = attrNames.Remove(attrNames.Length - 1);
            return attrNames;
        }

        public List<DocumentInstance> GetInstancesByDocID(int id)
        {
            string sql = "select id from docinstance where isdeleted='false' and docid='" + id + "' order by id";
            DataTable dt = BaseClass.mydataset(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                List<DocumentInstance> r = new List<DocumentInstance>();
                foreach (DataRow dr in dt.Rows)
                {
                    r.Add(ds.getDocInstanceById(int.Parse(dr["id"].ToString())));
                }
                return r;
            }
            else
                return null;
        }

        public List<DocumentInstance> GetDocumentInstancesByIDs(string ids)
        {
            if (!string.IsNullOrEmpty(ids))
            {
                List<DocumentInstance> r = new List<DocumentInstance>();
                foreach (var id in ids.Split(','))
                {
                    r.Add(ds.getDocInstanceById(id.ToInt()));
                }
                return r;
            }
            else
                return null;
        }

        public string GetInstanceAttrNamesByID(DocumentInstance docInstance)
        {
            string attrNames = null;
            if (docInstance != null)
            {
                foreach (var attr in docInstance.Document.Attrs)
                {
                    if (attr.IsRequired || attr.IsSearch)
                    {
                        string attrName = null;//一行属性值
                        attrName += attr.AttrName + "：";
                        attrName += attr.TranValue;
                        attrName += "<br />";//换行
                        attrNames += attrName;
                    }
                }
            }
            return attrNames;
        }

        #region 静态方法
        /// <summary>
        /// 根据ID得到清单类型名称
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string GetTypeNameByID(int id)
        {
            if (id <= 0)
                return string.Empty;
            BaseClass bc = new BaseClass();
            return bc.ecScalar("select Name from InventoryType_Tab where State<>'-1' and ID=" + id);
        }

        /// <summary>
        /// 根据ID得到文档对象名称
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string GetDocNameByID(int id)
        {
            if (id <= 0)
                return string.Empty;
            BaseClass bc = new BaseClass();
            return bc.ecScalar("select DocName from Document where IsExpired='0' and ID=" + id);
        }

        /// <summary>
        /// 根据清单获得清单文档的名称串
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string GetDocNamesByInventory(Inventory i)
        {
            string DocNames = null;
            BaseClass bc = new BaseClass();
            if (i.Docs != null)
            {
                foreach (Document d in i.Docs)
                {
                    DocNames += bc.ecScalar("select DocName from Document where IsExpired='0' and ID=" + d.ID) + ",";
                }
            }
            if (DocNames != null)
                DocNames = DocNames.Remove(DocNames.Length - 1);
            return DocNames;
        }

        /// <summary>
        /// 得到清单对象的名称
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string GetInventoryNameByID(int id)
        {
            if (id <= 0)
                return string.Empty;
            BaseClass bc = new BaseClass();
            return bc.ecScalar("select Name from InventoryBasic_Tab where IsExpired='0' and ID=" + id);
        }

        #endregion



    }
}
