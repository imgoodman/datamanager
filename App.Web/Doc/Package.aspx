<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="Package.aspx.cs" Inherits="App.Web.Doc.Package" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="header relative">
        <h1 class="pagetitle">
            详情</h1>
    </div>
    <div id="list" style="max-height: 370px; overflow: auto; padding:5px;">
    </div>
    <div class="form_actions">
        <asp:HiddenField ID="fileids" runat="server" />
        <asp:Button ID="btnPackage" runat="server" Text="打包下载" 
            CssClass="btn btn_primary btn_large save" onclick="btnPackage_Click" Width="100%" />
    </div>
    <div>
        <asp:Panel ID="Panel1" runat="server" Width="100%" Visible="false" style="text-align:center;">
            <asp:Label ID="Label1" runat="server" Text="打包下载出错" CssClass="red"></asp:Label>
        </asp:Panel>
        
    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            var id = request("id");
            var fileids = "";
            id = id.replace('#', '');
            $.ajax({
                type: "POST",
                url: "/handlers/docconfig/getBasicDocById.ashx",
                data: { id: id },
                dataType: "json",
                success: function (res) {
                    $(".pagetitle").html(res.DocName + "附件列表");
                }
            });
            //get its file list
            $.ajax({
                type: "POST",
                url: "/handlers/doc/getFilesById.ashx",
                data: { id: id },
                dataType: "json",
                success: function (res) {
                    if (res.length > 0) {
                        $.each(res, function (i, item) {
                            $("<div>").html("<input type='checkbox' checked='checked' /><input type='hidden' value='"+item.Name+"' ><span><a href='/data/" + item.Value + "' target='_blank' title='点击查看附件详情'>" + item.OrginalValue + "</a></span>").addClass("meta_item pull_left file_sel").appendTo($("#list"));
                            fileids += item.Name + ",";
                        });
                        $("input[id$='fileids']").val(fileids);
                        $("<div>").addClass("clear").appendTo($("#list"));
                    } else {
                        $("<p>").html("该文档没有任何附件").appendTo($("#list"));
                    }
                }
            });
            //select and unselect
            $("body").on("click", ".meta_item input[type=checkbox]", function () {
                var $parent = $(this).parent();
                $parent.toggleClass("file_sel");
                fileids = "";
                $("#list .file_sel").each(function (i, item) {
                    fileids += $(item).find("input[type=hidden]").val() + ",";
                });
                $("input[id$='fileids']").val(fileids);
            });
        });
    </script>
</asp:Content>
