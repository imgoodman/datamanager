<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="InventoryProfile.ascx.cs"
    Inherits="App.Web.Controls.InventoryProfile" %>
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
<br />
<div class="control_group">
    <label class="control_label">
        文档及属性</label>
    <div class="controls">
        <asp:Repeater ID="RepeaterList" runat="server">
            <ItemTemplate>
                <%--<asp:Label ID="lbID" runat="server" Text='<%#Eval("ID") %>' Visible="false"></asp:Label>--%>
                <asp:Label ID="lbDocName" runat="server" Text='<%#Eval("DocName")%>'></asp:Label>：&nbsp;[<asp:Label
                    ID="lbAttrNames" runat="server" Text='<%#Eval("AttrNames")%>'></asp:Label>]<br />
            </ItemTemplate>
        </asp:Repeater>
        &nbsp;
    </div>
</div>
