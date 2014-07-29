<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="DocInstanceChoose.aspx.cs" Inherits="App.Web.InventoryMng.DocInstanceChoose" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        function getSelected() {
            window.returnValue = "我是返回值";
            window.close();
        }  
    </script>
    <div class="header">
        <h1 class="pagetitle">
            选择清单对象包含的文档对象</h1>
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
                    <div id="jstree_doctype" style="max-height: 100px;">
                    </div>
                    <%--<asp:TreeView ID="TreeViewInventoryType" runat="server" >
                    </asp:TreeView>--%>
                </div>
            </div>
            <div class="control_group">
                <label class="control_label">
                    清单名称</label>
                <div class="controls">
                    <asp:Label ID="lbName" runat="server" Text=""></asp:Label>
                </div>
            </div>
            <div class="control_group">
                <label class="control_label">
                    描述</label>
                <div class="controls">
                    <asp:Label ID="lbDescription" runat="server" Text=""></asp:Label>
                </div>
            </div>
        </fieldset>
        <fieldset class="form">
            <legend>添加清单的文档对象</legend>
            <div class="control_group">
                <label class="control_label">
                    勾选文档</label>
                <div class="controls">
                    <asp:CheckBoxList ID="cblDocs" runat="server" RepeatColumns="4" RepeatDirection="Horizontal">
                    </asp:CheckBoxList>
                </div>
            </div>
        </fieldset>
        <div class="form_actions" style="float: left">
            <asp:Button ID="btnBack" runat="server" Text="上一步" class="btn btn_primary btn_large save"
                OnClick="btnBack_Click" />
        </div>
        <div class="form_actions" style="float: right">
            <asp:Button ID="btnSave" runat="server" Text="下一步" class="btn btn_primary btn_large save"
                OnClientClick="getSelected();" />
        </div>
    </div>
</asp:Content>
