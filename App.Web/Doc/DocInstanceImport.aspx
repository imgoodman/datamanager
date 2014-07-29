<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="DocInstanceImport.aspx.cs" Inherits="App.Web.Doc.DocInstanceImport" %>

<%@ Register Src="~/Controls/Category.ascx" TagName="DocTypeSelect" TagPrefix="uc1" %>
<%@ Register Src="~/Controls/CommonFileUpload.ascx" TagName="FileUpload" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Panel ID="plMsg" runat="server" Width="100%" Visible="false">
        <center>
            <asp:Label ID="lbMsg" runat="server" Text=""></asp:Label></center>
    </asp:Panel>
    <div class="header">
        <h1 class="pagetitle">
            导入文档数据</h1>
    </div>
    <asp:Panel ID="pl1" runat="server" Width="100%">
        <p class="message hidden">
            <span class="ui-icon ui-icon-info" style="float: left; margin-right: .3em;"></span>
            <strong>Hey!</strong></p>
        <div id="formContent">
            <fieldset class="form">
                <legend>选择文档对象和数据文件</legend>
                <div class="control_group">
                    <label class="control_label">
                        文档类型</label>
                    <div class="controls">
                        <uc1:DocTypeSelect ID="DocTypeSelect" runat="server" GetDataStyle="DocType" AutoPostBack="true"
                            OnTextChanged="DocType_Selected" />
                        <asp:Label ID="lbDocTypeID" runat="server" Text="0" Visible="false"></asp:Label>
                    </div>
                </div>
                <div class="control_group">
                    <label class="control_label">
                        文档对象</label>
                    <div class="controls">
                        <asp:DropDownList ID="ddlDoc" runat="server" Width="200px">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="control_group">
                    <label class="control_label">
                        选择导入文件</label>
                    <div class="controls">
                        <uc1:FileUpload ID="FU" runat="server" />
                        <asp:Label ID="lbFilePath" runat="server" Text="" Visible="false"></asp:Label>
                    </div>
                </div>
            </fieldset>
            <div class="form_actions">
                <asp:Button ID="btnNext" runat="server" Text="下一步" OnClick="btnNext_Click" class="btn btn_primary btn_large save"
                    Font-Size="Medium" Width="100%" />
            </div>
        </div>
    </asp:Panel>
    <asp:Panel ID="pl2" runat="server" Width="100%" Visible="false">
        <div id="formContent2">
            <fieldset class="form">
                <legend>校对导入文件的列名</legend>
                <asp:GridView ID="GridView1" runat="server" Width="100%" DataKeyNames="ColName" BorderWidth="1"
                    GridLines="None" BorderColor="#8A8A8A" AutoGenerateColumns="false" OnRowDataBound="GridView1_RowDataBound"
                    RowStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                    <RowStyle BackColor="#FFFFFF" Font-Size="Medium" Font-Names="华文仿宋" Height="35px"
                        BorderColor="gray" VerticalAlign="Middle" BorderWidth="1" />
                    <AlternatingRowStyle BackColor="#E4F1FF" />
                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                    <HeaderStyle BackColor="#9FD6FF" Font-Bold="True" Height="35px" CssClass="tbHeader"
                        Font-Names="华文仿宋" VerticalAlign="Middle" Font-Size="Medium" />
                    <Columns>
                        <asp:BoundField HeaderText="导入文件的列名" DataField="ColName" HtmlEncode="false" />
                        <asp:BoundField HeaderText="文档对象的属性" DataField="DocAttrName" HtmlEncode="false" />
                    </Columns>
                </asp:GridView>
            </fieldset>
            <div class="form_actions">
                <table>
                    <tr>
                        <td align="center">
                            <asp:Button ID="btnNext2" runat="server" Text="下一步" OnClick="btnNext2_Click" class="btn btn_primary btn_large save"
                                Font-Size="Medium" Width="120px" />
                        </td>
                        <td>
                            &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;
                        </td>
                        <td align="center">
                            <asp:Button ID="btnReturn2" runat="server" Text="上一步" OnClick="btnReturn2_Click"
                                class="btn btn_primary btn_large save" Font-Size="Medium" Width="120px" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </asp:Panel>
    <asp:Panel ID="pl3" runat="server" Width="100%" Visible="false">
        <div id="formContent3">
            <fieldset class="form">
                <legend>校对导入文件的列名</legend>
                <asp:GridView ID="gvData" runat="server" Width="100%" DataKeyNames="No" BorderWidth="1"
                    GridLines="None" BorderColor="#8A8A8A" AutoGenerateColumns="false" OnRowDataBound="gvData_RowDataBound"
                    RowStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                    <RowStyle BackColor="#FFFFFF" Font-Size="Medium" Font-Names="华文仿宋" Height="35px"
                        BorderColor="gray" VerticalAlign="Middle" BorderWidth="1" />
                    <AlternatingRowStyle BackColor="#E4F1FF" />
                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                    <HeaderStyle BackColor="#9FD6FF" Font-Bold="True" Height="35px" CssClass="tbHeader"
                        Font-Names="华文仿宋" VerticalAlign="Middle" Font-Size="Medium" />
                    <Columns>
                    </Columns>
                </asp:GridView>
            </fieldset>
            <div class="form_actions">
                <table>
                    <tr>
                        <td align="center">
                            <asp:Button ID="btnConfirm" runat="server" Text="确定导入" OnClick="btnConfirm_Click"
                                class="btn btn_primary btn_large save" Font-Size="Medium" Width="120px" />
                        </td>
                        <td>
                            &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;
                        </td>
                        <td align="center">
                            <asp:Button ID="btnReturn3" runat="server" Text="取消" OnClick="btnReturn3_Click" class="btn btn_primary btn_large save"
                                Font-Size="Medium" Width="120px" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </asp:Panel>
</asp:Content>
