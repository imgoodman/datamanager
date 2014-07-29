<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="AttrRelationList.aspx.cs" Inherits="App.Web.DocConfig.AttrRelationList" %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<%@ Register Src="~/Controls/Category.ascx" TagName="DocTypeSelect" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Panel ID="plMsg" runat="server" Width="100%" Visible="false">
        <center>
            <asp:Label ID="lbMsg" runat="server" Text=""></asp:Label></center>
    </asp:Panel>
    <div class="header relative">
        <h1 class="pagetitle">
            文档对象关联列表</h1>
    </div>
    <center>
        <table width="100%" style="border-color: #EFF6FE; border-width: 0px; border: 1px;
            border-collapse: collapse; height: 70px; border-style: solid" cellpadding="1"
            id="tb1" cellspacing="0" runat="server">
            <tr>
                <td style="background-color: #9FD6FF; font-weight: bold; width: 100px; font-family: 华文仿宋;
                    font-size: medium; vertical-align: middle; border-right: solid; border-width: 1px;"
                    align="center">
                    搜 索
                </td>
                <td align="left" style="vertical-align: middle; font-size: small">
                    <table width="95%">
                        <tr style="height: 32px; vertical-align: middle">
                            <td class="contenttd">
                                &nbsp;&nbsp;
                                <uc1:DocTypeSelect ID="RDocTypeSelect" runat="server" GetDataStyle="DocType" AutoPostBack="true"
                                    OnTextChanged="RDocType_Selected" />
                                <asp:Label ID="lbRDocTypeID" runat="server" Text="0" Visible="false"></asp:Label>
                                &nbsp;&nbsp;
                                <asp:DropDownList ID="ddlRDoc" runat="server" Width="200px">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr style="height: 32px; vertical-align: middle">
                            <td>
                                &nbsp;&nbsp;
                                <uc1:DocTypeSelect ID="SDocTypeSelect" runat="server" GetDataStyle="DocType" AutoPostBack="true"
                                    OnTextChanged="SDocType_Selected" />
                                <asp:Label ID="lbSDocTypeID" runat="server" Text="0" Visible="false"></asp:Label>
                                &nbsp;&nbsp;
                                <asp:DropDownList ID="ddlSDoc" runat="server" Width="200px">
                                </asp:DropDownList>
                                &nbsp;
                                <asp:Button ID="btnSearch" runat="server" Text="搜索" OnClick="btnSearch_Click" Width="80px"
                                    Height="24px" />
                            </td>
                        </tr>
                    </table>
                </td>
                <td style="font-weight: bold; width: 100px; vertical-align: middle; border-left: solid;
                    border-width: 1px;" align="center">
                    <asp:Button ID="btnShowAll" runat="server" Text="显示全部" OnClick="btnShowAll_Click"
                        Width="80px" Height="24px" />
                </td>
            </tr>
        </table>
        <br />
        <asp:GridView ID="GridView1" runat="server" Width="100%" DataKeyNames="ID" AllowSorting="true"
            BorderWidth="1" GridLines="None" BorderColor="#8A8A8A" OnSorting="GridView1_Sorting"
            OnRowCommand="GridView1_RowCommand" AutoGenerateColumns="false" OnRowDataBound="GridView1_RowDataBound"
            EmptyDataText="<font color='red'>没有符合条件的文档关联</font>" RowStyle-HorizontalAlign="Center"
            HeaderStyle-HorizontalAlign="Center">
            <RowStyle BackColor="#FFFFFF" Font-Size="Medium" Font-Names="华文仿宋" Height="35px"
                BorderColor="gray" VerticalAlign="Middle" BorderWidth="1" />
            <AlternatingRowStyle BackColor="#E4F1FF" />
            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
            <HeaderStyle BackColor="#9FD6FF" Font-Bold="True" Height="35px" CssClass="tbHeader"
                Font-Names="华文仿宋" VerticalAlign="Middle" Font-Size="Medium" />
            <Columns>
                <asp:TemplateField ItemStyle-Width="60px">
                    <HeaderTemplate>
                        选择<asp:CheckBox ID="chkHeader" runat="server" onclick="javascript: SelectAllCheckBoxInThisPageByID(this);">
                        </asp:CheckBox>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="chkSelect" runat="server"></asp:CheckBox>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField HeaderText="引用文档对象" DataField="RDocName" />
                <asp:BoundField HeaderText="引用文档对象属性" DataField="RAttrName" ItemStyle-Width="120px" />
                <asp:BoundField HeaderText="源文档对象" DataField="SDocName" />
                <asp:BoundField HeaderText="源文档对象属性" DataField="SAttrName" ItemStyle-Width="120px" />
<%--                <asp:ButtonField CommandName="ChooseEdit" HeaderText="编辑" ButtonType="Link" Text="编辑"
                    ItemStyle-Width="60px" />--%>
            </Columns>
        </asp:GridView>
        <asp:Panel ID="plBottom" runat="server">
            <table border="0" align="center" cellpadding="0" cellspacing="1" bgcolor="#9FD6FF"
                width="100%">
                <tr>
                    <td height="26" width="120px" bgcolor="EFF6FE" class="title_white">
                        <asp:Button ID="btnDel" runat="server" Text="删除选中记录" Width="120px" BackColor="#0099CC"
                            OnClick="btnDel_Click" OnClientClick="return confirm('确定要删除选定数据吗?');" />
                    </td>
                    <td>
                        <webdiyer:AspNetPager ID="AspNetPager1" runat="server" PageSize="25" OnPageChanged="AspNetPager1_PageChanged"
                            ShowPrevNext="true" ShowFirstLast="true" ShowInputBox="Always" ShowPageIndex="false"
                            PrevPageText="上一页" NextPageText="下一页" FirstPageText="首页" LastPageText="末页" HorizontalAlign="Right"
                            Font-Size="10pt" AlwaysShow="true" ShowCustomInfoSection="Left" CustomInfoHTML="<table width='100%'> <tr><td> 共<b style='color:red'>%RecordCount%</b>条记录</td> <td> 第<font color='red'><b>%CurrentPageIndex%</b></font>/%PageCount%页</td> <td>  第%StartRecordIndex% -%EndRecordIndex% 条记录</td></tr>   </table>">
                        </webdiyer:AspNetPager>
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </center>
</asp:Content>
