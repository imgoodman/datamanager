<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="Add.aspx.cs" Inherits="App.Web.DocConfig.Add" %>

<%@ Register Src="../Controls/DocTypeSelect.ascx" TagName="DocTypeSelect" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="header">
        <h1 class="pagetitle">
            新增文档对象</h1>
    </div>
    <p class="message warning hidden">
        <span class="ui-icon ui-icon-info" style="float: left; margin-right: .3em;"></span>
        <strong>Hey!</strong></p>
    <div id="formContent">
        <fieldset class="form">
            <legend>文档对象基本信息</legend>
            <div class="control_group">
                <label class="control_label">
                    文档对象类别</label>
                <div class="controls">
                    <uc1:DocTypeSelect ID="DocTypeSelect1" runat="server" />
                    
                </div>
            </div>
            <div class="control_group">
                <label class="control_label">
                    文档对象名称</label>
                <div class="controls">
                    <input type="text" class="input_medium" id="docName" />
                    <p class="help_block">
                        文档名称比如是“会议纪要”，“审定信函”等。</p>
                </div>
            </div>
            <div class="control_group">
                <label class="control_label">
                    描述</label>
                <div class="controls">
                    <textarea class="input_large multitext" id="desc"></textarea>
                </div>
            </div>
        </fieldset>
        <fieldset class="form">
            <legend>文档对象属性信息</legend>
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
                    <div class="controls" style="padding-top: 3px; margin-right: 20px;">
                        <input type="radio" title="内容简单文本" name="attrType" value="Text" /><span>单字符串</span>
                        <input type="radio" title="内容为复杂文本" name="attrType" value="RichText" /><span>多字符串</span>
                        <input type="radio" title="仅一个人参与" name="attrType" value="Person" /><span>单人</span>
                        <input type="radio" title="多个人参与" name="attrType" value="MultiPerson" /><span>多人参与</span>
                        <input type="radio" title="日期时间" name="attrType" value="Date" /><span>日期</span>
                        <input type="radio" title="文件" name="attrType" value="File" /><span>附件</span>
                        <input type="radio" title="枚举型属性值" name="attrType" value="EnumVal" /><span>枚举</span>
                        <input type="radio" title="关联其它文档对象" name="attrType" value="Obj" class="hidden" /><span
                            class="hidden">对象</span>
                        <div class="attr_content hidden" style="margin-top: 5px;">
                            <input type="text" title="输入枚举类型的值" class="hidden attrval" /><a href="#" class="btn attrval_add hidden"
                                title="新增属性值">新增属性值</a><a href="#" class="btn attrval_obj hidden" title="选择关联的文档对象">选择</a>
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
                        是否关键搜索属性</label>
                    <div class="controls" style="padding-top: 3px;">
                        <input type="radio" title="通常是单文本，日期，单人，多人属性" name="isSearch" value="true" /><span>是</span>
                        <input type="radio" title="通常是多文本和附件属性" name="isSearch" value="false" /><span>否</span>
                    </div>
                </div>
                <div class="control_group">
                    <label class="control_label">
                        是否允许重复</label>
                    <div class="controls" style="padding-top: 3px;">
                        <input type="radio" title="将进行重复性检查" name="isRepeat" value="true" /><span>是</span>
                        <input type="radio" title="" name="isRepeat" value="false" /><span>否</span>
                    </div>
                </div>
                <div class="abs order_action ui-state-default">
                    <span class="ui-icon ui-icon-arrowthick-1-n orderup" title="上移该属性"></span><span class="ui-icon ui-icon-close attr_del"
                        title="删除该属性"></span><span class="ui-icon ui-icon-arrowthick-1-s orderdown" title="下移该属性">
                        </span>
                </div>
            </div>
            <div class="more">
                继续增加属性
            </div>
        </fieldset>
        <div class="form_actions">
            <a href="javascript:void(0);" class="btn btn_primary btn_large save">保存</a>
        </div>
    </div>
    <div class="result hidden">
        <p>
            恭喜您，您已经成功配置了一个文档对象。您可以查看该文档对象的详情<a href="#" class="docLink">XXX</a></p>
        <p>
            您还可以继续配置新的文档对象,<a href="/DocConfig/Add.aspx">新增文档对象</a></p>
    </div>
    <div id="del_confirm" class="hidden">
        <p>
            您确定删除该属性吗？</p>
    </div>
    <div id="attrval_div" class="hidden">
        <div>
            <span class="userspan">枚举值<a href="#" title="删除"></a></span>
        </div>
        <div class="form">
            <div class="control_group">
                <label class="control_label">
                    属性值</label>
                <div class="controls">
                    <input type="text" class="input_medium attrname" />
                    <a href="#" class="btn">添加</a>
                </div>
            </div>
        </div>
    </div>
    <div id="obj_div" class="hidden">
        <div class="form">
            <div class="control_group">
                <label class="control_label">
                    文档类型</label>
                <div class="controls">
                    <ul>
                        <li>ddd</li></ul>
                </div>
            </div>
            <div class="control_group">
                <label class="control_label">
                    文档</label>
                <div class="controls">
                    <select id="docs" class="input_medium">
                        <option>xxxxx</option>
                    </select>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            var attrCount = 1;
            var attrCode = 1;
            var isExist = false;
            var selDocTypeID = "";
            //load doc types
            $("#isVirtual").val("2");
            //枚举属性和对象关联属性
            $("body").on("click", "input[name*='attrType']", function () {
                var attrType = $(this).val();
                var $parent = $(this).parents(".control_group");
                //console.log(attrType);
                if (attrType == "EnumVal") {

                    $parent.find(".userspan").remove();
                    $parent.find(".attr_content").show();
                    $parent.find(".attrval_add").show();
                    $parent.find(".attrval").show();
                    $parent.find(".attrval_obj").hide();
                } else if (attrType == "Obj") {
                    $parent.find(".userspan").remove();
                    $parent.find(".attr_content").show();
                    $parent.find(".attrval_add").hide();
                    $parent.find(".attrval").hide();
                    $parent.find(".attrval_obj").show();
                } else {
                    $parent.find(".attr_content").hide();
                }
            });
            //新增枚举
            $("body").on("click", ".attrval_add", function (e) {
                e.preventDefault();
                var attrval = $(this).prev().val();
                if (attrval == "" || attrval == " ") {
                    InfoTip.showMessage("请输入属性值", "warning", 6000);
                } else {
                    $("<span>").addClass("userspan").html(attrval + "<a  href='#' title='删除该属性值' class='delattrval'></a>").insertBefore($(this).prev());
                    $(this).prev().val('');
                }
            });
            //删除某个属性值
            $("body").on("click", ".delattrval", function (e) {
                e.preventDefault();
                $(this).parent().remove();
            });
            //选择关联对象
            $("body").on("click", ".attrval_obj", function (e) {
                e.preventDefault();
                var $btn = $(this);
                var $parent = $(this).parents("attr_content");
                var $div = $("#obj_div");
                $div.dialog({
                    title: "选择关联文档对象",
                    modal: true,
                    width: 600,
                    minHeight: 100,
                    buttons: {
                        "确认选择": function () {
                            var $selDoc = $("#docs option:selected");
                            $parent.find(".userspan").remove();
                            $("<span>").addClass("userspan").html($selDoc.text() + "<a  href='#' title='删除该关联文档对象' class='delattrval'></a>").insertBefore($btn);
                            $div.dialog("close");
                        },
                        "取消": function () { $div.dialog("close"); }
                    }
                });
            });
            //加载更多属性表单
            $(".more").click(function () {
                attrCount++;
                attrCode++;
                var $temp = $(".attritem:first").clone();
                $temp.find("input[type=text]").val('');
                $temp.find(".attrname")[0].focus();
                $temp.find("input[name=attrType]").removeAttr("checked").attr({ "name": "attrType" + attrCode });
                $temp.find("input[name=isRequired]").removeAttr("checked").attr("name", "isRequired" + attrCode);
                $temp.find("input[name=isSearch]").removeAttr("checked").attr("name", "isSearch" + attrCode);
                $temp.find("input[name=isRepeat]").prop("checked","").attr("name", "isRepeat" + attrCode);
                $temp.find(".attr_content .userspan").remove();
                $temp.find(".attr_content").hide();
                $("html, body").animate({ scrollTop: $(document).height() }, 1000);
                $temp.insertBefore($(".more"));
            });
            //验证该文档名称是否已经存在
            $("#docName").blur(function () {
                if ($(this).val() != "") {
                    $.ajax({
                        type: "POST",
                        url: "/handlers/docConfig/IsDocExist.ashx",
                        data: { docName: $(this).val() },
                        dataType: "json",
                        success: function (res) {
                            if (res == "1") {
                                showMessage("该文档名称已经存在，请修改。");
                                InfoTip.showMessage("该文档名称已经存在，请修改。", "warning", 6000);
                                isExist = true;
                            } else {
                                hideMessage();
                                isExist = false;
                            }
                        }
                    });
                }
            });
            /*************************back up of 属性上移 下移 删除功能******************/
            //属性上移下移，删除
            $("body").on("click", ".orderup", function (e) {
                e.preventDefault();
                var $parent = $(this).parents(".attritem");
                var $pre = $parent.prev();
                if ($pre.hasClass("attritem")) {
                    $parent.clone().insertBefore($pre);
                    $parent.remove();
                    InfoTip.showMessage("调整顺序成功", "success", 6000);
                } else {
                    //当前属性是第一个
                    showMessage("当前属性已经是第一个");
                    InfoTip.showMessage("当前属性已经是第一个", "warning", 6000);
                }
            });
            $("body").on("click", ".orderdown", function (e) {
                e.preventDefault();
                var $parent = $(this).parents(".attritem");
                var $next = $parent.next();
                if ($next.hasClass("attritem")) {
                    $parent.clone().insertAfter($next);
                    $parent.remove();
                    InfoTip.showMessage("调整顺序成功", "success", 6000);
                } else {
                    //已经是最后的了
                    showMessage("当前属性已经是最后一个");
                    InfoTip.showMessage("当前属性已经是最后一个", "warning", 6000);
                }
            });
            //delete attr
            $("body").on("click", ".attr_del", function (e) {
                e.preventDefault();
                var $parent = $(this).parents(".attritem");
                if (attrCount > 1) {
                    var $div = $("#del_confirm");
                    $div.dialog({
                        title: "删除属性",
                        modal: true,
                        width: 600,
                        minHeight: 100,
                        buttons: {
                            "确认": function () {
                                $parent.remove();
                                attrCount--;
                                $div.dialog("close");
                                InfoTip.showMessage("删除属性成功", "success", 6000);
                            },
                            "取消": function () { $div.dialog("close"); }
                        }
                    });
                }
                else {
                    showMessage("文档至少有一个属性");
                    InfoTip.showMessage("文档至少有一个属性", "warning", 6000);
                }
            });
            //保存该文档
            $(".save").click(function (e) {
                e.preventDefault();
                var isValid = true;
                selDocTypeID = $("#selDocType").val();
                if (selDocTypeID == "") {
                    isValid = false;
                    InfoTip.showMessage("请选择文档类别", "warning", 8000);
                    return false;
                }
                var docName = $("#docName").val();
                if (docName == "") {
                    showMessage("文档名称不能为空");
                    InfoTip.showMessage("文档名称不能为空", "warning", 6000);
                    isValid = false;
                    return false;
                }
                $(".attritem").each(function (i, item) {
                    var attrname = $(this).find(".attrname").val();
                    var attrtype = $(this).find("input[name*='attrType']:checked").val();
                    var isrequired = $(this).find("input[name*='isRequired']:checked").val();
                    var issearch = $(this).find("input[name*='isSearch']:checked").val();
                    var isRepeat = $(this).find("input[name*='isRepeat']:checked").val();
                    if (typeof (attrtype) != "undefined") {
                        //if this attr is an enum or obj
                        if (attrtype == "EnumVal" || attrtype == "Obj") {
                            //must have a value
                            //console.log(attrtype + "," + $(this).find(".userspan").length);
                            if ($(this).find(".userspan").length <= 0) {
                                InfoTip.showMessage((attrtype == "EnumVal" ? "枚举型必须有属性值" : "对象类型必须选择关联文档"), "warning", 6000);
                                $(this).find(".red").remove();
                                $("<span>").addClass("red").text((attrtype == "EnumVal" ? "枚举型必须有属性值" : "对象类型必须选择关联文档")).insertAfter($(this).find(".attrname"));
                                isValid = false;
                                return false;
                            }
                        }
                    }

                    //console.log(attrname + attrtype + isrequired + issearch);
                    if (attrname == "" || typeof (attrtype) == "undefined" || typeof (isrequired) == "undefined" || typeof (issearch) == "undefined" || typeof(isRepeat)=="undefined") {
                        showMessage("所有属性名称，属性类型都不能为空");
                        InfoTip.showMessage("所有属性名称，属性类型都不能为空", "warning", 6000);
                        $(this).find(".red").remove();
                        $("<span>").addClass("red").text("属性名称和类型不能为空").insertAfter($(this).find(".attrname"));
                        isValid = false;
                        return false;
                    } else {
                        $(this).find(".red").remove();
                    }
                });
                //console.log(isValid);
                //isValid = false;
                if (isExist) {
                    showMessage("该文档名称已经存在");
                    InfoTip.showMessage("该文档名称已经存在", "warning", 6000);
                    isValid = false;
                    return false;
                }
                if (isValid) {
                    var desc = $("#desc").val();
                    var userid = $("#userid").val();
                    //alert(docName + "," + desc + "," + userid);
                    $.ajax({
                        type: "POST",
                        url: "/handlers/docConfig/AddDoc.ashx",
                        data: { docName: docName, desc: desc, userid: userid, docType: selDocTypeID },
                        dataType: "json",
                        error: function (res) {
                            InfoTip.showMessage("新增文档失败", "error", 6000);
                        },
                        success: function (res) {
                            if (res != "0") {
                                $(".attritem").each(function (i, item) {
                                    var attrname = $(this).find(".attrname").val();
                                    var attrtype = $(this).find("input[name*='attrType']:checked").val();
                                    var isrequired = $(this).find("input[name*='isRequired']:checked").val();
                                    var issearch = $(this).find("input[name*='isSearch']:checked").val();
                                    var isrepeat = $(this).find("input[name*='isRepeat']:checked").val();
                                    var $userspan = $(this).find(".userspan");
                                    //alert(res + "," + attrname + "," + attrtype + "," + userid + "," + isrequired + "," + issearch + "," + i);
                                    $.ajax({
                                        type: "POST",
                                        url: "/handlers/docConfig/AddDocAttr.ashx",
                                        data: {
                                            docID: res,
                                            attrName: attrname,
                                            attrType: attrtype,
                                            desc: "",
                                            userid: userid,
                                            isRequired: isrequired,
                                            isSearch: issearch,
                                            vOrder: i
                                        },
                                        dataType: "json",
                                        success: function (res_attrid) {
                                            if (res_attrid != "0" || res_attrid != "-1") {
                                                $userspan.each(function (j, jitem) {
                                                    //add attr val
                                                    var attrval = $(jitem).text();
                                                    $.ajax({
                                                        type: "POST",
                                                        url: "/handlers/docconfig/addattrval.ashx",
                                                        data: {
                                                            attrid: res_attrid,
                                                            attrval: attrval,
                                                            userid: userid
                                                        },
                                                        dataType: "",
                                                        success: function (res) { }
                                                    });
                                                });
                                            }
                                        }
                                    });
                                });
                                hideMessage();
                                InfoTip.showMessage("新增文档成功", "success", 6000);
                                $("#formContent").slideUp("fast", function () {
                                    $(".result .docLink").attr({ "href": "/DocConfig/Profile.aspx?ID=" + res }).html(docName);
                                    $(".result").removeClass("hidden");
                                });
                            } else {
                                InfoTip.showMessage("新增文档失败", "error", 6000);
                            }
                        }
                    });
                }
            });
        });
    </script>
</asp:Content>
