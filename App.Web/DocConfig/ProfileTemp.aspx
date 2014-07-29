<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ProfileTemp.aspx.cs" Inherits="App.Web.DocConfig.ProfileTemp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div id="formContent">
        <div class="header relative">
            <h1 class="pagetitle">
                文档详细信息</h1>
            <a href="#" class="btn btn_primary btn_del abs del_doc" title="删除该文档">删除</a>
        </div>
        <p class="message warning hidden">
            <span class="ui-icon ui-icon-info" style="float: left; margin-right: .3em;"></span>
            <strong>Hey!</strong></p>
        <fieldset class="form detailinfo relative">
            <legend>文档基本信息</legend>
            <div class="abs btns">
                <a href="#" class="btn btn_primary edit_doc">编辑</a> <a href="#" class="btn btn_primary save_doc"
                    style="display: none;">保存</a> <a href="#" class="btn btn_primary cancel_doc" style="display: none;">
                        取消</a>
            </div>
            <div class="control_group">
                <label class="control_label">
                    文档类别</label>
                <div class="controls">
                    <label class="docType">
                        xxx
                    </label>
                    <div id="jstree_doctype" class="hidden" style="max-height:100px;">
                    </div>
                </div>
            </div>
            <div class="control_group">
                <label class="control_label">
                    文档名称</label>
                <div class="controls">
                    <label class="docName">
                        xxx
                    </label>
                    <input type="text" class="input_medium hidden" id="docName" />
                </div>
            </div>
            <div class="control_group">
                <label class="control_label">
                    描述</label>
                <div class="controls">
                    <label class="desc">
                        xxx
                    </label>
                    <textarea class="input_large multitext hidden" id="desc"></textarea>
                </div>
            </div>
        </fieldset>
        <fieldset class="form detailinfo">
            <legend>文档属性信息</legend>
            <div class="listattr">
                <div class="attritem itemtemp hidden relative">
                    <div class="abs btns hidden">
                        <a href="#" class="btn btn_primary">编辑</a> <a href="#" class="btn btn_primary">删除</a>
                    </div>
                    <div class="abs order_action ui-state-default">
                        <span class="ui-icon ui-icon-arrowthick-1-n attr_up" title="上移该属性"></span><span class="ui-icon ui-icon-arrowthick-2-e-w attr_follow hidden"
                            title="跟随前一个属性/取消跟随"></span><span class="ui-icon ui-icon-arrowthick-1-s attr_down"
                                title="下移该属性"></span><span class="ui-icon ui-icon-pencil attr_edit" title="编辑该属性">
                        </span><span class="ui-icon ui-icon-circle-close attr_del" title="删除该属性"></span>
                    </div>
                    <input type="hidden" />
                    <ul>
                        <li>
                            <label>
                                属性名称</label><span class="attrname"> xxx</span> </li>
                        <li>
                            <label>
                                属性类型</label><span class="attrtype"> xxx</span> </li>
                        <li>
                            <label>
                                是否内容必填</label><span class="isrequired"> xxx</span> </li>
                        <li>
                            <label>
                                是否关键搜索属性</label><span class="issearch"> xxx</span> </li>
                    </ul>
                </div>
            </div>
            <div class="more">
                继续增加属性
            </div>
        </fieldset>
    </div>
    <div id="form" class="form hidden">
        <input type="hidden" class="attrid" />
        <div class="attritem relative">
            <div class="control_group">
                <label class="control_label">
                    属性名称</label>
                <div class="controls">
                    <input type="text" class="input_medium attrname" />
                    <p class="help_block">
                        属性名称比如是“编号”，“标题”等。</p>
                </div>
            </div>
            <div class="control_group">
                <label class="control_label">
                    属性类型</label>
                <div class="controls" style="padding-top: 3px;">
                    <input type="radio" title="内容简单文本" name="attrType" value="Text" /><span>单字符串</span>
                    <input type="radio" title="内容为复杂文本" name="attrType" value="RichText" /><span>多字符串</span>
                    <input type="radio" title="仅一个人参与" name="attrType" value="Person" /><span>单人</span>
                    <input type="radio" title="多个人参与" name="attrType" value="MultiPerson" /><span>多人参与</span>
                    <input type="radio" title="日期时间" name="attrType" value="Date" /><span>日期</span>
                    <input type="radio" title="文件" name="attrType" value="File" /><span>附件</span>
                    <input type="radio" title="枚举型属性值" name="attrType" value="EnumVal" /><span>枚举</span>
                    <div class="attr_content hidden" style="margin-top: 5px;">
                        <input type="text" title="输入枚举类型的值" class="attrval" /><a href="#" class="btn attrval_add"
                            title="新增属性值">新增</a>
                    </div>
                </div>
            </div>
            <div class="control_group">
                <label class="control_label">
                    是否内容必填</label>
                <div class="controls" style="padding-top: 3px;">
                    <input type="radio" title="内容不能为空" name="isRequired" value="true" /><span>是</span>
                    <input type="radio" title="内容允许为空" name="isRequired" value="false" /><span>否</span>
                </div>
            </div>
            <div class="control_group">
                <label class="control_label">
                    是否搜索关键属性</label>
                <div class="controls" style="padding-top: 3px;">
                    <input type="radio" title="通常是单文本，日期，单人，多人属性" name="isSearch" value="true" /><span>是</span>
                    <input type="radio" title="通常是多文本和附件属性" name="isSearch" value="false" /><span>否</span>
                </div>
            </div>
        </div>
    </div>
    <div class="result hidden">
        <p>
            恭喜您，您已经成功删除了一个文档。查看<a href="/DocConfig/List.aspx" class="docLink">文档列表</a></p>
        <p>
            您还可以继续新增文档,<a href="/DocConfig/Add.aspx">新增文档</a></p>
    </div>
    <div id="del_confirm" class="hidden">
        <p>
            您确定删除该文档吗？</p>
    </div>
    <div id="attrval_div" class="hidden form">
        <div class="control_group">
            <label class="control_label">
                属性值</label>
            <div class="controls">
                <input type="text" class="input_medium" />
            </div>
        </div>
    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            var id = request("id");
            id = id.replace('#', '');
            var userid = $("#userid").val();
            var lastOrder;
            var selDocTypeID = "";
            var selDocTypeName = "";
            //load doc types
            $.ajax({
                type: "POST",
                url: "/handlers/docType/getDocTypes.ashx",
                data: {isVirtual:$("#isVirtual").val()},
                dataType: "json",
                success: function (res) {
                    $("#jstree_doctype").jstree({
                        "core": {
                            "data": res
                        }
                    }).on("changed.jstree", function (e, data) {
                        //点击事件
                        var i, j, r = [];
                        for (i = 0, j = data.selected.length; i < j; i++) {
                            r.push(data.instance.get_node(data.selected[i]).id);
                        }
                        //alert('Selected: ' + r.join(', '));
                        selDocTypeID = r[0];
                        selDocTypeName = data.instance.get_node(selDocTypeID).text;
                        selDocTypeID = selDocTypeID.replace('d', '');
                        //console.log(selDocTypeName);
                    });
                }
            });
            //alert(id);
            //加载文档信息 
            $.ajax({
                type: "POST",
                url: "/handlers/tempdocConfig/getItemById.ashx",
                data: { id: id },
                dataType: "json",
                success: function (res) {
                    if (res != null) {
                        $(".docName").html(res.DocName);
                        $(".docType").html(res.DocType.TypeName);
                        selDocTypeID = res.DocType.ID;
                        selDocTypeName = res.DocType.TypeName;
                        $("#docName").val(res.DocName);
                        $(".desc").html(showString(res.Description));
                        $("#desc").val(showString(res.Description));
                        $.each(res.Attrs, function (i, item) {
                            var $temp = $(".itemtemp").clone();
                            $temp.find(".attrname").text(item.AttrName);
                            if (showType(item.AttrType) == "枚举") {
                                var attrvals = showType(item.AttrType) + "{枚举值为:";
                                $.each(item.AttrVals, function (j, jitem) {
                                    attrvals += jitem.AttrValue + ",";
                                });
                                attrvals = attrvals.substr(0, attrvals.length - 1);
                                attrvals += "}";
                                $temp.find(".attrtype").text(attrvals);
                            } else
                                $temp.find(".attrtype").text(showType(item.AttrType));

                            $temp.find(".isrequired").text(showBool(item.IsRequired));
                            $temp.find(".issearch").text(showBool(item.IsSearch));
                            $temp.find("input[type=hidden]").val(item.ID);
                            lastOrder = item.VerticalOrder;
                            $temp.removeClass("hidden itemtemp").appendTo($(".listattr"));
                        });
                    } else {
                        window.location.href = "/404.aspx";
                    }
                }
            });
            //删除文档
            $(".del_doc").click(function (e) {
                e.preventDefault();
                var $div = $("#del_confirm");
                $div.dialog({
                    title: "删除文档",
                    modal: true,
                    width: 600,
                    minHeight: 100,
                    buttons: {
                        "确认": function () {
                            $.ajax({
                                type: "POST",
                                url: "/handlers/tempdocConfig/DisableDoc.ashx",
                                data: { id: id, userid: userid },
                                dataType: "",
                                success: function (res) {
                                    if (res == "1") {
                                        //window.location.href = "/DocConfig/List.aspx";
                                        $div.dialog("close");
                                        InfoTip.showMessage("删除文档成功", "success", 6000);
                                        $("#formContent").slideUp("fast", function () {
                                            $(".result").removeClass("hidden");
                                        });
                                    } else {
                                        showMessage("删除文档失败。");
                                        InfoTip.showMessage("删除文档失败", "error", 6000);
                                    }
                                }
                            });
                        },
                        "取消": function () { $div.dialog("close"); }
                    }
                });
            });
            //编辑基本信息
            $(".edit_doc").click(function (e) {
                e.preventDefault();
                $(".docType").hide();
                $("#jstree_doctype").show();
                $(".docName").addClass("hidden");
                $("#docName").removeClass("hidden");
                $(".desc").addClass("hidden");
                $("#desc").removeClass("hidden");
                $(".save_doc,.cancel_doc").show();
                $(this).hide();
            });
            //取消编辑基本信息
            $(".cancel_doc").click(function (e) {
                e.preventDefault();
                $(".docType").show();
                $("#jstree_doctype").hide();
                $(".docName").removeClass("hidden");
                $("#docName").addClass("hidden");
                $(".desc").removeClass("hidden");
                $("#desc").addClass("hidden");
                $(this).hide();
                $(".save_doc").hide();
                $(".edit_doc").show();
            });
            //验证修改后的文档是否存在
            var isDocExist = false;
            $("#docName").blur(function () {
                $.ajax({
                    type: "POST",
                    url: "/handlers/tempdocConfig/IsUpdateExist.ashx",
                    data: { docName: $(this).val(), id: id },
                    dataType: "json",
                    success: function (res) {
                        if (res == "1") {
                            showMessage("该文档名称已经存在，请修改。");
                            InfoTip.showMessage("该文档名称已经存在，请修改", "warning", 6000);
                            isDocExist = true;
                        } else {
                            hideMessage();
                            isDocExist = false;
                        }
                    }
                });
            });
            //确认修改基本信息
            $(".save_doc").click(function (e) {
                e.preventDefault();
                var isValid = true;
                if (isDocExist) {
                    showMessage("该文档名称已经存在，请修改。");
                    InfoTip.showMessage("该文档名称已经存在，请修改", "warning", 6000);
                    isValid = false;
                    return false;
                }
                var docName = $("#docName").val();
                if (docName == "") {
                    showMessage("文档名称不能为空");
                    InfoTip.showMessage("文档名称不能为空", "warning", 6000);
                    isValid = false;
                    return false;
                }
                if (isValid) {
                    $.ajax({
                        type: "POST",
                        url: "/handlers/tempdocConfig/UpdateDoc.ashx",
                        data: { docName: docName, id: id, desc: $("#desc").val(), userid: userid, docType: selDocTypeID },
                        dataType: "",
                        success: function (res) {
                            if (res == "1") {
                                $(".docType").show();
                                $(".docType").html(selDocTypeName);
                                $("#jstree_doctype").hide();
                                $(".docName").removeClass("hidden");
                                $("#docName").addClass("hidden");
                                $(".docName").html($("#docName").val());
                                $(".desc").removeClass("hidden");
                                $("#desc").addClass("hidden");
                                $(".desc").html(showString($("#desc").val()));
                                $(this).hide();
                                $(".cancel_doc").hide();
                                $(".edit_doc").show();
                                showMessage("成功编辑文档基本信息");
                                InfoTip.showMessage("成功编辑文档基本信息", "success", 6000);
                            } else {
                                showMessage("编辑文档基本信息失败");
                                InfoTip.showMessage("编辑文档基本信息失败", "error", 6000);
                            }
                        }
                    });
                }
            });
            //上移属性
            $("body").on("click", ".attr_up", function (e) {
                e.preventDefault();
                var $parent = $(this).parents(".attritem");
                var $prev = $parent.prev();
                if ($prev.hasClass("itemtemp")) {
                    showMessage("当前属性是第一个属性");
                    InfoTip.showMessage("当前属性是第一个属性", "warning", 6000);
                } else {
                    var downAttrid = $parent.find("input[type=hidden]").val();
                    $.ajax({
                        type: "post",
                        url: "/handlers/tempdocConfig/allowChangeOrder.ashx",
                        data: { docID: id, attrID: downAttrid, action: "up" },
                        dataType: "",
                        success: function (res) {
                            if (res == "1") {
                                var upAttrid = $prev.find("input[type=hidden]").val();
                                $.ajax({
                                    type: "POST",
                                    url: "/handlers/tempdocConfig/ChangeAttrOrder.ashx",
                                    data: { upAttrid: upAttrid, downAttrid: downAttrid },
                                    dataType: "",
                                    success: function (res) {
                                        if (res == "1") {
                                            $parent.clone().insertBefore($prev);
                                            $parent.remove();
                                            showMessage("上移该属性成功");
                                            InfoTip.showMessage("上移该属性成功", "success", 6000);
                                        } else {
                                            showMessage("上移该属性失败");
                                            InfoTip.showMessage("上移该属性失败", "error", 6000);
                                        }
                                    }
                                });
                            } else {
                                showMessage("不能上移该属性");
                                InfoTip.showMessage("不能上移该属性", "warning", 6000);
                            }
                        }
                    });
                }
            });
            //下移属性
            $("body").on("click", ".attr_down", function (e) {
                e.preventDefault();
                var $parent = $(this).parents(".attritem");
                var $next = $parent.next();
                if ($next.hasClass("more")) {
                    showMessage("当前属性是最后一个属性");
                    InfoTip.showMessage("当前属性是最后一个属性", "warning", 6000);
                } else {
                    var upAttrid = $parent.find("input[type=hidden]").val();
                    $.ajax({
                        type: "post",
                        url: "/handlers/tempdocConfig/allowChangeOrder.ashx",
                        data: { docID: id, attrID: upAttrid, action: "down" },
                        dataType: "",
                        success: function (res) {
                            if (res == "1") {
                                var downAttrid = $next.find("input[type=hidden]").val();
                                $.ajax({
                                    type: "POST",
                                    url: "/handlers/tempdocConfig/ChangeAttrOrder.ashx",
                                    data: { upAttrid: upAttrid, downAttrid: downAttrid },
                                    dataType: "",
                                    success: function (res) {
                                        if (res == "1") {
                                            $parent.clone().insertAfter($next);
                                            $parent.remove();
                                            showMessage("下移该属性成功");
                                            InfoTip.showMessage("下移该属性成功", "success", 6000);
                                        } else {
                                            showMessage("下移该属性失败");
                                            InfoTip.showMessage("下移该属性失败", "error", 6000);
                                        }
                                    }
                                });
                            } else {
                                showMessage("不能下移该属性");
                                InfoTip.showMessage("不能下移该属性", "warning", 6000);
                            }
                        }
                    });
                }
            });
            //跟随属性
            $("body").on("click", ".attr_follow", function (e) {
                e.preventDefault();
            });
            //删除属性
            $("body").on("click", ".attr_del", function (e) {
                e.preventDefault();
                var $div = $("#del_confirm");
                var $parent = $(this).parents(".attritem");
                var attrid = $parent.find("input[type=hidden]").val();
                var attrname = $parent.find(".attrname").text();
                $div.find("p").html("您确定删除属性'" + attrname + "'");
                $div.dialog({
                    title: "删除属性",
                    modal: true,
                    width: 600,
                    minHeight: 100,
                    buttons: {
                        "确认": function () {
                            $.ajax({
                                type: "POST",
                                url: "/handlers/tempdocConfig/deleteAttr.ashx",
                                data: { attrID: attrid, userid: userid, docID: id },
                                dataType: "",
                                success: function (res) {
                                    if (res == "1") {
                                        $div.dialog("close");
                                        $parent.slideUp("slow");
                                        showMessage("删除属性成功");
                                        lastOrder--;
                                        InfoTip.showMessage("删除属性成功", "success", 6000);
                                    } else {
                                        showMessage("删除属性失败");
                                        InfoTip.showMessage("删除属性失败", "error", 6000);
                                    }
                                }
                            });
                        },
                        "取消": function () { $div.dialog("close"); }
                    }
                });
            });
            //编辑属性
            //验证编辑中的属性是否存在
            var isAttrUpdateExist = false;
            var isAttrExist = false;
            $("#form .attrname").blur(function () {
                var attrName = $(this).val();
                var $parent = $(this).parents(".form");
                var attrid = $parent.find("input[type=hidden]").val();
                //alert(attrid);
                if (attrName != "") {
                    if (attrid != "") {
                        //更新时候的验证
                        $.ajax({
                            type: "POST",
                            url: "/handlers/tempdocConfig/IsAttrUpdateExist.ashx",
                            data: { docID: id, attrID: attrid, attrName: attrName },
                            dataType: "",
                            success: function (res) {
                                isAttrUpdateExist = (res == "1" ? true : false);
                            }
                        });
                    } else {
                        //新增时候的验证
                        $.ajax({
                            type: "POST",
                            url: "/handlers/tempdocConfig/IsAttrExist.ashx",
                            data: { docID: id, attrName: attrName },
                            dataType: "",
                            success: function (res) {
                                isAttrExist = (res == "1" ? true : false);
                            }
                        });
                    }
                } else {
                    isAttrUpdateExist = false;
                }
            });
            //枚举类型属性
            $("input[name=attrType]").click(function () {
                var attrType = $(this).val();
                var $parent = $(this).parents(".control_group");
                if (attrType == "EnumVal") {
                    $parent.find(".attr_content").show();
                } else {
                    $parent.find(".attr_content").hide();
                }
            });
            //删除属性值
            $("body").on("click", ".delattrval", function (e) {
                e.preventDefault();
                var $parent = $(this).parent();
                var id = $(this).next().val();
                $.ajax({
                    type: "POST",
                    url: "/handlers/tempdocConfig/deleteAttrVal.ashx",
                    data: { id: id },
                    dataType: "json",
                    success: function (res) {
                        if (res == "1") {
                            $parent.remove();
                            InfoTip.showMessage("删除属性值成功", "success", 6000);
                        } else {
                            InfoTip.showMessage("删除属性值失败", "error", 6000);
                        }
                    }
                });
            });
            //新增属性值
            $(".attrval_add").click(function (e) {
                e.preventDefault();
                var attrid = $("#form .attrid").val();
                var attrval = $(this).prev().val();
                var $pos = $(this).prev();
                if (attrval != "") {
                    $.ajax({
                        type: "POST",
                        url: "/handlers/tempdocConfig/addattrval.ashx",
                        data: {
                            attrid: attrid,
                            attrval: attrval,
                            userid: userid
                        },
                        dataType: "",
                        success: function (res) {
                            if (res != "-1") {
                                $("<span>").html(attrval + "<a  href='#' title='编辑该属性值' class='editattrval edit'></a>" + "<a  href='#' title='删除该属性值' class='delattrval'></a><input type='hidden' value='" + res + "' />").addClass("userspan").insertBefore($pos);
                                InfoTip.showMessage("新增属性值成功", "success", 6000);
                            } else {
                                InfoTip.showMessage("新增属性值失败", "error", 6000);
                            }
                        }
                    });
                } else {
                    InfoTip.showMessage("请输入属性值", "warning", 6000);
                }
            });
            //编辑属性值
            $("body").on("click", ".editattrval", function (e) {
                e.preventDefault();
                var $attrval = $(this).parent();
                var attrvalid = $attrval.find("input[type=hidden]").val();
                var attrval = $attrval.text();
                //console.log(attrvalid + "," + attrval);
                var $div = $("#attrval_div");
                $div.find("input[type=text]").val(attrval);
                $div.dialog({
                    title: "编辑属性值",
                    modal: true,
                    width: 550,
                    minHeight: 150,
                    buttons: {
                        "保存": function () {
                            var newattrval = $div.find("input[type=text]").val();
                            if (newattrval == "") {
                                InfoTip.showMessage("请输入属性值", "warning", 6000);
                            } else {
                                $.ajax({
                                    type: "POST",
                                    url: "/handlers/tempdocConfig/updateAttrVal.ashx",
                                    data: {
                                        id: attrvalid,
                                        attrVal: newattrval,
                                        userid: userid
                                    },
                                    dataType: "",
                                    success: function (res) {
                                        if (res == "1") {
                                            $attrval.text(newattrval);
                                            $div.dialog("close");
                                            InfoTip.showMessage("编辑属性值成功", "success", 6000);
                                        } else {
                                            InfoTip.showMessage("编辑属性值失败", "error", 6000);
                                        }
                                    },
                                    error: function (msg) { }
                                });
                            }
                        },
                        "取消": function () { $div.dialog("close"); }
                    }
                });
            });
            //编辑属性
            $("body").on("click", ".attr_edit", function (e) {
                e.preventDefault();
                var $parent = $(this).parents(".attritem");
                var attrid = $parent.find("input[type=hidden]").val();
                //alert(attrid);
                $.ajax({
                    type: "POST",
                    url: "/handlers/tempdocConfig/getAttrById.ashx",
                    data: { id: attrid },
                    dataType: "json",
                    success: function (res) {
                        var $div = $("#form");
                        //编辑赋值
                        $div.find(".attrname").val(res.AttrName);
                        $div.find("input[type=hidden]").val(attrid);
                        setType(res.AttrType.toString());
                        if (showType(res.AttrType) == "枚举") {
                            //枚举
                            //$div.find("input[name=attrType]").attr({"disabled":"disabled"});
                            $div.find(".attr_content").show();
                            $div.find(".userspan").remove();
                            $.each(res.AttrVals, function (i, item) {
                                $("<span>").html(item.AttrValue + "<a  href='#' title='编辑该属性值' class='editattrval edit'></a>" + "<a  href='#' title='删除该属性值' class='delattrval'></a><input type='hidden' value='" + item.ID + "' />").addClass("userspan").insertBefore($div.find(".attr_content .attrval"));
                            });
                        } else {
                            $div.find(".attr_content").hide();
                        }
                        $div.find("input[name=isSearch][value='" + res.IsSearch + "']").prop("checked", true);
                        $div.find("input[name=isRequired][value='" + res.IsRequired + "']").prop("checked", true);
                        $div.dialog({
                            title: "编辑属性",
                            modal: true,
                            width: 750,
                            minHeight: 200,
                            buttons: {
                                "保存": function () {
                                    //编辑时候保存
                                    var attrName = $div.find("input[type=text]").val();
                                    var isValid = true;
                                    var attrid = $div.find("input[type=hidden]").val();
                                    var attrtype = $div.find("input[name*='attrType']:checked").val();
                                    var attrTypeStr = $div.find("input[name*='attrType']:checked").next().html();
                                    var isrequired = $div.find("input[name*='isRequired']:checked").val();
                                    var issearch = $div.find("input[name*='isSearch']:checked").val();

                                    if (attrName == "" || typeof (attrtype) == "undefined" || typeof (isrequired) == "undefined" || typeof (issearch) == "undefined") {
                                        showMessage("属性名称,属性类型不能为空");
                                        InfoTip.showMessage("属性名称,属性类型不能为空", "warning", 6000);
                                        isValid = false;
                                        return false;
                                    }

                                    //更新属性
                                    if (isAttrUpdateExist) {
                                        showMessage("当前文档已经存在该属性");
                                        InfoTip.showMessage("当前文档已经存在该属性", "warning", 6000);
                                        isValid = false;
                                        return false;
                                    }
                                    if (isValid) {
                                        $.ajax({
                                            type: "POST",
                                            url: "/handlers/tempdocConfig/UpdateAttr.ashx",
                                            data: {
                                                docID: id,
                                                attrID: attrid,
                                                attrName: attrName,
                                                attrType: attrtype,
                                                desc: "",
                                                isRequired: isrequired,
                                                isSearch: issearch,
                                                userid: userid
                                            },
                                            dataType: "",
                                            success: function (res) {
                                                if (res == "1") {
                                                    $div.dialog("close");
                                                    showMessage("更新属性成功");
                                                    InfoTip.showMessage("更新属性成功", "success", 6000);
                                                    //var $item = $(".listattr input[type=hidden][value=" + attrid + "]").parent();
                                                    $parent.find(".attrname").html(attrName);
                                                    if (attrTypeStr != "枚举") {
                                                        $parent.find(".attrtype").html(attrTypeStr);
                                                    } else {
                                                        attrTypeStr += "{枚举值为:";
                                                        $div.find(".userspan").each(function (k, kitem) {
                                                            attrTypeStr += $(kitem).text() + ",";
                                                        });
                                                        attrTypeStr = attrTypeStr.substr(0, attrTypeStr.length - 1);
                                                        $parent.find(".attrtype").html(attrTypeStr + "}");
                                                    }
                                                    isrequired = (isrequired.toString() == "true" ? true : false);
                                                    issearch = (issearch.toString() == "true" ? true : false);
                                                    //console.log(isrequired + "," + showBool(isrequired) + "," + issearch + "," + showBool(issearch));
                                                    $parent.find(".isrequired").text(showBool(isrequired));
                                                    $parent.find(".issearch").text(showBool(issearch));

                                                    emptyContainer($div);
                                                } else {
                                                    showMessage("更新属性失败");
                                                    InfoTip.showMessage("更新属性失败", "error", 6000);
                                                    return false;
                                                }
                                            }
                                        });
                                    }
                                },
                                "取消": function () {
                                    $div.dialog('close');
                                    emptyContainer($div);
                                }
                            }
                        });
                    }
                });
            });
            //新增属性
            $(".more").click(function (e) {
                e.preventDefault();
                var $div = $("#form");
                $div.find("input[type=text]").val('');
                $div.find("input[type=hidden]").val('');
                emptyContainer($div);
                $div.dialog({
                    title: "新增属性",
                    modal: true,
                    width: 750,
                    minHeight: 200,
                    buttons: {
                        "保存": function () {
                            var isValid = true;
                            var attrName = $div.find("input[type=text]").val();
                            var attrtype = $div.find("input[name*='attrType']:checked").val();
                            var attrTypeStr = $div.find("input[name*='attrType']:checked").next().html();
                            var isrequired = $div.find("input[name*='isRequired']:checked").val();
                            var issearch = $div.find("input[name*='isSearch']:checked").val();
                            if (attrName == "" || typeof (attrtype) == "undefined" || typeof (isrequired) == "undefined" || typeof (issearch) == "undefined") {
                                showMessage("属性名称，属性类型不能为空");
                                InfoTip.showMessage("属性名称，属性类型不能为空", "warning", 6000);
                                isValid = false;
                                return false;
                            }
                            //新增属性
                            if (isAttrExist) {
                                showMessage("当前文档已经存在该属性");
                                InfoTip.showMessage("当前文档已经存在该属性", "warning", 6000);
                                isValid = false;
                                return false;
                            }
                            if (isValid) {
                                $.ajax({
                                    type: "POST",
                                    url: "/handlers/tempdocConfig/AddDocAttr.ashx",
                                    data: {
                                        docID: id,
                                        attrName: attrName,
                                        attrType: attrtype,
                                        desc: "",
                                        isRequired: isrequired,
                                        isSearch: issearch,
                                        userid: userid,
                                        vOrder: (lastOrder + 1)
                                    },
                                    dataType: "",
                                    success: function (res) {
                                        if (res != "-1" || res != "0") {
                                            $div.dialog("close");
                                            emptyContainer($div);
                                            showMessage("新增属性成功");
                                            InfoTip.showMessage("新增属性成功", "success", 6000);
                                            lastOrder++;
                                            var $item = $(".listattr .itemtemp").clone();
                                            $item.find(".attrname").html(attrName);
                                            $item.find(".attrtype").html(attrTypeStr);
                                            $item.find("input[type=hidden]").val(res);
                                            //两个radio属性未设置
                                            isrequired = (isrequired.toString() == "true" ? true : false);
                                            issearch = (issearch.toString() == "true" ? true : false);
                                            //console.log(isrequired + "," + showBool(isrequired) + "," + issearch + "," + showBool(issearch));
                                            $item.find(".isrequired").text(showBool(isrequired));
                                            $item.find(".issearch").text(showBool(issearch));
                                            $item.removeClass("hidden itemtemp").appendTo($(".listattr"));
                                        } else {
                                            showMessage("新增属性失败");
                                            InfoTip.showMessage("新增属性失败", "error", 6000);
                                            return false;
                                        }
                                    }
                                });
                            }
                        },
                        "取消": function () { $div.dialog('close'); emptyContainer($div); }
                    }
                });
            });
        });
    </script>
</asp:Content>
