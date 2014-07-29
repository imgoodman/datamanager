<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="InventoryType.aspx.cs" Inherits="App.Web.InventoryType" %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<%@ Register TagName="Category" TagPrefix="JQPLMWeb" Src="~/Controls/Category.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Panel ID="Panel1" runat="server">
        <div class="header">
            <h1 class="pagetitle">
                清单对象分类管理</h1>
        </div>
        <p class="titleRow">
            <img alt="" class="NaviImage" src="../Img/add.jpg" /><b>添加清单对象类型</b></p>
        <br />
        <center>
            <table class="tableBorder" cellspacing="0" cellpadding="3">
                <tr>
                    <td class="colortd" style="text-align: center;">
                        选择父级类型
                    </td>
                    <td class="contenttd">
                        <JQPLMWeb:Category ID="category1" runat="server" GetDataStyle="InventoryType" AutoPostBack="true"
                            OnTextChanged="TextChanged1"></JQPLMWeb:Category>
                        <asp:Label ID="lb_deptid" runat="server" Text="" Visible="false"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="colortd" style="text-align: center;">
                        清单对象类型名称
                    </td>
                    <td class="contenttd">
                        <asp:TextBox ID="txtDeptName" runat="server"></asp:TextBox>
                    </td>
                </tr>
            </table>
            <br />
            <asp:Button ID="btnAdd" runat="server" Text="添 加" OnClick="btnAdd_Click" Width="80px" />&nbsp;
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Button ID="btnReset" runat="server" Text="重 置" OnClick="btnReset_Click" Width="80px" />
        </center>
        <br />
        <p class="titleRow">
            <img alt="" class="NaviImage" src="../Img/Manage.jpg" /><b>管理清单对象类型</b></p>
        <br />
        <center>
            <table width="80%">
                <tr>
                    <td align="left">
                        <asp:Button ID="btnDel" runat="server" Text="删 除" OnClick="btnDel_Click" Width="80px"
                            OnClientClick="return confirm('确定要删除选定清单对象类型？');" />
                    </td>
                    <td>
                        <asp:Label ID="lbRemind" runat="server" Text="当前条件下数据库没有记录" Visible="false" ForeColor="red"></asp:Label>
                    </td>
                </tr>
            </table>
            <asp:GridView ID="GridView1" runat="server" Width="80%" DataKeyNames="ID" OnRowCommand="GridView1_RowCommand"
                AutoGenerateColumns="false" OnRowDataBound="GridView1_RowDataBound" BorderWidth="1"
                GridLines="None" BorderColor="#8A8A8A" RowStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                <RowStyle BackColor="#FFFFFF" BorderColor="gray" VerticalAlign="Middle" BorderWidth="1" />
                <AlternatingRowStyle BackColor="#E4F1FF" />
                <HeaderStyle BackColor="#9FD6FF" CssClass="tbHeader" VerticalAlign="Middle" Font-Size="Medium" />
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
                    <asp:BoundField HeaderText="父级类型" DataField="FatherName" />
                    <asp:BoundField HeaderText="清单对象类型名称" DataField="Name" />
                    <asp:ButtonField CommandName="ChooseEdit" HeaderText="编辑" ButtonType="Link" Text="编辑"
                        ItemStyle-Width="50px" />
                </Columns>
            </asp:GridView>
            <webdiyer:AspNetPager ID="AspNetPager1" runat="server" Width="80%" PageSize="25"
                OnPageChanged="AspNetPager1_PageChanged" ShowPrevNext="true" ShowFirstLast="true"
                ShowInputBox="Always" ShowPageIndex="false" PrevPageText="上一页" NextPageText="下一页"
                FirstPageText="首页" LastPageText="末页" HorizontalAlign="Right" Font-Size="10pt"
                AlwaysShow="true" ShowCustomInfoSection="Left" CustomInfoHTML="<table width='100%'> <tr><td> 共<b style='color:red'>%RecordCount%</b>条记录</td> <td> 第<font color='red'><b>%CurrentPageIndex%</b></font>/%PageCount%页</td> <td>  第%StartRecordIndex% -%EndRecordIndex% 条记录</td></tr>   </table>">
            </webdiyer:AspNetPager>
        </center>
    </asp:Panel>
    <asp:Panel ID="Panel2" runat="server" Visible="false">
        <p class="titleRow">
            <img alt="" class="NaviImage" src="../Img/Edit.jpg" /><b>修改清单对象类型信息</b></p>
        <br />
        <center>
            <table class="tableBorder" cellspacing="0" cellpadding="3">
                <tr>
                    <td class="colortd" style="text-align: center; width: 105px">
                        选择父级类型
                    </td>
                    <td class="contenttd">
                        <JQPLMWeb:Category ID="category2" runat="server" GetDataStyle="InventoryType" AutoPostBack="true"
                            OnTextChanged="TextChanged2"></JQPLMWeb:Category>
                        &nbsp;<asp:Button ID="btnSetRoot" runat="server" Text="设为根节点" OnClick="btnSetRoot_Click" />
                        <asp:Label ID="lb_deptid2" runat="server" Text="" Visible="false"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="colortd" style="text-align: center; width: 105px">
                        清单对象类型名称
                    </td>
                    <td class="contenttd">
                        <asp:TextBox ID="txtMDeptName" runat="server"></asp:TextBox>
                    </td>
                </tr>
            </table>
            <br />
            <asp:Button ID="btnModify" runat="server" Text="修 改" OnClick="btnModify_Click" Width="80px" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Button
                ID="btnReturn" runat="server" Text="返 回" OnClick="btnReturn_Click" Width="80px" />
            <asp:TextBox ID="txtMdataID" runat="server" Visible="false"></asp:TextBox>
        </center>
    </asp:Panel>
</asp:Content>
