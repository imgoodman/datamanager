<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="AddDocAttrs.aspx.cs" Inherits="App.Web.InventoryMng.AddDocAttrs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="header">
        <h1 class="pagetitle" runat="server" id="lbTitle">
            选择清单对象文档需要显示的属性</h1>
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
                    &nbsp;
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
                        OnRowDataBound="myGridView_RowDataBound" AutoGenerateColumns="false" OnRowCreated="myGridView_RowCreated">
                        <Columns>
                            <asp:TemplateField HeaderText="序号">
                                <ItemTemplate>
                                    <%#Container.DataItemIndex+1 %></ItemTemplate>
                                <ItemStyle Width="50px"></ItemStyle>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="文档">
                                <ItemTemplate>
                                    <%--<asp:Label ID="lbID" runat="server" Visible="false" Text='<%#Eval("ID") %>'></asp:Label>--%>
                                    <%#App.Dll.InventoryMethod.BusinessLayer.GetDocNameByID((Container.DataItem as App.Model.Document).ID)%></ItemTemplate>
                                <ItemStyle Width="150px"></ItemStyle>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="选择文档属性">
                                <ItemTemplate>
                                    <asp:CheckBoxList ID="cblDocAttrs" runat="server" RepeatColumns="3" RepeatDirection="Horizontal">
                                    </asp:CheckBoxList>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:ButtonField CommandName="ChooseEdit" HeaderText="编辑" Text="编辑" Visible="false">
                                <ItemStyle Width="50px" />
                            </asp:ButtonField>
                        </Columns>
                    </asp:GridView>
                    <asp:Label ID="Label1" runat="server" Text="Label" Visible="false"></asp:Label>
                </center>
            </asp:Panel>
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
