using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Web;
using App.Model;

namespace App.Dll.DocMethod
{
    public class DocRedundantCheck
    {
        BaseClass bc = new BaseClass();
        DocService ds = new DocService();

        public DataTable GetDocRedundantDt(int docID)
        {
            DataTable attrDt = BaseClass.mydataset("select * from DocumentAttr where DocID ='" + docID + "' and IsExpired='false' and IsRepeat='false'").Tables[0];
            if (attrDt.Rows.Count == 0)
                return null;

            string instIDReduStr = string.Empty;
            foreach (DataRow instDr in BaseClass.mydataset("select ID from DocInstance where DocID ='" + docID + "' and IsDeleted='false' order by ID asc").Tables[0].Rows)
            {
                bool IsFound = false;
                if (!string.IsNullOrEmpty(instIDReduStr))
                {
                    string[] instIDRedulist = instIDReduStr.Remove(instIDReduStr.Length - 1).Split(',');
                    foreach (string id in instIDRedulist)
                    {
                        if (instDr["ID"].ToString() == id)
                        {
                            IsFound = true;
                            break;
                        }
                    }
                }

                if (!IsFound)
                {
                    int instID = instDr["ID"].ToInt();
                    string atType = string.Empty;
                    int atID = 0;
                    string atValue = string.Empty;
                    foreach (DataRow atDr in attrDt.Rows)
                    {
                        switch (atDr["AttrType"].ToString())
                        {
                            case "Person":
                                atType = "PersonAttr";
                                break;
                            case "MultiPerson":
                                atType = "MultiPersonAttr";
                                break;
                            case "Date":
                                atType = "DateAttr";
                                break;
                            case "EnumVal":
                                atType = "EnumAttr";
                                break;
                            case "RichText":
                                atType = "RichTextAttr";
                                break;
                            case "File":
                                atType = "";
                                break;
                            case "Text":
                                atType = "Attr";
                                break;
                            default:
                                atType = "";
                                break;
                        }
                        if (!string.IsNullOrEmpty(atType))
                        {
                            atID = atDr["ID"].ToInt();
                            atValue = bc.ecScalar("select Value from docinstance" + atType + " where DocInstanceID = '" + instID + "' and DocAttrID='" + atID + "' and isdeleted='false'");
                            if (!string.IsNullOrEmpty(atValue))
                            {
                                string temp = "0";
                                if (!string.IsNullOrEmpty(instIDReduStr))
                                    temp = instIDReduStr.Remove(instIDReduStr.Length - 1);
                                foreach (DataRow instIDReduDr in BaseClass.mydataset("select DocInstanceID from docinstance" + atType + " a where DocInstanceID<>'" + instID + "' and DocInstanceID not in (" + temp + ") and DocAttrID='" + atID + "' and value='" + atValue + "' and isdeleted='false' and exists(select null from DocInstance b where b.ID = a.docinstanceid and IsDeleted='false')").Tables[0].Rows)
                                {
                                    instIDReduStr += instIDReduDr["DocInstanceID"].ToString() + ",";
                                }
                            }
                        }
                    }
                }
            }
            if (!string.IsNullOrEmpty(instIDReduStr))
            {
                DataTable dataforGv = new DataTable();
                dataforGv.Columns.Add("ID");
                dataforGv.Columns.Add("Name");

                instIDReduStr = instIDReduStr.Remove(instIDReduStr.Length - 1);
                string[] idlist = instIDReduStr.Split(',');
                foreach (var id in idlist)
                {
                    string attrStr = string.Empty;
                    foreach (var attr in ds.getDocInstanceById(id.ToInt()).Document.Attrs)
                    {
                        attrStr += attr.AttrName + ": " + attr.TranValue + "; ";
                    }
                    if (!string.IsNullOrEmpty(attrStr))
                    {
                        attrStr = attrStr.Remove(attrStr.Length - 1);
                    }

                    DataRow drforGv = dataforGv.NewRow();
                    drforGv["ID"] = id;
                    drforGv["Name"] = attrStr;
                    dataforGv.Rows.Add(drforGv);
                }
                return dataforGv;
            }
            else
                return null;
        }

        public string DeleteDocInstanceRedu(string idStr, int uid) 
        {
            string msg = "删除成功";
            string[] ids = idStr.Split(',');
            foreach (var id in ids) 
            {
                if (!ds.deleteDocInstance(id.ToInt(), uid)) 
                {
                    msg = "删除失败";
                    break;
                }
            }
            return msg;
        }
    }
}
