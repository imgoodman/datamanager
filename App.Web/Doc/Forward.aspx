<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="Forward.aspx.cs" Inherits="App.Web.Doc.Forward" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="header">
        <h1 class="pagetitle">
            新增文档数据</h1>
    </div>
    <p class="message hidden">
        <span class="ui-icon ui-icon-info" style="float: left; margin-right: .3em;"></span>
        <strong>Hey!</strong></p>
    <div id="formContent">
        <fieldset class="form">
            <legend>选择要新增的文档对象</legend>
            <div class="control_group">
                <label class="control_label">
                    文档类型(展开选择)</label>
                <div class="controls">
                    <div id="jstree_doctype">
                    </div>
                </div>
            </div>
            <div class="control_group">
                <label class="control_label">
                    文档对象</label>
                <div class="controls">
                    <select class="input_large" id="docs">
                        <option value="0">--请选择文档对象--</option>
                    </select>
                </div>
            </div>
        </fieldset>
        <div class="form_actions center">
            <a href="javascript:void(0);" class="btn btn_primary add_doc btn_large" title="请先选择一个文档对象,然后新增">
                新增</a> <a href="javascript:void(0);" class="btn btn_primary list_doc btn_large" style="display: none;"
                    title="请先选择一个文档对象,然后查看">查看</a> <a href="javascript:void(0);" class="btn btn_primary tran_doc btn_large"
                        style="display: none;" title="请先选择一个文档对象,然后移交">移交</a><a href="javascript:void(0);" class="btn btn_primary package_doc btn_large"
                        style="display: none;" title="请先选择一个文档对象,然后打包附件下载">附件打包</a>
        </div>
    </div>
    <script type="text/javascript" language="javascript">
        $(document).ready(function () {
            var action = request("action");
            if ($("#isadmin").val() == "True") {
                $("#isVirtual").val("2");
            } else {
                $("#isVirtual").val("0");
            }
            if (action == "list" || action == "admin") {
                $(".pagetitle").html("查看文档数据");
                $(".form legend").html("选择要查看的文档对象");
                $(".add_doc").remove();
                $(".list_doc").show();
            }
            if (action == "transfer") {
                $(".pagetitle").html("移交文档数据");
                $(".form legend").html("选择要移交的文档对象");
                $(".add_doc,.package_doc").remove();
                $(".tran_doc").show();
            }
            if (action == "package") {
                $(".pagetitle").html("文档附件打包下载");
                $(".form legend").html("选择要打包的文档对象");
                $(".add_doc,.tran_doc").remove();
                $(".package_doc").show();
            }
            //load doc types
            $.ajax({
                type: "POST",
                url: "/handlers/docType/getDocTypes.ashx",
                data: { isVirtual: $("#isVirtual").val() },
                dataType: "json",
                success: function (res) {
                    var $tree = $("#jstree_doctype").jstree({
                        "core": {
                            "data": res,
                            "multiple": false
                        }
                    }).on("changed.jstree", function (e, data) {
                        //点击事件
                        var i, j, r = [];
                        for (i = 0, j = data.selected.length; i < j; i++) {
                            r.push(data.instance.get_node(data.selected[i]).id);
                        }
                        var selDocTypeID = r[0];
                        selDocTypeID = selDocTypeID.replace('d', '');
                        $.ajax({
                            type: "POST",
                            url: "/handlers/docconfig/getDocsByTypeID.ashx",
                            data: { typeid: selDocTypeID },
                            dataType: "json",
                            success: function (res_docs) {
                                var $sel = $("#docs");
                                $sel.empty();
                                $("<option>").text("--请选择文档--").val("0").appendTo($sel);
                                if (res_docs != null) {
                                    $.each(res_docs, function (k, kitem) {
                                        $("<option>").text(kitem.DocName).val(kitem.ID).appendTo($sel);
                                    });
                                }
                            }
                        });
                    });
                    //$tree.bind('loaded.jstree', function (event, data) {
                    //    data.instance.open_all(-1);
                    //});
                    $.each(res, function (dtindex, dt) {
                        //console.log(dt);
                        if (dt.parent == "#") {
                            $tree.jstree("open_all", "#" + dt.id);
                        }
                    });
                    //$tree.jstree("open_all", "#d10");
                    //$tree.jstree("open_all", "#d27");
                }
            });
            //load all documents
            $.ajax({
                type: "POST",
                url: "/handlers/doc/getallbasicdocs.ashx",
                data: { isVirtual: $("#isVirtual").val() },
                dataType: "json",
                success: function (res) {
                    var $sel = $("#docs");
                    $sel.empty();
                    $("<option>").text("--请选择文档--").val("0").appendTo($sel);
                    $.each(res, function (i, item) {
                        $("<option>").text(item.DocName).val(item.ID).appendTo($sel);
                    });
                }
            });

            $(".add_doc").click(function (e) {
                e.preventDefault();
                var docid = $("#docs option:selected").val();
                if (docid != "0") {
                    window.location.href = "/Doc/Add.aspx?ID=" + docid;
                } else {
                    InfoTip.showMessage("请选择一个文档对象", "warning", 6000);
                }
            });
            $(".list_doc").click(function (e) {
                e.preventDefault();
                var docid = $("#docs option:selected").val();
                if (docid != "0") {
                    var action = request("action");
                    if (action == "admin")
                        window.location.href = "/Doc/TableList.aspx?ID=" + docid;
                    else
                        window.location.href = "/Doc/MyTableList.aspx?ID=" + docid;
                } else {
                    InfoTip.showMessage("请选择一个文档对象", "warning", 6000);
                }
            });
            $(".tran_doc").click(function (e) {
                e.preventDefault();
                var docid = $("#docs option:selected").val();
                if (docid != "0") {
                    window.location.href = "/Doc/Transfer.aspx?ID=" + docid;
                } else {
                    InfoTip.showMessage("请选择一个文档对象", "warning", 6000);
                }
            });
            $(".package_doc").click(function (e) {
                e.preventDefault();
                var docid = $("#docs option:selected").val();
                if (docid != "0") {
                    window.location.href = "/Doc/Package.aspx?ID=" + docid;
                } else {
                    InfoTip.showMessage("请选择一个文档对象", "warning", 6000);
                }
            });
        });
    </script>
</asp:Content>
