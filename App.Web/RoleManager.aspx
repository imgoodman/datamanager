<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="RoleManager.aspx.cs" Inherits="App.Web.RoleManager" %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Panel ID="plMsg" runat="server" Width="100%" Visible="false">
        <center>
            <asp:Label ID="lbMsg" runat="server" Text=""></asp:Label></center>
    </asp:Panel>
    <asp:Panel ID="PanelAdd" runat="server" Width="100%">
        <p class="titleRow">
            <img alt="" class="NaviImage" src="Img/Add.jpg" /><b>添加角色</b></p>
        <br />
        <center>
            <table class="tableBorder" cellspacing="0" cellpadding="3">
                <tr>
                    <td class="colortd">
                        角色名称
                    </td>
                    <td class="contenttd">
                        <asp:TextBox ID="txtRoleName" runat="server"></asp:TextBox>
                    </td>
                </tr>
            </table>
            <br />
            <asp:Button ID="btnAdd" runat="server" Text="添 加" OnClick="btnAdd_Click" Width="80px" />&nbsp;&nbsp;
            &nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Button ID="btnReset" runat="server" Text="重 置" OnClick="btnReset_Click" Width="80px" />
        </center>
        <br />
        <p class="titleRow">
            <img alt="" class="NaviImage" src="Img/Manage.jpg" /><b>管理角色</b></p>
        <br />
        <center>
            <table width="80%">
                <tr>
                    <td align="left">
                        <asp:Button ID="btnDel" runat="server" Text="删 除" OnClick="btnDel_Click" Width="80px"
                            OnClientClick="return confirm('确定要删除选定角色？');" />
                        <asp:Label ID="lbRemind" runat="server" Text="当前数据库中没有记录" ForeColor="red" Visible="false"></asp:Label>
                    </td>
                </tr>
            </table>
            <asp:GridView ID="GridView1" runat="server" Width="80%" DataKeyNames="ID" OnRowCommand="GridView1_RowCommand"
                OnRowDataBound="GridView1_RowDataBound" AllowSorting="true" OnSorting="GridView1_Sorting"
                AutoGenerateColumns="false" OnRowCreated="GridView1_RowCreated">
                <Columns>
                    <asp:TemplateField ItemStyle-Width="80px">
                        <HeaderTemplate>
                            选择<asp:CheckBox ID="chkHeader" runat="server" onclick="javascript: SelectAllCheckBoxInThisPageByID(this);">
                            </asp:CheckBox>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="chkSelect" runat="server"></asp:CheckBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="角色名称" DataField="RoleName" SortExpression="RoleName" />
                    <asp:ButtonField CommandName="ChooseEdit" HeaderText="编辑" ButtonType="Link" Text="编辑"
                        ItemStyle-Width="50px" />
                </Columns>
            </asp:GridView>
            <webdiyer:AspNetPager ID="AspNetPager1" runat="server" Width="80%" PageSize="10"
                OnPageChanged="AspNetPager1_PageChanged" ShowPrevNext="true" ShowFirstLast="true"
                ShowInputBox="Always" ShowPageIndex="false" PrevPageText="上一页" NextPageText="下一页"
                FirstPageText="首页" LastPageText="末页" HorizontalAlign="Right" Font-Size="10pt"
                AlwaysShow="true" ShowCustomInfoSection="Left" CustomInfoHTML="<table width='100%'> <tr><td> 共<b style='color:red'>%RecordCount%</b>条记录</td> <td> 第<font color='red'><b>%CurrentPageIndex%</b></font>/%PageCount%页</td> <td>  第%StartRecordIndex% -%EndRecordIndex% 条记录</td></tr>   </table>">
            </webdiyer:AspNetPager>
        </center>
    </asp:Panel>
    <asp:Panel ID="PanelMod" runat="server" Width="100%" Visible="false">
        <p class="titleRow">
            <img alt="" class="NaviImage" src="Img/Edit.jpg" /><b>修改角色信息</b></p>
        <br />
        <center>
            <table class="tableBorder" cellspacing="0" cellpadding="3">
                <tr>
                    <td class="colortd">
                        角色名称
                    </td>
                    <td class="contenttd">
                        <asp:TextBox ID="tbxRename" runat="server"></asp:TextBox>
                    </td>
                </tr>
            </table>
            <br />
            <asp:Button ID="btnMod" runat="server" Text="修 改" OnClick="btnMod_Click" Width="80px" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Button ID="btnBack" runat="server" Text="返 回" OnClick="btnBack_Click" Width="80px" />
            <asp:Label ID="LabelTraID" runat="server" Text="" Visible="false"></asp:Label>
        </center>
    </asp:Panel>
</asp:Content>
