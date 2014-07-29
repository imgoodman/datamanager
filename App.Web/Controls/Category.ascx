<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Category.ascx.cs" Inherits="App.Web.Controls.Category" %>
<asp:TextBox ID="tbx_name" runat="server" Width="150px"></asp:TextBox>
<div id="datatree" style="z-index: 1000; position: absolute; text-align: left; background-color: #ddddcc;
    display: none; border: solid 2px #e5e5e5;" runat="server">
    <asp:Panel ID="pl_category" runat="server" Width="200px" Height="300px" ScrollBars="Auto">
        <p style="float: right;">
            <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/Img/close.gif" OnClick="ImageButton1_Click" /></p>
        <asp:TreeView ID="TreeView1" runat="server" OnSelectedNodeChanged="TreeView1_SelectedNodeChanged"
            ShowLines="True" ExpandDepth="30" Height="270px">
        </asp:TreeView>
    </asp:Panel>
    <asp:Label ID="lb_id" runat="server" Text="0" Visible="false"></asp:Label>
</div>
