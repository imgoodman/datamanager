<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CommonFileUpload.ascx.cs"
    Inherits="App.Web.Controls.CommonFileUpload" %>
<asp:FileUpload ID="FileUpload1" runat="server" Width="200px"/><asp:HyperLink ID="HyperLink1"
    runat="server" Target="_blank"></asp:HyperLink>
<asp:Label ID="lbAttachName" runat="server" Text="" Visible="false"></asp:Label>
