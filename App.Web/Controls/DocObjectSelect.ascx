<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DocObjectSelect.ascx.cs"
    Inherits="App.Web.Controls.DocObjectSelect" %>
<div id="docobj_sel_uc" style="border:1px solid #aaa; padding:2px;">
    <input type="hidden" class="docs_cond" />
    <div class="pull_left" style="width: 240px; overflow: auto;">
        <div class="jstree_doctype">
        </div>
    </div>
    <div class="pull_left" style="width: 500px;">
        <fieldset class="form">
            <legend>选择文档对象及属性</legend>
            <div class="control_group">
                <label class="control_label">
                    文档对象</label>
                <div class="controls">
                    <select class="input_medium docs">
                        <option value="0">--请选择文档对象--</option>
                    </select>
                </div>
            </div>
            <div class="list">
                <table>
                    <thead>
                        <tr>
                            <th>
                                选择
                            </th>
                            <th>
                                属性名
                            </th>
                            <th>
                                属性显示名称
                            </th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr class="nothing">
                            <td colspan="3">请先选择文档对象</td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </fieldset>
    </div>
    <div class="clear">
    </div>
</div>
<script type="text/javascript">
    $(document).ready(function () {
        //load doc types
        $.ajax({
            type: "POST",
            url: "/handlers/docType/getDocTypes.ashx",
            data: { isVirtual: $("#isVirtual").val() },
            dataType: "json",
            success: function (res) {
                var $tree = $("#docobj_sel_uc .jstree_doctype").jstree({
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
                        data: { typeid: selDocTypeID, docCond: $("#docobj_sel_uc .docs_cond").val() },
                        dataType: "json",
                        success: function (res_docs) {
                            var $sel = $("#docobj_sel_uc .docs");
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
                //open all
                $.each(res, function (dindex, dt) {
                    if (dt.parent == "#") {
                        $tree.jstree("open_all", "#" + dt.id);
                    }
                });
            }
        });
        //load all documents
        $.ajax({
            type: "POST",
            url: "/handlers/doc/getallbasicdocs.ashx",
            data: { isVirtual: $("#isVirtual").val() },
            dataType: "json",
            success: function (res) {
                var $sel = $("#docobj_sel_uc .docs");
                $sel.empty();
                $("<option>").text("--请选择文档--").val("0").appendTo($sel);
                $.each(res, function (i, item) {
                    $("<option>").text(item.DocName).val(item.ID).appendTo($sel);
                });
            }
        });
        //show attrs
        $("#docobj_sel_uc .docs").change(function (e) {
            var docid = $(this).children("option:selected").val();
            if (docid == "0") {
                $("#docobj_sel_uc table tbody tr").remove();
                return false;
            }
            $.ajax({
                type: "POST",
                url: "/handlers/docconfig/getitembyid.ashx",
                data: { id: docid },
                dataType: "json",
                success: function (res) {
                    $("#docobj_sel_uc table tbody tr").remove();
                    $.each(res.Attrs, function (i, item) {
                        $("<tr>").addClass("sel").html("<td><input type='checkbox' value='" + item.ID + "' /></td><td>" + item.AttrName + "</td><td><input type='text' class='input_small' value='" + res.DocName + "(" + item.AttrName + ")' /></td>").appendTo($("#docobj_sel_uc table tbody"));
                    });
                }
            });
        });
        //select attrs
        $("body").on("click", "#docobj_sel_uc table tbody tr td input[type=checkbox]", function (e) {
            var $tr = $(this).parents("tr");
            if ($(this).prop("checked"))
                $tr.removeClass("sel").addClass("selected");
            else
                $tr.removeClass("selected").addClass("sel");
        });
    });
</script>