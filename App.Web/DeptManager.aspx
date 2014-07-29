<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="DeptManager.aspx.cs" Inherits="App.Web.DeptManager" %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Panel ID="plMsg" runat="server" Width="100%" Visible="false">
        <center>
            <asp:Label ID="lbMsg" runat="server" Text=""></asp:Label></center>
    </asp:Panel>
    <asp:Panel ID="Panel1" runat="server">
        <p class="titleRow">
            <img alt="" class="NaviImage" src="Img/Add.jpg" /><b>添加部门</b></p>
        <br />
        <center>
            <table class="tableBorder" cellspacing="0" cellpadding="3">
                <tr>
                    <td class="colortd">
                        部门名称
                    </td>
                    <td class="contenttd">
                        <asp:TextBox ID="txtDeptName" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="colortd">
                        父级部门
                    </td>
                    <td class="contenttd">
                        <asp:DropDownList ID="ddlParentDept" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlParentDept_SelectedIndexChanged">
                            <asp:ListItem Text="--请选择父级部门--" Value="0"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
            <br />
            <asp:Button ID="btnAdd" runat="server" Text="添 加" OnClick="btnAdd_Click" Width="80px" />&nbsp;
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <%--<asp:Button ID="btnUpdate" runat="server" Text="更 新" OnClick="btnUpdate_Click" Visible="false"
        Width="60px" />--%>
            <asp:Button ID="btnReset" runat="server" Text="重 置" OnClick="btnReset_Click" Width="80px" />
        </center>
        <br />
        <p class="titleRow">
            <img alt="" class="NaviImage" src="Img/Manage.jpg" /><b>管理部门信息</b></p>
        <br />
        <center>
            <table width="80%">
                <tr>
                    <td align="left">
                        <asp:Button ID="btnDel" runat="server" Text="删 除" OnClick="btnDel_Click" Width="80px"
                            OnClientClick="return confirm('确定要删除选定部门？');" />
                    </td>
                    <td>
                        <asp:Label ID="lbRemind" runat="server" Text="当前条件下数据库没有记录" Visible="false" ForeColor="red"></asp:Label>
                    </td>
                </tr>
            </table>
            <asp:GridView ID="GridView1" runat="server" Width="80%" DataKeyNames="ID" OnRowCommand="GridView1_RowCommand"
                AutoGenerateColumns="false" OnRowDataBound="GridView1_RowDataBound">
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
                    <asp:BoundField HeaderText="部门名称" DataField="DeptName" />
                    <asp:BoundField HeaderText="父级部门" DataField="FatherDeptName" />
                    <asp:ButtonField CommandName="ChooseEdit" HeaderText="编辑" ButtonType="Link" Text="编辑"
                        ItemStyle-Width="50px" />
                </Columns>
            </asp:GridView>
            <webdiyer:AspNetPager ID="AspNetPager1" runat="server" Width="80%" PageSize="20"
                OnPageChanged="AspNetPager1_PageChanged" ShowPrevNext="true" ShowFirstLast="true"
                ShowInputBox="Always" ShowPageIndex="false" PrevPageText="上一页" NextPageText="下一页"
                FirstPageText="首页" LastPageText="末页" HorizontalAlign="Right" Font-Size="10pt"
                AlwaysShow="true" ShowCustomInfoSection="Left" CustomInfoHTML="<table width='100%'> <tr><td> 共<b style='color:red'>%RecordCount%</b>条记录</td> <td> 第<font color='red'><b>%CurrentPageIndex%</b></font>/%PageCount%页</td> <td>  第%StartRecordIndex% -%EndRecordIndex% 条记录</td></tr>   </table>">
            </webdiyer:AspNetPager>
        </center>
    </asp:Panel>
    <asp:Panel ID="Panel2" runat="server" Visible="false">
        <p class="titleRow">
            <img alt="" class="NaviImage" src="Img/Edit.jpg" /><b>修改部门信息</b></p>
        <br />
        <center>
            <table class="tableBorder" cellspacing="0" cellpadding="3">
                <tr>
                    <td class="colortd">
                        部门名称
                    </td>
                    <td class="contenttd">
                        <asp:TextBox ID="txtMDeptName" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <%--            <tr>
                <td class="colortd">
                    父级部门
                </td>
                <td class="contenttd">
                    <asp:DropDownList ID="ddlMParentDept" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>--%>
            </table>
            <br />
            <asp:Button ID="btnModify" runat="server" Text="修 改" OnClick="btnModify_Click" Width="80px" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Button
                ID="btnReturn" runat="server" Text="返 回" OnClick="btnReturn_Click" Width="80px" />
            <asp:TextBox ID="txtMdataID" runat="server" Visible="false"></asp:TextBox>
        </center>
    </asp:Panel>
</asp:Content>
