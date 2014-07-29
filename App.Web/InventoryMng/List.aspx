<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="List.aspx.cs" Inherits="App.Web.InventoryMng.List" %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="header">
        <h1 class="pagetitle" runat="server" id="lbTitle">
            临时清单对象列表</h1>
    </div>
    <asp:Panel ID="Panel1" runat="server" Width="100%">
        <br />
        <center>
            <table width="100%">
                <tr>
                    <td align="left">
                        <asp:Button ID="btnDel" runat="server" Text="删 除" Width="80px" OnClick="btnDel_Click"
                            OnClientClick="return confirm('确定要删除选定清单吗？');" />
                        <asp:Label ID="lbRemind" runat="server" Text="当前数据库中没有记录" ForeColor="red" Visible="false"></asp:Label>
                    </td>
                </tr>
            </table>
            <asp:GridView ID="myGridView" runat="server" Width="100%" DataKeyNames="ID" OnRowCommand="myGridView_RowCommand"
                OnRowDataBound="myGridView_RowDataBound" AllowSorting="True" OnSorting="myGridView_Sorting"
                AutoGenerateColumns="false" OnRowCreated="myGridView_RowCreated">
                <Columns>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            选择<asp:CheckBox ID="chkHeader" runat="server" OnClick="javascript: SelectAllCheckBoxInThisPageByID(this);">
                            </asp:CheckBox>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="chkSelect" runat="server"></asp:CheckBox>
                            <asp:Label ID="ID" runat="server" Text='<%#Eval("ID") %>' Visible="false"></asp:Label>
                        </ItemTemplate>
                        <ItemStyle Width="80px" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="序号">
                        <ItemTemplate>
                            <%#Container.DataItemIndex+1+ (AspNetPager1.CurrentPageIndex-1)*AspNetPager1.PageSize %></ItemTemplate>
                        <ItemStyle Width="50px"></ItemStyle>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="清单类别" SortExpression="TypeID">
                        <ItemTemplate>
                            <%#App.Dll.InventoryMethod.BusinessLayer.GetTypeNameByID((Container.DataItem as App.Model.Inventory).Type.ID)%></ItemTemplate>
                        <ItemStyle Width="100px"></ItemStyle>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="清单名称" SortExpression="Name">
                        <ItemTemplate>
                            <asp:HyperLink runat="server" ID="hlName"></asp:HyperLink>
                            <%--<asp:HyperLink runat="server" ID="hlName" NavigateUrl='<%#"Profile.aspx?rid="+Eval("ID")%>'
                                Text='<%#Eval("Name")%>'></asp:HyperLink>--%>
                        </ItemTemplate>
                        <ItemStyle Width="100px"></ItemStyle>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Name" HeaderText="清单名称" SortExpression="Name" ItemStyle-Width="100px" />
                    <asp:BoundField DataField="Description" HeaderText="描述" SortExpression="Description"
                        ItemStyle-Width="120px" />
                    <asp:TemplateField HeaderText="清单文档">
                        <ItemTemplate>
                            <%#App.Dll.InventoryMethod.BusinessLayer.GetDocNamesByInventory(Container.DataItem as App.Model.Inventory)%></ItemTemplate>
                    </asp:TemplateField>
                    <asp:ButtonField CommandName="ChooseEdit" HeaderText="编辑" Text="编辑">
                        <ItemStyle Width="50px" />
                    </asp:ButtonField>
                </Columns>
            </asp:GridView>
            <webdiyer:AspNetPager ID="AspNetPager1" runat="server" Width="100%" PageSize="20"
                OnPageChanged="AspNetPager1_PageChanged" ShowPrevNext="true" ShowFirstLast="true"
                ShowInputBox="Always" ShowPageIndex="false" PrevPageText="上一页" NextPageText="下一页"
                FirstPageText="首页" LastPageText="末页" HorizontalAlign="Right" Font-Size="10pt"
                AlwaysShow="true" ShowCustomInfoSection="Left" CustomInfoHTML="<table width='100%'> <tr><td> 共<b style='color:red'>%RecordCount%</b>条记录</td> <td> 第<font color='red'><b>%CurrentPageIndex%</b></font>/%PageCount%页</td> <td>  第%StartRecordIndex% -%EndRecordIndex% 条记录</td></tr>   </table>">
            </webdiyer:AspNetPager>
            <asp:Label ID="Label1" runat="server" Text="Label" Visible="false"></asp:Label>
        </center>
    </asp:Panel>
</asp:Content>
