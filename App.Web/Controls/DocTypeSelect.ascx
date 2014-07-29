<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DocTypeSelect.ascx.cs"
    Inherits="App.Web.Controls.DocTypeSelect" %>
<div id="uc_docTypeSel">
    <input type="text" value="暂未选择文档类型" disabled="disabled" class="input_medium" />
    <a href="#" class="btn docTypeSel">选择</a> <a href="#" class="btn docTypeClr">清除选择</a>
    <input type="hidden" id="selDocType" />
</div>
<div id="docType_div" class="hidden">
    <input type="hidden" class="tempDocType" />
    <input type="hidden" class="tempLeaf" />
    <p>
        您选择了文档类型:<span></span></p>
    <div id="jstree_doctype">
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
                var $tree = $("#jstree_doctype").jstree({
                    "core": {
                        "data": res,
                        "multiple": false
                    }
                }).one("loaded.jstree", function (e, data) {
                    //data.instance.open_all();
                    //console.log("aaa");
                }).on("changed.jstree", function (e, data) {
                    //点击事件
                    var i, j, r = [];
                    for (i = 0, j = data.selected.length; i < j; i++) {
                        r.push(data.instance.get_node(data.selected[i]).id);
                    }
                    var selDocTypeID = r[0];
                    var sel_node = $("#" + selDocTypeID);
                    if (!sel_node.hasClass("jstree-leaf")) {
                        //非页节点
                        $("#docType_div .tempLeaf").val("0");
                        $("#docType_div p span").text(data.instance.get_node(selDocTypeID).text + ",请选择其下属文档类型");
                    } else {
                        $("#docType_div p span").text(data.instance.get_node(selDocTypeID).text);
                        selDocTypeID = selDocTypeID.replace('d', '');
                        $("#docType_div .tempDocType").val(selDocTypeID);
                        $("#docType_div .tempLeaf").val("1");
                    }
                });

                //open all
                $.each(res, function (dindex, dt) {
                    if (dt.parent == "#") {
                        $tree.jstree("open_all", "#" + dt.id);
                    }
                });
            }
        });

        //选择类型
        $(".docTypeSel").click(function (e) {
            e.preventDefault();
            var $div = $("#docType_div");
            $div.dialog({
                title: "选择文档类型",
                modal: true,
                width: 600,
                minHeight: 100,
                maxHeight: 400,
                buttons: {
                    "确认选择": function () {
                        if ($div.find(".tempDocType").val() == "") {
                            InfoTip.showMessage("请选择合适的文档类型", "warning", 6000);
                            return false;
                        }

                        var isleaf = $div.find(".tempLeaf").val();
                        if (isleaf == "1") {
                            $("#uc_docTypeSel input[type=text]").val($div.find("p span").text());
                            $("#selDocType").val($div.find(".tempDocType").val());
                            $div.dialog("close");
                        } else {
                            InfoTip.showMessage("您选择的文档类型还能细分,请选择不能再细分的类型", "warning", 6000);
                        }
                    },
                    "取消": function () { $div.dialog("close"); }
                }
            });
        });
        //清除选择
        $(".docTypeClr").click(function (e) {
            e.preventDefault();
            $("#selDocType").val("");
            $("#uc_docTypeSel input[type=text]").val("暂未选择文档类型");
        });
    });
</script>
