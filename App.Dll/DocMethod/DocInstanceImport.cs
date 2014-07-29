using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Web;
using Microsoft.Office.Interop.Excel;
using App.Model;

namespace App.Dll.DocMethod
{
    public class DocInstanceImport
    {
        BaseClass bc = new BaseClass();
        DocConfigService DBDoc = new DocConfigService();
        App.Dll.SysMethod.DBUserManager DBU = new SysMethod.DBUserManager();

        public System.Data.DataTable getDocAttrColNameDt(string filePath, int docID)
        {
            //引用Excel Application類別
            _Application myExcel = null;

            //引用活頁簿類別 
            _Workbook myBook = null;

            //引用工作表類別
            _Worksheet mySheet = null;

            //引用Range類別 
            Range myRange = null;

            myExcel = new Application();

            myExcel.Workbooks.Open(filePath, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            myExcel.DisplayAlerts = false;
            myExcel.Visible = false;

            myBook = myExcel.Workbooks[1];
            myBook.Activate();

            mySheet = (_Worksheet)myBook.Worksheets[1];
            //mySheet.Name = "hello";
            mySheet.Activate();

            Document doc = DBDoc.getDocumentById(docID);
            List<DocumentAttr> attrs = doc.Attrs;
            int i = 1;
            string colName = string.Empty;
            System.Data.DataTable dt = new System.Data.DataTable();
            dt.Columns.Add("DocAttrName");
            dt.Columns.Add("ColName");

            while (!string.IsNullOrEmpty(mySheet.Cells[1, i].Text.ToString()))
            {
                System.Data.DataRow dr = dt.NewRow();
                colName = mySheet.Cells[1, i].Text.ToString();
                dr["ColName"] = colName;

                int matchedattrNum = 0;
                int index = 0;
                for (index = 0; index < attrs.Count; index++)
                {
                    if (attrs[index].AttrName == colName)
                    {
                        matchedattrNum++;
                        dr["DocAttrName"] = attrs[index].AttrName;
                        break;
                    }
                }
                if (matchedattrNum == 0)
                    dr["DocAttrName"] = "<font color='red'>无匹配属性</font>";
                dt.Rows.Add(dr);
                i++;
            }
            if (i <= attrs.Count)
            {
                dt.Rows.Clear();
                for (int index = 0; index < attrs.Count; index++)
                {
                    System.Data.DataRow dr = dt.NewRow();
                    dr["DocAttrName"] = attrs[index].AttrName;
                    int matchedattrNum = 0;
                    int j = 1;

                    while (!string.IsNullOrEmpty(mySheet.Cells[1, j].Text.ToString()))
                    {
                        colName = mySheet.Cells[1, j].Text.ToString();
                        j++;
                        if (attrs[index].AttrName == colName)
                        {
                            matchedattrNum++;
                            dr["ColName"] = colName;
                            break;
                        }
                    }
                    if (matchedattrNum == 0)
                        dr["ColName"] = "<font color='red'>无匹配属性</font>";
                    dt.Rows.Add(dr);
                }
            }
            //關閉活頁簿
            myBook.Close(false, Type.Missing, Type.Missing);
            //關閉Excel
            myExcel.Quit();
            //釋放Excel資源
            System.Runtime.InteropServices.Marshal.ReleaseComObject(myExcel);
            myBook = null;
            mySheet = null;
            myRange = null;
            myExcel = null;
            GC.Collect();

            return dt;
        }

        public bool CheckTemplate(string filePath, int docID)
        {
            //引用Excel Application類別
            _Application myExcel = null;

            //引用活頁簿類別 
            _Workbook myBook = null;

            //引用工作表類別
            _Worksheet mySheet = null;

            //引用Range類別 
            Range myRange = null;

            myExcel = new Application();

            myExcel.Workbooks.Open(filePath, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            myExcel.DisplayAlerts = false;
            myExcel.Visible = false;

            myBook = myExcel.Workbooks[1];
            myBook.Activate();

            mySheet = (_Worksheet)myBook.Worksheets[1];
            //mySheet.Name = "hello";
            mySheet.Activate();

            bool IsValid = true;
            Document doc = DBDoc.getDocumentById(docID);
            List<DocumentAttr> attrs = doc.Attrs;
            int i = 1;
            string colName = string.Empty;

            while (!string.IsNullOrEmpty(mySheet.Cells[1, i].Text.ToString()))
            {
                colName = mySheet.Cells[1, i].Text.ToString();

                int matchedattrNum = 0;
                int index = 0;
                for (index = 0; index < attrs.Count; index++)
                {
                    if (attrs[index].AttrName == colName)
                    {
                        matchedattrNum++;
                        break;
                    }
                }
                if (matchedattrNum == 0)
                {
                    IsValid = false;
                }
                i++;
            }
            if (i <= attrs.Count)
            {
                for (int index = 0; index < attrs.Count; index++)
                {
                    int matchedattrNum = 0;
                    int j = 1;

                    while (!string.IsNullOrEmpty(mySheet.Cells[1, j].Text.ToString()))
                    {
                        colName = mySheet.Cells[1, j].Text.ToString();
                        j++;
                        if (attrs[index].AttrName == colName)
                        {
                            matchedattrNum++;
                            break;
                        }
                    }
                    if (matchedattrNum == 0)
                    {
                        IsValid = false;
                    }
                }
            }
            //關閉活頁簿
            myBook.Close(false, Type.Missing, Type.Missing);
            //關閉Excel
            myExcel.Quit();
            //釋放Excel資源
            System.Runtime.InteropServices.Marshal.ReleaseComObject(myExcel);
            myBook = null;
            mySheet = null;
            myRange = null;
            myExcel = null;
            GC.Collect();

            return IsValid;
        }

        public System.Data.DataTable DisplayDocInstanceFromFile(string filePath, int docID)
        {
            Document doc = DBDoc.getDocumentById(docID);
            List<DocumentAttr> attrs = doc.Attrs;
            int num = attrs.Count;

            //引用Excel Application類別
            _Application myExcel = null;

            //引用活頁簿類別 
            _Workbook myBook = null;

            //引用工作表類別
            _Worksheet mySheet = null;

            //引用Range類別 
            Range myRange = null;

            myExcel = new Application();

            myExcel.Workbooks.Open(filePath, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            myExcel.DisplayAlerts = false;
            myExcel.Visible = false;

            myBook = myExcel.Workbooks[1];
            myBook.Activate();

            mySheet = (_Worksheet)myBook.Worksheets[1];
            //mySheet.Name = "hello";
            mySheet.Activate();

            int crtRow = 2;
            int i = 0;
            DateTime time = DateTime.Now;

            System.Data.DataTable dt = new System.Data.DataTable();
            dt.Columns.Add("No");
            for (int kkk = 1; kkk <= num; kkk++)
            {
                dt.Columns.Add(mySheet.Cells[1, kkk].Text.ToString().Trim());
            }
            dt.Columns.Add("Note");

            while (!string.IsNullOrEmpty(mySheet.get_Range("A" + crtRow.ToString(), Type.Missing).Text.ToString()))
            {
                i++;

                string colStr = string.Empty;
                string colName = string.Empty;
                string attrvalue = string.Empty;
                string attrtype = string.Empty;
                string strRepeat = string.Empty;
                DateTime FormatTime = new DateTime();
                string errmsg = string.Empty;

                System.Data.DataRow dr = dt.NewRow();
                dr["No"] = crtRow.ToString();
                dr["Note"] = string.Empty;

                #region 遍历excel的各列
                for (int j = 1; j <= num; j++)
                {
                    bool IsValid = true;
                    bool IsIgnored = false;

                    colName = mySheet.Cells[1, j].Text.ToString().Trim();
                    int matchedattrNum = 0;
                    int index = 0;
                    for (index = 0; index < attrs.Count; index++)
                    {
                        if (attrs[index].AttrName == colName)
                        {
                            matchedattrNum++;
                            break;
                        }
                    }
                    if (matchedattrNum == 0)
                        return null;

                    attrvalue = mySheet.Cells[crtRow, j].Text.ToString().Trim();
                    dr[j] = attrvalue;

                    switch (attrs[index].AttrType)
                    {
                        case AttrType.Date:
                            attrtype = "dateattr ";
                            if (!string.IsNullOrEmpty(attrvalue))
                            {
                                if (!DateTime.TryParse(attrvalue, out FormatTime))
                                {
                                    IsIgnored = true;
                                    errmsg += "日期格式不正确,";
                                }
                            }
                            else
                            {
                                IsValid = false;
                            }
                            break;
                        case AttrType.File:
                            attrtype = "fileattr";
                            attrvalue = string.Empty;
                            break;
                        case AttrType.MultiPerson:
                            attrtype = "multipersonattr";
                            int muid = 0;
                            string uids = string.Empty;
                            foreach (string username in attrvalue.Split(','))
                            {
                                muid = DBU.GetUserIDByUserName(username);
                                if (muid != 0)
                                    uids += muid.ToString() + ",";
                                else
                                {
                                    IsIgnored = true;
                                    errmsg += "人员不存在,";
                                    break;
                                }
                            }
                            if (!string.IsNullOrEmpty(uids))
                            {
                                attrvalue = uids.Remove(uids.Length - 1);
                            }
                            else
                            {
                                IsValid = false;
                            }
                            break;
                        case AttrType.Person:
                            attrtype = "personattr";
                            if (!string.IsNullOrEmpty(attrvalue))
                            {
                                int uid = DBU.GetUserIDByUserName(attrvalue);
                                if (uid == 0)
                                {
                                    IsIgnored = true;
                                    errmsg += "人员不存在,";
                                }
                                else
                                    attrvalue = uid.ToString();
                            }
                            else
                            {
                                attrvalue = "0";
                                IsValid = false;
                            }
                            break;
                        case AttrType.RichText:
                            attrtype = "richtextattr";
                            break;
                        case AttrType.Text:
                            attrtype = "attr";
                            break;
                        case AttrType.EnumVal:
                            attrtype = "enumattr";
                            if (!string.IsNullOrEmpty(attrvalue))
                            {
                                string valueid = bc.ecScalar("select top 1 ID from DocumentAttrVal where DocAttrID='" + attrs[index].ID + "' and AttrValue='" + attrvalue + "' and IsExpired='false'");
                                if (string.IsNullOrEmpty(valueid))
                                {
                                    IsIgnored = true;
                                    errmsg += attrs[index].AttrName + "不存在,";
                                }
                                else
                                    attrvalue = valueid;
                            }
                            else
                            {
                                attrvalue = "0";
                                IsValid = false;
                            }
                            break;
                        default:
                            attrtype = "attr";
                            break;
                    }
                    if (IsValid)
                    {
                        if ((!attrs[index].IsRepeat) && (attrvalue != string.Empty))
                        {
                            strRepeat = "select count(ID) from docinstance" + attrtype + " a where value='" + attrvalue + "' and DocAttrID='" + attrs[index].ID + "' and isdeleted='false' and exists(select null from DocInstance b where b.ID = a.docinstanceid and IsDeleted='false')";
                            if (bc.intScalar(strRepeat) > 0)
                            {
                                IsIgnored = true;
                                errmsg += attrs[index].AttrName + "已存在,";
                            }
                        }
                    }
                }
                #endregion

                if (!string.IsNullOrEmpty(errmsg))
                {
                    errmsg = errmsg.Remove(errmsg.Length - 1);
                    dr["Note"] = "<font color='red'>" + errmsg + "</font>";
                }
                dt.Rows.Add(dr);
                crtRow++;
            }

            //關閉活頁簿
            myBook.Close(false, Type.Missing, Type.Missing);
            //關閉Excel
            myExcel.Quit();
            //釋放Excel資源
            System.Runtime.InteropServices.Marshal.ReleaseComObject(myExcel);
            myBook = null;
            mySheet = null;
            myRange = null;
            myExcel = null;
            GC.Collect();
            return dt;
        }

        public string ImportDocInstanceFromFile(string filePath, int docID)
        {
            Document doc = DBDoc.getDocumentById(docID);
            List<DocumentAttr> attrs = doc.Attrs;
            int num = attrs.Count;

            //引用Excel Application類別
            _Application myExcel = null;

            //引用活頁簿類別 
            _Workbook myBook = null;

            //引用工作表類別
            _Worksheet mySheet = null;

            //引用Range類別 
            Range myRange = null;

            myExcel = new Application();

            myExcel.Workbooks.Open(filePath, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            myExcel.DisplayAlerts = false;
            myExcel.Visible = false;

            myBook = myExcel.Workbooks[1];
            myBook.Activate();

            mySheet = (_Worksheet)myBook.Worksheets[1];
            //mySheet.Name = "hello";
            mySheet.Activate();

            int crtRow = 2;
            int i = 0;
            int userid = int.Parse(HttpContext.Current.Session["UserID"].ToString());
            DateTime time = DateTime.Now;

            while (!string.IsNullOrEmpty(mySheet.get_Range("A" + crtRow.ToString(), Type.Missing).Text.ToString()))
            {
                string sqlInsDocInsant = string.Empty;
                sqlInsDocInsant = "insert into docinstance (docid,creatorid,createtime,lastmodifierid,lastmodifytime,isdeleted) values (" + docID + "," + userid + ",'" + time + "'," + userid + ",'" + time + "','true')";
                int docinsantID = 0;
                if (bc.RunSqlTransaction(sqlInsDocInsant) == 0)
                    docinsantID = bc.intScalar("select top 1 ID from docinstance order by ID desc");
                else
                    return "新增第" + crtRow.ToString() + "行文档实例失败，请删除前面已经导入成功的数据，调整后进行操作！";

                string colStr = string.Empty;
                string attrvalue = string.Empty;
                string attrtype = string.Empty;
                string sqlInsAttr = string.Empty;
                string strRepeat = string.Empty;
                string colName = string.Empty;
                DateTime FormatTime = new DateTime();
                bool IsIgnored = false;

                #region 遍历excel的各列
                for (int j = 1; j <= num; j++)
                {
                    colName = mySheet.Cells[1, j].Text.ToString().Trim();
                    int matchedattrNum = 0;
                    int index = 0;
                    for (index = 0; index < attrs.Count; index++)
                    {
                        if (attrs[index].AttrName == colName)
                        {
                            matchedattrNum++;
                            break;
                        }
                    }
                    if (matchedattrNum == 0)
                        return "第" + j.ToString() + "列属性与所选文档对象的属性不匹配！";

                    bool IsValid = true;
                    //attrvalue = mySheet.get_Range(colStr + crtRow.ToString(), Type.Missing).Text.ToString().Trim();
                    attrvalue = mySheet.Cells[crtRow, j].Text.ToString().Trim();

                    switch (attrs[index].AttrType)
                    {
                        case AttrType.Date:
                            attrtype = "dateattr ";
                            if (!string.IsNullOrEmpty(attrvalue))
                            {
                                if (!DateTime.TryParse(attrvalue, out FormatTime))
                                    IsIgnored = true;
                                //return "第" + crtRow.ToString() + "行日期格式不正确，请删除前面已经导入成功的数据，调整后进行操作！";
                            }
                            else
                            {
                                IsValid = false;
                            }
                            break;
                        case AttrType.File:
                            attrtype = "fileattr";
                            attrvalue = string.Empty;
                            break;
                        case AttrType.MultiPerson:
                            attrtype = "multipersonattr";
                            int muid = 0;
                            string uids = string.Empty;
                            foreach (string username in attrvalue.Split(','))
                            {
                                muid = DBU.GetUserIDByUserName(username);
                                if (muid != 0)
                                    uids += muid.ToString() + ",";
                                else
                                {
                                    IsIgnored = true;
                                    break;
                                }
                                //return "第" + crtRow.ToString() + "行人员不存在，请删除前面已经导入成功的数据，调整后进行操作！";
                            }
                            if (!string.IsNullOrEmpty(uids))
                            {
                                attrvalue = uids.Remove(uids.Length - 1);
                            }
                            else
                            {
                                IsValid = false;
                            }
                            break;
                        case AttrType.Person:
                            attrtype = "personattr";
                            if (!string.IsNullOrEmpty(attrvalue))
                            {
                                int uid = DBU.GetUserIDByUserName(attrvalue);
                                if (uid == 0)
                                    IsIgnored = true;
                                //return "第" + crtRow.ToString() + "行人员不存在，请删除前面已经导入成功的数据，调整后进行操作！";
                                else
                                    attrvalue = uid.ToString();
                            }
                            else
                            {
                                attrvalue = "0";
                                IsValid = false;
                            }
                            break;
                        case AttrType.RichText:
                            attrtype = "richtextattr";
                            break;
                        case AttrType.Text:
                            attrtype = "attr";
                            break;
                        case AttrType.EnumVal:
                            attrtype = "enumattr";
                            if (!string.IsNullOrEmpty(attrvalue))
                            {
                                string valueid = bc.ecScalar("select top 1 ID from DocumentAttrVal where DocAttrID='" + attrs[index].ID + "' and AttrValue='" + attrvalue + "' and IsExpired='false'");
                                if (string.IsNullOrEmpty(valueid))
                                    IsIgnored = true;
                                //return "第" + crtRow.ToString() + "行" + attrs[index].AttrName + "不存在，请删除前面已经导入成功的数据，调整后进行操作！";
                                else
                                    attrvalue = valueid;
                            }
                            else
                            {
                                attrvalue = "0";
                                IsValid = false;
                            }
                            break;
                        default:
                            attrtype = "attr";
                            break;
                    }
                    if (IsValid)
                    {
                        if ((!attrs[index].IsRepeat) && (attrvalue != string.Empty))
                        {
                            strRepeat = "select count(ID) from docinstance" + attrtype + " a where value='" + attrvalue + "' and DocAttrID='" + attrs[index].ID + "' and isdeleted='false' and exists(select null from DocInstance b where b.ID = a.docinstanceid and IsDeleted='false')";
                            if (bc.intScalar(strRepeat) > 0)
                            {
                                IsIgnored = true;
                            }
                        }

                        sqlInsAttr += " insert into docinstance" + attrtype;
                        sqlInsAttr += "(docinstanceid,docattrid,value,creatorid,createtime,lastmodifierid,lastmodifytime,isdeleted) values (" + docinsantID + "," + attrs[index].ID + ",'" + attrvalue + "'," + userid + ",'" + time + "'," + userid + ",'" + time + "','false')";

                    }
                    else
                    {
                        sqlInsAttr += " insert into docinstance" + attrtype;
                        sqlInsAttr += "(docinstanceid,docattrid,creatorid,createtime,lastmodifierid,lastmodifytime,isdeleted) values (" + docinsantID + "," + attrs[index].ID + "," + userid + ",'" + time + "'," + userid + ",'" + time + "','false')";
                    }
                }
                #endregion

                sqlInsAttr += " update docinstance set IsDeleted='false' where ID ='" + docinsantID + "'";
                if (!IsIgnored)
                {
                    if (bc.RunSqlTransaction(sqlInsAttr) != 0)
                    {
                        return "第" + crtRow.ToString() + "行数据导入失败，请删除前面已经导入成功的数据，调整后进行操作！";
                    }
                    i++;
                }
                crtRow++;
            }

            //關閉活頁簿
            myBook.Close(false, Type.Missing, Type.Missing);
            //關閉Excel
            myExcel.Quit();
            //釋放Excel資源
            System.Runtime.InteropServices.Marshal.ReleaseComObject(myExcel);
            myBook = null;
            mySheet = null;
            myRange = null;
            myExcel = null;
            GC.Collect();
            return "导入成功，共导入" + i.ToString() + "条记录!";
        }
    }
}
