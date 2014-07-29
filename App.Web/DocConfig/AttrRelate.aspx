<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="AttrRelate.aspx.cs" Inherits="App.Web.DocConfig.AttrRelate" %>

<%@ Register Src="~/Controls/Category.ascx" TagName="DocTypeSelect" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Panel ID="plMsg" runat="server" Width="100%" Visible="false">
        <center>
            <asp:Label ID="lbMsg" runat="server" Text=""></asp:Label></center>
    </asp:Panel>
    <div class="header">
        <h1 class="pagetitle">
            关联文档对象</h1>
    </div>
    <asp:Panel ID="plAdd" runat="server">
        <p class="message warning hidden">
            <span class="ui-icon ui-icon-info" style="float: left; margin-right: .3em;"></span>
            <strong>Hey!</strong></p>
        <div id="formContent">
            <fieldset class="form">
                <legend>引用文档对象的属性</legend>
                <div class="control_group">
                    <label class="control_label">
                        文档对象类别</label>
                    <div class="controls">
                        <uc1:DocTypeSelect ID="RDocTypeSelect" runat="server" GetDataStyle="DocType" AutoPostBack="true"
                            OnTextChanged="RDocType_Selected" />
                        <asp:Label ID="lbRDocTypeID" runat="server" Text="0" Visible="false"></asp:Label>
                    </div>
                </div>
                <div class="control_group">
                    <label class="control_label">
                        引用文档对象</label>
                    <div class="controls">
                        <asp:DropDownList ID="ddlRDoc" runat="server" OnSelectedIndexChanged="ddlRDoc_SelectedIndexChanged"
                            AutoPostBack="true" Width="250px">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="control_group">
                    <label class="control_label">
                        属性</label>
                    <div class="controls">
                        <asp:DropDownList ID="ddlRDocAttr" runat="server" Width="250px">
                        </asp:DropDownList>
                    </div>
                </div>
            </fieldset>
            <fieldset class="form">
                <legend>源文档对象的属性</legend>
                <div class="control_group">
                    <label class="control_label">
                        文档对象类别</label>
                    <div class="controls">
                        <uc1:DocTypeSelect ID="SDocTypeSelect" runat="server" GetDataStyle="DocType" AutoPostBack="true"
                            OnTextChanged="SDocType_Selected" />
                        <asp:Label ID="lbSDocTypeID" runat="server" Text="0" Visible="false"></asp:Label>
                    </div>
                </div>
                <div class="control_group">
                    <label class="control_label">
                        源文档对象</label>
                    <div class="controls">
                        <asp:DropDownList ID="ddlSDoc" runat="server" OnSelectedIndexChanged="ddlSDoc_SelectedIndexChanged"
                            AutoPostBack="true" Width="250px">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="control_group">
                    <label class="control_label">
                        属性</label>
                    <div class="controls">
                        <asp:DropDownList ID="ddlSDocAttr" runat="server" Width="250px">
                        </asp:DropDownList>
                    </div>
                </div>
            </fieldset>
            <div class="form_actions">
                <asp:Button ID="btnSave" runat="server" Text="保存" OnClick="btnSave_Click" class="btn btn_primary btn_large save"
                    Font-Size="Medium" Width="100%" />
            </div>
        </div>
    </asp:Panel>
    <asp:Panel ID="plSaveResult" runat="server" Visible="false">
        <div class="formContent">
            <p>
                恭喜您，您已经成功配置了文档对象的关联。您可以查看<a href="/DocConfig/AttrRelationList.aspx" class="docLink">文档对象关联列表</a></p>
            <p>
                您还可以继续配置文档对象的关联,<a href="/DocConfig/AttrRelate.aspx">关联文档对象</a></p>
        </div>
    </asp:Panel>
</asp:Content>
