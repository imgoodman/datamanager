<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DocInstancesChoose.aspx.cs"
    Inherits="App.Web.InventoryMng.DocInstancesChoose" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script language="javascript" type="text/javascript">
        function ReturnResult(TxtClientID, lbIsPositionClientID, lbVariableClientID, ParamInfo, IsPosition, ParamID) {
            window.opener.document.getElementById(TxtClientID).value = ParamInfo;
            window.opener.document.getElementById(lbIsPositionClientID).innerHTML = IsPosition;
            window.opener.document.getElementById(lbVariableClientID).innerHTML = ParamID;
            window.close();
        }</script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <fieldset>
            <legend>文档基本信息</legend>
            <div>
                文档类型：<asp:Label ID="lbType" runat="server" Text=""></asp:Label></div>
            <div>
                文档名称：<asp:Label ID="lbName" runat="server" Text=""></asp:Label></div>
            <div>
                文档描述：<asp:Label ID="lbDescription" runat="server" Text=""></asp:Label></div>
        </fieldset>
        <br />
        <asp:Panel ID="Panel1" runat="server" Width="100%">
            <br />
            <center>
                <table width="100%">
                    <tr>
                        <td align="left">
                            <asp:Label ID="lbRemind" runat="server" Text="当前数据库中没有记录" ForeColor="red" Visible="false"></asp:Label>
                        </td>
                    </tr>
                </table>
                <asp:GridView ID="myGridView" runat="server" Width="100%" DataKeyNames="ID" OnRowCommand="myGridView_RowCommand"
                    OnRowDataBound="myGridView_RowDataBound" AllowSorting="True" OnSorting="myGridView_Sorting"
                    AutoGenerateColumns="false" OnRowCreated="myGridView_RowCreated">
                    <Columns>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                选择
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkSelect" runat="server"></asp:CheckBox>
                                <asp:Label ID="ID" runat="server" Text='<%#Eval("ID") %>' Visible="false"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Width="80px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="序号">
                            <ItemTemplate>
                                <%#Container.DataItemIndex+1 %></ItemTemplate>
                            <ItemStyle Width="50px"></ItemStyle>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="文档实例">
                            <ItemTemplate>
                                <%#Eval("InstanceAttrNames")%></ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </center>
        </asp:Panel>
        <%--<input type="button" name="button" id="btnSave" value="保 存" onclick="javascript:getSelected();return false;" />--%>
        <asp:Button ID="btnSave" runat="server" Text="保 存" class="btn btn_primary btn_large save"
            OnClick="btnSave_Click" />
        <%--<asp:Button ID="Button1" runat="server" Text="保 存" class="btn btn_primary btn_large save"
            OnClientClick="getSelected();return false;"  />--%>
    </div>
    </form>
</body>
</html>
