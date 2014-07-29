<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="Profile.aspx.cs" Inherits="App.Web.InventoryMng.Profile" %>

<%@ Register Src="~/Controls/InventoryProfile.ascx" TagPrefix="MyControl" TagName="InventoryProfile" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="header">
        <h1 class="pagetitle">
            清单详情</h1>
    </div>
    <p class="message hidden">
        <span class="ui-icon ui-icon-info" style="float: left; margin-right: .3em;"></span>
        <strong>Hey!</strong></p>
    <div id="formContent">
        <fieldset class="form">
            <legend>详情</legend>
            <MyControl:InventoryProfile runat="server" ID="InventoryProfile"></MyControl:InventoryProfile>
        </fieldset>
        <div class="form_actions" style="float: right">
            <asp:Button ID="btnDel" runat="server" Text="删除" class="btn btn_primary btn_large inst_del btn_del"
                OnClick="btnDel_Click" />
        </div>
        <div class="form_actions" style="float: right">
            <asp:Button ID="btnSave" runat="server" Text="编辑" class="btn btn_primary btn_large save"
                OnClick="btnEdit_Click" />
        </div>
    </div>
</asp:Content>
