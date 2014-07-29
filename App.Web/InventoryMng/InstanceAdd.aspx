<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="InstanceAdd.aspx.cs" Inherits="App.Web.InventoryMng.InstanceAdd"
    EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        function openDia(id, ivtinstanceid, inventoryid, instancetranids) {
            //var returned = window.showmodaldialog("DocInstancesChoose.aspx?rid=" + id + "&instanceids=" + instanceids, null, 'dialogHeight:500px;dialogWidth:800px;status:0;help:0 ');
            window.open("DocInstancesChoose.aspx?rid=" + id + "&ivtinstanceid=" + ivtinstanceid + "&inventoryid=" + inventoryid + "&instancetranids=" + instancetranids, '选择文档实例', 'height=500px, width=800px, alwaysRaised =yes, top=0, left=0, toolbar=no, menubar=no, scrollbars=yes, resizable=yes, location=no, status=no');
        }   
    </script>
    <div class="header">
        <h1 class="pagetitle">
            添加清单实例</h1>
    </div>
    <p class="message warning hidden">
        <span class="ui-icon ui-icon-info" style="float: left; margin-right: .3em;"></span>
        <strong>Hey!</strong></p>
    <div id="formContent">
        <fieldset class="form detailinfo relative">
            <div class="control_group">
                <label class="control_label">
                    选择清单对象</label>
                <div class="controls">
                    <asp:DropDownList ID="ddlInventory" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlInventory_SelectedIndexChanged">
                    </asp:DropDownList>
                </div>
            </div>
            <legend>清单基本信息</legend>
            <div class="control_group">
                <label class="control_label">
                    清单类别</label>
                <div class="controls">
                    <asp:TextBox ID="txtID" runat="server" Visible="false"></asp:TextBox>
                    &nbsp;
                </div>
            </div>
            <div class="control_group">
                <label class="control_label">
                    清单名称</label>
                <div class="controls">
                    <asp:Label ID="lbName" runat="server" Text=""></asp:Label>&nbsp;
                </div>
            </div>
            <div class="control_group">
                <label class="control_label">
                    描述</label>
                <div class="controls">
                    <asp:Label ID="lbDescription" runat="server" Text=""></asp:Label>&nbsp;
                </div>
            </div>
        </fieldset>
        <fieldset class="form">
            <legend>添加清单的文档实例</legend>
            <asp:Panel ID="Panel1" runat="server" Width="100%">
                <br />
                <center>
                    <asp:GridView ID="myGridView" runat="server" Width="100%" DataKeyNames="ID" OnRowCommand="myGridView_RowCommand"
                        OnRowDataBound="myGridView_RowDataBound" AllowSorting="True" OnSorting="myGridView_Sorting"
                        AutoGenerateColumns="false" OnRowCreated="myGridView_RowCreated">
                        <Columns>
                            <asp:TemplateField HeaderText="序号">
                                <ItemTemplate>
                                    <%#Container.DataItemIndex+1 %></ItemTemplate>
                                <ItemStyle Width="50px"></ItemStyle>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="文档对象">
                                <ItemTemplate>
                                    <%#Eval("DocName")%></ItemTemplate>
                                <ItemStyle Width="100px"></ItemStyle>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="已添加文档实例的属性">
                                <ItemTemplate>
                                    <%#Eval("DocInstanceNames")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="操 作">
                                <ItemTemplate>
                                    <asp:Button ID="btnAdd" runat="server" Text="添加文档实例" class="btn btn_primary save"
                                        OnClientClick=' <%# "openDia("+Eval("DocID")+","+Eval("ID")+","+Eval("InventoryID")+","+Eval("DocInstanceTranIDs")+")" %>' />
                                </ItemTemplate>
                                <ItemStyle Width="100px"></ItemStyle>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </center>
            </asp:Panel>
        </fieldset>
        <div class="form_actions" style="float: right">
            <asp:Button ID="btnSave" runat="server" Text="保 存" class="btn btn_primary btn_large save"
                OnClick="btnSave_Click" Visible="false" />
        </div>
    </div>
</asp:Content>
