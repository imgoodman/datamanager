<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="Add.aspx.cs" Inherits="App.Web.InventoryMng.Add" %>

<%@ Register TagName="Category" TagPrefix="JQPLMWeb" Src="~/Controls/Category.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="header">
        <h1 class="pagetitle">
            新增清单对象</h1>
    </div>
    <p class="message warning hidden">
        <span class="ui-icon ui-icon-info" style="float: left; margin-right: .3em;"></span>
        <strong>Hey!</strong></p>
    <div id="formContent">
        <fieldset class="form">
            <legend>清单基本信息</legend>
            <div class="control_group">
                <label class="control_label">
                    清单类别</label>
                <div class="controls">
                    <%--                    <div id="jstree_doctype" style="max-height: 100px;">
                    </div>--%>
                    <%--<asp:TreeView ID="TreeViewInventoryType" runat="server" >
                    </asp:TreeView>--%>
                    <JQPLMWeb:Category ID="category1" runat="server" GetDataStyle="InventoryType" AutoPostBack="true"
                        OnTextChanged="TextChanged1"></JQPLMWeb:Category>
                    <asp:Label ID="lb_deptid" runat="server" Text="" Visible="false"></asp:Label>
                </div>
            </div>
            <div class="control_group">
                <label class="control_label">
                    清单名称</label>
                <div class="controls">
                    <asp:TextBox ID="txtName" runat="server" class="input_medium"></asp:TextBox>
                    <%--<input type="text" class="input_medium" id="docName" />--%>
                    <%--<p class="help_block">
                        文档名称比如是“会议纪要”，“审定信函”等。</p>--%>
                </div>
            </div>
            <div class="control_group">
                <label class="control_label">
                    描述</label>
                <div class="controls">
                    <asp:TextBox ID="txtDescription" runat="server" class="input_large multitext" TextMode="MultiLine"></asp:TextBox>
                    <%--<textarea class="input_large multitext" id="desc"></textarea>--%>
                </div>
            </div>
        </fieldset>
        <div class="form_actions" style="float: right">
            <asp:Button ID="btnSave" runat="server" Text="下一步" class="btn btn_primary btn_large save"
                OnClick="btnSave_Click" />
            <%--<a id="btnNext" class="btn btn_primary btn_large save" runat="server" onclick="btnNext_Click">
                下一步</a>--%>
        </div>
    </div>
</asp:Content>
