<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="Add.aspx.cs" Inherits="App.Web.Doc.Add" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="header">
        <h1 class="pagetitle">
            新增xxxx</h1>
    </div>
    <p class="message hidden">
        <span class="ui-icon ui-icon-info" style="float: left; margin-right: .3em;"></span>
        <strong>Hey!</strong></p>
    <div id="formContent">
        <fieldset class="form">
            <legend>文档基本信息</legend>
            <div class="control_temp hidden">
                <input type="hidden" class="attrid" />
                <input type="hidden" class="attrtype" />
                <input type="hidden" class="isrequired" />
                <input type="hidden" class="attrval" />
                <label class="control_label">
                    attrName</label>
                <div class="controls">
                    <p class="help_block hidden">
                        desc</p>
                </div>
            </div>
        </fieldset>
        <div class="form_actions">
            <a href="javascript:void(0);" class="btn btn_primary btn_large save">保存</a>
        </div>
    </div>
    <span class="userspan hidden">username<a href="#" title="删除该人员"></a><input type="hidden" /></span>
    <div class="result hidden">
        <p>
            恭喜您，新增文档成功。您可以查看其详情<a href="#" class="docLink">XXX</a></p>
        <p>
            您还可以继续新增<a href="/doc/Add.aspx" class="addlink">新增</a></p>
    </div>
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
    <script type="text/javascript">
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
            var uid = $("#userid").val();
            var docName = "";
            //console.log(id);
            $.ajax({
                type: "POST",
                url: "/handlers/docConfig/getItemById.ashx",
                data: { id: id },
                dataType: "json",
                success: function (res) {
                    $(".pagetitle").html("新增" + res.DocName);
                    docName = res.DocName;
                    $("#formContent .form legend").text(res.DocName + "基本信息");
                    $.each(res.Attrs, function (i, item) {
                        var $temp = $("#formContent .control_temp").clone();
                        $temp.find(".control_label").text(item.AttrName);
                        $temp.find(".attrid").val(item.ID);
                        var attrType = showType(item.AttrType);
                        $temp.find(".attrtype").val(attrType);
                        $temp.find(".isrequired").val(item.IsRequired);
                        switch (attrType) {
                            case "单字符串":
                                $("<input>").attr({ "type": "text" }).addClass("input_medium" + (item.AttrName.indexOf(autoKeyWords) >= 0 ? " auto_exp" : "")).insertBefore($temp.find("p"));
                                break;
                            case "多字符串":
                                $("<textarea>").addClass("input_large multitext").insertBefore($temp.find("p"));
                                break;
                            case "日期":
                                var $dp = $("<input>").attr({ "type": "text" }).addClass("datepicker");
                                $dp.datepicker({ "dateFormat": "yy-mm-dd" });
                                $dp.insertBefore($temp.find("p"));
                                break;
                            case "单人":
                                //$("<select>").addClass("input_medium").insertBefore($temp.find("p"));
                                $("<a>").addClass("btn singleperson").html("选择").attr({ "title": "选择人员" }).insertBefore($temp.find("p"));
                                break;
                            case "多人参与":
                                $("<a>").addClass("btn multiperson").html("选择").attr({ "title": "选择多个人员" }).insertBefore($temp.find("p"));
                                break;
                            case "附件":
                                $("<a>").addClass("btn fileattach").html("上传文件").attr({ "title": "选择文件上传" }).insertBefore($temp.find("p"));
                                break;
                            case "枚举":
                                var $sel = $("<select>").addClass("input_medium");
                                $.each(item.AttrVals, function (j, jitem) {
                                    $("<option>").text(jitem.AttrValue).val(jitem.ID).appendTo($sel);
                                });
                                $sel.insertBefore($temp.find("p"));
                                break;
                        }
                        if (item.IsRequired) {
                            $("<span>").html("必填").addClass("red").insertBefore($temp.find("p"));
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
                    //load multi person departments

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
                                    $("#selected_users .useritem").each(function (i, item) {
                                        var username = $(item).find(".username").text();
                                        var userid = $(item).find("input[type=hidden]").val();
                                        //console.log(username + userid);
                                        var $temp = $("<span>").html(username + "<a href='#'></a><input type='hidden' value='" + userid + "' />");
                                        $temp.addClass("userspan").insertAfter($parent.find(".multiperson"));
                                    });
                                    $div.dialog("close");
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
                $(this).parent().remove();
            });
            //date picker
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
                        "保存": function () { },
                        "取消": function () { }
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
                    //console.log(data + "," + symIndex + "," + originalfilename + "," + filename);
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

            //save
            $(".save").click(function (e) {
                e.preventDefault();
                var isValid = true;
                var validAttr = "";
                $("#formContent .control_group").each(function (i, item) {
                    var isrequired = $(item).find(".isrequired").val();
                    var attrtype = $(item).find(".attrtype").val();
                    validAttr = $(item).find(".control_label").text();
                    if (isrequired.toString() == "true") {
                        switch (attrtype) {
                            case "单字符串":
                                if ($(item).find("input[type=text]").val() == "") {
                                    isValid = false;
                                    return false;
                                }
                                break;
                            case "多字符串":
                                if ($(item).find("textarea").val() == "") {
                                    isValid = false;
                                    return false;
                                }
                                break;
                            case "日期":
                                if ($(item).find(".datepicker").val() == "") {
                                    isValid = false;
                                    return false;
                                }
                                break;
                            case "单人":
                            case "多人参与":
                                if ($(item).find(".userspan").length == 0) {
                                    isValid = false;
                                    return false;
                                }
                                break;
                            case "附件":
                                if ($(item).find(".fileinfo").length == 0) {
                                    isValid = false;
                                    return false;
                                }
                                break;
                        }
                    }
                });
                //validate
                if (isValid) {
                    var userid = $("#userid").val();
                    $.ajax({
                        type: "POST",
                        url: "/handlers/doc/addDoc.ashx",
                        data: { docid: id, userid: userid },
                        datType: "",
                        success: function (res) {
                            $("#formContent .control_group").each(function (i, item) {
                                var attrid = $(item).find(".attrid").val();
                                var attrtype = $(item).find(".attrtype").val();
                                var attrValue = "";
                                switch (attrtype) {
                                    case "单字符串":
                                        attrValue = $(item).find("input[type=text]").val();
                                        if ($(item).find("input[type=text]").hasClass("auto_exp")) {
                                            attrValue = attrValue.substr(0, attrValue.indexOf('('));
                                        }
                                        break;
                                    case "多字符串":
                                        attrValue = $(item).find("textarea").val();
                                        break;
                                    case "日期":
                                        attrValue = $(item).find(".datepicker").val();
                                        break;
                                    case "单人":
                                        attrValue = $(item).find(".userspan").eq(0).find("input[type=hidden]").val();
                                        break;
                                    case "多人参与":
                                        $(item).find(".userspan").each(function (j, jitem) {
                                            attrValue += $(jitem).find("input[type=hidden]").val() + ",";
                                        });
                                        attrValue = attrValue.substr(0, attrValue.length - 1);
                                        break;
                                    case "附件":
                                        attrValue = $(item).find(".attrval").val();
                                        break;
                                    case "枚举":
                                        attrValue = $(item).find("select").val();
                                        break;
                                }
                                //console.log(res + attrid + attrValue);
                                $.ajax({
                                    type: "POST",
                                    url: "/handlers/doc/addAttr.ashx",
                                    data: { docinstanceid: res, attrid: attrid, attrVal: attrValue, userid: uid },
                                    dataType: "",
                                    success: function (res_attr) { }
                                });
                            });

                            //add done
                            if (res != "-1") {
                                InfoTip.showMessage("新增" + docName + "成功", "success", 6000);
                                $("#formContent").slideUp("fast", function () {
                                    $(".result .docLink").attr({ "href": "/Doc/Profile.aspx?ID=" + res }).html(docName);
                                    $(".result .addlink").attr({ "href": "/Doc/Add.aspx?ID=" + id }).text(docName);
                                    $(".result").removeClass("hidden");
                                });
                            } else {
                                InfoTip.showMessage("新增" + docName + "失败", "error", 6000);
                            }
                        }
                    });
                } else {
                    InfoTip.showMessage("必填项'" + validAttr + "'不能为空", "warning", 6000);
                }
            });
            //other methods
        });
    </script>
</asp:Content>
