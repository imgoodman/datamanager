<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="PwdManager.aspx.cs" Inherits="App.Web.PwdManager" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Panel ID="plMsg" runat="server" Width="100%" Visible="false">
        <center>
            <asp:Label ID="lbMsg" runat="server" Text=""></asp:Label></center>
    </asp:Panel>
    <p class="titleRow">
        <img alt="" class="NaviImage" src="Img/Manage.jpg" /><b>管理密码</b></p>
    <br />
    <center>
        <table class="tableBorder" cellspacing="0" cellpadding="3">
            <tr>
                <td class="colortd">
                    旧密码
                </td>
                <td>
                    <asp:TextBox ID="TextBox1" runat="server" TextMode="Password"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="colortd">
                    新密码
                </td>
                <td>
                    <asp:TextBox ID="TextBox2" runat="server" TextMode="Password"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="colortd">
                    确认新密码
                </td>
                <td>
                    <asp:TextBox ID="TextBox3" runat="server" TextMode="Password"></asp:TextBox>
                </td>
            </tr>
        </table>
        <asp:Button ID="Button1" runat="server" Text="确 定" OnClick="Button1_Click" Width="80px" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Button ID="Button2" runat="server" Text="重 置" OnClick="Button2_Click" Width="80px" />
    </center>
</asp:Content>
