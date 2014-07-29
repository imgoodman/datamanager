<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="AddSave.aspx.cs" Inherits="App.Web.InventoryMng.AddSave" %>

<%@ Register Src="~/Controls/InventoryProfile.ascx" TagPrefix="MyControl" TagName="InventoryProfile" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="header">
        <h1 class="pagetitle">
            保存正式清单</h1>
    </div>
    <p class="message hidden">
        <span class="ui-icon ui-icon-info" style="float: left; margin-right: .3em;"></span>
        <strong>Hey!</strong></p>
    <div id="formContent">
        <fieldset class="form">
            <legend>清单详情</legend>
            <MyControl:InventoryProfile runat="server" ID="InventoryProfile"></MyControl:InventoryProfile>
        </fieldset>
        <div class="form_actions" style="float: left">
            <asp:Button ID="btnBack" runat="server" Text="上一步" class="btn btn_primary btn_large save"
                OnClick="btnBack_Click" />
        </div>
        <div class="form_actions" style="float: right">
            <asp:Button ID="btnSave" runat="server" Text="提 交" class="btn btn_primary btn_large save"
                OnClick="btnSave_Click" />
        </div>
    </div>
</asp:Content>
