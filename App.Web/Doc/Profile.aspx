<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="Profile.aspx.cs" Inherits="App.Web.Doc.Profile" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="header relative">
        <h1 class="pagetitle">
            详情</h1>
    </div>
    <p class="message hidden">
        <span class="ui-icon ui-icon-info" style="float: left; margin-right: .3em;"></span>
        <strong>Hey!</strong></p>
    <div id="formContent">
        <fieldset class="form">
            <legend>基本信息</legend>
            <div class="control_temp hidden relative attr_control">
                <input type="hidden" class="attrid" />
                <input type="hidden" class="attrtype" />
                <input type="hidden" class="isrequired" />
                <input type="hidden" class="attrval" />
                <label class="control_label">
                    attrName</label>
                <div class="attr_action abs">
                    <span class="ui-icon ui-icon-pencil attr_edit" title="编辑"></span><span class="ui-icon ui-icon-check attr_save"
                        style="display: none;" title="保存"></span><span class="ui-icon ui-icon-cancel attr_cancel"
                            title="取消" style="display: none;"></span>
                </div>
                <div class="controls">
                    <p class="help_block hidden">
                        desc</p>
                </div>
            </div>
        </fieldset>
        <div class="form_actions">
            <a href="javascript:void(0);" class="btn btn_primary btn_large inst_del btn_del">删除</a>
        </div>
    </div>
    <span class="userspan hidden">username<a href="#" title="删除该人员"></a><input type="hidden" /></span>
    <div id="multiperson_div" class="hidden">
        <input type="hidden" class="sel_userids" />
        <input type="hidden" class="attrid" />
        <input type="hidden" class="sel_type" value="m" />
        <div class="depts_users">
            <div class="pull_left three_col_left sel_title">
                部门结构</div>
            <div class="pull_left its_users three_col_right sel_title">
                部门下属人员列表</div>
            <div id="jstree_dept_m" class="pull_left three_col_left container">
            </div>
            <div class="pull_left its_users three_col_right container">
                <div class="sel_users" id="user_container">
                    <ul>
                        <li class="user_temp hidden"><span class="pull_left" title="选择该人员">人员名称</span><input
                            type="hidden" /></li>
                    </ul>
                </div>
            </div>
            <div class="clear">
            </div>
        </div>
        <div id="selected_users">
            <fieldset class="form">
                <legend>已选择的人员列表</legend>
                <div class="sel_users">
                    <ul>
                        <li class="user_temp hidden"><span class="pull_left username">人员名称</span><span title="删除该人员"
                            class="ui-icon ui-icon-close user_del"></span><input type="hidden" /></li>
                    </ul>
                </div>
            </fieldset>
        </div>
        <span class="user_dis_temp hidden">username<a href="#" title="删除该人员"></a><input type="hidden" /></span>
    </div>
    <div id="file_div" class="hidden">
        <input type="hidden" class="attrid" />
        <a href="#" id="file_upload">选择文件上传</a>
    </div>
    <div class="result hidden">
        <p>
            恭喜您，您已经成功删除了一个文档。您可以查看文档的列表<a href="#" class="docLink">XXX</a></p>
        <p>
            您还可以继续新增文档,<a href="/Doc/Add.aspx" class="addlink">新增文档</a></p>
    </div>
    <div id="del_confirm" class="hidden">
        <p>
            您确定删除该文档吗？</p>
    </div>
    <script type="text/javascript">
        var uid = "";
        $(document).ready(function () {
            //get exp key words
            var autoKeyWords = "";
            $.ajax({
                type: "POST",
                url: "/handlers/utils/getappvalue.ashx",
                data: { key: "AutoKeyWords" },
                success: function (res) { autoKeyWords = res; }
            });


            var id = request("id");
            id = id.replace('#', '');
            uid = $("#userid").val();
            var doc_id = "";
            var docName = "";
            //console.log(id);
            $.ajax({
                type: "POST",
                url: "/handlers/doc/getDocById.ashx",
                data: { docinstanceid: id },
                dataType: "json",
                success: function (res) {
                    $(".pagetitle").html(res.Document.DocName + "详情");
                    docName = res.Document.DocName;
                    var cid = res.Creator.ID;
                    $("<a>").addClass("btn abs").prop({ "title": "查看" + res.Document.DocName + "列表", "href": "/Doc/" + (uid == cid ? "My" : "") + "List.aspx?ID=" + res.Document.ID }).text("查看列表").appendTo($(".header"));
                    doc_id = res.Document.ID;
                    $("#formContent .form legend").text(res.Document.DocName + "基本信息");
                    $.each(res.Document.Attrs, function (i, item) {
                        var $temp = $("#formContent .control_temp").clone();
                        $temp.find(".control_label").text(item.AttrName);
                        if (uid != cid) {
                            $temp.find(".attr_action").remove();
                            $(".form_actions").remove();
                            $("#t65").addClass("selected");
                        } else {
                            $("#t64").addClass("selected");
                        }
                        $temp.find(".attrid").val(item.ID);
                        var attrType = showType(item.AttrType);
                        $temp.find(".attrtype").val(attrType);
                        $temp.find(".isrequired").val(item.IsRequired);

                        switch (attrType) {
                            case "单字符串":
                                $("<label>").text(showString(item.TranValue)).insertBefore($temp.find("p"));
                                $temp.find(".controls").addClass("ctrl_padding");
                                $("<input>").attr({ "type": "text", "value": item.Value }).addClass("input_medium hidden" + (item.AttrName.indexOf(autoKeyWords) >= 0 ? " auto_exp" : "")).insertBefore($temp.find("p"));
                                if (item.IsRequired) {
                                    $("<span>").html("必填").addClass("red hidden").insertBefore($temp.find("p"));
                                }
                                break;
                            case "多字符串":
                                $("<label>").text(showString(item.TranValue)).insertBefore($temp.find("p"));
                                $temp.find(".controls").addClass("ctrl_padding");
                                $("<textarea>").addClass("input_large multitext hidden").val(item.Value).insertBefore($temp.find("p"));
                                if (item.IsRequired) {
                                    $("<span>").html("必填").addClass("red hidden").insertBefore($temp.find("p"));
                                }
                                break;
                            case "日期":
                                $("<label>").text(showString(item.TranValue)).insertBefore($temp.find("p"));
                                $temp.find(".controls").addClass("ctrl_padding");

                                var $dp = $("<input>").attr({ "type": "text" }).val(item.Value == "" ? "" : item.TranValue).addClass("datepicker hidden");
                                $dp.datepicker({ "dateFormat": "yy-mm-dd" });
                                $dp.insertBefore($temp.find("p"));
                                if (item.IsRequired) {
                                    $("<span>").html("必填").addClass("red hidden").insertBefore($temp.find("p"));
                                }
                                break;
                            case "单人":
                                //$("<select>").addClass("input_medium").insertBefore($temp.find("p"));
                                if (uid == cid) {
                                    $("<a>").addClass("btn singleperson").html("选择").attr({ "title": "选择人员" }).insertBefore($temp.find("p"));
                                    $temp.find(".attr_action").remove();
                                    if (item.Value != "") {
                                        $("<span>").addClass("userspan").html(item.TranValue + "<a href='#' title='删除该人员'></a><input type='hidden' value='" + item.Value + "' />").insertBefore($temp.find("p"));
                                    }
                                    if (item.IsRequired) {
                                        $("<span>").html("必填").addClass("red").insertBefore($temp.find("p"));
                                    }
                                } else {
                                    $("<label>").text(showString(item.TranValue)).insertBefore($temp.find("p"));
                                    $temp.find(".controls").addClass("ctrl_padding");
                                }
                                break;
                            case "多人参与":
                                if (uid == cid) {
                                    $("<a>").addClass("btn multiperson").html("选择").attr({ "title": "选择多个人员" }).insertBefore($temp.find("p"));
                                    $temp.find(".attr_action").remove();
                                    if (item.Value != "") {
                                        var idArray = item.Value.split(',');
                                        var nameArray = item.TranValue.split(',');
                                        for (var index = 0; index < idArray.length; index++) {
                                            //console.log(idArray[index] + "," + nameArray[index]);
                                            $("<span>").addClass("userspan").html(nameArray[index] + "<a href='#' title='删除该人员'></a><input type='hidden' value='" + idArray[index] + "' />").insertBefore($temp.find("p"));
                                        }
                                    }
                                    if (item.IsRequired) {
                                        $("<span>").html("必填").addClass("red").insertBefore($temp.find("p"));
                                    }
                                } else {
                                    var str = "";
                                    if (item.Value != "") {
                                        var idArray = item.Value.split(',');
                                        var nameArray = item.TranValue.split(',');

                                        for (var index = 0; index < idArray.length; index++) {
                                            str += nameArray[index] + ",";
                                        }
                                    }
                                    $("<label>").text(showString(str)).insertBefore($temp.find("p"));
                                    $temp.find(".controls").addClass("ctrl_padding");
                                }
                                break;
                            case "附件":
                                $temp.find(".controls").addClass("ctrl_padding");
                                if (uid == cid) {
                                    $("<a>").addClass("btn fileattach").html("上传文件").attr({ "title": "选择文件上传" }).insertBefore($temp.find("p"));
                                    $temp.find(".attr_action").remove();

                                    if (item.IsRequired) {
                                        $("<span>").html("必填").addClass("red").insertBefore($temp.find("p"));
                                    }
                                }
                                if (item.Value != "") {
                                    $("<a>").addClass("fileinfo").text(item.TranValue).attr({ "href": "/Avatar/" + item.Value, "target": "_blank", "title": "点击查看文档详情" }).insertBefore($temp.find("p"));
                                } else {
                                    $("<span>").html("暂无").insertBefore($temp.find("p"));
                                }
                                break;
                            case "枚举":
                                $("<label>").text(item.TranValue).insertBefore($temp.find("p"));
                                $temp.find(".controls").addClass("ctrl_padding");
                                var $sel = $("<select>").addClass("input_medium hidden");
                                $.each(item.AttrVals, function (k, kitem) {
                                    $("<option>").text(kitem.AttrValue).val(kitem.ID).appendTo($sel);
                                });
                                if (item.Value != "")
                                    $sel.val(item.Value);
                                $sel.insertBefore($temp.find("p"));
                                if (item.IsRequired) {
                                    $("<span>").html("必填").addClass("red hidden").insertBefore($temp.find("p"));
                                }
                                break;
                        }
                        //仅仅对单字符串加关联
                        if (attrType == "单字符串" && item.RelateAttrID != 0) {
                            $("<a>").addClass("rel_link").prop({ "href": "/Doc/Connect.aspx?RInstID=" + id + "&RAttrID=" + item.ID, "title": "查看关联文档", "target": "_blank" }).text("查看").insertBefore($temp.find("p"));
                        }
                        $temp.removeClass("control_temp hidden").addClass("control_group").attr({ "id": "item" + item.ID }).appendTo($("#formContent .form"));
                    });
                }
            });
            //load departments
            $.ajax({
                type: "POST",
                url: "/handlers/dept/getDepts.ashx",
                data: {},
                dataType: "json",
                success: function (res) {
                    $("#jstree_dept_m").jstree({
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
                        var selDeptid = r[0];
                        selDeptid = selDeptid.replace('d', '');
                        //alert(selDeptid);
                        //load its users of a selected department
                        $.ajax({
                            type: "POST",
                            url: "/handlers/user/getUsers.ashx",
                            data: { deptID: selDeptid },
                            dataType: "json",
                            success: function (res_users) {
                                $("#user_container .useritem").remove();
                                $.each(res_users, function (i, item) {
                                    var $temp = $("#user_container .user_temp").clone();
                                    $temp.find("span").html(item.Name);
                                    $temp.find("input[type=hidden]").val(item.ID);
                                    $temp.removeClass("hidden user_temp").addClass("useritem").appendTo($("#user_container ul"));
                                });
                            }
                        });
                    });
                }
            });
            //select user
            $("body").on("click", "#user_container .useritem", function (e) {
                e.preventDefault();
                var userid = $(this).find("input[type=hidden]").val();
                //console.log(userid);
                var $div = $("#multiperson_div");
                var sel_type = $div.find(".sel_type").val();
                var isExist = ($("#selected_users input[type=hidden][value='" + userid + "']").length > 0 ? true : false);
                //console.log(isExist + sel_type);
                if (!isExist) {
                    var $temp = $("#selected_users .user_temp").clone();
                    $temp.find(".username").html($(this).find("span").text());
                    $temp.find("input[type=hidden]").val(userid);
                    if (sel_type == "m") {
                        $temp.removeClass("hidden user_temp").addClass("useritem").appendTo($("#selected_users ul"));
                    } else {
                        $("#selected_users .useritem").remove();
                        $temp.removeClass("hidden user_temp").addClass("useritem").appendTo($("#selected_users ul"));
                    }
                }
            });
            //fill users
            $("body").on("click", ".singleperson", function (e) {
                e.preventDefault();
                var $div = $("#multiperson_div");
                var $parent = $(this).parents(".control_group");
                var attrid = $parent.find(".attrid").val();
                var isrequired = $parent.find(".isrequired").val();
                $div.find(".sel_type").val("s");
                $div.find(".attrid").val(attrid);
                $("#selected_users .useritem").remove();
                //如果已经选了人,就需要赋值给弹出div
                if ($parent.find(".userspan").length > 0) {
                    var $selitem = $parent.find(".userspan").eq(0);
                    var username = $selitem.text();
                    var userid = $selitem.find("input[type=hidden]").val();
                    //console.log(username + userid);
                    var $temp = $("#selected_users .user_temp").clone();
                    $temp.find(".username").text(username);
                    $temp.find("input[type=hidden]").val(userid);
                    $temp.removeClass("hidden user_temp").addClass("useritem").appendTo($("#selected_users ul"));
                }
                $div.dialog({
                    title: "选择人员（单选）",
                    modal: true,
                    width: 800,
                    minHeight: 300,
                    buttons: {
                        "确认选择": function () {
                            var isvalid = true;
                            if (isrequired == "true") {
                                if ($("#selected_users .useritem").length == 0) {
                                    isvalid = false;
                                }
                            }
                            if (isvalid) {
                                $parent.find(".userspan").remove();
                                if ($("#selected_users .useritem").length > 0) {
                                    var $selitem = $("#selected_users .useritem").eq(0);
                                    var username = $selitem.find(".username").text();
                                    var userid = $selitem.find("input[type=hidden]").val();
                                    var $temp = $("<span>").html(username + "<a href='#'></a><input type='hidden' value='" + userid + "' />");
                                    $temp.addClass("userspan").insertAfter($parent.find(".singleperson"));
                                    $div.dialog("close");
                                    updateAttr(id, attrid, "单人", userid, "");
                                } else {
                                    InfoTip.showMessage("请选择单个人员", "warning", 6000);
                                }
                            }
                        },
                        "取消": function () { $div.dialog("close"); }
                    }
                });
            });
            //delete select in pop up
            $("body").on("click", "#selected_users .user_del", function (e) {
                e.preventDefault();
                $(this).parent().remove();
            });
            $("body").on("click", ".multiperson", function (e) {
                e.preventDefault();
                var $parent = $(this).parents(".control_group");
                var attrid = $parent.find(".attrid").val();
                var isrequired = $parent.find(".isrequired").val();
                var $div = $("#multiperson_div");
                $div.find(".sel_type").val("m");
                $div.find(".attrid").val(attrid);
                $("#selected_users .useritem").remove();
                //如果已经选了人,就需要赋值给弹出div
                if ($parent.find(".userspan").length > 0) {
                    $parent.find(".userspan").each(function (i, item) {
                        var username = $(item).text();
                        var userid = $(item).find("input[type=hidden]").val();
                        //console.log(username + userid);
                        var $temp = $("#selected_users .user_temp").clone();
                        $temp.find(".username").text(username);
                        $temp.find("input[type=hidden]").val(userid);
                        $temp.removeClass("hidden user_temp").addClass("useritem").appendTo($("#selected_users ul"));
                    });
                }
                $div.dialog({
                    title: "选择人员（多选）",
                    modal: true,
                    width: 800,
                    minHeight: 300,
                    buttons: {
                        "确认选择": function () {
                            var isvalid = true;
                            if (isrequired == "true") {
                                if ($("#selected_users .useritem").length == 0) {
                                    isvalid = false;
                                }
                            }
                            if (isvalid) {
                                $parent.find(".userspan").remove();
                                if ($("#selected_users .useritem").length > 0) {
                                    var attrVals = "";
                                    $("#selected_users .useritem").each(function (i, item) {
                                        var username = $(item).find(".username").text();
                                        var userid = $(item).find("input[type=hidden]").val();
                                        //console.log(username + userid);
                                        var $temp = $("<span>").html(username + "<a href='#'></a><input type='hidden' value='" + userid + "' />");
                                        $temp.addClass("userspan").insertAfter($parent.find(".multiperson"));
                                        attrVals += userid + ",";
                                    });
                                    $div.dialog("close");
                                    attrVals = attrVals.substr(0, attrVals.length - 1);
                                    updateAttr(id, attrid, "多人参与", attrVals, "");
                                } else {
                                    InfoTip.showMessage("请选择人员", "warning", 6000);
                                }
                            }
                        },
                        "取消": function () { $div.dialog("close"); }
                    }
                });
            });
            //delete selected users in add form
            $("body").on("click", ".userspan a", function (e) {
                e.preventDefault();
                var $parent = $(this).parents(".control_group");
                var attrid = $parent.find(".attrid").val();
                var isrequired = $parent.find(".isrequired").val();
                var attrtype = $parent.find(".attrtype").val();
                //console.log(isrequired + "," + $parent.find(".userspan").length);
                if (isrequired == "true" && $parent.find(".userspan").length == 1) {
                    //必填，目前仅有一个  不能删除
                    InfoTip.showMessage("必填项,至少需要选择一人", "warning", 6000);
                } else {
                    $(this).parent().remove();
                    var attrValue = "";
                    if ($parent.find(".userspan").length > 0) {
                        $parent.find(".userspan").each(function (i, item) {
                            attrValue += $(item).find("input[type=hidden]").val() + ",";
                        });
                        attrValue = attrValue.substr(0, attrValue.length - 1);
                    }
                    updateAttr(id, attrid, attrtype, attrValue, "");
                }
            });
            //file
            $("body").on("click", ".fileattach", function (e) {
                e.preventDefault();
                var $parent = $(this).parents(".control_group");
                var attrid = $parent.find(".attrid").val();
                var $div = $("#file_div");
                $div.find(".attrid").val(attrid);
                $div.dialog({
                    title: "选择文件上传",
                    modal: true,
                    width: 350,
                    minHeight: 50,
                    buttons: {
                        "取消": function () { $div.dialog("close"); }
                    }
                });
            });
            //file upload
            $("#file_upload").uploadify({
                'swf': '/resources/uploadify-v3.1/uploadify.swf',
                'uploader': '/handlers/file/fileUpload.ashx',
                'folder': "Avatar",
                'width': 300,
                'multi': false,
                'fileSizeLimit': '10MB',
                'fileTypeDesc': 'Image Files',
                'onUploadSuccess': function (file, data, response) {
                    //$("#avatar_edit .filepath").val(data);                    
                    var attrid = $("#file_div .attrid").val();
                    var $parent = $("#item" + attrid);
                    var symIndex = data.lastIndexOf('_');
                    var originalfilename = data.substring(0, symIndex);
                    var filename = data.substr(symIndex + 1);
                    //console.log(data + "," + originalfilename + "," + filename);
                    updateAttr(id, attrid, "附件", filename, "");
                    $parent.find(".attrval").val(filename);
                    $parent.find(".fileinfo").remove();
                    $("<a>").attr({ "href": "/Avatar/" + filename, "target": "_blank", "title": "点击查看文档详情" }).addClass("fileinfo").html(originalfilename).insertAfter($parent.find(".fileattach"));
                    $("#file_div").dialog("close");
                    InfoTip.showMessage("文件上传成功", "success", 6000);
                    //alert(data);
                }
                // Put your options here
            });
            //auto complete experiments
            $("body").on("click", ".auto_exp", function (e) {
                $(this).autocomplete({
                    minLength: 2,
                    source: function (req, res) {
                        $.ajax({
                            type: "POST",
                            url: "/handlers/doc/getexperiments.ashx",
                            data: {
                                nameStart: req.term,
                                maxRows: 10
                            },
                            dataType: "json",
                            success: function (data) {
                                res($.map(data, function (item) {
                                    return {
                                        value: item.Code + "(" + item.Name + ")",
                                        label: item.Code + "(" + item.Name + ")"
                                    }
                                }));
                            }
                        });
                    },
                    select: function (e, ui) {
                        //ui.item ? to judge whether a item is selected..console.log(ui.item.label+","+ui.item.value);
                    }
                });
            });
            //edit attr
            $("body").on("click", ".attr_edit", function (e) {
                e.preventDefault();
                var $parent = $(this).parents(".control_group");
                $parent.find(".controls label").hide();
                $parent.find(".controls").removeClass("ctrl_padding");
                $parent.find("input[type=text],textarea,.red,select").show();
                $(this).hide();
                $parent.find(".attr_save,.attr_cancel").show();
            });
            //cancel edit attr
            $("body").on("click", ".attr_cancel", function (e) {
                e.preventDefault();
                var $parent = $(this).parents(".control_group");
                $parent.find(".controls label").show();
                $parent.find(".controls").addClass("ctrl_padding");
                $parent.find("input[type=text],textarea,.red,select").hide();
                $(this).hide();
                $parent.find(".attr_save").hide();
                $parent.find(".attr_edit").show();
            });
            //save the attr
            $("body").on("click", ".attr_save", function (e) {
                e.preventDefault();
                var isValid = true;
                var $parent = $(this).parents(".control_group");
                var isrequired = $parent.find(".isrequired").val();
                var attrtype = $parent.find(".attrtype").val();
                if (isrequired == "true") {
                    switch (attrtype) {
                        case "单字符串":
                            if ($parent.find("input[type=text]").val() == "") {
                                isValid = false;
                            }
                            break;
                        case "多字符串":
                            if ($parent.find("textarea").val() == "") {
                                isValid = false;
                            }
                            break;
                        case "日期":
                            if ($parent.find(".datepicker").val() == "") {
                                isValid = false;
                            }
                            break;
                    }
                }
                //valid
                if (isValid) {
                    var attrid = $parent.find(".attrid").val();
                    var attrValue = "";
                    var valStr = "";
                    switch (attrtype) {
                        case "单字符串":
                            attrValue = $parent.find("input[type=text]").val();
                            if ($parent.find("input[type=text]").hasClass("auto_exp")) {
                                attrValue = attrValue.substr(0, attrValue.indexOf('('));
                            }
                            break;
                        case "多字符串":
                            attrValue = $parent.find("textarea").val();
                            break;
                        case "日期":
                            attrValue = $parent.find(".datepicker").val();
                            break;
                        case "枚举":
                            attrValue = $parent.find("select").val();
                            valStr = $parent.find("select option:selected").text();
                            break;
                    }
                    //console.log(id + attrid + attrValue);
                    updateAttr(id, attrid, attrtype, attrValue, valStr);
                } else {
                    InfoTip.showMessage("必填项" + $parent.find(".control_label").text() + "不能为空", "warning", 6000);
                }
            });
            //other methods
            $(".inst_del").click(function (e) {
                e.preventDefault();
                var $div = $("#del_confirm");
                $div.find("p").html("您确定删除文档" + docName + "吗?");
                $div.dialog({
                    title: "删除文档",
                    modal: true,
                    width: 300,
                    minHeight: 100,
                    buttons: {
                        "确认": function () {
                            $.ajax({
                                type: "POST",
                                url: "/handlers/doc/deletedoc.ashx",
                                data: { docinstanceid: id, userid: uid },
                                dataType: "",
                                success: function (res) {
                                    if (res == "1") {
                                        //window.location.href = "/doc/list.aspx?ID=" + doc_id;
                                        $div.dialog("close");
                                        InfoTip.showMessage("删除文档成功", "success", 6000);
                                        $("#formContent").slideUp("fast", function () {
                                            $(".result .docLink").attr({ "href": "/Doc/List.aspx?ID=" + doc_id }).html(docName + "列表");
                                            $(".result .addlink").attr({ "href": "/Doc/Add.aspx?ID=" + doc_id }).text("新增" + docName);
                                            $(".result").removeClass("hidden");
                                        });
                                    } else {
                                        InfoTip.showMessage("删除" + docName + "失败", "error", 6000);
                                    }
                                }
                            });
                        },
                        "取消": function () { $div.dialog("close"); }
                    }
                });
            });
        });
        function updateAttr(docinstanceid, attrid, attrtype, value, valStr) {
            $.ajax({
                type: "POST",
                url: "/handlers/doc/updateAttr.ashx",
                data: { docinstanceid: docinstanceid, attrid: attrid, attrVal: value, userid: uid },
                dataType: "",
                success: function (res) {
                    if (res == "1") {
                        InfoTip.showMessage("编辑成功", "success", 6000);
                        if (attrtype == "单字符串" || attrtype == "多字符串" || attrtype == "日期" || attrtype == "枚举") {
                            var $parent = $("#item" + attrid);
                            if (attrtype != "枚举") {
                                $parent.find(".controls label").text(value).show();
                            } else {
                                $parent.find(".controls label").text(valStr).show();
                            }
                            $parent.find(".controls").addClass("ctrl_padding");
                            $parent.find("input[type=text],textarea,.red,select").hide();
                            $parent.find(".attr_save,.attr_cancel").hide();
                            $parent.find(".attr_edit").show();
                        }
                    } else {
                        InfoTip.showMessage(" 编辑失败", "error", 6000);
                    }
                }
            });
        };
    </script>
</asp:Content>
