using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using App.Dll;
using App.Model;

namespace App.Web.Controls
{
    public partial class InventoryProfile : System.Web.UI.UserControl
    {
        App.Dll.InventoryMethod.BusinessLayer bl = new App.Dll.InventoryMethod.BusinessLayer();
        App.Dll.InventoryMethod.DBInventoryType it = new App.Dll.InventoryMethod.DBInventoryType();
        App.Dll.DocConfigService dcs = new DocConfigService();

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        private Inventory _inventoryObject;

        public Inventory InventoryObject
        {
            get { return _inventoryObject; }
            set { _inventoryObject = value; }
        }

        public void Bind()
        {
            lbType.Text = it.GetItemByID(_inventoryObject.Type.ID).Name;
            lbName.Text = _inventoryObject.Name;
            lbDescription.Text = _inventoryObject.Description;
            Bind_RepeaterList();
        }

        private void Bind_RepeaterList()
        {
            if (_inventoryObject.Docs != null)
            {
                DataSet ds = new DataSet();
                DataTable tb = new DataTable("Table");
                ds.Tables.Add(tb);
                DataColumn col1 = tb.Columns.Add("DocName", typeof(string));
                DataColumn col2 = tb.Columns.Add("AttrNames", typeof(string));

                for (int i = 0; i < _inventoryObject.Docs.Count(); i++)
                {
                    DataRow row = ds.Tables["Table"].NewRow();
                    row["DocName"] = dcs.getDocumentById(_inventoryObject.Docs[i].ID).DocName;
                    row["AttrNames"] = bl.GetAttrNamesByDoc(_inventoryObject.Docs[i]);
                    ds.Tables["Table"].Rows.Add(row);
                }
                RepeaterList.DataSource = ds;
                RepeaterList.DataBind();
            }
        }
    }
}