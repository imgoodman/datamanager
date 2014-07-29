<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="Add.aspx.cs" Inherits="App.Web.BOM.Add" %>

<%@ Register Src="../Controls/DocObjectSelect.ascx" TagName="DocObjectSelect" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="header">
        <h1 class="pagetitle">
            定义清单</h1>
    </div>
    <div id="steps">
        <ul>
            <li class="selected">第一步：清单基本信息</li>
            <li>第二步：选择主文档对象</li>
            <li>第三步：选择关联文档对象</li>
            <li>第四步：确认保存</li>
        </ul>
        <div class="clear">
        </div>
    </div>
    <div id="step_1" class="step_div">
        <fieldset class="form">
            <legend>清单基本信息</legend>
            <div class="control_group">
                <label class="control_label">
                    名称</label>
                <div class="controls">
                    <input type="text" class="input_medium bomname" />
                </div>
            </div>
            <div class="control_group">
                <label class="control_label">
                    描述</label>
                <div class="controls">
                    <textarea class="input_large multitext desc"></textarea>
                </div>
            </div>
        </fieldset>
        <div class="btnsteps right">
            <a href="#" class="btn btn_primary btn_next_1">下一步</a></div>
    </div>
    <div id="step_2" class="hidden step_div">
        <div id="main_doc" style="padding: 2px; margin-top: 5px;">
            <div id="docobj_sel" style="border: 1px solid #aaa; padding: 2px;">
                <input type="hidden" class="docs_cond" />
                <div class="pull_left" style="width: 240px; overflow: auto;">
                    <div id="jstree_doctype">
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
                                        <td colspan="3">
                                            请先选择文档对象
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </fieldset>
                </div>
                <div class="clear">
                </div>
            </div>
        </div>
        <div class="btnsteps right">
            <a href="#" class="btn btn_pre_1">上一步</a> <a href="#" class="btn btn_primary btn_next_2">
                下一步</a></div>
    </div>
    <div id="step_3" class="hidden step_div">
        <fieldset class="form list">
            <legend>相关文档对象及属性</legend>
            <div class="item_temp hidden">
                <input type="hidden" class="rel_docid" />
                <a href="#" class="btn abs edit" style="right: 60px;">编辑</a> <a href="#" class="btn abs delete_rel_doc"
                    style="right: 5px;">删除</a>
                <ul class="docattrinfo">
                    <li>
                        <label>
                            文档对象:</label><span class="docname">docname</span></li>
                    <li>
                        <label>
                            文档属性:</label>
                    </li>
                </ul>
            </div>
            <div class="more">
                增加相关文档及属性
            </div>
        </fieldset>
        <div class="btnsteps right">
            <a href="#" class="btn btn_pre_2">上一步</a> <a href="#" class="btn btn_primary btn_next_3">
                下一步</a></div>
    </div>
    <div id="step_4" class="hidden step_div list">
        <div class="btnsteps right">
            <a href="#" class="btn btn_prev_3">上一步</a> <a href="#" class="btn btn_primary finish">
                完成</a></div>
    </div>
    <div class="final_mess hidden">
        <p>
            定义清单成功,查看其详情<a href="#"></a>,或者继续<a href="Add.aspx">新增清单</a></p>
    </div>
    <div id="doc_attr_div" class="hidden">
        <uc1:DocObjectSelect ID="DocObjectSelect2" runat="server" />
    </div>
    <div id="confirm_div" class="hidden">
        <p></p>
    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            var bomid = "";
            var bomname = "";
            var maindocid = "";
            var maindocname = "";
            var maindocattrs = "";
            var sourceDocIDs = "";
            var relatedDocids = "";

            var hasLoadDocTypes = false;

            bomid = request("id");
            if (bomid != "") {
                //get info of current bom
                $.ajax({
                    type: "POST",
                    url: "/handlers/bom/getitembyid.ashx",
                    data: { id: bomid },
                    dataType: "json",
                    success: function (res) {
                        //basic info
                        $(".bomname").val(res.Name);
                        bomname = res.Name;
                        $(".desc").val(res.Description);
                        //main doc
                        if (res.MainDoc != null) {
                            maindocid = res.MainDoc.ID;
                            maindocname = res.MainDoc.DocName;
                        }
                        //related doc
                        if (res.RelatedDocs.length > 0) {
                            $.each(res.RelatedDocs, function (i, item) {
                                var $temp = $("#step_3 .item_temp").clone();
                                $temp.find(".docname").html(item.DocName);
                                $temp.find("input[type=hidden]").val(item.ID);
                                var $li = $temp.find("ul li").eq(1);
                                var str = "";
                                $.each(item.RelatedDocAttrs, function (j, jitem) {
                                    str += jitem.AttrName + "(" + jitem.Surname + "),";
                                });
                                $("<span>").html(str).appendTo($li);
                                $temp.removeClass("item_temp hidden").addClass("relative listitem").insertBefore($("#step_3 .more"));
                                //ssss
                            });
                        }
                        //...
                    }
                });
            }

            //save basic info of bom
            $(".btn_next_1").click(function (e) {
                e.preventDefault();
                var name = $(".bomname").val();
                bomname = name;
                var desc = $(".desc").val();
                if (name == "") {
                    InfoTip.showMessage("请输入清单名称", "warning", 6000);
                    return false;
                }
                if (bomid == "") {
                    //add new bom
                    $.ajax({
                        type: "POST",
                        url: "/handlers/bom/addbom_basic.ashx",
                        data: {
                            name: name,
                            desc: desc
                        },
                        success: function (res) {
                            if (res == "0") {
                                InfoTip.showMessage("该清单对象已经存在,请修改清单名称.", "warning", 6000);
                                return false;
                            } else if (res == "-1") {
                                InfoTip.showMessage("保存清单对象出错,请稍后重试.", "error", 6000);
                                return false;
                            } else {
                                InfoTip.showMessage("保存清单基本信息成功,请选择其主对象.", "success", 6000);
                                bomid = res;
                                setCurrentStep(1);

                            }
                        }
                    });
                } else {
                    //update current bom
                    $.ajax({
                        type: "POST",
                        url: "/handlers/bom/updateBOM_Basic.ashx",
                        data: {
                            id: bomid,
                            name: name,
                            desc: desc
                        },
                        success: function (res) {
                            if (res == "1") {
                                InfoTip.showMessage("保存清单基本信息成功,请选择其主对象", "success", 6000);
                                setCurrentStep(1);
                            } else {
                                InfoTip.showMessage("保存清单对象出错,请稍后重试.", "error", 6000);
                                return false;
                            }
                        }
                    });
                }
                //load source docs
                //load source docs
                $.ajax({
                    type: "POST",
                    url: "/handlers/docconfig/getsourcedocs.ashx",
                    dataType: "json",
                    success: function (res_sourcedocs) {
                        var $sel = $("#main_doc select.docs");
                        $sel.empty();
                        sourceDocIDs = "";
                        $("<option>").text("--请选择文档--").val("0").appendTo($sel);
                        $.each(res_sourcedocs, function (i, item) {
                            $("<option>").text(item.DocName).val(item.ID).appendTo($sel);
                            sourceDocIDs += item.ID + ",";
                        });
                        if (sourceDocIDs != "")
                            sourceDocIDs = sourceDocIDs.substr(0, sourceDocIDs.length - 1);
                        $("#main_doc .docs_cond").val(sourceDocIDs);
                        if (maindocid != "") {
                            $sel.val(maindocid);
                            //get its attrs
                            $.ajax({
                                type: "POST",
                                url: "/handlers/docconfig/getitembyid.ashx",
                                data: { id: maindocid },
                                dataType: "json",
                                success: function (res_doc) {
                                    var $tbody = $("#main_doc table tbody");
                                    $tbody.empty();
                                    $.each(res_doc.Attrs, function (i, item) {
                                        var $tr = $("<tr>").html("<td><input type='checkbox' value='" + item.ID + "' /><td>" + item.AttrName + "</td><td><input type='text' class='input_small' /></td>");
                                        $tr.appendTo($tbody);
                                        $.ajax({
                                            type: "POST",
                                            url: "/handlers/bom/getmaindocattr.ashx",
                                            data: {
                                                id: bomid,
                                                attrid: item.ID
                                            },
                                            dataType: "json",
                                            success: function (res_attr) {
                                                if (res_attr != null) {
                                                    $tr.addClass("selected");
                                                    $tr.find("input[type=checkbox]").prop({ "checked": "checked" });
                                                    $tr.find("input[type=text]").val(res_attr.Surname);
                                                } else {
                                                    $tr.addClass("sel");
                                                }
                                            }
                                        });
                                    });
                                    //sds
                                }
                            });

                        }
                    }
                });
                //load doctypes in step 2               
                if (!hasLoadDocTypes) {
                    $.ajax({
                        type: "POST",
                        url: "/handlers/docType/getDocTypes.ashx",
                        data: { isVirtual: $("#isVirtual").val() },
                        dataType: "json",
                        success: function (res) {
                            hasLoadDocTypes = true;
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
                                    data: { typeid: selDocTypeID, docCond: $("#main_doc .docs_cond").val() },
                                    dataType: "json",
                                    success: function (res_docs) {
                                        var $sel = $("#main_doc .docs");
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
                    //show attrs
                    $("#main_doc .docs").change(function (e) {
                        var docid = $(this).children("option:selected").val();
                        if (docid == "0") {
                            $("#main_doc table tbody tr").remove();
                            return false;
                        }
                        $.ajax({
                            type: "POST",
                            url: "/handlers/docconfig/getitembyid.ashx",
                            data: { id: docid },
                            dataType: "json",
                            success: function (res) {
                                $("#docobj_sel table tbody tr").remove();
                                $.each(res.Attrs, function (i, item) {
                                    $("<tr>").addClass("sel").html("<td><input type='checkbox' value='" + item.ID + "' /></td><td>" + item.AttrName + "</td><td><input type='text' class='input_small' value='" + res.DocName + "(" + item.AttrName + ")' /></td>").appendTo($("#docobj_sel table tbody"));
                                });
                            }
                        });
                    });
                    //select attrs
                    $("body").on("click", "#docobj_sel table tbody tr td input[type=checkbox]", function (e) {
                        var $tr = $(this).parents("tr");
                        if ($(this).prop("checked"))
                            $tr.removeClass("sel").addClass("selected");
                        else
                            $tr.removeClass("selected").addClass("sel");
                    });
                }
            });
            /***************************methods in step 2*********************************/
            //previous step
            $(".btn_pre_1").click(function (e) {
                e.preventDefault();
                setCurrentStep(0);
            });
            //save main doc and attrs
            $(".btn_next_2").click(function (e) {
                e.preventDefault();
                maindocid = $("#main_doc select.docs").children("option:selected").val();
                maindocname = $("#main_doc select.docs").children("option:selected").text();
                if (maindocid == "0") {
                    InfoTip.showMessage("请选择清单的主文档对象", "warning", 6000);
                    return false;
                }
                var $cbs = $("#main_doc table tbody input[type=checkbox]:checked");
                if ($cbs.length > 0) {
                    //each surname is not empty
                    var isvalid = true;
                    $cbs.each(function (i, item) {
                        var $tr = $(item).parents("tr");
                        if ($tr.find("input[type=text]").val() == "") {
                            InfoTip.showMessage("属性的显示名称不能为空", "warning", 6000);
                            isvalid = false;
                            return false;
                        }
                    });
                    if (isvalid) {
                        //get related docs
                        maindocattrs = "";
                        $.ajax({
                            type: "POST",
                            url: "/handlers/bom/getRelatedDocsByMainDocID.ashx",
                            data: { id: maindocid },
                            dataType: "json",
                            success: function (res) {
                                relatedDocids = "";
                                $.each(res, function (i, item) {
                                    relatedDocids += item.ID + ",";
                                });
                                if (relatedDocids != "")
                                    relatedDocids = relatedDocids.substr(0, relatedDocids.length - 1);

                                $("#doc_attr_div .docs_cond").val(relatedDocids);
                                //check whether current related docs are related to the current main doc
                                var $rel_docs = $("#step_3 .list .listitem");
                                if ($rel_docs.length > 0) {
                                    var totalRelDocs = "," + relatedDocids + ",";
                                    $rel_docs.each(function (i, item) {
                                        var reldocid = "," + $(item).find(".rel_docid").val() + ",";
                                        if (totalRelDocs.indexOf(reldocid) < 0) {
                                            //not related
                                            $(item).addClass("not_rel");
                                            $(item).find(".edit").remove();
                                            $("<p>").addClass("red").html("当前文档与主文档没有关联,请删除该文档").appendTo($(item));
                                        }
                                    });
                                }
                            }
                        });
                        //add main doc
                        $.ajax({
                            type: "POST",
                            url: "/handlers/bom/updateBOM_MainDoc.ashx",
                            data: {
                                id: bomid,
                                maindocid: maindocid
                            },
                            success: function (res) { }
                        });
                        //clear old main doc attrs and set new values
                        $.ajax({
                            type: "POST",
                            url: "/handlers/bom/clearbBOM_MainDocAttr.ashx",
                            data: { id: bomid },
                            success: function (res) {
                                //add attrs of main doc
                                $cbs.each(function (i, item) {
                                    var attrid = $(item).val();
                                    var surname = $(item).parents("tr").find("input[type=text]").val();
                                    var attrname = $(item).parents("tr").find("td").eq(1).html();
                                    maindocattrs += attrname + "(" + surname + "),";
                                    $.ajax({
                                        type: "POST",
                                        url: "/handlers/bom/addBOM_MainDocAttr.ashx",
                                        data: {
                                            id: bomid,
                                            attrid: attrid,
                                            surname: surname
                                        },
                                        success: function (res_attr) { }
                                    });
                                });
                                setCurrentStep(2);
                            }
                        });


                    }
                } else {
                    InfoTip.showMessage("请选择文档的属性", "warning", 6000);
                    return false;
                }
            });
            /***********************************methods in step 3****************************/
            //previous step
            $(".btn_pre_2").click(function (e) {
                e.preventDefault();
                setCurrentStep(1);
            });
            //edit a related doc
            $("body").on("click", ".listitem .edit", function (e) {
                e.preventDefault();
                var reldocid = $(this).prev().val();
                var $parent = $(this).parent();
                var $div = $("#doc_attr_div");
                //get related docs
                $.ajax({
                    type: "POST",
                    url: "/handlers/bom/getRelatedDocsByMainDocID.ashx",
                    data: { id: maindocid },
                    dataType: "json",
                    success: function (res) {
                        var $sel = $("#doc_attr_div .docs");
                        $sel.empty();
                        $("<option>").text("--请选择文档--").val("0").appendTo($sel);
                        $.each(res, function (i, item) {
                            $("<option>").text(item.DocName).val(item.ID).appendTo($sel);
                        });
                        $sel.val(reldocid);
                        //set attrs
                        $.ajax({
                            type: "POST",
                            url: "/handlers/docconfig/getitembyid.ashx",
                            data: { id: reldocid },
                            dataType: "json",
                            success: function (res_doc) {
                                var $tbody = $div.find("table tbody");
                                $tbody.empty();
                                $.each(res_doc.Attrs, function (i, item) {
                                    var $tr = $("<tr>").html("<td><input type='checkbox' value='" + item.ID + "' /><td>" + item.AttrName + "</td><td><input type='text' class='input_small' /></td>");
                                    $tr.appendTo($tbody);
                                    $.ajax({
                                        type: "POST",
                                        url: "/handlers/bom/getRelateddocattr.ashx",
                                        data: {
                                            id: bomid,
                                            docid: reldocid,
                                            attrid: item.ID
                                        },
                                        dataType: "json",
                                        success: function (res_attr) {
                                            if (res_attr != null) {
                                                $tr.addClass("selected");
                                                $tr.find("input[type=checkbox]").prop({ "checked": "checked" });
                                                $tr.find("input[type=text]").val(res_attr.Surname);
                                            } else {
                                                $tr.addClass("sel");
                                            }
                                        }
                                    });
                                });
                                //sds
                            }
                        });
                    }
                });
                $div.dialog({
                    title: "选择文档对象及属性",
                    modal: true,
                    width: 600,
                    minHeight: 100,
                    buttons: {
                        "确认": function () {
                            var relatedDocID = $div.find("select.docs").children("option:selected").val();
                            var docname = $div.find("select.docs").children("option:selected").text();
                            var $cbs = $("#main_doc table tbody input[type=checkbox]:checked");
                            if ($cbs.length > 0) {
                                //each surname is not empty
                                var isvalid = true;
                                $cbs.each(function (i, item) {
                                    var $tr = $(item).parents("tr");
                                    if ($tr.find("input[type=text]").val() == "") {
                                        InfoTip.showMessage("属性的显示名称不能为空", "warning", 6000);
                                        isvalid = false;
                                        return false;
                                    }
                                });
                                if (isvalid) {
                                    //current doc is related or not
                                    $.ajax({
                                        type: "POST",
                                        url: "/handlers/bom/updateBOM_RelatedDoc.ashx",
                                        data: {
                                            id: bomid,
                                            olddocid: reldocid,
                                            newdocid: relatedDocID
                                        },
                                        success: function (res) {
                                            //not exist
                                            if (res == "0") {
                                                //repeat
                                                InfoTip.showMessage("该文档已经存在于当前清单", "warning", 6000);
                                                return false;
                                            } else {
                                                //update related doc successfully
                                                var bomdocid = res;
                                                //clear old attrs
                                                $.ajax({
                                                    type: "POST",
                                                    url: "/handlers/bom/clearRelatedDocAttrs.ashx",
                                                    data: { id: bomdocid },
                                                    success: function (res_clr) {
                                                        //add attrs of related doc
                                                        if (res_clr == "0") {
                                                            InfoTip.showMessage("重置相关文档属性失败", "error", 6000);
                                                            return false;
                                                        }
                                                        $cbs.each(function (i, item) {
                                                            var attrid = $(item).val();
                                                            var surname = $(item).parents("tr").find("input[type=text]").val();
                                                            $.ajax({
                                                                type: "POST",
                                                                url: "/handlers/bom/addBOM_RelatedDocAttr.ashx",
                                                                data: {
                                                                    id: bomdocid,
                                                                    attrid: attrid,
                                                                    surname: surname
                                                                },
                                                                success: function (res_attr) { }
                                                            });
                                                        });
                                                        //dialog close
                                                        var $temp = $("#step_3 .item_temp").clone();
                                                        $temp.find("input[type=hidden]").val(relatedDocID);
                                                        $temp.find(".docname").html(docname);
                                                        var $li = $temp.find("ul li").eq(1);
                                                        var str = "";
                                                        $cbs.each(function (i, item) {
                                                            var attrname = $(item).parents("tr").find("td").eq(1).html();
                                                            var surname = $(item).parents("tr").find("input[type=text]").val();
                                                            str += attrname + "(" + surname + "),";
                                                        });
                                                        $("<span>").html(str).appendTo($li);
                                                        //console.log(str);
                                                        $temp.removeClass("item_temp hidden").addClass("relative listitem").insertBefore($("#step_3 .more"));
                                                        $parent.remove();
                                                        $div.dialog("close");
                                                        InfoTip.showMessage("编辑相关文档成功", "success", 6000);
                                                    }
                                                });

                                            }
                                        }
                                    });
                                }
                            } else {
                                InfoTip.showMessage("请选择文档的属性", "warning", 6000);
                                return false;
                            }

                        },
                        "取消": function () { $div.dialog("close"); }
                    }
                });
            });
            //delete a related doc
            $("body").on("click", ".listitem .delete_rel_doc", function (e) {
                e.preventDefault();
                var reldocid = $(this).prev().prev().val();
                var $parent = $(this).parent();
                var $div = $("#confirm_div");
                $div.find("p").html("您确定删除该相关文档吗?");
                $div.dialog({
                    title: "删除相关文档",
                    modal: true,
                    width: 600,
                    minHeight: 100,
                    buttons: {
                        "确认": function () {
                            $.ajax({
                                type: "POST",
                                url: "/handlers/bom/deleteRelatedDoc.ashx",
                                data: {
                                    id: bomid,
                                    docid: reldocid
                                },
                                success: function (res) {
                                    if (res == "1") {
                                        $div.dialog("close");
                                        $parent.slideUp("slow");
                                        InfoTip.showMessage("删除相关文档成功", "success", 6000);
                                    } else {
                                        InfoTip.showMessage("删除相关文档失败", "error", 6000);
                                    }
                                }
                            });
                        },
                        "取消": function () { $div.dialog("close"); }
                    }
                });
            });
            //more related doc
            $(".more").click(function () {
                var $div = $("#doc_attr_div");
                //get related docs
                $.ajax({
                    type: "POST",
                    url: "/handlers/bom/getRelatedDocsByMainDocID.ashx",
                    data: { id: maindocid },
                    dataType: "json",
                    success: function (res) {
                        var $sel = $("#doc_attr_div .docs");
                        $sel.empty();
                        $("<option>").text("--请选择文档--").val("0").appendTo($sel);
                        $.each(res, function (i, item) {
                            $("<option>").text(item.DocName).val(item.ID).appendTo($sel);
                        });
                    }
                });
                //clear attrs
                $div.find("table tbody tr").remove();
                $("<tr>").addClass("nothing").html("<td colspan='3'>请先选择文档对象</td>").appendTo($div.find("table tbody"));
                $div.dialog({
                    title: "选择文档对象及属性",
                    modal: true,
                    width: 600,
                    minHeight: 100,
                    buttons: {
                        "确认": function () {
                            var relatedDocID = $div.find("select.docs").children("option:selected").val();
                            var docname = $div.find("select.docs").children("option:selected").text();
                            var $cbs = $div.find("table tbody input[type=checkbox]:checked");
                            if ($cbs.length > 0) {
                                //each surname is not empty
                                var isvalid = true;
                                $cbs.each(function (i, item) {
                                    var $tr = $(item).parents("tr");
                                    if ($tr.find("input[type=text]").val() == "") {
                                        InfoTip.showMessage("属性的显示名称不能为空", "warning", 6000);
                                        isvalid = false;
                                        return false;
                                    }
                                });
                                if (isvalid) {
                                    //current doc is related or not
                                    $.ajax({
                                        type: "POST",
                                        url: "/handlers/bom/addBOM_RelatedDoc.ashx",
                                        data: {
                                            id: bomid,
                                            docid: relatedDocID
                                        },
                                        success: function (res) {
                                            //not exist
                                            if (res == "0") {
                                                //repeat
                                                InfoTip.showMessage("该文档已经存在于当前清单", "warning", 6000);
                                                return false;
                                            } else if (res == "-1") {
                                                InfoTip.showMessage("保存相关文档出错", "error", 6000);
                                                return false;
                                            } else {
                                                //add related doc successfully
                                                //add attrs of related doc
                                                $cbs.each(function (i, item) {
                                                    var attrid = $(item).val();
                                                    var surname = $(item).parents("tr").find("input[type=text]").val();
                                                    $.ajax({
                                                        type: "POST",
                                                        url: "/handlers/bom/addBOM_RelatedDocAttr.ashx",
                                                        data: {
                                                            id: res,
                                                            attrid: attrid,
                                                            surname: surname
                                                        },
                                                        success: function (res_attr) { }
                                                    });
                                                });
                                                //dialog close
                                                var $temp = $("#step_3 .item_temp").clone();
                                                $temp.find("input[type=hidden]").val(relatedDocID);
                                                $temp.find(".docname").html(docname);
                                                var $li = $temp.find("ul li").eq(1);
                                                var str = "";
                                                $cbs.each(function (i, item) {
                                                    var attrname = $(item).parents("tr").find("td").eq(1).html();
                                                    var surname = $(item).parents("tr").find("input[type=text]").val();
                                                    str += attrname + "(" + surname + "),";
                                                });
                                                $("<span>").html(str).appendTo($li);
                                                //console.log(str);
                                                $temp.removeClass("item_temp hidden").addClass("relative listitem").insertBefore($("#step_3 .more"));
                                                $div.dialog("close");
                                                InfoTip.showMessage("保存相关文档成功", "success", 6000);
                                            }
                                        }
                                    });
                                }
                            } else {
                                InfoTip.showMessage("请选择文档的属性", "warning", 6000);
                                return false;
                            }

                        },
                        "取消": function () { $div.dialog("close"); }
                    }
                });

            });
            //save related doc and attrs
            $(".btn_next_3").click(function (e) {
                e.preventDefault();
                //check no relaed docs
                if ($("#step_3 .listitem").length == 0) {
                    InfoTip.showMessage("请增加相关文档及属性", "warning", 6000);
                    return false;
                }
                if ($("#step_3 .not_rel").length > 0) {
                    InfoTip.showMessage("当前部分文档与主文档无关,请删除", "warning", 6000);
                    return false;
                }
                var $holder = $("#step_4 .btnsteps");
                //basic info
                var $basic = $("#step_1 fieldset").clone();
                $basic.find("input[type=text],textarea").prop({ "disabled": "disabled" });
                $basic.insertBefore($holder);
                //main doc
                $("<p>").html("主文档对象及属性").insertBefore($holder);
                var $temp = $(".item_temp").clone();
                $temp.find(".docname").html(maindocname);
                var $li = $temp.find("ul li").eq(1);
                $("<span>").html(maindocattrs).appendTo($li);
                $temp.find("input[type=hidden],a").remove();
                $temp.removeClass("item_temp hidden").addClass("listitem").insertBefore($holder);
                //related docs
                $("<p>").html("相关文档对象及属性").insertBefore($holder);
                var $relateddocs = $("#step_3 .listitem").clone();
                $relateddocs.find("input[type=hidden],a").remove();
                $relateddocs.insertBefore($holder);
                setCurrentStep(3);
            });
            /**********************************methods in step 4 **************************/
            $(".finish").click(function (e) {
                e.preventDefault();
                $.ajax({
                    type: "POST",
                    url: "/handlers/bom/activateBOM.ashx",
                    data: { id: bomid },
                    success: function (res) {
                        $(".step_div").hide();
                        $(".final_mess a").eq(0).text(bomname).prop({ "href": "/BOM/Profile.aspx?ID=" + bomid });
                        $(".final_mess").show();
                    }
                });
            });
        });
        //other functions
        function setCurrentStep(stepIndex) {
            $("#steps li").removeClass("selected");
            $("#steps li").eq(stepIndex).addClass("selected");
            $(".step_div").hide();
            $("#step_" + (stepIndex + 1)).show();
        };
    </script>
</asp:Content>
