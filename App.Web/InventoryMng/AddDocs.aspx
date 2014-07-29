<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="AddDocs.aspx.cs" Inherits="App.Web.InventoryMng.AddDocs" %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="header">
        <h1 class="pagetitle">
            选择清单对象包含的文档对象</h1>
    </div>
    <p class="message warning hidden">
        <span class="ui-icon ui-icon-info" style="float: left; margin-right: .3em;"></span>
        <strong>Hey!</strong></p>
    <div id="formContent">
        <fieldset class="form detailinfo relative">
            <legend>清单基本信息</legend>
            <div class="control_group">
                <label class="control_label">
                    清单类别</label>
                <div class="controls">
                    <asp:Label ID="lbType" runat="server" Text=""></asp:Label>&nbsp;
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
            <legend>添加清单的文档对象</legend>
            <div class="control_group hidden">
                <label class="control_label">
                    勾选文档</label>
                <div class="controls">
                    <asp:CheckBoxList ID="cblDocs" runat="server" RepeatColumns="4" RepeatDirection="Horizontal">
                    </asp:CheckBoxList>
                </div>
            </div>
                        <asp:Repeater ID="Repeater1" runat="server" OnItemDataBound="Repeater1_ItemDataBound">
                <ItemTemplate>
                    <div class="control_group">
                        <label class="control_label">
                            <asp:Label runat="server" Text="" ID="lbTypeName"></asp:Label></label>
                        <div class="controls">
                            <asp:CheckBoxList ID="cblDoc" runat="server" RepeatColumns="4" RepeatDirection="Horizontal">
                            </asp:CheckBoxList>&nbsp;
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
            <%--<div class="control_group">
                <table>
                    <tr>
                        <td>
                            <label class="control_label">
                                已添加的文档对象</label>
                            <div class="controls">
                                <asp:GridView ID="gvDocs" runat="server" AutoGenerateColumns="false" ShowHeader="false"
                                    BorderColor="#8A8A8A" BorderWidth="1" GridLines="None" OnRowCommand="gvDocs_RowCommand"
                                    DataKeyNames="ID" EmptyDataText="<font color='red'>尚未添加任何文档对象</font>">
                                    <RowStyle BackColor="#FFFFFF" BorderColor="gray" BorderWidth="1" />
                                    <Columns>
                                        <asp:BoundField DataField="ID" />
                                        <asp:ButtonField CommandName="ChooseDel" ButtonType="Link" Text="删除" ItemStyle-Width="50px" />
                                    </Columns>
                                </asp:GridView>
                            </div>
                            <div class="controls">
                                <asp:Button ID="btnAdd" runat="server" OnClick="btnAdd_Click" />
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Panel ID="plSearch" runat="server" Visible="false">
                                    <table>
                                        <tr>
                                            <td>
                                                <table width="100%" style="border-color: #EFF6FE; border-width: 0px; border: 1px;
                                                    border-collapse: collapse; height: 35px; border-style: solid" cellpadding="1"
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
                                                                        &nbsp;
                                                                        <asp:DropDownList ID="ddlType" runat="server" AppendDataBoundItems="True">
                                                                        </asp:DropDownList>
                                                                        &nbsp;
                                                                        <asp:Label ID="lb2" runat="server" Font-Bold="true" Text="模糊搜索: 文档对象名称"></asp:Label>
                                                                        &nbsp;
                                                                        <asp:TextBox ID="txtfuzzyProjName" runat="server" Width="150px"></asp:TextBox>&nbsp;
                                                                        <asp:Button ID="btnSearch" runat="server" Text="搜索" OnClick="btnSearch_Click" Width="80px"
                                                                            Height="24px" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:GridView ID="gvAllDocs" runat="server" ShowHeader="false" DataKeyNames="ID"
                                                    Width="100%" AutoGenerateColumns="false" BorderWidth="1" GridLines="None" BorderColor="#8A8A8A"
                                                    RowStyle-HorizontalAlign="Center">
                                                    <RowStyle BackColor="#FFFFFF" BorderColor="gray" BorderWidth="1" />
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
                                                        <asp:BoundField DataField="DocName" />
                                                    </Columns>
                                                </asp:GridView>
                                                <webdiyer:AspNetPager ID="AspNetPager1" runat="server" PageSize="10" OnPageChanged="AspNetPager1_PageChanged"
                                                    Width="100%" ShowPrevNext="true" ShowFirstLast="true" ShowInputBox="Always" ShowPageIndex="false"
                                                    PrevPageText="上一页" NextPageText="下一页" FirstPageText="首页" LastPageText="末页" HorizontalAlign="Right"
                                                    Font-Size="10pt" AlwaysShow="true" ShowCustomInfoSection="Left" CustomInfoHTML="<table width='100%'> <tr><td> 共<b style='color:red'>%RecordCount%</b>条记录</td> <td> 第<font color='red'><b>%CurrentPageIndex%</b></font>/%PageCount%页</td> <td>  第%StartRecordIndex% -%EndRecordIndex% 条记录</td></tr>   </table>">
                                                </webdiyer:AspNetPager>
                                            </td>
                                        </tr>
                                    </table>
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
            </div>--%>
        </fieldset>
        <div class="form_actions" style="float: left">
            <asp:Button ID="btnBack" runat="server" Text="上一步" class="btn btn_primary btn_large save"
                OnClick="btnBack_Click" />
        </div>
        <div class="form_actions" style="float: right">
            <asp:Button ID="btnSave" runat="server" Text="下一步" class="btn btn_primary btn_large save"
                OnClick="btnSave_Click" />
        </div>
    </div>
</asp:Content>
